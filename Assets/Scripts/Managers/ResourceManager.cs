using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Xml.Serialization;
using System.IO;
using System.Linq;
using UnityEngine.Networking;
using UnityEngine.Audio;
using System.Threading.Tasks;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif

public class ResourceManager : MonoBehaviour
{
    #region Variables
    public static ResourceManager Instance { get; private set; }

    public bool customGraphicsQuality;

    Dictionary<string, string> localizationDict = new Dictionary<string, string>();

    Dictionary<string, AudioClip> audioClipsDict = new Dictionary<string, AudioClip>();

    Dictionary<string, MechChassisDefinition> mechChassisDefinitionsDict = new Dictionary<string, MechChassisDefinition>();

    Dictionary<string, WeaponDefinition> weaponDefinitionsDict = new Dictionary<string, WeaponDefinition>();

    Dictionary<string, ProjectileDefinition> projectileDefinitionsDict = new Dictionary<string, ProjectileDefinition>();

    Dictionary<string, ComponentDefinition> componentDefinitionsDict = new Dictionary<string, ComponentDefinition>();

    Dictionary<string, ComponentSet> componentSetsDict = new Dictionary<string, ComponentSet>();

    Dictionary<string, AmmoDefinition> ammoDefinitionsDict = new Dictionary<string, AmmoDefinition>();

    Dictionary<string, EquipmentDefinition> equipmentDefinitionDict = new Dictionary<string, EquipmentDefinition>();

    Dictionary<string, Dictionary<string, MechDesign>> mechDesignsDict = new Dictionary<string, Dictionary<string, MechDesign>>();

    Dictionary<string, Texture2D> mechSkinTexturesDict = new Dictionary<string, Texture2D>();

    Dictionary<string, List<Texture2D>> mechUniqueSkinTextureListsDict = new Dictionary<string, List<Texture2D>>();

    [SerializeField]
    List<Texture2D> mechUniversalSkinTextureList = new List<Texture2D>();

    Dictionary<string, MissionDefinition> missionDefinitionsDict = new Dictionary<string, MissionDefinition>();

    Dictionary<string, ContractDefinition> contractDefinitionsDict = new Dictionary<string, ContractDefinition>();

    Dictionary<string, ContractRandomDefinition> contractRandomDefinitionDict = new Dictionary<string, ContractRandomDefinition>();

    Dictionary<string, ContractUniqueDefinition> contractUniqueDefinitionDict = new Dictionary<string, ContractUniqueDefinition>();

    Dictionary<string, Texture2D> mapTextureDict = new Dictionary<string, Texture2D>();

    Dictionary<string, CareerSave> careerSaveDict = new Dictionary<string, CareerSave>();

    Dictionary<string, FactionDefinition> factionsDict = new Dictionary<string, FactionDefinition>();

    Dictionary<string, EncyclopediaDefinition> encyclopediaDict = new Dictionary<string, EncyclopediaDefinition>();

    Dictionary<string, GroundVehicleDefinition> groundVehiclesDict = new Dictionary<string, GroundVehicleDefinition>();

	Dictionary<string, BuildingDefinition> buildingDefinitionsDict = new Dictionary<string, BuildingDefinition>();

    Dictionary<string, PilotVoiceProfileDefinition> pilotVoiceProfilesDict = new Dictionary<string, PilotVoiceProfileDefinition>();

    List<PilotVoiceProfileDefinition> pilotVoiceProfilesMaleDict = new List<PilotVoiceProfileDefinition>();

    List<PilotVoiceProfileDefinition> pilotVoiceProfilesFemaleDict = new List<PilotVoiceProfileDefinition>();

    Dictionary<string, PilotDefinition> pilotDefinitionsDict = new Dictionary<string, PilotDefinition>();

    Dictionary<string, WeatherDefinition> weatherDefinitionsDict = new Dictionary<string, WeatherDefinition>();

    Dictionary<string, Texture2D> pilotTextureDict = new Dictionary<string, Texture2D>();

    Dictionary<string, Texture2D> logoTextureDict = new Dictionary<string, Texture2D>();

    Dictionary<string, PlanetDefinition> planetDefinitionsDict = new Dictionary<string, PlanetDefinition>();

    Dictionary<string, MapDefinition> mapDefinitionsDict = new Dictionary<string, MapDefinition>();

    Dictionary<string, Dictionary<string, MechPaintScheme>> mechPaintSchemeDict = new Dictionary<string, Dictionary<string, MechPaintScheme>>();

    Dictionary<string, TurretDefinition> turretDefinitionsDict = new Dictionary<string, TurretDefinition>();

    Dictionary<string, GameObject> mechPrefabsDict = new Dictionary<string, GameObject>();

    Dictionary<string, GameObject> turretPrefabsDict = new Dictionary<string, GameObject>();

    Dictionary<string, GameObject> buildingPrefabsDict = new Dictionary<string, GameObject>();

    Dictionary<string, GameObject> beamPrefabsDict = new Dictionary<string, GameObject>();

    Dictionary<string, GameObject> weaponModelPrefabsDict = new Dictionary<string, GameObject>();

    Dictionary<string, GameObject> projectilePrefabsDict = new Dictionary<string, GameObject>();

    Dictionary<string, GameObject> trailPrefabsDict = new Dictionary<string, GameObject>();

    Dictionary<string, AudioGroup> audioGroupsDict = new Dictionary<string, AudioGroup>();

    //Dictionary<string, UnityEngine.AzureSky.AzureWeatherProfile> skyProfilesDict = new Dictionary<string, UnityEngine.AzureSky.AzureWeatherProfile>();

    List<string> randomCallsigns = new List<string>();

	[SerializeField]
    AudioClip[] audioClips;

    [SerializeField]
    GameObject[] mechPrefabs;

    [SerializeField]
    GameObject[] groundVehiclePrefabs;

    [SerializeField]
    GameObject[] turretPrefabs;

	[SerializeField]
	GameObject[] buildingPrefabs;

    [SerializeField]
    GameObject[] cockpitPrefabs;

    [SerializeField]
    GameObject[] weaponModelPrefabs;

    [SerializeField]
    GameObject[] accessoryModelPrefabs;

    [SerializeField]
    GameObject[] beamPrefabs;

    [SerializeField]
    GameObject[] projectilePrefabs;

    [SerializeField]
    GameObject[] trailPrefabs;

    [SerializeField]
    GameObject[] effectPrefabs;

    [SerializeField]
    GameObject[] planetPrefabs;

    [SerializeField]
    GameObject[] baseFoundations;

    [SerializeField]
    GameObject turretFoundationLight;

    [SerializeField]
    GameObject turretFoundationMedium;

    [SerializeField]
    GameObject turretFoundationHeavy;

    [SerializeField]
    GameObject turretFoundationAssault;

    [SerializeField]
    MechSkinGroup[] mechSkinGroups;

    [SerializeField]
    Texture2D[] loadingScreenTextures2D;

    [SerializeField]
    Texture2D[] mapTextures2D;

    [SerializeField]
    Texture2D[] pilotMaleTextures2D;

    [SerializeField]
    Texture2D[] pilotFemaleTextures2D;

    [SerializeField]
    Texture2D[] pilotGenericTextures2D;

    [SerializeField]
    Texture2D[] pilotUniqueTextures2D;

    [SerializeField]
    Texture2D[] logoTextures2D;

    //[SerializeField]
    //UnityEngine.AzureSky.AzureWeatherProfile[] skyProfiles;

    [SerializeField]
    GameConstants gameConstants;

    [SerializeField]
    MissionDifficultyConfig missionDifficultyConfig;

    [SerializeField]
    Credits credits;

    [SerializeField]
    AudioMixer audioMixerMaster;

    [SerializeField]
    AudioMixer audioMixerGameplay;

    [SerializeField]
    GameObject postProcessingManagerPrefab;

    [SerializeField]
    GameObject cameraControllerPrefab;

    [SerializeField]
    GameObject missionCanvas;

    [SerializeField]
    GameObject skyPrefab;
    #endregion

    #region Properties

    public GameConstants GameConstants { get => gameConstants; }

    public MissionDifficultyConfig MissionDifficultyConfig { get => missionDifficultyConfig; }

    public AudioClip[] AudioClips { get => audioClips; }

    public GameObject[] BeamPrefabs { get => beamPrefabsDict.Values.ToArray(); }

    public GameObject[] ProjectilePrefabs { get => projectilePrefabsDict.Values.ToArray(); }

    public GameObject[] TrailPrefabs { get => trailPrefabsDict.Values.ToArray(); }

    public GameObject[] EffectPrefabs { get => effectPrefabs; }

    public GameObject PostProcessingManagerPrefab { get => postProcessingManagerPrefab; }

