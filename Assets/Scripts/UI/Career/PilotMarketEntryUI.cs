using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PilotMarketEntryUI : MonoBehaviour
{
    [SerializeField]
    PilotMarketEntryListUI pilotMarketEntryListUI;

    [SerializeField]
    Image background;

    [SerializeField]
    Image pilotIconImage;

    [SerializeField]
    Text pilotCallsignText;

    [SerializeField]
    Text pilotContractText;

    [SerializeField]
    int index;

    public Image Background { get => background; }

    public void Initialize(MechPilot mechPilot, int index)
    {
        pilotCallsignText.text = mechPilot.displayName;

        pilotContractText.text = StaticHelper.FormatMoney(mechPilot.contractValue);

        if (mechPilot.Icon != null)
        {
            pilotIconImage.enabled = true;
            pilotIconImage.sprite = mechPilot.Icon;
        }
        else
        {
            pilotIconImage.enabled = false;
        }

        this.index = index;
    }

    public void ClickButton()
    {
        AudioManager.Instance.PlayButtonClick(0);

        pilotMarketEntryListUI.SelectIndex(index);
    }
}
