using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public sealed class GroundVehicleController : MobileUnitController
{
    #region Variables
    [Header("Ground Vehicle Controller")]
    GroundVehicleData groundVehicleData;

    [SerializeField]
    GroundVehicleMetaController groundVehicleMetaController;

    //[SerializeField]
    //Rigidbody attachedRigidbody;

    [SerializeField]
    GroundVehicleMovementControllerBase groundVehicleMovementController;

    WeaponController[][] turretWeapons = new WeaponController[0][];

    //Quaternion startingBarrelRotation = Quaternion.identity;

    public int convoyIndex = 0;
    #endregion

    public override bool IsDestroyed { get => groundVehicleData.isDestroyed; }

    public override UnitData UnitData { get => groundVehicleData; }

    public GroundVehicleData GroundVehicleData { get => groundVehicleData; }

    public override Bounds Bounds { get => groundVehicleMetaController.MainCollider.bounds; }

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        if (Time.timeScale == 0.0f || !Application.isFocused)
        {
            return;
        }

        if (IsDestroyed)
        {
            return;
        }

        base.Update();

        if (isInLava)
        {
            groundVehicleData.TakeDamageInternal(10f * Time.deltaTime);
        }

        if (aiControlled)
        {
            UpdateAI();
        }
    }

    public void SetGroundVehicleData(GroundVehicleData data)
    {
        groundVehicleData = data;
        groundVehicleData.groundVehicleController = this;

        groundVehicleMetaController = GetComponent<GroundVehicleMetaController>();

        groundVehicleMetaController.SetHoverEffects(true);

        switch (groundVehicleData.groundVehicleDefinition.GroundVehicleMovement)
        {
            case GroundVehicleMovement.Tracked:
                {
                    groundVehicleMovementController = gameObject.AddComponent<GroundVehicleMovementControllerTracked>();
                    break;
                }
            case GroundVehicleMovement.Wheeled:
                {
                    groundVehicleMovementController = gameObject.AddComponent<GroundVehicleMovementControllerWheeled>();
                    break;
                }
            case GroundVehicleMovement.Hover:
                {
                    groundVehicleMovementController = gameObject.AddComponent<GroundVehicleMovementControllerHovered>();
                    break;
                }
            default:
                {
                    groundVehicleMovementController = gameObject.AddComponent<GroundVehicleMovementControllerTracked>();
                    break;
                }
        }

        groundVehicleMovementController.Initialize(groundVehicleMetaController, data.groundVehicleDefinition);

        List<WeaponController> weaponControllersTurretList = new List<WeaponController>();
        List<WeaponController> weaponControllersAllList = new List<WeaponController>();
        int weaponModelControllerIndex = 0;

        targetingPoints = new Vector3[groundVehicleMetaController.TurretControllers.Length];
        turretWeapons = new WeaponController[groundVehicleMetaController.TurretControllers.Length][];

        for (int turretIndex = 0; turretIndex < turretWeapons.Length; turretIndex++)
        {
            TurretController turretController = groundVehicleMetaController.TurretControllers[turretIndex];
            weaponControllersTurretList.Clear();

            if (turretIndex < groundVehicleData.groundVehicleDefinition.TurretSettings.Length)
            {
                turretController.SetTurretSetting(groundVehicleData.groundVehicleDefinition.TurretSettings[turretIndex]);
            }

            for (int weaponModelIndex = 0; weaponModelIndex < turretController.WeaponModelControllers.Length; weaponModelIndex++)
            {
                WeaponModelController weaponModelController = turretController.WeaponModelControllers[weaponModelIndex];

                if (weaponModelControllerIndex < groundVehicleData.componentDatas.Length)
                {
                    WeaponDefinition weaponDefinition = groundVehicleData.componentDatas[weaponModelControllerIndex].ComponentDefinition.GetWeaponDefinition();

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

                        beamWeaponController.targetingIndex = turretIndex;
                        beamWeaponController.SetOwner(this);
                        beamWeaponController.SetWeaponModelController(weaponModelController);
                        beamWeaponController.SetDefinition(weaponDefinition as BeamWeaponDefinition);
                        beamWeaponController.SetDefaultModifiers();

                        weaponControllersTurretList.Add(beamWeaponController);
                        weaponControllersAllList.Add(beamWeaponController);
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

                        projectileWeaponController.targetingIndex = turretIndex;
                        projectileWeaponController.SetOwner(this);
                        projectileWeaponController.SetWeaponModelController(weaponModelController);
                        projectileWeaponController.SetDefinition(projectileWeaponDefinition);

                        if (projectileWeaponDefinition.RequiresAmmo)
                        {
                            projectileWeaponController.SetValidAmmoPools(groundVehicleData.ammoPools);
                        }

                        projectileWeaponController.SetDefaultModifiers();
                        projectileWeaponController.AddPrefabsToPooling();

                        weaponControllersTurretList.Add(projectileWeaponController);
                        weaponControllersAllList.Add(projectileWeaponController);
                    }
                }

                weaponModelControllerIndex++;
            }

            turretWeapons[turretIndex] = weaponControllersTurretList.ToArray();
        }

        weaponControllersAll = weaponControllersAllList.ToArray();

        CalculateMaxLockOnRange();
    }

    protected override void UpdateAI()
    {
        switch (aI_Type)
        {
            case AI_Type.Convoy:
                {
                    UpdateConvoyAI();
                    break;
                }
            case AI_Type.Skirmish:
                {
                    UpdateSkirmishAI();
                    break;
                }
            case AI_Type.Passive:
                {
                    UpdatePassiveAI();
                    break;
                }
        }
    }

    void UpdateSkirmishAI()
    {
        if (Time.time > changeTargetTimer)
        {
            changeTargetTimer = Time.time + 2.0f;

            targetUnit = GetClosestEnemy();
        }

        if (targetUnit != null)
        {
            if (targetUnit.IsDestroyed)
            {
                targetUnit = null;

                return;
            }

            Vector3 targetDirection = targetUnit.transform.position - transform.position;
            float targetDistance = targetDirection.magnitude;

            if (Time.time > repathTimer)
            {
                if (targetDistance > 20f)
                {
                    repathTimer = Time.time + 1.0f;

                    CreatePath(targetUnit.transform.position);
                }
                else
                {
                    Vector3 randomPosition = targetUnit.transform.position;
                    randomPosition += Random.insideUnitSphere * 10f;

                    if (NavMesh.SamplePosition(randomPosition, out NavMeshHit navMeshHit, 15.0f, NavMesh.AllAreas))
                    {
                        repathTimer = Time.time + Random.Range(1.5f, 4.0f);

                        CreatePath(navMeshHit);
                    }
                    else
                    {
                        repathTimer = Time.time + 0.25f;
                    }
                }
            }

            if (navMeshPath.corners.Length > 1)
            {
                if (groundVehicleMovementController.MoveTowardsTargetPosition(navMeshPath.corners[1]))
                {
                    navMeshPath.ClearCorners();
                }
            }
            else
            {
                groundVehicleMovementController.MoveTowardsTargetPosition(targetUnit.transform.position);
            }

            if (weaponControllersAll.Length > 0)
            {
                FireAtTarget(targetDistance);
            }

            if (currentLockedOnTarget != null && Time.time > lockedOnTimer)
            {
                currentLockedOnTarget = null;
            }
        }
        else
        {
            if (groupIntel.targetLastDetectedPosition != Vector3.zero)
            {
                if (Time.time > repathTimer)
                {
                    repathTimer = Time.time + 1.0f;

                    CreatePath(groupIntel.targetLastDetectedPosition);
                }

                if (navMeshPath.corners.Length > 1)
                {
                    if (groundVehicleMovementController.MoveTowardsTargetPosition(navMeshPath.corners[1]))
                    {
                        navMeshPath.ClearCorners();
                    }
                }
            }
        }
    }

    void UpdateConvoyAI()
    {
        if (convoyIndex == 0)
        {
            ConvoyController convoyController = MissionManager.Instance.ConvoyController;

            if (convoyController.convoyReachedEnd)
            {
                return;
            }

            Transform wayPoint = MissionManager.Instance.ConvoyWayPoints[convoyController.wayPointIndex];

            Vector3 targetDirection = wayPoint.position - transform.position;
            float targetDistance = targetDirection.magnitude;

            if (Time.time > repathTimer)
            {
                if (targetDistance > 5f)
                {
                    repathTimer = Time.time + 1.0f;

                    CreatePath(wayPoint.position);
                }
                else
                {
                    if (convoyController.wayPointIndex == MissionManager.Instance.ConvoyWayPoints.Length - 1)
                    {
                        convoyController.convoyReachedEnd = true;
                        MissionManager.Instance.UpdateObjectives();
                    }
                    else
                    {
                        convoyController.wayPointIndex++;
                    }
                }
            }

            if (navMeshPath.corners.Length > 1)
            {
                if (groundVehicleMovementController.MoveTowardsTargetPosition(navMeshPath.corners[1]))
                {
                    navMeshPath.ClearCorners();
                }
            }
            else
            {
                groundVehicleMovementController.MoveTowardsTargetPosition(wayPoint.position);
            }
        }
        else
        {
            GroundVehicleController followTarget = MissionManager.Instance.ConvoyController.ConvoyUnits[convoyIndex - 1];

            Vector3 targetDirection = followTarget.transform.position - transform.position;
            float targetDistance = targetDirection.magnitude;

            if (targetDistance > 4.5f)
            {
                if (Time.time > repathTimer)
                {
                    repathTimer = Time.time + 1.0f;

                    CreatePath(followTarget.transform.position);
                }

                if (navMeshPath.corners.Length > 1)
                {
                    if (groundVehicleMovementController.MoveTowardsTargetPosition(navMeshPath.corners[1]))
                    {
                        navMeshPath.ClearCorners();
                    }
                }
                else
                {
                    groundVehicleMovementController.MoveTowardsTargetPosition(followTarget.transform.position);
                }
            }
        }
    }

    void UpdatePassiveAI()
    {
        if (orders.Count > 0)
        {
            if (orders.Peek() is OrderMoveToNavPoint)
            {
                OrderMoveToNavPoint orderMoveToNavPoint = orders.Peek() as OrderMoveToNavPoint;

                float distance = (transform.position - orderMoveToNavPoint.navigationPoint.position).magnitude;

                if (distance < 3.0f)
                {
                    orders.Dequeue();
                }
                else
                {
                    groundVehicleMovementController.MoveTowardsTargetPosition(orderMoveToNavPoint.navigationPoint.position);
                }
            }
        }
    }

    void FireAtTarget(float targetDistance)
    {
        if (!clearLineOfSightToTarget)
        {
            lockingOnValue = 0.0f;
            return;
        }

        for (int turretIndex = 0; turretIndex < turretWeapons.Length; turretIndex++)
        {
            TurretController turretController = groundVehicleMetaController.TurretControllers[turretIndex];

            if (turretController.AimAtPosition(targetUnit.TargetablePosition + aiTargetingOffset) < 20.0f)
            {
                targetingPoints[turretIndex] = turretController.TargetingPoint;

                for (int weaponIndex = 0; weaponIndex < turretWeapons[turretIndex].Length; weaponIndex++)
                {
                    WeaponController weaponController = turretWeapons[turretIndex][weaponIndex];

                    if (weaponController.IsDestroyed)
                        continue;

                    if (weaponController.InMaxRange(targetDistance))
                    {
                        weaponController.FireAI();
                    }
                }

                if (turretIndex == 0 && maxLockOnRange > 0.0f && targetDistance < maxLockOnRange)
                {
                    if (currentLockedOnTarget == targetUnit)
                    {
                        lockedOnTimer = Time.time + 2.0f;
                    }
                    else
                    {
                        lockingOnValue += GetLockValue(targetDistance, 200f, GetTargetLockOnBonus()) * 0.25f * groundVehicleData.GunnerySkill;

                        if (lockingOnValue >= 1.0f)
                        {
                            currentLockedOnTarget = targetUnit;
                            lockedOnTimer = Time.time + 2.0f;
                        }
                    }
                }
                else
                {
                    lockingOnValue = 0.0f;
                }
            }
            else if (turretIndex == 0)
            {
                lockingOnValue = 0.0f;
            }
        }
    }

    public override float GetRadarDetectionRange()
    {
        return groundVehicleData.radarDetectionRange;
    }

    public override float GetRadarDetectionReduction()
    {
        return groundVehicleData.radarDetectionReduction;
    }

    public override float GetTargetLockOnBonus()
    {
        return groundVehicleData.LockOnBonus;
    }

    public override void TakeDamage(Collider hitCollider, Vector2 direction, float damage, WeaponController weaponController)
    {
        if (IsDestroyed)
        {
            return;
        }

        if (weaponController.Owner is MechControllerPlayer)
        {
            PlayerHUD.Instance.SetHitIndication();
        }

        if (aiControlled)
        {
            if (targetUnit == null)
            {
                groupIntel.targetLastDetectedPosition = weaponController.Owner.transform.position;
            }
        }

        Vector2 forward = new Vector2(transform.forward.x, transform.forward.z);

        if (Vector2.Angle(-forward, direction) < 45.0f)
        {
            groundVehicleData.TakeDamageFront(damage, weaponController);
        }
        else if (Vector2.Angle(forward, direction) < 45.0f)
        {
            groundVehicleData.TakeDamageRear(damage, weaponController);
        }
        else if (Vector2.Angle(new Vector2(transform.right.x, transform.right.z), direction) < 45.0f)
        {
            groundVehicleData.TakeDamageLeft(damage, weaponController);
        }
        else
        {
            groundVehicleData.TakeDamageRight(damage, weaponController);
        }

        if (IsDestroyed)
        {
            if (weaponController.Owner is MechController)
            {
                (weaponController.Owner as MechController).GetKill(this);
            }
        }
    }

    public override void TakeDirectSplashDamage(Collider hitCollider, Vector2 direction, float damage, WeaponController weaponController)
    {
        if (IsDestroyed)
        {
            return;
        }

        if (weaponController.Owner is MechControllerPlayer)
        {
            PlayerHUD.Instance.SetHitIndication();
        }

        if (aiControlled)
        {
            if (targetUnit == null)
            {
                groupIntel.targetLastDetectedPosition = weaponController.Owner.transform.position;
            }
        }

        Vector2 forward = new Vector2(transform.forward.x, transform.forward.z);
        float damageDirect = damage * 0.5f;
        float damageAdjacent = damage * 0.25f;

        if (Vector2.Angle(-forward, direction) < 45.0f)
        {
            groundVehicleData.TakeDamageFront(damageDirect, weaponController);
            groundVehicleData.TakeDamageLeft(damageAdjacent, weaponController);
            groundVehicleData.TakeDamageRight(damageAdjacent, weaponController);
        }
        else if (Vector2.Angle(forward, direction) < 45.0f)
        {
            groundVehicleData.TakeDamageRear(damageDirect, weaponController);
            groundVehicleData.TakeDamageLeft(damageAdjacent, weaponController);
            groundVehicleData.TakeDamageRight(damageAdjacent, weaponController);
        }
        else if (Vector2.Angle(new Vector2(transform.right.x, transform.right.z), direction) < 45.0f)
        {
            groundVehicleData.TakeDamageLeft(damageDirect, weaponController);
            groundVehicleData.TakeDamageFront(damageAdjacent, weaponController);
            groundVehicleData.TakeDamageRear(damageAdjacent, weaponController);
        }
        else
        {
            groundVehicleData.TakeDamageRight(damageDirect, weaponController);
            groundVehicleData.TakeDamageFront(damageAdjacent, weaponController);
            groundVehicleData.TakeDamageRear(damageAdjacent, weaponController);
        }

        if (IsDestroyed)
        {
            if (weaponController.Owner is MechController)
            {
                (weaponController.Owner as MechController).GetKill(this);
            }
        }
    }

    public override void TakeIndirectSplashDamage(Vector2 direction, float damage, WeaponController weaponController)
    {
        if (IsDestroyed)
        {
            return;
        }

        if (weaponController.Owner is MechControllerPlayer)
        {
            PlayerHUD.Instance.SetHitIndication();
        }

        if (aiControlled)
        {
            if (targetUnit == null)
            {
                groupIntel.targetLastDetectedPosition = weaponController.Owner.transform.position;
            }
        }

        Vector2 forward = new Vector2(transform.forward.x, transform.forward.z);
        float damageSplash = damage * 0.334f;

        if (Vector2.Angle(-forward, direction) < 45.0f)
        {
            groundVehicleData.TakeDamageFront(damageSplash, weaponController);
            groundVehicleData.TakeDamageLeft(damageSplash, weaponController);
            groundVehicleData.TakeDamageRight(damageSplash, weaponController);
        }
        else if (Vector2.Angle(forward, direction) < 45.0f)
        {
            groundVehicleData.TakeDamageRear(damageSplash, weaponController);
            groundVehicleData.TakeDamageLeft(damageSplash, weaponController);
            groundVehicleData.TakeDamageRight(damageSplash, weaponController);
        }
        else if (Vector2.Angle(new Vector2(transform.right.x, transform.right.z), direction) < 45.0f)
        {
            groundVehicleData.TakeDamageLeft(damageSplash, weaponController);
            groundVehicleData.TakeDamageFront(damageSplash, weaponController);
            groundVehicleData.TakeDamageRear(damageSplash, weaponController);
        }
        else
        {
            groundVehicleData.TakeDamageRight(damageSplash, weaponController);
            groundVehicleData.TakeDamageFront(damageSplash, weaponController);
            groundVehicleData.TakeDamageRear(damageSplash, weaponController);
        }

        if (IsDestroyed)
        {
            if (weaponController.Owner is MechController)
            {
                (weaponController.Owner as MechController).GetKill(this);
            }
        }
    }

    protected override void TakeHeatDamage(float damage)
    {

    }

    public void CreateDeathExplosion()
    {
        GameObject explosion = PoolingManager.Instance.Spawn(ResourceManager.Instance.GetEffectPrefab("ExplosionMech"), Bounds.center, transform.rotation);

        EffectsController effectsController = explosion.GetComponent<EffectsController>();

        effectsController.AudioSource.PlayOneShot(ResourceManager.Instance.GetAudioClip("Explosion0"));
    }

    public override void Die()
    {
        groundVehicleData.isDestroyed = true;
        groundVehicleData.internalHealth = 0.0f;

        groundVehicleMovementController.SetDestroyed();

        groundVehicleMetaController.SetHoverEffects(false);

        StartCoroutine(TurnOffRadarDetectable());

        GameObject smokePrefab = ResourceManager.Instance.GetEffectPrefab("SmokeLoop");

        Collider[] colliders = gameObject.GetComponentsInChildren<Collider>();

        for (int i = 0; i < colliders.Length; i++)
        {
            colliders[i].gameObject.layer = LayerMask.NameToLayer("Debris");
        }

        if ((object)smokePrefab != null)
        {
            GameObject smokeEffect = Instantiate(smokePrefab);

            smokeEffect.transform.position = Bounds.center;
            smokeEffect.transform.parent = transform;
        }

        if (aI_Type == AI_Type.Convoy)
        {
            MissionManager.Instance.ConvoyController.RemoveVehicle(this);
        }

        MissionManager.Instance.UpdateObjectives();
    }

    protected override float GetAccelerationForward()
    {
        return 0f;
    }

    protected override float GetAccelerationReverse()
    {
        return 0f;
    }

    protected override float GetDeacceleration()
    {
        return 0f;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == environmentDestructibleLayer)
        {
            EnvironmentDestructible environmentDestructible = collision.collider.GetComponent<EnvironmentDestructible>();

            if (environmentDestructible != null && environmentDestructible.destroyOnCollision)
            {
                environmentDestructible.Die();
            }
        }
    }

    bool CreatePath(NavMeshHit navMeshHit)
    {
        return NavMesh.CalculatePath(transform.position, navMeshHit.position, NavMesh.AllAreas, navMeshPath);
    }

    bool CreatePath(Vector3 position)
    {
        return NavMesh.CalculatePath(transform.position, position, NavMesh.AllAreas, navMeshPath);
    }

    protected override void UpdateTargetOffSetAI()
    {
        aiTargetingOffset = Random.onUnitSphere * (1.0f - groundVehicleData.GunnerySkill);
    }
}
