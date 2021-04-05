using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContractListUI : MonoBehaviour
{
    [SerializeField]
    ContractsSubScreen contractsSubScreen;

    [SerializeField]
    Transform content;

    [SerializeField]
    List<ContractEntryUI> contractEntryUIs = new List<ContractEntryUI>();

    [SerializeField]
    Color activeColor;

    [SerializeField]
    Color contractActiveColor;

    public void BuildList(List<ContractData> contractData)
    {
        SetListLength(contractData.Count);

        for (int i = 0; i < contractData.Count; i++)
        {
            contractEntryUIs[i].Initialize(contractData[i], i);

            if (contractData[i] == GlobalDataManager.Instance.currentCareer.currentContract)
            {
                contractEntryUIs[i].NameText.color = contractActiveColor;
                contractEntryUIs[i].NameText.fontStyle = FontStyle.Bold;
            }
            else
            {
                contractEntryUIs[i].NameText.color = Color.black;
                contractEntryUIs[i].NameText.fontStyle = FontStyle.Normal;
            }
        }

        if (contractData.Count > 0)
        {
            SelectIndex(0);
        }
    }

    public void RefreshList(List<ContractData> contractData)
    {
        for (int i = 0; i < contractData.Count; i++)
        {
            if (contractData[i] == GlobalDataManager.Instance.currentCareer.currentContract)
            {
                contractEntryUIs[i].NameText.color = contractActiveColor;
                contractEntryUIs[i].NameText.fontStyle = FontStyle.Bold;
            }
            else
            {
                contractEntryUIs[i].NameText.color = Color.black;
                contractEntryUIs[i].NameText.fontStyle = FontStyle.Normal;
            }
        }
    }

    void SetListLength(int count)
    {
        while (contractEntryUIs.Count < count)
        {
            GameObject contractEntryObject = Instantiate(contractEntryUIs[0].gameObject, content);

            ContractEntryUI contractEntryUI = contractEntryObject.GetComponent<ContractEntryUI>();

            if (contractEntryUI != null)
            {
                contractEntryUIs.Add(contractEntryUI);
            }
            else
            {
                Debug.LogError("ContractEntryUI missing!");
            }
        }

        for (int i = 0; i < contractEntryUIs.Count; i++)
        {
            if (i < count)
            {
                contractEntryUIs[i].gameObject.SetActive(true);
            }
            else
            {
                contractEntryUIs[i].gameObject.SetActive(false);
            }
        }
    }

    public void SelectIndex(int index)
    {
        for (int i = 0; i < contractEntryUIs.Count; i++)
        {
            if (!contractEntryUIs[i].gameObject.activeInHierarchy)
                break;

            if (i == index)
            {
                contractEntryUIs[i].Background.color = activeColor;
            }
            else
            {
                contractEntryUIs[i].Background.color = Color.white;
            }
        }

        contractsSubScreen.SelectIndex(index);
    }
}
