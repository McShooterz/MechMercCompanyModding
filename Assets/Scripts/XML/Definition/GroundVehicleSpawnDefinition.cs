using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class GroundVehicleSpawnDefinition : SpawnDefinition
{
    public string Definition = "";

    public GroundVehicleDefinition GetGroundVehicleDefinition()
    {
        return ResourceManager.Instance.GetGroundVehicleDefinition(Definition);
    }
}
