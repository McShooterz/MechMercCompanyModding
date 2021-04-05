using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DutyRosterPilotSlotUI : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    DutyRosterSlotUI dutyRosterSlotUI;

    public void OnPointerDown(PointerEventData eventData)
    {
        dutyRosterSlotUI.PickupPilot();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        dutyRosterSlotUI.StartHoveredPilot();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        dutyRosterSlotUI.EndHoveredPilot();
    }
}