    public GameObject CameraControllerPrefab { get => cameraControllerPrefab; }

    public GameObject MissionCanvas { get => missionCanvas; }

    public GameObject SkyPrefab { get => skyPrefab; }
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

        //This stays in every scene
        DontDestroyOnLoad(gameObject);

        Directory.CreateDirectory(Application.persistentDataPath + "/Configs");
        Directory.CreateDirectory(Application.persistentDataPath + "/Careers");

        LoadAll();
    }
	
    void Start()
    {
        LoadAudioConfig(Application.persistentDataPath + "/Configs/AudioConfig.json");

        LoadGameOptionsConfig(Application.persistentDataPath + "/Configs/GameOptionsConfig.json");

        LoadGraphicsConfig(Application.persistentDataPath + "/Configs/GraphicsConfig.json");

        LoadControlConfig(Application.persistentDataPath + "/Configs/ControlsConfig.json");

        audioMixerMaster.updateMode = AudioMixerUpdateMode.UnscaledTime;

        List<WeaponDefinition> weaponDefinitions = weaponDefinitionsDict.Values.ToList();

        for (int i = 0; i < weaponDefinitions.Count; i++)
        {
            weaponDefinitions[i].BuildFromBase();
        }

        List<ProjectileDefinition> projectileDefinitions = projectileDefinitionsDict.Values.ToList();

        for (int i = 0; i < projectileDefinitions.Count; i++)
        {
            projectileDefinitions[i].BuildFromBase();
        }

        foreach (KeyValuePair<string, ContractRandomDefinition> keyValuePair in contractRandomDefinitionDict)
        {
            contractDefinitionsDict.Add(keyValuePair.Key, keyValuePair.Value);
        }

        foreach (KeyValuePair<string, ContractUniqueDefinition> keyValuePair in contractUniqueDefinitionDict)
        {
            contractDefinitionsDict.Add(keyValuePair.Key, keyValuePair.Value);
        }
    }

    void LoadAll()
    {
        DateTime LoadStartTime = DateTime.Now;

        StoreObjectCollectionToDictionary(mechPrefabs, mechPrefabsDict);
        StoreObjectCollectionToDictionary(turretPrefabs, turretPrefabsDict);
        StoreObjectCollectionToDictionary(buildingPrefabs, buildingPrefabsDict);
        StoreObjectCollectionToDictionary(audioClips, audioClipsDict);
        StoreObjectCollectionToDictionary(mechUniversalSkinTextureList, mechSkinTexturesDict);
        StoreObjectCollectionToDictionary(mapTextures2D, mapTextureDict);
        StoreObjectCollectionToDictionary(pilotMaleTextures2D, pilotTextureDict);
        StoreObjectCollectionToDictionary(pilotFemaleTextures2D, pilotTextureDict);
        StoreObjectCollectionToDictionary(pilotGenericTextures2D, pilotTextureDict);
        StoreObjectCollectionToDictionary(pilotUniqueTextures2D, pilotTextureDict);
        StoreObjectCollectionToDictionary(logoTextures2D, logoTextureDict);


        StoreObjectCollectionToDictionary(beamPrefabs, beamPrefabsDict);
        StoreObjectCollectionToDictionary(weaponModelPrefabs, weaponModelPrefabsDict);
        StoreObjectCollectionToDictionary(projectilePrefabs, projectilePrefabsDict);
        StoreObjectCollectionToDictionary(trailPrefabs, trailPrefabsDict);

        //StoreObjectCollectionToDictionary(skyProfiles, skyProfilesDict);

        LoadFileToClass(Application.streamingAssetsPath + "/GameConstants.xml", ref gameConstants);

        LoadFileToClass(Application.streamingAssetsPath + "/MissionDifficultyConfig.xml", ref missionDifficultyConfig);

        LoadFileToClass(Application.streamingAssetsPath + "/Credits.xml", ref credits);

        LoadLocalization(Application.streamingAssetsPath + "/Localization");

        LoadAudioDirectory(Application.streamingAssetsPath + "/Audio");

        LoadComponentsDirectory(Application.streamingAssetsPath + "/Components");
        LoadMechsDirectory(Application.streamingAssetsPath + "/Mechs");
		LoadBuildingsDirectory(Application.streamingAssetsPath + "/Buildings");

        LoadVehiclesDirectory(Application.streamingAssetsPath + "/Vehicles");
        LoadTurretsDirectory(Application.streamingAssetsPath + "/Turrets");

        LoadWeaponsDirectory(Application.streamingAssetsPath + "/Weapons");
        LoadEquipmentDirectory(Application.streamingAssetsPath + "/Equipment");

        LoadMissionsDirectory(Application.streamingAssetsPath + "/Missions");
        LoadContractsDirectory(Application.streamingAssetsPath + "/Contracts");
        LoadFactionsDirectory(Application.streamingAssetsPath + "/Factions");

        LoadPlanetsDirectory(Application.streamingAssetsPath + "/Planets");
        LoadMapsDirectory(Application.streamingAssetsPath + "/Maps");

        LoadSceneAssetBundlesDirectory(Application.streamingAssetsPath + "/SceneAssetBundles");

        LoadEncyclopediaDirectory(Application.streamingAssetsPath + "/Encyclopedia");

        LoadPilotsDirectory(Application.streamingAssetsPath + "/Pilots");

        LoadXMLsFromDirectory(Application.streamingAssetsPath + "/AmmoDefinitions", ammoDefinitionsDict);

        LoadXMLsFromDirectory(Application.persistentDataPath + "/Careers", careerSaveDict);

        for (int groupIndex = 0; groupIndex < mechSkinGroups.Length; groupIndex++)
        {
            MechSkinGroup mechSkinGroup = mechSkinGroups[groupIndex];

            mechUniqueSkinTextureListsDict.Add(mechSkinGroup.chassis, mechSkinGroup.skins);

            for (int skinIndex = 0; skinIndex < mechSkinGroup.skins.Count; skinIndex++)
            {
                Texture2D mechSkin = mechSkinGroup.skins[skinIndex];

                mechSkinTexturesDict.Add(mechSkin.name, mechSkin);
            }
        }

        Debug.Log("Resource Load time: " + (DateTime.Now - LoadStartTime).TotalSeconds.ToString());

        LoadStartTime = DateTime.Now;

        LoadMods(Application.streamingAssetsPath + "/Mods/ModLoadOrder.xml");

        Debug.Log("Mods Load time: " + (DateTime.Now - LoadStartTime).TotalSeconds.ToString());
    }

    void LoadFileToClass <T>(string filePath, ref T target) where T : class
    {
        if (File.Exists(filePath))
        {
            T targetLoadedInstance = LoadDataFromXML<T>(filePath);

            if (targetLoadedInstance != null)
            {
                target = targetLoadedInstance;
            }
        }
    }

    void LoadAudioDirectory(string path)
    {
        LoadAudioFilesFromDirector(path + "/Clips");
        LoadXMLsFromDirectory(path + "/Groups", audioGroupsDict);
    }

    void LoadComponentsDirectory(string path)
    {
        LoadLocalization(path + "/Localization");
        LoadXMLsFromDirectory(path + "/Definitions", componentDefinitionsDict);
        LoadXMLsFromDirectory(path + "/Sets", componentSetsDict);
    }

    void LoadMechsDirectory(string path)
    {
        if (Directory.Exists(path))
        {
            LoadAssetBundles(path + "/AssetBundles", mechPrefabsDict);

            LoadLocalization(path + "/Localization");
            LoadXMLsFromDirectory(path + "/ChassisDefinitions", mechChassisDefinitionsDict);

            LoadMechDesigns(path + "/Designs");
            LoadMechPaintSchemes(path + "/PaintScheme");
        }
    }

	void LoadBuildingsDirectory(string path) 
	{
		if (Directory.Exists(path))
		{
            LoadAssetBundles(path + "/AssetBundles", buildingPrefabsDict);
            LoadLocalization(path + "/Localization");
            LoadXMLsFromDirectory(path + "/Definitions", buildingDefinitionsDict);
        }
	}

    void LoadVehiclesDirectory(string path)
    {
        if (Directory.Exists(path))
        {
            LoadLocalization(path + "/Localization");
            LoadXMLsFromDirectory(path + "/GroundVehicleDefinitions", groundVehiclesDict);
        }
    }

    void LoadTurretsDirectory(string path)
    {
        if (Directory.Exists(path))
        {
            LoadLocalization(path + "/Localization");
            LoadXMLsFromDirectory(path + "/Definitions", turretDefinitionsDict);

            LoadAssetBundles(path + "/AssetBundles", turretPrefabsDict);
        }
    }

    void LoadWeaponsDirectory(string path)
    {
        if (Directory.Exists(path))
        {
            LoadXMLsFromDirectory<WeaponDefinition, BeamWeaponDefinition>(path + "/BeamWeaponDefinitions", weaponDefinitionsDict);
            LoadXMLsFromDirectory<WeaponDefinition, ProjectileWeaponDefinition>(path + "/ProjectileWeaponDefinitions", weaponDefinitionsDict);
            LoadXMLsFromDirectory(path + "/ProjectileDefinitions", projectileDefinitionsDict);

            LoadAssetBundles(path + "/BeamAssetBundles", beamPrefabsDict);
            LoadAssetBundles(path + "/ModelAssetBundles", weaponModelPrefabsDict);
            LoadAssetBundles(path + "/ProjectileAssetBundles", projectilePrefabsDict);
            LoadAssetBundles(path + "/TrailAssetBundles", trailPrefabsDict);

            LoadLocalization(path + "/Localization");
        }
    }

    void LoadEquipmentDirectory(string path)
    {
        if (Directory.Exists(path))
        {
            LoadXMLsFromDirectory(path + "/Definitions", equipmentDefinitionDict);
            LoadXMLsFromDirectory<EquipmentDefinition, MissileDefenseDefinition>(path + "/MissileDefenseDefinitions", equipmentDefinitionDict);
            LoadLocalization(path + "/Localization");
        }
    }

    void LoadMissionsDirectory(string path)
    {
        if (Directory.Exists(path))
        {
            LoadXMLsFromDirectory(path + "/Definitions", missionDefinitionsDict);
            LoadLocalization(path + "/Localization");
        }
    }

    void LoadContractsDirectory(string path)
    {
        if (Directory.Exists(path))
        {
            LoadXMLsFromDirectory(path + "/RandomDefinitions", contractRandomDefinitionDict);
            LoadXMLsFromDirectory(path + "/UniqueDefinitions", contractUniqueDefinitionDict);
            LoadLocalization(path + "/Localization");
        }
    }

    void LoadFactionsDirectory(string path)
    {
        if (Directory.Exists(path))
        {
            LoadXMLsFromDirectory(path + "/Definitions", factionsDict);
            LoadLocalization(path + "/Localization");
        }
    }

    void LoadEncyclopediaDirectory(string path)
    {
        if (Directory.Exists(path))
        {
            LoadXMLsFromDirectory(path + "/Definitions", encyclopediaDict);
            LoadLocalization(path + "/Localization");
        }
    }

    void LoadPilotsDirectory(string path)
    {
        if (Directory.Exists(path))
        {
            LoadXMLsFromDirectory(path + "/Definitions", pilotDefinitionsDict);
            LoadVoiceProfilesDirectory(path + "/VoiceProfiles");
            LoadRandomCallsigns(path + "/RandomCallsigns.txt");
            LoadLocalization(path + "/Localization");
        }
    }

    void LoadVoiceProfilesDirectory(string path)
    {
        if (Directory.Exists(path))
        {
            LoadXMLsFromDirectory(path, pilotVoiceProfilesDict);
            LoadXMLsFromDirectory(path + "/Male", pilotVoiceProfilesMaleDict);
            LoadXMLsFromDirectory(path + "/Female", pilotVoiceProfilesFemaleDict);
        }
    }

    void LoadPlanetsDirectory(string path)
    {
        if (Directory.Exists(path))
        {
            LoadXMLsFromDirectory(path + "/Definitions", planetDefinitionsDict);
            LoadLocalization(path + "/Localization");
        }
    }

    void LoadMapsDirectory(string path)
    {
        if (Directory.Exists(path))
        {
            LoadXMLsFromDirectory(path + "/Definitions", mapDefinitionsDict);
            LoadXMLsFromDirectory(path + "/WeatherDefinitions", weatherDefinitionsDict);

            LoadLocalization(path + "/Localization");
        }
    }

    void LoadSceneAssetBundlesDirectory(string path)
    {
        if (Directory.Exists(path))
        {
            FileInfo[] files = GetFilesByType(path, SearchOption.AllDirectories, "*.assetbundle");

            for (int fileIndex = 0; fileIndex < files.Length; fileIndex++)
            {
                AssetBundle assetBundle = AssetBundle.LoadFromFile(path + "/" + files[fileIndex].Name);
                string[] scenePaths = assetBundle.GetAllScenePaths();
            }
        }
    }

    void LoadMods(string path)
    {
        if (File.Exists(path))
        {
            ModLoadOrder modLoadOrder = LoadDataFromXML<ModLoadOrder>(path);

            if (modLoadOrder != null && modLoadOrder.LoadMods)
            {
                for (int i = 0; i < modLoadOrder.ModEntries.Length; i++)
                {
                    ModLoadOrder.ModEntry modEntry = modLoadOrder.ModEntries[i];

                    if (modEntry.IsActive)
                    {
                        LoadModDirectory(modEntry.Path);
                    }
                }
            }
        }
    }

    void LoadModDirectory(string path)
    {
        if (Directory.Exists(path))
        {
            if (File.Exists(path + "/GameConstants.xml"))
            {
                GameConstants constants = LoadDataFromXML<GameConstants>(path + "/GameConstants.xml");

                if (constants != null)
                {
                    gameConstants = constants;
                }
            }

            LoadLocalization(path + "/Localization");

            LoadAudioDirectory(path + "/Audio");

            LoadComponentsDirectory(path + "/Components");
            LoadMechsDirectory(path + "/Mechs");
            LoadVehiclesDirectory(path + "/Vehicles");
            LoadWeaponsDirectory(path + "/Weapons");
            LoadEquipmentDirectory(path + "/Weapons");
            LoadMissionsDirectory(path + "/Missions");
            LoadContractsDirectory(path + "/Contracts");
            LoadFactionsDirectory(path + "/Factions");
            LoadXMLsFromDirectory(path + "/AmmoDefinitions", ammoDefinitionsDict);

            LoadBuildingsDirectory(path + "/Buildings");
            LoadEncyclopediaDirectory(path + "/Encyclopedia");
            LoadTurretsDirectory(path + "/Turrets");
            LoadPlanetsDirectory(path + "/Planets");
            LoadMapsDirectory(path + "/Maps");
            LoadPilotsDirectory(path + "/Pilots");

            LoadSceneAssetBundlesDirectory(path + "/SceneAssetBundles");
        }
    }

    void LoadLocalization(string path)
    {
        if (Directory.Exists(path))
        {
            string fileName = "/" + Application.systemLanguage.ToString() + ".txt";

            if (!File.Exists(path + fileName))
            {
                fileName = "/English.txt";
            }

            if (!File.Exists(path + fileName))
            {
                return;
            }

            using (StreamReader Reader = new StreamReader(path + fileName))
            {
                string line;
                while ((line = Reader.ReadLine()) != null)
                {
                    if (line != "")
                    {
                        string[] parts = line.Split(new char[] { ' ' }, 2);
                        if (parts.Length > 1)
                        {
                            localizationDict[parts[0]] = parts[1];
                        }
                    }
                }
            }
        }
        else
        {
            print("Localization folder not found: " + path);
        }
    }

    public void LoadRandomCallsigns(string path)
    {
        if (File.Exists(path))
        {
            using (StreamReader Reader = new StreamReader(path))
            {
                string line;
                while ((line = Reader.ReadLine()) != null)
                {
                    if (line != "")
                    {
                        randomCallsigns.Add(line);
                    }
                }
            }
        }
    }

    void LoadMechDesigns(string path)
    {
        if (Directory.Exists(path))
        {
            FileInfo[] files = GetFilesByType(path, SearchOption.AllDirectories, "*.xml");

            foreach (FileInfo file in files)
            {
                string name = Path.GetFileNameWithoutExtension(file.Name);
                MechDesign mechDesign = null;

                try
                {
                    mechDesign = LoadDataFromXML<MechDesign>(file.FullName);
                }
                catch (Exception ex)
                {
                    print(ex.ToString());
                    Debug.LogError("Failed to load: " + file.Name);
                }

                if (mechDesign != null)
                {
                    StoreMechDesign(mechDesign);
                }
            }
        }
    }

    void LoadMechPaintSchemes(string path)
    {
        if (Directory.Exists(path))
        {
            FileInfo[] files = GetFilesByType(path, SearchOption.AllDirectories, "*.xml");

            foreach (FileInfo file in files)
            {
                string name = Path.GetFileNameWithoutExtension(file.Name);
                string chassis = file.Directory.Name;
                MechPaintScheme mechPaintScheme = null;

                try
                {
                    mechPaintScheme = LoadDataFromXML<MechPaintScheme>(file.FullName);
                }
                catch (Exception ex)
                {
                    print(ex.ToString());
                    Debug.LogError("Error: Failed to load: " + file.FullName);
                }

                if (mechPaintScheme != null)
                {
                    StoreMechPaintScheme(chassis, name, mechPaintScheme);
                }
            }
        }
    }

    void LoadXMLsFromDirectory<T>(string path, List<T> targetList)
    {
        if (Directory.Exists(path))
        {
            FileInfo[] files = GetFilesByType(path, SearchOption.AllDirectories, "*.xml");

            for (int i = 0; i < files.Length; i++)
            {
                FileInfo file = files[i];

                string name = Path.GetFileNameWithoutExtension(file.Name);

                try
                {
                    T loadedFile = LoadDataFromXML<T>(file.FullName);

                    if (loadedFile is Definition)
                        (loadedFile as Definition).Key = name;

                    targetList.Add(loadedFile);
                }
                catch
                {
                    Debug.LogError("Error: Failed to load: " + file.FullName);
                }
            }
        }
    }

    void LoadXMLsFromDirectory<T>(string path, Dictionary<string, T> targetDictionary)
    {
        if (Directory.Exists(path))
        {
            FileInfo[] files = GetFilesByType(path, SearchOption.AllDirectories, "*.xml");

            for (int i = 0; i < files.Length; i++)
            {
                FileInfo file = files[i];

                string name = Path.GetFileNameWithoutExtension(file.Name);

                try
                {
                    T loadedFile = LoadDataFromXML<T>(file.FullName);

                    if (loadedFile is Definition)
                        (loadedFile as Definition).Key = name;

                    targetDictionary[name] = loadedFile;
                }
                catch
                {
                    Debug.LogError("Error: Failed to load: " + file.FullName);
                }
            }
        }
    }

    void LoadXMLsFromDirectory<T, U>(string path, Dictionary<string, T> targetDictionary) where U : T
    {
        if (Directory.Exists(path))
        {
            FileInfo[] files = GetFilesByType(path, SearchOption.AllDirectories, "*.xml");

            for (int i = 0; i < files.Length; i++)
            {
                FileInfo file = files[i];

                string name = Path.GetFileNameWithoutExtension(file.Name);

                try
                {
                    U loadedData = LoadDataFromXML<U>(file.FullName);

                    if (loadedData is Definition)
                        (loadedData as Definition).Key = name;

                    targetDictionary[name] = loadedData;
                }
                catch
                {
                    Debug.LogError("Error: Failed to load: " + file.FullName);
                }
            }
        }
    }

    async void LoadAudioFilesFromDirector(string path)
    {
        if (Directory.Exists(path))
        {
            FileInfo[] files = GetFilesByType(path, SearchOption.AllDirectories, "*.wav", "*.ogg");

            for (int i = 0; i < files.Length; i++)
            {
                FileInfo file = files[i];

                AudioClip audioClip = await LoadAudioClip(file.FullName);

                if (audioClip != null)
                {
                    string name = Path.GetFileNameWithoutExtension(file.Name);

                    audioClipsDict[name] = audioClip;
                }
            }
        }
    }

    public T CreateMech<T>(MechData mechData) where T : MechController
    {
        GameObject mechPrefab = mechData.MechPrefab;

        if ((object)mechPrefab != null)
        {
            GameObject mechInstance = Instantiate(mechPrefab);

            T mechController = mechInstance.AddComponent<T>();
            mechController.SetMechData(mechData);

            return mechController;
        }

        Debug.LogError("Error: Missing prefab " + mechData.MechChassis.MechPrefab);

        return null;
    }

    public GroundVehicleController CreateGroundVehicle(GroundVehicleData groundVehicleData)
    {
        GameObject prefab = groundVehicleData.groundVehicleDefinition.GetPrefab();

        if ((object)prefab != null)
        {
            GameObject groundVehicleInstance = Instantiate(prefab);

            GroundVehicleController groundVehicleController = groundVehicleInstance.AddComponent<GroundVehicleController>();
            groundVehicleController.SetGroundVehicleData(groundVehicleData);

            return groundVehicleController;
        }

        Debug.LogError("Error: Missing prefab " + groundVehicleData.groundVehicleDefinition.Prefab);

        return null;
    }

    public TurretUnitController CreateTurret(TurretUnitData turretUnitData)
    {
        GameObject prefab = turretUnitData.turretDefinition.GetPrefab();

        if ((object)prefab != null)
        {
            GameObject turretInstance = Instantiate(prefab);

            TurretUnitController turretUnitController = turretInstance.AddComponent<TurretUnitController>();
            turretUnitController.SetTurretData(turretUnitData);

            return turretUnitController;
        }

        Debug.LogError("Error: Missing prefab " + turretUnitData.turretDefinition.Prefab);

        return null;
    }

    public BuildingController CreateBuilding(BuildingData buildingData)
    {
        GameObject prefab = buildingData.buildingDefinition.GetPrefab();

        if ((object)prefab != null)
        {
            GameObject buildingInstance = Instantiate(prefab);

            BuildingController buildingController = buildingInstance.AddComponent<BuildingController>();
            buildingController.SetData(buildingData);

            return buildingController;
        }

        Debug.LogError("Error: Missing prefab " + buildingData.buildingDefinition.Prefab);

        return null;
    }

    public string GetLocalization(string key)
    {
        if (localizationDict.TryGetValue(key, out string value))
        {
            return value;
        }

        return key;
    }

    public Credits GetCredits()
    {
        return credits;
    }

    public AudioClip GetAudioClip(string key)
    {
        return GetElementFromDictionary(key, audioClipsDict);
    }

    public Texture2D GetMechSkinTexture(string key)
    {
        if (mechSkinTexturesDict.TryGetValue(key, out Texture2D texture2D))
        {
            return texture2D;
        }

        return null;
    }

    public List<Texture2D> GetUniqueMechSkinList(string chassis)
    {
        if (mechUniqueSkinTextureListsDict.TryGetValue(chassis, out List<Texture2D> skinList))
        {
            return skinList;
        }

        return new List<Texture2D>();
    }

    public List<Texture2D> GetUniversalMechSkinList()
    {
        return mechUniversalSkinTextureList;
    }

    public ComponentDefinition GetComponentDefinition(string key)
    {
        return GetElementFromDictionary(key, componentDefinitionsDict);
    }

    public ComponentDefinition[] GetComponentDefinitions()
    {
        return componentDefinitionsDict.Values.ToArray();
    }

    public List<ComponentDefinition> GetComponentDefinitions(string[] componentNames)
    {
        return componentDefinitionsDict.Values.ToList();
    }

    public ComponentSet GetComponentSet(string key)
    {
        return GetElementFromDictionary(key, componentSetsDict);
    }

    public ComponentSet[] GetComponentSets()
    {
        return componentSetsDict.Values.ToArray();
    }

    public MechChassisDefinition GetMechChassisDefinition(string key)
    {
        return GetElementFromDictionary(key, mechChassisDefinitionsDict);
    }

    public MechChassisDefinition[] GetMechChassisDefinitions()
    {
        return mechChassisDefinitionsDict.Values.ToArray();
    }

    public BuildingDefinition GetBuildingDefinition(string key)
	{
        return GetElementFromDictionary(key, buildingDefinitionsDict);
	}

    public WeaponDefinition GetWeaponDefinition(string key)
    {
        return GetElementFromDictionary(key, weaponDefinitionsDict);
    }

    public ProjectileDefinition GetProjectileDefinition(string key)
    {
        return GetElementFromDictionary(key, projectileDefinitionsDict);
    }

    public AmmoDefinition GetAmmoDefinition(string key)
    {
        return GetElementFromDictionary(key, ammoDefinitionsDict);
    }

    public EquipmentDefinition GetEquipmentDefinition(string key)
    {
        return GetElementFromDictionary(key, equipmentDefinitionDict);
    }

    public MissionDefinition GetMissionDefinition(string key)
    {
        return GetElementFromDictionary(key, missionDefinitionsDict);
    }

    public ContractDefinition GetContractDefinition(string key)
    {
        return GetElementFromDictionary(key, contractDefinitionsDict);
    }

    public List<ContractRandomDefinition> GetContractRandomDefinitions()
    {
        return contractRandomDefinitionDict.Values.ToList();
    }

    public FactionDefinition GetFactionDefinition(string key)
    {
        return GetElementFromDictionary(key, factionsDict);
    }

    public List<FactionDefinition> GetFactionDefinitions()
    {
        return factionsDict.Values.ToList();
    }

    public GroundVehicleDefinition GetGroundVehicleDefinition(string key)
    {
        return GetElementFromDictionary(key, groundVehiclesDict);
    }

    public TurretDefinition GetTurretDefinition(string key)
    {
        return GetElementFromDictionary(key, turretDefinitionsDict);
    }

    public WeatherDefinition GetWeatherDefinition(string key)
    {
        return GetElementFromDictionary(key, weatherDefinitionsDict);
    }

    public PilotVoiceProfileDefinition GetPilotVoiceProfile(string key)
    {
        return GetElementFromDictionary(key, pilotVoiceProfilesDict);
    }

    public List<PilotVoiceProfileDefinition> GetPilotVoiceProfileDefinitions()
    {
        return pilotVoiceProfilesDict.Values.ToList();
    }

    public PilotVoiceProfileDefinition GetRandomVoiceProfileMale()
    {
        return GetRandomElementFromCollection(pilotVoiceProfilesMaleDict);
    }

    public PilotVoiceProfileDefinition GetRandomVoiceProfileFemale()
    {
        return GetRandomElementFromCollection(pilotVoiceProfilesFemaleDict);
    }

    public PilotDefinition GetPilotDefinition(string key)
    {
        return GetElementFromDictionary(key, pilotDefinitionsDict);
    }

    public AudioGroup GetAudioGroup(string key)
    {
        return GetElementFromDictionary(key, audioGroupsDict);
    }

    public Sprite GetRandomPilotMaleSprite()
    {
        if (pilotMaleTextures2D.Length > 0)
        {
            Texture2D pilotIconTexture = pilotMaleTextures2D[UnityEngine.Random.Range(0, pilotMaleTextures2D.Length)];

            if (pilotIconTexture != null)
            {
                return StaticHelper.GetSpriteUI(pilotIconTexture);
            }
        }

        return null;
    }

    public Sprite GetRandomPilotFemaleSprite()
    {
        if (pilotFemaleTextures2D.Length > 0)
        {
            Texture2D pilotIconTexture = pilotFemaleTextures2D[UnityEngine.Random.Range(0, pilotFemaleTextures2D.Length)];

            if (pilotIconTexture != null)
            {
                return StaticHelper.GetSpriteUI(pilotIconTexture);
            }
        }

        return null;
    }

    public Sprite GetRandomPilotGenericSprite()
    {
        if (pilotGenericTextures2D.Length > 0)
        {
            Texture2D pilotIconTexture = pilotGenericTextures2D[UnityEngine.Random.Range(0, pilotGenericTextures2D.Length)];

            if (pilotIconTexture != null)
            {
                return StaticHelper.GetSpriteUI(pilotIconTexture);
            }
        }

        return null;
    }

    public string GetRandomCallsign()
    {
        if (randomCallsigns.Count > 0)
        {
            return GetLocalization(randomCallsigns[UnityEngine.Random.Range(0, randomCallsigns.Count)]);
        }

        return "";
    }

    public PlanetDefinition GetPlanetDefinition(string key)
    {
        return GetElementFromDictionary(key, planetDefinitionsDict);
    }

    public MapDefinition GetMapDefinition(string key)
    {
        return GetElementFromDictionary(key, mapDefinitionsDict);
    }

    /*public UnityEngine.AzureSky.AzureWeatherProfile GetSkyProfile(string key)
    {
        return GetElementFromDictionary(key, skyProfilesDict);
    }*/

    public PilotDefinition[] GetPilotDefinitions()
    {
        return pilotDefinitionsDict.Values.ToArray();
    }

    public Texture2D GetPilotTexture2D(string key)
    {
        return GetElementFromDictionary(key, pilotTextureDict);
    }

    public Texture2D[] GetPilotTextures()
    {
        return pilotTextureDict.Values.ToArray();
    }

    public Texture2D GetLogoTexture(string key)
    {
        return GetElementFromDictionary(key, logoTextureDict);
    }

    public List<Texture2D> GetLogoTextures()
    {
        return logoTextureDict.Values.ToList();
    }

    public Texture2D GetMapTexture2D(string key)
    {
        return GetElementFromDictionary(key, mapTextureDict);
    }

    public Texture2D GetRandomLoadingScreenTexture()
    {
        return GetRandomElementFromCollection(loadingScreenTextures2D);
    }

    public MissionDefinition[] GetMissionDefinitions()
    {
        return missionDefinitionsDict.Values.ToArray();
    }

    public EncyclopediaDefinition[] GetEncyclopediaDefinitions()
    {
        return encyclopediaDict.Values.ToArray();
    }

    public List<PlanetDefinition> GetPlanetDefinitions()
    {
        return planetDefinitionsDict.Values.ToList();
    }

    public List<MapDefinition> GetMapDefinitions(BiomeType[] biomeTypes)
    {
        List<MapDefinition> mapDefinitions = new List<MapDefinition>();

        List<MapDefinition> allMapDefinitions = mapDefinitionsDict.Values.ToList();

        for (int i = 0; i < allMapDefinitions.Count; i++)
        {
            MapDefinition mapDefinition = allMapDefinitions[i];

            if (biomeTypes.Contains(mapDefinition.Biome))
            {
                mapDefinitions.Add(mapDefinition);
            }
        }

        return mapDefinitions;
    }

    public MechDesign GetMechDesign(string chassisKey, string designKey)
    {
        if (mechDesignsDict.TryGetValue(chassisKey, out Dictionary<string, MechDesign> mechDesignDictionary))
        {
            if (mechDesignDictionary.TryGetValue(designKey, out MechDesign mechDesign))
            {
                return mechDesign;
            }
        }

        return null;
    }

    public MechDesign[] GetMechDesignList(string mechChassisName)
    {
        if (mechDesignsDict.TryGetValue(mechChassisName, out Dictionary<string, MechDesign> designDictionary))
        {
            return designDictionary.Values.ToArray();
        }

        return null;
    }

    public string GetMechDesignKey(string mechChassisName, MechDesign mechDesign)
    {
        if (mechDesignsDict.TryGetValue(mechChassisName, out Dictionary<string, MechDesign> designDictionary))
        {
            return GetKeyFromDictionary(mechDesign, designDictionary);
        }

        return null;
    }

    public MechPaintScheme GetMechPaintScheme(string mechChassis, string name)
    {
        if (mechPaintSchemeDict.TryGetValue(mechChassis, out Dictionary<string, MechPaintScheme> paintDict))
        {
            if (paintDict.TryGetValue(name, out MechPaintScheme mechPaintScheme))
            {
                return mechPaintScheme;
            }
        }

        return null;
    }

    public GameObject GetMechPrefab(string key)
    {
        return GetElementFromDictionary(key, mechPrefabsDict);
    }

    public GameObject GetGroundVehiclePrefab(string key)
    {
        return GetElementFromCollection(key, groundVehiclePrefabs);
    }

    public GameObject GetTurretPrefab(string key)
    {
        return GetElementFromDictionary(key, turretPrefabsDict);
    }

    public GameObject GetBuildingPrefab(string key)
    {
        return GetElementFromDictionary(key, buildingPrefabsDict);
    }

    public GameObject GetCockpitPrefab(string key)
    {
        return GetElementFromCollection(key, cockpitPrefabs);
    }

    public GameObject GetWeaponModelPrefab(string key)
    {
        return GetElementFromDictionary(key, weaponModelPrefabsDict);
    }

    public GameObject GetAccessoryModelPrefab(string key)
    {
        return GetElementFromCollection(key, accessoryModelPrefabs);
    }

    public GameObject GetBeamPrefab(string key)
    {
        return GetElementFromDictionary(key, beamPrefabsDict);
    }

    public GameObject GetProjectilePrefab(string key)
    {
        return GetElementFromDictionary(key, projectilePrefabsDict);
    }

    public GameObject GetTrailPrefab(string key)
    {
        return GetElementFromDictionary(key, trailPrefabsDict);
    }

    public GameObject GetEffectPrefab(string key)
    {
        return GetElementFromCollection(key, effectPrefabs);
    }

    public GameObject GetPlanetPrefab(string key)
    {
        return GetElementFromCollection(key, planetPrefabs);
    }

    public GameObject GetBaseFoundationPrefab(string key)
    {
        return GetElementFromCollection(key, baseFoundations);
    }

    public GameObject GetTurretFoundationPrefab(UnitClass unitClass)
    {
        switch (unitClass)
        {
            case UnitClass.TurretLight:
                {
                    return turretFoundationLight;
                }
            case UnitClass.TurretMedium:
                {
                    return turretFoundationMedium;
                }
            case UnitClass.TurretHeavy:
                {
                    return turretFoundationHeavy;
                }
            case UnitClass.TurretAssault:
                {
                    return turretFoundationAssault;
                }
        }

        return turretFoundationLight;
    }

    public void SaveMechDesign(string mechChassisName, MechDesign mechDesign)
    {
        SaveToXML(mechDesign, Application.streamingAssetsPath + "/Mechs/Designs/" + mechChassisName + "/" + mechDesign.DesignName + ".xml");

        StoreMechDesign(mechDesign);
    }

    public void StoreMechDesign(MechDesign mechDesign)
    {
        Dictionary<string, MechDesign> designDict;

        if (mechDesignsDict.TryGetValue(mechDesign.MechChassisDefinition, out designDict))
        {
            designDict[mechDesign.DesignName] = mechDesign;
        }
        else
        {
            designDict = new Dictionary<string, MechDesign>();
            designDict.Add(mechDesign.DesignName, mechDesign);
            mechDesignsDict.Add(mechDesign.MechChassisDefinition, designDict);
        }
    }

    public void StoreMechPaintScheme(string mechChassis, string paintSchemeName, MechPaintScheme mechPaintScheme)
    {
        if (mechPaintSchemeDict.TryGetValue(mechChassis, out Dictionary<string, MechPaintScheme> mechPaintDict))
        {
            mechPaintDict[paintSchemeName] = mechPaintScheme;
        }
        else
        {
            mechPaintDict = new Dictionary<string, MechPaintScheme>();
            mechPaintDict.Add(paintSchemeName, mechPaintScheme);
            mechPaintSchemeDict.Add(mechChassis, mechPaintDict);
        }
    }

    public CareerSave GetCareer(string key)
    {
        return GetElementFromDictionary(key, careerSaveDict);
    }

    public List<CareerSave> GetCareers()
    {
        return careerSaveDict.Values.ToList();
    }

    public void StoreCareer(CareerSave career)
    {
        careerSaveDict[career.UniqueIdentifier] = career;

        SaveCareer(career);
    }

    void SaveCareer(CareerSave career)
    {
        SaveToXML(career, Application.persistentDataPath + "/Careers/" + career.UniqueIdentifier + ".xml");
    }

    public void DeleteCareer(string uniqueIdentifier)
    {
        careerSaveDict.Remove(uniqueIdentifier);

        if (Directory.Exists(Application.persistentDataPath + "/Careers/"))
        {
            string path = Application.persistentDataPath + "/Careers/" + uniqueIdentifier + ".xml";

            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }
    }

    public void SaveGameOptionsConfig()
    {
        GameOptionsConfig gameOptionsConfig = new GameOptionsConfig()
        {
            CheatGodMode = Cheats.godMode,
            CheatNoHeat = Cheats.noHeat,
            CheatUnlimitedAmmo = Cheats.unlimitedAmmo,
        };

        SaveToJson(gameOptionsConfig, Application.persistentDataPath + "/Configs/GameOptionsConfig.json");
    }

    public void LoadGameOptionsConfig(string path)
    {
        if (File.Exists(path))
        {
            GameOptionsConfig gameOptionsConfig = LoadFileFromJson<GameOptionsConfig>(path);

            if (gameOptionsConfig != null)
            {
                Cheats.godMode = gameOptionsConfig.CheatGodMode;
                Cheats.noHeat = gameOptionsConfig.CheatNoHeat;
                Cheats.unlimitedAmmo = gameOptionsConfig.CheatUnlimitedAmmo;
            }
        }
    }

    public void SaveAudioConfig()
    {
        AudioConfig audioConfig = new AudioConfig()
        {
            MasterVolume = AudioListener.volume,

            MusicVolume = StaticHelper.GetVolumeFromMixer(audioMixerMaster, "MusicVolume"),
            WeaponsVolume = StaticHelper.GetVolumeFromMixer(audioMixerGameplay, "WeaponsVolume"),
            EffectsVolume = StaticHelper.GetVolumeFromMixer(audioMixerGameplay, "EffectsVolume"),
            AmbienceVolume = StaticHelper.GetVolumeFromMixer(audioMixerGameplay, "AmbienceVolume"),
            UIVolume = StaticHelper.GetVolumeFromMixer(audioMixerMaster, "UIVolume"),
            VoicesVolume = StaticHelper.GetVolumeFromMixer(audioMixerGameplay, "VoicesVolume"),
            MechsVolume = StaticHelper.GetVolumeFromMixer(audioMixerGameplay, "MechsVolume"),
        };

        SaveToJson(audioConfig, Application.persistentDataPath + "/Configs/AudioConfig.json");
    }

    void LoadAudioConfig(string path)
    {
        if (File.Exists(path))
        {
            AudioConfig audioConfig = LoadFileFromJson<AudioConfig>(path);

            if (audioConfig != null)
            {
                AudioListener.volume = audioConfig.MasterVolume;

                audioMixerMaster.SetFloat("MusicVolume", StaticHelper.VolumeToDecibel(audioConfig.MusicVolume));
                audioMixerGameplay.SetFloat("WeaponsVolume", StaticHelper.VolumeToDecibel(audioConfig.WeaponsVolume));
                audioMixerGameplay.SetFloat("EffectsVolume", StaticHelper.VolumeToDecibel(audioConfig.EffectsVolume));
                audioMixerGameplay.SetFloat("AmbienceVolume", StaticHelper.VolumeToDecibel(audioConfig.AmbienceVolume));
                audioMixerMaster.SetFloat("UIVolume", StaticHelper.VolumeToDecibel(audioConfig.UIVolume));
                audioMixerGameplay.SetFloat("VoicesVolume", StaticHelper.VolumeToDecibel(audioConfig.VoicesVolume));
                audioMixerGameplay.SetFloat("MechsVolume", StaticHelper.VolumeToDecibel(audioConfig.MechsVolume));
            }
            else
            {
                print("Failed to load Audio config: " + path);

                AudioListener.volume = 0.75f;

                audioMixerMaster.SetFloat("MusicVolume", StaticHelper.VolumeToDecibel(0.5f));
                audioMixerGameplay.SetFloat("WeaponsVolume", StaticHelper.VolumeToDecibel(1.0f));
                audioMixerGameplay.SetFloat("EffectsVolume", StaticHelper.VolumeToDecibel(1.0f));
                audioMixerGameplay.SetFloat("AmbienceVolume", StaticHelper.VolumeToDecibel(1.0f));
                audioMixerMaster.SetFloat("UIVolume", StaticHelper.VolumeToDecibel(1.0f));
                audioMixerGameplay.SetFloat("VoicesVolume", StaticHelper.VolumeToDecibel(1.0f));
                audioMixerGameplay.SetFloat("MechsVolume", StaticHelper.VolumeToDecibel(1.0f));
            }
        }
        else
        {
            AudioListener.volume = 0.75f;

            audioMixerMaster.SetFloat("MusicVolume", StaticHelper.VolumeToDecibel(0.5f));
            audioMixerGameplay.SetFloat("WeaponsVolume", StaticHelper.VolumeToDecibel(1.0f));
            audioMixerGameplay.SetFloat("EffectsVolume", StaticHelper.VolumeToDecibel(1.0f));
            audioMixerGameplay.SetFloat("AmbienceVolume", StaticHelper.VolumeToDecibel(1.0f));
            audioMixerMaster.SetFloat("UIVolume", StaticHelper.VolumeToDecibel(1.0f));
            audioMixerGameplay.SetFloat("VoicesVolume", StaticHelper.VolumeToDecibel(1.0f));
            audioMixerGameplay.SetFloat("MechsVolume", StaticHelper.VolumeToDecibel(1.0f));
        }
    }

    public void SaveGraphicsConfig()
    {
        GraphicsConfig graphicsConfig;

        int qualityLevel = QualitySettings.GetQualityLevel();

        if (customGraphicsQuality)
        {
            qualityLevel = 6;
        }

        graphicsConfig = new GraphicsConfig()
        {
            QualityLevel = qualityLevel,
            ResolutionX = Screen.currentResolution.width,
            ResolutionY = Screen.currentResolution.height,
            RefreshRate = Screen.currentResolution.refreshRate,
            FullScreen = Screen.fullScreen,
            TextureQuality = QualitySettings.masterTextureLimit,
            AnisotropicFiltering = QualitySettings.anisotropicFiltering,
            RealTimeReflectionProbes = QualitySettings.realtimeReflectionProbes,
            DpiFactor = QualitySettings.resolutionScalingFixedDPIFactor,
            LodDistance = QualitySettings.lodBias,
            MaxLod = QualitySettings.maximumLODLevel,
            VSyncCount = QualitySettings.vSyncCount,
            AnimationSkinWeights = QualitySettings.skinWeights,
            BillboardFacingQuality = QualitySettings.billboardsFaceCameraPosition,
            SoftParticles = QualitySettings.softParticles,
            SoftVegetation = QualitySettings.softVegetation,
            ShadowQuality = QualitySettings.shadows,
            ShadowDistance = QualitySettings.shadowDistance,
            ShadowResolution = QualitySettings.shadowResolution,
            ShadowProjection = QualitySettings.shadowProjection,
            ShadowmaskMode = QualitySettings.shadowmaskMode,
            ShadowCascades = QualitySettings.shadowCascades,
            ShadowNearPlaneOffset = QualitySettings.shadowNearPlaneOffset,
            PixelLightCount = QualitySettings.pixelLightCount,
            AntiAliasingMode = CurrentConfig.AntiAliasingMode,
            UseBloom = CurrentConfig.UseBloom,
            BloomValue = CurrentConfig.BloomValue,
            UseAmbientOcclusion = CurrentConfig.UseAmbientOcclusion,
			AmbientOcclusionValue = CurrentConfig.AmbientOcclusionValue,

			UseAutoExposure = CurrentConfig.UseAutoExposure,
			UseChromaticAbberation = CurrentConfig.UseChromaticAbberation,
			UseColorGrading = CurrentConfig.UseColorGrading,
			UseDepthOfField = CurrentConfig.UseDepthOfField,
			UseGrain = CurrentConfig.UseGrain,
			UseMotionBlur = CurrentConfig.UseMotionBlur,
			UseScreenSpaceReflections = CurrentConfig.UseScreenSpaceReflections,
			UseVignette = CurrentConfig.UseVignette,

            FrameLimit = Application.targetFrameRate,
        };

        SaveToJson(graphicsConfig, Application.persistentDataPath + "/Configs/GraphicsConfig.json");
    }

    public void LoadGraphicsConfig(string path)
    {
        if (File.Exists(path))
        {
            GraphicsConfig graphicsConfig = LoadFileFromJson<GraphicsConfig>(path);

            if (graphicsConfig != null)
            {
                foreach (Resolution resolution in Screen.resolutions)
                {
                    if (resolution.width == graphicsConfig.ResolutionX && resolution.height == graphicsConfig.ResolutionY)
                    {
                        Screen.SetResolution(graphicsConfig.ResolutionX, graphicsConfig.ResolutionY, graphicsConfig.FullScreen, graphicsConfig.RefreshRate);
                        break;
                    }
                }
                CurrentConfig.AntiAliasingMode = graphicsConfig.AntiAliasingMode;
                CurrentConfig.UseBloom = graphicsConfig.UseBloom;
                CurrentConfig.BloomValue = graphicsConfig.BloomValue;
                CurrentConfig.UseAmbientOcclusion = graphicsConfig.UseAmbientOcclusion;
                CurrentConfig.AmbientOcclusionValue = graphicsConfig.AmbientOcclusionValue;

				CurrentConfig.UseAutoExposure = graphicsConfig.UseAutoExposure;
				CurrentConfig.UseChromaticAbberation = graphicsConfig.UseChromaticAbberation;
				CurrentConfig.UseColorGrading = graphicsConfig.UseColorGrading;
				CurrentConfig.UseDepthOfField = graphicsConfig.UseDepthOfField;
				CurrentConfig.UseGrain = graphicsConfig.UseGrain;
				CurrentConfig.UseMotionBlur = graphicsConfig.UseMotionBlur;
				CurrentConfig.UseScreenSpaceReflections = graphicsConfig.UseScreenSpaceReflections;
				CurrentConfig.UseVignette = graphicsConfig.UseVignette;

                Application.targetFrameRate = graphicsConfig.FrameLimit;

                QualitySettings.maximumLODLevel = graphicsConfig.MaxLod;
                QualitySettings.softParticles = graphicsConfig.SoftParticles;
                QualitySettings.softVegetation = graphicsConfig.SoftVegetation;
                QualitySettings.shadowProjection = graphicsConfig.ShadowProjection;
                QualitySettings.shadowNearPlaneOffset = graphicsConfig.ShadowNearPlaneOffset;

                if (graphicsConfig.QualityLevel < 6)
                {
                    QualitySettings.SetQualityLevel(graphicsConfig.QualityLevel);
                }
                else if (graphicsConfig.QualityLevel == 6)
                {
                    customGraphicsQuality = true;

                    QualitySettings.masterTextureLimit = graphicsConfig.TextureQuality;
                    QualitySettings.anisotropicFiltering = graphicsConfig.AnisotropicFiltering;
                    QualitySettings.realtimeReflectionProbes = graphicsConfig.RealTimeReflectionProbes;
                    QualitySettings.resolutionScalingFixedDPIFactor = graphicsConfig.DpiFactor;
                    QualitySettings.lodBias = graphicsConfig.LodDistance;
                    QualitySettings.vSyncCount = graphicsConfig.VSyncCount;
                    QualitySettings.skinWeights = graphicsConfig.AnimationSkinWeights;
                    QualitySettings.billboardsFaceCameraPosition = graphicsConfig.BillboardFacingQuality;                
                    QualitySettings.shadows = graphicsConfig.ShadowQuality;
                    QualitySettings.shadowDistance = graphicsConfig.ShadowDistance;
                    QualitySettings.shadowResolution = graphicsConfig.ShadowResolution;                   
                    QualitySettings.shadowmaskMode = graphicsConfig.ShadowmaskMode;
                    QualitySettings.shadowCascades = graphicsConfig.ShadowCascades;
                    QualitySettings.pixelLightCount = graphicsConfig.PixelLightCount;
                }
            }
            else
            {
                Debug.Log("Failed to load Graphics config: " + path);

                Application.targetFrameRate = 120;
            }
        }
    }

    public void SaveControlConfig()
    {
        SaveToJson(InputManager.Instance.ControlsConfig, Application.persistentDataPath + "/Configs/ControlsConfig.json");
    }

    public void LoadControlConfig(string path)
    {
        if (File.Exists(path))
        {
            try
            {
                ControlsConfig controlsConfig = LoadFileFromJson<ControlsConfig>(path);

                InputManager.Instance.LoadConfiguration(controlsConfig);

            }
            catch (Exception err)
            {
                Debug.Log($"{err.Message}, {err.StackTrace}");
            }
        }
    }

    T GetElementFromCollection<T>(string name, IList<T> collection) where T : UnityEngine.Object
    {       
        for (int i = 0; i < collection.Count; i++)
        {
            T element = collection[i];

            if (element.name == name)
            {
                return element;
            }
        }

        return null;
    }

    void StoreObjectCollectionToDictionary<T>(IList<T> collection, Dictionary<string, T> dictionary) where T : UnityEngine.Object
    {
        for (int i = 0; i < collection.Count; i++)
        {
            T element = collection[i];

            if (element == null)
            {
                Debug.LogError("Error: Tried to store null value to dictionary");
            }
            else
            {
                dictionary[element.name] = element;
            }
        }
    }

    public static FileInfo[] GetFilesByType(string path, SearchOption searchOption, params string[] extensions)
    {
        DirectoryInfo dirInfo = new DirectoryInfo(path);
        List<FileInfo> allFiles = new List<FileInfo>();
        FileInfo[] files;
        for (int i = 0; i < extensions.Length; i++)
        {
            files = dirInfo.GetFiles(extensions[i], searchOption);
            allFiles.AddRange(files);
        }
        return allFiles.ToArray();
    }

    public static void SaveToXML<T>(T source, string path)
    {
        XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));

        using (StreamWriter streamWriter = new StreamWriter(path, false, Encoding.UTF8))
        {
            xmlSerializer.Serialize(streamWriter, source);
            streamWriter.Close();
        }
    }

    public static Texture2D GetTextureFromFile(string path)
    {
        byte[] data = File.ReadAllBytes(path);

        if (data.Length > 0)
        {
            Texture2D texture = new Texture2D(2, 2);
            texture.LoadImage(data);
            return texture;
        }

        return null;
    }

    public static T LoadDataFromXML<T>(string path)
    {
        T xmlObject = default;

        XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));

        using (StreamReader streamReader = new StreamReader(path))
        {
            xmlObject = (T)xmlSerializer.Deserialize(streamReader);
            streamReader.Close();
        }

        return xmlObject;
    }

    public static void LoadAssetBundles(string path, Dictionary<string, GameObject> targetDictionary)
    {
        if (Directory.Exists(path))
        {
            FileInfo[] files = GetFilesByType(path, SearchOption.AllDirectories, "*.assetbundle");

            for (int fileIndex = 0; fileIndex < files.Length; fileIndex++)
            {
                AssetBundle assetBundle = AssetBundle.LoadFromFile(path + "/" + files[fileIndex].Name);

                if (assetBundle != null)
                {
                    GameObject[] assets = assetBundle.LoadAllAssets<GameObject>();

                    for (int i = 0; i < assets.Length; i++)
                    {
                        GameObject assetGameObject = assets[i];

                        targetDictionary[assetGameObject.name] = assetGameObject;
                    }
                }
            }
        }
    }

    async Task<AudioClip> LoadAudioClip(string path)
    {
        AudioClip clip = null;

        using (UnityWebRequest webRequest = UnityWebRequestMultimedia.GetAudioClip(path, AudioType.WAV))
        {
            webRequest.SendWebRequest();

            try
            {
                while (!webRequest.isDone) await Task.Delay(5);

                if (webRequest.isNetworkError || webRequest.isHttpError)
                {
                    Debug.Log($"{webRequest.error}");
                }
                else
                {
                    clip = DownloadHandlerAudioClip.GetContent(webRequest);
                }
            }
            catch (Exception err)
            {
                Debug.Log($"{err.Message}, {err.StackTrace}");
            }
        }

        return clip;
    }

    public static T GetElementFromDictionary<T>(string key, Dictionary<string, T> dictionary)
    {
        if (dictionary.TryGetValue(key, out T element))
        {
            return element;
        }

        return default;
    }

    public static string GetKeyFromDictionary<T>(T target, Dictionary<string, T> targetDictionary)
    {
        for (int i = 0; i < targetDictionary.Count; i++)
        {
            KeyValuePair<string, T> element = targetDictionary.ElementAt(i);

            if ((object)target == (object)element.Value)
            {
                return element.Key;
            }
        }

        return "";
    }

    public static T GetRandomElementFromCollection<T>(IList<T> collection)
    {
        if (collection.Count > 0)
        {
            return collection[UnityEngine.Random.Range(0, collection.Count)];
        }

        return default;
    }

    public static T GetRandomElementFromDictionary<T>(Dictionary<string, T> targetDictionary)
    {
        if (targetDictionary.Count > 0)
        {
            return targetDictionary.ElementAt(UnityEngine.Random.Range(0, targetDictionary.Count)).Value;
        }

        return default;
    }

    T LoadFileFromJson<T>(string path)
    {
        return JsonUtility.FromJson<T>(File.ReadAllText(path));
    }

    void SaveToJson(object source, string path)
    {
        string serializedJson = JsonUtility.ToJson(source, true);

        File.WriteAllText(path, serializedJson);
    }

