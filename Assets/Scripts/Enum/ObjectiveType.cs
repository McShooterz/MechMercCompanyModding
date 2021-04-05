using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ObjectiveType
{
    None,
    DestroyAllUnits,
    DestroyCountUnits,
    ProtectAllUnits,
    ProtectAnyUnits,
    ProtectCountUnits,
    MoveToNavPoint,
    ConvoyReachEnd,
    PreventConvoyReachEnd,
    BreakContactWithEnemy,
    AvoidEnemyDetection,
    RemainInRangeOfNavPoint,
}
