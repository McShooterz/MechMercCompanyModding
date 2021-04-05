using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundVehicleSetupController : UnitSetupControllerBase
{
    [SerializeField]
    string groundVehicleDefinitionKey = "";

    [SerializeField]
    AI_Type aI_Type = AI_Type.None;

    [SerializeField]
    GroupIntelSetup groupIntelSetup;

    public override void BuildUnit()
    {
        GroundVehicleDefinition groundVehicleDefinition = ResourceManager.Instance.GetGroundVehicleDefinition(groundVehicleDefinitionKey);

        if (groundVehicleDefinition != null)
        {
            GroundVehicleData groundVehicleData = new GroundVehicleData();

            groundVehicleData.BuildFromDefinition(groundVehicleDefinition);

            if (displayName != "")
            {
                groundVehicleData.customName = displayName;
            }

            GroundVehicleController groundVehicleController = gameObject.AddComponent<GroundVehicleController>();

            groundVehicleController.SetGroundVehicleData(groundVehicleData);

            groundVehicleController.SetAI(aI_Type, team);

            if (groupIntelSetup != null)
            {
                groundVehicleController.SetGroupIntel(groupIntelSetup.groupIntel);
            }
            else
            {
                groundVehicleController.SetGroupIntel(new GroupIntel());
            }

            MissionManager.Instance.AddUnit(groundVehicleController);
        }

        Destroy(this);
    }
}
