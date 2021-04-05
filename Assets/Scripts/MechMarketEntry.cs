using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MechMarketEntry
{
    public int count;

    public MechDesign MechDesign { get; set; }

    public MechMarketSave MechMarketSave
    {
        get
        {
            return new MechMarketSave()
            {
                MechChassisDefinition = MechDesign.MechChassisDefinition,
                MechDesign = MechDesign.Key,
                Count = count,
            };
        }
    }

    public MechMarketEntry() { }

    public MechMarketEntry(MechMarketSave mechMarketSave)
    {
        MechDesign = mechMarketSave.GetMechDesign();
        count = mechMarketSave.Count;
    }
}
