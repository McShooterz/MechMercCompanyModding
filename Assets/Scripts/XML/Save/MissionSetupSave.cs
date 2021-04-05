using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MissionSetupSave
{
    public string MapDefinition = "";

    public int WeatherIndex = -1;

    public int TimeOfDayIndex = -1;

    public int DifficultyTier = 0;

    public MissionType MissionType = MissionType.Battle;

    public string EnemyFactionDefinition = "";

    public string SecondFactionDefinition = "";

    public int RandomSeed = 0;

    public MissionSetup MissionSetup
    {
        get
        {
            MissionSetup missionSetup = new MissionSetup()
            {
                mapDefinion = MapDefinition,
                weatherIndex = WeatherIndex,
                timeOfDayIndex = TimeOfDayIndex,
                difficultyTier = DifficultyTier,
                missionType = MissionType,
                enemyFactionDefinition = EnemyFactionDefinition,
                secondFactionDefinition = SecondFactionDefinition,
            };

            missionSetup.SetSeed(RandomSeed);

            return missionSetup;
        }
    }

    public MissionSetupSave() { }

    public MissionSetupSave(MissionSetup missionSetup)
    {
        MapDefinition = missionSetup.mapDefinion;
        WeatherIndex = missionSetup.weatherIndex;
        TimeOfDayIndex = missionSetup.timeOfDayIndex;
        DifficultyTier = missionSetup.difficultyTier;
        MissionType = missionSetup.missionType;
        EnemyFactionDefinition = missionSetup.enemyFactionDefinition;
        SecondFactionDefinition = missionSetup.secondFactionDefinition;
        RandomSeed = missionSetup.RandomSeed;
    }
}