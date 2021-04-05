using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingSpawnDefinition : SpawnDefinition
{
    public string Definition = "";

    public BuildingDefinition GetBuildingDefinition()
    {
        return ResourceManager.Instance.GetBuildingDefinition(Definition);
    }
}
