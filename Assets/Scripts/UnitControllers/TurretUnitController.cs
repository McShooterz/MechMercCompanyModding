using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretUnitController : CombatUnitController
{
    [Header("Turret Unit Controller")]

    TurretUnitData turretUnitData;

    [SerializeField]
    TurretUnitMetaController turretUnitMetaController;

    public override UnitData UnitData { get => turretUnitData; }

    public TurretUnitData TurretUnitData { get => turretUnitData; }

    public override bool IsDestroyed { get => turretUnitData.isDestroyed; }

    public override Bounds Bounds { get => turretUnitMetaController.MainCollider.bounds; }

    protected override void Start()
    {
        
    }

    protected override void Update()
    {
        if (Time.timeScale == 0.0f || !Application.isFocused)
        {
            return;
        }

        if (IsDestroyed || IsDisabled)
        {
            return;
        }

        base.Update();

        if (aiControlled)
        {
            UpdateAI();
        }
    }

    public override void TakeDamage(Collider hitCollider, Vector2 direction, float damage, WeaponController weaponController)
    {
        if (turretUnitData.isDestroyed)
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

        if (turretUnitMetaController.IsBaseCollider(hitCollider))
        {
            turretUnitData.TakeDamageBase(damage, weaponController);
        }
        else if (turretUnitMetaController.IsCenterCollider(hitCollider))
        {
            turretUnitData.TakeDamageCenter(damage, weaponController);
        }
        else if (turretUnitMetaController.IsPodLeftCollider(hitCollider))
        {
            turretUnitData.TakeDamagePodLeft(damage, weaponController);
        }
        else if (turretUnitMetaController.IsPodRightCollider(hitCollider))
        {
            turretUnitData.TakeDamagePodRight(damage, weaponController);
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
        if (turretUnitData.isDestroyed)
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

        if (turretUnitMetaController.IsBaseCollider(hitCollider))
        {
            float majorDamage = damage * 0.75f;
            float minorDamage = damage - majorDamage;

            turretUnitData.TakeDamageBase(majorDamage, weaponController);
            turretUnitData.TakeDamageCenter(minorDamage, weaponController);
        }
        else if (turretUnitMetaController.IsCenterCollider(hitCollider))
        {
            float majorDamage = damage * 0.6667f;
            float minorDamage = (damage - majorDamage) / 3.0f;

            turretUnitData.TakeDamageCenter(majorDamage, weaponController);

            turretUnitData.TakeDamageBase(minorDamage, weaponController);         
            turretUnitData.TakeDamagePodLeft(minorDamage, weaponController);
            turretUnitData.TakeDamagePodRight(minorDamage, weaponController);
        }
        else if (turretUnitMetaController.IsPodLeftCollider(hitCollider))
        {
            float majorDamage = damage * 0.75f;
            float minorDamage = damage - majorDamage;

            turretUnitData.TakeDamagePodLeft(majorDamage, weaponController);

            turretUnitData.TakeDamageCenter(minorDamage, weaponController);

        }
        else if (turretUnitMetaController.IsPodRightCollider(hitCollider))
        {
            float majorDamage = damage * 0.75f;
            float minorDamage = damage - majorDamage;

            turretUnitData.TakeDamagePodRight(majorDamage, weaponController);

            turretUnitData.TakeDamageCenter(minorDamage, weaponController);
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
        if (turretUnitData.isDestroyed)
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

        damage = damage / 4.0f;

        turretUnitData.TakeDamageBase(damage, weaponController);
        turretUnitData.TakeDamageCenter(damage, weaponController);
        turretUnitData.TakeDamagePodLeft(damage, weaponController);
        turretUnitData.TakeDamagePodRight(damage, weaponController);

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

    public override float GetTargetLockOnBonus()
    {
        return turretUnitData.LockOnBonus;
    } 

    public override float GetRadarDetectionReduction()
    {
        return turretUnitData.radarDetectionReduction;
    }

    public override float GetRadarDetectionRange()
    {
        return turretUnitData.radarDetectionRange;
    }

    public void EjectPodLeft()
    {
        turretUnitMetaController.EjectPodLeft();
    }

    public void EjectPodRight()
    {
        turretUnitMetaController.EjectPodRight();
    }

    public override void Die()
    {
        turretUnitData.isDestroyed = true;

        StartCoroutine(TurnOffRadarDetectable());

        GameObject smokePrefab = ResourceManager.Instance.GetEffectPrefab("SmokeLoop");

        turretUnitMetaController.SetCollidersToDebris();

        turretUnitMetaController.EjectAllParts();

        if ((object)smokePrefab != null)
        {
            GameObject smokeEffect = Instantiate(smokePrefab);

            smokeEffect.transform.position = Bounds.center;
            smokeEffect.transform.parent = transform;
        }

        MissionManager.Instance.UpdateObjectives();
    }

    public void DisableTurret()
    {
        turretUnitData.isDisabled = true;

        StartCoroutine(TurnOffRadarDetectable());

        MissionManager.Instance.UpdateObjectives();
    }

    public void CreateDeathExplosion()
    {
        GameObject explosion = PoolingManager.Instance.Spawn(ResourceManager.Instance.GetEffectPrefab("ExplosionMech"), Bounds.center, transform.rotation);

        EffectsController effectsController = explosion.GetComponent<EffectsController>();

        effectsController.AudioSource.PlayOneShot(ResourceManager.Instance.GetAudioClip("Explosion0"));
    }

    public void SetTurretData(TurretUnitData data)
    {
        turretUnitData = data;

        turretUnitData.turretUnitController = this;

        turretUnitMetaController = GetComponent<TurretUnitMetaController>();

        turretUnitMetaController.TurretController.SetTurretSetting(turretUnitData.turretDefinition.TurretSetting);

        List<WeaponController> weaponControllers = new List<WeaponController>();

        weaponControllers.AddRange(AddWeapons(turretUnitData.componentDatasCenter, turretUnitMetaController.HardpointsCenter, turretUnitData.ammoPools));
        weaponControllers.AddRange(AddWeapons(turretUnitData.componentDatasPodLeft, turretUnitMetaController.HardpointsPodLeft, turretUnitData.ammoPools));
        weaponControllers.AddRange(AddWeapons(turretUnitData.componentDatasPodRight, turretUnitMetaController.HardpointsPodRight, turretUnitData.ammoPools));

        weaponControllersAll = weaponControllers.ToArray();

        CalculateMaxLockOnRange();
    }

    protected override void UpdateAI()
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

            if (weaponControllersAll.Length > 0)
            {
                FireAtTarget(targetDistance);
            }

            if (currentLockedOnTarget != null && Time.time > lockedOnTimer)
            {
                currentLockedOnTarget = null;
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

        float targetAngle = turretUnitMetaController.TurretController.AimAtPosition(targetUnit.TargetablePosition + aiTargetingOffset);

        if (targetAngle < 20.0f)
        {
            targetingPoints[0] = turretUnitMetaController.TurretController.TargetingPoint;

            foreach (WeaponController weaponController in weaponControllersAll)
            {
                if (weaponController.IsDestroyed)
                {
                    continue;
                }

                if (weaponController.InMaxRange(targetDistance))
                {
                    weaponController.FireAI();
                }
            }

            if (targetDistance < maxLockOnRange)
            {
                if (currentLockedOnTarget == targetUnit)
                {
                    lockedOnTimer = Time.time + 2.0f;
                }
                else
                {
                    lockingOnValue += GetLockValue(targetDistance, 200f, GetTargetLockOnBonus()) * 0.5f;

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
        else
        {
            lockingOnValue = 0.0f;
        }
    }

    protected override void UpdateTargetOffSetAI()
    {
        aiTargetingOffset = Random.onUnitSphere;
    }
}
