using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InstantActionGlobalData
{
    public PlanetDefinition selectedPlanetDefinition = null;

    public MapDefinition selectedMapDefinition = null;

    public int selectedWeatherIndex = -1;

    public int selectedTimeOfDayIndex = -1;

    public MissionType selectedMissionType = MissionType.None;

    public MissionDefinition selectedMissionDefintion = null;

    public FactionDefinition selectedEnemyFaction = null;

    public FactionDefinition selectedSecondFaction = null;

    public int difficultyTierIndex;

    public MechData playerMechData;

    public MechData[] squadMateMechDatas = new MechData[11];

    public PilotDefinition[] squadPilotDefinitions = new PilotDefinition[11];


    public InstantActionGlobalData()
    {

    }

    public void SetDefaultDesign(MechDesign defaultMechDesign)
    {
        if (playerMechData == null)
        {
            playerMechData = new MechData(defaultMechDesign);
            playerMechData.mechPaintScheme = new MechPaintScheme();
        }

        for (int i = 0; i < squadMateMechDatas.Length; i++)
        {
            if (squadMateMechDatas[i] == null)
            {
                squadMateMechDatas[i] = new MechData(defaultMechDesign);
                squadMateMechDatas[i].mechPaintScheme = new MechPaintScheme();
            }
        }
    }

    public void SetDifficultyTier(int targetTier)
    {
        difficultyTierIndex = Mathf.Clamp(targetTier, 0, ResourceManager.Instance.MissionDifficultyConfig.MissionDifficultyTiers.Length - 1);
    }

    public void IncrementDifficultyTier()
    {
        difficultyTierIndex++;

        if (difficultyTierIndex == ResourceManager.Instance.MissionDifficultyConfig.MissionDifficultyTiers.Length)
        {
            difficultyTierIndex = 0;
        }
    }

    public void DecrementDifficultyTier()
    {
        difficultyTierIndex--;

        if (difficultyTierIndex < 0)
        {
            difficultyTierIndex = ResourceManager.Instance.MissionDifficultyConfig.MissionDifficultyTiers.Length - 1;
        }
    }
}
