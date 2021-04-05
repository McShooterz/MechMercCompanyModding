using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretUnitSetupController : UnitSetupControllerBase
{
    [SerializeField]
    string turretDefinitionKey = "";

    [SerializeField]
    AI_Type aI_Type = AI_Type.None;

    [SerializeField]
    GroupIntelSetup groupIntelSetup;

    public override void BuildUnit()
    {
        TurretDefinition turretDefinition = ResourceManager.Instance.GetTurretDefinition(turretDefinitionKey);

        if (turretDefinition != null)
        {
            TurretUnitData turretUnitData = new TurretUnitData();

            turretUnitData.BuildFromDefinition(turretDefinition);

            if (displayName != "")
            {
                turretUnitData.customName = displayName;
            }

            TurretUnitController turretUnitController = gameObject.AddComponent<TurretUnitController>();

            turretUnitController.SetTurretData(turretUnitData);

            turretUnitController.SetAI(aI_Type, team);

            if (groupIntelSetup != null)
            {
                turretUnitController.SetGroupIntel(groupIntelSetup.groupIntel);
            }
            else
            {
                turretUnitController.SetGroupIntel(new GroupIntel());
            }

            MissionManager.Instance.AddUnit(turretUnitController);
        }

        Destroy(this);
    }
}
