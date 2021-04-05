using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretSpawnDefinition : SpawnDefinition
{
    public string Definition = "";

    public TurretDefinition GetTurretDefinition()
    {
        return ResourceManager.Instance.GetTurretDefinition(Definition);
    }
}
