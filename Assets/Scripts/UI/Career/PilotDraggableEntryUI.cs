using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PilotDraggableEntryUI : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    Image pilotIconImage;

    [SerializeField]
    Text pilotNameText;

    [SerializeField]
    Text pilotStatusText;

    public delegate void CallBackSelect(MechPilot mechPilot);
    public CallBackSelect callBackSelect;

    public delegate void CallBackHover(MechPilot mechPilot);
    public CallBackHover callBackHover;

    public MechPilot MechPilot { get; private set; } = null;

    public void Initialize(MechPilot mechPilot, CallBackSelect callBack, CallBackHover callBackHover)
    {
        MechPilot = mechPilot;

        callBackSelect = callBack;

        this.callBackHover = callBackHover;

        pilotNameText.text = MechPilot.displayName;

        pilotStatusText.text = MechPilot.PilotStatusDisplay;

        Sprite pilotIconSprite = MechPilot.Icon;

        if (pilotIconSprite != null)
        {
            pilotIconImage.enabled = true;
            pilotIconImage.sprite = pilotIconSprite;
        }
        else
        {
            pilotIconImage.enabled = false;
        }
    }

    public void Clear()
    {
        MechPilot = null;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        gameObject.SetActive(false);

        callBackSelect(MechPilot);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        callBackHover?.Invoke(MechPilot);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        callBackHover?.Invoke(null);
    }
}
