using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ComponentPlacedUI : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    RectTransform backgroundRectTransform;

    [SerializeField]
    Image backgroundImage;

    [SerializeField]
    Text componentNameText;

    public SlotGroupUI slotGroupUI;

    public ComponentDefinition componentDefinition;

    public WeaponGrouping weaponGrouping = new WeaponGrouping();

    public RectTransform BackgroundRectTransform
    {
        get
        {
            return backgroundRectTransform;
        }
    }
    public Image BackgroundImage
    {
        get
        {
            return backgroundImage;
        }
    }
    public Text ComponentNameText
    {
        get
        {
            return componentNameText;
        }
    }

    // Use this for initialization
    void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public ComponentSaved GetComponentSaved()
    {
        ComponentSaved componentSaved = new ComponentSaved();

        componentSaved.ComponentDefinition = componentDefinition.Key;

        if (componentDefinition.HasWeapon())
        {
            componentSaved.WeaponGrouping = new WeaponGrouping();
            componentSaved.WeaponGrouping.WeaponGroup1 = weaponGrouping.WeaponGroup1;
            componentSaved.WeaponGrouping.WeaponGroup2 = weaponGrouping.WeaponGroup2;
            componentSaved.WeaponGrouping.WeaponGroup3 = weaponGrouping.WeaponGroup3;
            componentSaved.WeaponGrouping.WeaponGroup4 = weaponGrouping.WeaponGroup4;
            componentSaved.WeaponGrouping.WeaponGroup5 = weaponGrouping.WeaponGroup5;
            componentSaved.WeaponGrouping.WeaponGroup6 = weaponGrouping.WeaponGroup6;
        }

        return componentSaved;
    }

    public string GetSectionName()
    {
        return slotGroupUI.sectionSlotGroupsUI.GetSectionName();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        slotGroupUI.unitCustomizationScreen.PickupPlacedComponent(componentDefinition);
        slotGroupUI.RemoveComponent(this);
        slotGroupUI.unitCustomizationScreen.RecalculateComponents();
        slotGroupUI.unitCustomizationScreen.UpdateArmorButtons();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        slotGroupUI.unitCustomizationScreen.SetHovedComponent(componentDefinition);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        slotGroupUI.unitCustomizationScreen.SetHovedComponent(null);
    }
}
