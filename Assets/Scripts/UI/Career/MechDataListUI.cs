using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MechDataListUI : MonoBehaviour
{
    [SerializeField]
    Transform content;

    [SerializeField]
    List<MechDataEntryUI> mechDataEntryUIs = new List<MechDataEntryUI>();

    [SerializeField]
    Color activeColor;

    public delegate void CallBackSelect(int index);
    public CallBackSelect callBackSelect;

    public void BuildList(List<MechData> mechDatas, CallBackSelect callback)
    {
        callBackSelect = callback;

        SetListLength(mechDatas.Count);

        for (int i = 0; i < mechDatas.Count; i++)
        {
            mechDataEntryUIs[i].Initialize(mechDatas[i], i, SelectIndex);
        }

        if (mechDatas.Count > 0)
        {
            SelectIndex(0);
        }
    }

    void SetListLength(int count)
    {
        while (mechDataEntryUIs.Count < count)
        {
            GameObject mechEntryObject = Instantiate(mechDataEntryUIs[0].gameObject, content);

            MechDataEntryUI mechDataEntryUI = mechEntryObject.GetComponent<MechDataEntryUI>();

            if (mechDataEntryUI != null)
            {
                mechDataEntryUIs.Add(mechDataEntryUI);
            }
            else
            {
                Debug.LogError("MechDataEntryUI missing!");
            }
        }

        for (int i = 0; i < mechDataEntryUIs.Count; i++)
        {
            mechDataEntryUIs[i].gameObject.SetActive(i < count);
        }
    }

    public void SelectIndex(int index)
    {
        for (int i = 0; i < mechDataEntryUIs.Count; i++)
        {
            if (!mechDataEntryUIs[i].gameObject.activeInHierarchy)
                break;

            if (i == index)
            {
                mechDataEntryUIs[i].Background.color = activeColor;
            }
            else
            {
                mechDataEntryUIs[i].Background.color = Color.white;
            }
        }

        callBackSelect(index);
    }
}
