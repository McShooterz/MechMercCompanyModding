using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class CombatUnitData : UnitData
{
    protected float lockOnBonus = 0f;

    public float LockOnBonus { get => lockOnBonus; }

    protected AmmoPool[] BuildAmmoPools(ComponentData[] componentDatas)
    {
        List<AmmoPool> ammoPoolList = new List<AmmoPool>();
        Dictionary<string, List<ComponentData>> ammoPoolDict = new Dictionary<string, List<ComponentData>>();

        for (int i = 0; i < componentDatas.Length; i++)
        {
            ComponentData componentData = componentDatas[i];

            if (componentData.ComponentDefinition.AmmoType != "" && !componentData.ComponentDefinition.AmmoInternal)
            {
                if (!ammoPoolDict.ContainsKey(componentData.ComponentDefinition.AmmoType))
                {
                    ammoPoolDict.Add(componentData.ComponentDefinition.AmmoType, new List<ComponentData>());
                }

                ammoPoolDict[componentData.ComponentDefinition.AmmoType].Add(componentData);
            }
        }

        List<List<ComponentData>> componentDatasAmmoList = ammoPoolDict.Values.ToList();

        for (int i = 0; i < componentDatasAmmoList.Count; i++)
        {
            ammoPoolList.Add(new AmmoPool(componentDatasAmmoList[i].ToArray()));
        }

        return ammoPoolList.ToArray();
    }
}
