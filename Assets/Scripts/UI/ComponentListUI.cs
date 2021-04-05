using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ComponentListUI : MonoBehaviour
{
    [SerializeField]
    ScrollRect scrollRect;

    [SerializeField]
    Transform contentTransform;

    [SerializeField]
    GameObject componentItemPrefab;

    [SerializeField]
    GameObject componentSetItemPrefab;

    [SerializeField]
    List<ComponentMenuItem> componentMenuItems;

    [SerializeField]
    List<ComponentSetMenuItem> componentSetMenuItems;

    [SerializeField]
    List<ComponentSet> weaponComponentSets = new List<ComponentSet>();

    [SerializeField]
    List<ComponentSet> ammoComponentSets = new List<ComponentSet>();

    [SerializeField]
    List<ComponentSet> equipmentComponentSets = new List<ComponentSet>();

    [SerializeField]
    List<ComponentSet> reactorComponentSets = new List<ComponentSet>();

    [SerializeField]
    Image weaponButtonBackground;

    [SerializeField]
    Image ammoButtonBackground;

    [SerializeField]
    Image equipmentButtonBackground;

    [SerializeField]
    Image reactorButtonBackground;

    [SerializeField]
    Color activeColor;

    [SerializeField]
    float itemWidth = 396f;

    [SerializeField]
    float itemHeight = 30f;

    [SerializeField]
    float itemSetHeight = 35f;

    [SerializeField]
    ComponentType selectedComponentType;

    [SerializeField]
    public bool HasBeenSetup { get; private set; }

    [SerializeField]
    bool marketMode = false;

    Inventory targetInventory;

    public delegate void CallBackSelect(ComponentDefinition componentDefinition);
    public CallBackSelect callBackSelect;

    public delegate void CallBackHover(ComponentDefinition componentDefinition);
    public CallBackHover callBackHover;

    public void Setup(Inventory inventory, CallBackSelect select, CallBackHover hover)
    {
        HasBeenSetup = true;

        itemHeight = itemHeight / 1080 * Screen.height;
        itemSetHeight = itemSetHeight / 1080 * Screen.height;

        CalculateItemWidth();

        targetInventory = inventory;

        callBackSelect = select;

        callBackHover = hover;

        ComponentSet[] componentSets = ResourceManager.Instance.GetComponentSets();

        foreach (ComponentSet componentSet in componentSets)
        {
            switch (componentSet.ComponentType)
            {
                case ComponentType.Weapon:
                    {
                        weaponComponentSets.Add(componentSet);
                        break;
                    }
                case ComponentType.Ammo:
                    {
                        ammoComponentSets.Add(componentSet);
                        break;
                    }
                case ComponentType.Equipment:
                    {
                        equipmentComponentSets.Add(componentSet);
                        break;
                    }
                case ComponentType.Reactor:
                    {
                        reactorComponentSets.Add(componentSet);
                        break;
                    }
            }
        }

        BuildWeaponsList();
    }

    void ResizeComponentList(int count)
    {
        while (componentMenuItems.Count < count)
        {
            ComponentMenuItem componentMenuItem = Instantiate(componentItemPrefab, contentTransform).GetComponent<ComponentMenuItem>();
            componentMenuItem.LayoutElement.preferredWidth = itemWidth;
            componentMenuItem.LayoutElement.preferredHeight = itemHeight;
            componentMenuItem.TargetInventory = targetInventory;

            componentMenuItems.Add(componentMenuItem);
        }

        for (int i = 0; i < componentMenuItems.Count; i++)
        {
            componentMenuItems[i].gameObject.SetActive(i < count);
        }
    }

    void ResizeComponentSetList(int count)
    {
        while (componentSetMenuItems.Count < count)
        {
            ComponentSetMenuItem componentSetMenuItem = Instantiate(componentSetItemPrefab, contentTransform).GetComponent<ComponentSetMenuItem>();
            componentSetMenuItem.LayoutElement.preferredWidth = itemWidth;
            componentSetMenuItem.LayoutElement.preferredHeight = itemSetHeight;

            componentSetMenuItems.Add(componentSetMenuItem);
        }

        for (int i = 0; i < componentSetMenuItems.Count; i++)
        {
            componentSetMenuItems[i].gameObject.SetActive(i < count);
        }
    }

    public void BuildWeaponsList()
    {
        selectedComponentType = ComponentType.Weapon;

        BuildComponentList(targetInventory.GetComponents(ComponentType.Weapon), weaponComponentSets);

        StartCoroutine(ResetScrollPos());

        weaponButtonBackground.color = activeColor;
        ammoButtonBackground.color = Color.white;
        equipmentButtonBackground.color = Color.white;
        reactorButtonBackground.color = Color.white;
    }

    void BuildAmmoList()
    {
        selectedComponentType = ComponentType.Ammo;

        BuildComponentList(targetInventory.GetComponents(ComponentType.Ammo), ammoComponentSets);

        StartCoroutine(ResetScrollPos());

        weaponButtonBackground.color = Color.white;
        ammoButtonBackground.color = activeColor;
        equipmentButtonBackground.color = Color.white;
        reactorButtonBackground.color = Color.white;
    }

    void BuildEquipmentList()
    {
        selectedComponentType = ComponentType.Equipment;

        BuildComponentList(targetInventory.GetComponents(ComponentType.Equipment), equipmentComponentSets);

        StartCoroutine(ResetScrollPos());

        weaponButtonBackground.color = Color.white;
        ammoButtonBackground.color = Color.white;
        equipmentButtonBackground.color = activeColor;
        reactorButtonBackground.color = Color.white;
    }

    void BuildReactorsList()
    {
        selectedComponentType = ComponentType.Reactor;

        BuildComponentList(targetInventory.GetComponents(ComponentType.Reactor), reactorComponentSets);

        StartCoroutine(ResetScrollPos());

        weaponButtonBackground.color = Color.white;
        ammoButtonBackground.color = Color.white;
        equipmentButtonBackground.color = Color.white;
        reactorButtonBackground.color = activeColor;
    }

    void BuildComponentList(List<KeyValuePair<ComponentDefinition, int>> components, List<ComponentSet> componentSets)
    {
        // Get open component sets
        List<ComponentSet> openComponentSets = new List<ComponentSet>();
        foreach (ComponentSetMenuItem componentSetMenuItem in componentSetMenuItems)
        {
            if (componentSetMenuItem.gameObject.activeInHierarchy && !componentSetMenuItem.IsCallapsed)
            {
                openComponentSets.Add(componentSetMenuItem.ComponentSet);
            }
        }

        List<ComponentSet> usedComponentSets = new List<ComponentSet>();
        bool componentSetFound;

        ResizeComponentList(components.Count);

        for (int i = 0; i < components.Count; i++)
        {
            if (marketMode)
            {
                componentMenuItems[i].InitializeMarket(components[i], SelectComponent, HoverComponent);
            }
            else
            {
                componentMenuItems[i].Initialize(components[i], SelectComponent, HoverComponent);
            }
        }

        foreach (ComponentSet componentSet in componentSets)
        {
            componentSetFound = false;

            foreach (ComponentDefinition componentDefinition in componentSet.ComponentDefinitions)
            {
                foreach (KeyValuePair<ComponentDefinition, int> keyValuePair in components)
                {
                    if (componentDefinition == keyValuePair.Key)
                    {
                        usedComponentSets.Add(componentSet);
                        componentSetFound = true;
                        break;
                    }
                }

                if (componentSetFound)
                    break;
            }
        }

        ResizeComponentSetList(usedComponentSets.Count);

        for (int i = 0; i < usedComponentSets.Count; i++)
        {
            componentSetMenuItems[i].SetComponentSet(usedComponentSets[i]);
        }

        // Assign components to sets
        List<ComponentMenuItem> componentMenuItemsOfSet = new List<ComponentMenuItem>();
        foreach (ComponentSetMenuItem componentSetMenuItem in componentSetMenuItems)
        {
            componentMenuItemsOfSet.Clear();

            foreach (ComponentDefinition componentDefinition in componentSetMenuItem.ComponentSet.ComponentDefinitions)
            {
                foreach (ComponentMenuItem componentMenuItem in componentMenuItems)
                {
                    if (!componentMenuItem.gameObject.activeInHierarchy)
                        break;

                    if (componentMenuItem.ComponentDefinition == componentDefinition)
                    {
                        componentMenuItemsOfSet.Add(componentMenuItem);
                    }
                }
            }

            componentSetMenuItem.SetComponentMenuItems(componentMenuItemsOfSet.ToArray());
        }

        // Sort everything
        int index = 0;
        Transform swapTarget;

        foreach (ComponentSetMenuItem componentSetMenuItem in componentSetMenuItems)
        {
            if (componentSetMenuItem.gameObject.activeInHierarchy)
            {
                swapTarget = contentTransform.GetChild(index);

                if (componentSetMenuItem.transform != swapTarget)
                {
                    swapTarget.SetSiblingIndex(componentSetMenuItem.transform.GetSiblingIndex());
                    componentSetMenuItem.transform.SetSiblingIndex(index);
                }

                foreach (ComponentMenuItem componentMenuItem in componentSetMenuItem.ComponentMenuItems)
                {
                    if (componentMenuItem.gameObject.activeInHierarchy)
                    {
                        index++;

                        swapTarget = contentTransform.GetChild(index);

                        if (componentMenuItem.transform != swapTarget)
                        {
                            swapTarget.SetSiblingIndex(componentMenuItem.transform.GetSiblingIndex());
                            componentMenuItem.transform.SetSiblingIndex(index);
                        }
                    }
                }

                index++;
            }
        }

        foreach (ComponentSetMenuItem componentSetMenuItem in componentSetMenuItems)
        {
            if (componentSetMenuItem.gameObject.activeInHierarchy)
            {
                if (openComponentSets.Contains(componentSetMenuItem.ComponentSet))
                {
                    componentSetMenuItem.Expand();
                }
                else
                {
                    componentSetMenuItem.Callapse();
                }
            }
        }
    }

    public void Refresh()
    {
        if (targetInventory.ComponentsChanged)
        {
            switch (selectedComponentType)
            {
                case ComponentType.Weapon:
                    {
                        BuildWeaponsList();
                        break;
                    }
                case ComponentType.Ammo:
                    {
                        BuildAmmoList();
                        break;
                    }
                case ComponentType.Equipment:
                    {
                        BuildEquipmentList();
                        break;
                    }
                case ComponentType.Reactor:
                    {
                        BuildReactorsList();
                        break;
                    }
            }
        }
        else
        {
            foreach (ComponentMenuItem componentMenuItem in componentMenuItems)
            {
                if (componentMenuItem.gameObject.activeInHierarchy)
                {
                    componentMenuItem.RefreshCount();
                }
            }
        }
    }

    IEnumerator ResetScrollPos()
    {
        yield return null; // Waiting just one frame is probably good enough, yield return null does that
        scrollRect.verticalNormalizedPosition = 1;
    }

    void CalculateItemWidth()
    {
        RectTransform contentRectTransform = contentTransform.GetComponent<RectTransform>();

        itemWidth = contentRectTransform.rect.width;
    }

    public void SelectComponent(ComponentDefinition componentDefinition)
    {
        callBackSelect(componentDefinition);
    }

    public void HoverComponent(ComponentDefinition componentDefinition)
    {
        callBackHover(componentDefinition);
    }

    public void ClickWeaponsButton()
    {
        if (selectedComponentType != ComponentType.Weapon)
            BuildWeaponsList();

        AudioManager.Instance.PlayButtonClick(0);
    }

    public void ClickAmmoButton()
    {
        if (selectedComponentType != ComponentType.Ammo)
            BuildAmmoList();

        AudioManager.Instance.PlayButtonClick(0);
    }

    public void ClickEquipmentButton()
    {
        if (selectedComponentType != ComponentType.Equipment)
            BuildEquipmentList();

        AudioManager.Instance.PlayButtonClick(0);
    }

    public void ClickReactorsButton()
    {
        if (selectedComponentType != ComponentType.Reactor)
            BuildReactorsList();

        AudioManager.Instance.PlayButtonClick(0);
    }
}