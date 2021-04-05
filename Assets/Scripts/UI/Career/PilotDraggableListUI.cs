using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PilotDraggableListUI : MonoBehaviour
{
    [SerializeField]
    Transform content;

    [SerializeField]
    List<PilotDraggableEntryUI> pilotDraggableEntryUIs = new List<PilotDraggableEntryUI>();

    public void BuildList(List<MechPilot> mechPilots, PilotDraggableEntryUI.CallBackSelect callBackSelect, PilotDraggableEntryUI.CallBackHover callBackHover)
    {
        SetListLength(mechPilots.Count);

        for (int i = 0; i < mechPilots.Count; i++)
        {
            pilotDraggableEntryUIs[i].Initialize(mechPilots[i], callBackSelect, callBackHover);
        }
    }

    void SetListLength(int count)
    {
        pilotDraggableEntryUIs[0].Clear();

        while (pilotDraggableEntryUIs.Count < count)
        {
            GameObject pilotEntryObject = Instantiate(pilotDraggableEntryUIs[0].gameObject, content);

            PilotDraggableEntryUI pilotDraggableEntryUI = pilotEntryObject.GetComponent<PilotDraggableEntryUI>();

            if (pilotDraggableEntryUI != null)
            {
                pilotDraggableEntryUIs.Add(pilotDraggableEntryUI);
            }
            else
            {
                Debug.LogError("PilotDraggableEntryUI missing!");
            }
        }

        for (int i = 0; i < pilotDraggableEntryUIs.Count; i++)
        {
            if (i < count)
            {
                pilotDraggableEntryUIs[i].gameObject.SetActive(true);
            }
            else
            {
                pilotDraggableEntryUIs[i].gameObject.SetActive(false);
                pilotDraggableEntryUIs[i].Clear();
            }
        }
    }

    public void RestorePilot(MechPilot mechPilot)
    {
        foreach (PilotDraggableEntryUI pilotEntryUI in pilotDraggableEntryUIs)
        {
            if (pilotEntryUI.MechPilot == mechPilot)
            {
                pilotEntryUI.gameObject.SetActive(true);
                break;
            }
        }
    }

    public void RemovePilot(MechPilot mechPilot)
    {
        if (mechPilot == null)
            return;

        foreach (PilotDraggableEntryUI pilotEntryUI in pilotDraggableEntryUIs)
        {
            if (pilotEntryUI.MechPilot == mechPilot)
            {
                pilotEntryUI.gameObject.SetActive(false);
                break;
            }
        }
    }
}
