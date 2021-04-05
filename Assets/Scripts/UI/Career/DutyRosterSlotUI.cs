using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DutyRosterSlotUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    DutyRosterSubScreen dutyRosterSubScreen;

    [SerializeField]
    DutyRosterPilotSlotUI dutyRosterPilotSlotUI;

    [SerializeField]
    DutyRosterMechSlotUI dutyRosterMechSlotUI;

    [SerializeField]
    Image pilotIconImage;

    [SerializeField]
    Text pilotText;

    [SerializeField]
    MechIconDamageUI mechIconDamageUI;

    [SerializeField]
    Text mechText;

    [SerializeField]
    PulseGraphicElement pilotBackgroundText;

    [SerializeField]
    PulseGraphicElement mechBackgroundText;

    [SerializeField]
    bool playerSlot = false;

    public DutyRosterMechSlotUI DutyRosterMechSlotUI { get => dutyRosterMechSlotUI; }

    public MechPilot MechPilot { get; private set; }

    public MechData MechData { get; private set; }

    public bool PlacePilot(MechPilot mechPilot)
    {
        if (mechPilot == null)
            return false;

        if (mechPilot.PilotStatus != PilotStatusType.Ready)
            return false;

        if (playerSlot)
            return false;

        if (MechPilot != null)
            return false;

        MechPilot = mechPilot;

        dutyRosterPilotSlotUI.gameObject.SetActive(true);

        Sprite pilotIconSprite = MechPilot.Icon;

        if (pilotIconSprite != null)
        {
            pilotIconImage.enabled = true;
            pilotIconImage.sprite = pilotIconSprite;
        }

        pilotText.text = MechPilot.displayName;

        if (MechData == null)
        {
            mechBackgroundText.TargetGraphicElement.color = Color.red;
            mechBackgroundText.Frequency = 5.0f;
        }
        else
        {
            mechBackgroundText.TargetGraphicElement.color = Color.white;
            mechBackgroundText.Frequency = 0.0f;
        }

        return true;
    }

    public bool PlaceMech(MechData mechData)
    {
        if (mechData == null)
        {
            dutyRosterMechSlotUI.gameObject.SetActive(false);
            MechData = null;
            return false;
        }

        if (MechData != null)
            return false;

        MechStatusType mechStatusType = mechData.MechStatus;

        if (mechStatusType == MechStatusType.Crippled || mechStatusType == MechStatusType.InvalidDesign || mechStatusType == MechStatusType.Repairing)
            return false;

        MechData = mechData;

        dutyRosterMechSlotUI.gameObject.SetActive(true);

        mechIconDamageUI.SetMech(MechData);

        mechText.text = MechData.customName;

        if (!playerSlot)
        {
            if (MechPilot == null)
            {
                pilotBackgroundText.TargetGraphicElement.color = Color.red;
                pilotBackgroundText.Frequency = 5.0f;
            }
            else
            {
                pilotBackgroundText.TargetGraphicElement.color = Color.white;
                pilotBackgroundText.Frequency = 0.0f;
            }
        }

        return true;
    }

    public void StartHoveredPilot()
    {
        dutyRosterSubScreen.SetHoveredPilot(MechPilot);
    }

    public void EndHoveredPilot()
    {
        dutyRosterSubScreen.SetHoveredPilot(null);
    }

    public void StartHoveredMech()
    {
        dutyRosterSubScreen.SetHoveredMechData(MechData);
    }

    public void EndHoveredMech()
    {
        dutyRosterSubScreen.SetHoveredMechData(null);
    }

    public void PickupPilot()
    {
        if (MechPilot != null)
        {
            dutyRosterPilotSlotUI.gameObject.SetActive(false);

            dutyRosterSubScreen.SelectPilot(MechPilot);

            MechPilot = null;

            if (!playerSlot)
            {
                if (MechData != null)
                {
                    pilotBackgroundText.TargetGraphicElement.color = Color.red;
                    pilotBackgroundText.Frequency = 5.0f;
                }
                else
                {                    
                    pilotBackgroundText.TargetGraphicElement.color = Color.white;
                    pilotBackgroundText.Frequency = 0.0f;
                    mechBackgroundText.TargetGraphicElement.color = Color.white;
                    mechBackgroundText.Frequency = 0.0f;
                }
            }
        }
    }

    public void PickupMech()
    {
        if (MechData != null)
        {
            dutyRosterMechSlotUI.gameObject.SetActive(false);

            dutyRosterSubScreen.SelectMech(MechData);

            MechData = null;

            if (!playerSlot)
            {
                if (MechPilot != null)
                {
                    mechBackgroundText.TargetGraphicElement.color = Color.red;
                    mechBackgroundText.Frequency = 5.0f;
                }
                else
                {

                    mechBackgroundText.TargetGraphicElement.color = Color.white;
                    mechBackgroundText.Frequency = 0.0f;
                    pilotBackgroundText.TargetGraphicElement.color = Color.white;
                    pilotBackgroundText.Frequency = 0.0f;
                }
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        dutyRosterSubScreen.CurrentDutyRosterSlotUI = this;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        dutyRosterSubScreen.CurrentDutyRosterSlotUI = null;
    }
}
