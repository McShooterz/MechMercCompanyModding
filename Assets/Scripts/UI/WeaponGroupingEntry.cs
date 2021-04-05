using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponGroupingEntry : MonoBehaviour
{
    [SerializeField]
    Text weaponNameText;

    [SerializeField]
    Text sectionText;

    [SerializeField]
    Text ammoText;

    [SerializeField]
    Image[] groupButtonFills;

    [SerializeField]
    ComponentPlacedUI componentPlacedUI;

    // Use this for initialization
    void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void SetComponentPlacedUI(ComponentPlacedUI component)
    {
        componentPlacedUI = component;
        WeaponDefinition weaponDefinition = componentPlacedUI.componentDefinition.GetWeaponDefinition();
        weaponNameText.text = weaponDefinition.GetDisplayName();
        sectionText.text = componentPlacedUI.GetSectionName();

        if (weaponDefinition is BeamWeaponDefinition)
        {
            ammoText.text = "-";
        }
        else if (weaponDefinition is ProjectileWeaponDefinition)
        {
            ProjectileWeaponDefinition projectileWeaponDefinition = weaponDefinition as ProjectileWeaponDefinition;

            if (projectileWeaponDefinition.RequiresAmmo)
            {
                ammoText.text = componentPlacedUI.slotGroupUI.sectionSlotGroupsUI.GetAmmo(projectileWeaponDefinition.GetAmmoTypes()).ToString();
            }
            else
            {
                ammoText.text = "-";
            }
        }

        SetGroupButtonFill(0, componentPlacedUI.weaponGrouping.WeaponGroup1);
        SetGroupButtonFill(1, componentPlacedUI.weaponGrouping.WeaponGroup2);
        SetGroupButtonFill(2, componentPlacedUI.weaponGrouping.WeaponGroup3);
        SetGroupButtonFill(3, componentPlacedUI.weaponGrouping.WeaponGroup4);
        SetGroupButtonFill(4, componentPlacedUI.weaponGrouping.WeaponGroup5);
        SetGroupButtonFill(5, componentPlacedUI.weaponGrouping.WeaponGroup6);
    }

    void ToggleGroupButtonFill(int index)
    {
        groupButtonFills[index].gameObject.SetActive(!groupButtonFills[index].gameObject.activeInHierarchy);
    }

    void SetGroupButtonFill(int index, bool state)
    {
        groupButtonFills[index].gameObject.SetActive(state);
    }

    public void ClickGroup1Button()
    {
        ToggleGroupButtonFill(0);

        componentPlacedUI.weaponGrouping.WeaponGroup1 = !componentPlacedUI.weaponGrouping.WeaponGroup1;
    }

    public void ClickGroup2Button()
    {
        ToggleGroupButtonFill(1);

        componentPlacedUI.weaponGrouping.WeaponGroup2 = !componentPlacedUI.weaponGrouping.WeaponGroup2;
    }

    public void ClickGroup3Button()
    {
        ToggleGroupButtonFill(2);

        componentPlacedUI.weaponGrouping.WeaponGroup3 = !componentPlacedUI.weaponGrouping.WeaponGroup3;
    }

    public void ClickGroup4Button()
    {
        ToggleGroupButtonFill(3);

        componentPlacedUI.weaponGrouping.WeaponGroup4 = !componentPlacedUI.weaponGrouping.WeaponGroup4;
    }

    public void ClickGroup5Button()
    {
        ToggleGroupButtonFill(4);

        componentPlacedUI.weaponGrouping.WeaponGroup5 = !componentPlacedUI.weaponGrouping.WeaponGroup5;
    }

    public void ClickGroup6Button()
    {
        ToggleGroupButtonFill(5);

        componentPlacedUI.weaponGrouping.WeaponGroup6 = !componentPlacedUI.weaponGrouping.WeaponGroup6;
    }
}
