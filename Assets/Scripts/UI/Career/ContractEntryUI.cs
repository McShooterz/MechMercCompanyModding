using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ContractEntryUI : MonoBehaviour
{
    [SerializeField]
    ContractListUI contractListUI;

    [SerializeField]
    LayoutElement layoutElement;

    [SerializeField]
    Image background;

    [SerializeField]
    FactionLogoUI factionLogoUI;

    [SerializeField]
    Text nameText;

    [SerializeField]
    Text payText;

    [SerializeField]
    Text difficultyText;

    [SerializeField]
    int index;

    public LayoutElement LayoutElement { get => layoutElement; }

    public Image Background { get => background; }

    public Text NameText { get => nameText; }

    public void Initialize(ContractData contractData, int index)
    {
        this.index = index;

        Career career = GlobalDataManager.Instance.currentCareer;

        nameText.text = contractData.ContractDefinition.GetDisplayName();

        int missionPay = Mathf.CeilToInt(contractData.MissionPay * career.missionPayModifier);
        int contractPay = Mathf.CeilToInt(contractData.ContractPayPotential * career.contractPayModifier);

        payText.text = StaticHelper.FormatMoney(missionPay + contractPay);

        difficultyText.text = "Difficulty Estimate: " + (contractData.DifficultyEstimated + 1).ToString();

        factionLogoUI.SetFactionLogo(contractData.EmployerDefinition.FactionLogo);
    }










    public void ClickButton()
    {
        AudioManager.Instance.PlayButtonClick(0);

        contractListUI.SelectIndex(index);
    }
}
