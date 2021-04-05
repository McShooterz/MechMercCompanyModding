using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class ProjectileWeaponControllerStandard : ProjectileWeaponController
{
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
        else
        {
            UpdateJamming(ProjectileWeaponDefinition.JamDecay, ProjectileWeaponDefinition.JammedDecay);
        }

        if (weaponModelController.AudioSource.isPlaying && ProjectileWeaponDefinition.FiringSoundLoops && Time.time > firingAudioTimer)
        {
            weaponModelController.AudioSource.Stop();
        }
    }

    public override void Fire(bool firedThisFrame)
    {
        if (isDestroyed || ammoEmpty || jammed || Time.time < recycleTimer)
        {
            return;
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
    }

    public override void FireAI()
    {
        if (jamming + jammingPerFiring > 1.0f)
        {
            return;
        }

        Fire(false);
    }
}
