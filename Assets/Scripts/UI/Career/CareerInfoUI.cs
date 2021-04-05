using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CareerInfoUI : MonoBehaviour
{
    [SerializeField]
    GameObject parentScreen;

    [SerializeField]
    CareerPauseMenu careerPauseMenu;

    [SerializeField]
    Image playerPilotImage;

    [SerializeField]
    FactionLogoUI factionLogoUI;

    [SerializeField]
    Text careerInfoText;

    [SerializeField]
    Button skipWeekButton;

    void OnEnable()
    {
        Career career = GlobalDataManager.Instance.currentCareer;

        Sprite pilotSprite = career.PlayerPilotSprite;

        if (pilotSprite != null)
        {
            playerPilotImage.enabled = true;
            playerPilotImage.sprite = pilotSprite;
        }
        else
        {
            playerPilotImage.enabled = false;
        }

        factionLogoUI.SetFactionLogo(career.factionLogo);

        skipWeekButton.interactable = career.funds >= 0;

        UpdateText();
    }

    void UpdateText()
    {
        System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
        Career career = GlobalDataManager.Instance.currentCareer;
        ContractData contractData = career.currentContract;

        stringBuilder.AppendLine(career.companyName);

        if (contractData != null)
        {
            stringBuilder.AppendLine("Contract: " + contractData.ContractDefinition.GetDisplayName());
        }
        else
        {
            stringBuilder.AppendLine("<color=red>No Contract!</color>");
        }

        stringBuilder.AppendLine(career.gameDate.Display);

        stringBuilder.AppendLine(StaticHelper.FormatMoney(career.funds));

        stringBuilder.AppendLine("Reputation: " + career.reputation);

        stringBuilder.AppendLine("Infamy: " + career.infamy);

        careerInfoText.text = stringBuilder.ToString();
    }

    public void ClickButtonSkipWeek()
    {
        AudioManager.Instance.PlayButtonClick(0);

        Career career = GlobalDataManager.Instance.currentCareer;

        career.AdvanceWeek();

        skipWeekButton.interactable = career.funds >= 0;

        UpdateText();
    }

    public void ClickButtonMenu()
    {
        AudioManager.Instance.PlayButtonClick(0);

        parentScreen.gameObject.SetActive(false);

        careerPauseMenu.gameObject.SetActive(true);
    }
}
