using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponModelController : AccessoryModelController
{
    [SerializeField]
    Transform[] firingPoints;

    [SerializeField]
    int firePointIndex = 0;

    public Transform PrimaryFiringPoint
    {
        get
        {
            if (firingPoints.Length > 0)
            {
                return firingPoints[0].transform;
            }

            return transform;
        }
    }

    public Transform NextFiringPoint
    {
        get
        {
            if (firingPoints.Length > 0)
            {
                firePointIndex++;

                if (firePointIndex >= firingPoints.Length)
                {
                    firePointIndex = 0;
                }

                return firingPoints[firePointIndex].transform;
            }
            else
            {
                return transform;
            }
        }
    }

    public Transform GetFiringPoint(int index)
    {
        if (firingPoints.Length > 0)
        {
            return firingPoints[Mathf.Clamp(index, 0, firingPoints.Length - 1)];
        }

        return transform;
    }
}
