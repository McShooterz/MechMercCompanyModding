using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ContractSave
{
    public string ContractDefinition = "";

    public string EmployerDefinition = "";

    public string EnemyDefinition = "";

    public string PlanetDefinition = "";

    public int Difficulty = 1;

    public int DifficultyEstimated = 1;

    public GameDate StartDate;

    public GameDate EndDate;

    public int MissionIndex = 0;

    public int MissionsSuccessfulCount = 0;

    public MissionSetupSave[] Missions = new MissionSetupSave[0];

    public bool IsCareerCurrent = false;

    public ContractDefinition GetContractDefinition()
    {
        return ResourceManager.Instance.GetContractDefinition(ContractDefinition);
    }

    public FactionDefinition GetEmployerDefinition()
    {
        return ResourceManager.Instance.GetFactionDefinition(EmployerDefinition);
    }

    public FactionDefinition GetEnemyDefinition()
    {
        return ResourceManager.Instance.GetFactionDefinition(EnemyDefinition);
    }

    public PlanetDefinition GetPlanetDefinition()
    {
        return ResourceManager.Instance.GetPlanetDefinition(PlanetDefinition);
    }
}
