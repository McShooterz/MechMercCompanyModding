using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class ProjectileWeaponControllerCharging : ProjectileWeaponController
{
    [SerializeField]
    float charging = 0.0f;

    [SerializeField]
    float dischargeTimer = 0.0f;

    public override bool CanFire { get => !ammoEmpty; }

    public override bool IsCharged { get => charging == 1.0f; }

    void Update()
    {
        if (IsDestroyed || ammoEmpty)
        {
            return;
        }

        if (jammed)
        {
            jamming -= ProjectileWeaponDefinition.JammedDecay;

            if (jamming < 0.0f)
            {
                jamming = 0.0f;
                jammed = false;
            }
        }
        else if (salvoCount > 0 && Time.time > salvoTimer)
        {
            if (ProjectileWeaponDefinition.RequiresAmmo)
            {
                int shotsCount;

                if (owner is MechControllerPlayer && Cheats.unlimitedAmmo)
                {
                    shotsCount = ProjectileWeaponDefinition.ShotsPerSalvo;
                }
                else
                {
                    shotsCount = CurrentAmmoPool.TakeAmmo(ProjectileWeaponDefinition.ShotsPerSalvo);
                }

                if (shotsCount > 0)
                {
                    FireShots(owner.GetTargetingPoint(targetingIndex), shotsCount, ProjectileDefinition.ProjectileCount);
                    PlayFiringSound();

                    salvoCount -= 1;

                    salvoTimer = Time.time + ProjectileWeaponDefinition.SalvoDelay;
                }
                else
                {
                    if (ProjectileWeaponDefinition.FiringSoundLoops && weaponModelController.AudioSource.isPlaying)
                    {
                        weaponModelController.AudioSource.Stop();
                    }

                    AutoSwitchAmmo();
                }
            }
            else
            {
                FireShots(owner.GetTargetingPoint(targetingIndex), ProjectileWeaponDefinition.ShotsPerSalvo, ProjectileDefinition.ProjectileCount);
                PlayFiringSound();
                salvoCount -= 1;

                salvoTimer = Time.time + ProjectileWeaponDefinition.SalvoDelay;
            }
        }
        else if (charging == 1.0f)
        {
            jamming += ProjectileDefinition.Jamming * Time.deltaTime;

            if (jamming > 1.0f)
            {
                charging = 0.0f;

                if (ProjectileWeaponDefinition.JammedDecay > 0.0f)
                {
                    jammed = true;
                }
                else
                {
                    jamming = 0.0f;
                }
            }
        }
        else if (Time.time > dischargeTimer)
        {
            charging = 0.0f;
        }

        if (weaponModelController.AudioSource.isPlaying && ProjectileWeaponDefinition.FiringSoundLoops && Time.time > firingAudioTimer)
        {
            weaponModelController.AudioSource.Stop();
        }
    }

    public override void Fire(bool firedThisFrame)
    {
        if (isDestroyed || ammoEmpty || jammed)
        {
            return;
        }
        
        if (Time.time > recycleTimer)
        {
            if (charging < 1.0f)
            {
                charging += ProjectileWeaponDefinition.ChargingRate * Time.deltaTime;

                dischargeTimer = Time.time + 0.1f;

                if (charging > 1.0f)
                {
                    charging = 1.0f;
                }
            }
            else if (firedThisFrame)
            {
                if (ProjectileWeaponDefinition.RequiresAmmo)
                {
                    int shotsCount;

                    if (owner is MechControllerPlayer && Cheats.unlimitedAmmo)
                    {
                        shotsCount = ProjectileWeaponDefinition.ShotsPerSalvo;
                    }
                    else
                    {
                        shotsCount = CurrentAmmoPool.TakeAmmo(ProjectileWeaponDefinition.ShotsPerSalvo);
                    }

                    if (shotsCount > 0)
                    {
                        if (ProjectileDefinition.HomingType == ProjectileHomingType.LockOn)
                        {
                            homingTarget = owner.CurrentLockedOnTarget;

                            if (homingTarget == null)
                            {
                                homingTargetPosition = owner.GetTargetingPoint(targetingIndex);
                            }
                        }

                        FireShots(owner.GetTargetingPoint(targetingIndex), shotsCount, ProjectileDefinition.ProjectileCount);

                        recycleTimer = Time.time + RecycleTimeModified;

                        PlayFiringSound();

                        if (ProjectileWeaponDefinition.SalvoCount > 1 && !ammoEmpty)
                        {
                            salvoCount = ProjectileWeaponDefinition.SalvoCount - 1;
                            salvoTimer = Time.time + ProjectileWeaponDefinition.SalvoDelay;
                        }
                    }
                    else
                    {
                        if (ProjectileDefinition.HomingType == ProjectileHomingType.LockOn)
                        {
                            homingTarget = owner.CurrentLockedOnTarget;

                            if (homingTarget == null)
                            {
                                homingTargetPosition = owner.GetTargetingPoint(targetingIndex);
                            }
                        }

                        if (ProjectileWeaponDefinition.FiringSoundLoops && weaponModelController.AudioSource.isPlaying)
                        {
                            weaponModelController.AudioSource.Stop();
                        }

                        AutoSwitchAmmo();
                    }
                }
                else
                {
                    FireShots(owner.GetTargetingPoint(targetingIndex), ProjectileWeaponDefinition.ShotsPerSalvo, ProjectileDefinition.ProjectileCount);

                    recycleTimer = Time.time + RecycleTimeModified;

                    PlayFiringSound();

                    if (ProjectileWeaponDefinition.SalvoCount > 1)
                    {
                        salvoCount = ProjectileWeaponDefinition.SalvoCount - 1;
                        salvoTimer = Time.time + ProjectileWeaponDefinition.SalvoDelay;
                    }
                }

                charging = 0.0f;
                jamming = 0.0f;
                jammed = false;
            }
        }
    }

    public override void FireAI()
    {
        if (isDestroyed || ammoEmpty || jammed)
        {
            return;
        }

        if (Time.time > recycleTimer)
        {
            if (charging < 1.0f)
            {
                charging += ProjectileWeaponDefinition.ChargingRate * Time.deltaTime;

                dischargeTimer = Time.time + 2.0f;

                if (charging > 1.0f)
                {
                    charging = 1.0f;
                }
            }
            else
            {
                if (ProjectileWeaponDefinition.RequiresAmmo)
                {
                    int shotsCount;

                    if (owner is MechControllerPlayer && Cheats.unlimitedAmmo)
                    {
                        shotsCount = ProjectileWeaponDefinition.ShotsPerSalvo;
                    }
                    else
                    {
                        shotsCount = CurrentAmmoPool.TakeAmmo(ProjectileWeaponDefinition.ShotsPerSalvo);
                    }

                    if (shotsCount > 0)
                    {
                        if (ProjectileDefinition.HomingType == ProjectileHomingType.LockOn)
                        {
                            homingTarget = owner.CurrentLockedOnTarget;

                            if (homingTarget == null)
                            {
                                homingTargetPosition = owner.GetTargetingPoint(targetingIndex);
                            }
                        }

                        FireShots(owner.GetTargetingPoint(targetingIndex), shotsCount, ProjectileDefinition.ProjectileCount);

                        recycleTimer = Time.time + RecycleTimeModified;

                        PlayFiringSound();

                        if (ProjectileWeaponDefinition.SalvoCount > 1 && !ammoEmpty)
                        {
                            salvoCount = ProjectileWeaponDefinition.SalvoCount - 1;
                            salvoTimer = Time.time + ProjectileWeaponDefinition.SalvoDelay;
                        }
                    }
                    else
                    {
                        if (ProjectileDefinition.HomingType == ProjectileHomingType.LockOn)
                        {
                            homingTarget = owner.CurrentLockedOnTarget;

                            if (homingTarget == null)
                            {
                                homingTargetPosition = owner.GetTargetingPoint(targetingIndex);
                            }
                        }

                        if (ProjectileWeaponDefinition.FiringSoundLoops && weaponModelController.AudioSource.isPlaying)
                        {
                            weaponModelController.AudioSource.Stop();
                        }

                        AutoSwitchAmmo();
                    }
                }
                else
                {
                    FireShots(owner.GetTargetingPoint(targetingIndex), ProjectileWeaponDefinition.ShotsPerSalvo, ProjectileDefinition.ProjectileCount);

                    recycleTimer = Time.time + RecycleTimeModified;

                    PlayFiringSound();

                    if (ProjectileWeaponDefinition.SalvoCount > 1)
                    {
                        salvoCount = ProjectileWeaponDefinition.SalvoCount - 1;
                        salvoTimer = Time.time + ProjectileWeaponDefinition.SalvoDelay;
                    }
                }

                charging = 0.0f;
                jamming = 0.0f;
                jammed = false;
            }
        }
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
            return 1.0f - difference / RecycleTimeModified;
        }

        return 1.0f - charging;
    }
}
