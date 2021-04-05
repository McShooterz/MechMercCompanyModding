using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
//using SickscoreGames.HUDNavigationSystem;

public abstract class MechController : MobileUnitController
{
    #region Variables
    [Header("Mech Variables")]

    protected MechData mechData;

    protected MechChassisDefinition mechChassisDefinition;

    [SerializeField]
    protected CharacterController characterController;

    [SerializeField]
    protected MechMetaController mechMetaController;

    [SerializeField]
    protected Animator animator;

    protected AudioClip[] footStepsAudioClips = new AudioClip[0];

    [SerializeField]
    protected float currentTorsoTwist;

    [SerializeField]
    protected float currentTorsoPitch;

    [SerializeField]
    protected float torsoMovementVolume;

    [SerializeField]
    protected float currentJumpJetFuel;

    [SerializeField]
    protected float animationMovement = 0;

    [SerializeField]
    protected float animationTurning = 0;

    [SerializeField]
    protected float collisionTimer;

    [SerializeField]
    protected float jumpJetRechargeTimer;

    [SerializeField]
    protected Quaternion startingTorsoLocalRotation;

    [SerializeField]
    public OrderBase currentOrderSquadAI;

    [SerializeField]
    protected float heatCurrent;

    [SerializeField]
    protected UnitPowerState unitPowerState;

    [SerializeField]
    protected float coolant;

    [SerializeField]
    protected bool autoRestart;

    [SerializeField]
    protected UnitController lastHoveredTarget;

    [SerializeField]
    protected GameObject lastHoveredLockonObject;

    [SerializeField]
    protected UnitController lastHoveredLockonTarget;

    [SerializeField]
    protected float shutdownTimer;

    [SerializeField]
    protected float shuttingDownTimer;

    [SerializeField]
    protected float startUpTimer;

    [SerializeField]
    protected float targetThrottle;

    [SerializeField]
    protected float currentThrottle; 

    [SerializeField]
    protected Vector3 characterControllerMovement = Vector3.zero;

    [SerializeField]
    protected Vector3 characterControllerMovementLast = Vector3.zero;

    [SerializeField]
    protected float airTimer;

    protected float recoilVerticalTarget = 0.0f;
    protected float recoilVerticalCurrent = 0.0f;

    protected float recoilHorizontalTarget = 0.0f;
    protected float recoilHorizontalCurrent = 0.0f;

    protected float cockpitVerticalTarget = 0.0f;
    protected float cockpitVerticalCurrent = 0.0f;

    protected float cockpitHorizontalTarget = 0.0f;
    protected float cockpitHorizontalCurrent = 0.0f;

    protected float cockpitRotationTarget = 0.0f;
    protected float cockpitRotationCurrent = 0.0f;

    protected float slopeCheckTimer;

    protected LayerMask terrainLayerMask;

    protected RaycastHit raycastHit;

    protected Vector3 impactForce = Vector3.zero;
    #endregion

    #region Properties
    public override bool IsDestroyed { get => mechData.isDestroyed; }

    public override bool CanBeDetectedByRadar
    {
        get
        {
            if (unitPowerState == UnitPowerState.Shutdown)
                return false;

            return canBeDetectedByRadar;
        }
    }

    public override UnitData UnitData { get => mechData; }

    public MechData MechData { get => mechData; }

    public override string TargetingDisplayName { get => mechData.currentMechPilot.displayName; }

    public MechMetaController MechMetaController { get => mechMetaController; }

    public override Vector3 TargetablePosition { get => mechMetaController.TargetableHardPoint.position; }

    public override Bounds Bounds { get => characterController.bounds; }
    #endregion

    protected override void Awake()
    {
        base.Awake();

        terrainLayerMask |= 1 << LayerMask.NameToLayer("Terrain");
    }

    protected override void Start()
    {
        base.Start();
    }
	
	protected override void Update()
    {
        base.Update();

        UpdateHeat();

        characterController.Move(characterControllerMovement * Time.deltaTime);

        isGroundedLast = isGrounded;
        isGrounded = characterController.isGrounded;
        characterControllerMovementLast = characterControllerMovement;

        if (isGrounded)
        {
            characterControllerMovement.y = -0.981f;
            characterControllerMovement.x = 0.0f;
            characterControllerMovement.z = 0.0f;
        }
        else
        {
            characterControllerMovement.y -= 5.0f * Time.deltaTime;
        }

        if (!isGrounded && isGroundedLast)
        {
            characterControllerMovement += animator.velocity;
            animator.applyRootMotion = false;
            airTimer = Time.time;
        }
        else if (isGrounded && !isGroundedLast)
        {
            CheckFallDamage();
        }

        if (unitPowerState == UnitPowerState.StartingUp)
        {
            CenterTorsoVertical();

            if (Time.time > startUpTimer)
            {
                StartUp();
            }
        }
        else if (unitPowerState == UnitPowerState.Shutdown || unitPowerState == UnitPowerState.ShuttingDown)
        {
            targetThrottle = 0.0f;

            animationTurning = 0.0f;

            if (unitPowerState == UnitPowerState.ShuttingDown)
            {
                currentTorsoPitch += mechData.torsoPitchSpeed * 0.1f * Time.deltaTime;
            }

            if (unitPowerState == UnitPowerState.ShuttingDown && Time.time > shuttingDownTimer && currentThrottle == 0 && isGrounded)
            {
                Shutdown();
            }
        }

        if (unitPowerState == UnitPowerState.Normal)
        {
            if (missileDefenseControllers.Length > 0)
            {
                UpdateEnemyMissiles();

                FireAtEnemyMissiles();
            }
        }

        if (unitPowerState != UnitPowerState.Shutdown)
        {
            UpdateThrottle();
        }

        UpdateTorsoTwistAndPitch();

        UpdateAnimation();


    }

    protected override void UpdateAI()
    {
        
    }



    public virtual void SetMechData(MechData data)
    {
        mechData = data;
        mechData.mechController = this;
        mechChassisDefinition = mechData.MechChassis;
        mechData.ResetMissionData();
        mechData.ReloadAllAmmo();
        mechData.RepairArmor();
        mechData.RepairComponents();

        coolant = mechData.coolantMax;

        footStepsAudioClips = mechChassisDefinition.GetFootStepAudioClips();

        characterController = GetComponent<CharacterController>();
        mechMetaController = GetComponent<MechMetaController>();
        animator = GetComponent<Animator>();

        startingTorsoLocalRotation = mechMetaController.TorsoTransform.localRotation;

        BuildWeapons();

        CalculateMaxLockOnRange();

        if (missileDefenseControllers.Length > 0)
        {
            UpdateMissileDefenseRange();
        }

        currentJumpJetFuel = mechData.jumpCapacity;

        if (coolant > 0)
        {
            mechMetaController.CreateCoolantParticleSystems();
        }

        for (int i = 0; i < mechMetaController.JumpJetThrusters.Length; i++)
        {
            mechMetaController.JumpJetThrusters[i].gameObject.SetActive(true);
        }

        mechMetaController.SetJumpJetThrustersState(false);

        if (mechData.TorsoLeftDestroyed || mechData.ArmLeftDestroyed)
        {
            mechMetaController.DisableArmLeftColliders();

            foreach (GameObject armObject in mechMetaController.ArmLeft)
            {
                armObject.SetActive(false);
            }
        }

        if (mechData.TorsoRightDestroyed || mechData.ArmRightDestroyed)
        {
            mechMetaController.DisableArmRightColliders();

            foreach (GameObject armObject in mechMetaController.ArmRight)
            {
                armObject.SetActive(false);
            }
        }
    }

