using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileDefenseController : EquipmentController
{
    [SerializeField]
    WeaponModelController weaponModelController;

    [SerializeField]
    BeamController beamController;

    [SerializeField]
    AudioClip firingAudioClip;

    [SerializeField]
    float firingAudioTimer;

    [SerializeField]
    float recycleTimer;

    [SerializeField]
    float firingTimer;

    public MissileDefenseDefinition MissileDefenseDefinition { get; private set; }

    public float Range { get => MissileDefenseDefinition.Range; }

    public MissileDefenseType CurrentMissileDefenseType
    {
        get
        {
            return CurrentMode.MissileDefenseType;
        }
    }

    public bool CanFire { get => !ammoEmpty && Time.time > recycleTimer; }

    void Update()
    {
        if (Time.time > firingTimer)
        {
            if (beamController != null && beamController.gameObject.activeInHierarchy)
            {
                beamController.TurnOff();
            }

            if (weaponModelController.AudioSource.isPlaying && MissileDefenseDefinition.FiringSoundLoops)
            {
                weaponModelController.AudioSource.Stop();
            }
        }
        else
        {
            if (beamController != null && beamController.gameObject.activeInHierarchy)
            {
                beamController.LineRenderer.SetPosition(0, weaponModelController.NextFiringPoint.position);
            }
        }
    }

    public void Initialize(CombatUnitController ownerController, MissileDefenseDefinition definition, WeaponModelController modelController)
    {
        Owner = ownerController;
        Definition = definition;
        MissileDefenseDefinition = definition;
        weaponModelController = modelController;

        if (Definition.EquipmentModes.Length > 0)
        {
            CurrentMode = Definition.EquipmentModes[0];
        }

        weaponModelController.InitializeAudioSource();

        firingAudioClip = MissileDefenseDefinition.GetFiringSound();

        if (firingAudioClip != null & MissileDefenseDefinition.FiringSoundLoops)
        {
            weaponModelController.AudioSource.clip = firingAudioClip;
            weaponModelController.AudioSource.loop = true;
        }

        GameObject beamPrefab = MissileDefenseDefinition.GetBeamPrefab();

        if ((object)beamPrefab != null)
        {
            GameObject beamObject = Instantiate(beamPrefab, transform);

            beamController = beamObject.GetComponent<BeamController>();

            if (beamController != null)
            {
                beamController.SetBeamWidth(MissileDefenseDefinition.BeamWidth);
            }
            else
            {
                Destroy(beamObject);
            }
        }
    }

    public void Fire(ProjectileController targetMissile)
    {
        if (ammoPool != null && !(Owner is MechControllerPlayer && Cheats.unlimitedAmmo))
        {
            if (ammoPool.TakeAmmo(1) == 0)
            {
                ammoEmpty = true;
                Owner.UpdateMissileDefenseRange();
                return;
            }
        }

        recycleTimer = Time.time + MissileDefenseDefinition.RecycleTime;
        firingTimer = Time.time + MissileDefenseDefinition.FiringTime;

        Owner.AddHeat(MissileDefenseDefinition.Heat);

        targetMissile.TakeDamage(MissileDefenseDefinition.Damage);

        if (firingAudioClip != null)
        {
            if (MissileDefenseDefinition.FiringSoundLoops)
            {
                weaponModelController.AudioSource.Play();
            }
            else
            {
                AudioManager.Instance.PlayClip(weaponModelController.AudioSource, firingAudioClip, true, true);
            }
        }

        if (beamController != null)
        {
            beamController.LineRenderer.SetPosition(1, targetMissile.transform.position);
        }
    }

    public bool InRange(float range) { return MissileDefenseDefinition.InRange(range); }
}