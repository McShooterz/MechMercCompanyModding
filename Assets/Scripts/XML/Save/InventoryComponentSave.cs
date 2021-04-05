using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryComponentSave
{
    public string ComponentDefinition = "";
    public int Count = 0;

    public InventoryComponentSave() { }

    public InventoryComponentSave(string componentDefintion, int count)
    {
        ComponentDefinition = componentDefintion;
        Count = count;
    }

    public ComponentDefinition GetComponentDefinition()
    {
        return ResourceManager.Instance.GetComponentDefinition(ComponentDefinition);
    }
}