    protected virtual void BuildWeapons()
    {
        List<WeaponController> weaponControllers = new List<WeaponController>();
        List<EquipmentController> equipmentControllerList = new List<EquipmentController>();
        List<MissileDefenseController> missileDefenseControllerList = new List<MissileDefenseController>();

        BuildSectionWeapons(mechData.componentsHead, mechChassisDefinition.HeadSlotGroups, mechMetaController.HeadHardpoints, mechData.ammoPoolsHead, weaponControllers, MechSectionType.Head);
        BuildSectionWeapons(mechData.componentsTorsoCenter, mechChassisDefinition.TorsoCenterSlotGroups, mechMetaController.TorsoCenterHardpoints, mechData.ammoPoolsTorsoCenter, weaponControllers, MechSectionType.TorsoCenter);
        BuildSectionWeapons(mechData.componentsTorsoLeft, mechChassisDefinition.TorsoLeftSlotGroups, mechMetaController.TorsoLeftHardpoints, mechData.ammoPoolsTorsoLeft, weaponControllers, MechSectionType.TorsoLeft);
        BuildSectionWeapons(mechData.componentsTorsoRight, mechChassisDefinition.TorsoRightSlotGroups, mechMetaController.TorsoRightHardpoints, mechData.ammoPoolsTorsoRight, weaponControllers, MechSectionType.TorsoRight);
        BuildSectionWeapons(mechData.componentsArmLeft, mechChassisDefinition.ArmLeftSlotGroups, mechMetaController.ArmLeftHardpoints, mechData.ammoPoolsArmLeft, weaponControllers, MechSectionType.ArmLeft);
        BuildSectionWeapons(mechData.componentsArmRight, mechChassisDefinition.ArmRightSlotGroups, mechMetaController.ArmRightHardpoints, mechData.ammoPoolsArmRight, weaponControllers, MechSectionType.ArmRight);

        BuildSelectionEquipment(mechData.componentsHead, mechChassisDefinition.HeadSlotGroups, mechMetaController.HeadHardpoints, mechData.ammoPoolsHead, equipmentControllerList, missileDefenseControllerList);
        BuildSelectionEquipment(mechData.componentsTorsoCenter, mechChassisDefinition.TorsoCenterSlotGroups, mechMetaController.TorsoCenterHardpoints, mechData.ammoPoolsTorsoCenter, equipmentControllerList, missileDefenseControllerList);
        BuildSelectionEquipment(mechData.componentsTorsoLeft, mechChassisDefinition.TorsoLeftSlotGroups, mechMetaController.TorsoLeftHardpoints, mechData.ammoPoolsTorsoLeft, equipmentControllerList, missileDefenseControllerList);
        BuildSelectionEquipment(mechData.componentsTorsoRight, mechChassisDefinition.TorsoRightSlotGroups, mechMetaController.TorsoRightHardpoints, mechData.ammoPoolsTorsoRight, equipmentControllerList, missileDefenseControllerList);
        BuildSelectionEquipment(mechData.componentsArmLeft, mechChassisDefinition.ArmLeftSlotGroups, mechMetaController.ArmLeftHardpoints, mechData.ammoPoolsArmLeft, equipmentControllerList, missileDefenseControllerList);
        BuildSelectionEquipment(mechData.componentsArmRight, mechChassisDefinition.ArmRightSlotGroups, mechMetaController.ArmRightHardpoints, mechData.ammoPoolsArmRight, equipmentControllerList, missileDefenseControllerList);

        mechData.RecalculateStats();

        weaponControllersAll = weaponControllers.ToArray();
        equipmentControllers = equipmentControllerList.ToArray();
        missileDefenseControllers = missileDefenseControllerList.ToArray();
    }

