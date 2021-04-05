using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ComponentDefinitionArray
{
    public string[] ComponentDefinitions = new string[0];

    public List<ComponentDefinition> GetComponentDefinitions()
    {
        List<ComponentDefinition> componentDefinitions = new List<ComponentDefinition>();

        for (int i = 0; i < ComponentDefinitions.Length; i++)
        {
            ComponentDefinition componentDefinition = ResourceManager.Instance.GetComponentDefinition(ComponentDefinitions[i]);

            if (componentDefinition != null)
            {
                componentDefinitions.Add(componentDefinition);
            }
        }

        return componentDefinitions;
    }
}
