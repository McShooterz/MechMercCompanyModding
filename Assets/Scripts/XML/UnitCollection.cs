using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitCollection
{
    public MechSpawnDefinition[] MechSpawnDefinitions = new MechSpawnDefinition[0];

    public MechSpawnDefinition GetRandomMechSpawnDefinition()
    {
        if (MechSpawnDefinitions.Length > 0)
        {
            return MechSpawnDefinitions[Random.Range(0, MechSpawnDefinitions.Length)];
        }

        return null;
    }
}
