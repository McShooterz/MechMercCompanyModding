using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MechControllerGeneralAI : MechControllerAI
{











    protected override void Update()
    {
        if (Time.timeScale == 0.0f || IsDestroyed)
        {
            return;
        }

        base.Update();

        UpdateAI();

        if (isInLava)
        {
            mechData.TakeDamageInternalLegLeft(2f * Time.deltaTime);
            mechData.TakeDamageInternalLegRight(2f * Time.deltaTime);

            AddHeat(3000f * Time.deltaTime);

            currentThrottle = Mathf.Clamp(currentThrottle, -0.5f, 0.5f);
        }

        CheckGround();

        UpdateImpactForce();
    }

    protected override void UpdateAI()
    {
        if (Time.time > changeTargetTimer)
        {
            changeTargetTimer = Time.time + 2.0f;

            targetUnit = GetClosestEnemy();
        }

        if (unitPowerState == UnitPowerState.Normal && targetUnit != null)
        {
            if (targetUnit.IsDestroyed)
            {
                targetUnit = null;

                return;
            }

            Vector3 targetPosition = targetUnit.TargetablePosition + aiTargetingOffset;
            Vector3 targetDirection = targetPosition - mechMetaController.CockpitHardpoint.position;
            float targetDistance = targetDirection.magnitude;

            RotateTorsoTowardsTarget(targetDirection);

            if (Time.time > repathTimer)
            {
                if (targetDistance > 20.0f)
                {
                    repathTimer = Time.time + 1.0f;

                    NavMesh.CalculatePath(transform.position, targetUnit.transform.position, NavMesh.AllAreas, navMeshPath);
                }
                else
                {
                    Vector3 randomPosition = targetUnit.transform.position;
                    randomPosition += Random.insideUnitSphere * 10f;

                    if (NavMesh.SamplePosition(randomPosition, out NavMeshHit navMeshHit, 15.0f, NavMesh.AllAreas))
                    {
                        repathTimer = Time.time + Random.Range(1.5f, 4.0f);

                        NavMesh.CalculatePath(transform.position, navMeshHit.position, NavMesh.AllAreas, navMeshPath);
                    }
                    else
                    {
                        repathTimer = Time.time + 0.25f;

                        navMeshPath.ClearCorners();
                    }
                }
            }

            if (navMeshPath.corners.Length > 1)
            {
                MoveTowardsTargetPosition(navMeshPath.corners[1]);
            }
            else
            {
                MoveTowardsTargetPosition(targetUnit.transform.position);
            }

            FireAtTarget(targetDirection, targetDistance);

            if (currentLockedOnTarget != null && Time.time > lockedOnTimer)
            {
                currentLockedOnTarget = null;
            }
        }
        else
        {
            if (groupIntel.targetLastDetectedPosition != Vector3.zero)
            {
                if (Time.time > repathTimer)
                {
                    repathTimer = Time.time + 1.0f;

                    NavMesh.CalculatePath(transform.position, groupIntel.targetLastDetectedPosition, NavMesh.AllAreas, navMeshPath);
                }

                if (navMeshPath.corners.Length > 1)
                {
                    MoveTowardsTargetPosition(navMeshPath.corners[1]);
                }
            }
            else
            {
                targetThrottle = 0.0f;
                animationTurning = Mathf.Lerp(animationTurning, 0f, 0.334f * Time.deltaTime);

                CenterTorsoHorizontal();
                CenterTorsoVertical();
            }
        }
    }





    public override void TakeDamage(Collider hitCollider, Vector2 direction, float damage, WeaponController weaponController)
    {
        if (IsDestroyed)
        {
            return;
        }

        if (targetUnit == null)
        {
            groupIntel.targetLastDetectedPosition = weaponController.Owner.transform.position;
        }

        if (weaponController.Owner is MechControllerPlayer)
        {
            PlayerHUD.Instance.SetHitIndication();
        }

        base.TakeDamage(hitCollider, direction, damage, weaponController);
    }

    public override void TakeDirectSplashDamage(Collider hitCollider, Vector2 direction, float damage, WeaponController weaponController)
    {
        if (IsDestroyed)
        {
            return;
        }

        if (targetUnit == null)
        {
            groupIntel.targetLastDetectedPosition = weaponController.Owner.transform.position;
        }

        if (weaponController.Owner is MechControllerPlayer)
        {
            PlayerHUD.Instance.SetHitIndication();
        }

        base.TakeDirectSplashDamage(hitCollider, direction, damage, weaponController);
    }

    public override void TakeIndirectSplashDamage(Vector2 direction, float damage, WeaponController weaponController)
    {
        if (IsDestroyed)
        {
            return;
        }

        if (targetUnit == null)
        {
            groupIntel.targetLastDetectedPosition = weaponController.Owner.transform.position;
        }

        if (weaponController.Owner is MechControllerPlayer)
        {
            PlayerHUD.Instance.SetHitIndication();
        }

        base.TakeIndirectSplashDamage(direction, damage, weaponController);
    }

    protected override bool GetLineOfSightToTarget()
    {
        Vector3 startingPoint = mechMetaController.CockpitHardpoint.position + mechMetaController.CockpitHardpoint.forward;
        Vector3 direction = targetUnit.Bounds.center - startingPoint;

        if (Physics.Raycast(startingPoint, direction, out raycastHit, GetRadarDetectionRange(), aiVisionLayerMask, QueryTriggerInteraction.Ignore))
        {
            if (raycastHit.transform.root.gameObject == targetUnit.gameObject)
            {
                return true;
            }
        }

        return false;
    }





    public override void Die()
    {
        base.Die();

        MissionManager.Instance.UpdateObjectives();
    }

    public override void PlayFootStep()
    {
        base.PlayFootStep();
    }

    public override void FootStepLeft()
    {
        base.FootStepLeft();
    }

    public override void FootStepRight()
    {
        base.FootStepRight();
    }
}