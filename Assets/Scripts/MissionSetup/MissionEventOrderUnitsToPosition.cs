using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionEventOrderUnitsToPosition : MissionEventBase
{
    [SerializeField]
    GameObject[] units = new GameObject[0];

    [SerializeField]
    Transform[] targetPositions;

    public override void Trigger()
    {
        if (wasTriggered)
        {
            return;
        }

        wasTriggered = true;

        foreach (GameObject unitObject in units)
        {
            MobileUnitController mobileUnitController = unitObject.GetComponent<MobileUnitController>();

            if (mobileUnitController != null)
            {
                foreach (Transform targetPosition in targetPositions)
                {
                    mobileUnitController.Orders.Enqueue(new OrderMoveToNavPoint(targetPosition));
                }
            }
        }
    }
}
