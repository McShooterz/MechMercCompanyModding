using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public sealed class OrderMoveToNavPoint : OrderBase
{
    public Transform navigationPoint;

    public OrderMoveToNavPoint(Transform navPoint)
    {
        navigationPoint = navPoint;
    }
}
