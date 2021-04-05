using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ProjectileWeaponController : WeaponController
{
    #region Variables
    [SerializeField]
    protected float recycleTimer;
    
    [SerializeField]
    protected bool ammoEmpty = false;

    [SerializeField]
    protected int salvoCount = 0;

    [SerializeField]
    protected float salvoTimer = 0f;

    [SerializeField]
    AudioClip firingAudioClip;

    [SerializeField]
    protected float firingAudioTimer;

    [SerializeField]
    int ammoPoolIndex = 0;

    [SerializeField]
    int projectileLayer;

    [SerializeField]
    protected float jammingPerFiring = 0.0f;

    protected UnitController homingTarget;

    protected Vector3 homingTargetPosition = Vector3.zero;

    GameObject projectilePrefab = null;

    GameObject trailPrefab = null;

    float firingPitchMax = 1.05f;
    float firingPitchMin = 0.95f;

    ProjectileDefinition[] projectileDefinitions;
    #endregion

    public ProjectileWeaponDefinition ProjectileWeaponDefinition { get; set; }

    public ProjectileDefinition ProjectileDefinition { get; private set; }

    protected ComponentData modComponentData { get; set; }

    protected float RecycleTimeModified { get; set; }

    public override bool CanFire { get => Time.time > recycleTimer && !ammoEmpty; }

    public MechSectionType MechSectionType { get; private set; }

    public AmmoPool[] AmmoPools { get; private set; }

    public AmmoPool CurrentAmmoPool
    {
        get
        {
            if (AmmoPools.Length > 0)
            {
                return AmmoPools[ammoPoolIndex];
            }

            return null;
        }
    }

    public bool AmmoEmpty { get => ammoEmpty; }

    float RandomFiringPitch { get => Random.Range(firingPitchMin, firingPitchMax); }

    public string AmmoNameDisplay
    {
        get
        {
            if (CurrentAmmoPool != null)
            {
                return CurrentAmmoPool.AmmoDefinition.GetDisplayName();
            }

            return "";
        }
    }

    public string AmmoCountDisplay
    {
        get
        {
            if (CurrentAmmoPool != null)
            {
                return CurrentAmmoPool.AmmoCount.ToString();
            }

            return "";
        }
    }

    public override string GetDisplayName()
    {
        return ProjectileWeaponDefinition.GetDisplayName();
    }

    public virtual void SetDefinition(ProjectileWeaponDefinition definition)
    {
        ProjectileWeaponDefinition = definition;

        projectileDefinitions = ProjectileWeaponDefinition.GetProjectileDefinitions().ToArray();

        if (projectileDefinitions.Length == 0)
        {
            isDestroyed = true;
            recycleTimer = Mathf.Infinity;
        }
        else if (!ProjectileWeaponDefinition.RequiresAmmo)
        {
            SetProjectileDefinition(projectileDefinitions[0]);
        }
    }

    public override void SetDestroyed()
    {
        base.SetDestroyed();

        recycleTimer = Mathf.Infinity;

        if (ProjectileWeaponDefinition.FiringSoundLoops && weaponModelController.AudioSource.isPlaying)
        {
            weaponModelController.AudioSource.Stop();
        }

        owner.CalculateMaxLockOnRange();
    }

    public void SetValidAmmoPools(AmmoPool[] potentialAmmoPools)
    {
        List<AmmoPool> validAmmoPools = new List<AmmoPool>();

        for (int i = 0; i < potentialAmmoPools.Length; i++)
        {
            AmmoPool ammoPool = potentialAmmoPools[i];

            for (int projectileIndex = 0; projectileIndex < projectileDefinitions.Length; projectileIndex++)
            {
                ProjectileDefinition projectileDefinition = projectileDefinitions[projectileIndex];

                if (ammoPool.AmmoType == projectileDefinition.AmmoType)
                {
                    validAmmoPools.Add(ammoPool);
                }
            }
        }

        AmmoPools = validAmmoPools.ToArray();

        if (AmmoPools.Length > 0 && CheckAmmoPoolsHaveAmmo())
        {
            ProjectileDefinition projectileDefinition;

            for (int projectileIndex = 0; projectileIndex < projectileDefinitions.Length; projectileIndex++)
            {
                projectileDefinition = projectileDefinitions[projectileIndex];

                for (int ammoIndex = 0; ammoIndex < AmmoPools.Length; ammoIndex++)
                {
                    if (projectileDefinition.AmmoType == AmmoPools[ammoIndex].AmmoType)
                    {
                        SetProjectileDefinition(projectileDefinition);
                        ammoPoolIndex = ammoIndex;
                        return;
                    }
                }
            }
        }
        else
        {
            ammoEmpty = true;
        }
    }

    public void SetMechSeciontType(MechSectionType mechSectionType)
    {
        MechSectionType = mechSectionType;
    }

    public void AddPrefabsToPooling()
    {
        if (ammoEmpty)
            return;

        List<ProjectileDefinition> projectileDefinitions = ProjectileWeaponDefinition.GetProjectileDefinitions();

        for (int index = 0; index < projectileDefinitions.Count; index++)
        {
            ProjectileDefinition definition = projectileDefinitions[index];

            if (ProjectileWeaponDefinition.RequiresAmmo)
            {
                bool hasAmmo = false;

                for(int i = 0; i < AmmoPools.Length; i++)
                {
                    if (AmmoPools[i].AmmoType == definition.AmmoType)
                    {
                        hasAmmo = true;
                        break;
                    }
                }

                if (!hasAmmo)
                    continue;
            }

            GameObject projectilePrefab = definition.GetProjectilePrefab();

            if ((object)projectilePrefab != null)
            {
                int count = definition.ProjectileCount * ProjectileWeaponDefinition.ShotsPerSalvo;

                count *= Mathf.CeilToInt(definition.ProjectileLifeTime / ProjectileWeaponDefinition.RecycleTime);

                PoolingManager.Instance.CreateMembers(projectilePrefab, count);

                GameObject trailPrefab = definition.GetTrailPrefab();

                if ((object)trailPrefab != null)
                {
                    PoolingManager.Instance.CreateMembers(trailPrefab, count);
                }

                GameObject impactPrefab = definition.GetImpactEffectPrefab();

                if ((object)impactPrefab != null)
                {
                    PoolingManager.Instance.CreateMembers(impactPrefab, count);
                }

                ProjectileDefinition secondaryProjectileDefinition = definition.GetSecondaryProjectile();

                if (secondaryProjectileDefinition != null)
                {
                    projectilePrefab = secondaryProjectileDefinition.GetProjectilePrefab();

                    if ((object)projectilePrefab != null)
                    {
                        PoolingManager.Instance.CreateMembers(projectilePrefab, count);

                        trailPrefab = secondaryProjectileDefinition.GetTrailPrefab();

                        if ((object)trailPrefab != null)
                        {
                            PoolingManager.Instance.CreateMembers(trailPrefab, count);
                        }

                        impactPrefab = secondaryProjectileDefinition.GetImpactEffectPrefab();

                        if ((object)impactPrefab != null)
                        {
                            PoolingManager.Instance.CreateMembers(impactPrefab, count);
                        }
                    }
                }
            }
        }
    }

    protected void FireShots(Vector3 targetPosition, int shotsCount, int projectilesCount)
    {
        GameObject muzzleEffect = ProjectileWeaponDefinition.GetMuzzleEffectPrefab();

        for (int k = 0; k < shotsCount; k++)
        {
            Transform firingTransform = weaponModelController.NextFiringPoint;
            Vector3 firingPoint;

            if ((object)muzzleEffect != null)
            {
                GameObject muzzleEffectInstance = PoolingManager.Instance.Spawn(muzzleEffect, firingTransform.position, firingTransform.rotation);
                muzzleEffectInstance.transform.parent = firingTransform;
            }

            firingPoint = firingTransform.position + transform.forward * ProjectileWeaponDefinition.ProjectSpawnForwardMulti;

            for (int i = 0; i < projectilesCount; i++)
            {
                CreateProjectile(targetPosition, firingPoint);
            }

            owner.AddHeat(HeatModified);

            if (ProjectileDefinition.Recoil != 0.0f && owner is MechController)
            {
                if (owner is MechControllerPlayer)
                {
                    ApplyRecoilPlayer(ProjectileDefinition.Recoil);
                }
                else
                {
                    ApplyRecoil(ProjectileDefinition.Recoil);
                }
            }
        }

        jamming += ProjectileDefinition.Jamming;
        jammingDecayTimer = Time.time + ProjectileWeaponDefinition.JamDecayDelay;

        if (jamming >= 1.0f)
        {
            jammed = true;

            salvoCount = 0;
        }
    }

    void CreateProjectile(Vector3 targetPosition, Vector3 firingPoint)
    {
        if ((object)projectilePrefab != null)
        {
            GameObject projectileObject = PoolingManager.Instance.Spawn(projectilePrefab, firingPoint, Quaternion.LookRotation(targetPosition - firingPoint));
            ProjectileController projectileController = projectileObject.GetComponent<ProjectileController>();

            if (projectileController != null)
            {
                projectileController.Initialize(this, ProjectileDefinition);
                projectileObject.layer = projectileLayer;

                if (ProjectileDefinition.HomingType == ProjectileHomingType.LockOn)
                {
                    if (homingTarget != null)
                    {
                        projectileController.SetHomingTarget(homingTarget);

                        if (ProjectileDefinition.HomingFlightHeightOffSet > 0.0f)
                        {
                            projectileController.SetFlightHeight(Mathf.Max(transform.position.y, homingTarget.Bounds.center.y) + ProjectileDefinition.HomingFlightHeightOffSet);
                        }
                    }
                    else
                    {
                        projectileController.SetHomingTarget(homingTargetPosition);

                        if (ProjectileDefinition.HomingFlightHeightOffSet > 0.0f)
                        {
                            projectileController.SetFlightHeight(Mathf.Max(transform.position.y, owner.GetTargetingPoint(targetingIndex).y) + ProjectileDefinition.HomingFlightHeightOffSet);
                        }
                    }
                }
                else if (ProjectileDefinition.HomingType == ProjectileHomingType.Auto)
                {
                    if (ProjectileDefinition.HomingFlightHeightOffSet > 0.0f)
                    {
                        UnitController homingTarget = owner.TargetUnit;

                        if (homingTarget != null)
                        {
                            projectileController.SetFlightHeight(Mathf.Max(transform.position.y, homingTarget.Bounds.center.y) + ProjectileDefinition.HomingFlightHeightOffSet);
                        }
                        else
                        {
                            projectileController.SetFlightHeight(transform.position.y + ProjectileDefinition.HomingFlightHeightOffSet);
                        }
                    }
                }

                if ((object)trailPrefab != null)
                {
                    GameObject trailObject = PoolingManager.Instance.Spawn(trailPrefab, projectileObject.transform.position, projectileObject.transform.rotation);
                    trailObject.transform.localScale = ProjectileDefinition.TrailScaleVector3;
                    TrailController trailController = trailObject.GetComponent<TrailController>();

                    if (trailController != null)
                    {
                        projectileController.SetTrailController(trailController);
                        trailController.SetScale(ProjectileDefinition.TrailScale);
                    }
                }

                if (ProjectileDefinition.MissileDefenseTargetable)
                {
                    projectileController.SetHealth(ProjectileDefinition.MissileHealth);
                }
            }
            else
            {
                print("Warning! projectile controller missing");
                Destroy(projectileObject);
            }
        }
        else
        {
            print("Warning! prefab not found: " + ProjectileDefinition.ProjectilePrefab);
        }
    }

    public override void Stop()
    {
        base.Stop();

        salvoCount = 0;
    }

    public override float GetRefireBar()
    {
        if (ammoEmpty)
        {
            return 1f;
        }

        if (recycleTimer > Time.time)
        {
            float difference = recycleTimer - Time.time;
            return difference / RecycleTimeModified;
        }

        return 0f;
    }

    protected void PlayFiringSound()
    {
        if (firingAudioClip != null)
        {
            if (ProjectileWeaponDefinition.FiringSoundLoops)
            {
                firingAudioTimer = Time.time + RecycleTimeModified;
                
                if (!weaponModelController.AudioSource.isPlaying)
                {
                    weaponModelController.AudioSource.Stop();
                    weaponModelController.AudioSource.Play();
                }
            }
            else
            {
                weaponModelController.AudioSource.pitch = RandomFiringPitch;
                weaponModelController.AudioSource.PlayOneShot(firingAudioClip);
            }
        }
    }

    public float GetlockOnRange()
    {
        if (isDestroyed || ammoEmpty)
            return 0.0f;

        if (ProjectileDefinition.HomingType == ProjectileHomingType.LockOn)
        {
            return ProjectileDefinition.RangeDamageMin;
        }

        return 0.0f;
    }

    void ApplyRecoil(float recoil)
    {
        MechController mechControllerOwner = owner as MechController;

        switch (MechSectionType)
        {
            case MechSectionType.Head:
                {
                    mechControllerOwner.AddRecoilVertical(recoil);
                    break;
                }
            case MechSectionType.TorsoCenter:
                {
                    recoil *= 0.5f;
                    mechControllerOwner.AddRecoilVertical(recoil);
                    break;
                }
            case MechSectionType.TorsoLeft:
                {
                    recoil *= 0.5f;
                    mechControllerOwner.AddRecoilHorizontal(-recoil);
                    mechControllerOwner.AddRecoilVertical(recoil);
                    break;
                }
            case MechSectionType.TorsoRight:
                {
                    recoil *= 0.5f;
                    mechControllerOwner.AddRecoilHorizontal(recoil);
                    mechControllerOwner.AddRecoilVertical(recoil);
                    break;
                }
            case MechSectionType.ArmLeft:
                {
                    mechControllerOwner.AddRecoilHorizontal(-recoil);
                    recoil *= 0.25f;
                    mechControllerOwner.AddRecoilVertical(recoil);
                    break;
                }
            case MechSectionType.ArmRight:
                {
                    mechControllerOwner.AddRecoilHorizontal(recoil);
                    recoil *= 0.25f;
                    mechControllerOwner.AddRecoilVertical(recoil);
                    break;
                }
        }
    }

    void ApplyRecoilPlayer(float recoil)
    {
        MechController mechControllerOwner = owner as MechController;

        switch (MechSectionType)
        {
            case MechSectionType.Head:
                {
                    mechControllerOwner.AddRecoilVertical(recoil);
                    CameraController.Instance.AddShakeRotation(Vector3.left * recoil);
                    break;
                }
            case MechSectionType.TorsoCenter:
                {
                    recoil *= 0.5f;
                    mechControllerOwner.AddRecoilVertical(recoil);
                    CameraController.Instance.AddShakeRotation(Vector3.left * recoil);
                    break;
                }
            case MechSectionType.TorsoLeft:
                {
                    recoil *= 0.5f;
                    mechControllerOwner.AddRecoilHorizontal(-recoil);
                    mechControllerOwner.AddRecoilVertical(recoil);
                    CameraController.Instance.AddShakeRotation(new Vector3(-1, -1, 0) * recoil);
                    break;
                }
            case MechSectionType.TorsoRight:
                {
                    recoil *= 0.5f;
                    mechControllerOwner.AddRecoilHorizontal(recoil);
                    mechControllerOwner.AddRecoilVertical(recoil);
                    CameraController.Instance.AddShakeRotation(new Vector3(-1, 1, 0) * recoil);
                    break;
                }
            case MechSectionType.ArmLeft:
                {
                    mechControllerOwner.AddRecoilHorizontal(-recoil);
                    recoil *= 0.25f;
                    mechControllerOwner.AddRecoilVertical(recoil);
                    CameraController.Instance.AddShakeRotation(new Vector3(-1, -1, 0) * recoil);
                    break;
                }
            case MechSectionType.ArmRight:
                {
                    mechControllerOwner.AddRecoilHorizontal(recoil);
                    recoil *= 0.25f;
                    mechControllerOwner.AddRecoilVertical(recoil);
                    CameraController.Instance.AddShakeRotation(new Vector3(-1, 1, 0) * recoil);
                    break;
                }
        }
    }

    bool CheckAmmoPoolsHaveAmmo()
    {
        for (int i = 0; i < AmmoPools.Length; i++)
        {
            if (AmmoPools[i].HasAmmo)
                return true;
        }

        ammoEmpty = true;
        jamming = 0.0f;
        owner.CalculateMaxLockOnRange();
        return false;
    }

    public float GetWeaponHeat()
    {
        if (ProjectileDefinition != null)
        {
            return ProjectileWeaponDefinition.GetShotCount() * HeatModified;
        }

        return 0.0f;
    }

    public override float GetRangeEffective()
    {
        if (ProjectileDefinition != null && !ammoEmpty)
        {
            return ProjectileDefinition.RangeEffective;
        }

        return 0.0f;
    }

    public override bool InEffectiveRange(float distance)
    {
        if (ProjectileDefinition != null)
        {
            return ProjectileDefinition.InEffectiveRange(distance);
        }

        return false;
    }

    public override bool InMaxRange(float distanceSqr)
    {
        if (ProjectileDefinition != null)
        {
            return ProjectileDefinition.InMaxRange(distanceSqr);
        }

        return false;
    }

    public override DamageType GetDamageType()
    {
        if (ProjectileDefinition != null)
        {
            return ProjectileDefinition.DamageType;
        }

        return DamageType.Generic;
    }

    public override bool GetCriticalRole()
    {
        return ProjectileDefinition.GetCriticalRole();
    }

    public override float GetCriticalDamageMulti()
    {
        return ProjectileDefinition.CriticalDamageMulti;
    }

    public void CycleNextAmmoPool()
    {
        if (AmmoPools.Length > 1)
        {
            ammoPoolIndex++;

            if (ammoPoolIndex == AmmoPools.Length)
            {
                ammoPoolIndex = 0;
            }

            for (int i = 0; i < projectileDefinitions.Length; i++)
            {
                ProjectileDefinition projectileDefinition = projectileDefinitions[i];

                if (CurrentAmmoPool.AmmoType == projectileDefinition.AmmoType)
                {
                    SetProjectileDefinition(projectileDefinition);
                    recycleTimer = Time.time + RecycleTimeModified;
                    owner.CalculateMaxLockOnRange();
                }
            }
        }
    }

    protected void AutoSwitchAmmo()
    {
        if (CheckAmmoPoolsHaveAmmo())
        {
            for (int projectileIndex = 0; projectileIndex < projectileDefinitions.Length; projectileIndex++)
            {
                ProjectileDefinition projectileDefinition = projectileDefinitions[projectileIndex];

                for (int i = 0; i < AmmoPools.Length; i++)
                {
                    if (AmmoPools[i].HasAmmo && projectileDefinition.AmmoType == AmmoPools[i].AmmoType)
                    {
                        ammoPoolIndex = i;
                        SetProjectileDefinition(projectileDefinition);
                        recycleTimer = Time.time + RecycleTimeModified;
                        owner.CalculateMaxLockOnRange();
                    }
                }
            }
        }
    }

    void SetProjectileDefinition(ProjectileDefinition definition)
    {
        ProjectileDefinition = definition;
        firingAudioClip = ProjectileDefinition.GetFiringSound();

        if (firingAudioClip != null)
        {
            weaponModelController.AudioSource.volume = ProjectileDefinition.FiringVolume;
            firingPitchMin = ProjectileDefinition.FiringPitchMin;
            firingPitchMax = ProjectileDefinition.FiringPitchMax;

            if (ProjectileWeaponDefinition.FiringSoundLoops)
            {
                weaponModelController.AudioSource.clip = firingAudioClip;
                weaponModelController.AudioSource.loop = true;
            }
        }

        if (ProjectileDefinition.MissileDefenseTargetable)
        {
            projectileLayer = LayerMask.NameToLayer("ProjectileMissile");
        }
        else
        {
            projectileLayer = LayerMask.NameToLayer("Projectile");
        }

        projectilePrefab = ProjectileDefinition.GetProjectilePrefab();

        trailPrefab = ProjectileDefinition.GetTrailPrefab();

        jammingPerFiring = ProjectileWeaponDefinition.GetShotCount() * ProjectileDefinition.Jamming;

        if (modComponentData != null)
        {
            SetWeaponModifier(modComponentData);
        }
    }

    public override void SetWeaponModifier(ComponentData componentData)
    {
        SetDefaultModifiers();

        if (!componentData.isDestroyed)
        {
            modComponentData = componentData;

            for (int modIndex = 0; modIndex < componentData.ComponentDefinition.WeaponModifications.Length; modIndex++)
            {
                WeaponModification weaponModification = componentData.ComponentDefinition.WeaponModifications[modIndex];

                for (int weaponClassIndex = 0; weaponClassIndex < ProjectileWeaponDefinition.WeaponClassifications.Length; weaponClassIndex++)
                {
                    if (ProjectileWeaponDefinition.WeaponClassifications[weaponClassIndex] == weaponModification.WeaponClassification)
                    {
                        switch (weaponModification.WeaponModificationType)
                        {
                            case WeaponModificationType.Damage:
                                {
                                    DamageModifier = 1.0f + weaponModification.Value;
                                    break;
                                }
                            case WeaponModificationType.Heat:
                                {
                                    HeatModified = ProjectileDefinition.Heat * (1.0f + weaponModification.Value);
                                    break;
                                }
                            case WeaponModificationType.RecycleTime:
                                {
                                    RecycleTimeModified = ProjectileWeaponDefinition.RecycleTime * (1.0f + weaponModification.Value);
                                    break;
                                }
                        }

                        break;
                    }
                }
            }
        }
    }

    public override void SetDefaultModifiers()
    {
        DamageModifier = 1.0f;

        if (ProjectileDefinition != null)
        {
            HeatModified = ProjectileDefinition.Heat;

            RecycleTimeModified = ProjectileWeaponDefinition.RecycleTime;
        }
    }
}
