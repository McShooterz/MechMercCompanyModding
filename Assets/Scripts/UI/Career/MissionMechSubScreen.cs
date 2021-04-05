using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class MissionMechSubScreen : MonoBehaviour
{
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
    Camera mechPreviewCamera;

    [SerializeField]
    GameObject mechPreviewObject;

    MechData selectedMechData;

    public MechDataListUI MechDataListUI { get => mechDataListUI; }

    List<MechData> MechsDropship { get; set; }

    void Awake()
    {
        MechsDropship = GlobalDataManager.Instance.currentCareer.DropshipMechs.Where(mech => mech != null).ToList();
    }

    void Update()
    {
        if ((object)mechPreviewObject != null)
        {
            mechPreviewObject.transform.RotateAround(mechPreviewObject.transform.position, Vector3.up, 100f * Time.unscaledDeltaTime);
        }
    }

    void OnEnable()
    {
        mechRepairWindowUI.gameObject.SetActive(false);

        UpdateMechList();
    }

    public void UpdateMechList()
    {
        if (MechsDropship == null)
            MechsDropship = GlobalDataManager.Instance.currentCareer.DropshipMechs.Where(mech => mech != null).ToList();

        mechDataListUI.BuildList(MechsDropship, SelectIndex);

        if (MechsDropship.Count > 0)
        {
            sellButton.interactable = true;
            renameButton.interactable = true;

            mechNameInputField.interactable = true;
        }
        else
        {
            ClearPreviewMech();

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
        UpdateMechList();

        if (MechsDropship.Count > 0)
        {
            int selectedIndex = MechsDropship.IndexOf(selectedMechData);

            SelectIndex(selectedIndex);
        }
    }

    public void SelectIndex(int index)
    {
        selectedMechData = MechsDropship[index];

        SetPreviewMech(selectedMechData.MechChassis, selectedMechData.mechPaintScheme);

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

    public void SetPreviewMech(MechChassisDefinition mechChassis, MechPaintScheme mechPaintScheme)
    {
        if (mechPreviewObject != null)
        {
            Destroy(mechPreviewObject);
        }

        GameObject mechPreviewPrefab = mechChassis.GetMechPrefab();

        if ((object)mechPreviewPrefab != null)
        {
            mechPreviewObject = Instantiate(mechPreviewPrefab, mechPreviewCamera.transform);

            mechPreviewObject.transform.Rotate(new Vector3(0f, 180f, 0f));

            CharacterController characterController = mechPreviewObject.GetComponent<CharacterController>();

            mechPreviewObject.transform.localPosition = new Vector3(0f, -characterController.bounds.extents.y, 2.5f);

            if (mechPaintScheme != null)
            {
                MechMetaController mechMetaController = mechPreviewObject.GetComponent<MechMetaController>();

                if (mechMetaController != null)
                {
                    mechMetaController.ApplyMechPaintScheme(mechPaintScheme);
                }
            }
        }
    }

    public void ClearPreviewMech()
    {
        if ((object)mechPreviewObject != null)
        {
            Destroy(mechPreviewObject);
            mechPreviewObject = null;
        }
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

        selectedMechData.customName = mechNameInputField.text;

        int index = MechsDropship.IndexOf(selectedMechData);

        mechDataListUI.BuildList(MechsDropship, SelectIndex);

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
        GlobalDataManager.Instance.backSceneName = "Dropship";
        GlobalDataManager.Instance.inventoryCurrent = GlobalDataManager.Instance.currentCareer.inventory;

        LoadingScreen.Instance.LoadScene("MechCustomizingScreen");
    }

    public void ClickSellButton()
    {
        AudioManager.Instance.PlayButtonClick(0);

        foreach (ComponentDefinition componentDefinition in selectedMechData.ComponentsSalvagable)
        {
            GlobalDataManager.Instance.currentCareer.inventory.AddComponent(componentDefinition);
        }

        GlobalDataManager.Instance.currentCareer.funds += Mathf.CeilToInt(selectedMechData.MarketValue * 0.75f);

        GlobalDataManager.Instance.currentCareer.RemoveMech(selectedMechData);

        UpdateMechList();
    }
}
