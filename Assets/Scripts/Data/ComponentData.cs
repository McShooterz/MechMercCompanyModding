using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ComponentData
{
    public int groupIndex;

    public bool isDestroyed;

    float health;

    public int ammoCount;

    public ComponentDefinition ComponentDefinition { get; private set; }

    public WeaponController WeaponController { get; set; }

    public EquipmentController EquipmentController { get; set; }

    public WeaponController ModifiedWeaponController { get; set; }

    public WeaponGrouping WeaponGrouping { get; set; }

    public float ExplosionDamage
    {
        get
        {
            float damage = ComponentDefinition.ExplosionDamage;

            if (AmmoCanExplode)
            {
                damage += ComponentDefinition.AmmoExplosionDamage * ammoCount;
            }

            return damage;
        }
    }

    public bool AmmoCanExplode { get => ammoCount > 0 && ComponentDefinition.AmmoExplosionDamage > 0.0f; }

    public ComponentSave ComponentSave
    {
        get
        {
            return new ComponentSave()
            {
                ComponentDefinition = ComponentDefinition.Key,
                WeaponGrouping = new WeaponGrouping(WeaponGrouping),
                GroupIndex = groupIndex,
                IsDestroyed = isDestroyed,
            };
        }
    }

    public ComponentData(ComponentDefinition definition, WeaponGrouping grouping, int i)
    {
        ComponentDefinition = definition;
        groupIndex = i;
        health = ComponentDefinition.Health;

        if (grouping != null)
        {
            WeaponGrouping = new WeaponGrouping(grouping);
        }
    }

    public ComponentData(ComponentSave componentSave)
    {
        ComponentDefinition = componentSave.GetComponentDefinition();

        if (ComponentDefinition != null)
        {
            WeaponGrouping = new WeaponGrouping(componentSave.WeaponGrouping);
            health = ComponentDefinition.Health;
            groupIndex = componentSave.GroupIndex;
            isDestroyed = componentSave.IsDestroyed;
        }
        else
        {
            Debug.LogError("Error: Component is Null");
        }
    }

    public bool TakeDamage(float damage)
    {
        if (damage >= health)
        {
            health = 0;
            return true;
        }
        else
        {
            health -= damage;
            return false;
        }
    }

    public void SetDestroyed()
    {
        if (!isDestroyed)
        {
            isDestroyed = true;

            if (WeaponController != null)
            {
                WeaponController.SetDestroyed();
            }

            if (EquipmentController != null)
            {
                EquipmentController.SetDestroyed();
            }

            if (ModifiedWeaponController != null && !ModifiedWeaponController.IsDestroyed)
            {
                ModifiedWeaponController.SetDefaultModifiers();
            }
        }
    }

    public void SetAmmo(int value)
    {
        ammoCount = value;
    }

    public void ReloadAmmo()
    {
        if (isDestroyed)
            return;

        ammoCount = ComponentDefinition.AmmoCount;
    }

    public void Repair()
    {
        isDestroyed = false;

        health = ComponentDefinition.Health;
    }

    public bool GetExplosionRole()
    {
        if (WeaponController == null || WeaponController.IsCharged)
        {
            return ComponentDefinition.GetExplosionRole();
        }

        return false;
    }

    public static ComponentData[] BuildComponentDatas(string[] componentKeys)
    {
        List<ComponentData> componentDatasList = new List<ComponentData>();

        foreach (string componentKey in componentKeys)
        {
            ComponentDefinition componentDefinition = ResourceManager.Instance.GetComponentDefinition(componentKey);

            if (componentDefinition != null)
            {
                componentDatasList.Add(new ComponentData(componentDefinition, new WeaponGrouping(), 0));
            }
        }

        return componentDatasList.ToArray();
    }

    public static ComponentData[] BuildComponentDatas(ComponentSave[] componentSaves)
    {
        List<ComponentData> componentDatasList = new List<ComponentData>();

        for (int componentIndex = 0; componentIndex < componentSaves.Length; componentIndex++)
        {
            ComponentSave componentSave = componentSaves[componentIndex];

            ComponentDefinition componentDefinition = componentSave.GetComponentDefinition();

            if (componentDefinition != null)
            {
                componentDatasList.Add(new ComponentData(componentSave));
            }
        }

        return componentDatasList.ToArray();
    }

    public static ComponentData[] BuildComponentDatas(ComponentGroup[] componentGroups)
    {
        List<ComponentData> componentDatasList = new List<ComponentData>();

        for (int i = 0; i < componentGroups.Length; i++)
        {
            foreach (ComponentSaved componentSaved in componentGroups[i].ComponentsSaved)
            {
                ComponentDefinition componentDefinition = componentSaved.GetComponentDefinition();

                if (componentDefinition != null)
                {
                    ComponentData componentData = new ComponentData(componentDefinition, componentSaved.WeaponGrouping, i);
                    componentDatasList.Add(componentData);
                }
            }
        }

        return componentDatasList.ToArray();
    }
}