    void BuildSectionWeapons(ComponentData[] componentDatas, SlotGroup[] slotGroups, Transform[] hardPoints, AmmoPool[] ammoPools, List<WeaponController> weaponControllers, MechSectionType mechSectionType)
    {
        for (int i = 0; i < componentDatas.Length; i++)
        {
            ComponentData componentData = componentDatas[i];

            // Skip destroyed components
            if (componentData.isDestroyed)
                continue;

            WeaponDefinition weaponDefinition = componentData.ComponentDefinition.GetWeaponDefinition();
            
            if (weaponDefinition != null)
            {
                int hardPoint = slotGroups[componentData.groupIndex].Hardpoint;

                if (hardPoint > -1 && hardPoints.Length > hardPoint)
                {
                    GameObject weaponPrefab = weaponDefinition.GetModelPrefab();

                    if ((object)weaponPrefab != null)
                    {
                        GameObject weaponObjectInstance = Instantiate(weaponPrefab, hardPoints[hardPoint]);
                        weaponObjectInstance.transform.localScale = weaponDefinition.ModelScale;

                        if (weaponDefinition is BeamWeaponDefinition)
                        {
                            BeamWeaponDefinition beamWeaponDefinition = weaponDefinition as BeamWeaponDefinition;
                            BeamWeaponController beamWeaponController;

                            switch (beamWeaponDefinition.BeamFiringModeType)
                            {
                                case BeamFiringModeType.Continuous:
                                    {
                                        beamWeaponController = weaponObjectInstance.AddComponent<BeamWeaponControllerContinuous>();
                                        break;
                                    }
                                case BeamFiringModeType.Charging:
                                    {
                                        beamWeaponController = weaponObjectInstance.AddComponent<BeamWeaponControllerCharging>();
                                        break;
                                    }
                                default:
                                    {
                                        beamWeaponController = weaponObjectInstance.AddComponent<BeamWeaponControllerStandard>();
                                        break;
                                    }
                            }

                            beamWeaponController.SetOwner(this);
                            beamWeaponController.weaponGrouping = componentData.WeaponGrouping;
                            beamWeaponController.SetWeaponModelController(weaponObjectInstance.GetComponent<WeaponModelController>());
                            beamWeaponController.SetDefinition(weaponDefinition as BeamWeaponDefinition);

                            weaponControllers.Add(beamWeaponController);

                            componentData.WeaponController = beamWeaponController;

                            bool modifierFound = false;

                            for (int j = 0; j < componentDatas.Length; j++)
                            {
                                ComponentData data = componentDatas[j];

                                if (componentData != data &&
                                    data.ComponentDefinition.HasWeaponModification &&
                                    componentData.groupIndex == data.groupIndex)
                                {
                                    beamWeaponController.SetWeaponModifier(data);
                                    modifierFound = true;
                                    break;
                                }
                            }

                            if (!modifierFound)
                            {
                                beamWeaponController.SetDefaultModifiers();
                            }
                        }
                        else
                        {
                            ProjectileWeaponDefinition projectileWeaponDefinition = weaponDefinition as ProjectileWeaponDefinition;
                            ProjectileWeaponController projectileWeaponController;

                            switch (projectileWeaponDefinition.ProjectileFiringModeType)
                            {
                                case ProjectileFiringModeType.RapidRefire:
                                    {
                                        projectileWeaponController = weaponObjectInstance.AddComponent<ProjectileWeaponControllerRapidRefire>();
                                        break;
                                    }
                                case ProjectileFiringModeType.SingleShot:
                                    {
                                        projectileWeaponController = weaponObjectInstance.AddComponent<ProjectileWeaponControllerSingleShot>();
                                        break;
                                    }
                                case ProjectileFiringModeType.Charging:
                                    {
                                        projectileWeaponController = weaponObjectInstance.AddComponent<ProjectileWeaponControllerCharging>();
                                        break;
                                    }
                                default:
                                    {
                                        projectileWeaponController = weaponObjectInstance.AddComponent<ProjectileWeaponControllerStandard>();
                                        break;
                                    }
                            }

                            projectileWeaponController.SetWeaponModelController(weaponObjectInstance.GetComponent<WeaponModelController>());
                            projectileWeaponController.SetDefinition(projectileWeaponDefinition);
                            projectileWeaponController.SetOwner(this);
                            projectileWeaponController.weaponGrouping = componentData.WeaponGrouping;                     
                            projectileWeaponController.SetMechSeciontType(mechSectionType);

                            if (projectileWeaponDefinition.RequiresAmmo)
                            {
                                if (componentData.ComponentDefinition.AmmoInternal)
                                {
                                    List<string> ammoTypes = projectileWeaponDefinition.GetAmmoTypes();

                                    if (ammoTypes.Count > 0 && componentData.ComponentDefinition.AmmoType == ammoTypes[0])
                                    {
                                        projectileWeaponController.SetValidAmmoPools(new AmmoPool[] { new AmmoPool(new ComponentData[] { componentData }) });
                                    }
                                }
                                else
                                {
                                    projectileWeaponController.SetValidAmmoPools(ammoPools);
                                }
                            }

                            projectileWeaponController.AddPrefabsToPooling();

                            weaponControllers.Add(projectileWeaponController);

                            componentData.WeaponController = projectileWeaponController;

                            bool modifierFound = false;

                            for (int j = 0; j < componentDatas.Length; j++)
                            {
                                ComponentData data = componentDatas[j];

                                if (componentData != data &&
                                    data.ComponentDefinition.HasWeaponModification &&
                                    componentData.groupIndex == data.groupIndex)
                                {
                                    projectileWeaponController.SetWeaponModifier(data);
                                    modifierFound = true;
                                    break;
                                }
                            }

                            if (!modifierFound)
                            {
                                projectileWeaponController.SetDefaultModifiers();
                            }
                        }
                    }
                }
            }
        }
    }

    void BuildSelectionEquipment(ComponentData[] componentDatas, SlotGroup[] slotGroups, Transform[] hardPoints, AmmoPool[] ammoPools, List<EquipmentController> equipmentControllerList, List<MissileDefenseController> missileDefenseControllerList)
    {
        for (int i = 0; i < componentDatas.Length; i++)
        {
            ComponentData componentData = componentDatas[i];

            EquipmentDefinition equipmentDefinition = componentData.ComponentDefinition.GetEquipmentDefinition();

            if (equipmentDefinition != null)
            {
                int hardPoint = slotGroups[componentData.groupIndex].Hardpoint;
                bool validHardPoint = hardPoint > -1 && hardPoints.Length > hardPoint;

                if (equipmentDefinition is MissileDefenseDefinition)
                {
                    if (validHardPoint)
                    {
                        MissileDefenseDefinition missileDefenseDefinition = equipmentDefinition as MissileDefenseDefinition;

                        GameObject missileDefensePrefab = missileDefenseDefinition.GetModelPrefab();

                        if ((object)missileDefensePrefab != null)
                        {
                            GameObject missileDefenseObjectInstance = Instantiate(missileDefensePrefab, hardPoints[hardPoint]);
                            WeaponModelController weaponModelController = missileDefenseObjectInstance.GetComponent<WeaponModelController>();

                            if (weaponModelController != null)
                            {
                                missileDefenseObjectInstance.transform.localScale = equipmentDefinition.ModelScale;

                                MissileDefenseController missileDefenseController = missileDefenseObjectInstance.AddComponent<MissileDefenseController>();

                                missileDefenseController.Initialize(this, missileDefenseDefinition, weaponModelController);

                                equipmentControllerList.Add(missileDefenseController);
                                missileDefenseControllerList.Add(missileDefenseController);

                                componentData.EquipmentController = missileDefenseController;

                                if (equipmentDefinition.AmmoType != "")
                                {
                                    missileDefenseController.SetAmmoPool(ammoPools);
                                }
                            }
                            else
                            {
                                Destroy(missileDefenseObjectInstance);
                            }
                        }
                    }
                }
                else
                {
                    EquipmentController equipmentController;

                    GameObject prefab = equipmentDefinition.GetModelPrefab();

                    if ((object)prefab != null && validHardPoint)
                    {
                        GameObject equipmentObject = Instantiate(prefab, hardPoints[hardPoint]);
                        equipmentController = equipmentObject.AddComponent<EquipmentController>();
                    }
                    else
                    {
                        GameObject equipmentObject = new GameObject();
                        equipmentObject.transform.parent = transform;
                        equipmentObject.transform.localPosition = Vector3.zero;
                        equipmentController = equipmentObject.AddComponent<EquipmentController>();
                    }

                    equipmentController.Initialize(this, equipmentDefinition);

                    componentData.EquipmentController = equipmentController;

                    equipmentControllerList.Add(equipmentController);

                    if (equipmentDefinition.AmmoType != "")
                    {
                        equipmentController.SetAmmoPool(ammoPools);
                    }
                }
            }
        }
    }

