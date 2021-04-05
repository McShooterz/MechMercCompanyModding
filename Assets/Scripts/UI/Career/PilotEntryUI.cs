using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PilotEntryUI : MonoBehaviour
{
    [SerializeField]
    PilotEntryListUI pilotEntryListUI;

    [SerializeField]
    Image background;

    [SerializeField]
    Image pilotIconImage;

    [SerializeField]
    Text pilotCallsignText;

    [SerializeField]
    Text pilotStatusText;

    [SerializeField]
    int index;

    public Image Background { get => background; }

    public void Initialize(MechPilot mechPilot, int index)
    {
        pilotCallsignText.text = mechPilot.displayName;

        pilotStatusText.text = mechPilot.PilotStatusDisplay;

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

        pilotEntryListUI.SelectIndex(index);
    }
}
