using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MechMarketSave
{
    public string MechChassisDefinition = "";

    public string MechDesign = "";

    public int Count;

    public MechDesign GetMechDesign()
    {
        return ResourceManager.Instance.GetMechDesign(MechChassisDefinition, MechDesign);
    }
}