    void UpdateTorsoTwistAndPitch()
    {
        // Allow 360 rotation
        if (currentTorsoTwist > 180)
        {
            float differenceOver = currentTorsoTwist - 180;
            currentTorsoTwist = 180 - differenceOver;
            currentTorsoTwist *= -1f;
        }
        else if (currentTorsoTwist < -180)
        {
            currentTorsoTwist *= -1f;
            float differenceOver = currentTorsoTwist - 180f;
            currentTorsoTwist = 180f - differenceOver;
        }

        currentTorsoTwist = Mathf.Clamp(currentTorsoTwist, -mechChassisDefinition.TorsoTwistMax, mechChassisDefinition.TorsoTwistMax);
        currentTorsoPitch = Mathf.Clamp(currentTorsoPitch, mechChassisDefinition.TorsoPitchMin, mechChassisDefinition.TorsoPitchMax);

        mechMetaController.TorsoTransform.localRotation = startingTorsoLocalRotation;

        mechMetaController.TorsoTransform.RotateAround(mechMetaController.TorsoTransform.position, Vector3.up, currentTorsoTwist);
        mechMetaController.TorsoTransform.RotateAround(mechMetaController.TorsoTransform.position, mechMetaController.TorsoTransform.up, currentTorsoPitch);
    }

    protected virtual void UpdateAnimation()
    {
        if (isGrounded)
        {
            animator.SetBool("Flying", false);
            animator.applyRootMotion = true;

            if (!isGroundedLast) //Time.time - airTimer > 1.0f)
            {
                if (characterControllerMovementLast.magnitude > 8f)
                {
                    animator.SetTrigger("Landing");
                }

                float airTime = Time.time - airTimer;

                if (airTime > 1.0f)
                {
                    AudioManager.Instance.PlayClip(mechMetaController.AudioSourceFeet, ResourceManager.Instance.GetAudioClip("MechLand"), true, false);
                }
            }
        }
        else if (Time.time - airTimer > 0.4f)
        {
            animator.SetBool("Flying", true);
            animator.speed = 1f;
        }

        if (currentThrottle > 0)
        {
            if (mechData.LegLeftDestroyed)
            {
                if (animationMovement < 3f)
                {
                    animationMovement += 3f * Time.deltaTime;

                    if (animationMovement > 3f)
                    {
                        animationMovement = 3f;
                    }
                }
            }
            else if (mechData.LegRightDestroyed)
            {
                if (animationMovement < 2f)
                {
                    animationMovement += 3f * Time.deltaTime;

                    if (animationMovement > 2f)
                    {
                        animationMovement = 2f;
                    }
                }
            }
            else
            {
                if (animationMovement < 1f)
                {
                    animationMovement += 3f * Time.deltaTime;

                    if (animationMovement > 1f)
                    {
                        animationMovement = 1f;
                    }
                }
            }

            animator.speed = currentThrottle * mechData.movementSpeedForward;
            animator.SetFloat("Turning", 0f);
        }
        else if (currentThrottle < 0)
        {
            if (animationMovement > -1f)
            {
                animationMovement -= 3f * Time.deltaTime;

                if (animationMovement < -1f)
                {
                    animationMovement = -1f;
                }
            }

            animator.speed = -currentThrottle * mechData.movementSpeedReverse;
            animator.SetFloat("Turning", 0f);
        }
        else
        {
            if (animationMovement > 0f)
            {
                animationMovement -= 3f * Time.deltaTime;

                if (animationMovement < 0f)
                {
                    animationMovement = 0f;
                }
            }
            else if (animationMovement < 0f)
            {
                animationMovement += 3f * Time.deltaTime;

                if (animationMovement > 0f)
                {
                    animationMovement = 0f;
                }
            }

            if (animationTurning < 0.01f)
                animationTurning = 0.0f;

            animator.SetFloat("Turning", animationTurning);
            animator.speed = 1f;
        }

        animator.SetFloat("Movement", animationMovement);
    }

