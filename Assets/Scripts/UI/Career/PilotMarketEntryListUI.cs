using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PilotMarketEntryListUI : MonoBehaviour
{
    [SerializeField]
    PersonnelSubScreen personnelSubScreen;

    [SerializeField]
    Transform content;

    [SerializeField]
    List<PilotMarketEntryUI> pilotMarketEntryUIs = new List<PilotMarketEntryUI>();

    [SerializeField]
    Color activeColor;

    public void BuildList(List<MechPilot> mechPilots)
    {
        SetListLength(mechPilots.Count);

        for (int i = 0; i < mechPilots.Count; i++)
        {
            pilotMarketEntryUIs[i].Initialize(mechPilots[i], i);
        }

        if (mechPilots.Count > 0)
        {
            SelectIndex(0);
        }
        else
        {
            personnelSubScreen.ClearSelectedPilot();
        }
    }

    void SetListLength(int count)
    {
        while (pilotMarketEntryUIs.Count < count)
        {
            GameObject pilotMarketEntryObject = Instantiate(pilotMarketEntryUIs[0].gameObject, content);

            PilotMarketEntryUI pilotMarketEntryUI = pilotMarketEntryObject.GetComponent<PilotMarketEntryUI>();

            if (pilotMarketEntryUI != null)
            {
                pilotMarketEntryUIs.Add(pilotMarketEntryUI);
            }
            else
            {
                Debug.LogError("MechHangarEntryUI missing!");
            }
        }

        for (int i = 0; i < pilotMarketEntryUIs.Count; i++)
        {
            if (i < count)
            {
                pilotMarketEntryUIs[i].gameObject.SetActive(true);
            }
            else
            {
                pilotMarketEntryUIs[i].gameObject.SetActive(false);
            }
        }
    }

    public void SelectIndex(int index)
    {
        for (int i = 0; i < pilotMarketEntryUIs.Count; i++)
        {
            if (!pilotMarketEntryUIs[i].gameObject.activeInHierarchy)
                break;

            if (i == index)
            {
                pilotMarketEntryUIs[i].Background.color = activeColor;
            }
            else
            {
                pilotMarketEntryUIs[i].Background.color = Color.white;
            }
        }

        personnelSubScreen.SelectMarketPilotIndex(index);
    }
}
