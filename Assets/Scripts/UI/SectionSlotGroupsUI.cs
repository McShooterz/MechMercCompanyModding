using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SectionSlotGroupsUI : MonoBehaviour
{
    [SerializeField]
    UnitCustomizationScreen unitCustomizationScreen;

    [SerializeField]
    SectionHeaderOneSided sectionHeader;

    [SerializeField]
    GameObject slotGroupPrefab;

    [SerializeField]
    SlotGroupUI[] slotGroupUIs;

    [SerializeField]
    string sectionName;

    public SlotGroupUI[] SlotGroupUIs
    {
        get
        {
            return slotGroupUIs;
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

    public void Clear()
    {
        foreach(SlotGroupUI slotGroupUI in slotGroupUIs)
        {
            if (slotGroupUI.gameObject != null)
            {
                Destroy(slotGroupUI.gameObject);
            }
        }

        slotGroupUIs = null;
    }

    public void CreateSlotGroups(SlotGroup[] slotGroups)
    {
        slotGroupUIs = new SlotGroupUI[slotGroups.Length];

        for (int i = 0; i < slotGroups.Length; i++)
        {
            GameObject slotGroupObject = Instantiate(slotGroupPrefab, transform);
            slotGroupUIs[i] = slotGroupObject.GetComponent<SlotGroupUI>();
            if (slotGroupUIs[i])
            {
                slotGroupUIs[i].unitCustomizationScreen = unitCustomizationScreen;
                slotGroupUIs[i].sectionSlotGroupsUI = this;
                slotGroupUIs[i].SetSlots(slotGroups[i].SlotCount, ResourceManager.Instance.GameConstants.GetSlotColor(slotGroups[i].SlotType), slotGroups[i].SlotType, slotGroups[i].Hardpoint);
            }
            else
            {
                print("Error: slot group ui not found");
            }
        }
    }

    public float GetComponentsWeight()
    {
        float totalWeight = 0f;

        foreach (SlotGroupUI slotGroupUI in slotGroupUIs)
        {
            totalWeight += slotGroupUI.GetComponentsWeight();
        }

        return totalWeight;
    }

    public List<ComponentDefinition> GetComponentDefinitions()
    {
        List<ComponentDefinition> componentDefinitions = new List<ComponentDefinition>();

        foreach (SlotGroupUI slotGroupUI in slotGroupUIs)
        {
            componentDefinitions.AddRange(slotGroupUI.GetComponentDefinitions());
        }

        return componentDefinitions;
    }

    public List<WeaponDefinition> GetWeaponDefinitions()
    {
        List<WeaponDefinition> weaponDefinitions = new List<WeaponDefinition>();

        foreach (SlotGroupUI slotGroupUI in slotGroupUIs)
        {
            WeaponDefinition weaponDefinition = slotGroupUI.GetWeapon();

            if (weaponDefinition != null)
                weaponDefinitions.Add(weaponDefinition);
        }

        return weaponDefinitions;
    }

    public List<string> GetAmmoTypes()
    {
        List<string> ammoTypes = new List<string>();

        foreach (ComponentDefinition componentDefinition in GetComponentDefinitions())
        {
            if (componentDefinition.AmmoType != "" && componentDefinition.AmmoCount > 0)
                ammoTypes.Add(componentDefinition.AmmoType);
        }

        return ammoTypes;
    }

    public ComponentGroup[] GetComponentGroups()
    {
        ComponentGroup[] componentGroups = new ComponentGroup[slotGroupUIs.Length];

        for (int i = 0; i < componentGroups.Length; i++)
        {
            componentGroups[i] = slotGroupUIs[i].GetComponentGroup();
        }

        return componentGroups;
    }

    public void LoadComponentGroups(ComponentGroup[] componentGroups, bool doNotTakeFromInventory)
    {
        for (int i = 0; i < slotGroupUIs.Length && i < componentGroups.Length; i++)
        {
            slotGroupUIs[i].LoadComponentGroup(componentGroups[i], doNotTakeFromInventory);
        }
    }

    public void CreateMountedWeapons(List<GameObject> weaponMountObjects, Transform[] hardPoints)
    {
        foreach (SlotGroupUI slotGroupUI in slotGroupUIs)
        {
            if (slotGroupUI.HardPointIndex < 0 || slotGroupUI.HardPointIndex >= hardPoints.Length)
            {
                continue;
            }

            WeaponDefinition weaponDefinition = slotGroupUI.GetWeapon();

            if (weaponDefinition != null)
            {
                GameObject weaponMountPref = weaponDefinition.GetModelPrefab();

                if ((object)weaponMountPref != null)
                {
                    GameObject weaponMountInstance = Instantiate(weaponMountPref, hardPoints[slotGroupUI.HardPointIndex]);
                    weaponMountInstance.transform.localScale = weaponDefinition.ModelScale;
                    weaponMountObjects.Add(weaponMountInstance);
                }
                else
                {
                    print("Weapon model not found: " + weaponDefinition.ModelPrefab);
                }
            }
            else
            {
                EquipmentDefinition equipmentDefinition = slotGroupUI.GetEquipment();

                if (equipmentDefinition != null)
                {
                    GameObject prefab = equipmentDefinition.GetModelPrefab();

                    if ((object)prefab != null)
                    {
                        GameObject equipmentModelInstance = Instantiate(prefab, hardPoints[slotGroupUI.HardPointIndex]);
                        equipmentModelInstance.transform.localScale = equipmentDefinition.ModelScale;
                        weaponMountObjects.Add(equipmentModelInstance);
                    }
                }
            }
        }
    }

    public List<ComponentPlacedUI> GetComponentPlacedUIsWithWeapons()
    {
        List<ComponentPlacedUI> componentPlacedUIs = new List<ComponentPlacedUI>();

        foreach (SlotGroupUI slotGroupUI in slotGroupUIs)
        {
            componentPlacedUIs.AddRange(slotGroupUI.GetComponentPlacedUIsWithWeapons());
        }

        return componentPlacedUIs;
    }

    public string GetSectionName()
    {
        return ResourceManager.Instance.GetLocalization(sectionName);
    }

    public int GetAmmo(List<string> ammoTypes)
    {
        int ammoSum = 0;

        foreach (SlotGroupUI slotGroupUI in slotGroupUIs)
        {
            foreach (string ammoType in ammoTypes)
            {
                ammoSum += slotGroupUI.GetAmmo(ammoType);
            }
        }

        return ammoSum;
    }

    public void RemovePlacedComponents()
    {
        foreach(SlotGroupUI slotGroupUI in slotGroupUIs)
        {
            slotGroupUI.RemovePlacedComponents();
        }
    }

    public void CheckWeaponAmmoState()
    {
        if (!sectionHeader.HasAmmoIndicator)
            return;

        List<ProjectileWeaponDefinition> weaponsRequiringAmmo = new List<ProjectileWeaponDefinition>();
        List<EquipmentDefinition> equipmentRequiringAmmo = new List<EquipmentDefinition>();
        List<string> ammoTypes = new List<string>();

        // Get weapons and ammo
        foreach (SlotGroupUI slotGroupUI in slotGroupUIs)
        {
            foreach (ComponentPlacedUI componentPlacedUI in slotGroupUI.ComponentPlacedUIs)
            {
                ComponentDefinition componentDefinition = componentPlacedUI.componentDefinition;

                if (componentDefinition.ComponentType == ComponentType.Weapon)
                {
                    WeaponDefinition weaponDefinition = componentDefinition.GetWeaponDefinition();

                    if (weaponDefinition != null && weaponDefinition is ProjectileWeaponDefinition)
                    {
                        ProjectileWeaponDefinition projectileWeaponDefinition = weaponDefinition as ProjectileWeaponDefinition;

                        if (projectileWeaponDefinition.RequiresAmmo && !componentDefinition.AmmoInternal)
                        {
                            weaponsRequiringAmmo.Add(projectileWeaponDefinition);
                        }
                    }
                }
                else if (componentDefinition.ComponentType == ComponentType.Ammo)
                {
                    if (componentDefinition.AmmoCount > 0 && componentDefinition.AmmoType != "")
                    {
                        ammoTypes.Add(componentDefinition.AmmoType);
                    }
                }
                else if (componentDefinition.ComponentType == ComponentType.Equipment)
                {
                    EquipmentDefinition equpmentDefinition = componentDefinition.GetEquipmentDefinition();

                    if (equpmentDefinition != null && equpmentDefinition.AmmoType != "")
                    {
                        equipmentRequiringAmmo.Add(equpmentDefinition);
                    }
                }
            }
        }

        if (weaponsRequiringAmmo.Count == 0 && equipmentRequiringAmmo.Count == 0 && ammoTypes.Count == 0)
        {
            sectionHeader.SetAmmoIndicatorColor(new Color(0.7f, 0.7f, 0.7f, 0.7f));
            return;
        }

        // Check weapons for missing ammo
        bool ammoTypeFound;
        foreach (ProjectileWeaponDefinition projectileWeapon in weaponsRequiringAmmo)
        {
            ammoTypeFound = false;

            foreach (string weaponAmmoType in projectileWeapon.GetAmmoTypes())
            {
                foreach (string ammoType in ammoTypes)
                {
                    if (weaponAmmoType == ammoType)
                    {
                        ammoTypeFound = true;
                        break;
                    }
                }

                if (ammoTypeFound)
                {
                    break;
                }
            }

            if (!ammoTypeFound)
            {
                sectionHeader.SetAmmoIndicatorColor(new Color(1.0f, 0.0f, 0.0f, 1.0f));
                return;
            }
        }

        foreach (EquipmentDefinition equipment in equipmentRequiringAmmo)
        {
            ammoTypeFound = false;

            foreach (string ammoType in ammoTypes)
            {
                if (equipment.AmmoType == ammoType)
                {
                    ammoTypeFound = true;
                    break;
                }
            }

            if (!ammoTypeFound)
            {
                sectionHeader.SetAmmoIndicatorColor(new Color(1.0f, 0.0f, 0.0f, 1.0f));
                return;
            }
        }

        // Check for ammo not being used
        bool ammoUsed;
        foreach (string ammoType in ammoTypes)
        {
            ammoUsed = false;

            foreach (ProjectileWeaponDefinition projectileWeapon in weaponsRequiringAmmo)
            {
                foreach (string weaponAmmoType in projectileWeapon.GetAmmoTypes())
                {
                    if (weaponAmmoType == ammoType)
                    {
                        ammoUsed = true;
                        break;
                    }
                }

                if (ammoUsed)
                {
                    break;
                }
            }

            if (!ammoUsed)
            {
                foreach (EquipmentDefinition equipment in equipmentRequiringAmmo)
                {
                    if (equipment.AmmoType == ammoType)
                    {
                        ammoUsed = true;
                        break;
                    }
                }
            }

            if (!ammoUsed)
            {
                sectionHeader.SetAmmoIndicatorColor(new Color(1.0f, 1.0f, 0.0f, 1.0f));
                return;
            }
        }

        sectionHeader.SetAmmoIndicatorColor(new Color(0.0f, 1.0f, 0.0f, 1.0f));
    }
}
