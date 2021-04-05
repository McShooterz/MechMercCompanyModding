using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeamWeaponControllerContinuous : BeamWeaponController
{
    protected override void Awake()
    {
        base.Awake();
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        beamCharge = 1.0f;
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
                EndFiring();
            }
            else
            {
                FireBeam();

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
            UpdateJamming(beamWeaponDefinition.JamDecay, beamWeaponDefinition.JammedDecay);
        }
    }

    public override void Fire(bool firedThisFrame)
    {
        if (isDestroyed || jammed)
        {
            return;
        }

        firingTimer = Time.time + beamWeaponDefinition.DurationMin;

        if (!isFiring)
        {
            StartFiring();
        }
    }

    public override void FireAI()
    {
        if (isDestroyed || jammed || jamming > 0.9f)
        {
            return;
        }

        firingTimer = Time.time + beamWeaponDefinition.DurationMin;

        if (!isFiring)
        {
            StartFiring();
        }
    }

    public override float GetRefireBar()
    {
        return 0f;
    }
}
