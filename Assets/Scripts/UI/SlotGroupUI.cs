using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SlotGroupUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    GameObject firstSlot;

    [SerializeField]
    Image[] slots;

    [SerializeField]
    RectTransform[] slotRectTransforms;

    [SerializeField]
    SlotType slotType;

    [SerializeField]
    int hardPointIndex = -1;

    [SerializeField]
    GameObject componentPlacedPrefab;

    [SerializeField]
    List<ComponentPlacedUI> componentPlacedUIs = new List<ComponentPlacedUI>();

    public UnitCustomizationScreen unitCustomizationScreen;

    public SectionSlotGroupsUI sectionSlotGroupsUI;

    public List<ComponentPlacedUI> ComponentPlacedUIs { get => componentPlacedUIs; }

    public int HardPointIndex { get => hardPointIndex; }

    bool HasWeaponModification
    {
        get
        {
            foreach (ComponentPlacedUI componentPlaced in componentPlacedUIs)
            {
                if (componentPlaced.componentDefinition.HasWeaponModification)
                {
                    return true;
                }
            }

            return false;
        }
    }

    public void SetSlots(int count, Color color, SlotType type, int hardPoint)
    {
        slots = new Image[count];
        slotRectTransforms = new RectTransform[count];
        slots[0] = firstSlot.GetComponent<Image>();
        slots[0].color = color;
        slotRectTransforms[0] = firstSlot.GetComponent<RectTransform>();

        LayoutElement layoutElement = firstSlot.GetComponent<LayoutElement>();
        layoutElement.preferredWidth = unitCustomizationScreen.SlotSize.x;
        layoutElement.preferredHeight = unitCustomizationScreen.SlotSize.y;

        slotType = type;
        hardPointIndex = hardPoint;

        if (count > 1)
        {
            for(int i = 1; i < count; i++)
            {
                GameObject slotObject = Instantiate(firstSlot, transform);
                slots[i] = slotObject.GetComponent<Image>();
                slots[i].color = color;
                slotRectTransforms[i] = slotObject.GetComponent<RectTransform>();

                layoutElement = slotObject.GetComponent<LayoutElement>();
                layoutElement.preferredWidth = unitCustomizationScreen.SlotSize.x;
                layoutElement.preferredHeight = unitCustomizationScreen.SlotSize.y;
            }
        }
    }

    public void TryPlaceComponent(ComponentDefinition componentDefinition, WeaponGrouping weaponGrouping, bool giveWarning)
    {
        if (componentDefinition.SlotSize > GetAvailableSlots())
        {
            if (giveWarning)
            {
                unitCustomizationScreen.SetWarningMessage("NOT ENOUGH SLOTS!");
                AudioManager.Instance.PlayClipUI(ResourceManager.Instance.GetAudioClip("Cancel"));
            }

            unitCustomizationScreen.TempInventory.AddComponent(componentDefinition);
            unitCustomizationScreen.ComponentListUI.Refresh();

            return;
        }

        if (componentDefinition.RequiredSlotType != SlotType.General && componentDefinition.RequiredSlotType != slotType)
        {
            if (giveWarning)
            {
                unitCustomizationScreen.SetWarningMessage("SLOT TYPE INCOMPATIBLE!");
                AudioManager.Instance.PlayClipUI(ResourceManager.Instance.GetAudioClip("Cancel"));
            }

            unitCustomizationScreen.TempInventory.AddComponent(componentDefinition);
            unitCustomizationScreen.ComponentListUI.Refresh();

            return;
        }

        // Allow only one weapon
        if (componentDefinition.ComponentType == ComponentType.Weapon && HasWeapon())
        {
            if (giveWarning)
            {
                unitCustomizationScreen.SetWarningMessage("LIMIT ONE WEAPON TO SLOT GROUP!");
                AudioManager.Instance.PlayClipUI(ResourceManager.Instance.GetAudioClip("Cancel"));
            }

            unitCustomizationScreen.TempInventory.AddComponent(componentDefinition);
            unitCustomizationScreen.ComponentListUI.Refresh();

            return;
        }

        // Allow only one reactor
        if (componentDefinition.ComponentType == ComponentType.Reactor && HasEngine())
        {
            if (giveWarning)
            {
                unitCustomizationScreen.SetWarningMessage("LIMIT ONE REACTOR!");
                AudioManager.Instance.PlayClipUI(ResourceManager.Instance.GetAudioClip("Cancel"));
            }

            unitCustomizationScreen.TempInventory.AddComponent(componentDefinition);
            unitCustomizationScreen.ComponentListUI.Refresh();

            return;
        }

        // Allow only one electronics package
        if (componentDefinition.RequiredSlotType == SlotType.Electronics && HasElectronicsPackage())
        {
            if (giveWarning)
            {
                unitCustomizationScreen.SetWarningMessage("LIMIT ONE ELECTRONICS TO SLOT GROUP!");
                AudioManager.Instance.PlayClipUI(ResourceManager.Instance.GetAudioClip("Cancel"));
            }

            unitCustomizationScreen.TempInventory.AddComponent(componentDefinition);
            unitCustomizationScreen.ComponentListUI.Refresh();

            return;
        }

        // Allow only one missile defense weapon
        if (componentDefinition.HasEquipment() && HasMissileDefenseWeapon())
        {
            if (giveWarning)
            {
                unitCustomizationScreen.SetWarningMessage("LIMIT ONE POINT DEFENSE TO SLOT GROUP!");
                AudioManager.Instance.PlayClipUI(ResourceManager.Instance.GetAudioClip("Cancel"));
            }

            unitCustomizationScreen.TempInventory.AddComponent(componentDefinition);
            unitCustomizationScreen.ComponentListUI.Refresh();

            return;
        }

        if (componentDefinition.HasWeaponModification && HasWeaponModification)
        {
            if (giveWarning)
            {
                unitCustomizationScreen.SetWarningMessage("LIMIT ONE WEAPON MODIFICATION TO SLOT GROUP!");
                AudioManager.Instance.PlayClipUI(ResourceManager.Instance.GetAudioClip("Cancel"));
            }

            unitCustomizationScreen.TempInventory.AddComponent(componentDefinition);
            unitCustomizationScreen.ComponentListUI.Refresh();

            return;
        }

        PlaceComponent(componentDefinition, weaponGrouping, giveWarning);

        if (componentDefinition.HasWeapon() || componentDefinition.HasEquipment())
        {
            unitCustomizationScreen.RemountWeapons();
        }

        if (componentDefinition.InternalBonus != 0f)
        {
            unitCustomizationScreen.RecalculateInternals();
        }

        if (componentDefinition.ComponentType == ComponentType.Weapon)
        {
            WeaponDefinition weaponDefinition = componentDefinition.GetWeaponDefinition();

            if (weaponDefinition != null && weaponDefinition is ProjectileWeaponDefinition)
            {
                ProjectileWeaponDefinition projectileWeaponDefinition = weaponDefinition as ProjectileWeaponDefinition;

                if (projectileWeaponDefinition.RequiresAmmo && !componentDefinition.AmmoInternal)
                {
                    sectionSlotGroupsUI.CheckWeaponAmmoState();
                }
            }
        }
        else if (componentDefinition.ComponentType == ComponentType.Ammo)
        {
            if (componentDefinition.AmmoType != "" && componentDefinition.AmmoCount > 0)
            {
                sectionSlotGroupsUI.CheckWeaponAmmoState();
            }
        }
        else if (componentDefinition.ComponentType == ComponentType.Equipment)
        {
            EquipmentDefinition equipmentDefinition = componentDefinition.GetEquipmentDefinition();

            if (equipmentDefinition != null && equipmentDefinition.AmmoType != "")
            {
                sectionSlotGroupsUI.CheckWeaponAmmoState();
            }
        }

        unitCustomizationScreen.RecalculateComponents();
    }

    void PlaceComponent(ComponentDefinition componentDefinition, WeaponGrouping weaponGrouping, bool playSound)
    {
        GameObject componentPlacedObject = Instantiate(componentPlacedPrefab, slots[0].transform);

        ComponentPlacedUI componentPlacedUI = componentPlacedObject.GetComponent<ComponentPlacedUI>();

        if (componentPlacedUI)
        {
            if (playSound)
            {
                AudioManager.Instance.PlayClipUI(ResourceManager.Instance.GetAudioClip("MountComponent"));
            }

            componentPlacedUI.slotGroupUI = this;
            componentPlacedObject.GetComponent<RectTransform>().sizeDelta = unitCustomizationScreen.ComponentPlacedSize;
            componentPlacedUI.ComponentNameText.GetComponent<RectTransform>().sizeDelta = unitCustomizationScreen.ComponentPlacedSize;
            componentPlacedUI.ComponentNameText.text = componentDefinition.GetDisplayName();
            componentPlacedUI.ComponentNameText.color = componentDefinition.TextColor;
            componentPlacedUI.BackgroundImage.color = componentDefinition.Color;
            componentPlacedUI.componentDefinition = componentDefinition;
            //componentPlacedUI.BackgroundRectTransform.sizeDelta = new Vector2(267, (componentDefinition.SlotSize - 1) * 26f + 20f);
            componentPlacedUI.BackgroundRectTransform.sizeDelta = new Vector2(unitCustomizationScreen.ComponentPlacedSize.x, (componentDefinition.SlotSize - 1) * (unitCustomizationScreen.SlotSize.y) + unitCustomizationScreen.ComponentPlacedSize.y);
            componentPlacedUIs.Add(componentPlacedUI);
            FitPlacedComponents();

            WeaponDefinition weaponDefinition = componentDefinition.GetWeaponDefinition();

            if (weaponDefinition != null)
            {
                if (weaponGrouping != null)
                {
                    componentPlacedUI.weaponGrouping.WeaponGroup1 = weaponGrouping.WeaponGroup1;
                    componentPlacedUI.weaponGrouping.WeaponGroup2 = weaponGrouping.WeaponGroup2;
                    componentPlacedUI.weaponGrouping.WeaponGroup3 = weaponGrouping.WeaponGroup3;
                    componentPlacedUI.weaponGrouping.WeaponGroup4 = weaponGrouping.WeaponGroup4;
                    componentPlacedUI.weaponGrouping.WeaponGroup5 = weaponGrouping.WeaponGroup5;
                    componentPlacedUI.weaponGrouping.WeaponGroup6 = weaponGrouping.WeaponGroup6;
                }      
                else if (weaponDefinition.DefaultWeaponGrouping != null)
                {
                    componentPlacedUI.weaponGrouping.WeaponGroup1 = weaponDefinition.DefaultWeaponGrouping.WeaponGroup1;
                    componentPlacedUI.weaponGrouping.WeaponGroup2 = weaponDefinition.DefaultWeaponGrouping.WeaponGroup2;
                    componentPlacedUI.weaponGrouping.WeaponGroup3 = weaponDefinition.DefaultWeaponGrouping.WeaponGroup3;
                    componentPlacedUI.weaponGrouping.WeaponGroup4 = weaponDefinition.DefaultWeaponGrouping.WeaponGroup4;
                    componentPlacedUI.weaponGrouping.WeaponGroup5 = weaponDefinition.DefaultWeaponGrouping.WeaponGroup5;
                    componentPlacedUI.weaponGrouping.WeaponGroup6 = weaponDefinition.DefaultWeaponGrouping.WeaponGroup6;
                }
            }
        }
        else if (componentPlacedObject != null)
        {
            Destroy(componentPlacedObject);
        }
    }

    public void RemoveComponent(ComponentPlacedUI componentPlacedUI)
    {
        componentPlacedUIs.Remove(componentPlacedUI);

        if (componentPlacedUI.componentDefinition.HasWeapon() || componentPlacedUI.componentDefinition.HasEquipment())
        {
            unitCustomizationScreen.RemountWeapons();
        }

        if (componentPlacedUI.componentDefinition.ComponentType == ComponentType.Weapon)
        {
            WeaponDefinition weaponDefinition = componentPlacedUI.componentDefinition.GetWeaponDefinition();

            if (weaponDefinition != null && weaponDefinition is ProjectileWeaponDefinition)
            {
                ProjectileWeaponDefinition projectileWeaponDefinition = weaponDefinition as ProjectileWeaponDefinition;

                if (projectileWeaponDefinition.RequiresAmmo && !componentPlacedUI.componentDefinition.AmmoInternal)
                {
                    sectionSlotGroupsUI.CheckWeaponAmmoState();
                }
            }
        }
        else if (componentPlacedUI.componentDefinition.ComponentType == ComponentType.Ammo)
        {
            if (componentPlacedUI.componentDefinition.AmmoType != "" && componentPlacedUI.componentDefinition.AmmoCount > 0)
            {
                sectionSlotGroupsUI.CheckWeaponAmmoState();
            }
        }
        else if (componentPlacedUI.componentDefinition.ComponentType == ComponentType.Equipment)
        {
            EquipmentDefinition equipmentDefinition = componentPlacedUI.componentDefinition.GetEquipmentDefinition();

            if (equipmentDefinition != null && equipmentDefinition.AmmoType != "")
            {
                sectionSlotGroupsUI.CheckWeaponAmmoState();
            }
        }

        Destroy(componentPlacedUI.gameObject);

        FitPlacedComponents();

        unitCustomizationScreen.RecalculateComponents();
        unitCustomizationScreen.RecalculateInternals();
    }

    bool HasWeapon()
    {
        foreach(ComponentPlacedUI componentPlaced in componentPlacedUIs)
        {
            if (componentPlaced.componentDefinition.ComponentType == ComponentType.Weapon)
            {
                return true;
            }
        }

        return false;
    }

    bool HasEngine()
    {
        foreach (ComponentPlacedUI componentPlaced in componentPlacedUIs)
        {
            if (componentPlaced.componentDefinition.ComponentType == ComponentType.Reactor)
            {
                return true;
            }
        }

        return false;
    }

    bool HasElectronicsPackage()
    {
        foreach (ComponentPlacedUI componentPlaced in componentPlacedUIs)
        {
            if (componentPlaced.componentDefinition.RequiredSlotType == SlotType.Electronics)
            {
                return true;
            }
        }

        return false;
    }

    bool HasMissileDefenseWeapon()
    {
        foreach (ComponentPlacedUI componentPlaced in componentPlacedUIs)
        {
            if (componentPlaced.componentDefinition.HasEquipment())
            {
                return true;
            }
        }

        return false;
    }

    public WeaponDefinition GetWeapon()
    {
        foreach (ComponentPlacedUI componentPlaced in componentPlacedUIs)
        {
            WeaponDefinition weaponDefinition = componentPlaced.componentDefinition.GetWeaponDefinition();

            if (weaponDefinition != null)
            {
                return weaponDefinition;
            }
        }

        return null;
    }

    public EquipmentDefinition GetEquipment()
    {
        foreach (ComponentPlacedUI componentPlaced in componentPlacedUIs)
        {
            EquipmentDefinition equipmentDefinition = componentPlaced.componentDefinition.GetEquipmentDefinition();

            if (equipmentDefinition != null)
            {
                return equipmentDefinition;
            }
        }

        return null;
    }

    public List<ComponentDefinition> GetComponentDefinitions()
    {
        List<ComponentDefinition> componentDefinitions = new List<ComponentDefinition>();

        foreach (ComponentPlacedUI componentPlaced in componentPlacedUIs)
        {
            componentDefinitions.Add(componentPlaced.componentDefinition);
        }

        return componentDefinitions;
    }

    public ComponentGroup GetComponentGroup()
    {
        ComponentGroup componentGroup = new ComponentGroup();
        componentGroup.ComponentsSaved = new ComponentSaved[componentPlacedUIs.Count];

        for(int i = 0; i < componentPlacedUIs.Count; i++)
        {
            componentGroup.ComponentsSaved[i] = componentPlacedUIs[i].GetComponentSaved();
        }

        return componentGroup;
    }

    public void LoadComponentGroup(ComponentGroup componentGroup, bool doNotTakeFromInventory)
    {
        foreach (ComponentSaved componentSaved in componentGroup.ComponentsSaved)
        {
            ComponentDefinition componentDefinition = componentSaved.GetComponentDefinition();

            if (componentDefinition != null)
            {
                if (doNotTakeFromInventory || unitCustomizationScreen.TempInventory.TakeComponent(componentDefinition))
                {
                    TryPlaceComponent(componentDefinition, componentSaved.WeaponGrouping, false);
                }
            }
        }
    }

    int GetAvailableSlots()
    {
        int usedSlots = 0;

        foreach(ComponentPlacedUI componentPlaced in componentPlacedUIs)
        {
            usedSlots += componentPlaced.componentDefinition.SlotSize;
        }

        return slots.Length - usedSlots;
    }

    void FitPlacedComponents()
    {
        int index = -1;
        float offSetY;

        foreach (ComponentPlacedUI componentPlacedUI in componentPlacedUIs)
        {
            index += componentPlacedUI.componentDefinition.SlotSize;

            if (index < slots.Length)
            {
                offSetY = (componentPlacedUI.componentDefinition.SlotSize - 1) * (unitCustomizationScreen.SlotSize.y / 2f);
                componentPlacedUI.transform.SetParent(slots[index].transform);
                componentPlacedUI.transform.localPosition = new Vector3(0, offSetY, 0);
            }
        }
    }

    public float GetComponentsWeight()
    {
        float totalWeight = 0f;

        foreach (ComponentPlacedUI componentPlacedUI in ComponentPlacedUIs)
        {
            totalWeight += componentPlacedUI.componentDefinition.Weight;
        }

        return totalWeight;
    }

    public List<ComponentPlacedUI> GetComponentPlacedUIsWithWeapons()
    {
        List<ComponentPlacedUI> componentPlacedUIsWithWeapons = new List<ComponentPlacedUI>();

        foreach (ComponentPlacedUI componentPlacedUI in componentPlacedUIs)
        {
            if (componentPlacedUI.componentDefinition.HasWeapon())
            {
                componentPlacedUIsWithWeapons.Add(componentPlacedUI);
            }
        }

        return componentPlacedUIsWithWeapons;
    }

    public int GetAmmo(string ammoType)
    {
        int ammoSum = 0;

        foreach (ComponentPlacedUI componentPlacedUI in componentPlacedUIs)
        {
            if (componentPlacedUI.componentDefinition.AmmoType == ammoType)
            {
                ammoSum += componentPlacedUI.componentDefinition.AmmoCount;
            }
        }

        return ammoSum;
    }

    public void RemovePlacedComponents()
    {
        foreach(ComponentPlacedUI componentPlacedUI in componentPlacedUIs)
        {
            unitCustomizationScreen.TempInventory.AddComponent(componentPlacedUI.componentDefinition);
            Destroy(componentPlacedUI.gameObject);
        }

        componentPlacedUIs.Clear();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        unitCustomizationScreen.SetCurrentSlotGroupUI(this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        unitCustomizationScreen.ClearCurrentSlotGroupUI();
    }
}
