using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public sealed class OrderAttackTarget : OrderBase
{
    public UnitController targetUnit;

    public OrderAttackTarget(UnitController unitController)
    {
        targetUnit = unitController;
    }
}
