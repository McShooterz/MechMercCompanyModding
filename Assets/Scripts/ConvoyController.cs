using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConvoyController
{
    public int wayPointIndex = 0;

    public bool convoyReachedEnd = false;

    public List<GroundVehicleController> ConvoyUnits { get; private set; }

    public ConvoyController(List<GroundVehicleController> groundVehicles)
    {
        ConvoyUnits = groundVehicles;

        AssignIndices();
    }

    public void RemoveVehicle(GroundVehicleController vehicle)
    {
        ConvoyUnits.Remove(vehicle);

        AssignIndices();
    }

    void AssignIndices()
    {
        for (int i = 0; i < ConvoyUnits.Count; i++)
        {
            ConvoyUnits[i].convoyIndex = i;
        }
    }
}