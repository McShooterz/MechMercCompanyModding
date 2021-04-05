using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MechDraggableListUI : MonoBehaviour
{
    [SerializeField]
    Transform content;

    [SerializeField]
    List<MechDraggableEntryUI> mechDraggableEntryUIs = new List<MechDraggableEntryUI>();

    public void BuildList(List<MechData> mechDatas, MechDraggableEntryUI.CallBackSelect callBackSelect, MechDraggableEntryUI.CallBackHover callBackHover)
    {
        SetListLength(mechDatas.Count);

        for (int i = 0; i < mechDatas.Count; i++)
        {
            mechDraggableEntryUIs[i].Initialize(mechDatas[i], callBackSelect, callBackHover);
        }
    }

    public void RefreshList()
    {
        for (int i = 0; i < mechDraggableEntryUIs.Count; i++)
        {
            mechDraggableEntryUIs[i].Refresh();
        }
    }

    void SetListLength(int count)
    {
        mechDraggableEntryUIs[0].Clear();

        while (mechDraggableEntryUIs.Count < count)
        {
            GameObject mechDropshipEntryObject = Instantiate(mechDraggableEntryUIs[0].gameObject, content);

            MechDraggableEntryUI mechDraggableEntryUI = mechDropshipEntryObject.GetComponent<MechDraggableEntryUI>();

            if (mechDraggableEntryUI != null)
            {
                mechDraggableEntryUIs.Add(mechDraggableEntryUI);
            }
            else
            {
                Debug.LogError("MechDraggableEntryUI missing!");
            }
        }

        for (int i = 0; i < mechDraggableEntryUIs.Count; i++)
        {
            if (i < count)
            {
                mechDraggableEntryUIs[i].gameObject.SetActive(true);
            }
            else
            {
                mechDraggableEntryUIs[i].gameObject.SetActive(false);
                mechDraggableEntryUIs[i].Clear();
            }
        }
    }

    public void RestoreMech(MechData mechData)
    {
        foreach (MechDraggableEntryUI mechDropshipEntryUI in mechDraggableEntryUIs)
        {
            if (mechDropshipEntryUI.MechData == mechData)
            {
                mechDropshipEntryUI.gameObject.SetActive(true);
                break;
            }
        }
    }

    public void RemoveMech(MechData mechData)
    {
        if (mechData == null)
            return;

        foreach (MechDraggableEntryUI mechDropshipEntryUI in mechDraggableEntryUIs)
        {
            if (mechDropshipEntryUI.MechData == mechData)
            {
                mechDropshipEntryUI.gameObject.SetActive(false);
                break;
            }
        }
    }
}
