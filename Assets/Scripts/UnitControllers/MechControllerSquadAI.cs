using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public sealed class MechControllerSquadAI : MechControllerAI
{
    int unitLayerMask;

    bool friendlyFireClear = true;

    float friendlyFireCheckTimer = 0.0f;

    protected override void Awake()
    {
        base.Awake();

        unitLayerMask = 1 << LayerMask.NameToLayer("Unit");
    }

    protected override void Update()
    {
        if (Time.timeScale == 0.0f || IsDestroyed)
        {
            return;
        }

        base.Update();

        UpdateSquadMateAI();

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

    void UpdateSquadMateAI()
    {
        Vector3 playerDirection = MechControllerPlayer.Instance.transform.position - transform.position;
        float distanceToPlayer = playerDirection.magnitude;

        if (distanceToPlayer > 200.0f)
        {
            SetSquadMateOrder(null);
        }

        if (Time.time > changeTargetTimer)
        {
            changeTargetTimer = Time.time + 1.5f;

            targetUnit = GetClosestEnemy();
        }

        if (currentOrderSquadAI == null)
        {
            UpdateSquadMateAIFollow(playerDirection, distanceToPlayer);
        }
        else
        {
            if (currentOrderSquadAI is OrderHoldPosition)
            {
                UpdateSquateMateAIHoldPosition();
            }
            else if (currentOrderSquadAI is OrderMoveToNavPoint)
            {
                UpdateSquadMateAIMoveToNavPoint();
            }
            else if (currentOrderSquadAI is OrderAttackTarget)
            {
                if ((currentOrderSquadAI as OrderAttackTarget).targetUnit.IsDestroyed)
                {
                    SetSquadMateOrder(null);
                    return;
                }

                UpdateSquadMateAIAttackTarget();
            }
            else if (currentOrderSquadAI is OrderClosestEnemy)
            {
                UpdateSquadMateAIAttackClosestTarget(playerDirection, distanceToPlayer);
            }
        }
    }

    void UpdateSquadMateAIFollow(Vector3 playerDirection, float distanceToPlayer)
    {
        if (Time.time > repathTimer)
        {
            NavMeshHit navMeshHit;

            if (distanceToPlayer > 20.0f)
            {
                repathTimer = Time.time + 3.0f;

                NavMesh.CalculatePath(transform.position, MechControllerPlayer.Instance.transform.position, NavMesh.AllAreas, navMeshPath);
            }
            else if (distanceToPlayer > 15.0f)
            {
                Vector3 randomPosition = MechControllerPlayer.Instance.transform.position + MechControllerPlayer.Instance.transform.TransformDirection(Random.onUnitSphere * 10f);

                if (NavMesh.SamplePosition(randomPosition, out navMeshHit, 15.0f, NavMesh.AllAreas))
                {
                    repathTimer = Time.time + 2.0f;

                    NavMesh.CalculatePath(transform.position, navMeshHit.position, NavMesh.AllAreas, navMeshPath);
                }
                else
                {
                    repathTimer = Time.time + 0.25f;

                    navMeshPath.ClearCorners();
                }
            }
            else if (targetUnit != null)
            {
                Vector3 randomPosition = targetUnit.transform.position;
                randomPosition += Random.insideUnitSphere * 10f;

                if (NavMesh.SamplePosition(randomPosition, out navMeshHit, 15.0f, NavMesh.AllAreas))
                {
                    repathTimer = Time.time + Random.Range(1.5f, 4.0f);

                    NavMesh.CalculatePath(transform.position, navMeshHit.position, NavMesh.AllAreas, navMeshPath);
                }
                else
                {
                    repathTimer = Time.time + 0.5f;

                    navMeshPath.ClearCorners();
                }
            }
            else
            {
                repathTimer = Time.time + 1.0f;

                navMeshPath.ClearCorners();
            }
        }

        if (navMeshPath.corners.Length > 1)
        {
            MoveTowardsTargetPosition(navMeshPath.corners[1]);
        }

        if (targetUnit != null)
        {
            Vector3 targetPosition = targetUnit.TargetablePosition + aiTargetingOffset;
            Vector3 targetDirection = targetPosition - mechMetaController.CockpitHardpoint.position;
            float targetDistance = targetDirection.magnitude;

            RotateTorsoTowardsTarget(targetDirection);

            if (navMeshPath.corners.Length < 2)
            {
                TurnTowardsDirection(targetDirection);
            }

            FireAtTarget(targetDirection, targetDistance);

            if (currentLockedOnTarget != null && Time.time > lockedOnTimer)
            {
                currentLockedOnTarget = null;
            }
        }
        else
        {
            if (navMeshPath.corners.Length < 2)
            {
                if (distanceToPlayer > 15.0f)
                {
                    MoveTowardsTargetPosition(MechControllerPlayer.Instance.transform.position);
                }
                else
                {
                    TurnTowardsDirection(playerDirection);
                }
            }

            CenterTorsoHorizontal();
            CenterTorsoVertical();
        }
    }

    void UpdateSquateMateAIHoldPosition()
    {
        if (targetUnit != null)
        {
            Vector3 targetPosition = targetUnit.TargetablePosition + aiTargetingOffset;
            Vector3 targetDirection = targetPosition - mechMetaController.CockpitHardpoint.position;
            float targetDistance = targetDirection.magnitude;

            RotateTorsoTowardsTarget(targetDirection);

            TurnTowardsDirection(targetDirection);

            FireAtTarget(targetDirection, targetDistance);

            if (currentLockedOnTarget != null && Time.time > lockedOnTimer)
            {
                currentLockedOnTarget = null;
            }
        }
        else
        {
            animationTurning = Mathf.Lerp(animationTurning, 0f, 0.334f * Time.deltaTime);

            CenterTorsoHorizontal();
            CenterTorsoVertical();
        }
    }

    void UpdateSquadMateAIMoveToNavPoint()
    {
        float distanceToNavPoint = ((currentOrderSquadAI as OrderMoveToNavPoint).navigationPoint.transform.position - transform.position).magnitude;

        if (distanceToNavPoint < 5.0f)
        {
            SetSquadMateOrder(new OrderHoldPosition());
            return;
        }

        if (Time.time > repathTimer)
        {
            if (NavMesh.SamplePosition((currentOrderSquadAI as OrderMoveToNavPoint).navigationPoint.transform.position, out NavMeshHit navMeshHit, 15.0f, NavMesh.AllAreas))
            {
                repathTimer = Time.time + 2.0f;

                NavMesh.CalculatePath(transform.position, navMeshHit.position, NavMesh.AllAreas, navMeshPath);
            }
            else
            {
                repathTimer = Time.time + 0.25f;

                navMeshPath.ClearCorners();
            }
        }

        if (navMeshPath.corners.Length > 1)
        {
            MoveTowardsTargetPosition(navMeshPath.corners[1]);
        }
        else
        {
            MoveTowardsTargetPosition((currentOrderSquadAI as OrderMoveToNavPoint).navigationPoint.transform.position);
        }

        if (targetUnit != null)
        {
            Vector3 targetPosition = targetUnit.TargetablePosition + aiTargetingOffset;
            Vector3 targetDirection = targetPosition - mechMetaController.CockpitHardpoint.position;
            float targetDistance = targetDirection.magnitude;

            RotateTorsoTowardsTarget(targetDirection);

            FireAtTarget(targetDirection, targetDistance);

            if (currentLockedOnTarget != null && Time.time > lockedOnTimer)
            {
                currentLockedOnTarget = null;
            }
        }
        else
        {
            CenterTorsoHorizontal();
            CenterTorsoVertical();
        }
    }

    void UpdateSquadMateAIAttackTarget()
    {
        if (Time.time > repathTimer)
        {
            Vector3 randomPosition = (currentOrderSquadAI as OrderAttackTarget).targetUnit.transform.position;
            randomPosition += Random.insideUnitSphere * 10f;

            if (NavMesh.SamplePosition(randomPosition, out NavMeshHit navMeshHit, 15.0f, NavMesh.AllAreas))
            {
                repathTimer = Time.time + Random.Range(1.5f, 4.0f);

                NavMesh.CalculatePath(transform.position, navMeshHit.position, NavMesh.AllAreas, navMeshPath);
            }
            else
            {
                repathTimer = Time.time + 0.5f;

                navMeshPath.ClearCorners();
            }
        }

        if (navMeshPath.corners.Length > 1)
        {
            MoveTowardsTargetPosition(navMeshPath.corners[1]);
        }

        Vector3 targetPosition = (currentOrderSquadAI as OrderAttackTarget).targetUnit.TargetablePosition + aiTargetingOffset;
        Vector3 targetDirection = targetPosition - mechMetaController.CockpitHardpoint.position;
        float targetDistance = targetDirection.magnitude;

        RotateTorsoTowardsTarget(targetDirection);

        if (navMeshPath.corners.Length < 2)
        {
            if (targetDistance > 20.0f)
            {
                MoveTowardsTargetPosition((currentOrderSquadAI as OrderAttackTarget).targetUnit.transform.position);
            }
            else
            {
                TurnTowardsDirection(targetDirection);
            }
        }

        FireAtTarget(targetDirection, targetDistance);

        if (currentLockedOnTarget != null && Time.time > lockedOnTimer)
        {
            currentLockedOnTarget = null;
        }
    }

    void UpdateSquadMateAIAttackClosestTarget(Vector3 playerDirection, float distanceToPlayer)
    {
        if (Time.time > repathTimer)
        {
            NavMeshHit navMeshHit;

            if (targetUnit != null)
            {
                Vector3 randomPosition = targetUnit.transform.position;
                randomPosition += Random.insideUnitSphere * 10f;

                if (NavMesh.SamplePosition(randomPosition, out navMeshHit, 15.0f, NavMesh.AllAreas))
                {
                    repathTimer = Time.time + Random.Range(1.5f, 4.0f);

                    NavMesh.CalculatePath(transform.position, navMeshHit.position, NavMesh.AllAreas, navMeshPath);
                }
                else
                {
                    repathTimer = Time.time + 0.5f;

                    navMeshPath.ClearCorners();
                }
            }
            else if (distanceToPlayer > 20.0f)
            {
                repathTimer = Time.time + 3.0f;

                NavMesh.CalculatePath(transform.position, MechControllerPlayer.Instance.transform.position, NavMesh.AllAreas, navMeshPath);
            }
            else if (distanceToPlayer > 15.0f)
            {
                Vector3 randomPosition = MechControllerPlayer.Instance.transform.position + MechControllerPlayer.Instance.transform.TransformDirection(Random.onUnitSphere * 10f);

                if (NavMesh.SamplePosition(randomPosition, out navMeshHit, 15.0f, NavMesh.AllAreas))
                {
                    repathTimer = Time.time + 2.0f;

                    NavMesh.CalculatePath(transform.position, navMeshHit.position, NavMesh.AllAreas, navMeshPath);
                }
                else
                {
                    repathTimer = Time.time + 0.25f;

                    navMeshPath.ClearCorners();
                }
            }
            else
            {
                repathTimer = Time.time + 1.0f;

                navMeshPath.ClearCorners();
            }

        }

        if (navMeshPath.corners.Length > 1)
        {
            MoveTowardsTargetPosition(navMeshPath.corners[1]);
        }

        if (targetUnit != null)
        {
            Vector3 targetPosition = targetUnit.TargetablePosition + aiTargetingOffset;
            Vector3 targetDirection = targetPosition - mechMetaController.CockpitHardpoint.position;
            float targetDistance = targetDirection.magnitude;

            RotateTorsoTowardsTarget(targetDirection);

            if (navMeshPath.corners.Length < 2)
            {
                if (targetDistance > 15.0f)
                {
                    MoveTowardsTargetPosition(targetUnit.transform.position);
                }
                else
                {
                    TurnTowardsDirection(targetDirection);
                }
            }

            FireAtTarget(targetDirection, targetDistance);

            if (currentLockedOnTarget != null && Time.time > lockedOnTimer)
            {
                currentLockedOnTarget = null;
            }
        }
        else
        {
            if (navMeshPath.corners.Length < 2)
            {
                if (distanceToPlayer > 20.0f)
                {
                    MoveTowardsTargetPosition(MechControllerPlayer.Instance.transform.position);
                }
                else
                {
                    TurnTowardsDirection(playerDirection);
                }
            }

            CenterTorsoHorizontal();
            CenterTorsoVertical();
        }
    }

    protected override void FireAtTarget(Vector3 targetDirection, float targetDistance)
    {
        if (!clearLineOfSightToTarget)
        {
            lockingOnValue = 0.0f;
            return;
        }

        float targetAngle = Vector3.Angle(mechMetaController.CockpitHardpoint.forward, targetDirection);

        if (targetAngle < 15.0f)
        {
            if (Time.time > friendlyFireCheckTimer)
            {
                friendlyFireClear = GetFriendlyFireCheck();

                if (friendlyFireClear)
                {
                    friendlyFireCheckTimer = Time.time + 0.4f;
                }
                else
                {
                    friendlyFireCheckTimer = Time.time + 1.0f;
                }
            }

            if (!friendlyFireClear)
            {
                return;
            }

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

                for (int i = 0; i < weaponControllersAll.Length; i++)
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

    public override void TakeDamage(Collider hitCollider, Vector2 direction, float damage, WeaponController weaponController)
    {
        if (IsDestroyed)
        {
            return;
        }

        if (weaponController.Owner is MechControllerPlayer)
        {
            PlayerHUD.Instance.SetHitIndication();

            if (aI_Type == AI_Type.PlayerSquad && MechControllerPlayer.Instance.CommandSystem.CanAddFriendlyFireWarning)
            {
                MechControllerPlayer.Instance.CommandSystem.AddFriendFireWarning(mechData.currentMechPilot.PilotVoiceProfile.GetFriendlyFireWarning());
            }
        }

        base.TakeDamage(hitCollider, direction, damage, weaponController);
    }

    public override void TakeDirectSplashDamage(Collider hitCollider, Vector2 direction, float damage, WeaponController weaponController)
    {
        if (IsDestroyed)
        {
            return;
        }

        if (weaponController.Owner is MechControllerPlayer)
        {
            PlayerHUD.Instance.SetHitIndication();

            if (aI_Type == AI_Type.PlayerSquad && MechControllerPlayer.Instance.CommandSystem.CanAddFriendlyFireWarning)
            {
                MechControllerPlayer.Instance.CommandSystem.AddFriendFireWarning(mechData.currentMechPilot.PilotVoiceProfile.GetFriendlyFireWarning());
            }
        }

        base.TakeDirectSplashDamage(hitCollider, direction, damage, weaponController);
    }

    public override void TakeIndirectSplashDamage(Vector2 direction, float damage, WeaponController weaponController)
    {
        if (IsDestroyed)
        {
            return;
        }

        if (weaponController.Owner is MechControllerPlayer)
        {
            PlayerHUD.Instance.SetHitIndication();

            if (aI_Type == AI_Type.PlayerSquad && MechControllerPlayer.Instance.CommandSystem.CanAddFriendlyFireWarning)
            {
                MechControllerPlayer.Instance.CommandSystem.AddFriendFireWarning(mechData.currentMechPilot.PilotVoiceProfile.GetFriendlyFireWarning());
            }
        }

        base.TakeIndirectSplashDamage(direction, damage, weaponController);
    }

    bool GetFriendlyFireCheck()
    {
        Vector3 startingPoint = mechMetaController.CockpitHardpoint.position;
        float distance = Mathf.Clamp((targetUnit.Bounds.center - startingPoint).magnitude, 20.0f, 100.0f);

        float diameter;

        switch (mechData.MechChassis.UnitClass)
        {
            case UnitClass.MechAssault:
                {
                    diameter = 4.0f;
                    break;
                }
            case UnitClass.MechHeavy:
                {
                    diameter = 3.5f;
                    break;
                }
            default:
                {
                    diameter = 3.0f;
                    break;
                }
        }

        if (Physics.SphereCast(startingPoint, diameter, mechMetaController.CockpitHardpoint.forward, out raycastHit, distance, unitLayerMask, QueryTriggerInteraction.Ignore))
        {
            UnitController unitController = raycastHit.transform.root.gameObject.GetComponent<UnitController>();

            if (unitController != null && unitController.Team != TeamType.Enemy)
            {
                return false;
            }
        }

        return true;
    }

    public override void Die()
    {
        base.Die();

        MissionManager.Instance.UpdateSquadMateDisplay();

        MechControllerPlayer.Instance.CommandSystem.UpdateInstructions();

        AudioClip audioClip = mechData.currentMechPilot.PilotVoiceProfile.GetEjection();

        if (audioClip != null)
        {
            MechControllerPlayer.Instance.CommandSystem.AudioClipsRadio.Enqueue(audioClip);
        }
    }

    public override void GetKill(UnitController killTarget)
    {
        if (killTarget.Team != TeamType.Enemy)
            return;

        mechData.GetKill(killTarget.UnitData);

        AudioClip audioClip = mechData.currentMechPilot.PilotVoiceProfile.GetEnemyDestroyed();

        if (audioClip != null)
        {
            MechControllerPlayer.Instance.CommandSystem.AudioClipsRadio.Enqueue(audioClip);
        }
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
