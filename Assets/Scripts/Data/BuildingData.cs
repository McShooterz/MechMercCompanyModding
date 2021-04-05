using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BuildingData : UnitData
{
    [Header("Building Data")]

    public BuildingController buildingController;

    public BuildingDefinition buildingDefinition;

    public float health = 0.0f;

    public float healthMax = 0.0f;

    public float HealthPercentage { get => health / healthMax; }

    public static BuildingData CreateBuildingData(BuildingSpawnDefinition buildingSpawnDefinition)
    {
        BuildingDefinition buildingDefinition = buildingSpawnDefinition.GetBuildingDefinition();

        if (buildingDefinition != null)
        {
            BuildingData buildingData = new BuildingData();
            buildingData.BuildFromBuildingDefinition(buildingDefinition);
            return buildingData;
        }
        else
        {
            Debug.LogError("Error: Invalid building definition");
        }

        return null;
    }

    public void BuildFromBuildingDefinition(BuildingDefinition definition)
    {
        buildingDefinition = definition;
        customName = buildingDefinition.GetDisplayName();

        health = buildingDefinition.Health;
        healthMax = health;
    }

    public void TakeDamage(float damage)
    {
        if (damage >= health)
        {
            health = 0;
            Die();
        }
        else
        {
            health -= damage;
        }
    }

    protected override void Die()
    {
        buildingController.CreateDeathExplosion();
        buildingController.Die();
    }
}
