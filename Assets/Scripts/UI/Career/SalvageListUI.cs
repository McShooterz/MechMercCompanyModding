using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SalvageListUI : MonoBehaviour
{
    [SerializeField]
    Transform content;

    [SerializeField]
    List<SalvageEntryUI> salvageEntryUIs = new List<SalvageEntryUI>();

    public List<SalvageEntryUI> SalvageEntryUIs { get => salvageEntryUIs; }

    public void BuildList(List<MechData> mechDatas, List<ComponentDefinition> componentDefinitions)
    {
        int entriesTotal = mechDatas.Count + componentDefinitions.Count;

        if (entriesTotal > 0)
        {
            // Size list
            while (salvageEntryUIs.Count < entriesTotal)
            {
                GameObject salvageEntryObject = Instantiate(salvageEntryUIs[0].gameObject, content);

                SalvageEntryUI salvageEntryUI = salvageEntryObject.GetComponent<SalvageEntryUI>();

                if (salvageEntryUI != null)
                {
                    salvageEntryUIs.Add(salvageEntryUI);
                }
                else
                {
                    Debug.LogError("SalvageEntryUI missing!");
                }
            }

            int index = 0;

            foreach (MechData mechData in mechDatas)
            {
                salvageEntryUIs[index].Initialize(mechData);
                index++;
            }

            foreach (ComponentDefinition componentDefinition in componentDefinitions)
            {
                salvageEntryUIs[index].Initialize(componentDefinition);
                index++;
            }
        }
        else
        {
            salvageEntryUIs[0].gameObject.SetActive(false);
        }
    }
}