using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class BeamWeaponControllerCharging : BeamWeaponController
{
    protected override void Awake()
    {
        base.Awake();
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        beamCharge = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (isDestroyed)
            return;

        if (isFiring)
        {
            beamCharge -= Time.deltaTime;

            if (beamCharge < 0)
            {
                beamCharge = 0.0f;
                EndFiring();
            }
            else
            {
                FireBeam();

                owner.AddHeat(HeatModified * Time.deltaTime);
            }
        }
        else if (beamCharge > 0 && Time.time > firingTimer)
        {
            StartFiring();

            jamming = 0.0f;
        }
        else if (jammed)
        {
            jamming -= beamWeaponDefinition.JammedDecay * Time.deltaTime;

            if (jamming <= 0.0f)
            {
                jamming = 0.0f;
                jammed = false;
            }
        }
    }

    public override void Fire(bool firedThisFrame)
    {
        if (PreventFiring)
        {
            return;
        }

        if (!isFiring)
        {
            firingTimer = Time.time + beamWeaponDefinition.DurationMin;

            if (beamCharge < BeamDurationModified)
            {
                beamCharge += BeamRechargeModified * Time.deltaTime;

                if (beamCharge > BeamDurationModified)
                {
                    beamCharge = BeamDurationModified;
                }
            }
            else
            {
                jamming += beamWeaponDefinition.JamPerSecond * Time.deltaTime;

                if (jamming > 1.0f)
                {
                    jamming = 1.0f;
                    jammed = true;
                    beamCharge = 0.0f;
                }
            }
        }
        else if (firedThisFrame)
        {
            EndFiring();
        }
    }

    public override void FireAI()
    {
        if (PreventFiring || beamCharge == BeamDurationModified)
        {
            return;
        }

        if (!isFiring)
        {
            firingTimer = Time.time + beamWeaponDefinition.DurationMin;

            beamCharge += BeamRechargeModified * Time.deltaTime;

            if (beamCharge > BeamDurationModified)
            {
                beamCharge = BeamDurationModified;
            }
        }
    }

    public override void Stop()
    {
        base.Stop();

        beamCharge = 0.0f;
    }
}
