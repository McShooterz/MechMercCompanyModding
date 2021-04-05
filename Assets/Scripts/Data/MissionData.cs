using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MissionData
{
    public MissionType missionType;

    public MapDefinition mapDefinition;

    public bool missionStarted = false;

    public bool successful = false;

    public bool failed = false;

    public Objective[] objectives = new Objective[0];

    public GameObject baseFoundationPrefab;

    const int searchAndDestroyCount = 5;

    public string DisplayName
    {
        get
        {
            MissionDefinition missionDefinition = mapDefinition.GetCustomMissionDefinition();

            if (missionDefinition != null)
            {
                return missionDefinition.GetDisplayName();
            }

            return StaticHelper.GetMissionTypeName(missionType);
        }
    }

    public bool MissionOver { get => successful || failed; }

    public int MissionPayPotential
    {
        get
        {
            MissionDefinition missionDefinition = mapDefinition.GetCustomMissionDefinition();

            if (missionDefinition != null)
            {
                return missionDefinition.MissionPay;
            }

            int pay = 0;

            foreach (Objective objective in objectives)
            {
                pay += objective.pay;
            }

            return pay;
        }
    }

    public ObjectiveSave[] ObjectiveSaves
    {
        get
        {
            List<ObjectiveSave> objectiveSaves = new List<ObjectiveSave>();

            for (int i = 0; i < objectives.Length; i++)
            {
                Objective objective = objectives[i];

                if (objective.pay > 0)
                { 
                    objectiveSaves.Add(objectives[i].ObjectiveSave);
                }
            }

            return objectiveSaves.ToArray();
        }
    }

    public void SetCompleted()
    {
        successful = true;

        for (int i = 0; i < objectives.Length; i++)
        {
            Objective objective = objectives[i];

            if (objective.objectiveState == ObjectiveState.Active)
            {
                objective.objectiveState = ObjectiveState.Completed;
            }
        }
    }

    public void SetFailed()
    {
        failed = true;

        for (int i = 0; i < objectives.Length; i++)
        {
            Objective objective = objectives[i];

            if (objective.objectiveState == ObjectiveState.Active)
            {
                objective.objectiveState = ObjectiveState.Failed;
            }
        }
    }

    public void Build(MapDefinition mapDefinition, MissionType missionType, int difficultyTier, FactionDefinition enemyFaction, FactionDefinition secondFaction)
    {
        this.mapDefinition = mapDefinition;
        this.missionType = missionType;

        switch (missionType)
        {
            case MissionType.Battle:
                {
                    BuildBattleMission(difficultyTier, enemyFaction);
                    break;
                }
            case MissionType.SearchAndDestroy:
                {
                    BuildSearchAndDestroyMission(difficultyTier, enemyFaction);
                    break;
                }
            case MissionType.Assassination:
                {
                    BuildAssassinationMission(difficultyTier, enemyFaction);
                    break;
                }
            case MissionType.ConvoyDestroy:
                {
                    BuildConvoyDestroyMission(difficultyTier, enemyFaction);
                    break;
                }
            case MissionType.ConvoyCapture:
                {
                    BuildConvoyCaptureMission(difficultyTier, enemyFaction, secondFaction);
                    break;
                }
            case MissionType.ConvoyEscort:
                {
                    BuildConvoyEscortMission(difficultyTier, enemyFaction, secondFaction);
                    break;
                }
            case MissionType.BattleSupport:
                {
                    BuildBattleSupportMission(difficultyTier, enemyFaction, secondFaction);
                    break;
                }
            case MissionType.Recon:
                {
                    BuildReconMission(difficultyTier, enemyFaction);
                    break;
                }
            case MissionType.BaseDestroy:
                {
                    BuildBaseDestroyMission(difficultyTier, enemyFaction);
                    break;
                }
            case MissionType.BaseCapture:
                {
                    BuildBaseCaptureMission(difficultyTier, enemyFaction);
                    break;
                }
            case MissionType.BaseDefend:
                {
                    BuildBaseDefendMission(difficultyTier, enemyFaction, secondFaction);
                    break;
                }
        }
    }

    void BuildBattleMission(int difficultyTier, FactionDefinition enemyFaction)
    {
        List<UnitData> enemyUnits = ResourceManager.Instance.MissionDifficultyConfig.GetRandomForce(difficultyTier, MissionType.Battle, enemyFaction, false);

        objectives = new Objective[1];
        objectives[0] = new Objective();
        objectives[0].displayText = "Destroy all enemy units";
        objectives[0].objectiveType = ObjectiveType.DestroyAllUnits;
        objectives[0].objectiveState = ObjectiveState.Active;
        objectives[0].instantCompleteMission = true;
        objectives[0].units = enemyUnits.ToArray();
        objectives[0].pay = ResourceManager.Instance.MissionDifficultyConfig.GetPay_Battle_DestroyAllEnemies(difficultyTier);
    }

    void BuildSearchAndDestroyMission(int difficultyTier, FactionDefinition enemyFaction)
    {
        List<UnitData> allEnemyDatas = ResourceManager.Instance.MissionDifficultyConfig.GetRandomForce(difficultyTier, MissionType.SearchAndDestroy, enemyFaction, false);
        List<List<UnitData>> enemyUnits = new List<List<UnitData>>();
        int[] enemyCounts = new int[4];

        // Set hard limits for enemy count
        int enemyCount = allEnemyDatas.Count;
        
        // Add one so each nav point has at least one enemy
        for (int i = 0; i < enemyCounts.Length; i++)
        {
            if (enemyCount > 0)
            {
                enemyCount--;
                enemyCounts[i] += 1;
            }
            else
            {
                break;
            }
        }

        // Add units to each nav point
        while (enemyCount > 0)
        {
            int index = Random.Range(0, enemyCounts.Length);

            if (enemyCounts[index] < searchAndDestroyCount + 1)
            {
                enemyCount--;
                enemyCounts[index] += 1;
            }
        }

        // Sort unit datas into groups
        int indexAll = 0;

        for (int indexGroup = 0; indexGroup < 4; indexGroup++)
        {
            enemyUnits.Add(new List<UnitData>());

            for (int i = 0; i < enemyCounts[indexGroup]; i++)
            {
                enemyUnits[indexGroup].Add(allEnemyDatas[indexAll]);
                indexAll++;
            }
        }

        // Create objectives
        objectives = new Objective[4];
        for (int i = 0; i < objectives.Length; i++)
        {
            objectives[i] = new Objective();
            objectives[i].displayText = "Destroy enemy units at Nav Point " + StaticHelper.GetNavPointName(i);
            objectives[i].objectiveType = ObjectiveType.DestroyAllUnits;
            objectives[i].objectiveState = ObjectiveState.Active;
            objectives[i].units = enemyUnits[i].ToArray();
            objectives[i].pay = ResourceManager.Instance.MissionDifficultyConfig.GetPay_SearchAndDestroy_DestroyEnemiesAtNavPoint(difficultyTier);

            if (i < objectives.Length - 1)
            {
                objectives[i].navPointIndexSetOnSuccess = i + 1;
            }
            else
            {
                objectives[i].navPointIndexSetOnSuccess = 0;
            }
        }
    }

    void BuildAssassinationMission(int difficultyTier, FactionDefinition enemyFaction)
    {
        List<UnitData> enemyUnits = ResourceManager.Instance.MissionDifficultyConfig.GetRandomForce(difficultyTier, MissionType.Assassination, enemyFaction, false);

        objectives = Objective.CreateObjectiveArray(8);

        objectives[0].displayText = "Assassinate the target: " + (enemyUnits[0] as MechData).currentMechPilot.displayName;
        objectives[0].objectiveType = ObjectiveType.DestroyAllUnits;
        objectives[0].objectiveState = ObjectiveState.Active;
        objectives[0].units = new UnitData[] { enemyUnits[0] };
        objectives[0].objectivesToActivateOnSuccess = new Objective[] { objectives[5], objectives[6] };
        objectives[0].objectivesToDisableOnSuccess = new Objective[] { objectives[1], objectives[2], objectives[3], objectives[4] };
        objectives[0].objectivesToHideOnSuccess = new Objective[] { objectives[1], objectives[2], objectives[3], objectives[4] };
        objectives[0].objectivesToUnHideOnSuccess = new Objective[] { objectives[5], objectives[6] };
        objectives[0].navPointIndexSetOnSuccess = 4;
        objectives[0].alertEnemiesPlayerLocationOnSuccess = true;
        objectives[0].pay = ResourceManager.Instance.MissionDifficultyConfig.GetPay_Assassination_AssassinateTarget(difficultyTier);

        objectives[1].displayText = "Search for target at Nav Point " + StaticHelper.GetNavPointName(0);
        objectives[1].objectiveType = ObjectiveType.MoveToNavPoint;
        objectives[1].objectiveState = ObjectiveState.Active;
        objectives[1].optional = true;
        objectives[1].navPointIndex = 0;
        objectives[1].navPointIndexSetOnSuccess = 1;
        objectives[1].range = 20.0f;
        objectives[1].showDebriefing = false;

        objectives[2].displayText = "Search for target at Nav Point " + StaticHelper.GetNavPointName(1);
        objectives[2].objectiveType = ObjectiveType.MoveToNavPoint;
        objectives[2].objectiveState = ObjectiveState.Active;
        objectives[2].optional = true;
        objectives[2].navPointIndex = 1;
        objectives[2].navPointIndexSetOnSuccess = 2;
        objectives[2].range = 20.0f;
        objectives[2].showDebriefing = false;

        objectives[3].displayText = "Search for target at Nav Point " + StaticHelper.GetNavPointName(2);
        objectives[3].objectiveType = ObjectiveType.MoveToNavPoint;
        objectives[3].objectiveState = ObjectiveState.Active;
        objectives[3].optional = true;
        objectives[3].navPointIndex = 2;
        objectives[3].navPointIndexSetOnSuccess = 3;
        objectives[3].range = 20.0f;
        objectives[3].showDebriefing = false;

        objectives[4].displayText = "Search for target at Nav Point " + StaticHelper.GetNavPointName(3);
        objectives[4].objectiveType = ObjectiveType.MoveToNavPoint;
        objectives[4].objectiveState = ObjectiveState.Active;
        objectives[4].optional = true;
        objectives[4].navPointIndex = 3;
        objectives[4].range = 20.0f;
        objectives[4].showDebriefing = false;

        objectives[5].displayText = "Return to Nav Point " + StaticHelper.GetNavPointName(4) + " for extraction";
        objectives[5].objectiveType = ObjectiveType.MoveToNavPoint;
        objectives[5].objectiveState = ObjectiveState.Disabled;
        objectives[5].instantCompleteMission = true;
        objectives[5].navPointIndex = 4;
        objectives[5].range = 5.0f;
        objectives[5].hidden = true;
        objectives[5].objectivesToDisableOnSuccess = new Objective[] { objectives[6] };

        objectives[6].displayText = "Break contact with enemy forces for 120 seconds";
        objectives[6].objectiveType = ObjectiveType.BreakContactWithEnemy;
        objectives[6].objectiveState = ObjectiveState.Disabled;
        objectives[6].instantCompleteMission = true;
        objectives[6].timeLimit = 120.0f;
        objectives[6].objectivesToDisableOnSuccess = new Objective[] { objectives[5] };
        objectives[6].hidden = true;

        objectives[7].displayText = "Destroy all enemy units";
        objectives[7].objectiveType = ObjectiveType.DestroyAllUnits;
        objectives[7].objectiveState = ObjectiveState.Active;
        objectives[7].instantCompleteMission = true;
        objectives[7].objectivesToDisableOnSuccess = new Objective[] { objectives[5], objectives[6] };
        objectives[7].hidden = true;
        objectives[7].units = enemyUnits.ToArray();
        objectives[7].pay = ResourceManager.Instance.MissionDifficultyConfig.GetPay_Assassination_DestroyAllEnemies(difficultyTier);
    }

    void BuildConvoyDestroyMission(int difficultyTier, FactionDefinition enemyFaction)
    {
        List<UnitData> enemyUnits = ResourceManager.Instance.MissionDifficultyConfig.GetRandomForce(difficultyTier, MissionType.ConvoyDestroy, enemyFaction, false);
        List<UnitData> convoyUnits = ResourceManager.Instance.MissionDifficultyConfig.GetRandomConvoy(difficultyTier, enemyFaction);

        objectives = Objective.CreateObjectiveArray(5);

        objectives[0].displayText = "Prevent convoy from leaving mission area";
        objectives[0].objectiveType = ObjectiveType.PreventConvoyReachEnd;
        objectives[0].objectiveState = ObjectiveState.Active;
        objectives[0].instantFailMission = true;
        objectives[0].objectivesToFailOnFailure = new Objective[] { objectives[1] };

        objectives[1].displayText = "Destroy all convoy units";
        objectives[1].objectiveType = ObjectiveType.DestroyAllUnits;
        objectives[1].objectiveState = ObjectiveState.Active;
        objectives[1].units = convoyUnits.ToArray();
        objectives[1].objectivesToActivateOnSuccess = new Objective[] { objectives[2], objectives[3], objectives[4] };
        objectives[1].objectivesToCompleteOnSuccess = new Objective[] { objectives[0] };
        objectives[1].objectivesToUnHideOnSuccess = new Objective[] { objectives[2], objectives[3] };
        objectives[1].navPointIndexSetOnSuccess = 4;
        objectives[1].alertEnemiesPlayerLocationOnSuccess = true;
        objectives[1].pay = ResourceManager.Instance.MissionDifficultyConfig.GetPay_ConvoyDestroy_ConvoyDestroyed(difficultyTier);

        objectives[2].displayText = "Return to Nav Point " + StaticHelper.GetNavPointName(4) + " for extraction";
        objectives[2].objectiveType = ObjectiveType.MoveToNavPoint;
        objectives[2].objectiveState = ObjectiveState.Disabled;
        objectives[2].instantCompleteMission = true;
        objectives[2].hidden = true;
        objectives[2].navPointIndex = 4;
        objectives[2].range = 5.0f;
        objectives[2].objectivesToDisableOnSuccess = new Objective[] { objectives[3], objectives[4] };

        objectives[3].displayText = "Break contact with enemy forces for 120 seconds";
        objectives[3].objectiveType = ObjectiveType.BreakContactWithEnemy;
        objectives[3].objectiveState = ObjectiveState.Disabled;
        objectives[3].instantCompleteMission = true;
        objectives[3].timeLimit = 120.0f;
        objectives[3].objectivesToDisableOnSuccess = new Objective[] { objectives[2] };
        objectives[3].hidden = true;

        objectives[4].displayText = "Destroy all enemy units";
        objectives[4].objectiveType = ObjectiveType.DestroyAllUnits;
        objectives[4].objectiveState = ObjectiveState.Disabled;
        objectives[4].instantCompleteMission = true;
        objectives[4].hidden = true;
        objectives[4].units = enemyUnits.ToArray();
        objectives[4].objectivesToDisableOnSuccess = new Objective[] { objectives[2], objectives[3] };
        objectives[4].pay = ResourceManager.Instance.MissionDifficultyConfig.GetPay_ConvoyDestroy_DestroyAllEnemies(difficultyTier);
    }

    void BuildBattleSupportMission(int difficultyTier, FactionDefinition enemyFaction, FactionDefinition secondFaction)
    {
        List<UnitData> enemyUnits = ResourceManager.Instance.MissionDifficultyConfig.GetRandomForce(difficultyTier, MissionType.BattleSupport, enemyFaction, false);

        List<UnitData> allyUnits = ResourceManager.Instance.MissionDifficultyConfig.GetRandomForce(difficultyTier, MissionType.BattleSupport, secondFaction, true);

        objectives = Objective.CreateObjectiveArray(2);

        objectives[0].displayText = "Destroy all enemy units";
        objectives[0].objectiveType = ObjectiveType.DestroyAllUnits;
        objectives[0].objectiveState = ObjectiveState.Active;
        objectives[0].objectivesToCompleteOnSuccess = new Objective[] { objectives[1] };
        objectives[0].instantCompleteMission = true;
        objectives[0].units = enemyUnits.ToArray();
        objectives[0].pay = ResourceManager.Instance.MissionDifficultyConfig.GetPay_BattleSupport_DestroyAllEnemies(difficultyTier);

        objectives[1].displayText = "Some allied units survive the battle";
        objectives[1].objectiveType = ObjectiveType.ProtectAnyUnits;
        objectives[1].objectiveState = ObjectiveState.Active;
        objectives[1].units = allyUnits.ToArray();
        objectives[1].optional = true;
        objectives[1].pay = ResourceManager.Instance.MissionDifficultyConfig.GetPay_BattleSupport_ProtectAllies(difficultyTier);
    }

    void BuildReconMission(int difficultyTier, FactionDefinition enemyFaction)
    {
        List<UnitData> enemyUnits = ResourceManager.Instance.MissionDifficultyConfig.GetRandomForce(difficultyTier, MissionType.Recon, enemyFaction, false);

        objectives = Objective.CreateObjectiveArray(6);

        objectives[0].displayText = "Move to Nav Point " + StaticHelper.GetNavPointName(0);
        objectives[0].objectiveType = ObjectiveType.MoveToNavPoint;
        objectives[0].objectiveState = ObjectiveState.Active;
        objectives[0].range = 5.0f;
        objectives[0].navPointIndex = 0;
        objectives[0].navPointIndexSetOnSuccess = 0;
        objectives[0].alertEnemiesPlayerLocationOnSuccess = true;
        objectives[0].objectivesToActivateOnSuccess = new Objective[] { objectives[1] };
        objectives[0].objectivesToUnHideOnSuccess = new Objective[] { objectives[1] };
        objectives[0].objectivesToHideOnSuccess = new Objective[] { objectives[0] };
        objectives[0].showDebriefing = false;

        objectives[1].displayText = "Remain within 100m of Nav Point " + StaticHelper.GetNavPointName(0) + " for 10 seconds";
        objectives[1].objectiveType = ObjectiveType.RemainInRangeOfNavPoint;
        objectives[1].objectiveState = ObjectiveState.Disabled;
        objectives[1].range = 10.0f;
        objectives[1].timeLimit = 10.0f;
        objectives[1].objectivesToActivateOnSuccess = new Objective[] { objectives[3], objectives[4] };
        objectives[1].objectivesToUnHideOnSuccess = new Objective[] { objectives[3], objectives[4] };
        objectives[1].navPointIndex = 0;
        objectives[1].navPointIndexSetOnSuccess = 1;
        objectives[1].hidden = true;
        objectives[1].pay = ResourceManager.Instance.MissionDifficultyConfig.GetPay_Recon_SurveyPosition(difficultyTier);

        objectives[2].displayText = "Avoid detection by enemy forces";
        objectives[2].objectiveType = ObjectiveType.AvoidEnemyDetection;
        objectives[2].objectiveState = ObjectiveState.Active;
        objectives[2].optional = true;
        objectives[2].pay = ResourceManager.Instance.MissionDifficultyConfig.GetPay_Recon_AvoidDetection(difficultyTier);

        objectives[3].displayText = "Return to Nav Point " + StaticHelper.GetNavPointName(1) + " for extraction";
        objectives[3].objectiveType = ObjectiveType.MoveToNavPoint;
        objectives[3].objectiveState = ObjectiveState.Disabled;
        objectives[3].instantCompleteMission = true;
        objectives[3].objectivesToCompleteOnSuccess = new Objective[] { objectives[2] };
        objectives[3].objectivesToDisableOnSuccess = new Objective[] { objectives[4] };
        objectives[3].hidden = true;
        objectives[3].navPointIndex = 1;
        objectives[3].range = 5.0f;
        objectives[3].showDebriefing = false;

        objectives[4].displayText = "Break contact with enemy forces for 120 seconds";
        objectives[4].objectiveType = ObjectiveType.BreakContactWithEnemy;
        objectives[4].objectiveState = ObjectiveState.Disabled;
        objectives[4].instantCompleteMission = true;
        objectives[4].timeLimit = 120.0f;
        objectives[4].objectivesToCompleteOnSuccess = new Objective[] { objectives[2] };
        objectives[4].objectivesToDisableOnSuccess = new Objective[] { objectives[3] };
        objectives[4].hidden = true;
        objectives[4].showDebriefing = false;

        objectives[5].displayText = "Destroy all enemy units";
        objectives[5].objectiveType = ObjectiveType.DestroyAllUnits;
        objectives[5].objectiveState = ObjectiveState.Active;
        objectives[5].instantCompleteMission = true;
        objectives[5].hidden = true;
        objectives[5].units = enemyUnits.ToArray();
        objectives[5].objectivesToCompleteOnSuccess = new Objective[] { objectives[0], objectives[1], objectives[2] };
        objectives[5].objectivesToDisableOnSuccess = new Objective[] { objectives[3], objectives[4] };
        objectives[5].pay = ResourceManager.Instance.MissionDifficultyConfig.GetPay_Recon_DestroyAllEnemies(difficultyTier);
    }

    void BuildConvoyCaptureMission(int difficultyTier, FactionDefinition enemyFaction, FactionDefinition secondFaction)
    {
        List<UnitData> enemyUnits;

        List<UnitData> neutralUnits;

        objectives = Objective.CreateObjectiveArray(6);
    }

    void BuildConvoyEscortMission(int difficultyTier, FactionDefinition enemyFaction, FactionDefinition secondFaction)
    {
        List<UnitData> enemyUnits;

        List<UnitData> allyUnits;

        objectives = Objective.CreateObjectiveArray(6);
    }

    void BuildBaseDestroyMission(int difficultyTier, FactionDefinition enemyFaction)
    {
        List<UnitData> enemyUnits = ResourceManager.Instance.MissionDifficultyConfig.GetRandomForce(difficultyTier, MissionType.BaseDestroy, enemyFaction, false);

        baseFoundationPrefab = ResourceManager.Instance.MissionDifficultyConfig.GetRandomBaseFoundation(difficultyTier, enemyFaction);

        if (baseFoundationPrefab == null)
        {
            Debug.Log("Base foundation is null");
        }

        BaseFoundationController baseFoundationController = baseFoundationPrefab.GetComponent<BaseFoundationController>();

        UnitData primaryBuilding = enemyFaction.GetRandomPrimaryBuilding();

        UnitData turretTowerBuilding = enemyFaction.GetTurretControlTower();

        UnitData turretGeneratorBuilding = enemyFaction.GetTurretGenerator();

        List<UnitData> optionalBuildings = enemyFaction.GetRandomOptionalBuildings(baseFoundationController.OptionalBuildingSpots.Length);

        List<UnitData> allBuildings = new List<UnitData>();

        List<UnitData> turretUnits = ResourceManager.Instance.MissionDifficultyConfig.GetRandomTurrets(difficultyTier, enemyFaction);

        allBuildings.AddRange(optionalBuildings);
        allBuildings.Add(turretTowerBuilding);
        allBuildings.Add(turretGeneratorBuilding);

        objectives = Objective.CreateObjectiveArray(6);

        objectives[0].displayText = "Destroy " + (primaryBuilding as BuildingData).customName + " at Nav Point " + StaticHelper.GetNavPointName(0) + " or destroy all enemies";
        objectives[0].objectiveType = ObjectiveType.DestroyAllUnits;
        objectives[0].objectiveState = ObjectiveState.Active;
        objectives[0].units = new UnitData[] { primaryBuilding };
        objectives[0].objectivesToActivateOnSuccess = new Objective[] { objectives[3] };
        objectives[0].objectivesToUnHideOnSuccess = new Objective[] { objectives[3] };
        objectives[0].pay = ResourceManager.Instance.MissionDifficultyConfig.GetPay_BaseDestroy_PrimaryBuilding(difficultyTier);

        objectives[1].displayText = "Destroy all buildings at Nav Point " + StaticHelper.GetNavPointName(0) + " or destroy all enemies";
        objectives[1].objectiveType = ObjectiveType.DestroyAllUnits;
        objectives[1].objectiveState = ObjectiveState.Active;
        objectives[1].units = allBuildings.ToArray();
        objectives[1].optional = true;
        objectives[1].objectivesToActivateOnSuccess = new Objective[] { objectives[4] };
        objectives[1].objectivesToUnHideOnSuccess = new Objective[] { objectives[4] };
        objectives[1].pay = ResourceManager.Instance.MissionDifficultyConfig.GetPay_BaseDestroy_OptionalBuildings(difficultyTier);

        objectives[2].displayText = "Destroy or disable all turrets at Nav Point " + StaticHelper.GetNavPointName(0);
        objectives[2].objectiveType = ObjectiveType.DestroyAllUnits;
        objectives[2].objectiveState = ObjectiveState.Active;
        objectives[2].units = turretUnits.ToArray();
        objectives[2].optional = true;
        objectives[2].objectivesToActivateOnSuccess = new Objective[] { objectives[5] };
        objectives[2].pay = ResourceManager.Instance.MissionDifficultyConfig.GetPay_BaseDestroy_Turrets(difficultyTier);

        objectives[3].displayText = "Return to Nav Point " + StaticHelper.GetNavPointName(1) + " for extraction";
        objectives[3].objectiveType = ObjectiveType.MoveToNavPoint;
        objectives[3].objectiveState = ObjectiveState.Disabled;
        objectives[3].instantCompleteMission = true;
        objectives[3].objectivesToDisableOnSuccess = new Objective[] { objectives[4] };
        objectives[3].hidden = true;
        objectives[3].navPointIndex = 1;
        objectives[3].range = 5.0f;

        objectives[4].displayText = "Break contact with enemy forces for 120 seconds";
        objectives[4].objectiveType = ObjectiveType.BreakContactWithEnemy;
        objectives[4].objectiveState = ObjectiveState.Disabled;
        objectives[4].instantCompleteMission = true;
        objectives[4].timeLimit = 120.0f;
        objectives[4].objectivesToDisableOnSuccess = new Objective[] { objectives[3] };
        objectives[4].hidden = true;

        objectives[5].displayText = "Destroy all enemy units";
        objectives[5].objectiveType = ObjectiveType.DestroyAllUnits;
        objectives[5].objectiveState = ObjectiveState.Disabled;
        objectives[5].instantCompleteMission = true;
        objectives[5].hidden = true;
        objectives[5].units = enemyUnits.ToArray();
        objectives[5].objectivesToDisableOnSuccess = new Objective[] { objectives[3], objectives[4] };
        objectives[5].pay = ResourceManager.Instance.MissionDifficultyConfig.GetPay_BaseDestroy_DestroyAllEnemies(difficultyTier);
    }

    void BuildBaseCaptureMission(int difficultyTier, FactionDefinition enemyFaction)
    {
        List<UnitData> enemyUnits = ResourceManager.Instance.MissionDifficultyConfig.GetRandomForce(difficultyTier, MissionType.BaseCapture, enemyFaction, false);

        baseFoundationPrefab = ResourceManager.Instance.MissionDifficultyConfig.GetRandomBaseFoundation(difficultyTier, enemyFaction);

        if (baseFoundationPrefab == null)
        {
            Debug.Log("Base foundation is null");
        }

        BaseFoundationController baseFoundationController = baseFoundationPrefab.GetComponent<BaseFoundationController>();

        UnitData primaryBuilding = enemyFaction.GetRandomPrimaryBuilding();

        UnitData turretTowerBuilding = enemyFaction.GetTurretControlTower();

        UnitData turretGeneratorBuilding = enemyFaction.GetTurretGenerator();

        List<UnitData> optionalBuildings = enemyFaction.GetRandomOptionalBuildings(baseFoundationController.OptionalBuildingSpots.Length);

        List<UnitData> turretUnits = ResourceManager.Instance.MissionDifficultyConfig.GetRandomTurrets(difficultyTier, enemyFaction);

        enemyUnits.AddRange(turretUnits);

        optionalBuildings.Add(turretTowerBuilding);
        optionalBuildings.Add(turretGeneratorBuilding);

        objectives = Objective.CreateObjectiveArray(3);

        objectives[0].displayText = "Destroy all enemy units";
        objectives[0].objectiveType = ObjectiveType.DestroyAllUnits;
        objectives[0].objectiveState = ObjectiveState.Active;
        objectives[0].instantCompleteMission = true;
        objectives[0].units = enemyUnits.ToArray();
        objectives[0].pay = ResourceManager.Instance.MissionDifficultyConfig.GetPay_BaseCapture_DestroyAllEnemies(difficultyTier);

        objectives[1].displayText = "Protect " + primaryBuilding.customName + " at Nav Point " + StaticHelper.GetNavPointName(0);
        objectives[1].objectiveType = ObjectiveType.ProtectAllUnits;
        objectives[1].objectiveState = ObjectiveState.Active;
        objectives[1].instantFailMission = true;
        objectives[1].units = new UnitData[] { primaryBuilding };
        objectives[1].pay = ResourceManager.Instance.MissionDifficultyConfig.GetPay_BaseCapture_ProtectPrimaryBuilding(difficultyTier);

        objectives[2].displayText = "Protect all buildings at " + StaticHelper.GetNavPointName(0);
        objectives[2].objectiveType = ObjectiveType.ProtectAllUnits;
        objectives[2].objectiveState = ObjectiveState.Active;
        objectives[2].units = optionalBuildings.ToArray();
        objectives[2].optional = true;
        objectives[2].pay = ResourceManager.Instance.MissionDifficultyConfig.GetPay_BaseCapture_ProtectAllBuildings(difficultyTier);
    }

    void BuildBaseDefendMission(int difficultyTier, FactionDefinition enemyFaction, FactionDefinition secondFaction)
    {
        List<UnitData> enemyUnits = ResourceManager.Instance.MissionDifficultyConfig.GetRandomForce(difficultyTier, MissionType.BaseDefend, enemyFaction, false); ;

        baseFoundationPrefab = ResourceManager.Instance.MissionDifficultyConfig.GetRandomBaseFoundation(difficultyTier, secondFaction);

        if (baseFoundationPrefab == null)
        {
            Debug.Log("Base foundation is null");
        }

        BaseFoundationController baseFoundationController = baseFoundationPrefab.GetComponent<BaseFoundationController>();

        UnitData primaryBuilding = secondFaction.GetRandomPrimaryBuilding();

        UnitData turretTowerBuilding = secondFaction.GetTurretControlTower();

        UnitData turretGeneratorBuilding = secondFaction.GetTurretGenerator();

        List<UnitData> optionalBuildings = secondFaction.GetRandomOptionalBuildings(baseFoundationController.OptionalBuildingSpots.Length);

        List<UnitData> turretUnits = ResourceManager.Instance.MissionDifficultyConfig.GetRandomTurrets(difficultyTier, secondFaction);

        optionalBuildings.Add(turretTowerBuilding);
        optionalBuildings.Add(turretGeneratorBuilding);

        objectives = Objective.CreateObjectiveArray(4);

        objectives[0].displayText = "Destroy all enemy units";
        objectives[0].objectiveType = ObjectiveType.DestroyAllUnits;
        objectives[0].objectiveState = ObjectiveState.Active;
        objectives[0].instantCompleteMission = true;
        objectives[0].units = enemyUnits.ToArray();
        objectives[0].pay = ResourceManager.Instance.MissionDifficultyConfig.GetPay_BaseDefend_DestroyAllEnemies(difficultyTier);

        objectives[1].displayText = "Protect " + primaryBuilding.customName + " at Nav Point " + StaticHelper.GetNavPointName(0);
        objectives[1].objectiveType = ObjectiveType.ProtectAllUnits;
        objectives[1].objectiveState = ObjectiveState.Active;
        objectives[1].instantFailMission = true;
        objectives[1].units = new UnitData[] { primaryBuilding };
        objectives[1].pay = ResourceManager.Instance.MissionDifficultyConfig.GetPay_BaseDefend_ProtectPrimaryBuilding(difficultyTier);

        objectives[2].displayText = "Protect all buildings at Nav Point " + StaticHelper.GetNavPointName(0);
        objectives[2].objectiveType = ObjectiveType.ProtectAllUnits;
        objectives[2].objectiveState = ObjectiveState.Active;
        objectives[2].units = optionalBuildings.ToArray();
        objectives[2].optional = true;
        objectives[2].pay = ResourceManager.Instance.MissionDifficultyConfig.GetPay_BaseDefend_ProtectOptionalBuildings(difficultyTier);

        objectives[3].displayText = "Protect all turrets at Nav Point " + StaticHelper.GetNavPointName(0);
        objectives[3].objectiveType = ObjectiveType.ProtectAllUnits;
        objectives[3].objectiveState = ObjectiveState.Active;
        objectives[3].units = turretUnits.ToArray();
        objectives[3].optional = true;
        objectives[3].pay = ResourceManager.Instance.MissionDifficultyConfig.GetPay_BaseDefend_ProtectTurrets(difficultyTier);
    }

    public List<UnitData> GetDestroyedEnemies()
    {
        List<UnitData> enemyUnits = new List<UnitData>();

        for (int objectiveIndex = 0; objectiveIndex < objectives.Length; objectiveIndex++)
        {
            Objective objective = objectives[objectiveIndex];

            for (int unitIndex = 0; unitIndex < objective.units.Length; unitIndex++)
            {
                UnitData unitData = objective.units[unitIndex];

                if (unitData.isDestroyed && unitData.teamType == TeamType.Enemy && !enemyUnits.Contains(unitData))
                {
                    enemyUnits.Add(unitData);
                }
            }
        }

        return enemyUnits;
    }
}