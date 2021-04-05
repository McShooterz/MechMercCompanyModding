using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class MechControllerAI : MechController
{




    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Update()
    {


        base.Update();


    }

    protected void MoveTowardsTargetPosition(Vector3 position)
    {
        if (isGrounded)
        {
            Vector3 direction = position - transform.position;
            float distance = direction.magnitude;


            if (distance > 1.0f)
            {
                float angle = Vector2.SignedAngle(new Vector2(transform.forward.x, transform.forward.z), new Vector2(direction.x, direction.z));

                if (angle > 1.0f)
                {
                    transform.Rotate(0, -mechData.movementSpeedTurn * Time.deltaTime, 0);
                    animationTurning = Mathf.Lerp(animationTurning, 1f, 10f * Time.deltaTime);
                }
                else if (angle < -1.0f)
                {
                    transform.Rotate(0, mechData.movementSpeedTurn * Time.deltaTime, 0);
                    animationTurning = Mathf.Lerp(animationTurning, 1f, 10f * Time.deltaTime);
                }
                else
                {
                    animationTurning = Mathf.Lerp(animationTurning, 0f, 0.334f * Time.deltaTime);
                }

                if (Mathf.Abs(angle) < 30.0f)
                {
                    if (distance > 3.0f)
                    {
                        targetThrottle = 1.0f;
                    }
                    else
                    {
                        targetThrottle = 0.5f;
                    }
                }
            }
            else
            {
                navMeshPath.ClearCorners();
            }
        }
    }

    protected void TurnTowardsDirection(Vector3 direction)
    {
        targetThrottle = 0.0f;

        if (isGrounded)
        {
            float angle = Vector2.SignedAngle(new Vector2(transform.forward.x, transform.forward.z), new Vector2(direction.x, direction.z));

            if (angle > 1.0f)
            {
                transform.Rotate(0, -mechData.movementSpeedTurn * Time.deltaTime, 0);
                animationTurning = Mathf.Lerp(animationTurning, 1f, 10f * Time.deltaTime);
            }
            else if (angle < -1.0f)
            {
                transform.Rotate(0, mechData.movementSpeedTurn * Time.deltaTime, 0);
                animationTurning = Mathf.Lerp(animationTurning, 1f, 10f * Time.deltaTime);
            }
            else
            {
                animationTurning = Mathf.Lerp(animationTurning, 0f, 0.334f * Time.deltaTime);
            }
        }
    }

    protected void RotateTorsoTowardsTarget(Vector3 targetDirection)
    {
        float torsoAngleHorizontal = Vector2.SignedAngle(new Vector2(mechMetaController.CockpitHardpoint.forward.x, mechMetaController.CockpitHardpoint.forward.z), new Vector2(targetDirection.x, targetDirection.z));
        float baseAngleHorizontal = Vector2.SignedAngle(new Vector2(transform.forward.x, transform.forward.z), new Vector2(targetDirection.x, targetDirection.z));

        float torsoAngleVertical = Vector3.SignedAngle(mechMetaController.CockpitHardpoint.forward, targetDirection, mechMetaController.CockpitHardpoint.right);

        if (Mathf.Abs(baseAngleHorizontal) < mechChassisDefinition.TorsoTwistMax)
        {
            if (torsoAngleHorizontal > 0.0f)
            {
                float change = Mathf.Min(torsoAngleHorizontal, mechData.torsoTwistSpeed * Time.deltaTime);

                currentTorsoTwist -= change;
            }
            else if (torsoAngleHorizontal < 0.0f)
            {
                float change = Mathf.Min(-torsoAngleHorizontal, mechData.torsoTwistSpeed * Time.deltaTime);

                currentTorsoTwist += change;
            }
        }
        else if (baseAngleHorizontal > 0f)
        {
            currentTorsoTwist -= mechData.torsoTwistSpeed * Time.deltaTime;
        }
        else
        {
            currentTorsoTwist += mechData.torsoTwistSpeed * Time.deltaTime;
        }

        if (Mathf.Abs(torsoAngleHorizontal) < 90f)
        {
            if (torsoAngleVertical > 0f)
            {
                float change = Mathf.Min(torsoAngleVertical, mechData.torsoPitchSpeed * Time.deltaTime);

                currentTorsoPitch += change;
            }
            else if (torsoAngleVertical < 0f)
            {
                float change = Mathf.Max(torsoAngleVertical, -mechData.torsoPitchSpeed * Time.deltaTime);

                currentTorsoPitch += change;
            }
        }
        else
        {
            CenterTorsoVertical();
        }
    }

    protected virtual void FireAtTarget(Vector3 targetDirection, float targetDistance)
    {
        if (!clearLineOfSightToTarget)
        {
            lockingOnValue = 0.0f;
            return;
        }

        float targetAngle = Vector3.Angle(mechMetaController.CockpitHardpoint.forward, targetDirection);

        if (targetAngle < 15.0f)
        {
            if (heatCurrent < mechData.heatWarning)
            {
                // Get Targeting Point
                Ray ray = new Ray(mechMetaController.CockpitHardpoint.position, mechMetaController.CockpitHardpoint.forward);

                if (Physics.Raycast(ray, out raycastHit, Mathf.Infinity, targetingLayerMask, QueryTriggerInteraction.Ignore))
                {
                    targetingPoints[0] = raycastHit.point;
                }
                else
                {
                    targetingPoints[0] = mechMetaController.CockpitHardpoint.position + mechMetaController.CockpitHardpoint.forward * 100f;
                }

                for (int i = 0;  i < weaponControllersAll.Length; i++)
                {
                    WeaponController weaponController = weaponControllersAll[i];

                    if (heatCurrent > mechData.heatWarning)
                    {
                        break;
                    }

                    if (weaponController.IsDestroyed)
                    {
                        continue;
                    }

                    if (!weaponController.InMaxRange(targetDistance))
                    {
                        continue;
                    }

                    if (weaponController is ProjectileWeaponController)
                    {
                        ProjectileWeaponController projectileWeaponController = weaponController as ProjectileWeaponController;

                        if (projectileWeaponController.CanFire)
                        {
                            float potentialHeat = heatCurrent + projectileWeaponController.GetWeaponHeat();

                            if (potentialHeat > mechData.heatWarning)
                            {
                                continue;
                            }
                        }
                        else
                        {
                            continue;
                        }
                    }

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
                    lockingOnValue += GetLockValue(targetDistance, 200f, GetTargetLockOnBonus()) * mechData.currentMechPilot.gunnerySkill * 0.3f;

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

    public override void Die()
    {
        base.Die();
    }
}
