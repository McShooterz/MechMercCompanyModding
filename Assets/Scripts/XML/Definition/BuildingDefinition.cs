using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingDefinition : Definition
{
    public string Prefab = "";

	public string DisplayName = "";
	

	public float Health = 5.0f;

    public GameObject GetPrefab()
    {
        return ResourceManager.Instance.GetBuildingPrefab(Prefab);
    }

    public string GetDisplayName()
    {
        return ResourceManager.Instance.GetLocalization(DisplayName);
    }
}
