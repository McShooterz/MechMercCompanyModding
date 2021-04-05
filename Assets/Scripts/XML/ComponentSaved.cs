using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComponentSaved
{
    public string ComponentDefinition;

    public WeaponGrouping WeaponGrouping;

    public ComponentDefinition GetComponentDefinition()
    {
        return ResourceManager.Instance.GetComponentDefinition(ComponentDefinition);
    }
}
