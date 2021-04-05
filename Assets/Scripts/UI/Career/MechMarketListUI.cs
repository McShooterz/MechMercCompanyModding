using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MechMarketListUI : MonoBehaviour
{
    [SerializeField]
    MechMarketSubScreen mechMarketSubScreen;

    [SerializeField]
    Transform content;

    [SerializeField]
    List<MechMarketEntryUI> mechMarketEntryUIs = new List<MechMarketEntryUI>();

    [SerializeField]
    Color activeColor;

    public void BuildList(List<MechMarketEntry> mechMarketEntries)
    {
        SetListLength(mechMarketEntries.Count);

        for (int i = 0; i < mechMarketEntries.Count; i++)
        {
            mechMarketEntryUIs[i].Initialize(mechMarketEntries[i], i);
        }

        if (mechMarketEntries.Count > 0)
        {
            SelectIndex(0);
        }
    }

    void SetListLength(int count)
    {
        while (mechMarketEntryUIs.Count < count)
        {
            GameObject mechMarketEntryObject = Instantiate(mechMarketEntryUIs[0].gameObject, content);

            MechMarketEntryUI mechMarketEntryUI = mechMarketEntryObject.GetComponent<MechMarketEntryUI>();

            if (mechMarketEntryUI != null)
            {
                mechMarketEntryUIs.Add(mechMarketEntryUI);
            }
            else
            {
                Debug.LogError("MechMarketEntryUI missing!");
            }
        }

        for (int i = 0; i < mechMarketEntryUIs.Count; i++)
        {
            if (i < count)
            {
                mechMarketEntryUIs[i].gameObject.SetActive(true);
            }
            else
            {
                mechMarketEntryUIs[i].gameObject.SetActive(false);
            }
        }
    }

    public void SelectIndex(int index)
    {
        for (int i = 0; i < mechMarketEntryUIs.Count; i++)
        {
            if (!mechMarketEntryUIs[i].gameObject.activeInHierarchy)
                break;

            if (i == index)
            {
                mechMarketEntryUIs[i].Background.color = activeColor;
            }
            else
            {
                mechMarketEntryUIs[i].Background.color = Color.white;
            }
        }

        mechMarketSubScreen.SelectIndex(index);
    }
}
