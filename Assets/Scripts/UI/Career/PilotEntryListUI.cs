using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PilotEntryListUI : MonoBehaviour
{
    [SerializeField]
    PersonnelSubScreen personnelSubScreen;

    [SerializeField]
    Transform content;

    [SerializeField]
    List<PilotEntryUI> pilotEntryUIs = new List<PilotEntryUI>();

    [SerializeField]
    Color activeColor;

    public void BuildList(List<MechPilot> mechPilots)
    {
        SetListLength(mechPilots.Count);

        for (int i = 0; i < mechPilots.Count; i++)
        {
            pilotEntryUIs[i].Initialize(mechPilots[i], i);
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
        while (pilotEntryUIs.Count < count)
        {
            GameObject pilotEntryObject = Instantiate(pilotEntryUIs[0].gameObject, content);

            PilotEntryUI pilotEntryUI = pilotEntryObject.GetComponent<PilotEntryUI>();

            if (pilotEntryUI != null)
            {
                pilotEntryUIs.Add(pilotEntryUI);
            }
            else
            {
                Debug.LogError("MechHangarEntryUI missing!");
            }
        }

        for (int i = 0; i < pilotEntryUIs.Count; i++)
        {
            if (i < count)
            {
                pilotEntryUIs[i].gameObject.SetActive(true);
            }
            else
            {
                pilotEntryUIs[i].gameObject.SetActive(false);
            }
        }
    }

    public void SelectIndex(int index)
    {
        for (int i = 0; i < pilotEntryUIs.Count; i++)
        {
            if (!pilotEntryUIs[i].gameObject.activeInHierarchy)
                break;

            if (i == index)
            {
                pilotEntryUIs[i].Background.color = activeColor;
            }
            else
            {
                pilotEntryUIs[i].Background.color = Color.white;
            }
        }

        personnelSubScreen.SelectYourPilotIndex(index);
    }
}
