using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MechMarketSubScreen : MonoBehaviour
{
    [SerializeField]
    MechHangarTerminalScreen mechHangarTerminalScreen;

    [SerializeField]
    MechMarketListUI mechMarketListUI;

    [SerializeField]
    Button purchaseButton;

    [SerializeField]
    Text mechStatsText;

    [SerializeField]
    Text fundsValueText;

    [SerializeField]
    Text costValueText;

    [SerializeField]
    Text resultValueText;

    MechMarketEntry selectedMechMarketEntry;

    void OnEnable()
    {
        UpdateMechList();
    }

    void UpdateMechList()
    {
        mechMarketListUI.BuildList(GlobalDataManager.Instance.currentCareer.MechMarketEntries);

        if (GlobalDataManager.Instance.currentCareer.MechMarketEntries.Count > 0)
        {
            purchaseButton.interactable = true;
        }
        else
        {
            mechHangarTerminalScreen.ClearPreviewMech();
            purchaseButton.interactable = false;
            mechStatsText.text = "";
            fundsValueText.text = "";
            costValueText.text = "";
            resultValueText.text = "";
        }
    }

    public void SelectIndex(int index)
    {
        System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();

        Career career = GlobalDataManager.Instance.currentCareer;

        selectedMechMarketEntry = career.MechMarketEntries[index];

        MechDesign mechDesign = selectedMechMarketEntry.MechDesign;
        MechChassisDefinition mechChassisDefinition = mechDesign.GetMechChassisDefinition();

        int cost = Mathf.CeilToInt(mechDesign.GetMarketValue() * 1.25f);

        purchaseButton.interactable = career.funds > cost;

        mechHangarTerminalScreen.SetPreviewMech(mechChassisDefinition, null);

        stringBuilder.AppendLine(mechChassisDefinition.GetDisplayInformation());

        stringBuilder.AppendLine("");

        stringBuilder.AppendLine(mechDesign.GetDisplayInformation());

        mechStatsText.text = stringBuilder.ToString();

        fundsValueText.text = StaticHelper.FormatMoney(career.funds);
        costValueText.text = StaticHelper.FormatMoney(cost);

        int result = career.funds - cost;

        resultValueText.text = StaticHelper.FormatMoney(result);

        if (result > 0)
        {
            resultValueText.color = Color.white;
        }
        else
        {
            resultValueText.color = Color.red;
        }
    }

    public void ClickPurchaseButton()
    {
        AudioManager.Instance.PlayButtonClick(0);

        GlobalDataManager.Instance.currentCareer.PurchaseMech(selectedMechMarketEntry);

        int selectedIndex = GlobalDataManager.Instance.currentCareer.MechMarketEntries.IndexOf(selectedMechMarketEntry);     

        UpdateMechList();

        if (GlobalDataManager.Instance.currentCareer.MechMarketEntries.Count > 0)
        {
            if (selectedIndex != -1)
            {
                mechMarketListUI.SelectIndex(selectedIndex);
            }
            else
            {
                mechMarketListUI.SelectIndex(0);
            }
        }
    }
}
