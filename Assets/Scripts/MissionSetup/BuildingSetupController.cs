using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingSetupController : UnitSetupControllerBase
{
    [SerializeField]
    string buildingDefinitionKey = "";

    [SerializeField]
    GameObject[] turretsToDisableOnDie = new GameObject[0];

    public override void BuildUnit()
    {
        BuildingDefinition buildingDefinition = ResourceManager.Instance.GetBuildingDefinition(buildingDefinitionKey);

        if (buildingDefinition != null)
        {
            BuildingData buildingData = new BuildingData();

            buildingData.BuildFromBuildingDefinition(buildingDefinition);

            if (displayName != "")
            {
                buildingData.customName = displayName;
            }

            BuildingController buildingController = gameObject.AddComponent<BuildingController>();

            buildingController.SetData(buildingData);

            buildingController.SetTeam(team);

            MissionManager.Instance.AddUnit(buildingController);

            if (turretsToDisableOnDie.Length > 0)
            {
                StartCoroutine(AddTurretsToDisableOnDie());
            }
            else
            {
                Destroy(this);
            }
        }
    }

    protected IEnumerator AddTurretsToDisableOnDie()
    {
        yield return null;

        BuildingController buildingController = gameObject.GetComponent<BuildingController>();

        if (buildingController != null)
        {
            List<TurretUnitController> turretUnitControllers = new List<TurretUnitController>();

            foreach (GameObject turretObject in turretsToDisableOnDie)
            {
                TurretUnitController turretUnitController = turretObject.GetComponent<TurretUnitController>();

                if (turretUnitController != null)
                {
                    turretUnitControllers.Add(turretUnitController);
                }
            }

            buildingController.SetTurretsToDisableOnDie(turretUnitControllers.ToArray());
        }

        Destroy(this);
    }
}