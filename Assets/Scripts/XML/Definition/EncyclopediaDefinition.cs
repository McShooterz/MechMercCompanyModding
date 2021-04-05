using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EncyclopediaDefinition : Definition
{
    public string DisplayName = "";

    public string Description = "";

    public string Image = "";

    public string MechPrefab = "";

    public string GroundVehiclePrefab = "";

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
        if (MechPrefab != "")
        {
            GameObject mechPrefab = ResourceManager.Instance.GetMechPrefab(MechPrefab);

            if ((object)mechPrefab != null)
            {
                return mechPrefab;
            }
        }
        
        if (GroundVehiclePrefab != "")
        {
            GameObject groundVehiclePrefab = ResourceManager.Instance.GetGroundVehiclePrefab(GroundVehiclePrefab);

            if ((object)groundVehiclePrefab != null)
            {
                return groundVehiclePrefab;
            }
        }

        return null;
    }
}
