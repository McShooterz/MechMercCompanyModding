using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitCustomizationScreen : MonoBehaviour
{
    #region Variables
    [SerializeField]
    MechPaintScreen mechPaintScreen;

    [Header("Sub Menus")]

    [SerializeField]
    WeaponGroupingMenu weaponGroupingMenu;

    [SerializeField]
    MechDesignSaveMenu mechDesignSaveMenu;

    [SerializeField]
    MechDesignLoadMenu mechDesignLoadMenu;

    [Header("Components")]

    [SerializeField]
    ComponentListUI componentListUI;

    [SerializeField]
    MechCustomizationStatsUI mechCustomizationStatsUI;

    [SerializeField]
    GameObject overlayGameObject;

    [SerializeField]
    Button saveDesignButton;

    [SerializeField]
    Button acceptChangesButton;

    [SerializeField]
    SectionHeaderOneSided sectionHeaderHead;

    [SerializeField]
    SectionHeaderTwoSided sectionHeaderTorsoCenter;

    [SerializeField]
    SectionHeaderTwoSided sectionHeaderTorsoLeft;

    [SerializeField]
    SectionHeaderTwoSided sectionHeaderTorsoRight;

    [SerializeField]
    SectionHeaderOneSided sectionHeaderArmLeft;

    [SerializeField]
    SectionHeaderOneSided sectionHeaderArmRight;

    [SerializeField]
    SectionHeaderOneSided sectionHeaderLegLeft;

    [SerializeField]
    SectionHeaderOneSided sectionHeaderLegRight;

    [SerializeField]
    SectionSlotGroupsUI sectionSlotGroupsHead;

    [SerializeField]
    SectionSlotGroupsUI sectionSlotGroupsTorsoCenter;

    [SerializeField]
    SectionSlotGroupsUI sectionSlotGroupsTorsoLeft;

    [SerializeField]
    SectionSlotGroupsUI sectionSlotGroupsTorsoRight;

    [SerializeField]
    SectionSlotGroupsUI sectionSlotGroupsArmLeft;

    [SerializeField]
    SectionSlotGroupsUI sectionSlotGroupsArmRight;

    [SerializeField]
    SectionSlotGroupsUI sectionSlotGroupsLegLeft;

    [SerializeField]
    SectionSlotGroupsUI sectionSlotGroupsLegRight;

    [SerializeField]
    SlotGroupUI currentSlotGroupUI;

    [SerializeField]
    ComponentDragUI componentDragUI;

    [SerializeField]
    ComponentInformationUI componentInformationUI;

    [SerializeField]
    List<GameObject> mountedWeaponsTorsoCenter = new List<GameObject>();

    [SerializeField]
    List<GameObject> mountedWeaponsTorsoLeft = new List<GameObject>();

    [SerializeField]
    List<GameObject> mountedWeaponsTorsoRight = new List<GameObject>();

    [SerializeField]
    List<GameObject> mountedWeaponsArmLeft = new List<GameObject>();

    [SerializeField]
    List<GameObject> mountedWeaponsArmRight = new List<GameObject>();

    [SerializeField]
    Text warningMessageText;

    [Header("Design Stats")]

    [SerializeField]
    float currentTonnageComponents = 0.0f;

    [SerializeField]
    float currentTonnageArmor = 0.0f;

    [SerializeField]
    int currentArmorPoints = 0;

    [SerializeField]
    float currentEnginePower = 0.0f;

    [SerializeField]
    float currentHeatLimit = 0.0f;

    [SerializeField]
    float currentCooling = 0.0f;

    [SerializeField]
    float currentCoolant = 0.0f;

    [SerializeField]
    float currentJumpJetThrust = 0.0f;

    [SerializeField]
    float currentWeaponMaxRange = 0.0f;

    [SerializeField]
    float currentWeaponDamage = 0.0f;

    [SerializeField]
    float currentWeaponHeat = 0.0f;

    [SerializeField]
    float currentWeaponDPS = 0.0f;

    [SerializeField]
    float currentWeaponHPS = 0.0f;

    [Header("Misc")]

    [SerializeField]
    Vector2 componentPlacedSize;

    [SerializeField]
    Vector2 slotSize;

    [SerializeField]
    string currentMechChassisName;

    [SerializeField]
    bool isValidDesign;

    MechChassisDefinition currentMechChassis;

    ComponentDefinition selectedComponentDefinition;

    [SerializeField]
    Inventory tempInventory;
    #endregion

    public Inventory TempInventory { get => tempInventory; }

    public ComponentListUI ComponentListUI { get => componentListUI; }

    public MechMetaController CurrentMechMetaController { get; private set; }

    public Material CurrentMechMaterial { get; private set; }

    public float CurrentTonnage { get => currentTonnageComponents + currentTonnageArmor; }

    public Vector2 ComponentPlacedSize { get => componentPlacedSize; }

    public Vector2 SlotSize { get => slotSize; }

    void Awake()
    {
        componentDragUI.gameObject.SetActive(false);
        mechDesignSaveMenu.gameObject.SetActive(false);
        mechDesignLoadMenu.gameObject.SetActive(false);
        weaponGroupingMenu.gameObject.SetActive(false);
        componentInformationUI.gameObject.SetActive(false);

        warningMessageText.color = new Color(warningMessageText.color.r, warningMessageText.color.g, warningMessageText.color.b, 0f);

        componentPlacedSize = new Vector2(componentPlacedSize.x / 1920f * Screen.width, componentPlacedSize.y / 1080 * Screen.height);
        slotSize = new Vector2(slotSize.x / 1920f * Screen.width, slotSize.y / 1080 * Screen.height);
    }

    // Use this for initialization
    void Start ()
    {
        tempInventory = new Inventory(GlobalDataManager.Instance.inventoryCurrent);

        componentListUI.Setup(tempInventory, SetSelectedComponent, SetHovedComponent);

        if (GlobalDataManager.Instance.mechDataCustomizing != null)
        {
            if (GlobalDataManager.Instance.mechDataCustomizing != null)
            {
                SetCurrentMechChassis(GlobalDataManager.Instance.mechDataCustomizing.MechChassis);

                if (currentMechChassis != null)
                {
                    LoadMechDesign(GlobalDataManager.Instance.mechDataCustomizing.MechDesign, true);
                }

                CurrentMechMaterial = CurrentMechMetaController.ApplyMechPaintScheme(GlobalDataManager.Instance.mechDataCustomizing.mechPaintScheme);

                mechPaintScreen.Initialize(currentMechChassis.Key, CurrentMechMetaController, CurrentMechMaterial);
            }
        }
        else
        {
            Debug.LogError("Error: No mech data to customize");
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (selectedComponentDefinition != null)
        {
            if (Input.GetMouseButtonUp(0))
            {
                TryPlaceSelectedComponent();
            }
        }

        if (warningMessageText.color.a > 0f)
        {
            float alpha = warningMessageText.color.a - Time.unscaledDeltaTime * 0.5f;

            if (alpha < 0f)
            {
                alpha = 0f;
            }

            warningMessageText.color = new Color(warningMessageText.color.r, warningMessageText.color.g, warningMessageText.color.b, alpha);
        }
	}

    void OnEnable()
    {
        if (CurrentMechMetaController != null)
        {
            CurrentMechMetaController.gameObject.transform.rotation = Quaternion.identity;
        }

        Time.timeScale = 1.0f;
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }

    public void SetSelectedComponent(ComponentDefinition componentDefinition)
    {
        selectedComponentDefinition = componentDefinition;

        tempInventory.TakeComponent(selectedComponentDefinition);

        AudioManager.Instance.PlayClipUI(ResourceManager.Instance.GetAudioClip("PickupComponent"));

        componentDragUI.gameObject.SetActive(true);
        componentDragUI.ComponentNameText.text = selectedComponentDefinition.GetDisplayName();
        componentDragUI.ComponentNameText.color = selectedComponentDefinition.TextColor;
        componentDragUI.BackgroundImage.color = selectedComponentDefinition.Color;
        componentDragUI.BackgroundRectTransform.sizeDelta = new Vector2(componentPlacedSize.x, (selectedComponentDefinition.SlotSize - 1) * slotSize.y + componentPlacedSize.y);

        componentDragUI.GetComponent<RectTransform>().sizeDelta = componentPlacedSize;
        componentDragUI.ComponentNameText.GetComponent<RectTransform>().sizeDelta = componentPlacedSize;

        componentDragUI.transform.position = Input.mousePosition;

        componentInformationUI.gameObject.SetActive(false);
    }

    public void PickupPlacedComponent(ComponentDefinition componentDefinition)
    {
        selectedComponentDefinition = componentDefinition;

        AudioManager.Instance.PlayClipUI(ResourceManager.Instance.GetAudioClip("PickupComponent"));

        componentDragUI.gameObject.SetActive(true);
        componentDragUI.ComponentNameText.text = selectedComponentDefinition.GetDisplayName();
        componentDragUI.ComponentNameText.color = selectedComponentDefinition.TextColor;
        componentDragUI.BackgroundImage.color = selectedComponentDefinition.Color;
        componentDragUI.BackgroundRectTransform.sizeDelta = new Vector2(componentPlacedSize.x, (selectedComponentDefinition.SlotSize - 1) * slotSize.y + componentPlacedSize.y);

        componentDragUI.GetComponent<RectTransform>().sizeDelta = componentPlacedSize;
        componentDragUI.ComponentNameText.GetComponent<RectTransform>().sizeDelta = componentPlacedSize;

        componentDragUI.transform.position = Input.mousePosition;

        componentInformationUI.gameObject.SetActive(false);
    }

    public void SetHovedComponent(ComponentDefinition componentDefinition)
    {
        if (componentDefinition != null)
        {
            componentInformationUI.gameObject.SetActive(true);
            componentInformationUI.SetComponent(componentDefinition);
        }
        else
        {
            componentInformationUI.gameObject.SetActive(false);
        }
    }

    void ClearSelectedComponent()
    {
        selectedComponentDefinition = null;

        componentDragUI.gameObject.SetActive(false);
    }

    public void SetCurrentSlotGroupUI(SlotGroupUI slotGroupUI)
    {
        currentSlotGroupUI = slotGroupUI;
    }

    public void ClearCurrentSlotGroupUI()
    {
        currentSlotGroupUI = null;
    }

    public void SetCurrentMechChassis(MechChassisDefinition mechChassisDefinition)
    {
        if (mechChassisDefinition != null)
        {
            currentMechChassisName = mechChassisDefinition.Key;
            currentMechChassis = mechChassisDefinition;

            mechCustomizationStatsUI.SetChassis(mechChassisDefinition);

            if (CurrentMechMetaController)
            {
                Destroy(CurrentMechMetaController.gameObject);
            }

            GameObject mechObject = Instantiate(mechChassisDefinition.GetMechPrefab(), Vector3.zero, Quaternion.identity);

            if ((object)mechObject != null)
            {
                CurrentMechMetaController = mechObject.GetComponent<MechMetaController>();

                //Texture2D defaulSkin = mechChassisDefinition.GetDefaultSkin();
                //List<Texture2D> mechSkins = mechChassisDefinition.GetSkins();

                //mechSkinButtonList.CreateButtons(mechSkins, SetMechSkin);
            }
            else
            {
                print("Error: Mech Prefab not found");
            }

            sectionHeaderHead.SetBaseValues(currentMechChassis.HeadArmorLimit, currentMechChassis.HeadInternal);
            sectionHeaderTorsoCenter.SetBaseValues(currentMechChassis.TorsoCenterArmorLimit, currentMechChassis.TorsoCenterInternal);
            sectionHeaderTorsoLeft.SetBaseValues(currentMechChassis.TorsoLeftArmorLimit, currentMechChassis.TorsoLeftInternal);
            sectionHeaderTorsoRight.SetBaseValues(currentMechChassis.TorsoRightArmorLimit, currentMechChassis.TorsoRightInternal);
            sectionHeaderLegLeft.SetBaseValues(currentMechChassis.LegLeftArmorLimit, currentMechChassis.LegLeftInternal);
            sectionHeaderLegRight.SetBaseValues(currentMechChassis.LegRightArmorLimit, currentMechChassis.LegRightInternal);
            sectionHeaderArmLeft.SetBaseValues(currentMechChassis.ArmLeftArmorLimit, currentMechChassis.ArmLeftInternal);
            sectionHeaderArmRight.SetBaseValues(currentMechChassis.ArmRightArmorLimit, currentMechChassis.ArmRightInternal);

            sectionSlotGroupsHead.Clear();
            sectionSlotGroupsTorsoCenter.Clear();
            sectionSlotGroupsTorsoLeft.Clear();
            sectionSlotGroupsTorsoRight.Clear();
            sectionSlotGroupsArmLeft.Clear();
            sectionSlotGroupsArmRight.Clear();
            sectionSlotGroupsLegLeft.Clear();
            sectionSlotGroupsLegRight.Clear();

            sectionSlotGroupsHead.CreateSlotGroups(currentMechChassis.HeadSlotGroups);
            sectionSlotGroupsTorsoCenter.CreateSlotGroups(currentMechChassis.TorsoCenterSlotGroups);
            sectionSlotGroupsTorsoLeft.CreateSlotGroups(currentMechChassis.TorsoLeftSlotGroups);
            sectionSlotGroupsTorsoRight.CreateSlotGroups(currentMechChassis.TorsoRightSlotGroups);
            sectionSlotGroupsArmLeft.CreateSlotGroups(currentMechChassis.ArmLeftSlotGroups);
            sectionSlotGroupsArmRight.CreateSlotGroups(currentMechChassis.ArmRightSlotGroups);
            sectionSlotGroupsLegLeft.CreateSlotGroups(currentMechChassis.LegLeftSlotGroups);
            sectionSlotGroupsLegRight.CreateSlotGroups(currentMechChassis.LegRightSlotGroups);

            RecalculateAllStats();
        }
        else
        {
            currentMechChassisName = "";
        }

        UpdateUnitStats();
    }

    public void UpdateUnitStats()
    {
        if (currentMechChassis != null)
        {
            mechCustomizationStatsUI.SetTonnageCurrent((float)System.Math.Round(CurrentTonnage, 2));

            mechCustomizationStatsUI.SetEnginePower(currentEnginePower);

            mechCustomizationStatsUI.SetHeatLimit(currentHeatLimit);

            mechCustomizationStatsUI.SetHeatShutdown(currentHeatLimit * 0.8f);

            mechCustomizationStatsUI.SetCooling(currentCooling);

            mechCustomizationStatsUI.SetCoolant(currentCoolant);

            mechCustomizationStatsUI.SetJumpJetThrust(currentJumpJetThrust);

            mechCustomizationStatsUI.SetArmorPoints(currentArmorPoints);

            mechCustomizationStatsUI.SetArmorWeight(currentTonnageArmor);

            mechCustomizationStatsUI.SetWeaponMaxRange(currentWeaponMaxRange);

            mechCustomizationStatsUI.SetWeaponDamage(currentWeaponDamage);

            mechCustomizationStatsUI.SetWeaponDPS(currentWeaponDPS);

            mechCustomizationStatsUI.SetWeaponHeat(currentWeaponHeat, currentHeatLimit * 0.8f);

            mechCustomizationStatsUI.SetWeaponHPS(currentWeaponHPS, currentCooling);
        }
    }

    public void RecalculateAllStats()
    {
        if (currentMechChassis != null)
        {
            RecalculateArmor();

            RecalculateInternals();

            RecalculateComponents();

            RemountWeapons();
         
        }
        else
        {
            currentTonnageArmor = 0f;
            currentTonnageComponents = 0f;
        }

        UpdateArmorButtons();

        UpdateUnitStats();
    }

    public void RecalculateArmor()
    {
        currentTonnageArmor = 0f;
        currentArmorPoints = 0;

        currentArmorPoints += sectionHeaderHead.ArmorPoints;
        currentArmorPoints += sectionHeaderTorsoCenter.TotalArmor;
        currentArmorPoints += sectionHeaderTorsoLeft.TotalArmor;
        currentArmorPoints += sectionHeaderTorsoRight.TotalArmor;
        currentArmorPoints += sectionHeaderArmLeft.ArmorPoints;
        currentArmorPoints += sectionHeaderArmRight.ArmorPoints;
        currentArmorPoints += sectionHeaderLegLeft.ArmorPoints;
        currentArmorPoints += sectionHeaderLegRight.ArmorPoints;

        currentTonnageArmor += sectionHeaderHead.GetArmorWeight();
        currentTonnageArmor += sectionHeaderTorsoCenter.GetArmorWeight();
        currentTonnageArmor += sectionHeaderTorsoLeft.GetArmorWeight();
        currentTonnageArmor += sectionHeaderTorsoRight.GetArmorWeight();
        currentTonnageArmor += sectionHeaderLegLeft.GetArmorWeight();
        currentTonnageArmor += sectionHeaderLegRight.GetArmorWeight();
        currentTonnageArmor += sectionHeaderArmLeft.GetArmorWeight();
        currentTonnageArmor += sectionHeaderArmRight.GetArmorWeight();

        CheckValid();

        UpdateUnitStats();      
    }

    public void RecalculateInternals()
    {
        float internalsHead = currentMechChassis.HeadInternal;
        float internalTorsoCenter = currentMechChassis.TorsoCenterInternal;
        float internalTorsoLeft = currentMechChassis.TorsoLeftInternal;
        float internalTorsoRight = currentMechChassis.TorsoRightInternal;
        float internalArmLeft = currentMechChassis.ArmLeftInternal;
        float internalArmRight = currentMechChassis.ArmRightInternal;
        float internalLegLeft = currentMechChassis.LegLeftInternal;
        float internalLegRight = currentMechChassis.LegRightInternal;

        foreach (ComponentDefinition componentDefinition in sectionSlotGroupsHead.GetComponentDefinitions())
        {
            internalsHead += componentDefinition.InternalBonus;
        }

        sectionHeaderHead.InternalValueText.text = internalsHead.ToString("0.#");

        foreach (ComponentDefinition componentDefinition in sectionSlotGroupsTorsoCenter.GetComponentDefinitions())
        {
            internalTorsoCenter += componentDefinition.InternalBonus;
        }

        sectionHeaderTorsoCenter.InternalValueText.text = internalTorsoCenter.ToString("0.#");

        foreach (ComponentDefinition componentDefinition in sectionSlotGroupsTorsoLeft.GetComponentDefinitions())
        {
            internalTorsoLeft += componentDefinition.InternalBonus;
        }

        sectionHeaderTorsoLeft.InternalValueText.text = internalTorsoLeft.ToString("0.#");

        foreach (ComponentDefinition componentDefinition in sectionSlotGroupsTorsoRight.GetComponentDefinitions())
        {
            internalTorsoRight += componentDefinition.InternalBonus;
        }

        sectionHeaderTorsoRight.InternalValueText.text = internalTorsoRight.ToString("0.#");

        foreach (ComponentDefinition componentDefinition in sectionSlotGroupsArmLeft.GetComponentDefinitions())
        {
            internalArmLeft += componentDefinition.InternalBonus;
        }

        sectionHeaderArmLeft.InternalValueText.text = internalArmLeft.ToString("0.#");

        foreach (ComponentDefinition componentDefinition in sectionSlotGroupsArmRight.GetComponentDefinitions())
        {
            internalArmRight += componentDefinition.InternalBonus;
        }

        sectionHeaderArmRight.InternalValueText.text = internalArmRight.ToString("0.#");

        foreach (ComponentDefinition componentDefinition in sectionSlotGroupsLegLeft.GetComponentDefinitions())
        {
            internalLegLeft += componentDefinition.InternalBonus;
        }

        sectionHeaderLegLeft.InternalValueText.text = internalLegLeft.ToString("0.#");

        foreach (ComponentDefinition componentDefinition in sectionSlotGroupsLegRight.GetComponentDefinitions())
        {
            internalLegRight += componentDefinition.InternalBonus;
        }

        sectionHeaderLegRight.InternalValueText.text = internalLegRight.ToString("0.#");
    }

    public void RecalculateComponents()
    {
        currentTonnageComponents = 0f;

        currentEnginePower = 0f;
        currentHeatLimit = 0f;
        currentCooling = 0f;
        currentCoolant = 0f;
        currentJumpJetThrust = 0f;
        currentWeaponMaxRange = 0f;
        currentWeaponDPS = 0f;

        float heatLimitBonus = 0f;
        isValidDesign = true;

        List<ComponentDefinition> componentDefinitions = new List<ComponentDefinition>();

        componentDefinitions.AddRange(sectionSlotGroupsHead.GetComponentDefinitions());
        componentDefinitions.AddRange(sectionSlotGroupsTorsoCenter.GetComponentDefinitions());
        componentDefinitions.AddRange(sectionSlotGroupsTorsoLeft.GetComponentDefinitions());
        componentDefinitions.AddRange(sectionSlotGroupsTorsoRight.GetComponentDefinitions());
        componentDefinitions.AddRange(sectionSlotGroupsLegLeft.GetComponentDefinitions());
        componentDefinitions.AddRange(sectionSlotGroupsLegRight.GetComponentDefinitions());
        componentDefinitions.AddRange(sectionSlotGroupsArmLeft.GetComponentDefinitions());
        componentDefinitions.AddRange(sectionSlotGroupsArmRight.GetComponentDefinitions());

        foreach (ComponentDefinition componentDefinition in componentDefinitions)
        {
            currentTonnageComponents += componentDefinition.Weight;

            currentEnginePower += componentDefinition.EnginePower;
            currentCooling += componentDefinition.Cooling;
            currentCoolant += componentDefinition.Coolant;
            currentJumpJetThrust += componentDefinition.JumpJetThrust;
            heatLimitBonus += componentDefinition.HeatLimitBonus;

            if (componentDefinition.HeatLimit > currentHeatLimit)
            {
                currentHeatLimit = componentDefinition.HeatLimit;
            }
        }

        currentHeatLimit += heatLimitBonus;

        CalculateSectionWeaponStats(sectionSlotGroupsHead.GetWeaponDefinitions(), sectionSlotGroupsHead.GetAmmoTypes(), out float headDamage, out float headDPS, out float headRangeMax, out float headHeat, out float headHPS);
        CalculateSectionWeaponStats(sectionSlotGroupsTorsoCenter.GetWeaponDefinitions(), sectionSlotGroupsTorsoCenter.GetAmmoTypes(), out float torsoCenterDamage, out float torsoCenterDPS, out float torsoCenterRangeMax, out float torsoCenterHeat, out float torsoCenterHPS);
        CalculateSectionWeaponStats(sectionSlotGroupsTorsoLeft.GetWeaponDefinitions(), sectionSlotGroupsTorsoLeft.GetAmmoTypes(), out float torsoLeftDamage, out float torsoLeftDPS, out float torsoLeftRangeMax, out float torsoLeftHeat, out float torsoLeftHPS);
        CalculateSectionWeaponStats(sectionSlotGroupsTorsoRight.GetWeaponDefinitions(), sectionSlotGroupsTorsoRight.GetAmmoTypes(), out float torsoRightDamage, out float torsoRightDPS, out float torsoRightRangeMax, out float torsoRightHeat, out float torsoRightHPS);
        CalculateSectionWeaponStats(sectionSlotGroupsArmLeft.GetWeaponDefinitions(), sectionSlotGroupsArmLeft.GetAmmoTypes(), out float armLeftDamage, out float armLeftDPS, out float armLeftRangeMax, out float armLeftHeat, out float armLeftHPS);
        CalculateSectionWeaponStats(sectionSlotGroupsArmRight.GetWeaponDefinitions(), sectionSlotGroupsArmRight.GetAmmoTypes(), out float armRightDamage, out float armRightDPS, out float armRightRangeMax, out float armRightHeat, out float armRightHPS);

        currentWeaponDamage = headDamage + torsoCenterDamage + torsoLeftDamage + torsoRightDamage + armLeftDamage + armRightDamage;
        currentWeaponDPS = headDPS + torsoCenterDPS + torsoLeftDPS + torsoRightDPS + armLeftDPS + armRightDPS;
        currentWeaponHeat = headHeat + torsoCenterHeat + torsoLeftHeat + torsoRightHeat + armLeftHeat + armRightHeat;
        currentWeaponHPS = headHPS + torsoCenterHPS + torsoLeftHPS + torsoRightHPS + armLeftHPS + armRightHPS;
        currentWeaponMaxRange = Mathf.Max(headRangeMax, torsoCenterRangeMax, torsoLeftRangeMax, torsoRightRangeMax, armLeftRangeMax, armRightRangeMax);

        CheckValid();

        UpdateUnitStats();
    }

    public void RemountWeapons()
    {
        ClearMountedWeapons();
        sectionSlotGroupsTorsoCenter.CreateMountedWeapons(mountedWeaponsTorsoCenter, CurrentMechMetaController.TorsoCenterHardpoints);
        sectionSlotGroupsTorsoLeft.CreateMountedWeapons(mountedWeaponsTorsoLeft, CurrentMechMetaController.TorsoLeftHardpoints);
        sectionSlotGroupsTorsoRight.CreateMountedWeapons(mountedWeaponsTorsoRight, CurrentMechMetaController.TorsoRightHardpoints);
        sectionSlotGroupsArmLeft.CreateMountedWeapons(mountedWeaponsArmLeft, CurrentMechMetaController.ArmLeftHardpoints);
        sectionSlotGroupsArmRight.CreateMountedWeapons(mountedWeaponsArmRight, CurrentMechMetaController.ArmRightHardpoints);
    }

    public void UpdateArmorButtons()
    {
        sectionHeaderHead.UpdateArmorButtons();
        sectionHeaderTorsoCenter.UpdateArmorButtons();
        sectionHeaderTorsoLeft.UpdateArmorButtons();
        sectionHeaderTorsoRight.UpdateArmorButtons();
        sectionHeaderLegLeft.UpdateArmorButtons();
        sectionHeaderLegRight.UpdateArmorButtons();
        sectionHeaderArmLeft.UpdateArmorButtons();
        sectionHeaderArmRight.UpdateArmorButtons();
    }

    public bool CheckTonnage(float weight)
    {
        return System.Math.Round(weight + CurrentTonnage, 2) <= currentMechChassis.Tonnage;
    }

    void TryPlaceSelectedComponent()
    {
        if (currentSlotGroupUI != null)
        {
            currentSlotGroupUI.TryPlaceComponent(selectedComponentDefinition, null, true);
            componentInformationUI.gameObject.SetActive(false);
        }
        else
        {
            AudioManager.Instance.PlayClipUI(ResourceManager.Instance.GetAudioClip("Cancel"));
            TempInventory.AddComponent(selectedComponentDefinition);
        }

        componentListUI.Refresh();

        ClearSelectedComponent();
    }

    void MaxArmor()
    {
        sectionHeaderHead.MaxArmor();
        sectionHeaderTorsoCenter.MaxArmor();
        sectionHeaderTorsoLeft.MaxArmor();
        sectionHeaderTorsoRight.MaxArmor();
        sectionHeaderArmLeft.MaxArmor();
        sectionHeaderArmRight.MaxArmor();
        sectionHeaderLegLeft.MaxArmor();
        sectionHeaderLegRight.MaxArmor();

        UpdateArmorButtons();

        sectionHeaderHead.UpdateArmorText();
        sectionHeaderTorsoCenter.UpdateArmorText();
        sectionHeaderTorsoLeft.UpdateArmorText();
        sectionHeaderTorsoRight.UpdateArmorText();
        sectionHeaderArmLeft.UpdateArmorText();
        sectionHeaderArmRight.UpdateArmorText();
        sectionHeaderLegLeft.UpdateArmorText();
        sectionHeaderLegRight.UpdateArmorText();

        RecalculateArmor();
    }

    void CheckValid()
    {
        isValidDesign = true;

        if (!CheckTonnage(0.0f))
        {
            isValidDesign = false;
        }

        if (currentEnginePower <= 0)
        {
            isValidDesign = false;
        }


        if (currentHeatLimit <= 0)
        {
            isValidDesign = false;
        }

        if (currentCooling <= 0)
        {
            isValidDesign = false;
        }

        if (isValidDesign)
        {
            acceptChangesButton.interactable = true;
            saveDesignButton.interactable = true;
        }
        else
        {
            acceptChangesButton.interactable = false;
            saveDesignButton.interactable = false;
        }
    }

    public void SaveMechDesign(string designName)
    {
        ResourceManager.Instance.SaveMechDesign(currentMechChassisName, GetMechDesign(designName));
    }

    public void LoadMechDesign(MechDesign mechDesign, bool doNotTakeFromInventory)
    {
        mechDesignLoadMenu.gameObject.SetActive(false);
        ClearPlacedComponents();

        sectionHeaderHead.ChangeArmorType(mechDesign.ArmorTypeHead, mechDesign.ArmorPointsHead);
        sectionHeaderTorsoCenter.ChangeArmorType(mechDesign.ArmorTypeTorsoCenter, mechDesign.ArmorPointsTorsoCenter, mechDesign.ArmorPointsTorsoCenterRear);
        sectionHeaderTorsoLeft.ChangeArmorType(mechDesign.ArmorTypeTorsoLeft, mechDesign.ArmorPointsTorsoLeft, mechDesign.ArmorPointsTorsoLeftRear);
        sectionHeaderTorsoRight.ChangeArmorType(mechDesign.ArmorTypeTorsoRight, mechDesign.ArmorPointsTorsoRight, mechDesign.ArmorPointsTorsoRightRear);
        sectionHeaderArmLeft.ChangeArmorType(mechDesign.ArmorTypeArmLeft, mechDesign.ArmorPointsArmLeft);
        sectionHeaderArmRight.ChangeArmorType(mechDesign.ArmorTypeArmRight, mechDesign.ArmorPointsArmRight);
        sectionHeaderLegLeft.ChangeArmorType(mechDesign.ArmorTypeLegLeft, mechDesign.ArmorPointsLegLeft);
        sectionHeaderLegRight.ChangeArmorType(mechDesign.ArmorTypeLegRight, mechDesign.ArmorPointsLegRight);

        sectionSlotGroupsHead.LoadComponentGroups(mechDesign.ComponentGroupsHead, doNotTakeFromInventory);
        sectionSlotGroupsTorsoCenter.LoadComponentGroups(mechDesign.ComponentGroupsTorsoCenter, doNotTakeFromInventory);
        sectionSlotGroupsTorsoLeft.LoadComponentGroups(mechDesign.ComponentGroupsTorsoLeft, doNotTakeFromInventory);
        sectionSlotGroupsTorsoRight.LoadComponentGroups(mechDesign.ComponentGroupsTorsoRight, doNotTakeFromInventory);
        sectionSlotGroupsArmLeft.LoadComponentGroups(mechDesign.ComponentGroupsArmLeft, doNotTakeFromInventory);
        sectionSlotGroupsArmRight.LoadComponentGroups(mechDesign.ComponentGroupsArmRight, doNotTakeFromInventory);
        sectionSlotGroupsLegLeft.LoadComponentGroups(mechDesign.ComponentGroupsLegLeft, doNotTakeFromInventory);
        sectionSlotGroupsLegRight.LoadComponentGroups(mechDesign.ComponentGroupsLegRight, doNotTakeFromInventory);

        RecalculateAllStats();

        componentListUI.Refresh();
    }

    void ClearArmor()
    {
        sectionHeaderHead.ChangeArmorType(ArmorType.standard, 0);
        sectionHeaderTorsoCenter.ChangeArmorType(ArmorType.standard, 0, 0);
        sectionHeaderTorsoLeft.ChangeArmorType(ArmorType.standard, 0, 0);
        sectionHeaderTorsoRight.ChangeArmorType(ArmorType.standard, 0, 0);
        sectionHeaderArmLeft.ChangeArmorType(ArmorType.standard, 0);
        sectionHeaderArmRight.ChangeArmorType(ArmorType.standard, 0);
        sectionHeaderLegLeft.ChangeArmorType(ArmorType.standard, 0);
        sectionHeaderLegRight.ChangeArmorType(ArmorType.standard, 0);

        RecalculateArmor();
    }

    void ClearPlacedComponents()
    {
        sectionSlotGroupsHead.RemovePlacedComponents();
        sectionSlotGroupsTorsoCenter.RemovePlacedComponents();
        sectionSlotGroupsTorsoLeft.RemovePlacedComponents();
        sectionSlotGroupsTorsoRight.RemovePlacedComponents();
        sectionSlotGroupsArmLeft.RemovePlacedComponents();
        sectionSlotGroupsArmRight.RemovePlacedComponents();
        sectionSlotGroupsLegLeft.RemovePlacedComponents();
        sectionSlotGroupsLegRight.RemovePlacedComponents();

        componentListUI.Refresh();
    }

    void ClearMountedWeapons()
    {
        foreach(GameObject mountedWeapon in mountedWeaponsTorsoCenter)
        {
            if (mountedWeapon)
            {
                Destroy(mountedWeapon);
            }
        }

        foreach (GameObject mountedWeapon in mountedWeaponsTorsoLeft)
        {
            if (mountedWeapon)
            {
                Destroy(mountedWeapon);
            }
        }

        foreach (GameObject mountedWeapon in mountedWeaponsTorsoRight)
        {
            if (mountedWeapon)
            {
                Destroy(mountedWeapon);
            }
        }

        foreach (GameObject mountedWeapon in mountedWeaponsArmLeft)
        {
            if (mountedWeapon)
            {
                Destroy(mountedWeapon);
            }
        }

        foreach (GameObject mountedWeapon in mountedWeaponsArmRight)
        {
            if (mountedWeapon)
            {
                Destroy(mountedWeapon);
            }
        }

        mountedWeaponsTorsoCenter.Clear();
        mountedWeaponsTorsoLeft.Clear();
        mountedWeaponsTorsoRight.Clear();
        mountedWeaponsArmLeft.Clear();
        mountedWeaponsArmRight.Clear();
    }

    MechDesign GetMechDesign(string designName)
    {
        MechDesign mechDesign = new MechDesign();
        mechDesign.DesignName = designName;
        mechDesign.MechChassisDefinition = currentMechChassis.Key;

        mechDesign.ArmorTypeHead = sectionHeaderHead.ArmorType;
        mechDesign.ArmorPointsHead = sectionHeaderHead.ArmorPoints;

        mechDesign.ArmorTypeTorsoCenter = sectionHeaderTorsoCenter.ArmorType;
        mechDesign.ArmorPointsTorsoCenter = sectionHeaderTorsoCenter.ArmorPoints;
        mechDesign.ArmorPointsTorsoCenterRear = sectionHeaderTorsoCenter.ArmorPointsRear;

        mechDesign.ArmorTypeTorsoLeft = sectionHeaderTorsoLeft.ArmorType;
        mechDesign.ArmorPointsTorsoLeft = sectionHeaderTorsoLeft.ArmorPoints;
        mechDesign.ArmorPointsTorsoLeftRear = sectionHeaderTorsoLeft.ArmorPointsRear;

        mechDesign.ArmorTypeTorsoRight = sectionHeaderTorsoRight.ArmorType;
        mechDesign.ArmorPointsTorsoRight = sectionHeaderTorsoRight.ArmorPoints;
        mechDesign.ArmorPointsTorsoRightRear = sectionHeaderTorsoRight.ArmorPointsRear;

        mechDesign.ArmorTypeArmLeft = sectionHeaderArmLeft.ArmorType;
        mechDesign.ArmorPointsArmLeft = sectionHeaderArmLeft.ArmorPoints;

        mechDesign.ArmorTypeArmRight = sectionHeaderArmRight.ArmorType;
        mechDesign.ArmorPointsArmRight = sectionHeaderArmRight.ArmorPoints;

        mechDesign.ArmorTypeLegLeft = sectionHeaderLegLeft.ArmorType;
        mechDesign.ArmorPointsLegLeft = sectionHeaderLegLeft.ArmorPoints;

        mechDesign.ArmorTypeLegRight = sectionHeaderLegRight.ArmorType;
        mechDesign.ArmorPointsLegRight = sectionHeaderLegRight.ArmorPoints;

        mechDesign.ComponentGroupsHead = sectionSlotGroupsHead.GetComponentGroups();
        mechDesign.ComponentGroupsTorsoCenter = sectionSlotGroupsTorsoCenter.GetComponentGroups();
        mechDesign.ComponentGroupsTorsoLeft = sectionSlotGroupsTorsoLeft.GetComponentGroups();
        mechDesign.ComponentGroupsTorsoRight = sectionSlotGroupsTorsoRight.GetComponentGroups();
        mechDesign.ComponentGroupsArmLeft = sectionSlotGroupsArmLeft.GetComponentGroups();
        mechDesign.ComponentGroupsArmRight = sectionSlotGroupsArmRight.GetComponentGroups();
        mechDesign.ComponentGroupsLegLeft = sectionSlotGroupsLegLeft.GetComponentGroups();
        mechDesign.ComponentGroupsLegRight = sectionSlotGroupsLegRight.GetComponentGroups();

        return mechDesign;
    }

    public void CalculateSectionWeaponStats(List<WeaponDefinition> weaponDefinitions, List<string> ammoTypes, out float damage, out float dps, out float rangeMax, out float heat, out float hps)
    {
        damage = 0.0f;
        dps = 0.0f;
        rangeMax = 0.0f;
        heat = 0.0f;
        hps = 0.0f;
        float weaponRange;

        for (int i = 0; i < weaponDefinitions.Count; i++)
        {
            WeaponDefinition weaponDefinition = weaponDefinitions[i];

            if (weaponDefinition is BeamWeaponDefinition)
            {
                BeamWeaponDefinition beamWeaponDefinition = weaponDefinition as BeamWeaponDefinition;

                damage += beamWeaponDefinition.EffectiveDamageDisplay;

                dps += beamWeaponDefinition.DPS;

                heat += beamWeaponDefinition.GetWeaponHeat();

                hps += beamWeaponDefinition.HPS;

                if (beamWeaponDefinition.RangeMax > rangeMax)
                {
                    rangeMax = beamWeaponDefinition.RangeMax;
                }
            }
            else
            {
                ProjectileWeaponDefinition projectileWeaponDefinition = weaponDefinition as ProjectileWeaponDefinition;

                if (projectileWeaponDefinition.RequiresAmmo)
                {
                    damage += projectileWeaponDefinition.GetMaxDamage(ammoTypes);

                    dps += projectileWeaponDefinition.GetMaxDPS(ammoTypes);

                    heat += projectileWeaponDefinition.GetMaxHeat(ammoTypes);

                    hps += projectileWeaponDefinition.GetMaxHPS(ammoTypes);

                    weaponRange = projectileWeaponDefinition.GetMaxRange(ammoTypes);

                    if (weaponRange > rangeMax)
                    {
                        rangeMax = weaponRange;
                    }
                }
                else
                {
                    damage = projectileWeaponDefinition.GetDamage();

                    dps += projectileWeaponDefinition.GetDPS();

                    heat += projectileWeaponDefinition.GetHeat();

                    hps += projectileWeaponDefinition.GetHPS();

                    weaponRange = projectileWeaponDefinition.GetMaxRange();

                    if (weaponRange > rangeMax)
                    {
                        rangeMax = weaponRange;
                    }
                }
            }
        }
    }

    public void ClickLoadDesignButton()
    {
        mechDesignLoadMenu.gameObject.SetActive(true);
        mechDesignLoadMenu.BuildDesignsList(ResourceManager.Instance.GetMechDesignList(currentMechChassis.Key));
        mechDesignLoadMenu.SetCallBackFunction(LoadMechDesign);

        AudioManager.Instance.PlayButtonClick(0);
    }

    public void ClickSaveDesignButton()
    {
        if (CheckTonnage(0.0f))
        {
            mechDesignSaveMenu.gameObject.SetActive(true);
            mechDesignSaveMenu.BuildDesignsList(currentMechChassis.Key);

            AudioManager.Instance.PlayButtonClick(0);
        }
    }

    public void ClickGroupWeaponsButton()
    {
        weaponGroupingMenu.gameObject.SetActive(true);

        List<ComponentPlacedUI> componentPlacedUIs = new List<ComponentPlacedUI>();

        componentPlacedUIs.AddRange(sectionSlotGroupsTorsoCenter.GetComponentPlacedUIsWithWeapons());
        componentPlacedUIs.AddRange(sectionSlotGroupsTorsoLeft.GetComponentPlacedUIsWithWeapons());
        componentPlacedUIs.AddRange(sectionSlotGroupsTorsoRight.GetComponentPlacedUIsWithWeapons());
        componentPlacedUIs.AddRange(sectionSlotGroupsArmLeft.GetComponentPlacedUIsWithWeapons());
        componentPlacedUIs.AddRange(sectionSlotGroupsArmRight.GetComponentPlacedUIsWithWeapons());

        weaponGroupingMenu.BuildWeaponsList(componentPlacedUIs);

        AudioManager.Instance.PlayButtonClick(0);
    }

    public void ClickAcceptChangesButton()
    {
        if (CheckTonnage(0.0f))
        {
            AudioManager.Instance.PlayButtonClick(0);

            GlobalDataManager.Instance.inventoryCurrent.Duplicate(TempInventory);

            GlobalDataManager.Instance.mechDataCustomizing.BuildFromDesign(GetMechDesign(""));
            GlobalDataManager.Instance.mechDataCustomizing.designName = "Custom";
            GlobalDataManager.Instance.mechDataCustomizing.mechPaintScheme = mechPaintScreen.GetMechPaintScheme();

            LoadingScreen.Instance.LoadScene(GlobalDataManager.Instance.backSceneName);
        }
    }

    public void ClickRejectChangesButton()
    {
        AudioManager.Instance.PlayButtonClick(0);

        LoadingScreen.Instance.LoadScene(GlobalDataManager.Instance.backSceneName);
    }

    public void ClickToggleOverlayButton()
    {
        overlayGameObject.SetActive(!overlayGameObject.activeInHierarchy);

        AudioManager.Instance.PlayButtonClick(0);
    }

    public void ClickStripMechButton()
    {
        ClearArmor();
        ClearPlacedComponents();
        ClearMountedWeapons();

        RecalculateAllStats();

        AudioManager.Instance.PlayButtonClick(0);
    }

    public void ClickMaxArmorButton()
    {
        MaxArmor();

        AudioManager.Instance.PlayButtonClick(0);
    }

    public void ClickPaintButton()
    {
        gameObject.SetActive(false);
        mechPaintScreen.gameObject.SetActive(true);

        AudioManager.Instance.PlayButtonClick(0);
    }

    public void SetWarningMessage(string warningMessage)
    {
        warningMessageText.color = new Color(warningMessageText.color.r, warningMessageText.color.g, warningMessageText.color.b, 1f);

        warningMessageText.text = warningMessage;
    }
}