    public override void TakeDamage(Collider hitCollider, Vector2 direction, float damage, WeaponController weaponController)
    {
        if (mechMetaController.IsTorsoCenterCollider(hitCollider))
        {
            if (DirectionIsForward(direction, new Vector2(hitCollider.transform.forward.x, hitCollider.transform.forward.z)))
            {
                mechData.TakeDamageTorsoCenterFront(damage, weaponController);
            }
            else
            {
                mechData.TakeDamageTorsoCenterRear(damage, weaponController);
            }
        }
        else if (mechMetaController.IsTorsoLeftCollider(hitCollider))
        {
            if (DirectionIsForward(direction, new Vector2(hitCollider.transform.forward.x, hitCollider.transform.forward.z)))
            {
                mechData.TakeDamageTorsoLeftFront(damage, weaponController);
            }
            else
            {
                mechData.TakeDamageTorsoLeftRear(damage, weaponController);
            }
        }
        else if (mechMetaController.IsTorsoRightCollider(hitCollider))
        {
            if (DirectionIsForward(direction, new Vector2(hitCollider.transform.forward.x, hitCollider.transform.forward.z)))
            {
                mechData.TakeDamageTorsoRightFront(damage, weaponController);
            }
            else
            {
                mechData.TakeDamageTorsoRightRear(damage, weaponController);
            }
        }
        else if (mechMetaController.IsArmLeftCollider(hitCollider))
        {
            mechData.TakeDamageArmLeft(damage, weaponController);
        }
        else if (mechMetaController.IsArmRightCollider(hitCollider))
        {
            mechData.TakeDamageArmRight(damage, weaponController);
        }
        else if (mechMetaController.IsLegLeftCollider(hitCollider))
        {
            mechData.TakeDamageLegLeft(damage, weaponController);
        }
        else if (mechMetaController.IsLegRightCollider(hitCollider))
        {
            mechData.TakeDamageLegRight(damage, weaponController);
        }
        else if (mechMetaController.IsHeadCollider(hitCollider))
        {
            mechData.TakeDamageHead(damage, weaponController);
        }
        else
        {
            print("Hit error: " + hitCollider.transform.gameObject.name);
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
        if (mechMetaController.IsTorsoCenterCollider(hitCollider))
        {
            float damageCenter = damage * 0.5f;
            float damageSides = damage * 0.25f;

            if (DirectionIsForward(direction, new Vector2(hitCollider.transform.forward.x, hitCollider.transform.forward.z)))
            {
                mechData.TakeDamageTorsoCenterFront(damageCenter, weaponController);
                mechData.TakeDamageTorsoLeftFront(damageSides, weaponController);
                mechData.TakeDamageTorsoRightFront(damageSides, weaponController);
            }
            else
            {
                mechData.TakeDamageTorsoCenterRear(damageCenter, weaponController);
                mechData.TakeDamageTorsoLeftRear(damageSides, weaponController);
                mechData.TakeDamageTorsoRightRear(damageSides, weaponController);
            }
        }
        else if (mechMetaController.IsTorsoLeftCollider(hitCollider))
        {
            float damageCenter = damage * 0.5f;
            float damageSides = damage * 0.25f;

            if (DirectionIsForward(direction, new Vector2(hitCollider.transform.forward.x, hitCollider.transform.forward.z)))
            {
                mechData.TakeDamageTorsoLeftFront(damageCenter, weaponController);
                mechData.TakeDamageTorsoCenterFront(damageSides, weaponController);
            }
            else
            {
                mechData.TakeDamageTorsoLeftRear(damageCenter, weaponController);
                mechData.TakeDamageTorsoCenterRear(damageSides, weaponController);
            }

            mechData.TakeDamageArmLeft(damageSides, weaponController);
        }
        else if (mechMetaController.IsTorsoRightCollider(hitCollider))
        {
            float damageCenter = damage * 0.5f;
            float damageSides = damage * 0.25f;

            if (DirectionIsForward(direction, new Vector2(hitCollider.transform.forward.x, hitCollider.transform.forward.z)))
            {
                mechData.TakeDamageTorsoRightFront(damageCenter, weaponController);
                mechData.TakeDamageTorsoCenterFront(damageSides, weaponController);
            }
            else
            {
                mechData.TakeDamageTorsoRightRear(damageCenter, weaponController);
                mechData.TakeDamageTorsoCenterRear(damageSides, weaponController);
            }

            mechData.TakeDamageArmRight(damageSides, weaponController);
        }
        else if (mechMetaController.IsArmLeftCollider(hitCollider))
        {
            float damageCenter = damage * 0.6f;
            float damageSide = damage * 0.4f;

            mechData.TakeDamageArmLeft(damageCenter, weaponController);

            if (DirectionIsForward(direction, new Vector2(hitCollider.transform.forward.x, hitCollider.transform.forward.z)))
            {
                mechData.TakeDamageTorsoLeftFront(damageSide, weaponController);
            }
            else
            {
                mechData.TakeDamageTorsoLeftRear(damageSide, weaponController);
            }
        }
        else if (mechMetaController.IsArmRightCollider(hitCollider))
        {
            float damageCenter = damage * 0.6f;
            float damageSide = damage * 0.4f;

            mechData.TakeDamageArmRight(damageCenter, weaponController);

            if (DirectionIsForward(direction, new Vector2(hitCollider.transform.forward.x, hitCollider.transform.forward.z)))
            {
                mechData.TakeDamageTorsoRightFront(damageSide, weaponController);
            }
            else
            {
                mechData.TakeDamageTorsoRightRear(damageSide, weaponController);
            }
        }
        else if (mechMetaController.IsLegLeftCollider(hitCollider))
        {
            float damageCenter = damage * 0.5f;
            float damageSides = damage * 0.25f;

            mechData.TakeDamageLegLeft(damageCenter, weaponController);

            if (DirectionIsForward(direction, new Vector2(hitCollider.transform.forward.x, hitCollider.transform.forward.z)))
            {
                mechData.TakeDamageTorsoCenterFront(damageSides, weaponController);
                mechData.TakeDamageTorsoLeftFront(damageSides, weaponController);
            }
            else
            {
                mechData.TakeDamageTorsoCenterRear(damageSides, weaponController);
                mechData.TakeDamageTorsoLeftRear(damageSides, weaponController);
            }
        }
        else if (mechMetaController.IsLegRightCollider(hitCollider))
        {
            float damageCenter = damage * 0.5f;
            float damageSides = damage * 0.25f;

            mechData.TakeDamageLegRight(damageCenter, weaponController);

            if (DirectionIsForward(direction, new Vector2(hitCollider.transform.forward.x, hitCollider.transform.forward.z)))
            {
                mechData.TakeDamageTorsoCenterFront(damageSides, weaponController);
                mechData.TakeDamageTorsoRightFront(damageSides, weaponController);
            }
            else
            {
                mechData.TakeDamageTorsoCenterRear(damageSides, weaponController);
                mechData.TakeDamageTorsoRightRear(damageSides, weaponController);
            }
        }
        else if (mechMetaController.IsHeadCollider(hitCollider))
        {
            float damageCenter = damage * 0.4f;
            float damageSides = damage * 0.2f;

            mechData.TakeDamageHead(damageCenter, weaponController);

            if (DirectionIsForward(direction, new Vector2(hitCollider.transform.forward.x, hitCollider.transform.forward.z)))
            {
                mechData.TakeDamageTorsoCenterFront(damageSides, weaponController);
                mechData.TakeDamageTorsoLeftFront(damageSides, weaponController);
                mechData.TakeDamageTorsoRightFront(damageSides, weaponController);
            }
            else
            {
                mechData.TakeDamageTorsoCenterRear(damageSides, weaponController);
                mechData.TakeDamageTorsoLeftRear(damageSides, weaponController);
                mechData.TakeDamageTorsoRightRear(damageSides, weaponController);
            }
        }
        else
        {
            print("Hit error: " + hitCollider.transform.gameObject.name);
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
        float damageHead = damage * 0.05f;
        float damageSplash = damage * 0.1357142857142857f;

        if (DirectionIsForward(direction, new Vector2(transform.forward.x, transform.forward.z)))
        {
            mechData.TakeDamageTorsoCenterFront(damageSplash, weaponController);
            mechData.TakeDamageTorsoLeftFront(damageSplash, weaponController);
            mechData.TakeDamageTorsoRightFront(damageSplash, weaponController);
        }
        else
        {
            mechData.TakeDamageTorsoCenterRear(damageSplash, weaponController);
            mechData.TakeDamageTorsoLeftRear(damageSplash, weaponController);
            mechData.TakeDamageTorsoRightRear(damageSplash, weaponController);
        }

        mechData.TakeDamageHead(damageHead, weaponController);
        mechData.TakeDamageLegLeft(damageSplash, weaponController);
        mechData.TakeDamageLegRight(damageSplash, weaponController);
        mechData.TakeDamageArmLeft(damageSplash, weaponController);
        mechData.TakeDamageArmRight(damageSplash, weaponController);

        if (IsDestroyed)
        {
            if (weaponController.Owner is MechController)
            {
                (weaponController.Owner as MechController).GetKill(this);
            }
        }
    }

    public override float GetRadarDetectionReduction()
    {
        return mechData.radarDetectionReduction;
    }

    public override float GetRadarDetectionRange()
    {
        return mechData.radarDetectionRange;
    }

    public override float GetTargetLockOnBonus()
    {
        return mechData.LockOnBonus;
    }

    public void DestroyArmLeft()
    {
        mechMetaController.DisableArmLeftColliders();

        foreach (GameObject armObject in mechMetaController.ArmLeft)
        {
            armObject.SetActive(false);
        }

        mechMetaController.JointDestroyedLeft.SetActive(true);
        mechMetaController.ArmDestroyedLeft.SetActive(true);
        mechMetaController.ArmDestroyedLeft.transform.parent = null;
        mechMetaController.ArmDestroyedLeft.layer = LayerMask.NameToLayer("Debris");

        Rigidbody armRigidbody = mechMetaController.ArmDestroyedLeft.GetComponent<Rigidbody>();

        if (armRigidbody != null)
        {
            armRigidbody.velocity += Random.onUnitSphere;
            armRigidbody.angularVelocity += Random.onUnitSphere;
        }

        GameObject sparksEffectPrefab = ResourceManager.Instance.GetEffectPrefab("SparksLooping");

        if ((object)sparksEffectPrefab != null)
        {
            Instantiate(sparksEffectPrefab, mechMetaController.ShoulderLeftTransform);
        }
    }

    public void DestroyArmRight()
    {
        mechMetaController.DisableArmRightColliders();

        foreach (GameObject armObject in mechMetaController.ArmRight)
        {
            armObject.SetActive(false);
        }

        mechMetaController.JointDestroyedRight.SetActive(true);
        mechMetaController.ArmDestroyedRight.SetActive(true);
        mechMetaController.ArmDestroyedRight.transform.parent = null;
        mechMetaController.ArmDestroyedRight.layer = LayerMask.NameToLayer("Debris");

        Rigidbody armRigidbody = mechMetaController.ArmDestroyedRight.GetComponent<Rigidbody>();

        if (armRigidbody != null)
        {
            armRigidbody.velocity += Random.onUnitSphere;
            armRigidbody.angularVelocity += Random.onUnitSphere;
        }

        GameObject sparksEffectPrefab = ResourceManager.Instance.GetEffectPrefab("SparksLooping");

        if ((object)sparksEffectPrefab != null)
        {
            Instantiate(sparksEffectPrefab, mechMetaController.ShoulderRightTransform);
        }
    }

    public void CreateDeathExplosion()
    {
        CreateBodyExplosion(mechMetaController.TorsoTransform, "ExplosionMech", "ExplosionMechDestroyed", false);
    }

    public void CreateExplosionHead()
    {
        CreateBodyExplosion(mechMetaController.CockpitHardpoint, "ExplosionMechSection", "Explosion0", false);
    }

    public void CreateExplosionTorsoCenter()
    {
        CreateBodyExplosion(mechMetaController.TorsoTransform, "ExplosionMechSection", "Explosion0", false);
    }

    public void CreateExplosionTorsoLeft()
    {
        CreateBodyExplosion(mechMetaController.TorsoLeftExplosionHardpoint, "ExplosionMechSection", "Explosion0", false);
    }

    public void CreateExplosionTorsoRight()
    {
        CreateBodyExplosion(mechMetaController.TorsoRightExplosionHardpoint, "ExplosionMechSection", "Explosion0", false);
    }

    public void CreateExplosionArmLeft()
    {
        CreateBodyExplosion(mechMetaController.ShoulderLeftTransform, "ExplosionMechSection", "Explosion0", false);
    }

    public void CreateExplosionArmRight()
    {
        CreateBodyExplosion(mechMetaController.ShoulderRightTransform, "ExplosionMechSection", "Explosion0", false);
    }

    public void CreateExplosionLegLeft()
    {
        if (targetThrottle < 0)
        {
            targetThrottle = 0;
        }

        CreateBodyExplosion(mechMetaController.LegLeftExplosionHardpoint, "ExplosionMechSection", "Explosion0", false);
    }

    public void CreateExplosionLegRight()
    {
        if (targetThrottle < 0)
        {
            targetThrottle = 0;
        }

        CreateBodyExplosion(mechMetaController.LegRightExplosionHardpoint, "ExplosionMechSection", "Explosion0", false);
    }

    public void CreateAmmoExplosionHead()
    {
        CreateBodyExplosion(mechMetaController.CockpitHardpoint, "ExplosionAmmunition", "ExplosionAmmunition", true);
    }

    public void CreateAmmoExplosionTorsoCenter()
    {
        CreateBodyExplosion(mechMetaController.TorsoTransform, "ExplosionAmmunition", "ExplosionAmmunition", true);
    }

    public void CreateAmmoExplosionTorsoLeft()
    {
        CreateBodyExplosion(mechMetaController.TorsoLeftExplosionHardpoint, "ExplosionAmmunition", "ExplosionAmmunition", true);
    }

    public void CreateAmmoExplosionTorsoRight()
    {
        CreateBodyExplosion(mechMetaController.TorsoRightExplosionHardpoint, "ExplosionAmmunition", "ExplosionAmmunition", true);
    }

    public void CreateAmmoExplosionArmLeft()
    {
        CreateBodyExplosion(mechMetaController.ShoulderLeftTransform, "ExplosionAmmunition", "ExplosionAmmunition", true);
    }

    public void CreateAmmoExplosionArmRight()
    {
        CreateBodyExplosion(mechMetaController.ShoulderRightTransform, "ExplosionAmmunition", "ExplosionAmmunition", true);
    }

    public override void Die()
    {
        mechData.isDestroyed = true;
        animator.enabled = false;
        characterController.enabled = false;

        if (mechData.HeadDestroyed && mechData.currentMechPilot != null)
            mechData.currentMechPilot.PilotStatus = PilotStatusType.Deceased;

        StartCoroutine(TurnOffRadarDetectable());

        mechMetaController.DisableAudioSources();

        Vector3 forceToAddToRigidBodies;

        if (isGrounded)
        {
            forceToAddToRigidBodies = animator.velocity;
        }
        else
        {
            forceToAddToRigidBodies = characterControllerMovement;
        }

        for (int i = 0; i < mechMetaController.Rigidbodies.Length; i++)
        {
            Rigidbody rigidbody = mechMetaController.Rigidbodies[i];

            rigidbody.isKinematic = false;

            rigidbody.velocity += Vector3.down + forceToAddToRigidBodies;
        }

        for (int i = 0; i < weaponControllersAll.Length; i++)
        {
            WeaponController weaponController = weaponControllersAll[i];

            weaponController.SetDestroyed();
            weaponController.Stop();
        }

        mechMetaController.SetCollidersLayer(LayerMask.NameToLayer("Debris"));

        GameObject smokePrefab = ResourceManager.Instance.GetEffectPrefab("SmokeLoop");

        if ((object)smokePrefab != null)
        {
            GameObject smokeEffect = Instantiate(smokePrefab);

            smokeEffect.transform.position = mechMetaController.TorsoTransform.position;
            smokeEffect.transform.parent = mechMetaController.TorsoTransform;
        }

        if (mechMetaController.JumpJetThrusters.Length > 0)
        {
            mechMetaController.AudioSourceJumpJet.enabled = false;

            mechMetaController.SetJumpJetThrustersState(false);
        }

        mechMetaController.SetCoolantEffect(false);

        mechMetaController.SetJumpJetThrustersState(false);
    }

    public virtual void GetKill(UnitController killTarget)
    {
        
    }

    protected virtual void Shutdown()
    {
        unitPowerState = UnitPowerState.Shutdown;
        shutdownTimer = Time.time + ResourceManager.Instance.GameConstants.ShutDownMinTime;
    }

    protected virtual void StartUp()
    {
        unitPowerState = UnitPowerState.Normal;
        autoRestart = false;
    }

    protected float GetCoolingRate()
    {
        float cooling = mechData.cooling;

        cooling *= MissionManager.Instance.CoolingModifier;

        if (unitPowerState == UnitPowerState.Shutdown)
        {
            cooling *= ResourceManager.Instance.GameConstants.ShutDownCoolingFactor;
        }

        return cooling;
    }

    protected override void TakeHeatDamage(float damage)
    {
        mechData.TakeDamageInternalTorsoCenter(damage);
    }

    protected virtual void StartShuttingDown()
    {
        unitPowerState = UnitPowerState.ShuttingDown;
        shuttingDownTimer = Time.time + ResourceManager.Instance.GameConstants.ShuttingDownTime;

        jumpJetRechargeTimer = Time.time + 0.5f;

        mechMetaController.AudioSourceJumpJet.Pause();

        mechMetaController.SetJumpJetThrustersState(false);

        for (int i = 0; i < weaponControllersAll.Length; i++)
        {
            weaponControllersAll[i].Stop();
        }
    }

    protected override float GetAccelerationForward()
    {
        return mechData.accelerationForward;
    }

    protected override float GetAccelerationReverse()
    {
        return mechData.accelerationReverse;
    }

    protected override float GetDeacceleration()
    {
        return mechData.deacceleration;
    }

    protected override void UpdateTargetOffSetAI()
    {
        aiTargetingOffset = Random.onUnitSphere * (1.0f - mechData.currentMechPilot.gunnerySkill);
    }

    public virtual void PlayFootStep()
    {
        if (footStepsAudioClips.Length > 0)
        {
            AudioClip footStepClip = footStepsAudioClips[Random.Range(0, footStepsAudioClips.Length)];

            if (footStepClip != null)
            {
                mechMetaController.AudioSourceFeet.PlayOneShot(footStepClip);
            }
        }
    }

    public virtual void FootStepLeft()
    {
        PlayFootStep();
    }

    public virtual void FootStepRight()
    {
        PlayFootStep();
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (IsDestroyed)
        {
            return;
        }

        if (hit.transform.root.gameObject.layer == unitLayer)
        {
            UnitController unitController = hit.transform.root.GetComponent<UnitController>();

            if (unitController != null)
            {
                float dotProduct = Vector2.Dot(new Vector2(hit.transform.position.x - transform.position.x, hit.transform.position.z - transform.position.z).normalized, new Vector2(transform.forward.x, transform.forward.z));

                if (currentThrottle > 0.0f && dotProduct < 0.0f)
                {
                    return;
                }
                else if (currentThrottle < 0.0f && dotProduct > 0.0f)
                {
                    return;
                }

                if (unitController is GroundVehicleController)
                {
                    GroundVehicleController groundVehicleController = unitController as GroundVehicleController;

                    if (!groundVehicleController.IsDestroyed)
                    {
                        if (groundVehicleController.Team == Team || groundVehicleController.Team == TeamType.Neutral)
                        {
                            return;
                        }

                        if (!mechChassisDefinition.CanStepOn(groundVehicleController.GroundVehicleData.groundVehicleDefinition.UnitClass))
                        {
                            impactForce += (transform.position - unitController.transform.position).normalized * 0.02f;

                            currentThrottle = 0.0f;

                            collisionTimer = Time.time + 0.5f;

                            return;
                        }

                        float legDamage = groundVehicleController.GroundVehicleData.groundVehicleDefinition.MechLegDamage;

                        TakeDamageLegs(legDamage);

                        currentThrottle = Mathf.Min(currentThrottle, 0.5f);

                        groundVehicleController.CreateDeathExplosion();
                        groundVehicleController.Die();
                        GetKill(groundVehicleController);
                    }

                    currentThrottle = Mathf.Clamp(currentThrottle, -0.8f, 0.8f);
                }
                else if (unitController is TurretUnitController)
                {
                    TurretUnitController turretUnitController = unitController as TurretUnitController;

                    if (turretUnitController.Team == Team || turretUnitController.Team == TeamType.Neutral)
                    {
                        return;
                    }

                    turretUnitController.CreateDeathExplosion();
                    turretUnitController.Die();
                    GetKill(turretUnitController);
                }
                else
                {
                    impactForce += (transform.position - unitController.transform.position).normalized * 0.02f;

                    currentThrottle = 0.0f;

                    collisionTimer = Time.time + 0.5f;
                }
            }
            
        }
        else if (hit.gameObject.layer == environmentDestructibleLayer)
        {
            EnvironmentDestructible environmentDestructible = hit.collider.GetComponent<EnvironmentDestructible>();

            if (environmentDestructible != null && environmentDestructible.destroyOnCollision)
            {
                environmentDestructible.Die((environmentDestructible.transform.position - transform.position).normalized);
            }
        }
    }

    protected virtual void TakeDamageLegs(float legDamage)
    {
        if (IsDestroyed)
        {
            return;
        }

        mechData.TakeDamageLegLeft(legDamage);
        mechData.TakeDamageLegRight(legDamage);
    }

    protected virtual void TakeDamageLegsInternal(float legDamage)
    {
        if (IsDestroyed)
        {
            return;
        }

        mechData.TakeDamageInternalLegLeft(legDamage);
        mechData.TakeDamageInternalLegRight(legDamage);
    }

    protected void CenterTorsoHorizontal()
    {
        if (currentTorsoTwist > 0.0f)
        {
            currentTorsoTwist -= mechData.torsoTwistSpeed * Time.deltaTime;

            if (currentTorsoTwist < 0.0f)
            {
                currentTorsoTwist = 0.0f;
            }
        }
        else if (currentTorsoTwist < 0.0f)
        {
            currentTorsoTwist += mechData.torsoTwistSpeed * Time.deltaTime;

            if (currentTorsoTwist > 0.0f)
            {
                currentTorsoTwist = 0.0f;
            }
        }
    }

    protected void CenterTorsoVertical()
    {
        if (currentTorsoPitch > 0.0f)
        {
            currentTorsoPitch -= mechData.torsoPitchSpeed * 0.25f * Time.deltaTime;

            if (currentTorsoPitch < 0.0f)
            {
                currentTorsoPitch = 0.0f;
            }
        }
        else if (currentTorsoPitch < 0.0f)
        {
            currentTorsoPitch += mechData.torsoPitchSpeed * 0.25f * Time.deltaTime;

            if (currentTorsoPitch > 0.0f)
            {
                currentTorsoPitch = 0.0f;
            }
        }
    }

    public override void AddHeat(float value)
    {
        if (unitPowerState != UnitPowerState.Shutdown)
        {
            heatCurrent += value;
        }
    }

    public override void AddHeatDamage(float value)
    {
        if (unitPowerState != UnitPowerState.Shutdown)
        {
            if (heatCurrent < mechData.heatDamageMajor)
            {
                heatCurrent += value;

                if (heatCurrent > mechData.heatDamageMajor)
                {
                    heatCurrent = mechData.heatDamageMajor;
                }
            }
        }
    }

    public virtual void UpdateHeat()
    {
        if (heatCurrent > 0)
        {
            // Apply cooling
            heatCurrent -= GetCoolingRate() * Time.deltaTime;

            if (heatCurrent < 0f)
            {
                heatCurrent = 0f;
            }

            // Take heat related damage
            if (unitPowerState != UnitPowerState.Shutdown)
            {
                if (heatCurrent > mechData.heatLimit)
                {
                    TakeHeatDamage(ResourceManager.Instance.GameConstants.HeatDamageExtreme * Time.deltaTime);
                }
                if (heatCurrent > mechData.heatDamageMajor)
                {
                    TakeHeatDamage(ResourceManager.Instance.GameConstants.HeatDamageMajor * Time.deltaTime);
                }
                else if (heatCurrent > mechData.heatDamageMinor)
                {
                    TakeHeatDamage(ResourceManager.Instance.GameConstants.HeatDamageMinor * Time.deltaTime);
                }
            }

            // Check for shutdown or auto restart
            if (unitPowerState == UnitPowerState.Normal && heatCurrent > mechData.heatShutdown)
            {
                StartShuttingDown();

                autoRestart = true;
            }
            else if (autoRestart && unitPowerState == UnitPowerState.Shutdown && Time.time > shutdownTimer)
            {
                StartStartingUp();
            }
        }
        else
        {
            heatCurrent = 0;

            if (autoRestart && unitPowerState == UnitPowerState.Shutdown && Time.time > shutdownTimer)
            {
                StartStartingUp();
            }
        }
    }

    protected virtual void StartStartingUp()
    {
        unitPowerState = UnitPowerState.StartingUp;
        startUpTimer = Time.time + ResourceManager.Instance.GameConstants.StartingUpTime;
    }

    protected void UpdateThrottle()
    {
        if (currentThrottle < targetThrottle)
        {
            if (currentThrottle > 0f)
            {
                currentThrottle += GetAccelerationForward() * Time.deltaTime;
            }
            else
            {
                currentThrottle += GetDeacceleration() * Time.deltaTime;
            }

            if (currentThrottle > targetThrottle)
            {
                currentThrottle = targetThrottle;
            }
        }
        else if (currentThrottle > targetThrottle)
        {
            if (currentThrottle < 0f)
            {
                currentThrottle -= GetAccelerationReverse() * Time.deltaTime;
            }
            else
            {
                currentThrottle -= GetDeacceleration() * Time.deltaTime;
            }

            if (currentThrottle < targetThrottle)
            {
                currentThrottle = targetThrottle;
            }
        }
    }

    void CreateBodyExplosion(Transform targetTransform, string prefabName, string audioClipName, bool attach)
    {
        GameObject prefab = ResourceManager.Instance.GetEffectPrefab(prefabName);

        if ((object)prefab != null)
        {
            GameObject explosion = PoolingManager.Instance.Spawn(prefab, targetTransform.position, targetTransform.rotation);

            if (attach)
            {
                explosion.transform.parent = targetTransform;
            }

            EffectsController effectsController = explosion.GetComponent<EffectsController>();

            if (effectsController != null)
            {
                AudioClip audioClip = ResourceManager.Instance.GetAudioClip(audioClipName);

                if (audioClip != null)
                {
                    effectsController.AudioSource.PlayOneShot(audioClip);
                }
            }
        }
    }

	

    public void SetSquadMateOrder(OrderBase orderBase)
    {
        currentOrderSquadAI = orderBase;

        if (currentOrderSquadAI != null)
        {
            if (currentOrderSquadAI is OrderHoldPosition)
            {
                targetThrottle = 0f;
            }
        }

        MissionManager.Instance.UpdateSquadMateDisplay();
    }

    public void AddRecoilHorizontal(float value)
    {
        recoilHorizontalTarget += value * mechData.RecoilModifier;
    }

    public void AddRecoilVertical(float value)
    {
        recoilVerticalTarget += value * mechData.RecoilModifier;
    }

    public void AddCockpitShakeVertical(float value)
    {
        cockpitVerticalTarget += value;
    }

    public void AddCockpitShakeHorizontal(float value)
    {
        cockpitHorizontalTarget += value;
    }

    public void AddCockpitShakeRotation(float value)
    {
        cockpitRotationTarget += value;
    }

    protected virtual void CheckFallDamage()
    {
        if (characterControllerMovementLast.y < -8f)
        {
            float fallDamage = -(characterControllerMovementLast.y + 8);

            mechData.TakeDamageInternalLegLeft(fallDamage);
            mechData.TakeDamageInternalLegRight(fallDamage);
        }
    }

    protected void CheckGround()
    {
        if (currentThrottle == 0.0f || Time.time < slopeCheckTimer)
        {
            return;        
        }

        slopeCheckTimer = Time.time + 0.2f;

        Vector3 rayOrigin = transform.position + new Vector3(0.0f, 0.1f, 0.0f);

        if (currentThrottle > 0.0f)
        {
            rayOrigin += transform.forward * 0.15f;
        }
        else if (currentThrottle < 0.0f)
        {
            rayOrigin -= transform.forward * 0.15f;
        }

        if (Physics.Raycast(rayOrigin, Vector3.down, out raycastHit, 0.2f, terrainLayerMask, QueryTriggerInteraction.Ignore))
        {
            Vector3 slopeForward = Vector3.Cross(transform.right, raycastHit.normal);

            float angle = Vector3.SignedAngle(slopeForward, transform.forward, transform.right);

            if (currentThrottle > 0.0f)
            {
                if (angle > mechData.MechChassis.SlopeLimit)
                {
                    currentThrottle = 0.0f;
                    collisionTimer = Time.time + 0.2f;
                }
                else if (angle > mechData.MechChassis.SlopeThreshold)
                {
                    float ratio = (angle - mechData.MechChassis.SlopeThreshold) / (mechData.MechChassis.SlopeLimit - mechData.MechChassis.SlopeThreshold);
                    float limit = 1.0f - ratio;

                    currentThrottle = Mathf.Min(currentThrottle, limit);

                    if (limit < 0.05f)
                    {
                        collisionTimer = Time.time + 0.2f;
                    }
                }
            }
            else
            {
                if (angle < -mechData.MechChassis.SlopeLimit)
                {
                    currentThrottle = 0.0f;
                    collisionTimer = Time.time + 0.2f;
                }
                else if (angle < -mechData.MechChassis.SlopeThreshold)
                {
                    float ratio = (angle + mechData.MechChassis.SlopeThreshold) / (mechData.MechChassis.SlopeLimit - mechData.MechChassis.SlopeThreshold);
                    float limit = -1.0f + ratio;

                    //float limit = -1 + -((Mathf.Max(angle, -30.0f) + 10.0f) / 20.0f);

                    currentThrottle = Mathf.Max(currentThrottle, limit);

                    if (limit > -0.05f)
                    {
                        collisionTimer = Time.time + 0.2f;
                    }
                }
            }
        }
    }

    protected void UpdateImpactForce()
    {
        if (impactForce.magnitude > 0.004f)
        {
            characterController.Move(impactForce);

            impactForce = Vector3.Lerp(impactForce, Vector3.zero, 5 * Time.deltaTime);
        }
        else
        {
            impactForce = Vector3.zero;
        }
    }

    
}
