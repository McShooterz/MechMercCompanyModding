using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ContractData
{
    [SerializeField]
    int difficulty;

    [SerializeField]
    int difficultyEstimated;

    [SerializeField]
    GameDate startDate = new GameDate();

    [SerializeField]
    GameDate endDate = new GameDate();

    [SerializeField]
    int missionIndex = 0;

    [SerializeField]
    int missionsSuccessfulCount = 0;

    [SerializeField]
    MissionSetup[] missionSetups = new MissionSetup[0];

    public ContractDefinition ContractDefinition { get; private set; }

    public FactionDefinition EmployerDefinition { get; private set; }

    public FactionDefinition EnemyDefinition { get; private set; }

    public PlanetDefinition PlanetDefinition { get; private set; }

    public int Difficulty { get => difficulty; }

    public int DifficultyEstimated { get => difficultyEstimated; }

    public GameDate StartDate { get => startDate; }

    public GameDate EndDate { get => endDate; }

    public int Duration { get => missionSetups.Length * 7 + 14; }

    public int MissionIndex { get => missionIndex; }

    public bool OnLastMission { get => missionIndex == missionSetups.Length - 1; }

    public int MissionsSuccessfulCount { get => missionsSuccessfulCount; }

    public int TravelCoverage
    {
        get
        {
            if (ContractDefinition is ContractUniqueDefinition)
            {

            }

            return ResourceManager.Instance.MissionDifficultyConfig.MissionDifficultyTiers[difficultyEstimated].TravelCoverage;
        }
    }

    public int BountyKillPay
    {
        get
        {
            if (ContractDefinition is ContractUniqueDefinition)
            {

            }

            return ResourceManager.Instance.MissionDifficultyConfig.MissionDifficultyTiers[difficultyEstimated].KillBountyPay;
        }
    }

    public int MissionPay
    {
        get
        {
            if (ContractDefinition is ContractUniqueDefinition)
            {

            }

            int missionPay = 0;

            for (int i = 0; i < missionSetups.Length; i++)
            {
                missionPay += ResourceManager.Instance.MissionDifficultyConfig.GetPayPotential(missionSetups[i].missionType, difficulty);
            }

            return missionPay;
        }
    }

    public int ContractPayPotential
    {
        get
        {
            if (ContractDefinition is ContractUniqueDefinition)
            {

            }

            return ResourceManager.Instance.MissionDifficultyConfig.MissionDifficultyTiers[difficultyEstimated].ContractPayMultiplier * (int)Mathf.Pow(1.5f, missionSetups.Length);
        }
    }

    public int ContractPayActual
    {
        get
        {
            if (ContractDefinition is ContractUniqueDefinition)
            {

            }

            return ResourceManager.Instance.MissionDifficultyConfig.MissionDifficultyTiers[difficultyEstimated].ContractPayMultiplier * (int)Mathf.Pow(1.5f, missionsSuccessfulCount);
        }
    }

    public int ReputationFinal
    {
        get
        {
            if (ContractDefinition is ContractUniqueDefinition)
            {

            }

            MissionDifficultyTier missionDifficultyTier = ResourceManager.Instance.MissionDifficultyConfig.MissionDifficultyTiers[difficultyEstimated];

            int reputationPerMission = missionDifficultyTier.Reputation + missionDifficultyTier.ReputationGrowth * (int)(missionsSuccessfulCount - 0.5f);

            return missionsSuccessfulCount * reputationPerMission;
        }
    }

    public MissionSetup CurrentMission { get => missionSetups[missionIndex]; }

    public MissionSetup[] MissionSetups { get => missionSetups; }

    public ContractSave ContractSave
    {
        get
        {
            MissionSetupSave[] missionSaves = new MissionSetupSave[missionSetups.Length];

            for (int i = 0; i < missionSaves.Length; i++)
            {
                missionSaves[i] = new MissionSetupSave(missionSetups[i]);
            }

            return new ContractSave()
            {
                ContractDefinition = ContractDefinition.Key,

                EmployerDefinition = EmployerDefinition.Key,

                EnemyDefinition = EnemyDefinition.Key,

                PlanetDefinition = PlanetDefinition.Key,

                Difficulty = difficulty,

                DifficultyEstimated = difficultyEstimated,

                StartDate = new GameDate(startDate),

                EndDate = new GameDate(endDate),

                MissionIndex = missionIndex,

                MissionsSuccessfulCount = missionsSuccessfulCount,

                Missions = missionSaves,
            };
        }
    }

    public ContractData(ContractDefinition contractDefinition)
    {
        ContractDefinition = contractDefinition;

        if (ContractDefinition == null)
        {
            Debug.LogError("Contract definition is null");
        }

        EmployerDefinition = contractDefinition.GetEmployer();

        if (ContractDefinition is ContractRandomDefinition)
        {
            ContractRandomDefinition contractRandomDefinition = ContractDefinition as ContractRandomDefinition;

            EnemyDefinition = contractRandomDefinition.RandomEnemyFaction;

            if (EnemyDefinition == null)
                Debug.LogError("No Enemy Definition");

            PlanetDefinition = contractRandomDefinition.RandomPlanetDefinition;

            if (PlanetDefinition == null)
                Debug.LogError("No Planet Definition");

            difficulty = contractRandomDefinition.RandomDifficulty;
            difficultyEstimated = difficulty + Random.Range(-1, 2);
            difficultyEstimated = Mathf.Clamp(difficultyEstimated, 0, ResourceManager.Instance.MissionDifficultyConfig.MissionDifficultyTiers.Length - 1);

            int missionCount = contractRandomDefinition.RandomMissionCount;
            List<MapDefinition> mapDefinitions = PlanetDefinition.GetMaps();

            // Check to see if maps have mission type
            for (int i = 0; i < mapDefinitions.Count; i++)
            {
                bool hasValidMissionType = false;

                for (int missionTypeIndex = 0; missionTypeIndex < mapDefinitions[i].MissionTypes.Length; missionTypeIndex++)
                {
                    if (contractRandomDefinition.MissionTypes.Contains(mapDefinitions[i].MissionTypes[missionTypeIndex]))
                    {
                        hasValidMissionType = true;
                        break;
                    }
                }

                if (!hasValidMissionType)
                {
                    mapDefinitions.RemoveAt(i);
                    i--;
                }
            }

            if (mapDefinitions.Count > 0)
            {
                missionSetups = new MissionSetup[missionCount];

                FactionDefinition secondFactionDefinition = contractRandomDefinition.RandomSecondFaction;

                for (int i = 0; i < missionCount; i++)
                {
                    MapDefinition randomMap = mapDefinitions[Random.Range(0, mapDefinitions.Count)];

                    List<MissionType> missionTypes = new List<MissionType>();

                    for (int missionTypeIndex = 0; missionTypeIndex < randomMap.MissionTypes.Length; missionTypeIndex++)
                    {
                        MissionType missionType = randomMap.MissionTypes[missionTypeIndex];

                        if (contractRandomDefinition.MissionTypes.Contains(missionType))
                        {
                            missionTypes.Add(missionType);
                        }
                    }

                    MissionType randomMissionType = missionTypes[Random.Range(0, missionTypes.Count)];

                    MissionSetup missionSetup = new MissionSetup();

                    missionSetups[i] = missionSetup;

                    missionSetup.GenerateSeed();
                    missionSetup.planetDefinition = PlanetDefinition.Key;
                    missionSetup.mapDefinion = randomMap.Key;
                    missionSetup.timeOfDayIndex = randomMap.RandomIndexSkyTime;
                    missionSetup.weatherIndex = randomMap.RandomIndexSkyWeather;
                    missionSetup.missionType = randomMissionType;
                    missionSetup.difficultyTier = difficulty;
                    missionSetup.enemyFactionDefinition = EnemyDefinition.Key;
                    missionSetup.secondFactionDefinition = secondFactionDefinition.Key;
                }
            }
            else
            {
                Debug.Log("Planet: " + PlanetDefinition.DisplayName);
                Debug.LogError("No valid maps");
            }

            int randomStartOffset = Random.Range(0, 5) * 7;

            startDate = new GameDate(GlobalDataManager.Instance.currentCareer.gameDate);
            startDate.AddDays(randomStartOffset);

            endDate = new GameDate(startDate);
            endDate.AddDays(14 + missionCount * 7);
        }
        else
        {
            ContractUniqueDefinition contractUniqueDefinition = ContractDefinition as ContractUniqueDefinition;

            Debug.LogError("Error: Unique contracts not supported");
        }
    }

    public ContractData(ContractSave contractSave)
    {
        ContractDefinition = contractSave.GetContractDefinition();
        EmployerDefinition = contractSave.GetEmployerDefinition();
        EnemyDefinition = contractSave.GetEnemyDefinition();
        PlanetDefinition = contractSave.GetPlanetDefinition();

        if (ContractDefinition == null)
            Debug.LogError("Error: ContractDefinition is null");

        if (EmployerDefinition == null)
            Debug.LogError("Error: Employer FactionDefinition is null");

        if (EnemyDefinition == null)
            Debug.LogError("Error: Enemy FactionDefinition is null");

        if (PlanetDefinition == null)
            Debug.LogError("Error: PlanetDefinition is null");

        difficulty = contractSave.Difficulty;
        difficultyEstimated = contractSave.DifficultyEstimated;
        startDate = new GameDate(contractSave.StartDate);
        endDate = new GameDate(contractSave.EndDate);

        missionIndex = contractSave.MissionIndex;

        missionsSuccessfulCount = contractSave.MissionsSuccessfulCount;

        if (contractSave.Missions.Length > 0)
        {
            List<MissionSetup> missionSetupList = new List<MissionSetup>();

            for (int i = 0; i < contractSave.Missions.Length; i++)
            {
                missionSetupList.Add(contractSave.Missions[i].MissionSetup);
            }

            missionSetups = missionSetupList.ToArray();
        }
    }

    public static ContractData GetRandomContractData()
    {
        List<ContractRandomDefinition> contractDefinitions = ResourceManager.Instance.GetContractRandomDefinitions();

        if (contractDefinitions.Count > 0)
        {
            return new ContractData(contractDefinitions[Random.Range(0, contractDefinitions.Count)]);
        }

        return null;
    }

    public void IncrementSuccessfulMissionCount()
    {
        missionsSuccessfulCount++;
    }

    public void IncrementMissionIndex()
    {
        missionIndex++;
    }
}
