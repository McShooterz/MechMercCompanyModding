using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class ProjectileWeaponControllerRapidRefire : ProjectileWeaponController
{
    float jammingAtEndOfSalvo = 0.0f;
    float refireThreshold = 0.0f;

    void Update()
    {
        if (IsDestroyed || ammoEmpty)
        {
            return;
        }

        if (salvoCount > 0 && Time.time > salvoTimer)
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

                    if (salvoCount == 0)
                    {
                        jamming += jammingAtEndOfSalvo;
                        jammingDecayTimer = Time.time + ProjectileWeaponDefinition.JamDecayDelay;

                        if (jamming > 1.0f)
                        {
                            jammingAtEndOfSalvo = 0.0f;
                            jamming = 1.0f;
                            jammed = true;
                        }
                    }
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

                if (salvoCount == 0)
                {
                    jamming += jammingAtEndOfSalvo;
                    jammingDecayTimer = Time.time + ProjectileWeaponDefinition.JamDecayDelay;

                    if (jamming > 1.0f)
                    {
                        jammingAtEndOfSalvo = 0.0f;
                        jamming = 1.0f;
                        jammed = true;
                    }
                }
            }
        }
        else
        {
            UpdateJamming(ProjectileWeaponDefinition.JamDecay, ProjectileWeaponDefinition.JammedDecay);
        }

        if (weaponModelController.AudioSource.isPlaying && ProjectileWeaponDefinition.FiringSoundLoops && Time.time > firingAudioTimer)
        {
            weaponModelController.AudioSource.Stop();
        }
    }

    public override void SetDefinition(ProjectileWeaponDefinition definition)
    {
        base.SetDefinition(definition);

        refireThreshold = definition.RecycleTime * 0.4f;
    }

    public override void Fire(bool firedThisFrame)
    {
        if (isDestroyed || ammoEmpty || jammed)
        {
            return;
        }

        bool isRapidRefire = firedThisFrame && Time.time < recycleTimer && Time.time > salvoTimer;

        if (Time.time > recycleTimer || isRapidRefire)
        {
            float recycleRatio = 0.0f;

            if (isRapidRefire)
            {
                recycleRatio = (recycleTimer - Time.time) / RecycleTimeModified;
            }

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

                    if (isRapidRefire)
                    {
                        float JammingAmount = recycleRatio * ProjectileWeaponDefinition.JammingRapidRefire;

                        if (salvoCount > 0)
                        {
                            jammingAtEndOfSalvo = JammingAmount;
                        }
                        else
                        {
                            jamming += JammingAmount;
                            jammingDecayTimer = Time.time + ProjectileWeaponDefinition.JamDecayDelay;

                            if (jamming > 1.0f)
                            {
                                jamming = 1.0f;
                                jammed = true;
                            }
                        }
                    }
                    else
                    {
                        jammingAtEndOfSalvo = 0.0f;
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

                if (isRapidRefire)
                {
                    float JammingAmount = recycleRatio * ProjectileWeaponDefinition.JammingRapidRefire;

                    if (salvoCount > 0)
                    {
                        jammingAtEndOfSalvo = JammingAmount;
                    }
                    else
                    {
                        jamming += JammingAmount;
                        jammingDecayTimer = Time.time + ProjectileWeaponDefinition.JamDecayDelay;

                        if (jamming > 1.0f)
                        {
                            jamming = 1.0f;
                            jammed = true;
                        }
                    }
                }
                else
                {
                    jammingAtEndOfSalvo = 0.0f;
                }
            }
        }
    }

    public override void FireAI()
    {
        if (isDestroyed || ammoEmpty || jammed || jamming + jammingPerFiring > 1.0f)
        {
            return;
        }

        if (Time.time > recycleTimer)
        {
            Fire(false);
        }
        else if (jamming < 0.7f && Time.time > (recycleTimer - refireThreshold))
        {
            Fire(true);
        }
    }
}