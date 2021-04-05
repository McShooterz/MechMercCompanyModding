using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Inventory
{
    [SerializeField]
    bool componentsChanged = false;

    public Dictionary<ComponentDefinition, int> Components { get; private set; } = new Dictionary<ComponentDefinition, int>();

    public bool ComponentsChanged
    {
        get
        {
            if (componentsChanged)
            {
                componentsChanged = false;
                return true;
            }

            return false;
        }
    }

    public int InventoryValue
    {
        get
        {
            int sum = 0;

            foreach (KeyValuePair<ComponentDefinition, int> keyValuePair in Components)
            {
                sum += keyValuePair.Key.MarketValue * keyValuePair.Value;
            }

            return sum;
        }
    }

    public InventorySave InventorySave
    {
        get
        {
            List<InventoryComponentSave> inventoryComponentSaves = new List<InventoryComponentSave>();

            foreach (KeyValuePair<ComponentDefinition, int> keyValuePair in Components)
            {
                if (keyValuePair.Value > 0)
                    inventoryComponentSaves.Add(new InventoryComponentSave(keyValuePair.Key.Key, keyValuePair.Value));
            }

            return new InventorySave
            {
                InventoryComponentSaves = inventoryComponentSaves.ToArray(),
            };
        }
    }

    public Inventory() { }

    public Inventory(Inventory inventory) { Duplicate(inventory); }

    public Inventory(InventorySave inventorySave)
    {
        foreach (InventoryComponentSave inventoryComponentSave in inventorySave.InventoryComponentSaves)
        {
            AddComponent(inventoryComponentSave.GetComponentDefinition(), inventoryComponentSave.Count);
        }
    }

    public void Duplicate(Inventory inventory)
    {
        Components = new Dictionary<ComponentDefinition, int>(inventory.Components);
    }

    public void Clear()
    {
        Components.Clear();
    }

    public List<KeyValuePair<ComponentDefinition, int>> GetComponents(ComponentType componentType)
    {
        List<KeyValuePair<ComponentDefinition, int>> componentList = new List<KeyValuePair<ComponentDefinition, int>>();

        foreach (KeyValuePair<ComponentDefinition, int> keyValuePair in Components)
        {
            if (componentType == keyValuePair.Key.ComponentType)
            {
                componentList.Add(keyValuePair);
            }
        }

        return componentList;
    }

    public int GetComponentCount(ComponentDefinition componentDefinition)
    {
        if (Components.TryGetValue(componentDefinition, out int count))
        {
            return count;
        }

        return 0;
    }

    public void AddComponent(ComponentDefinition componentDefinition)
    {
        if (componentDefinition == null)
            return;

        if (Components.ContainsKey(componentDefinition))
        {
            Components[componentDefinition]++;
        }
        else
        {
            Components.Add(componentDefinition, 1);
            componentsChanged = true;
        }
    }

    public void AddComponent(ComponentDefinition componentDefinition, int count)
    {
        if (componentDefinition == null)
            return;

        if (Components.ContainsKey(componentDefinition))
        {
            Components[componentDefinition] += count;
        }
        else
        {
            Components.Add(componentDefinition, count);
            componentsChanged = true;
        }
    }

    public bool TakeComponent(ComponentDefinition componentDefinition)
    {
        if (Components.TryGetValue(componentDefinition, out int count))
        {
            if (count > 0)
            {
                count -= 1;

                if (count == 0)
                {
                    Components.Remove(componentDefinition);
                    componentsChanged = true;
                }
                else
                {
                    Components[componentDefinition] = count;
                }

                return true;
            }
            else
            {
                Components.Remove(componentDefinition);
                componentsChanged = true;
            }
        }

        return false;
    }

    public static Inventory GetInstantActionInventory()
    {
        Inventory instantActionInventory = new Inventory();

        foreach (ComponentDefinition componentDefinition in ResourceManager.Instance.GetComponentDefinitions())
        {
            instantActionInventory.AddComponent(componentDefinition, 100);
        }

        return instantActionInventory;
    }
}
