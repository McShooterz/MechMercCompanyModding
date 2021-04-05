using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PilotDraggableUI : MonoBehaviour
{
    [SerializeField]
    Image pilotIconImage;

    [SerializeField]
    Text pilotNameText;

    [SerializeField]
    Text pilotStatusText;

    public void SetPilot(MechPilot mechPilot)
    {
        Sprite pilotIconSprite = mechPilot.Icon;

        if (pilotIconSprite != null)
        {
            pilotIconImage.enabled = true;
            pilotIconImage.sprite = pilotIconSprite;
        }

        pilotNameText.text = mechPilot.displayName;

        pilotStatusText.text = mechPilot.PilotStatusDisplay;
    }
}
