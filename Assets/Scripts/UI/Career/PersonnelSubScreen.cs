using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class PersonnelSubScreen : MonoBehaviour
{
    [SerializeField]
    PilotEntryListUI pilotEntryListUI;

    [SerializeField]
    PilotMarketEntryListUI pilotMarketEntryListUI;

    [SerializeField]
    Image yourPilotsButtonBackground;

    [SerializeField]
    Image forHirePilotsButtonBackground;

    [SerializeField]
    Button firePilotButton;

    [SerializeField]
    Button hirePilotButton;

    [SerializeField]
    Button editPilotButton;

    [SerializeField]
    Image pilotIconImage;

    [SerializeField]
    Text pilotDisplayText;

    [SerializeField]
    GameObject financialInfoRoot;

    [SerializeField]
    Text fundsValueText;

    [SerializeField]
    Text contractValueText;

    [SerializeField]
    Text balanceValueText;

    [SerializeField]
    PilotEditWindow pilotEditWindow;

    [SerializeField]
    Color activeColor;

    MechPilot selectedMechPilot;

    void OnEnable()
    {
        pilotEditWindow.gameObject.SetActive(false);

        yourPilotsButtonBackground.color = activeColor;
        forHirePilotsButtonBackground.color = Color.white;

        firePilotButton.gameObject.SetActive(false);
        hirePilotButton.gameObject.SetActive(false);
        editPilotButton.gameObject.SetActive(false);
        financialInfoRoot.gameObject.SetActive(false);

        pilotEntryListUI.gameObject.SetActive(true);
        pilotMarketEntryListUI.gameObject.SetActive(false);
        pilotEntryListUI.BuildList(GlobalDataManager.Instance.currentCareer.Pilots.Values.ToList());
    }

    public void SelectYourPilotIndex(int index)
    {
        firePilotButton.gameObject.SetActive(true);
        hirePilotButton.gameObject.SetActive(false);
        editPilotButton.gameObject.SetActive(true);

        financialInfoRoot.SetActive(false);

        selectedMechPilot = GlobalDataManager.Instance.currentCareer.Pilots.Values.ToList()[index];

        SetPilotInfo(selectedMechPilot);
    }

    public void SelectMarketPilotIndex(int index)
    {
        firePilotButton.gameObject.SetActive(false);
        hirePilotButton.gameObject.SetActive(true);
        editPilotButton.gameObject.SetActive(false);

        financialInfoRoot.SetActive(true);

        selectedMechPilot = GlobalDataManager.Instance.currentCareer.PilotsForHire[index];

        SetPilotInfo(selectedMechPilot);

        fundsValueText.text = StaticHelper.FormatMoney(GlobalDataManager.Instance.currentCareer.funds);

        contractValueText.text = StaticHelper.FormatMoney(selectedMechPilot.contractValue);

        int balance = GlobalDataManager.Instance.currentCareer.funds - selectedMechPilot.contractValue;

        balanceValueText.text = StaticHelper.FormatMoney(balance);

        if (balance < 0)
        {
            hirePilotButton.interactable = false;
            balanceValueText.color = Color.red;
        }
        else
        {
            hirePilotButton.interactable = true;
            balanceValueText.color = Color.white;
        }
    }

    void SetPilotInfo(MechPilot mechPilot)
    {
        if (mechPilot.Icon != null)
        {
            pilotIconImage.enabled = true;
            pilotIconImage.sprite = mechPilot.Icon;
        }
        else
        {
            pilotIconImage.enabled = false;
        }

        pilotDisplayText.enabled = true;
        pilotDisplayText.text = mechPilot.CareerDisplay;
    }

    public void RefreshInfo()
    {
        List<MechPilot> pilotList = GlobalDataManager.Instance.currentCareer.Pilots.Values.ToList();

        int index = pilotList.IndexOf(selectedMechPilot);

        pilotEntryListUI.BuildList(pilotList);

        if (index != -1)
            SelectYourPilotIndex(index);
    }

    public void ClearSelectedPilot()
    {
        selectedMechPilot = null;

        pilotIconImage.enabled = false;
        pilotDisplayText.enabled = false;

        financialInfoRoot.SetActive(false);

        firePilotButton.gameObject.SetActive(false);
        hirePilotButton.gameObject.SetActive(false);
        editPilotButton.gameObject.SetActive(false);
    }

    public void ClickYourPilotsButton()
    {
        AudioManager.Instance.PlayButtonClick(0);

        yourPilotsButtonBackground.color = activeColor;
        forHirePilotsButtonBackground.color = Color.white;

        hirePilotButton.gameObject.SetActive(false);

        pilotEntryListUI.gameObject.SetActive(true);
        pilotMarketEntryListUI.gameObject.SetActive(false);
        pilotEntryListUI.BuildList(GlobalDataManager.Instance.currentCareer.Pilots.Values.ToList());
    }

    public void ClickForHirePilotsButton()
    {
        AudioManager.Instance.PlayButtonClick(0);

        yourPilotsButtonBackground.color = Color.white;
        forHirePilotsButtonBackground.color = activeColor;

        firePilotButton.gameObject.SetActive(false);

        pilotEntryListUI.gameObject.SetActive(false);
        pilotMarketEntryListUI.gameObject.SetActive(true);
        pilotMarketEntryListUI.BuildList(GlobalDataManager.Instance.currentCareer.PilotsForHire);
    }

    public void ClickFirePilotButton()
    {
        AudioManager.Instance.PlayButtonClick(0);

        selectedMechPilot.contractEndDate = new GameDate();
        selectedMechPilot.GenerateContractValues();

        GlobalDataManager.Instance.currentCareer.PilotsForHire.Add(selectedMechPilot);

        GlobalDataManager.Instance.currentCareer.RemovePilot(selectedMechPilot);

        firePilotButton.gameObject.SetActive(false);
        editPilotButton.gameObject.SetActive(false);

        pilotEntryListUI.BuildList(GlobalDataManager.Instance.currentCareer.Pilots.Values.ToList());
    }

    public void ClickHirePilotButton()
    {
        AudioManager.Instance.PlayButtonClick(0);

        selectedMechPilot.contractEndDate = new GameDate(GlobalDataManager.Instance.currentCareer.gameDate);
        selectedMechPilot.contractEndDate.AddDays(182);

        GlobalDataManager.Instance.currentCareer.funds -= selectedMechPilot.contractValue;

        GlobalDataManager.Instance.currentCareer.AddPilotNew(selectedMechPilot);

        GlobalDataManager.Instance.currentCareer.PilotsForHire.Remove(selectedMechPilot);

        hirePilotButton.gameObject.SetActive(false);

        pilotMarketEntryListUI.BuildList(GlobalDataManager.Instance.currentCareer.PilotsForHire);
    }

    public void ClickEditButton()
    {
        AudioManager.Instance.PlayButtonClick(0);

        pilotEditWindow.gameObject.SetActive(true);

        pilotEditWindow.SetPilot(selectedMechPilot);
    }
}
