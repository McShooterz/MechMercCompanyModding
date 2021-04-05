using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class MechsSubScreen : MonoBehaviour
{
    [SerializeField]
    MechHangarTerminalScreen mechHangarTerminalScreen;

    [SerializeField]
    MechRepairWindowUI mechRepairWindowUI;

    [SerializeField]
    MechDataListUI mechDataListUI;

    [SerializeField]
    InputField mechNameInputField;

    [SerializeField]
    Button renameButton;

    [SerializeField]
    Button repairButton;

    [SerializeField]
    Button customizationButton;

    [SerializeField]
    Button sellButton;

    [SerializeField]
    Text fundsValueText;

    [SerializeField]
    Text mechValueText;

    [SerializeField]
    Text resultValueText;

    [SerializeField]
    Text mechInfoText;

    [SerializeField]
    Text mechStatusText;

    [SerializeField]
    GameObject mechSellConfirmationWindow;

    MechData selectedMechData;

    public MechDataListUI MechDataListUI { get => mechDataListUI; }

    void OnEnable()
    {
        mechRepairWindowUI.gameObject.SetActive(false);

        if (mechSellConfirmationWindow != null)
        {
            mechSellConfirmationWindow.SetActive(false);
        }

        UpdateMechList();
    }

    void UpdateMechList()
    {
        mechDataListUI.BuildList(GlobalDataManager.Instance.currentCareer.Mechs.Values.ToList(), SelectIndex);

        if (GlobalDataManager.Instance.currentCareer.Mechs.Count > 0)
        {
            sellButton.interactable = true;
            renameButton.interactable = true;

            mechNameInputField.interactable = true;
        }
        else
        {
            mechHangarTerminalScreen.ClearPreviewMech();

            repairButton.interactable = false;
            customizationButton.interactable = false;
            sellButton.interactable = false;
            renameButton.interactable = false;

            mechNameInputField.text = "";
            mechNameInputField.interactable = false;

            fundsValueText.text = "";
            mechValueText.text = "";
            resultValueText.text = "";

            mechInfoText.text = "";
            mechStatusText.text = "";
        }
    }

    public void Refresh()
    {
        int selectedIndex = GlobalDataManager.Instance.currentCareer.Mechs.Values.ToList().IndexOf(selectedMechData);

        UpdateMechList();

        SelectIndex(selectedIndex);
    }

    public void SelectIndex(int index)
    {
        selectedMechData = GlobalDataManager.Instance.currentCareer.Mechs.Values.ToList()[index];

        mechHangarTerminalScreen.SetPreviewMech(selectedMechData.MechChassis, selectedMechData.mechPaintScheme);

        mechNameInputField.text = selectedMechData.customName;

        int mechValue = Mathf.CeilToInt(selectedMechData.MarketValue * 0.75f);

        fundsValueText.text = StaticHelper.FormatMoney(GlobalDataManager.Instance.currentCareer.funds);
        mechValueText.text = StaticHelper.FormatMoney(mechValue);
        resultValueText.text = StaticHelper.FormatMoney(GlobalDataManager.Instance.currentCareer.funds + mechValue);

        System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();

        stringBuilder.AppendLine(selectedMechData.MechChassis.GetDisplayInformation());

        stringBuilder.AppendLine("");

        stringBuilder.AppendLine(selectedMechData.MechDesign.GetDisplayInformation());

        mechInfoText.text = stringBuilder.ToString();

        UpdateMechStatus();
    }

    public void UpdateMechStatus()
    {
        MechStatusType mechStatusType = selectedMechData.MechStatus;

        mechStatusText.text = StaticHelper.GetMechStatusName(mechStatusType);
        mechStatusText.color = StaticHelper.GetMechStatusColor(mechStatusType);

        bool isDamaged = selectedMechData.IsDamaged;
        repairButton.interactable = isDamaged;
        customizationButton.interactable = !isDamaged;
    }

    public void OnValueChangedNameInputField()
    {
        if (mechNameInputField.text == "")
        {
            renameButton.interactable = false;
        }
        else
        {
            renameButton.interactable = true;
        }
    }

    public void ClickRenameButton()
    {
        AudioManager.Instance.PlayButtonClick(0);

        List<MechData> mechDataList = GlobalDataManager.Instance.currentCareer.Mechs.Values.ToList();

        selectedMechData.customName = mechNameInputField.text;

        int index = mechDataList.IndexOf(selectedMechData);

        mechDataListUI.BuildList(mechDataList, SelectIndex);

        mechDataListUI.SelectIndex(index);
    }

    public void ClickRepairButton()
    {
        AudioManager.Instance.PlayButtonClick(0);

        mechRepairWindowUI.callBack = Refresh;
        mechRepairWindowUI.gameObject.SetActive(true);

        mechRepairWindowUI.SetMech(selectedMechData);
    }

    public void ClickCustomizeButton()
    {
        AudioManager.Instance.PlayButtonClick(0);

        GlobalDataManager.Instance.currentCareer.CustomizingMechData = selectedMechData;

        GlobalDataManager.Instance.mechDataCustomizing = selectedMechData;
        GlobalDataManager.Instance.backSceneName = "HomeBase";
        GlobalDataManager.Instance.inventoryCurrent = GlobalDataManager.Instance.currentCareer.inventory;

        LoadingScreen.Instance.LoadScene("MechCustomizingScreen");
    }

    public void ClickSellButton()
    {
        AudioManager.Instance.PlayButtonClick(0);

        mechSellConfirmationWindow.SetActive(true);
    }

    public void ClickAcceptSellButton()
    {
        AudioManager.Instance.PlayButtonClick(0);

        foreach (ComponentDefinition componentDefinition in selectedMechData.ComponentsSalvagable)
        {
            GlobalDataManager.Instance.currentCareer.inventory.AddComponent(componentDefinition);
        }

        GlobalDataManager.Instance.currentCareer.funds += Mathf.CeilToInt(selectedMechData.MarketValue * 0.75f);

        GlobalDataManager.Instance.currentCareer.RemoveMech(selectedMechData);

        UpdateMechList();

        mechSellConfirmationWindow.SetActive(false);
    }

    public void ClickCancelSellButton()
    {
        AudioManager.Instance.PlayButtonClick(0);

        mechSellConfirmationWindow.SetActive(false);
    }
}