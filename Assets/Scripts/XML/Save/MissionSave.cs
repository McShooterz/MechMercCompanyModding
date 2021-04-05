using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MissionSave
{
    public MissionType MissionType;

    public string MapDefinition = "";

    public bool Successful = false;

    public ObjectiveSave[] Objectives = new ObjectiveSave[0];

    public MissionSave() { }

    public MapDefinition GetMapDefinition()
    {
        return ResourceManager.Instance.GetMapDefinition(MapDefinition);
    }


}
