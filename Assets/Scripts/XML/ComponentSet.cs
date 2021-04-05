using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComponentSet
{
    public string DisplayName = "";

    public ComponentType ComponentType = ComponentType.Equipment;

    public Color Color = Color.black;

    public Color TextColor = Color.white;

    public string[] Components = new string[0];

    public List<ComponentDefinition> ComponentDefinitions
    {
        get
        {
            List<ComponentDefinition> componentDefinitions = new List<ComponentDefinition>();

            foreach (string componentKey in Components)
            {
                ComponentDefinition componentDefinition = ResourceManager.Instance.GetComponentDefinition(componentKey);

                if (componentDefinition != null)
                {
                    componentDefinitions.Add(componentDefinition);
                }
            }

            return componentDefinitions;
        }
    }

    public string GetDisplayName()
    {
        return ResourceManager.Instance.GetLocalization(DisplayName);
    }
}
