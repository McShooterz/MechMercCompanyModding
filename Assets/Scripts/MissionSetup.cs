using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MissionSetup
{
    public string planetDefinition = "";

    public string mapDefinion = "";

    public int weatherIndex = -1;

    public int timeOfDayIndex = -1;

    public MissionType missionType = MissionType.Battle;

    public int difficultyTier = 0;

    public string enemyFactionDefinition = "";

    public string secondFactionDefinition = "";

    [SerializeField]
    int randomSeed;

    public PlanetDefinition PlanetDefinition { get => ResourceManager.Instance.GetPlanetDefinition(planetDefinition); }

    public MapDefinition MapDefinition { get => ResourceManager.Instance.GetMapDefinition(mapDefinion); }

    public FactionDefinition EnemyFactionDefinition { get => ResourceManager.Instance.GetFactionDefinition(enemyFactionDefinition); }

    public FactionDefinition SecondFactionDefinition { get => ResourceManager.Instance.GetFactionDefinition(secondFactionDefinition); }

    public int RandomSeed { get => randomSeed; }

    public int MissionPayPotential
    {
        get
        {
            MissionDefinition missionDefinition = MapDefinition.GetCustomMissionDefinition();

            if (missionDefinition != null)
            {
                return missionDefinition.MissionPay;
            }

            return ResourceManager.Instance.MissionDifficultyConfig.GetPayPotential(missionType, difficultyTier);
        }
    }

    public MissionSetup()
    {

    }

    public void Copy(MissionSetup copyTarget)
    {
        planetDefinition = copyTarget.planetDefinition;

        mapDefinion = copyTarget.mapDefinion;

        weatherIndex = copyTarget.weatherIndex;

        timeOfDayIndex = copyTarget.timeOfDayIndex;

        missionType = copyTarget.missionType;

        difficultyTier = copyTarget.difficultyTier;

        enemyFactionDefinition = copyTarget.enemyFactionDefinition;

        secondFactionDefinition = copyTarget.secondFactionDefinition;

        randomSeed = copyTarget.RandomSeed;
    }

    public void GenerateSeed()
    {
        randomSeed = StaticHelper.RandomGeneratedSeed;
    }

    public void SetSeed(int seed) { randomSeed = seed; }

    public MissionData BuildMissionData()
    {
        Random.InitState(randomSeed);

        MissionData missionData = new MissionData();

        MapDefinition map = MapDefinition;

        if (map == null)
        {
            Debug.LogError("Map Definition not found");
        }

        FactionDefinition enemyFaction = EnemyFactionDefinition;

        if (enemyFaction == null)
        {
            Debug.LogError("Enemy Faction not found");
        }

        FactionDefinition secondFaction = null;

        if (secondFactionDefinition != "")
        {
            secondFaction = SecondFactionDefinition;

            if (secondFaction == null)
            {
                Debug.LogError("Second Faction not found");
            }
        }

        missionData.Build(map, missionType, difficultyTier, enemyFaction, secondFaction);

        return missionData;
    }

    public string DisplayName
    {
        get
        {
            MissionDefinition missionDefinition = MapDefinition.GetCustomMissionDefinition();

            if (missionDefinition != null)
            {
                return missionDefinition.GetDisplayName();
            }

            return StaticHelper.GetMissionTypeName(missionType);
        }
    }
}
