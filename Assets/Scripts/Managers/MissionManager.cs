using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MissionManager : MonoBehaviour
{
    #region Variables
    public static MissionManager Instance { get; private set; }

    [SerializeField]
    float coolingModifier = 1f;

    [SerializeField]
    float mapBounds = 200f;
	
	[SerializeField]
    Terrain terrain;

    [SerializeField]
    GameObject landingZoneNavPoint;

    [SerializeField]
    GameObject[] playerSpawnPoints;

    public MissionData MissionData { get; private set; }

    public ConvoyController ConvoyController { get; private set; }

    [Header("Missions Setups")]
    [Space(5)]

    [SerializeField]
    CustomMissionSetup customMissionSetup;

    [SerializeField]
    RandomGenMissionSetupAssassination randomGenMissionSetupAssassination;

    [SerializeField]
    RandomGenMissionSetupBaseCapture randomGenMissionSetupBaseCapture;

    [SerializeField]
    RandomGenMissionSetupBaseDefend randomGenMissionSetupBaseDefend;

    [SerializeField]
    RandomGenMissionSetupBaseDestroy randomGenMissionSetupBaseDestroy;

    [SerializeField]
    RandomGenMissionSetupBattle randomGenMissionSetupBattle;

    [SerializeField]
    RandomGenMissionSetupBattleSupport randomGenMissionSetupBattleSupport;

    [SerializeField]
    RandomGenMissionSetupConvoyDestroy randomGenMissionSetupConvoyDestroy;

    [SerializeField]
    RandomGenMissionSetupRecon randomGenMissionSetupRecon;

    [SerializeField]
    RandomGenMissionSetupSearchAndDestroy randomGenMissionSetupSearchAndDestroy;

    [Header("Active Mission Data")]
    [Space(5)]

    [SerializeField]
    SkyManager skyManager;

    [SerializeField]
    Transform[] navigationPoints;

    [SerializeField]
    Transform[] convoyWayPoints;

    [SerializeField]
    List<UnitController> enemyUnits = new List<UnitController>();

    [SerializeField]
    List<UnitController> allyUnits = new List<UnitController>();

    [SerializeField]
    List<UnitController> neutralUnits = new List<UnitController>();

    [SerializeField]
    MechController[] squadMateUnits = new MechController[0];

    [SerializeField]
    MechController[] squadCommandUnits = new MechController[0];

    [SerializeField]
    MechController[] squadSecondaryUnits = new MechController[0];

    [SerializeField]
    MechController[] squadTertiaryUnits = new MechController[0];

    [SerializeField]
    float objectiveCheckTimer = 0.0f;
    #endregion

    #region Properties
    public float CoolingModifier { get => coolingModifier; }

    public Transform[] NavigationPoints { get => navigationPoints; }

    public Transform[] ConvoyWayPoints { get => convoyWayPoints; }

    public List<UnitController> EnemyUnits { get => enemyUnits; }

    public List<UnitController> AllyUnits { get => allyUnits; }

    public List<UnitController> NeutralUnits { get => neutralUnits; }

    public MechController[] SquadMateUnits { get => squadMateUnits; }

    public MechController[] SqaudCommandUnits { get => squadCommandUnits; }

    public MechController[] SqaudSecondaryUnits { get => squadSecondaryUnits; }

    public MechController[] SqaudTertiaryUnits { get => squadTertiaryUnits; }
    #endregion

    void Awake()
    {
        //Make a Singleton
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        PoolingManager.Instance.Clear();

        Instantiate(ResourceManager.Instance.MissionCanvas);

        if (PostProcessingManager.Instance == null)
        {
            Instantiate(ResourceManager.Instance.PostProcessingManagerPrefab);
        }

        if (CameraController.Instance == null)
        {
            Instantiate(ResourceManager.Instance.CameraControllerPrefab);
        }

        terrain.gameObject.layer = LayerMask.NameToLayer("Terrain");

        GlobalData.Instance.ActiveMissionData = GlobalData.Instance.MissionSetup.BuildMissionData();

        Random.InitState(GlobalData.Instance.MissionSetup.RandomSeed);
    }

    // Use this for initialization
    void Start ()
    {
        Career career = GlobalDataManager.Instance.currentCareer;
        if (career.IsReal)
        {
            career.playerMissionMechKills = 0;
            career.playerMissionVehicleKills = 0;
        }

        MissionData = GlobalData.Instance.ActiveMissionData;

        //MissionData = GlobalDataManager.Instance.currentMissionData;


        AudioClip missionMusic = MissionData.mapDefinition.GetMissionMusic();

        if (missionMusic != null)
        {
            AudioManager.Instance.PlayClipMusic(missionMusic);
        }

        coolingModifier = 1.0f + MissionData.mapDefinition.CoolingModifier;

        GroupIntel groupIntel = new GroupIntel();

        MechControllerPlayer playerMech = CreateMech<MechControllerPlayer>(GlobalData.Instance.GetPlayerMechMission(), playerSpawnPoints[0].transform);
        playerMech.MechMetaController.ApplyMechPaintScheme(playerMech.MechData.mechPaintScheme);
        playerMech.SetGroupIntel(groupIntel);

        List<MechControllerSquadAI> squadMateUnitsList = new List<MechControllerSquadAI>();
        List<MechControllerSquadAI> squad1UnitsList = new List<MechControllerSquadAI>();
        List<MechControllerSquadAI> squad2UnitsList = new List<MechControllerSquadAI>();
        List<MechControllerSquadAI> squad3UnitsList = new List<MechControllerSquadAI>();

        MechData[] squadMechDatas = GlobalData.Instance.GetSquadMateMechsMission();

        for (int i = 0; i < 11; i++)
        {
            if (squadMechDatas[i] != null)
            {
                MechControllerSquadAI squadMateMech = CreateMech<MechControllerSquadAI>(squadMechDatas[i], playerSpawnPoints[i + 1].transform);
                squadMateMech.MechMetaController.ApplyMechPaintScheme(squadMateMech.MechData.mechPaintScheme);
                squadMateMech.SetAI(AI_Type.PlayerSquad, TeamType.Player);
                squadMateMech.SetGroupIntel(groupIntel);
                AllyUnits.Add(squadMateMech);
                squadMateUnitsList.Add(squadMateMech);

                // Split into squads
                if (i < 3) //0-2
                {
                    squad1UnitsList.Add(squadMateMech);
                }
                else if (i > 6) //7-11
                {
                    squad3UnitsList.Add(squadMateMech);
                }
                else //3-6
                {
                    squad2UnitsList.Add(squadMateMech);
                }
            }
        }

        squadMateUnits = squadMateUnitsList.ToArray();
        squadCommandUnits = squad1UnitsList.ToArray();
        squadSecondaryUnits = squad2UnitsList.ToArray();
        squadTertiaryUnits = squad3UnitsList.ToArray();

        PlayerHUD.Instance.SquadInfoGroup.SetPilots(squadCommandUnits, squadSecondaryUnits, squadTertiaryUnits);

        MechControllerPlayer.Instance.CommandSystem.UpdateInstructions();

        switch (MissionData.missionType)
        {
            case MissionType.None:
                {
                    BuildCustomMission();
                    break;
                }
            case MissionType.Battle:
                {
                    BuildBattleMission();
                    break;
                }
            case MissionType.SearchAndDestroy:
                {
                    BuildSearchAndDestroyMission();
                    break;
                }
            case MissionType.Assassination:
                {
                    BuildAssassinationMission();
                    break;
                }
            case MissionType.ConvoyDestroy:
                {
                    BuildConvoyDestroyMission();
                    break;
                }
            case MissionType.BattleSupport:
                {
                    BuildBattleSupportMission();
                    break;
                }
            case MissionType.Recon:
                {
                    BuildReconMission();
                    break;
                }
            case MissionType.ConvoyCapture:
                {
                    BuildConvoyCaptureMission();
                    break;
                }
            case MissionType.ConvoyEscort:
                {
                    BuildConvoyEscortMission();
                    break;
                }
            case MissionType.BaseDestroy:
                {
                    BuildBaseDestroyMission();
                    break;
                }
            case MissionType.BaseCapture:
                {
                    BuildBaseCaptureMission();
                    break;
                }
            case MissionType.BaseDefend:
                {
                    BuildBaseDefendMission();
                    break;
                }
            default:
                {
                    BuildBattleMission();
                    break;
                }
        }

        for (int i = 0; i < MissionData.objectives.Length; i++)
        {
            Objective objective = MissionData.objectives[i];

            if (objective.objectiveState == ObjectiveState.Active)
            {
                objective.SetStartTime();
            }
        }

        UpdateObjectives();

        PlayerHUD.Instance.SetObjectivesWindowVisibility(true);
        PlayerHUD.Instance.ObjectivesControllerUI.RebuildObjectivesText();

        if (navigationPoints.Length > 0)
        {
            playerMech.SetNavPoint(0, false);
        }
        else
        {
            Debug.LogError("Error no navigation points");
        }

        // Create sky and weather
        skyManager = gameObject.AddComponent<SkyManager>();

        WeatherDefinition weatherDefinition = null;
        float skyTimeOfDay = 12.0f;

        MapDefinition mapDefinition = GlobalData.Instance.MissionSetup.MapDefinition;

        if (mapDefinition != null)
        {
            if (GlobalData.Instance.MissionSetup.weatherIndex != -1)
            {
                weatherDefinition = mapDefinition.SkyWeatherElements[GlobalData.Instance.MissionSetup.weatherIndex].GetWeatherDefinition();
            }

            if (GlobalData.Instance.MissionSetup.timeOfDayIndex != -1)
            {
                skyTimeOfDay = mapDefinition.SkyTimeElements[GlobalData.Instance.MissionSetup.timeOfDayIndex].Time;
            }
        }

        skyManager.CreateSky(weatherDefinition, skyTimeOfDay, playerMech);
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.F10) && (MissionData.MissionOver))
        {
            // Store mission information in career
            Career career = GlobalDataManager.Instance.currentCareer;
            if (career.IsReal)
            {
                career.lastMissionSuccessful = MissionData.successful;

                if (MissionData.successful)
                {
                    career.currentContract.IncrementSuccessfulMissionCount();
                }


                // Store mech and pilot copies back into career
                career.Mechs[GlobalData.Instance.PlayerMechMission.guid] = GlobalData.Instance.PlayerMechMission;

                for (int i = 0; i < 11; i++)
                {
                    MechData mechData = GlobalData.Instance.SquadMechsMission[i];
                    MechPilot pilot = GlobalData.Instance.SquadPilotsMission[i];

                    if (mechData != null)
                    {
                        career.Mechs[mechData.guid] = mechData;
                    }

                    if (pilot != null)
                    {
                        career.Pilots[pilot.guid] = pilot;
                    }
                }

                career.lastMissionObjectives = MissionData.ObjectiveSaves;

                career.BuildSalvageList();

                if (!career.deceased || career.permadeath)
                {
                    career.currentScreen = "MissionSummery";
                    ResourceManager.Instance.StoreCareer(career.CareerSave);
                }
            }

            PlayerHUD.Instance.gameObject.SetActive(false);
            LoadingScreen.Instance.LoadScene(GlobalDataManager.Instance.backSceneName);

            return;
        }

        if (Time.time > objectiveCheckTimer)
        {
            objectiveCheckTimer = Time.time + 1.0f;

            UpdateObjectives();
        }
	}

    public void UpdateObjectives()
    {
        if (MissionData.MissionOver)
        {
            return;
        }

        bool objectivesChanged = false;

        for (int i = 0; i < MissionData.objectives.Length; i++)
        {
            Objective objective = MissionData.objectives[i];

            if (objective.objectiveState == ObjectiveState.Active)
            {
                if (objective.Update())
                {
                    objectivesChanged = true;
                }
            }
        }

        if (objectivesChanged)
        {
            bool allObjectivesCompleted = true;
            bool allObjectivesFailed = true;
            bool breakLoop = false;

            for (int i = 0; i < MissionData.objectives.Length; i++)
            {
                Objective objective = MissionData.objectives[i];

                switch (objective.objectiveState)
                {
                    case ObjectiveState.Active:
                        {
                            allObjectivesCompleted = false;
                            allObjectivesFailed = false;
                            break;
                        }
                    case ObjectiveState.Completed:
                        {
                            allObjectivesFailed = false;

                            if (objective.instantCompleteMission)
                            {
                                MissionData.SetCompleted();
                                breakLoop = true;
                            }
                            break;
                        }
                    case ObjectiveState.Failed:
                        {
                            if (objective.instantFailMission)
                            {
                                MissionData.SetFailed();
                                breakLoop = true;
                            }
                            break;
                        }
                }

                if (breakLoop)
                {
                    break;
                }
            }

            if (!MissionData.failed && !MissionData.successful)
            {
                if (allObjectivesFailed)
                {
                    MissionData.SetFailed();
                }
                else if (allObjectivesCompleted)
                {
                    MissionData.successful = true;
                }
            }

            if (MissionData.MissionOver)
            {
                PlayerHUD.Instance.SetObjectivesWindowVisibility(true);
            }
            
            PlayerHUD.Instance.ObjectivesControllerUI.RebuildObjectivesText();
            PlayerHUD.Instance.SetObjectiveUpdated();
            MechControllerPlayer.Instance.PlayObjectiveUpdated();
        }
    }

    public void AddUnit(UnitController unitController)
    {
        switch (unitController.Team)
        {
            case TeamType.Player:
                {
                    allyUnits.Add(unitController);
                    break;
                }
            case TeamType.Enemy:
                {
                    enemyUnits.Add(unitController);
                    break;
                }
            case TeamType.Neutral:
                {
                    neutralUnits.Add(unitController);
                    break;
                }
            default:
                {
                    enemyUnits.Add(unitController);
                    break;
                }
        }
    }

    void BuildCustomMission()
    {
        navigationPoints = customMissionSetup.navigationPoints;

        customMissionSetup.customUnitsSetup.BuildUnits();

        MissionData.objectives = customMissionSetup.customObjectivesSetup.GetObjectives();
    }

    void BuildBattleMission()
    {
        if (randomGenMissionSetupBattle != null)
        {
            navigationPoints = randomGenMissionSetupBattle.navigationPoints;

            UnitData[] enemyUnitDatas = MissionData.objectives[0].units;

            GameObject[] selectedEnemySpawnPoints = StaticHelper.ShuffleList(randomGenMissionSetupBattle.spawnPointsEnemy).Take(enemyUnitDatas.Length).ToArray();

            SpawnUnits(enemyUnitDatas, selectedEnemySpawnPoints, AI_Type.Skirmish, TeamType.Enemy, enemyUnits, new GroupIntel());
        }
        else
        {
            Debug.LogError("Error: Battle Mission Setup not assigned");
        }
    }

    void BuildSearchAndDestroyMission()
    {
        if (randomGenMissionSetupSearchAndDestroy != null)
        {
            navigationPoints = randomGenMissionSetupSearchAndDestroy.navigationPoints;

            for (int index = 0; index < randomGenMissionSetupSearchAndDestroy.spawnPointGroupsEnemy.Length; index++)
            {
                GameObject[] selectedEnemySpawnPoints = StaticHelper.ShuffleList(randomGenMissionSetupSearchAndDestroy.spawnPointGroupsEnemy[index].array).Take(MissionData.objectives[index].units.Length).ToArray();

                GroupIntel groupIntel = new GroupIntel();

                for (int i = 0; i < selectedEnemySpawnPoints.Length; i++)
                {
                    if (MissionData.objectives[index].units[i] is MechData)
                    {
                        MechControllerGeneralAI enemyMech = CreateMech<MechControllerGeneralAI>(MissionData.objectives[index].units[i] as MechData, selectedEnemySpawnPoints[i].transform);
                        enemyMech.SetAI(AI_Type.Skirmish, TeamType.Enemy);
                        enemyMech.MechMetaController.ApplyMechPaintScheme(enemyMech.MechData.mechPaintScheme);
                        enemyMech.SetGroupIntel(groupIntel);
                        enemyUnits.Add(enemyMech);
                    }
                    else if (MissionData.objectives[index].units[i] is GroundVehicleData)
                    {
                        GroundVehicleController enemyGroundVehicle = CreateGroundVehicle(MissionData.objectives[index].units[i] as GroundVehicleData, selectedEnemySpawnPoints[i].transform);
                        enemyGroundVehicle.SetAI(AI_Type.Skirmish, TeamType.Enemy);
                        enemyGroundVehicle.SetGroupIntel(groupIntel);
                        enemyUnits.Add(enemyGroundVehicle);
                    }
                }
            }
        }
        else
        {
            Debug.LogError("Error: Search and Destroy Mission Setup not assigned");
        }
    }

    void BuildAssassinationMission()
    {
        if (randomGenMissionSetupAssassination != null)
        {
            navigationPoints = randomGenMissionSetupAssassination.navigationPoints;

            UnitData[] enemyUnitDatas = MissionData.objectives[7].units;

            GameObject assassinationSpawnPoint = StaticHelper.ShuffleList(randomGenMissionSetupAssassination.spawnPointsAssassinationTarget)[0];

            GameObject[] selectedEnemySpawnPoints = StaticHelper.ShuffleList(randomGenMissionSetupAssassination.spawnPointsEnemy).Take(enemyUnitDatas.Length - 1).ToArray();

            for (int i = 0; i < selectedEnemySpawnPoints.Length + 1; i++)
            {
                GameObject enemySpawnPoint;

                if (i == 0)
                {
                    enemySpawnPoint = assassinationSpawnPoint;
                }
                else
                {
                    enemySpawnPoint = selectedEnemySpawnPoints[i - 1];
                }

                if (enemyUnitDatas[i] is MechData)
                {
                    MechControllerGeneralAI enemyMech = CreateMech<MechControllerGeneralAI>(enemyUnitDatas[i] as MechData, enemySpawnPoint.transform);
                    enemyMech.SetAI(AI_Type.Skirmish, TeamType.Enemy);
                    enemyMech.MechMetaController.ApplyMechPaintScheme(enemyMech.MechData.mechPaintScheme);
                    enemyMech.SetGroupIntel(new GroupIntel());
                    enemyUnits.Add(enemyMech);
                }
                else if (enemyUnitDatas[i] is GroundVehicleData)
                {
                    GroundVehicleController enemyGroundVehicle = CreateGroundVehicle(enemyUnitDatas[i] as GroundVehicleData, enemySpawnPoint.transform);
                    enemyGroundVehicle.SetAI(AI_Type.Skirmish, TeamType.Enemy);
                    enemyGroundVehicle.SetGroupIntel(new GroupIntel());
                    enemyUnits.Add(enemyGroundVehicle);
                }
            }
        }
        else
        {
            Debug.LogError("Error: Assassination Mission Setup not assigned");
        }
    }

    void BuildConvoyDestroyMission()
    {
        if (randomGenMissionSetupConvoyDestroy != null)
        {
            UnitData[] enemyUnitDatas = MissionData.objectives[4].units;
            List<GroundVehicleController> convoyUnits = new List<GroundVehicleController>();

            navigationPoints = randomGenMissionSetupConvoyDestroy.navigationPoints;
            convoyWayPoints = randomGenMissionSetupConvoyDestroy.wayPointsConvoy;

            GameObject[] selectedEnemySpawnPoints = StaticHelper.ShuffleList(randomGenMissionSetupConvoyDestroy.spawnPointsEnemy).Take(enemyUnitDatas.Length).ToArray();

            for (int i = 0; i < selectedEnemySpawnPoints.Length; i++)
            {
                GameObject enemySpawnPoint = selectedEnemySpawnPoints[i];
                UnitData enemyUnitData = enemyUnitDatas[i];

                if (enemyUnitData is MechData)
                {
                    MechControllerGeneralAI enemyMech = CreateMech<MechControllerGeneralAI>(enemyUnitData as MechData, enemySpawnPoint.transform);
                    enemyMech.SetAI(AI_Type.Skirmish, TeamType.Enemy);
                    enemyMech.MechMetaController.ApplyMechPaintScheme(enemyMech.MechData.mechPaintScheme);
                    enemyMech.SetGroupIntel(new GroupIntel());
                    enemyUnits.Add(enemyMech);
                }
                else if (enemyUnitData is GroundVehicleData)
                {
                    GroundVehicleController enemyGroundVehicle = CreateGroundVehicle(enemyUnitData as GroundVehicleData, enemySpawnPoint.transform);
                    enemyGroundVehicle.SetAI(AI_Type.Skirmish, TeamType.Enemy);
                    enemyGroundVehicle.SetGroupIntel(new GroupIntel());
                    enemyUnits.Add(enemyGroundVehicle);
                }
            }

            for (int i = 0; i < MissionData.objectives[1].units.Length; i++)
            {
                GroundVehicleController convoyVehicle = CreateGroundVehicle(MissionData.objectives[1].units[i] as GroundVehicleData, randomGenMissionSetupConvoyDestroy.spawnPointsConvoyEnemy[i].transform);
                convoyVehicle.SetAI(AI_Type.Convoy, TeamType.Enemy);
                convoyVehicle.SetGroupIntel(new GroupIntel());
                enemyUnits.Add(convoyVehicle);
                convoyUnits.Add(convoyVehicle);
            }

            ConvoyController = new ConvoyController(convoyUnits);
        }
        else
        {
            Debug.LogError("Error: Convoy Destroy Mission Setup not assigned");
        }
    }

    void BuildBattleSupportMission()
    {
        if (randomGenMissionSetupBattleSupport != null)
        {
            navigationPoints = randomGenMissionSetupBattleSupport.navigationPoints;

            GameObject[] selectedEnemySpawnPoints = StaticHelper.ShuffleList(randomGenMissionSetupBattleSupport.spawnPointsEnemy).Take(MissionData.objectives[0].units.Length).ToArray();

            SpawnUnits(MissionData.objectives[0].units, selectedEnemySpawnPoints, AI_Type.Skirmish, TeamType.Enemy, enemyUnits, new GroupIntel());

            GameObject[] selectedAllySpawnPoints = StaticHelper.ShuffleList(randomGenMissionSetupBattleSupport.spawnPointsAlly).Take(MissionData.objectives[1].units.Length).ToArray();

            SpawnUnits(MissionData.objectives[1].units, selectedAllySpawnPoints, AI_Type.Skirmish, TeamType.Player, allyUnits, new GroupIntel());
        }
        else
        {
            Debug.LogError("Error: Battle Support Mission Setup not assigned");
        }
    }

    void BuildReconMission()
    {
        if (randomGenMissionSetupRecon != null)
        {
            navigationPoints = randomGenMissionSetupRecon.navigationPoints;

            UnitData[] enemyUnitDatas = MissionData.objectives[5].units;

            GameObject[] selectedEnemySpawnPoints = StaticHelper.ShuffleList(randomGenMissionSetupRecon.spawnPointsEnemy).Take(enemyUnitDatas.Length).ToArray();

            SpawnUnits(enemyUnitDatas, selectedEnemySpawnPoints, AI_Type.Skirmish, TeamType.Enemy, enemyUnits, new GroupIntel());
        }
        else
        {
            Debug.LogError("Error: Recon Mission Setup not assigned");
        }
    }

    void BuildConvoyCaptureMission()
    {

    }

    void BuildConvoyEscortMission()
    {

    }

    void BuildBaseDestroyMission()
    {
        if (randomGenMissionSetupBaseDestroy != null)
        {
            navigationPoints = randomGenMissionSetupBaseDestroy.navigationPoints;

            List<UnitData> spawnPointEnemies = new List<UnitData>();

            UnitData[] enemyUnitsAll = MissionData.objectives[5].units;

            UnitData[] turretUnitDatas = MissionData.objectives[2].units;

            UnitData primaryBuildingUnitData = MissionData.objectives[0].units[0];

            UnitData[] allBuildings = MissionData.objectives[1].units;

            GroupIntel groupIntelAll = new GroupIntel();

            // Filter out the enemies that are spawned at spawn points
            for (int i = 0; i < enemyUnitsAll.Length; i++)
            {
                UnitData enemyUnit = enemyUnitsAll[i];

                if (!(enemyUnit is TurretUnitData) && !(enemyUnit is BuildingData))
                {
                    spawnPointEnemies.Add(enemyUnit);
                }
            }

            GameObject[] selectedEnemySpawnPoints = StaticHelper.ShuffleList(randomGenMissionSetupBaseDestroy.spawnPointsEnemy).Take(spawnPointEnemies.Count).ToArray();

            // Spawn normal enemies
            SpawnUnits(spawnPointEnemies.ToArray(), selectedEnemySpawnPoints, AI_Type.Skirmish, TeamType.Enemy, enemyUnits, groupIntelAll);

            GameObject[] turretSpawnPoints = StaticHelper.ShuffleList(randomGenMissionSetupBaseDestroy.spawnPointsTurret).Take(turretUnitDatas.Length).ToArray();

            TurretUnitController[] turretUnitControllers = SpawnTurrets(turretUnitDatas, turretSpawnPoints, AI_Type.Skirmish, TeamType.Enemy, enemyUnits, groupIntelAll);

            BaseFoundationController baseFoundationController = CreateBaseFoundation(MissionData.baseFoundationPrefab, randomGenMissionSetupBaseDestroy.spawnPointBase.transform);

            BuildingController buildingController = CreateBuilding(primaryBuildingUnitData as BuildingData, TeamType.Enemy, enemyUnits);
            buildingController.transform.position = baseFoundationController.PrimaryBuildingSpots[0].position;
            buildingController.transform.rotation = baseFoundationController.PrimaryBuildingSpots[0].rotation;

            BuildingData buildingData;

            // Create and place buildings onto base foundation
            for (int i = 0; i < allBuildings.Length; i++)
            {
                buildingData = allBuildings[i] as BuildingData;
                buildingController = CreateBuilding(buildingData, TeamType.Enemy, enemyUnits);

                if (i == allBuildings.Length - 2) // Turret Tower
                {
                    buildingController.transform.position = baseFoundationController.TurretTowerSpot.position;
                    buildingController.transform.rotation = baseFoundationController.TurretTowerSpot.rotation;

                    buildingController.SetTurretsToDisableOnDie(turretUnitControllers);
                }
                else if (i == allBuildings.Length - 1) // Turret Generator
                {
                    buildingController.transform.position = baseFoundationController.PowerGeneratorSpot.position;
                    buildingController.transform.rotation = baseFoundationController.PowerGeneratorSpot.rotation;

                    buildingController.SetTurretsToDisableOnDie(turretUnitControllers);
                }
                else // Optional Buildings
                {
                    buildingController.transform.position = baseFoundationController.OptionalBuildingSpots[i].position;
                    buildingController.transform.rotation = baseFoundationController.OptionalBuildingSpots[i].rotation;
                }
            }
        }
        else
        {
            Debug.LogError("Error: Base Capture Mission Setup not assigned");
        }
    }

    void BuildBaseCaptureMission()
    {
        if (randomGenMissionSetupBaseCapture != null)
        {
            navigationPoints = randomGenMissionSetupBaseCapture.navigationPoints;

            List<UnitData> spawnPointEnemies = new List<UnitData>();

            List<UnitData> turretUnitDatas = new List<UnitData>();

            UnitData[] enemyUnitsAll = MissionData.objectives[0].units;

            UnitData primaryBuildingUnitData = MissionData.objectives[1].units[0];

            UnitData[] allBuildings = MissionData.objectives[2].units;

            GroupIntel groupIntelEnemy = new GroupIntel();
            GroupIntel groupIntelNeutralBuidlings = new GroupIntel();

            // Filter out mobile and turret units
            for (int i = 0; i < enemyUnitsAll.Length; i++)
            {
                UnitData enemyUnit = enemyUnitsAll[i];

                if (!(enemyUnit is TurretUnitData))
                {
                    spawnPointEnemies.Add(enemyUnit);
                }
                else
                {
                    turretUnitDatas.Add(enemyUnit);
                }
            }

            GameObject[] selectedEnemySpawnPoints = StaticHelper.ShuffleList(randomGenMissionSetupBaseCapture.spawnPointsEnemy).Take(spawnPointEnemies.Count).ToArray();

            // Spawn normal enemies
            SpawnUnits(spawnPointEnemies.ToArray(), selectedEnemySpawnPoints, AI_Type.Skirmish, TeamType.Enemy, enemyUnits, groupIntelEnemy);

            GameObject[] turretSpawnPoints = StaticHelper.ShuffleList(randomGenMissionSetupBaseCapture.spawnPointsTurret).Take(turretUnitDatas.Count).ToArray();

            TurretUnitController[] turretUnitControllers = SpawnTurrets(turretUnitDatas.ToArray(), turretSpawnPoints, AI_Type.Skirmish, TeamType.Enemy, enemyUnits, groupIntelEnemy);

            BaseFoundationController baseFoundationController = CreateBaseFoundation(MissionData.baseFoundationPrefab, randomGenMissionSetupBaseCapture.spawnPointBase.transform);

            BuildingController buildingController = CreateBuilding(primaryBuildingUnitData as BuildingData, TeamType.Neutral, neutralUnits);
            buildingController.transform.position = baseFoundationController.PrimaryBuildingSpots[0].position;
            buildingController.transform.rotation = baseFoundationController.PrimaryBuildingSpots[0].rotation;

            BuildingData buildingData;

            // Create and place buildings onto base foundation
            for (int i = 0; i < allBuildings.Length; i++)
            {
                buildingData = allBuildings[i] as BuildingData;
                buildingController = CreateBuilding(buildingData, TeamType.Neutral, neutralUnits);

                if (i == allBuildings.Length - 2) // Turret Tower
                {
                    buildingController.transform.position = baseFoundationController.TurretTowerSpot.position;
                    buildingController.transform.rotation = baseFoundationController.TurretTowerSpot.rotation;

                    buildingController.SetTurretsToDisableOnDie(turretUnitControllers);
                }
                else if (i == allBuildings.Length - 1) // Turret Generator
                {
                    buildingController.transform.position = baseFoundationController.PowerGeneratorSpot.position;
                    buildingController.transform.rotation = baseFoundationController.PowerGeneratorSpot.rotation;

                    buildingController.SetTurretsToDisableOnDie(turretUnitControllers);
                }
                else // Optional Buildings
                {
                    buildingController.transform.position = baseFoundationController.OptionalBuildingSpots[i].position;
                    buildingController.transform.rotation = baseFoundationController.OptionalBuildingSpots[i].rotation;
                }
            }
        }
        else
        {
            Debug.LogError("Error: Base Capture Mission Setup not assigned");
        }
    }

    void BuildBaseDefendMission()
    {
        if (randomGenMissionSetupBaseDefend != null)
        {
            navigationPoints = randomGenMissionSetupBaseDefend.navigationPoints;

            UnitData[] enemyUnitDatas = MissionData.objectives[0].units;

            UnitData[] turretUnitDatas = MissionData.objectives[3].units;

            UnitData primaryBuildingUnitData = MissionData.objectives[1].units[0];

            UnitData[] allBuildings = MissionData.objectives[2].units;

            GroupIntel groupIntelEnemy = new GroupIntel();
            GroupIntel groupIntelAllied = new GroupIntel();

            groupIntelEnemy.targetLastDetectedPosition = navigationPoints[0].position;

            GameObject[] selectedEnemySpawnPoints = StaticHelper.ShuffleList(randomGenMissionSetupBaseDefend.spawnPointsEnemy).Take(enemyUnitDatas.Length).ToArray();

            // Spawn normal enemies
            SpawnUnits(enemyUnitDatas, selectedEnemySpawnPoints, AI_Type.Skirmish, TeamType.Enemy, enemyUnits, groupIntelEnemy);

            GameObject[] turretSpawnPoints = StaticHelper.ShuffleList(randomGenMissionSetupBaseDefend.spawnPointsTurret).Take(turretUnitDatas.Length).ToArray();

            TurretUnitController[] turretUnitControllers = SpawnTurrets(turretUnitDatas.ToArray(), turretSpawnPoints, AI_Type.Skirmish, TeamType.Player, allyUnits, groupIntelAllied);

            BaseFoundationController baseFoundationController = CreateBaseFoundation(MissionData.baseFoundationPrefab, randomGenMissionSetupBaseDefend.spawnPointBase.transform);

            BuildingController buildingController = CreateBuilding(primaryBuildingUnitData as BuildingData, TeamType.Player, allyUnits);
            buildingController.transform.position = baseFoundationController.PrimaryBuildingSpots[0].position;
            buildingController.transform.rotation = baseFoundationController.PrimaryBuildingSpots[0].rotation;

            BuildingData buildingData;

            // Create and place buildings onto base foundation
            for (int i = 0; i < allBuildings.Length; i++)
            {
                buildingData = allBuildings[i] as BuildingData;
                buildingController = CreateBuilding(buildingData, TeamType.Player, allyUnits);

                if (i == allBuildings.Length - 2) // Turret Tower
                {
                    buildingController.transform.position = baseFoundationController.TurretTowerSpot.position;
                    buildingController.transform.rotation = baseFoundationController.TurretTowerSpot.rotation;

                    buildingController.SetTurretsToDisableOnDie(turretUnitControllers);
                }
                else if (i == allBuildings.Length - 1) // Turret Generator
                {
                    buildingController.transform.position = baseFoundationController.PowerGeneratorSpot.position;
                    buildingController.transform.rotation = baseFoundationController.PowerGeneratorSpot.rotation;

                    buildingController.SetTurretsToDisableOnDie(turretUnitControllers);
                }
                else // Optional Buildings
                {
                    buildingController.transform.position = baseFoundationController.OptionalBuildingSpots[i].position;
                    buildingController.transform.rotation = baseFoundationController.OptionalBuildingSpots[i].rotation;
                }
            }
        }
        else
        {
            Debug.LogError("Error: Base Defend Mission Setup not assigned");
        }
    }

    void SpawnUnits(UnitData[] unitDatas, GameObject[] spawnPoints, AI_Type aI_Type, TeamType team, List<UnitController> unitControllers, GroupIntel groupIntel)
    {
        for (int i = 0; i < spawnPoints.Length; i++)
        {
            GameObject spawnPoint = spawnPoints[i];
            UnitData unitData = unitDatas[i];
            CombatUnitController combatUnitController;

            if (unitData is MechData)
            {
                combatUnitController = CreateMech<MechControllerGeneralAI>(unitData as MechData, spawnPoint.transform);

                MechControllerGeneralAI mechController = combatUnitController as MechControllerGeneralAI;

                mechController.MechMetaController.ApplyMechPaintScheme(mechController.MechData.mechPaintScheme);
            }
            else if (unitData is GroundVehicleData)
            {
                combatUnitController = CreateGroundVehicle(unitData as GroundVehicleData, spawnPoint.transform);
            }
            else
            {
                combatUnitController = CreateGroundVehicle(new GroundVehicleData(), spawnPoint.transform);
            }

            combatUnitController.SetAI(aI_Type, team);
            
            if (groupIntel != null)
            {
                combatUnitController.SetGroupIntel(groupIntel);
            }
            else
            {
                combatUnitController.SetGroupIntel(new GroupIntel());
            }

            unitControllers.Add(combatUnitController);
        }
    }

    TurretUnitController[] SpawnTurrets(UnitData[] unitDatas, GameObject[] spawnPoints, AI_Type aI_Type, TeamType team, List<UnitController> unitControllers, GroupIntel groupIntel)
    {
        List<TurretUnitController> turretUnitControllers = new List<TurretUnitController>();

        for (int i = 0; i < unitDatas.Length; i++)
        {
            GameObject spawnPoint = spawnPoints[i];
            TurretUnitData turretUnitData = unitDatas[i] as TurretUnitData;

            TurretUnitController turretUnitController = CreateTurret(turretUnitData, spawnPoint.transform);

            turretUnitController.SetAI(aI_Type, team);

            if (groupIntel != null)
            {
                turretUnitController.SetGroupIntel(groupIntel);
            }
            else
            {
                turretUnitController.SetGroupIntel(new GroupIntel());
            }

            unitControllers.Add(turretUnitController);
            turretUnitControllers.Add(turretUnitController);
        }

        return turretUnitControllers.ToArray();
    }

    T CreateMech<T>(MechData mechData, Transform spawnPoint) where T : MechController
    {
        T mechController = ResourceManager.Instance.CreateMech<T>(mechData);

        if (mechController != null)
        {
            Vector3 position = spawnPoint.position + new Vector3(0.0f ,0.05f ,0.0f);

            mechController.transform.position = position;
            mechController.transform.rotation = spawnPoint.rotation;

            return mechController;
        }

        return null;
    }

    GroundVehicleController CreateGroundVehicle(GroundVehicleData groundVehicleData, Transform spawnPoint)
    {
        GroundVehicleController groundVehicleController = ResourceManager.Instance.CreateGroundVehicle(groundVehicleData);

        if (groundVehicleController != null)
        {
            Vector3 position = spawnPoint.position + new Vector3(0.0f, 0.05f, 0.0f);

            groundVehicleController.transform.position = position;
            groundVehicleController.transform.rotation = spawnPoint.rotation;

            return groundVehicleController;
        }

        return null;
    }

    TurretUnitController CreateTurret(TurretUnitData turretUnitData, Transform spawnPoint)
    {
        TurretUnitController turretUnitController = ResourceManager.Instance.CreateTurret(turretUnitData);

        if (turretUnitController != null)
        {
            GameObject turretFoundationObject = Instantiate(ResourceManager.Instance.GetTurretFoundationPrefab(turretUnitData.turretDefinition.UnitClass), spawnPoint);
            TurretFoundation turretFoundation = turretFoundationObject.GetComponent<TurretFoundation>();

            turretUnitController.transform.position = turretFoundation.TurretLocation.position;
            turretUnitController.transform.rotation = turretFoundation.TurretLocation.rotation;

            return turretUnitController;
        }

        return null;
    }

    BuildingController CreateBuilding(BuildingData buildingData, TeamType team, List<UnitController> unitControllers)
    {
        BuildingController buildingController = ResourceManager.Instance.CreateBuilding(buildingData);

        buildingController.SetTeam(team);

        unitControllers.Add(buildingController);

        return buildingController;
    }

    BaseFoundationController CreateBaseFoundation(GameObject baseFoundationPrefab, Transform spawnPoint)
    {
        return Instantiate(baseFoundationPrefab, spawnPoint).GetComponent<BaseFoundationController>();
    }

    public bool PlayerIsInsideBounds(Vector3 point)
    {
        if (point.x > mapBounds || point.x < -mapBounds || point.z > mapBounds || point.z < -mapBounds)
        {
            return false;
        }

        return true;
    }

    public void SetMissionFailed()
    {
        if (!MissionData.successful)
        {
            MissionData.SetFailed();

            PlayerHUD.Instance.SetObjectivesWindowVisibility(true);
            PlayerHUD.Instance.ObjectivesControllerUI.RebuildObjectivesText();
        }
    }

    public void UpdateSquadMateDisplay()
    {
        PlayerHUD.Instance.SquadInfoGroup.UpdatePilots(squadCommandUnits, squadSecondaryUnits, squadTertiaryUnits);
    }

    public bool PlayerInContactWithEnemy()
    {
        for (int i = 0; i < enemyUnits.Count; i++)
        {
            UnitController enemyUnit = enemyUnits[i];

            if (enemyUnit.IsDestroyed)
                continue;

            if (enemyUnit is CombatUnitController)
            {
                CombatUnitController enemyCombatUnit = enemyUnit as CombatUnitController;

                if (enemyCombatUnit.TargetUnit != null && enemyCombatUnit.TargetUnit.Team == TeamType.Player)
                {
                    return true;
                }
            }
        }

        return false;
    }

    public void AlertEnemiesToPlayerLocation()
    {
        Vector3 playerPosition = MechControllerPlayer.Instance.transform.position;

        for (int i = 0; i < enemyUnits.Count; i++)
        {
            UnitController enemyUnit = enemyUnits[i];

            if (enemyUnit.IsDestroyed)
                continue;

            if (enemyUnit is MobileUnitController)
            {
                MobileUnitController enemyMobileUnit = enemyUnit as MobileUnitController;

                enemyMobileUnit.GroupIntel.targetLastDetectedPosition = playerPosition;
            }
        }
    }

    

    // Editor specific code
