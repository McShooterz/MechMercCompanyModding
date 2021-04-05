using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomUnitsSetup : MonoBehaviour
{
    [SerializeField]
    UnitSetupControllerBase[] unitSetups = new UnitSetupControllerBase[0];

    public void BuildUnits()
    {
        for (int i = 0; i < unitSetups.Length; i++)
        {
            unitSetups[i].BuildUnit();
        }
    }

#if UNITY_EDITOR
    public void AutoFillUnitSetups()
    {
        unitSetups = FindObjectsOfType<UnitSetupControllerBase>();
    }
#endif
}
