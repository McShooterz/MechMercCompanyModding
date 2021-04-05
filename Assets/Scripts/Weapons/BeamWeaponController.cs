using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BeamWeaponController : WeaponController
{
    protected BeamWeaponDefinition beamWeaponDefinition;

    [SerializeField]
    protected BeamController[] beamControllers;

    [SerializeField]
    protected bool isFiring = false;

    [SerializeField]
    protected bool fullRecharging = false;

    [SerializeField]
    protected float firingTimer = 0f;

    [SerializeField]
    protected float beamCharge;

    [SerializeField]
    protected float beamRechargeTimer = 0f;

    static LayerMask hitLayerMask;

    int unitLayer = -1;
    int environmentDestructibleLayer = -1;
    int projectileMissileLayer = -1;

    protected float BeamDurationModified { get; set; }

    protected float BeamRechargeModified { get; set; }

    public override bool CanFire { get => !fullRecharging; }

    protected bool PreventFiring { get => isDestroyed || fullRecharging || jammed; }

    protected virtual void Awake()
    {
        if (unitLayer == -1)
        {
            unitLayer = LayerMask.NameToLayer("Unit");
        }

        if (environmentDestructibleLayer == -1)
        {
            environmentDestructibleLayer = LayerMask.NameToLayer("EnvironmentDestructible");
        }

        if (projectileMissileLayer == -1)
        {
            projectileMissileLayer = LayerMask.NameToLayer("ProjectileMissile");
        }

        hitLayerMask |= (1 << LayerMask.NameToLayer("Default")) | (1 << LayerMask.NameToLayer("Terrain")) | (1 << environmentDestructibleLayer) | (1 << projectileMissileLayer);
    }

    // Use this for initialization
    protected virtual void Start ()
    {
        AudioClip audioClip = beamWeaponDefinition.GetFiringSound();

        if (audioClip != null)
        {
            weaponModelController.AudioSource.clip = audioClip;
            weaponModelController.AudioSource.loop = true;
            weaponModelController.AudioSource.volume = beamWeaponDefinition.FiringVolume;
            weaponModelController.AudioSource.pitch = beamWeaponDefinition.FiringPitch;
        }
    }

    public void SetDefinition(BeamWeaponDefinition definition)
    {
        beamWeaponDefinition = definition;

        GameObject beamPrefab = beamWeaponDefinition.GetBeamPrefab();

        if (beamPrefab != null)
        {
            beamControllers = new BeamController[beamWeaponDefinition.BeamCount];

            for (int beamIndex = 0; beamIndex < beamControllers.Length; beamIndex++)
            {
                beamControllers[beamIndex] = Instantiate(beamPrefab).GetComponent<BeamController>();
                beamControllers[beamIndex].transform.parent = weaponModelController.GetFiringPoint(beamIndex);
                beamControllers[beamIndex].transform.localPosition = Vector3.zero;

                beamControllers[beamIndex].SetScaling(beamWeaponDefinition);
            }
        }

        SetDefaultModifiers();
    }

    public override string GetDisplayName()
    {
        return beamWeaponDefinition.GetDisplayName();
    }

    public override void SetDestroyed()
    {
        base.SetDestroyed();

        EndFiring();
    }

    protected void StartFiring()
    {
        isFiring = true;

        for (int i = 0; i < beamControllers.Length; i++)
        {
            BeamController beamController = beamControllers[i];

            beamController.MuzzleEffect.SetActive(true);
        }

        if (!weaponModelController.AudioSource.isPlaying)
        {
            weaponModelController.AudioSource.Play();
        }
    }

    protected void EndFiring()
    {
        isFiring = false;

        foreach (BeamController beamController in beamControllers)
        {
            beamController.TurnOff();
        }

        weaponModelController.AudioSource.Pause();
    }

    protected void FireBeam()
    {
        Vector3 firingDirection = owner.GetTargetingPoint(targetingIndex) - beamControllers[0].transform.position;
        RaycastHit hit;

        if (Physics.Raycast(beamControllers[0].transform.position, firingDirection, out hit, beamWeaponDefinition.RangeMax, hitLayerMask, QueryTriggerInteraction.Ignore))
        {
            for (int i = 0; i < beamControllers.Length; i++)
            {
                BeamController beamController = beamControllers[i];

                beamController.ImpactEffect.SetActive(true);
                beamController.ImpactEffect.transform.position = hit.point + hit.normal * 0.02f;

                beamController.LineRenderer.SetPosition(0, beamController.transform.position);
                beamController.LineRenderer.SetPosition(1, hit.point);
            }

            if (hit.collider.transform.root.gameObject.layer == unitLayer)
            {
                UnitController unitController = hit.collider.transform.root.gameObject.GetComponent<UnitController>();

                unitController.TakeDamage(hit.collider, new Vector2(firingDirection.x, firingDirection.z), beamWeaponDefinition.GetDamage((hit.point - transform.position).magnitude) * Time.deltaTime, this);
            }
            else if (hit.collider.gameObject.layer == environmentDestructibleLayer)
            {
                EnvironmentDestructible environmentDestructible = hit.collider.GetComponentInParent<EnvironmentDestructible>();

                environmentDestructible.TakeDamage(beamWeaponDefinition.GetDamage((hit.point - transform.position).magnitude) * Time.deltaTime);
            }
            else if (hit.collider.gameObject.layer == projectileMissileLayer)
            {
                ProjectileController projectileController = hit.collider.GetComponent<ProjectileController>();

                projectileController.TakeDamage(beamWeaponDefinition.GetDamage((hit.point - transform.position).magnitude) * Time.deltaTime);
            }
        }
        else
        {
            for (int i = 0; i < beamControllers.Length; i++)
            {
                BeamController beamController = beamControllers[i];

                beamController.ImpactEffect.SetActive(false);
                beamController.LineRenderer.SetPosition(0, beamController.transform.position);
                beamController.LineRenderer.SetPosition(1, beamController.transform.position + firingDirection.normalized * beamWeaponDefinition.RangeMax);
            }
        }

        if (beamWeaponDefinition.PulseFrequency != 0.0f)
        {
            float pulseValue = Mathf.PingPong(Time.time * beamWeaponDefinition.PulseFrequency, 1f) * beamWeaponDefinition.BeamWidth;

            for (int i = 0; i < beamControllers.Length; i++)
            {
                beamControllers[i].SetBeamWidth(pulseValue);
            }
        }
    }

    public override void Stop()
    {
        base.Stop();

        EndFiring();
    }

    public override float GetRefireBar()
    {
        if (beamCharge == 0f)
        {
            return 1.0f;
        }
        else if (beamCharge == BeamDurationModified)
        {
            return 0f;
        }

        return 1.0f - beamCharge / BeamDurationModified;
    }

    public override float GetRangeEffective()
    {
        return beamWeaponDefinition.RangeEffective;
    }

    public override bool InEffectiveRange(float distance)
    {
        return beamWeaponDefinition.InEffectiveRange(distance);
    }

    public override bool InMaxRange(float distanceSqr)
    {
        return beamWeaponDefinition.InMaxRange(distanceSqr);
    }

    public override DamageType GetDamageType()
    {
        return beamWeaponDefinition.DamageType;
    }

    public override bool GetCriticalRole()
    {
        return beamWeaponDefinition.GetCriticalRole();
    }

    public override float GetCriticalDamageMulti()
    {
        return beamWeaponDefinition.CriticalDamageMulti;
    }

    public override void SetWeaponModifier(ComponentData componentData)
    {
        SetDefaultModifiers();

        if (!componentData.isDestroyed)
        {
            for (int modIndex = 0; modIndex < componentData.ComponentDefinition.WeaponModifications.Length; modIndex++)
            {
                WeaponModification weaponModification = componentData.ComponentDefinition.WeaponModifications[modIndex];

                for (int weaponClassIndex = 0; weaponClassIndex < beamWeaponDefinition.WeaponClassifications.Length; weaponClassIndex++)
                {
                    if (beamWeaponDefinition.WeaponClassifications[weaponClassIndex] == weaponModification.WeaponClassification)
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
                                    HeatModified = beamWeaponDefinition.Heat * (1.0f + weaponModification.Value);
                                    break;
                                }
                            case WeaponModificationType.BeamDuration:
                                {
                                    BeamDurationModified = beamWeaponDefinition.Duration * (1.0f + weaponModification.Value);
                                    break;
                                }
                            case WeaponModificationType.BeamRecharge:
                                {
                                    BeamRechargeModified = beamWeaponDefinition.Recharge * (1.0f + weaponModification.Value);
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

        HeatModified = beamWeaponDefinition.Heat;

        BeamDurationModified = beamWeaponDefinition.Duration;

        BeamRechargeModified = beamWeaponDefinition.Recharge;
    }
}
