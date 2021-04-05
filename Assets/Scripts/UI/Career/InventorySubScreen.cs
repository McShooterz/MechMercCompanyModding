using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySubScreen : MonoBehaviour
{
    [SerializeField]
    ComponentListUI componentListUI;

    [SerializeField]
    ComponentInformationUI componentInformationUI;

    [SerializeField]
    Text componentInfoText;

    [SerializeField]
    Button sellButton;

    [SerializeField]
    Text fundsText;

    [SerializeField]
    Text sellValueText;

    [SerializeField]
    Text resultText;

    ComponentDefinition selectedComponentDefinition;

    void Start()
    {
        componentListUI.Setup(GlobalDataManager.Instance.currentCareer.inventory, SelectComponent, HoverComponent);
    }

    void OnEnable()
    {
        selectedComponentDefinition = null;
        componentInfoText.text = "";

        componentInformationUI.gameObject.SetActive(false);

        sellButton.interactable = false;

        fundsText.text = "";
        sellValueText.text = "";
        resultText.text = "";

        if (componentListUI.HasBeenSetup)
            componentListUI.BuildWeaponsList();
    }


    public void SelectComponent(ComponentDefinition componentDefinition)
    {
        AudioManager.Instance.PlayButtonClick(0);

        selectedComponentDefinition = componentDefinition;

        componentInfoText.text = selectedComponentDefinition.GetDisplayInformation();

        sellButton.interactable = true;

        SetFinancialInfo(selectedComponentDefinition);
    }

    public void HoverComponent(ComponentDefinition componentDefinition)
    {
        if (componentDefinition != null)
        {
            componentInformationUI.gameObject.SetActive(true);
            componentInformationUI.SetComponent(componentDefinition);
        }
        else
        {
            componentInformationUI.gameObject.SetActive(false);
        }
    }

    void SetFinancialInfo(ComponentDefinition componentDefinition)
    {
        int cost = Mathf.CeilToInt(selectedComponentDefinition.MarketValue * 0.75f);
        int result = GlobalDataManager.Instance.currentCareer.funds + cost;

        fundsText.text = StaticHelper.FormatMoney(GlobalDataManager.Instance.currentCareer.funds);
        sellValueText.text = StaticHelper.FormatMoney(cost);
        resultText.text = StaticHelper.FormatMoney(result);
    }

    public void ClickSellButton()
    {
        AudioManager.Instance.PlayButtonClick(0);

        GlobalDataManager.Instance.currentCareer.funds += Mathf.CeilToInt(selectedComponentDefinition.MarketValue * 0.75f);

        GlobalDataManager.Instance.currentCareer.MarketInventory.AddComponent(selectedComponentDefinition);

        GlobalDataManager.Instance.currentCareer.inventory.TakeComponent(selectedComponentDefinition);

        if (GlobalDataManager.Instance.currentCareer.inventory.GetComponentCount(selectedComponentDefinition) > 0)
        {
            SetFinancialInfo(selectedComponentDefinition);
        }
        else
        {
            selectedComponentDefinition = null;
            componentInfoText.text = "";

            sellButton.interactable = false;

            fundsText.text = "";
            sellValueText.text = "";
            resultText.text = "";
        }

        componentListUI.Refresh();
    }
}
