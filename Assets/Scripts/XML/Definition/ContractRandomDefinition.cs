using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class ContractRandomDefinition : ContractDefinition
{
    public float RoleChance = 1.0f;

    public int RoleMin = 1;

    public int RoleMax = 4;

    public string[] Planets = new string[0];

    public MissionType[] MissionTypes = new MissionType[0];

    public string[] EnemyFactions = new string[0];

    public string[] SecondFactions = new string[0];

    public int RandomMissionsMin = 1;

    public int RandomMissionMax = 5;

    public int RandomDifficultyMin = 0;

    public int RandomDifficultyMax = 11;

    public int RandomContractCount
    {
        get
        {
            int contractCount = 0;

            RoleMin = Mathf.Max(RoleMin, 1);

            RoleMax = Mathf.Max(RoleMin, RoleMax);

            int tries = Random.Range(RoleMin, RoleMax + 1);

            for (int i = 0; i < tries; i++)
            {
                if (Random.Range(0.0f, 1.0f) <= RoleChance)
                    contractCount++;
            }

            return contractCount;
        }
    }

    public int RandomMissionCount
    {
        get
        {
            if (RandomMissionsMin < 1)
            {
                RandomMissionsMin = 1;
            }

            if (RandomMissionMax <= RandomMissionsMin)
            {
                return RandomMissionsMin;
            }

            return Random.Range(RandomMissionsMin, RandomMissionMax + 1);
        }
    }

    public int RandomDifficulty
    {
        get
        {
            if (RandomDifficultyMin < 1)
            {
                RandomDifficultyMin = 1;
            }

            if (RandomDifficultyMax <= RandomDifficultyMin)
            {
                return RandomDifficultyMin;
            }

            return Random.Range(RandomDifficultyMin, RandomDifficultyMax + 1);
        }
    }

    public PlanetDefinition RandomPlanetDefinition
    {
        get
        {
            if (Planets.Length > 0)
            {
                List<PlanetDefinition> planetDefinitions = new List<PlanetDefinition>();
                PlanetDefinition planetDefinition;

                foreach (string planetKey in Planets)
                {
                    planetDefinition = ResourceManager.Instance.GetPlanetDefinition(planetKey);

                    if (planetDefinition != null)
                    {
                        planetDefinitions.Add(planetDefinition);
                    }
                }

                if (planetDefinitions.Count > 0)
                {
                    return planetDefinitions[Random.Range(0, planetDefinitions.Count)];
                }
            }

            return null;
        }
    }

    public FactionDefinition RandomEnemyFaction
    {
        get
        {
            if (EnemyFactions.Length > 0)
            {
                List<FactionDefinition> factionDefinitions = new List<FactionDefinition>();
                FactionDefinition factionDefinition;

                foreach (string factionKey in EnemyFactions)
                {
                    factionDefinition = ResourceManager.Instance.GetFactionDefinition(factionKey);

                    if (factionDefinition != null)
                    {
                        factionDefinitions.Add(factionDefinition);
                    }
                }

                if (factionDefinitions.Count > 0)
                {
                    return factionDefinitions[Random.Range(0, factionDefinitions.Count)];
                }
            }

            return null;
        }
    }

    public FactionDefinition RandomSecondFaction
    {
        get
        {
            if (SecondFactions.Length > 0)
            {
                List<FactionDefinition> factionDefinitions = new List<FactionDefinition>();
                FactionDefinition factionDefinition;

                foreach (string factionKey in SecondFactions)
                {
                    factionDefinition = ResourceManager.Instance.GetFactionDefinition(factionKey);

                    if (factionDefinition != null)
                    {
                        factionDefinitions.Add(factionDefinition);
                    }
                }

                if (factionDefinitions.Count > 0)
                {
                    return factionDefinitions[Random.Range(0, factionDefinitions.Count)];
                }
            }

            return null;
        }
    }
}
