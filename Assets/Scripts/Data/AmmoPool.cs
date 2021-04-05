using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoPool
{
    [SerializeField]
    ComponentData[] componentDatas = new ComponentData[0];

    [SerializeField]
    string ammoType = "";

    public AmmoDefinition AmmoDefinition { get; private set; }

    public string AmmoType
    {
        get
        {
            return ammoType;
        }
    }

    public int AmmoCount
    {
        get
        {
            int ammoCount = 0;

            for (int i = 0; i < componentDatas.Length; i++)
            {
                ammoCount += componentDatas[i].ammoCount;
            }

            return ammoCount;
        }
    }

    public int AmmoMax
    {
        get
        {
            int ammoMax = 0;

            for (int i = 0; i < componentDatas.Length; i++)
            {
                ComponentData componentData = componentDatas[i];

                if (!componentData.isDestroyed)
                {
                    ammoMax += componentData.ComponentDefinition.AmmoCount;
                }
            }

            return ammoMax;
        }
    }

    public bool HasAmmo
    {
        get
        {
            for (int i = 0; i < componentDatas.Length; i++)
            {
                if (componentDatas[i].ammoCount > 0)
                    return true;
            }

            return false;
        }
    }

    public AmmoPool(ComponentData[] components)
    {
        if (components.Length > 0)
        {
            ammoType = components[0].ComponentDefinition.AmmoType;
            componentDatas = components;

            AmmoDefinition = ResourceManager.Instance.GetAmmoDefinition(ammoType);
        }
    }

    public int TakeAmmo(int amountTarget)
    {
        int amountCurrent = 0;

        for (int i = 0; i < componentDatas.Length; i++)
        {
            ComponentData componentData = componentDatas[i];

            if (!componentData.isDestroyed && componentData.ammoCount > 0)
            {
                if (amountTarget > amountCurrent + componentData.ammoCount)
                {
                    amountCurrent += componentData.ammoCount;
                    componentData.ammoCount = 0;
                }
                else
                {
                    componentData.ammoCount -= (amountTarget - amountCurrent);
                    return amountTarget;
                }
            }
        }

        return amountCurrent;
    }

    public void FullReload()
    {
        for (int i = 0; i < componentDatas.Length; i++)
        {
            componentDatas[i].ReloadAmmo();
        }
    }
}
