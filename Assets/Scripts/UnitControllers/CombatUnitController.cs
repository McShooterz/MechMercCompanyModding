using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CombatUnitController : UnitController
{
    [Header("Combat Unit Variables")]

    [SerializeField]
    protected Vector3[] targetingPoints = new Vector3[1];

    [SerializeField]
    protected float lockingOnValue;

    [SerializeField]
    protected float lockedOnTimer;

    [SerializeField]
    protected float maxLockOnRange;

    [SerializeField]
    protected float radarTimer;

    [SerializeField]
    protected float missileDefenseRange;

    [SerializeField]
    protected GroupIntel groupIntel;

    [SerializeField]
    protected LayerMask targetingLayerMask;

    [SerializeField]
    protected LayerMask aiVisionLayerMask;

    [SerializeField]
    protected LayerMask projectileMissileLayerMask;

    [SerializeField]
    protected WeaponController[] weaponControllersAll = new WeaponController[0];

    [SerializeField]
    protected EquipmentController[] equipmentControllers = new EquipmentController[0];

    [SerializeField]
    protected MissileDefenseController[] missileDefenseControllers = new MissileDefenseController[0];

    //[SerializeField]
    //protected MissileDefenseWeaponController[] missileDefenseWeaponControllersAll = new MissileDefenseWeaponController[0];

    [SerializeField]
    protected Vector3 aiTargetingOffset;

    [SerializeField]
    protected bool clearLineOfSightToTarget;

    [SerializeField]
    protected float aiTargetCheckTimer;

    [SerializeField]
    protected float changeTargetTimer;

    [SerializeField]
    protected float missileDefenseTimer;

    [SerializeField]
    protected AI_Type aI_Type;

    [SerializeField]
    protected UnitController targetUnit;

    [SerializeField]
    protected UnitController currentLockedOnTarget;

    [SerializeField]
    protected List<ProjectileController> enemyMissiles = new List<ProjectileController>();

    public UnitController TargetUnit { get => targetUnit; }

    public UnitController CurrentLockedOnTarget { get => currentLockedOnTarget; }

    public GroupIntel GroupIntel { get => groupIntel; }

    public void CalculateMaxLockOnRange()
    {
        maxLockOnRange = 0f;

        for (int i = 0; i < weaponControllersAll.Length; i++)
        {
            WeaponController weaponController = weaponControllersAll[i];

            if (weaponController is ProjectileWeaponController)
            {
                maxLockOnRange = Mathf.Max(maxLockOnRange, (weaponController as ProjectileWeaponController).GetlockOnRange());
            }
        }
    }

    protected override void Awake()
    {
        base.Awake();

        projectileMissileLayerMask |= (1 << LayerMask.NameToLayer("ProjectileMissile"));

        targetingLayerMask |= (1 << LayerMask.NameToLayer("Default")) | (1 << LayerMask.NameToLayer("Terrain")) | (1 << LayerMask.NameToLayer("EnvironmentDestructible"));

        aiVisionLayerMask |= (1 << LayerMask.NameToLayer("Terrain")) | (1 << LayerMask.NameToLayer("Unit"));
    }

    // Start is called before the first frame update
    protected override void Start() { base.Start(); }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        UpdateRadar();

        if (aiControlled)
        {
            if (targetUnit != null && Time.time > aiTargetCheckTimer)
            {
                UpdateTargetOffSetAI();

                clearLineOfSightToTarget = GetLineOfSightToTarget();

                aiTargetCheckTimer = Time.time + 1.0f;
            }
        }
    }

    public Vector3 GetTargetingPoint(int index) { return targetingPoints[index]; }

    public void SetGroupIntel(GroupIntel groupIntel) { this.groupIntel = groupIntel; }

    protected void SetTargetUnit(UnitController target)
    {
        if (target != null)
        {
            targetUnit = target;

            if (aiControlled)
            {
                currentLockedOnTarget = null;
                lockingOnValue = 0.0f;
            }
        }
    }

    protected virtual void UpdateRadar()
    {
        if (Time.time > radarTimer)
        {
            radarTimer = Time.time + 0.5f;

            groupIntel.ClearEnemyRadarTargets();
            List<UnitController> enemyRadarTargets = groupIntel.EnemyRadarTargets;

            List<UnitController> enemyUnitControllers;

            if (Team == TeamType.Enemy)
            {
                enemyUnitControllers = MissionManager.Instance.AllyUnits;
                MechControllerPlayer playerMech = MechControllerPlayer.Instance;

                if (RadarCanDetect(playerMech))
                {
                    enemyRadarTargets.Add(playerMech);
                }
            }
            else
            {
                enemyUnitControllers = MissionManager.Instance.EnemyUnits;
            }

            CheckRadarCanDetectUnits(enemyUnitControllers, enemyRadarTargets);

            if (targetUnit == null)
            {
                SetTargetUnit(GetClosestEnemy());
            }
        }
    }

    protected bool RadarCanDetect(UnitController unitController)
    {
        if (unitController.CanBeDetectedByRadar)
        {
            float distance2D = new Vector2(unitController.transform.position.x - transform.position.x, unitController.transform.position.z - transform.position.z).magnitude;

            return distance2D < GetRadarDetectionRange() - unitController.GetRadarDetectionReduction();
        }

        return false;
    }

    protected void CheckRadarCanDetectUnits(List<UnitController> potentialUnits, List<UnitController> detectedUnits)
    {
        for (int i = 0; i < potentialUnits.Count; i++)
        {
            UnitController potentialUnit = potentialUnits[i];

            if (RadarCanDetect(potentialUnit))
            {
                detectedUnits.Add(potentialUnit);
            }
        }
    }

    protected UnitController GetClosestEnemy()
    {
        List<UnitController> enemyRadarTargets = groupIntel.EnemyRadarTargets;

        if (enemyRadarTargets.Count == 0)
        {
            return null;
        }

        UnitController closestEnemy = null;
        float shortestDistance = Mathf.Infinity;

        for (int i = 0; i < enemyRadarTargets.Count; i++)
        {
            if (enemyRadarTargets[i].IsDestroyed)
                continue;

            float distance = (enemyRadarTargets[i].transform.position - transform.position).sqrMagnitude;

            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                closestEnemy = enemyRadarTargets[i];
            }
        }

        return closestEnemy;
    }

    protected virtual void UpdateTargetOffSetAI()
    {
        aiTargetingOffset = Random.onUnitSphere * 2.0f;
    }

    protected virtual bool GetLineOfSightToTarget()
    {
        if (Physics.Raycast(Bounds.center, targetUnit.Bounds.center - Bounds.center, out RaycastHit hit, GetRadarDetectionRange(), aiVisionLayerMask, QueryTriggerInteraction.Ignore))
        {
            if (hit.transform.root.gameObject == targetUnit.gameObject)
            {
                return true;
            }
        }

        return false;
    }

    protected float GetLockValue(float range, float factor, float bonus)
    {
        float lockOnValue = (ResourceManager.Instance.GameConstants.LockingOnValueMin + (factor - range) / factor) * Time.deltaTime;

        if (bonus != 0.0f)
        {
            lockOnValue += lockOnValue * bonus;
        }

        return lockOnValue;
    }

    protected abstract void UpdateAI();

    public abstract float GetTargetLockOnBonus();

    public override string GetWeaponsDisplay()
    {
        System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();

        for (int i = 0; i < weaponControllersAll.Length; i++)
        {
            WeaponController weaponController = weaponControllersAll[i];

            if (weaponController.IsDestroyed)
            {
                stringBuilder.AppendLine("<color=red>" + weaponController.GetDisplayName() + "</color>");
            }
            else
            {
                stringBuilder.AppendLine(weaponController.GetDisplayName());
            }
        }

        return stringBuilder.ToString().TrimEnd('\r', '\n');
    }

    public void UpdateMissileDefenseRange()
    {
        missileDefenseRange = 0.0f;

        for (int i = 0; i < missileDefenseControllers.Length; i++)
        {
            MissileDefenseController missileDefenseController = missileDefenseControllers[i];

            if (!missileDefenseController.IsDisabled && missileDefenseController.CurrentMissileDefenseType != MissileDefenseType.None)
            {
                missileDefenseRange = Mathf.Max(missileDefenseRange, missileDefenseController.Range);
            }
        }
    }

    protected void UpdateEnemyMissiles()
    {
        if (Time.time > missileDefenseTimer)
        {
            missileDefenseTimer = Time.time + 0.5f;
            enemyMissiles.Clear();

            Collider[] colliders = Physics.OverlapSphere(transform.position, missileDefenseRange, projectileMissileLayerMask);

            for (int i = 0; i < colliders.Length; i++)
            {
                ProjectileController projectileController = colliders[i].gameObject.GetComponent<ProjectileController>();

                if (projectileController.Team != Team)
                {
                    enemyMissiles.Add(projectileController);
                }
            }
        }
    }

    protected void FireAtEnemyMissiles()
    {
        int enemyMissileIndex;
        float enemyMissileRange = -1.0f;

        for (enemyMissileIndex = 0; enemyMissileIndex < enemyMissiles.Count; enemyMissileIndex++)
        {
            if (enemyMissiles[enemyMissileIndex].gameObject.activeInHierarchy)
            {
                enemyMissileRange = (enemyMissiles[enemyMissileIndex].transform.position - transform.position).magnitude;
                break;
            }
        }

        if (enemyMissileRange == -1.0f)
            return;

        for (int i = 0; i < missileDefenseControllers.Length; i++)
        {
            MissileDefenseController missileDefenseController = missileDefenseControllers[i];

            if (missileDefenseController.CanFire && missileDefenseController.CurrentMissileDefenseType == MissileDefenseType.DirectFireAll)
            {
                if (!enemyMissiles[enemyMissileIndex].gameObject.activeInHierarchy)
                {
                    enemyMissileRange = -1.0f;

                    for (; enemyMissileIndex < enemyMissiles.Count; enemyMissileIndex++)
                    {
                        if (enemyMissiles[enemyMissileIndex].gameObject.activeInHierarchy)
                        {
                            enemyMissileRange = (enemyMissiles[enemyMissileIndex].transform.position - transform.position).magnitude;
                            break;
                        }
                    }

                    if (enemyMissileRange == -1.0f)
                        return;
                }

                if (missileDefenseController.InRange(enemyMissileRange))
                {
                    missileDefenseController.Fire(enemyMissiles[enemyMissileIndex]);
                }
            }
        }
    }

    public void SetAI(AI_Type type, TeamType teamType)
    {
        aiControlled = true;
        aI_Type = type;

        SetTeam(teamType);
    }

    protected List<WeaponController> AddWeapons(ComponentData[] componentDatas, Transform[] hardpoints, AmmoPool[] ammoPools)
    {
        List<WeaponController> weaponControllers = new List<WeaponController>();
        int hardpointIndex = 0;

        for (int componentIndex = 0; componentIndex < componentDatas.Length; componentIndex++)
        {
            ComponentData componentData = componentDatas[componentIndex];

            WeaponDefinition weaponDefinition = componentData.ComponentDefinition.GetWeaponDefinition();

            if (weaponDefinition != null && hardpointIndex < hardpoints.Length)
            {
                WeaponController weaponController = AddWeapon(weaponDefinition, hardpoints[hardpointIndex], ammoPools);
                hardpointIndex++;

                if (weaponController != null)
                {
                    weaponControllers.Add(weaponController);
                    componentData.WeaponController = weaponController;
                }
            }
        }

        return weaponControllers;
    }

    protected WeaponController AddWeapon(WeaponDefinition weaponDefinition, Transform hardpoint, AmmoPool[] ammoPools)
    {
        WeaponController weaponController = null;
        GameObject weaponPrefab = weaponDefinition.GetModelPrefab();

        if (weaponPrefab != null)
        {
            GameObject weaponPrefabInstance = Instantiate(weaponPrefab, hardpoint);
            WeaponModelController weaponModelController = weaponPrefabInstance.GetComponent<WeaponModelController>();

            if (weaponModelController != null)
            {
                if (weaponDefinition is BeamWeaponDefinition)
                {
                    BeamWeaponDefinition beamWeaponDefinition = weaponDefinition as BeamWeaponDefinition;
                    BeamWeaponController beamWeaponController;

                    switch (beamWeaponDefinition.BeamFiringModeType)
                    {
                        case BeamFiringModeType.Continuous:
                            {
                                beamWeaponController = weaponModelController.gameObject.AddComponent<BeamWeaponControllerContinuous>();
                                break;
                            }
                        case BeamFiringModeType.Charging:
                            {
                                beamWeaponController = weaponModelController.gameObject.AddComponent<BeamWeaponControllerCharging>();
                                break;
                            }
                        default:
                            {
                                beamWeaponController = weaponModelController.gameObject.AddComponent<BeamWeaponControllerStandard>();
                                break;
                            }
                    }

                    beamWeaponController.SetOwner(this);
                    beamWeaponController.SetWeaponModelController(weaponModelController);
                    beamWeaponController.SetDefinition(beamWeaponDefinition);
                    beamWeaponController.SetDefaultModifiers();

                    return beamWeaponController;
                }
                else
                {
                    ProjectileWeaponDefinition projectileWeaponDefinition = weaponDefinition as ProjectileWeaponDefinition;
                    ProjectileWeaponController projectileWeaponController;

                    switch (projectileWeaponDefinition.ProjectileFiringModeType)
                    {
                        case ProjectileFiringModeType.RapidRefire:
                            {
                                projectileWeaponController = weaponModelController.gameObject.AddComponent<ProjectileWeaponControllerRapidRefire>();
                                break;
                            }
                        case ProjectileFiringModeType.SingleShot:
                            {
                                projectileWeaponController = weaponModelController.gameObject.AddComponent<ProjectileWeaponControllerSingleShot>();
                                break;
                            }
                        case ProjectileFiringModeType.Charging:
                            {
                                projectileWeaponController = weaponModelController.gameObject.AddComponent<ProjectileWeaponControllerCharging>();
                                break;
                            }
                        default:
                            {
                                projectileWeaponController = weaponModelController.gameObject.AddComponent<ProjectileWeaponControllerStandard>();
                                break;
                            }
                    }

                    projectileWeaponController.SetWeaponModelController(weaponModelController);
                    projectileWeaponController.SetDefinition(projectileWeaponDefinition);
                    projectileWeaponController.SetOwner(this);

                    if (projectileWeaponDefinition.RequiresAmmo)
                    {
                        projectileWeaponController.SetValidAmmoPools(ammoPools);
                    }

                    projectileWeaponController.SetDefaultModifiers();
                    projectileWeaponController.AddPrefabsToPooling();

                    return projectileWeaponController;
                }
            }
            else
            {
                Destroy(weaponPrefabInstance);
            }
        }

        return weaponController;
    }
}