#if UNITY_EDITOR
    #region Editor
    [Header("Editor")]
    [Space(5)]

    [SerializeField]
    MissionType showMissionSetup = MissionType.Battle;

    [SerializeField]
    bool showMapBounds;

    [SerializeField]
    bool showPlayerSpawnPoints;
	
	[SerializeField]
    bool showLandingZone = true;

    [SerializeField]
    float mapColliderBounds;

    [SerializeField]
    Material mapBorderMaterial;

    [SerializeField]
    Mesh spawnPointMesh;

    [SerializeField]
    Vector3 spawnPointScale = Vector3.one;

    [SerializeField]
    Mesh spawnPointConvoyMesh;

    [SerializeField]
    Vector3 spawnPointConvoyScale = Vector3.one;

    [SerializeField]
    Mesh baseFoundationMesh;

    [SerializeField]
    Vector3 baseFoundationScale = Vector3.one;

    [SerializeField]
    Mesh turretFoundationMesh;

    [SerializeField]
    Vector3 turretFoundationScale = Vector3.one;

    [SerializeField]
    Color playerSpawnPointColor = new Color(0f, 1f, 0f, 0.5f);

    [SerializeField]
    Color allySpawnPointColor = new Color(0f, 0f, 1f, 0.5f);

    [SerializeField]
    Color enemySpawnPointColor = new Color(1f, 0f, 0f, 0.5f);

    [SerializeField]
    Color navPointColor = new Color(0f, 1f, 1f, 0.5f);

    [SerializeField]
    Color navPoint1kColor = new Color(0f, 0.5f, 0.1f, 0.3f);

    [SerializeField]
    Color wayPointColor = new Color(1.0f, 0.92f, 0.016f, 0.5f);

    [SerializeField]
    GameObject borderNorth;

    [SerializeField]
    GameObject borderSouth;

    [SerializeField]
    GameObject borderEast;

    [SerializeField]
    GameObject borderWest;

    [SerializeField]
    BoxCollider borderColliderNorth;

    [SerializeField]
    BoxCollider borderColliderSouth;

    [SerializeField]
    BoxCollider borderColliderEast;

    [SerializeField]
    BoxCollider borderColliderWest;

    Vector3 spawnPointScaleUnit = new Vector3(1.5f, 2.25f, 1.5f);

    Color playerSpawnColor = new Color(0.0f, 1.0f, 0.0f, 0.5f);

    void OnDrawGizmos()
    {
        if (showPlayerSpawnPoints)
        {
            //DrawColoredMeshGizmos(playerSpawnPoints, spawnPointMesh, playerSpawnPointColor, spawnPointScale);

            Gizmos.color = playerSpawnColor;

            Vector3 offset = new Vector3(0.0f, spawnPointScaleUnit.y / 2.0f, 0.0f);

            for (int i = 0; i < playerSpawnPoints.Length; i++)
            {
                GameObject point = playerSpawnPoints[i];

                if (point != null)
                {
                    Gizmos.DrawCube(point.transform.position + offset, spawnPointScaleUnit);
                }
            }
        }

        if (showMapBounds)
        {
            Gizmos.color = new Color(1f, 0f, 0f, 0.6f);

            if (terrain != null)
            {
                float height = terrain.terrainData.bounds.max.y * 2.0f + 10f;

                float borderWidth = terrain.terrainData.bounds.size.x / 300f;

                Gizmos.DrawCube(new Vector3(mapBounds, 0f, 0f), new Vector3(borderWidth, height, 2f * mapBounds + borderWidth));

                Gizmos.DrawCube(new Vector3(-mapBounds, 0f, 0f), new Vector3(borderWidth, height, 2f * mapBounds + borderWidth));

                Gizmos.DrawCube(new Vector3(0f, 0f, mapBounds), new Vector3(2f * mapBounds + borderWidth, height, borderWidth));

                Gizmos.DrawCube(new Vector3(0f, 0f, -mapBounds), new Vector3(2f * mapBounds + borderWidth, height, borderWidth));
            }
        }

        if (showLandingZone && landingZoneNavPoint != null)
        {
            Gizmos.color = playerSpawnColor;

            Gizmos.DrawSphere(landingZoneNavPoint.transform.position, 10.0f);
        }

        switch (showMissionSetup)
        {
            case MissionType.Battle:
                {
                    if (randomGenMissionSetupBattle != null)
                    {
                        randomGenMissionSetupBattle.DrawGizmos();
                    }

                    break;
                }
            case MissionType.SearchAndDestroy:
                {
                    if (randomGenMissionSetupSearchAndDestroy != null)
                    {
                        randomGenMissionSetupSearchAndDestroy.DrawGizmos();
                    }

                    break;
                }
            case MissionType.Assassination:
                {
                    if (randomGenMissionSetupAssassination != null)
                    {
                        randomGenMissionSetupAssassination.DrawGizmos();
                    }

                    break;
                }
            case MissionType.ConvoyDestroy:
                {
                    if (randomGenMissionSetupConvoyDestroy != null)
                    {
                        randomGenMissionSetupConvoyDestroy.DrawGizmos();
                    }

                    break;
                }
            case MissionType.ConvoyCapture:
                {
                    break;
                }
            case MissionType.ConvoyEscort:
                {
                    break;
                }
            case MissionType.BattleSupport:
                {
                    if (randomGenMissionSetupBattleSupport != null)
                    {
                        randomGenMissionSetupBattleSupport.DrawGizmos();
                    }

                    break;
                }
            case MissionType.Recon:
                {
                    if (randomGenMissionSetupRecon != null)
                    {
                        randomGenMissionSetupRecon.DrawGizmos();
                    }

                    break;
                }
            case MissionType.BaseDestroy:
                {
                    if (randomGenMissionSetupBaseDestroy != null)
                    {
                        randomGenMissionSetupBaseDestroy.DrawGizmos();
                    }

                    break;
                }
            case MissionType.BaseCapture:
                {
                    if (randomGenMissionSetupBaseCapture != null)
                    {
                        randomGenMissionSetupBaseCapture.DrawGizmos();
                    }

                    break;
                }
            case MissionType.BaseDefend:
                {
                    if (randomGenMissionSetupBaseDefend != null)
                    {
                        randomGenMissionSetupBaseDefend.DrawGizmos();
                    }

                    break;
                }
        }
    }

    void DrawColoredMeshGizmos(GameObject[] points, Mesh mesh, Color color, Vector3 scale)
    {
        if (mesh == null)
            return;

        Gizmos.color = color;

        for (int i = 0; i < points.Length; i++)
        {
            GameObject point = points[i];

            if (point != null)
            {
                Gizmos.DrawMesh(mesh, point.transform.position, point.transform.rotation, scale);
            }
        }
    }

    void DrawColoredMeshGizmo(GameObject point, Mesh mesh, Color color, Vector3 scale)
    {
        if (mesh == null || point == null)
            return;

        Gizmos.color = color;

        Gizmos.DrawMesh(mesh, point.transform.position, point.transform.rotation, scale);
    }

    void DrawNavPointGizmos(Transform[] navPoints)
    {
        for (int i = 0; i < navPoints.Length; i++)
        {
            Transform navPoint = navPoints[i];

            if (navPoint != null)
            {
                Gizmos.color = navPointColor;
                Gizmos.DrawSphere(navPoint.transform.position, 3f);

                Gizmos.color = navPoint1kColor;
                Gizmos.DrawSphere(navPoint.transform.position, 100f);
            }
        }

        Gizmos.color = navPointColor;

        if (navPoints.Length > 2)
        {
            if (navPoints[navPoints.Length - 1] != null && navPoints[0] != null)
            {
                Gizmos.DrawLine(navPoints[navPoints.Length - 1].position, navPoints[0].position);
            }

            for (int i = 0; i < navPoints.Length - 1; i++)
            {
                if (navPoints[i] != null && navPoints[i + 1] != null)
                {
                    Gizmos.DrawLine(navPoints[i].position, navPoints[i + 1].position);
                }
            }
        }
        else if (navPoints.Length > 1)
        {
            if (navPoints[0] != null && navPoints[1] != null)
            {
                Gizmos.DrawLine(navPoints[0].position, navPoints[1].position);
            }
        }
    }

    void DrawWayPointGizmos(Transform[] wayPoints, Color color)
    {
        Gizmos.color = color;

        for (int i = 0; i < wayPoints.Length; i++)
        {
            Transform wayPoint = wayPoints[i];

            if (wayPoint != null)
            {
                Gizmos.DrawSphere(wayPoint.transform.position, 1.5f);
            }
        }

        for (int i = 0; i < wayPoints.Length - 1; i++)
        {
            if (wayPoints[i] != null && wayPoints[i + 1] != null)
            {
                Gizmos.DrawLine(wayPoints[i].position, wayPoints[i + 1].position);
            }
        }
    }

    public void AutoSizeBorderObjects()
    {
        if (borderNorth == null)
        {
            borderNorth = CreateBorderCube(mapBorderMaterial, "BorderNorth");
        }
        else
        {
            borderNorth.GetComponent<Renderer>().sharedMaterial = mapBorderMaterial;
        }

        if (borderSouth == null)
        {
            borderSouth = CreateBorderCube(mapBorderMaterial, "BorderSouth");
        }
        else
        {
            borderSouth.GetComponent<Renderer>().sharedMaterial = mapBorderMaterial;
        }

        if (borderEast == null)
        {
            borderEast = CreateBorderCube(mapBorderMaterial, "BorderEast");
        }
        else
        {
            borderEast.GetComponent<Renderer>().sharedMaterial = mapBorderMaterial;
        }

        if (borderWest == null)
        {
            borderWest = CreateBorderCube(mapBorderMaterial, "BorderWest");
        }
        else
        {
            borderWest.GetComponent<Renderer>().sharedMaterial = mapBorderMaterial;
        }

        if (borderColliderNorth == null)
        {
            borderColliderNorth = CreateBorderCollider("BorderColliderNorth");
        }

        if (borderColliderSouth == null)
        {
            borderColliderSouth = CreateBorderCollider("BorderColliderSouth");
        }

        if (borderColliderEast == null)
        {
            borderColliderEast = CreateBorderCollider("BorderColliderEast");
        }

        if (borderColliderWest == null)
        {
            borderColliderWest = CreateBorderCollider("BorderColliderWest");
        }

        borderNorth.SetActive(true);
        borderSouth.SetActive(true);
        borderEast.SetActive(true);
        borderWest.SetActive(true);

        float borderWidth = 4f;
        float height = 350f;

        if (terrain != null)
        {
            height = terrain.terrainData.bounds.max.y * 2.0f + 10f;

            borderWidth = terrain.terrainData.bounds.size.x / 300f;
        }

        borderNorth.transform.position = new Vector3(0f, 0f, mapBounds);
        borderNorth.transform.localScale = new Vector3(mapBounds * 2f + borderWidth, height, borderWidth);

        borderSouth.transform.position = new Vector3(0f, 0f, -mapBounds);
        borderSouth.transform.localScale = new Vector3(mapBounds * 2f + borderWidth, height, borderWidth);

        borderEast.transform.position = new Vector3(mapBounds, 0f, 0f);
        borderEast.transform.localScale = new Vector3(borderWidth, height, mapBounds * 2f + borderWidth);

        borderWest.transform.position = new Vector3(-mapBounds, 0f, 0f);
        borderWest.transform.localScale = new Vector3(borderWidth, height, mapBounds * 2f + borderWidth);

        borderColliderNorth.center = new Vector3(0f, 0f, mapColliderBounds);
        borderColliderNorth.size = new Vector3(mapColliderBounds * 2.0f, 1000f, 5f);
        borderColliderNorth.gameObject.layer = 29;

        borderColliderSouth.center = new Vector3(0f, 0f, -mapColliderBounds);
        borderColliderSouth.size = new Vector3(mapColliderBounds * 2.0f, 1000f, 5f);
        borderColliderSouth.gameObject.layer = 29;

        borderColliderEast.center = new Vector3(mapColliderBounds, 0f, 0f);
        borderColliderEast.size = new Vector3(5f, 1000f, mapColliderBounds * 2.0f);
        borderColliderEast.gameObject.layer = 29;

        borderColliderWest.center = new Vector3(-mapColliderBounds, 0f, 0f);
        borderColliderWest.size = new Vector3(5f, 1000f, mapColliderBounds * 2.0f);
        borderColliderWest.gameObject.layer = 29;
		
		GameObject borderObjectsContainer = new GameObject();
        borderObjectsContainer.name = "BorderObjects";
        borderObjectsContainer.transform.parent = transform;
		
		borderNorth.transform.parent = borderObjectsContainer.transform;
        borderSouth.transform.parent = borderObjectsContainer.transform;
        borderEast.transform.parent = borderObjectsContainer.transform;
        borderWest.transform.parent = borderObjectsContainer.transform;
        borderColliderNorth.transform.parent = borderObjectsContainer.transform;
        borderColliderSouth.transform.parent = borderObjectsContainer.transform;
        borderColliderEast.transform.parent = borderObjectsContainer.transform;
        borderColliderWest.transform.parent = borderObjectsContainer.transform;
    }

    GameObject CreateBorderCube(Material material, string name)
    {
        GameObject borderObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
        borderObject.transform.parent = transform;
        borderObject.name = name;

        Collider collider = borderObject.GetComponent<Collider>();

        if (collider != null)
        {
            DestroyImmediate(collider);
        }

        Renderer renderer = borderObject.GetComponent<Renderer>();

        if (renderer != null)
        {
            renderer.sharedMaterial = material;
        }

        return borderObject;
    }

    BoxCollider CreateBorderCollider(string name)
    {
        GameObject borderColliderObject = new GameObject();
        borderColliderObject.transform.parent = transform;
        borderColliderObject.name = name;

        BoxCollider borderCollider = borderColliderObject.AddComponent<BoxCollider>();

        return borderCollider;
    }

    public KeyValuePair<string, Color> GetValidPlayerSpawnPoints()
    {
        if (playerSpawnPoints.Length != 12)
        {
            return new KeyValuePair<string, Color>("Requires 12 Spawn Points", Color.red);
        }
        else
        {
            for (int i = 0; i < playerSpawnPoints.Length; i++)
            {
                if (playerSpawnPoints[i] == null)
                {
                    return new KeyValuePair<string, Color>("Invalid Spawn Point", Color.red);
                }
            }
        }

        return new KeyValuePair<string, Color>("Valid", Color.green);
    }

    public KeyValuePair<string, Color> GetValidLandingZoneNavPoint()
    {
        if (landingZoneNavPoint == null)
        {
            return new KeyValuePair<string, Color>("No Landing Zone Nav Point", Color.red);
        }

        return new KeyValuePair<string, Color>("Valid", Color.green);
    }

    public KeyValuePair<string, Color> GetValidMissionBattle()
    {
        if (randomGenMissionSetupBattle != null)
        {
            return randomGenMissionSetupBattle.GetMissionValidity();
        }

        return new KeyValuePair<string, Color>("Setup not assigned", Color.red);
    }

    public KeyValuePair<string, Color> GetValidMissionSearchAndDestroy()
    {
        if (randomGenMissionSetupSearchAndDestroy != null)
        {
            return randomGenMissionSetupSearchAndDestroy.GetMissionValidity();
        }

        return new KeyValuePair<string, Color>("Setup not assigned", Color.red);
    }

    public KeyValuePair<string, Color> GetValidMissionAssassination()
    {
        if (randomGenMissionSetupAssassination != null)
        {
            return randomGenMissionSetupAssassination.GetMissionValidity();
        }

        return new KeyValuePair<string, Color>("Setup not assigned", Color.red);
    }

    public KeyValuePair<string, Color> GetValidMissionConvoyDestroy()
    {
        if (randomGenMissionSetupConvoyDestroy != null)
        {
            return randomGenMissionSetupConvoyDestroy.GetMissionValidity();
        }

        return new KeyValuePair<string, Color>("Setup not assigned", Color.red);
    }

    public KeyValuePair<string, Color> GetValidMissionBattleSupport()
    {
        if (randomGenMissionSetupBattleSupport != null)
        {
            return randomGenMissionSetupBattleSupport.GetMissionValidity();
        }

        return new KeyValuePair<string, Color>("Setup not assigned", Color.red);
    }

    public KeyValuePair<string, Color> GetValidMissionRecon()
    {
        if (randomGenMissionSetupRecon != null)
        {
            return randomGenMissionSetupRecon.GetMissionValidity();
        }

        return new KeyValuePair<string, Color>("Setup not assigned", Color.red);
    }

    public KeyValuePair<string, Color> GetValidMissionBaseDestroy()
    {
        if (randomGenMissionSetupBaseDestroy != null)
        {
            return randomGenMissionSetupBaseDestroy.GetMissionValidity();
        }

        return new KeyValuePair<string, Color>("Setup not assigned", Color.red);
    }

    public KeyValuePair<string, Color> GetValidMissionBaseCapture()
    {
        if (randomGenMissionSetupBaseCapture != null)
        {
            return randomGenMissionSetupBaseCapture.GetMissionValidity();
        }

        return new KeyValuePair<string, Color>("Setup not assigned", Color.red);
    }

    public KeyValuePair<string, Color> GetValidMissionBaseDefend()
    {
        if (randomGenMissionSetupBaseDefend != null)
        {
            return randomGenMissionSetupBaseDefend.GetMissionValidity();
        }

        return new KeyValuePair<string, Color>("Setup not assigned", Color.red);
    }
	
	public void CreatePlayerSpawnPoints()
    {
        GameObject playerSpawnPointsContainer = new GameObject();
        playerSpawnPointsContainer.name = "PlayerSpawnPoints";
        playerSpawnPointsContainer.transform.parent = transform;

        playerSpawnPoints = new GameObject[12];

        for (int i = 0; i < 12; i++)
        {
            GameObject playerSpawnPoint = new GameObject();
            playerSpawnPoint.name = "PlayerSpawnPoint" + (i + 1).ToString();
            playerSpawnPoint.transform.parent = playerSpawnPointsContainer.transform;
            playerSpawnPoints[i] = playerSpawnPoint;
        }
    }

    public void CreateLandingZoneNavPoint()
    {
        if (landingZoneNavPoint == null)
        {
            landingZoneNavPoint = new GameObject();
        }
        
        landingZoneNavPoint.name = "LandingZoneNavPoint";
        landingZoneNavPoint.transform.parent = transform;
    }

    public void CreateMissionSetupBattle()
    {
        if (randomGenMissionSetupBattle == null)
        {
            randomGenMissionSetupBattle = new GameObject().AddComponent<RandomGenMissionSetupBattle>();
        }

        randomGenMissionSetupBattle.transform.parent = transform;
        randomGenMissionSetupBattle.gameObject.name = "MissionSetupBattle";
        randomGenMissionSetupBattle.CreateSetupObjects(landingZoneNavPoint.transform);
    }

    public void CreateMissionSetupBattleSupport()
    {
        if (randomGenMissionSetupBattleSupport == null)
        {
            randomGenMissionSetupBattleSupport = new GameObject().AddComponent<RandomGenMissionSetupBattleSupport>();
        }

        randomGenMissionSetupBattleSupport.transform.parent = transform;
        randomGenMissionSetupBattleSupport.gameObject.name = "MissionSetupBattleSupport";
        randomGenMissionSetupBattleSupport.CreateSetupObjects(landingZoneNavPoint.transform);
    }

    public void CreateMissionSetupSearchAndDestroy()
    {
        if (randomGenMissionSetupSearchAndDestroy == null)
        {
            randomGenMissionSetupSearchAndDestroy = new GameObject().AddComponent<RandomGenMissionSetupSearchAndDestroy>();
        }

        randomGenMissionSetupSearchAndDestroy.transform.parent = transform;
        randomGenMissionSetupSearchAndDestroy.gameObject.name = "MissionSetupSearchAndDestroy";
        randomGenMissionSetupSearchAndDestroy.CreateSetupObjects(landingZoneNavPoint.transform);
    }

    public void CreateMissionSetupAssassination()
    {
        if (randomGenMissionSetupAssassination == null)
        {
            randomGenMissionSetupAssassination = new GameObject().AddComponent<RandomGenMissionSetupAssassination>();
        }

        randomGenMissionSetupAssassination.transform.parent = transform;
        randomGenMissionSetupAssassination.gameObject.name = "MissionSetupAssassination";
        randomGenMissionSetupAssassination.CreateSetupObjects(landingZoneNavPoint.transform);
    }

    public void CreateMissionSetupRecon()
    {
        if (randomGenMissionSetupRecon == null)
        {
            randomGenMissionSetupRecon = new GameObject().AddComponent<RandomGenMissionSetupRecon>();
        }

        randomGenMissionSetupRecon.transform.parent = transform;
        randomGenMissionSetupRecon.gameObject.name = "MissionSetupRecon";
        randomGenMissionSetupRecon.CreateSetupObjects(landingZoneNavPoint.transform);
    }

    public void CreateMissionSetupBaseDestroy()
    {
        if (randomGenMissionSetupBaseDestroy == null)
        {
            randomGenMissionSetupBaseDestroy = new GameObject().AddComponent<RandomGenMissionSetupBaseDestroy>();
        }

        randomGenMissionSetupBaseDestroy.transform.parent = transform;
        randomGenMissionSetupBaseDestroy.gameObject.name = "MissionSetupBaseDestroy";
        randomGenMissionSetupBaseDestroy.CreateSetupObjects(landingZoneNavPoint.transform);
    }

    public void CreateMissionSetupBaseCapture()
    {
        if (randomGenMissionSetupBaseCapture == null)
        {
            randomGenMissionSetupBaseCapture = new GameObject().AddComponent<RandomGenMissionSetupBaseCapture>();
        }

        randomGenMissionSetupBaseCapture.transform.parent = transform;
        randomGenMissionSetupBaseCapture.gameObject.name = "MissionSetupBaseCapture";
        randomGenMissionSetupBaseCapture.CreateSetupObjects(landingZoneNavPoint.transform);
    }

    public void CreateMissionSetupBaseDefend()
    {
        if (randomGenMissionSetupBaseDefend == null)
        {
            randomGenMissionSetupBaseDefend = new GameObject().AddComponent<RandomGenMissionSetupBaseDefend>();
        }

        randomGenMissionSetupBaseDefend.transform.parent = transform;
        randomGenMissionSetupBaseDefend.gameObject.name = "MissionSetupBaseDefend";
        randomGenMissionSetupBaseDefend.CreateSetupObjects(landingZoneNavPoint.transform);
    }

    public void CreateMissionSetupConvoyDestroy()
    {
        if (randomGenMissionSetupConvoyDestroy == null)
        {
            randomGenMissionSetupConvoyDestroy = new GameObject().AddComponent<RandomGenMissionSetupConvoyDestroy>();
        }

        randomGenMissionSetupConvoyDestroy.transform.parent = transform;
        randomGenMissionSetupConvoyDestroy.gameObject.name = "MissionSetupConvoyDestroy";
        randomGenMissionSetupConvoyDestroy.CreateSetupObjects(landingZoneNavPoint.transform);
    }
    #endregion
