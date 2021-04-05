using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SalvageSubScreen : MonoBehaviour
{
    [SerializeField]
    SalvageListUI salvageListUI;

    [SerializeField]
    Button salvageButton;

    [SerializeField]
    Button clearButton;

    [SerializeField]
    Text fundsValueText;

    [SerializeField]
    Text salvageCostValueText;

    [SerializeField]
    Text fundsBalanceValueText;

    [SerializeField]
    Text salvageValueValueText;

    [SerializeField]
    Text salvageTonsValueText;

    [SerializeField]
    Text costPerTonValueText;

    [SerializeField]
    MechSalvageSortWindow mechSalvageSortWindow;

    [SerializeField]
    int salvageCost;

    [SerializeField]
    bool salvageDone = false;

    public bool SalvageDone { get => salvageDone; }

    void Start()
    {
        Career career = GlobalDataManager.Instance.currentCareer;

        salvageListUI.BuildList(career.MechsSalvage, career.ComponentsSalvage);

        fundsValueText.text = StaticHelper.FormatMoney(career.funds);

        salvageCostValueText.text = StaticHelper.FormatMoney(0);

        fundsBalanceValueText.text = StaticHelper.FormatMoney(career.funds);

        salvageValueValueText.text = StaticHelper.FormatMoney(0);

        salvageTonsValueText.text = "0";

        costPerTonValueText.text = StaticHelper.FormatMoney(0);

        salvageButton.interactable = false;
        clearButton.interactable = false;

        mechSalvageSortWindow.gameObject.SetActive(false);
    }

    public void UpdateSalvage()
    {
        Career career = GlobalDataManager.Instance.currentCareer;
        List<MechData> mechDatas = new List<MechData>();
        List<ComponentDefinition> componentDefinitions = new List<ComponentDefinition>();

        AudioManager.Instance.PlayButtonClick(0);

        foreach (SalvageEntryUI salvageEntry in salvageListUI.SalvageEntryUIs)
        {
            if (salvageEntry.TakeToggle.isOn)
            {
                if (salvageEntry.Mech != null)
                {
                    mechDatas.Add(salvageEntry.Mech);
                }
                else if (salvageEntry.Component != null)
                {
                    componentDefinitions.Add(salvageEntry.Component);
                }
            }
        }

        if (mechDatas.Count > 0 || componentDefinitions.Count > 0)
        {
            float salvageTons = 0;
            int costPerTon = 0;
            int salvageValue = 0;
            int balance;

            foreach (MechData mechData in mechDatas)
            {
                salvageTons += mechData.MechChassis.Tonnage;
                salvageValue += mechData.MarketValue;
            }

            foreach (ComponentDefinition componentDefinition in componentDefinitions)
            {
                salvageTons += componentDefinition.Weight;
                salvageValue += componentDefinition.MarketValue;
            }

            for (int i = 0; i < mechDatas.Count; i++)
            {
                if (mechDatas[i].HeadDestroyed)
                {
                    costPerTon += 5000;
                }
                else if (mechDatas[i].LegLeftDestroyed && mechDatas[i].LegRightDestroyed)
                {
                    costPerTon += 10000;
                }
                else
                {
                    costPerTon += 20000;
                }
            }

            costPerTon += componentDefinitions.Count * 4000;

            salvageCost = Mathf.CeilToInt(salvageTons * costPerTon);

            balance = career.funds - salvageCost;

            fundsValueText.text = StaticHelper.FormatMoney(career.funds);

            salvageCostValueText.text = StaticHelper.FormatMoney(salvageCost);

            fundsBalanceValueText.text = StaticHelper.FormatMoney(balance);

            salvageValueValueText.text = StaticHelper.FormatMoney(salvageValue);

            salvageTonsValueText.text = salvageTons.ToString("0.##");

            costPerTonValueText.text = StaticHelper.FormatMoney(costPerTon);

            if (balance >= 0)
            {
                fundsBalanceValueText.color = Color.white;
            }
            else
            {
                fundsBalanceValueText.color = Color.red;
            }

            salvageButton.interactable = balance >= 0;
            clearButton.interactable = true;
        }
        else
        {
            fundsValueText.text = StaticHelper.FormatMoney(career.funds);

            salvageCostValueText.text = StaticHelper.FormatMoney(0);

            fundsBalanceValueText.text = StaticHelper.FormatMoney(career.funds);

            salvageValueValueText.text = StaticHelper.FormatMoney(0);

            salvageTonsValueText.text = "0";

            costPerTonValueText.text = StaticHelper.FormatMoney(0);

            salvageButton.interactable = false;
            clearButton.interactable = false;
        }
    }

    public void ClickSalvageButton()
    {
        AudioManager.Instance.PlayButtonClick(0);

        Career career = GlobalDataManager.Instance.currentCareer;

        List<MechData> mechsSalvaged = new List<MechData>();

        salvageButton.interactable = false;
        clearButton.interactable = false;

        career.funds -= salvageCost;

        foreach (SalvageEntryUI salvageEntry in salvageListUI.SalvageEntryUIs)
        {
            if (salvageEntry.TakeToggle.isOn)
            {
                if (salvageEntry.Mech != null)
                {
                    career.AddMechNew(salvageEntry.Mech);
                    mechsSalvaged.Add(salvageEntry.Mech);
                }
                else if (salvageEntry.Component != null)
                {
                    career.inventory.AddComponent(salvageEntry.Component);
                }
            }

            salvageEntry.TakeToggle.interactable = false;
        }

        if (mechsSalvaged.Count > 0)
        {
            mechSalvageSortWindow.gameObject.SetActive(true);

            mechSalvageSortWindow.BuildList(mechsSalvaged);
        }

        salvageDone = true;
    }

    public void ClickClearButton()
    {
        foreach (SalvageEntryUI salvageEntry in salvageListUI.SalvageEntryUIs)
        {
            salvageEntry.TakeToggle.SetIsOnWithoutNotify(false);
        }

        UpdateSalvage();
    }
}
