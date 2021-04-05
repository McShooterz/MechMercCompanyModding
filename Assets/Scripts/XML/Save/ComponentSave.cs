using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ComponentSave
{
    public string ComponentDefinition = "";

    public WeaponGrouping WeaponGrouping;

    public int GroupIndex;

    public bool IsDestroyed;

    public ComponentDefinition GetComponentDefinition()
    {
        return ResourceManager.Instance.GetComponentDefinition(ComponentDefinition);
    }

    public static ComponentSave[] GetComponentSaves(ComponentData[] componentDatas)
    {
        ComponentSave[] componentSaves = new ComponentSave[componentDatas.Length];

        for (int i = 0; i < componentDatas.Length; i++)
        {
            componentSaves[i] = componentDatas[i].ComponentSave;
        }

        return componentSaves;
    }
}