#endif

    public abstract class AbstractMissionSetup
    {
        public Transform[] navigationPoints = new Transform[0];
    }

    [System.Serializable]
    class CustomMissionSetup : AbstractMissionSetup
    {
        public CustomUnitsSetup customUnitsSetup;

        public CustomObjectivesSetup customObjectivesSetup;
    }

    /*
    [System.Serializable]
    class BattleMissionSetup : AbstractMissionSetup
    {
        public GameObject[] enemySpawnPoints = new GameObject[0];
    }

    [System.Serializable]
    class SearchAndDestroyMissionSetup : AbstractMissionSetup
    {
        public SpawnPointGroup[] navPointSpawnPoints = new SpawnPointGroup[0];
    }

    [System.Serializable]
    class AssassinateMissionSetup : AbstractMissionSetup
    {
        public GameObject[] enemySpawnPoints = new GameObject[0];

        public GameObject[] enemyAssassinationTargetSpawnPoints = new GameObject[0];
    }

    [System.Serializable]
    class BattleSupportMissionSetup : AbstractMissionSetup
    {
        public GameObject[] enemySpawnPoints = new GameObject[0];

        public GameObject[] allySpawnPoints = new GameObject[0];
    }

    [System.Serializable]
    class ReconMissionSetup : AbstractMissionSetup
    {
        public GameObject[] enemySpawnPoints = new GameObject[0];
    }

    [System.Serializable]
    class BaseDestroyMissionSetup : AbstractMissionSetup
    {
        public GameObject[] enemySpawnPoints = new GameObject[0];

        public GameObject baseSpawnPoint;

        public GameObject[] turretSpawnPoints = new GameObject[0];
    }

    [System.Serializable]
    class ConvoyDestroyMissionSetup : AbstractMissionSetup
    {
        public GameObject[] enemySpawnPoints = new GameObject[0];

        public GameObject[] convoySpawnPoints = new GameObject[0];

        public Transform[] convoyWayPoints = new Transform[0];
    }

    [System.Serializable]
    class ConvoyEscortMissionSetup : AbstractMissionSetup
    {
        public GameObject[] enemySpawnPoints = new GameObject[0];
    }

    [System.Serializable]
    class ConvoyCaptureMissionSetup : AbstractMissionSetup
    {
        public GameObject[] enemySpawnPoints = new GameObject[0];
    }

    [System.Serializable]
    class SpawnPointGroup
    {
        public GameObject[] enemySpawnPoints = new GameObject[0];
    }*/
}