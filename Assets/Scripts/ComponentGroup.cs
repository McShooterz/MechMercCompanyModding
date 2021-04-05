using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComponentGroup
{
    public ComponentSaved[] ComponentsSaved = new ComponentSaved[0];

    public List<ComponentDefinition> GetComponents()
    {
        List<ComponentDefinition> componentDefinitions = new List<ComponentDefinition>();

        foreach (ComponentSaved componentSaved in ComponentsSaved)
        {
            ComponentDefinition componentDefinition = componentSaved.GetComponentDefinition();

            if (componentDefinition != null)
            {
                componentDefinitions.Add(componentDefinition);
            }
        }

        return componentDefinitions;
    }
}
