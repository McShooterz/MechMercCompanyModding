using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class BeamWeaponControllerStandard : BeamWeaponController
{
    protected override void Awake()
    {
        base.Awake();
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        beamCharge = BeamDurationModified;
    }

    // Update is called once per frame
    void Update()
    {
        if (isDestroyed)
            return;

        if (isFiring)
        {
            if (Time.time > firingTimer)
            {
                beamRechargeTimer = Time.time + beamWeaponDefinition.RechargeDelay;
                EndFiring();
            }
            else
            {
                FireBeam();

                beamCharge -= Time.deltaTime;

                if (beamCharge < 0f)
                {
                    beamCharge = 0f;
                    fullRecharging = true;
                    beamRechargeTimer = 0f;
                    EndFiring();
                }

                owner.AddHeat(HeatModified * Time.deltaTime);

                if (beamWeaponDefinition.JamPerSecond > 0)
                {
                    jamming += beamWeaponDefinition.JamPerSecond * Time.deltaTime;
                    jammingDecayTimer = Time.time + beamWeaponDefinition.JamDecayDelay;

                    if (jamming > 1.0f)
                    {
                        jamming = 1.0f;
                        jammed = true;
                        EndFiring();
                    }
                }
            }
        }
        else
        {
            if (beamCharge < BeamDurationModified && Time.time > beamRechargeTimer)
            {
                beamCharge += BeamRechargeModified * Time.deltaTime;

                if (beamCharge > BeamDurationModified)
                {
                    beamCharge = BeamDurationModified;
                    fullRecharging = false;
                }
            }

            UpdateJamming(beamWeaponDefinition.JamDecay, beamWeaponDefinition.JammedDecay);
        }
    }

    public override void Fire(bool firedThisFrame)
    {
        if (PreventFiring)
        {
            return;
        }

        firingTimer = Time.time + beamWeaponDefinition.DurationMin;

        if (beamCharge > 0 && !isFiring)
        {
            StartFiring();
        }
    }

    public override void FireAI()
    {
        if (PreventFiring || jamming > 0.9f)
        {
            return;
        }

        firingTimer = Time.time + beamWeaponDefinition.DurationMin;

        if (beamCharge > 0 && !isFiring)
        {
            StartFiring();
        }
    }
}
