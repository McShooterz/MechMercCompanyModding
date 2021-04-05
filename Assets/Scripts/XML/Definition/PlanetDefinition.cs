using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetDefinition : Definition
{
    public string DisplayName = "";
    public string Description = "";

    public string Prefab = "";

    public float Scale = 1f;

    public BiomeType[] Biomes = new BiomeType[0];

    public string[] MissionMaps = new string[0];

    public string GetDisplayName()
    {
        return ResourceManager.Instance.GetLocalization(DisplayName);
    }

    public string GetDescription()
    {
        return ResourceManager.Instance.GetLocalization(Description);
    }

    public GameObject GetPrefab()
    {
        return ResourceManager.Instance.GetPlanetPrefab(Prefab);
    }

    public List<MapDefinition> GetMaps()
    {
        List<MapDefinition> mapDefinitions = new List<MapDefinition>();
        List<MapDefinition> biomeMaps = ResourceManager.Instance.GetMapDefinitions(Biomes);

        foreach (string mapKey in MissionMaps)
        {
            MapDefinition mapDefinition = ResourceManager.Instance.GetMapDefinition(mapKey);

            if (mapDefinition != null && !biomeMaps.Contains(mapDefinition))
            {
                mapDefinitions.Add(mapDefinition);
            }
        }

        mapDefinitions.AddRange(biomeMaps);

        return mapDefinitions;
    }
}
