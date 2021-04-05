using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionEventSetUnitsLastEnemyPosition : MissionEventBase
{
    [SerializeField]
    GameObject[] units = new GameObject[0];

    [SerializeField]
    Transform targetPosition;

    public override void Trigger()
    {
        if (wasTriggered)
        {
            return;
        }

        wasTriggered = true;

        foreach (GameObject unitObject in units)
        {
            CombatUnitController combatUnitController = unitObject.GetComponent<CombatUnitController>();

            if (combatUnitController != null)
            {
                combatUnitController.GroupIntel.targetLastDetectedPosition = targetPosition.position;
            }
        }
    }
}
