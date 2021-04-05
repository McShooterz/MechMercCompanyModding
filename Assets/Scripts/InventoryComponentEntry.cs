using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InventoryComponentEntry
{
    public ComponentDefinition componentDefinition;

    public int Count = 0;

    public InventoryComponentEntry()
    {

    }

    public InventoryComponentEntry (ComponentDefinition definition, int startingCount)
    {
        componentDefinition = definition;
        Count = startingCount;
    }
}
