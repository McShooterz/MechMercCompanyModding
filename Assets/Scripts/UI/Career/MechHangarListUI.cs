using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MechHangarListUI : MonoBehaviour
{
    [SerializeField]
    MechsSubScreen mechsSubScreen;

    [SerializeField]
    Transform content;

    [SerializeField]
    List<MechHangarEntryUI> mechHangarEntryUIs = new List<MechHangarEntryUI>();

    [SerializeField]
    Color activeColor;

    public void BuildList(List<MechData> mechDatas)
    {
        SetListLength(mechDatas.Count);

        for (int i = 0; i < mechDatas.Count; i++)
        {
            mechHangarEntryUIs[i].Initialize(mechDatas[i], i);
        }

        if (mechDatas.Count > 0)
        {
            SelectIndex(0);
        }
    }

    void SetListLength(int count)
    {
        while (mechHangarEntryUIs.Count < count)
        {
            GameObject mechHangarEntryObject = Instantiate(mechHangarEntryUIs[0].gameObject, content);

            MechHangarEntryUI mechHangarEntryUI = mechHangarEntryObject.GetComponent<MechHangarEntryUI>();

            if (mechHangarEntryUI != null)
            {
                mechHangarEntryUIs.Add(mechHangarEntryUI);
            }
            else
            {
                Debug.LogError("MechHangarEntryUI missing!");
            }
        }

        for (int i = 0; i < mechHangarEntryUIs.Count; i++)
        {
            if (i < count)
            {
                mechHangarEntryUIs[i].gameObject.SetActive(true);
            }
            else
            {
                mechHangarEntryUIs[i].gameObject.SetActive(false);
            }
        }
    }

    public void SelectIndex(int index)
    {
        for (int i = 0; i < mechHangarEntryUIs.Count; i++)
        {
            if (!mechHangarEntryUIs[i].gameObject.activeInHierarchy)
                break;

            if (i == index)
            {
                mechHangarEntryUIs[i].Background.color = activeColor;
            }
            else
            {
                mechHangarEntryUIs[i].Background.color = Color.white;
            }
        }

        mechsSubScreen.SelectIndex(index);
    }
}
