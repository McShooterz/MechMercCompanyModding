using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DutyRosterMechSlotUI : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    DutyRosterSlotUI dutyRosterSlotUI;

    public void OnPointerDown(PointerEventData eventData)
    {
        dutyRosterSlotUI.PickupMech();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        dutyRosterSlotUI.StartHoveredMech();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        dutyRosterSlotUI.EndHoveredMech();
    }
}