#if UNITY_EDITOR
    public void AutoFillPrefabs()
    {
        string path = Application.dataPath + "/Prefabs/";

        audioClips = GetPrefabsFromFiles<AudioClip>(GetFilesByType(Application.dataPath + "/AudioClips/", SearchOption.AllDirectories, "*.wav", "*.mp3", "*.ogg"));

        mechPrefabs = GetPrefabsFromFiles<GameObject>(GetFilesByType(path + "Mechs/", SearchOption.AllDirectories, "*.prefab"));
        groundVehiclePrefabs = GetPrefabsFromFiles<GameObject>(GetFilesByType(path + "GroundVehicles/", SearchOption.AllDirectories, "*.prefab"));
        turretPrefabs = GetPrefabsFromFiles<GameObject>(GetFilesByType(path + "Turrets/", SearchOption.AllDirectories, "*.prefab"));
        buildingPrefabs = GetPrefabsFromFiles<GameObject>(GetFilesByType(path + "Buildings/", SearchOption.AllDirectories, "*.prefab"));
        cockpitPrefabs = GetPrefabsFromFiles<GameObject>(GetFilesByType(path + "Cockpits/", SearchOption.AllDirectories, "*.prefab"));
        weaponModelPrefabs = GetPrefabsFromFiles<GameObject>(GetFilesByType(path + "Weapons/Models/", SearchOption.AllDirectories, "*.prefab"));
        beamPrefabs = GetPrefabsFromFiles<GameObject>(GetFilesByType(path + "Weapons/Beams/", SearchOption.AllDirectories, "*.prefab"));
        projectilePrefabs = GetPrefabsFromFiles<GameObject>(GetFilesByType(path + "Weapons/Projectiles/", SearchOption.AllDirectories, "*.prefab"));
        trailPrefabs = GetPrefabsFromFiles<GameObject>(GetFilesByType(path + "Weapons/Trails/", SearchOption.AllDirectories, "*.prefab"));
        effectPrefabs = GetPrefabsFromFiles<GameObject>(GetFilesByType(path + "Effects/", SearchOption.AllDirectories, "*.prefab"));
        accessoryModelPrefabs = GetPrefabsFromFiles<GameObject>(GetFilesByType(path + "Accessories/Models/", SearchOption.AllDirectories, "*.prefab"));
        planetPrefabs = GetPrefabsFromFiles<GameObject>(GetFilesByType(path + "Space/Planets/", SearchOption.AllDirectories, "*.prefab"));
        baseFoundations = GetPrefabsFromFiles<GameObject>(GetFilesByType(path + "Foundations/BaseFoundations/", SearchOption.AllDirectories, "*.prefab"));
        //skyProfiles = GetPrefabsFromFiles<UnityEngine.AzureSky.AzureWeatherProfile>(GetFilesByType(path + "SkyProfiles/", SearchOption.AllDirectories, "*.asset"));

        mapTextures2D = GetPrefabsFromFiles<Texture2D>(GetFilesByType(Application.dataPath + "/Textures/Maps/", SearchOption.AllDirectories, "*.png"));
        pilotMaleTextures2D = GetPrefabsFromFiles<Texture2D>(GetFilesByType(Application.dataPath + "/Textures/Pilots/Male/", SearchOption.AllDirectories, "*.png"));
        pilotFemaleTextures2D = GetPrefabsFromFiles<Texture2D>(GetFilesByType(Application.dataPath + "/Textures/Pilots/Female/", SearchOption.AllDirectories, "*.png"));
        pilotGenericTextures2D = GetPrefabsFromFiles<Texture2D>(GetFilesByType(Application.dataPath + "/Textures/Pilots/Generic/", SearchOption.AllDirectories, "*.png"));
        pilotUniqueTextures2D = GetPrefabsFromFiles<Texture2D>(GetFilesByType(Application.dataPath + "/Textures/Pilots/Unique/", SearchOption.AllDirectories, "*.png"));
        logoTextures2D = GetPrefabsFromFiles<Texture2D>(GetFilesByType(Application.dataPath + "/Textures/Logos/", SearchOption.AllDirectories, "*.png"));
        loadingScreenTextures2D = GetPrefabsFromFiles<Texture2D>(GetFilesByType(Application.dataPath + "/Textures/LoadingScreens/", SearchOption.AllDirectories, "*.png"));

        string[] mechSkinDirectories = Directory.GetDirectories(Application.dataPath + "/Textures/Mechs/UniqueSkins/");
        FileInfo[] mechUniqueSkinTextureFiles = GetFilesByType(Application.dataPath + "/Textures/Mechs/UniqueSkins/", SearchOption.AllDirectories, "*.png");
        FileInfo[] mechUniversalSkinTextureFiles = GetFilesByType(Application.dataPath + "/Textures/Mechs/UniversalSkins/", SearchOption.AllDirectories, "*.png");
        mechSkinGroups = new MechSkinGroup[mechSkinDirectories.Length];

        for (int i = 0; i < mechSkinGroups.Length; i++)
        {
            mechSkinGroups[i] = new MechSkinGroup()
            {
                chassis = new DirectoryInfo(mechSkinDirectories[i]).Name,
            };
        }

        for (int i = 0; i < mechUniqueSkinTextureFiles.Length; i++)
        {
            for (int groupIndex = 0; groupIndex < mechSkinGroups.Length; groupIndex++)
            {
                MechSkinGroup mechSkinGroup = mechSkinGroups[groupIndex];

                if (mechSkinGroup.chassis == mechUniqueSkinTextureFiles[i].Directory.Name)
                {
                    mechSkinGroup.skins.Add(StaticHelper.GetAssetFromFile<Texture2D>(mechUniqueSkinTextureFiles[i]));
                }
            }
        }

        mechUniversalSkinTextureList.Clear();

        for (int i = 0; i < mechUniversalSkinTextureFiles.Length; i++)
        {
            mechUniversalSkinTextureList.Add(StaticHelper.GetAssetFromFile<Texture2D>(mechUniversalSkinTextureFiles[i]));
        }

        EditorUtility.SetDirty(this);
        EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
    }

    T[] GetPrefabsFromFiles<T>(FileInfo[] files) where T : class
    {
        List<T> elements = new List<T>();

        for (int i = 0; i < files.Length; i++)
        {
            T element = StaticHelper.GetAssetFromFile<T>(files[i]);

            if (element != null)
            {
                elements.Add(element);
            }
        }

        return elements.ToArray();
    }
#endif

    [Serializable]
    public class MechSkinGroup
    {
        public string chassis;

        public List<Texture2D> skins = new List<Texture2D>();
    }
}
