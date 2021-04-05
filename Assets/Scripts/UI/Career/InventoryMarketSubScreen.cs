using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryMarketSubScreen : MonoBehaviour
{
    [SerializeField]
    ComponentListUI componentListUI;

    [SerializeField]
    ComponentInformationUI componentInformationUI;

    [SerializeField]
    Text componentInfoText;

    [SerializeField]
    Button buyButton;

    [SerializeField]
    Text fundsText;

    [SerializeField]
    Text costText;

    [SerializeField]
    Text resultText;

    ComponentDefinition selectedComponentDefinition;

    void Start()
    {
        componentListUI.Setup(GlobalDataManager.Instance.currentCareer.MarketInventory, SelectComponent, HoverComponent);
    }

    void OnEnable()
    {
        selectedComponentDefinition = null;
        componentInfoText.text = "";

        componentInformationUI.gameObject.SetActive(false);

        if (componentListUI.HasBeenSetup)
            componentListUI.BuildWeaponsList();

        buyButton.interactable = false;

        fundsText.text = "";
        costText.text = "";
        resultText.text = "";
    }

    public void SelectComponent(ComponentDefinition componentDefinition)
    {
        AudioManager.Instance.PlayButtonClick(0);

        selectedComponentDefinition = componentDefinition;

        componentInfoText.text = selectedComponentDefinition.GetDisplayInformation();

        SetFinancialInfo(selectedComponentDefinition);
    }

    void SetFinancialInfo(ComponentDefinition componentDefinition)
    {
        int cost = Mathf.CeilToInt(selectedComponentDefinition.MarketValue * 1.25f);
        int result = GlobalDataManager.Instance.currentCareer.funds - cost;

        fundsText.text = StaticHelper.FormatMoney(GlobalDataManager.Instance.currentCareer.funds);
        costText.text = StaticHelper.FormatMoney(cost);
        resultText.text = StaticHelper.FormatMoney(result);

        if (result < 0)
        {
            buyButton.interactable = false;

            resultText.color = Color.red;
        }
        else
        {
            buyButton.interactable = true;

            resultText.color = Color.white;
        }
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

    public void ClickBuyButton()
    {
        AudioManager.Instance.PlayButtonClick(0);

        GlobalDataManager.Instance.currentCareer.funds -= Mathf.CeilToInt(selectedComponentDefinition.MarketValue * 1.25f);

        GlobalDataManager.Instance.currentCareer.inventory.AddComponent(selectedComponentDefinition);

        GlobalDataManager.Instance.currentCareer.MarketInventory.TakeComponent(selectedComponentDefinition);

        if (GlobalDataManager.Instance.currentCareer.MarketInventory.GetComponentCount(selectedComponentDefinition) > 0)
        {
            SetFinancialInfo(selectedComponentDefinition);
        }
        else
        {
            selectedComponentDefinition = null;
            componentInfoText.text = "";

            buyButton.interactable = false;

            fundsText.text = "";
            costText.text = "";
            resultText.text = "";
        }

        componentListUI.Refresh();
    }
}
