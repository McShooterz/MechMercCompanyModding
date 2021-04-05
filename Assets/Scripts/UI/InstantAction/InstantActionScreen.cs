using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class InstantActionScreen : MonoBehaviour
{
    #region Variables
    [SerializeField]
    MechChassisSelectionMenu mechChassisSelectionMenu;

    [SerializeField]
    MechDesignLoadMenu mechDesignLoadMenu;

    [SerializeField]
    InstantActionPilotSelectionMenu instantActionPilotSelectionMenu;

    [SerializeField]
    InstantActionPlanetMenu instantActionPlanetMenu;

    [SerializeField]
    InstantActionMapMenu instantActionMapMenu;

    [SerializeField]
    InstantActionMissionMenu instantActionMissionMenu;

    [SerializeField]
    InstantActionFactionMenu instantActionFactionMenu;

    [SerializeField]
    InstantActionPilotInfo[] pilotInfos;

    [SerializeField]
    Text planetButtonText;

    [SerializeField]
    Text mapButtonText;

    [SerializeField]
    Text weatherValueText;

    [SerializeField]
    Button weatherIncrementButton;

    [SerializeField]
    Button weatherDecrementButton;

    [SerializeField]
    Text timeOfDayValue;

    [SerializeField]
    Button timeOfDayIncrementButton;

    [SerializeField]
    Button timeOfDayDecrementButton;

    [SerializeField]
    Button missionTypeButton;

    [SerializeField]
    Text missionButtonText;

    [SerializeField]
    Text enemyFactionButtonText;

    [SerializeField]
    Button enemyFactionButton;

    [SerializeField]
    Button secondFactionButton;

    [SerializeField]
    Button difficultyDecreaseButton;

    [SerializeField]
    Button difficultyIncreaseButton;

    [SerializeField]
    Text secondFactionButtonText;

    [SerializeField]
    Text missionDifficultyValueText;

    [SerializeField]
    Image mapImage;

    MechData currentMechData;

    InstantActionPilotInfo currentInstantActionPilotInfo;

    bool secondFaction = false;

    #endregion

    private void Awake()
    {
        mechChassisSelectionMenu.gameObject.SetActive(false);
        mechDesignLoadMenu.gameObject.SetActive(false);
        instantActionPilotSelectionMenu.gameObject.SetActive(false);

        instantActionPlanetMenu.gameObject.SetActive(false);
        instantActionMapMenu.gameObject.SetActive(false);
        instantActionMissionMenu.gameObject.SetActive(false);
        instantActionFactionMenu.gameObject.SetActive(false);

        mechChassisSelectionMenu.SetCallBackFunction(ChangeSelectedMech);

        instantActionPilotSelectionMenu.SetCallBackFunction(ChangeSelectedPilot);
    }

    // Use this for initialization
    void Start ()
    {
        Time.timeScale = 1.0f;

        MechChassisDefinition[] mechChassisDefinitions = ResourceManager.Instance.GetMechChassisDefinitions();

        if (mechChassisDefinitions.Length > 0)
        {
            GlobalDataManager.Instance.instantActionGlobalData.SetDefaultDesign(mechChassisDefinitions[0].GetDefaultDesign());
        }
        else
        {
            Debug.LogError("Error: No mech chassis");
        }

        mechChassisSelectionMenu.BuildDesignsList(mechChassisDefinitions);      

        for (int i = 1; i < pilotInfos.Length; i++)
        {
            pilotInfos[i].SetPilotDefinition(GlobalDataManager.Instance.instantActionGlobalData.squadPilotDefinitions[i - 1]);
        }

        PilotDefinition[] pilotDefinitions = ResourceManager.Instance.GetPilotDefinitions();

        if (pilotDefinitions.Length > 0)
        {
            List<PilotDefinition> pilotDefinitionsList = new List<PilotDefinition>();
            pilotDefinitionsList.Add(null);
            pilotDefinitionsList.AddRange(pilotDefinitions);

            instantActionPilotSelectionMenu.BuildButtonList<PilotDefinition, InstantActionPilotButton>(pilotDefinitionsList.ToArray());
        }
        else
        {
            print("Error: No pilots");
        }

        UpdateButtons();

        // Setup planets

        List<PlanetDefinition> planetDefinitions = ResourceManager.Instance.GetPlanetDefinitions();

        if (planetDefinitions.Count > 0)
        {
            instantActionPlanetMenu.Initialize(SelectPlanet, planetDefinitions);

            if (GlobalDataManager.Instance.instantActionGlobalData.selectedPlanetDefinition != null)
            {
                instantActionPlanetMenu.SelectPlanet(GlobalDataManager.Instance.instantActionGlobalData.selectedPlanetDefinition);               
            }
            else
            {
                GlobalDataManager.Instance.instantActionGlobalData.selectedPlanetDefinition = planetDefinitions[0];
            }

            planetButtonText.text = GlobalDataManager.Instance.instantActionGlobalData.selectedPlanetDefinition.GetDisplayName();

            // Setup Maps

            List<MapDefinition> maps = GlobalDataManager.Instance.instantActionGlobalData.selectedPlanetDefinition.GetMaps();

            if (maps.Count > 0)
            {
                instantActionMapMenu.Initialize(SelectMap, maps);

                if (GlobalDataManager.Instance.instantActionGlobalData.selectedMapDefinition != null)
                {
                    instantActionMapMenu.SelectMap(GlobalDataManager.Instance.instantActionGlobalData.selectedMapDefinition);
                }
                else
                {
                    GlobalDataManager.Instance.instantActionGlobalData.selectedMapDefinition = maps[0];
                }

                mapButtonText.text = GlobalDataManager.Instance.instantActionGlobalData.selectedMapDefinition.GetDisplayName();

                mapImage.overrideSprite = StaticHelper.GetSpriteUI(GlobalDataManager.Instance.instantActionGlobalData.selectedMapDefinition.GetMapTexture());

                if (GlobalDataManager.Instance.instantActionGlobalData.selectedMapDefinition.SkyWeatherElements.Length > 0)
                {
                    if (GlobalDataManager.Instance.instantActionGlobalData.selectedWeatherIndex == -1)
                    {
                        GlobalDataManager.Instance.instantActionGlobalData.selectedWeatherIndex = 0;
                    }

                    weatherValueText.text = GlobalDataManager.Instance.instantActionGlobalData.selectedMapDefinition.SkyWeatherElements[GlobalDataManager.Instance.instantActionGlobalData.selectedWeatherIndex].GetDisplayName();
                    weatherIncrementButton.interactable = true;
                    weatherDecrementButton.interactable = true;
                }
                else
                {
                    weatherValueText.text = "N/A";
                    GlobalDataManager.Instance.instantActionGlobalData.selectedWeatherIndex = -1;
                    weatherIncrementButton.interactable = false;
                    weatherDecrementButton.interactable = false;
                }

                if (GlobalDataManager.Instance.instantActionGlobalData.selectedMapDefinition.SkyTimeElements.Length > 0)
                {
                    if (GlobalDataManager.Instance.instantActionGlobalData.selectedTimeOfDayIndex == -1)
                    {
                        GlobalDataManager.Instance.instantActionGlobalData.selectedTimeOfDayIndex = 0;
                    }
                  
                    timeOfDayValue.text = GlobalDataManager.Instance.instantActionGlobalData.selectedMapDefinition.SkyTimeElements[GlobalDataManager.Instance.instantActionGlobalData.selectedTimeOfDayIndex].GetDisplayName();
                    timeOfDayIncrementButton.interactable = true;
                    timeOfDayDecrementButton.interactable = true;
                }
                else
                {
                    timeOfDayValue.text = "N/A";
                    GlobalDataManager.Instance.instantActionGlobalData.selectedTimeOfDayIndex = -1;
                    timeOfDayIncrementButton.interactable = false;
                    timeOfDayDecrementButton.interactable = false;
                }

                // Setup Mission
                MissionDefinition missionDefinition = GlobalDataManager.Instance.instantActionGlobalData.selectedMapDefinition.GetCustomMissionDefinition();

                if (missionDefinition != null)
                {
                    GlobalDataManager.Instance.instantActionGlobalData.selectedMissionDefintion = missionDefinition;

                    weatherIncrementButton.interactable = false;
                    weatherDecrementButton.interactable = false;
                    timeOfDayIncrementButton.interactable = false;
                    timeOfDayDecrementButton.interactable = false;
                }
                else
                {
                    if (GlobalDataManager.Instance.instantActionGlobalData.selectedMapDefinition.MissionTypes.Length > 0)
                    {
                        instantActionMissionMenu.Initialize(SelectMissionType, GlobalDataManager.Instance.instantActionGlobalData.selectedMapDefinition.MissionTypes, GlobalDataManager.Instance.instantActionGlobalData.selectedMissionType);

                        if (GlobalDataManager.Instance.instantActionGlobalData.selectedMissionType != MissionType.None)
                        {
                            instantActionMissionMenu.SelectMission(GlobalDataManager.Instance.instantActionGlobalData.selectedMissionType);
                        }
                        else
                        {
                            GlobalDataManager.Instance.instantActionGlobalData.selectedMissionType = GlobalDataManager.Instance.instantActionGlobalData.selectedMapDefinition.MissionTypes[0];
                        }

                        SelectMissionType(GlobalDataManager.Instance.instantActionGlobalData.selectedMissionType);
                    }
                }
            }
        }

        List<FactionDefinition> factionDefinitions = ResourceManager.Instance.GetFactionDefinitions();

        if (factionDefinitions.Count > 0)
        {
            instantActionFactionMenu.Initialize(SelectFaction, factionDefinitions);

            if (GlobalDataManager.Instance.instantActionGlobalData.selectedEnemyFaction == null)
            {
                GlobalDataManager.Instance.instantActionGlobalData.selectedEnemyFaction = factionDefinitions[0];
            }

            if (GlobalDataManager.Instance.instantActionGlobalData.selectedSecondFaction == null)
            {
                GlobalDataManager.Instance.instantActionGlobalData.selectedSecondFaction = factionDefinitions[factionDefinitions.Count - 1];
            }

            enemyFactionButtonText.text = GlobalDataManager.Instance.instantActionGlobalData.selectedEnemyFaction.GetDisplayName();
            secondFactionButtonText.text = GlobalDataManager.Instance.instantActionGlobalData.selectedSecondFaction.GetDisplayName();
        }

        if (ResourceManager.Instance.MissionDifficultyConfig.MissionDifficultyTiers.Length > 0)
        {
            missionDifficultyValueText.text = "Tier " + (GlobalDataManager.Instance.instantActionGlobalData.difficultyTierIndex + 1).ToString();
        }
        else
        {
            missionDifficultyValueText.text = "Error";
        }

        if (GlobalDataManager.Instance.instantActionGlobalData.selectedMissionDefintion != null)
        {
            SelectMissionDefinition(GlobalDataManager.Instance.instantActionGlobalData.selectedMissionDefintion);
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    void OnEnable()
    {
        Time.timeScale = 1.0f;
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;

#if UNITY_EDITOR
        Cursor.lockState = CursorLockMode.None;
#endif
    }

    void OnDisable()
    {
        StoreInstantActionSettings();
    }

    public void UpdateButtons()
    {
        foreach (InstantActionPilotInfo instantActionPilotInfo in pilotInfos)
        {
            instantActionPilotInfo.UpdateButtonTexts();
        }
    }

    public void ChangeSelectedPilot(PilotDefinition pilotDefinition)
    {
        AudioManager.Instance.PlayButtonClick(0);

        currentInstantActionPilotInfo.SetPilotDefinition(pilotDefinition);

        UpdateButtons();

        instantActionPilotSelectionMenu.gameObject.SetActive(false);
    }

    public void ChangeSelectedMech(MechChassisDefinition mechChassisDefinition)
    {
        AudioManager.Instance.PlayButtonClick(0);

        currentInstantActionPilotInfo.SetMechData(new MechData(mechChassisDefinition.GetDefaultDesign()));

        UpdateButtons();

        mechChassisSelectionMenu.gameObject.SetActive(false);
    }

    public void ChangeSelectedDesign(MechDesign mechDesign, bool doNotTakeFromInventory)
    {
        AudioManager.Instance.PlayButtonClick(0);

        currentMechData.BuildFromDesign(mechDesign);

        UpdateButtons();

        mechDesignLoadMenu.gameObject.SetActive(false);
    }

    public void ClickPilotSelectButton(PilotDefinition pilotDefinition, InstantActionPilotInfo instantActionPilotInfo)
    {
        AudioManager.Instance.PlayButtonClick(0);

        currentInstantActionPilotInfo = instantActionPilotInfo;

        instantActionPilotSelectionMenu.gameObject.SetActive(true);
        instantActionPilotSelectionMenu.SelectElement(pilotDefinition);
    }

    public void SelectMechChassis(InstantActionPilotInfo instantActionPilotInfo)
    {
        AudioManager.Instance.PlayButtonClick(0);

        currentInstantActionPilotInfo = instantActionPilotInfo;

        mechChassisSelectionMenu.gameObject.SetActive(true);
        mechChassisSelectionMenu.SelectMechChassis(currentInstantActionPilotInfo.MechData.MechChassis);
    }

    public void ClickDesignSelectButton(MechData mechData)
    {
        AudioManager.Instance.PlayButtonClick(0);

        currentMechData = mechData;

        mechDesignLoadMenu.BuildDesignsList(ResourceManager.Instance.GetMechDesignList(mechData.MechChassis.Key));
        mechDesignLoadMenu.SetCallBackFunction(ChangeSelectedDesign);
        mechDesignLoadMenu.SelectMechDesign(mechData.MechChassis.Key, mechData.designName);
        mechDesignLoadMenu.gameObject.SetActive(true);
    }

    public void ClickCustomizeButton(MechData mechData)
    {
        AudioManager.Instance.PlayButtonClick(0);

        GlobalDataManager.Instance.mechDataCustomizing = mechData;
        GlobalDataManager.Instance.backSceneName = "InstantAction";
        GlobalDataManager.Instance.inventoryCurrent = Inventory.GetInstantActionInventory();

        LoadingScreen.Instance.LoadScene("MechCustomizingScreen");
    }

    void StoreInstantActionSettings()
    {
        for (int i = 1; i < pilotInfos.Length; i++)
        {
            GlobalDataManager.Instance.instantActionGlobalData.squadPilotDefinitions[i - 1] = pilotInfos[i].pilotDefinition;
        }
    }

    public void SelectPlanet(PlanetDefinition planetDefinition)
    {
        GlobalDataManager.Instance.instantActionGlobalData.selectedPlanetDefinition = planetDefinition;

        planetButtonText.text = planetDefinition.GetDisplayName();

        List<MapDefinition> maps = planetDefinition.GetMaps();

        instantActionMapMenu.Initialize(SelectMap, maps);

        if (maps.Count > 0)
        {
            SelectMap(maps[0]);
        }
    }

    public void SelectMap(MapDefinition mapDefinition)
    {
        GlobalDataManager.Instance.instantActionGlobalData.selectedMapDefinition = mapDefinition;

        mapButtonText.text = mapDefinition.GetDisplayName();

        mapImage.overrideSprite = StaticHelper.GetSpriteUI(mapDefinition.GetMapTexture());

        if (mapDefinition.SkyWeatherElements.Length > 0)
        {
            GlobalDataManager.Instance.instantActionGlobalData.selectedWeatherIndex = 0;
            weatherValueText.text = mapDefinition.SkyWeatherElements[GlobalDataManager.Instance.instantActionGlobalData.selectedWeatherIndex].GetDisplayName();
            weatherIncrementButton.interactable = true;
            weatherDecrementButton.interactable = true;
        }
        else
        {
            weatherValueText.text = "N/A";
            GlobalDataManager.Instance.instantActionGlobalData.selectedWeatherIndex = -1;
            weatherIncrementButton.interactable = false;
            weatherDecrementButton.interactable = false;
        }

        if (mapDefinition.SkyTimeElements.Length > 0)
        {
            GlobalDataManager.Instance.instantActionGlobalData.selectedTimeOfDayIndex = 0;
            timeOfDayValue.text = mapDefinition.SkyTimeElements[GlobalDataManager.Instance.instantActionGlobalData.selectedTimeOfDayIndex].GetDisplayName();
            timeOfDayIncrementButton.interactable = true;
            timeOfDayDecrementButton.interactable = true;
        }
        else
        {
            timeOfDayValue.text = "N/A";
            GlobalDataManager.Instance.instantActionGlobalData.selectedTimeOfDayIndex = -1;
            timeOfDayIncrementButton.interactable = false;
            timeOfDayDecrementButton.interactable = false;
        }

        MissionDefinition missionDefinition = mapDefinition.GetCustomMissionDefinition();     

        if (missionDefinition != null)
        {
            SelectMissionDefinition(missionDefinition);

            weatherIncrementButton.interactable = false;
            weatherDecrementButton.interactable = false;
            timeOfDayIncrementButton.interactable = false;
            timeOfDayDecrementButton.interactable = false;
        }
        else if (mapDefinition.MissionTypes.Length > 0)
        {
            instantActionMissionMenu.Initialize(SelectMissionType, mapDefinition.MissionTypes, GlobalDataManager.Instance.instantActionGlobalData.selectedMissionType);

            SelectMissionType(mapDefinition.MissionTypes[0]);
        }
    }

    public void SelectMissionType(MissionType missionType)
    {        
        GlobalDataManager.Instance.instantActionGlobalData.selectedMissionType = missionType;
        GlobalDataManager.Instance.instantActionGlobalData.selectedMissionDefintion = null;

        missionButtonText.text = StaticHelper.GetMissionTypeName(missionType);

        missionTypeButton.interactable = true;
        enemyFactionButton.interactable = true;

        difficultyDecreaseButton.interactable = true;
        difficultyIncreaseButton.interactable = true;

        switch (missionType)
        {
            case MissionType.BattleSupport:
                {
                    secondFactionButton.interactable = true;
                    break;
                }
            case MissionType.BaseDefend:
                {
                    secondFactionButton.interactable = true;
                    break;
                }
            case MissionType.ConvoyEscort:
                {
                    secondFactionButton.interactable = true;
                    break;
                }
            default:
                {
                    secondFactionButton.interactable = false;
                    break;
                }
        }
    }

    public void SelectFaction(FactionDefinition factionDefinition)
    {
        if (secondFaction)
        {
            GlobalDataManager.Instance.instantActionGlobalData.selectedSecondFaction = factionDefinition;

            secondFactionButtonText.text = factionDefinition.GetDisplayName();
        }
        else
        {
            GlobalDataManager.Instance.instantActionGlobalData.selectedEnemyFaction = factionDefinition;

            enemyFactionButtonText.text = factionDefinition.GetDisplayName();
        }
    }

    void SelectMissionDefinition(MissionDefinition missionDefinition)
    {
        GlobalDataManager.Instance.instantActionGlobalData.selectedMissionDefintion = missionDefinition;
        GlobalDataManager.Instance.instantActionGlobalData.selectedMissionType = MissionType.None;

        missionButtonText.text = missionDefinition.GetDisplayName();
        missionTypeButton.interactable = false;

        enemyFactionButton.interactable = false;
        secondFactionButton.interactable = false;

        difficultyDecreaseButton.interactable = false;
        difficultyIncreaseButton.interactable = false;

        SetDifficultyTier(missionDefinition.DifficultyTier);
    }

    void SetDifficultyTier(int tier)
    {
        GlobalDataManager.Instance.instantActionGlobalData.SetDifficultyTier(tier);

        missionDifficultyValueText.text = "Tier " + (GlobalDataManager.Instance.instantActionGlobalData.difficultyTierIndex + 1).ToString();
    }

    public void ClickPlanetButton()
    {
        instantActionPlanetMenu.gameObject.SetActive(true);

        AudioManager.Instance.PlayButtonClick(0);
    }

    public void ClickMapButton()
    {
        instantActionMapMenu.gameObject.SetActive(true);

        AudioManager.Instance.PlayButtonClick(0);
    }

    public void ClickMissionTypeButton()
    {
        instantActionMissionMenu.gameObject.SetActive(true);

        AudioManager.Instance.PlayButtonClick(0);
    }

    public void ClickWeatherIncrementButton()
    {
        if (GlobalDataManager.Instance.instantActionGlobalData.selectedMapDefinition.SkyWeatherElements.Length > 0)
        {
            GlobalDataManager.Instance.instantActionGlobalData.selectedWeatherIndex++;

            if (GlobalDataManager.Instance.instantActionGlobalData.selectedWeatherIndex == GlobalDataManager.Instance.instantActionGlobalData.selectedMapDefinition.SkyWeatherElements.Length)
            {
                GlobalDataManager.Instance.instantActionGlobalData.selectedWeatherIndex = 0;
            }

            weatherValueText.text = GlobalDataManager.Instance.instantActionGlobalData.selectedMapDefinition.SkyWeatherElements[GlobalDataManager.Instance.instantActionGlobalData.selectedWeatherIndex].GetDisplayName();
        }

        AudioManager.Instance.PlayButtonClick(0);
    }

    public void ClickWeatherDecrementButton()
    {
        if (GlobalDataManager.Instance.instantActionGlobalData.selectedMapDefinition.SkyWeatherElements.Length > 0)
        {
            GlobalDataManager.Instance.instantActionGlobalData.selectedWeatherIndex--;

            if (GlobalDataManager.Instance.instantActionGlobalData.selectedWeatherIndex < 0)
            {
                GlobalDataManager.Instance.instantActionGlobalData.selectedWeatherIndex = GlobalDataManager.Instance.instantActionGlobalData.selectedMapDefinition.SkyWeatherElements.Length - 1;
            }

            weatherValueText.text = GlobalDataManager.Instance.instantActionGlobalData.selectedMapDefinition.SkyWeatherElements[GlobalDataManager.Instance.instantActionGlobalData.selectedWeatherIndex].GetDisplayName();
        }

        AudioManager.Instance.PlayButtonClick(0);
    }

    public void ClickTimeOfDayIncrementButton()
    {
        if (GlobalDataManager.Instance.instantActionGlobalData.selectedMapDefinition.SkyTimeElements.Length > 0)
        {
            GlobalDataManager.Instance.instantActionGlobalData.selectedTimeOfDayIndex++;

            if (GlobalDataManager.Instance.instantActionGlobalData.selectedTimeOfDayIndex == GlobalDataManager.Instance.instantActionGlobalData.selectedMapDefinition.SkyTimeElements.Length)
            {
                GlobalDataManager.Instance.instantActionGlobalData.selectedTimeOfDayIndex = 0;
            }

            timeOfDayValue.text = GlobalDataManager.Instance.instantActionGlobalData.selectedMapDefinition.SkyTimeElements[GlobalDataManager.Instance.instantActionGlobalData.selectedTimeOfDayIndex].GetDisplayName();
        }

        AudioManager.Instance.PlayButtonClick(0);
    }

    public void ClickTimeOfDayDecrementButton()
    {
        if (GlobalDataManager.Instance.instantActionGlobalData.selectedMapDefinition.SkyTimeElements.Length > 0)
        {
            GlobalDataManager.Instance.instantActionGlobalData.selectedTimeOfDayIndex--;

            if (GlobalDataManager.Instance.instantActionGlobalData.selectedTimeOfDayIndex < 0)
            {
                GlobalDataManager.Instance.instantActionGlobalData.selectedTimeOfDayIndex = GlobalDataManager.Instance.instantActionGlobalData.selectedMapDefinition.SkyTimeElements.Length - 1;
            }

            timeOfDayValue.text = GlobalDataManager.Instance.instantActionGlobalData.selectedMapDefinition.SkyTimeElements[GlobalDataManager.Instance.instantActionGlobalData.selectedTimeOfDayIndex].GetDisplayName();
        }

        AudioManager.Instance.PlayButtonClick(0);
    }

    public void ClickEnemyFactionButton()
    {
        secondFaction = false;

        instantActionFactionMenu.SelectFaction(GlobalDataManager.Instance.instantActionGlobalData.selectedEnemyFaction);

        instantActionFactionMenu.gameObject.SetActive(true);

        AudioManager.Instance.PlayButtonClick(0);
    }

    public void ClickSecondFactionButton()
    {
        secondFaction = true;

        instantActionFactionMenu.SelectFaction(GlobalDataManager.Instance.instantActionGlobalData.selectedSecondFaction);

        instantActionFactionMenu.gameObject.SetActive(true);

        AudioManager.Instance.PlayButtonClick(0);
    }

    public void ClickEnemyCombatValueIncrease()
    {
        GlobalDataManager.Instance.instantActionGlobalData.IncrementDifficultyTier();

        missionDifficultyValueText.text = "Tier " + (GlobalDataManager.Instance.instantActionGlobalData.difficultyTierIndex + 1).ToString();

        AudioManager.Instance.PlayButtonClick(0);
    }

    public void ClickEnemyCombatValueDecrease()
    {
        GlobalDataManager.Instance.instantActionGlobalData.DecrementDifficultyTier();

        missionDifficultyValueText.text = "Tier " + (GlobalDataManager.Instance.instantActionGlobalData.difficultyTierIndex + 1).ToString();

        AudioManager.Instance.PlayButtonClick(0);
    }

    public void ClickBackButton()
    {
        AudioManager.Instance.PlayButtonClick(0);

        LoadingScreen.Instance.LoadScene("MainMenu");
    }

    public void ClickLaunchButton()
    {
        StoreInstantActionSettings();

        GlobalDataManager.Instance.backSceneName = "InstantAction";

        MechData mechData = new MechData(GlobalDataManager.Instance.instantActionGlobalData.playerMechData.MechDesign);
        mechData.mechPaintScheme = GlobalDataManager.Instance.instantActionGlobalData.playerMechData.mechPaintScheme;

        GlobalData.Instance.PlayerMechSetup = new MechData(GlobalDataManager.Instance.instantActionGlobalData.playerMechData.MechDesign).MechSave;

        MechPilot[] squadPilots = new MechPilot[11];

        for (int i = 0; i < 11; i++)
        {
            if (GlobalDataManager.Instance.instantActionGlobalData.squadPilotDefinitions[i] != null)
            {
                squadPilots[i] = new MechPilot(GlobalDataManager.Instance.instantActionGlobalData.squadPilotDefinitions[i]);
            }
        }

        GlobalData.Instance.SetSquadMatesSetup(GlobalDataManager.Instance.instantActionGlobalData.squadMateMechDatas);
        GlobalData.Instance.SetSquadPilotsSetup(squadPilots);


        GlobalData.Instance.MissionSetup.GenerateSeed();
        GlobalData.Instance.MissionSetup.planetDefinition = GlobalDataManager.Instance.instantActionGlobalData.selectedPlanetDefinition.Key;
        GlobalData.Instance.MissionSetup.mapDefinion = GlobalDataManager.Instance.instantActionGlobalData.selectedMapDefinition.Key;
        GlobalData.Instance.MissionSetup.weatherIndex = GlobalDataManager.Instance.instantActionGlobalData.selectedWeatherIndex;
        GlobalData.Instance.MissionSetup.timeOfDayIndex = GlobalDataManager.Instance.instantActionGlobalData.selectedTimeOfDayIndex;
        GlobalData.Instance.MissionSetup.missionType = GlobalDataManager.Instance.instantActionGlobalData.selectedMissionType;
        GlobalData.Instance.MissionSetup.difficultyTier = GlobalDataManager.Instance.instantActionGlobalData.difficultyTierIndex;
        GlobalData.Instance.MissionSetup.enemyFactionDefinition = GlobalDataManager.Instance.instantActionGlobalData.selectedEnemyFaction.Key;

        if (GlobalDataManager.Instance.instantActionGlobalData.selectedSecondFaction != null)
        {
            GlobalData.Instance.MissionSetup.secondFactionDefinition = GlobalDataManager.Instance.instantActionGlobalData.selectedSecondFaction.Key;
        }
        else
        {
            GlobalData.Instance.MissionSetup.secondFactionDefinition = "";
        }

        //MissionData missionData = new MissionData();
        //missionData.Build(GlobalDataManager.Instance.instantActionGlobalData.selectedMapDefinition, GlobalDataManager.Instance.instantActionGlobalData.selectedMissionType, GlobalDataManager.Instance.instantActionGlobalData.difficultyTierIndex, GlobalDataManager.Instance.instantActionGlobalData.selectedEnemyFaction, GlobalDataManager.Instance.instantActionGlobalData.selectedSecondFaction);

        //GlobalDataManager.Instance.currentMissionData = missionData;

        AudioManager.Instance.PlayButtonClick(1);

        LoadingScreen.Instance.LoadScene(GlobalDataManager.Instance.instantActionGlobalData.selectedMapDefinition.Scene);
    }
}