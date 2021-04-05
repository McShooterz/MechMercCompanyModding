using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MechSetupController : UnitSetupControllerBase
{
    [SerializeField]
    string mechChassisKey = "";

    [SerializeField]
    string mechDesignKey = "";

    [SerializeField]
    float pilotGunnerySkill = 0.2f;

    [SerializeField]
    string mechPaintSchemeKey = "";

    [SerializeField]
    AI_Type aI_Type = AI_Type.None;

    [SerializeField]
    GroupIntelSetup groupIntelSetup;

    public override void BuildUnit()
    {
        MechDesign mechDesign = ResourceManager.Instance.GetMechDesign(mechChassisKey, mechDesignKey);

        if (mechDesign != null)
        {
            MechData mechData = new MechData(mechDesign);

            MechPilot mechPilot;

            if (displayName != "")
            {
                mechPilot = new MechPilot(displayName);
            }
            else
            {
                mechPilot = new MechPilot(mechDesign.DesignName);
            }

            mechPilot.gunnerySkill = pilotGunnerySkill;

            mechData.currentMechPilot = mechPilot;

            MechControllerGeneralAI mechController = gameObject.AddComponent<MechControllerGeneralAI>();

            mechController.SetMechData(mechData);

            mechController.SetAI(aI_Type, team);

            if (groupIntelSetup != null)
            {
                mechController.SetGroupIntel(groupIntelSetup.groupIntel);
            }
            else
            {
                mechController.SetGroupIntel(new GroupIntel());
            }

            MechPaintScheme mechPaintScheme = ResourceManager.Instance.GetMechPaintScheme(mechChassisKey, mechPaintSchemeKey);

            if (mechPaintScheme != null)
            {
                MechMetaController mechMetaController = gameObject.GetComponent<MechMetaController>();

                mechMetaController.ApplyMechPaintScheme(mechPaintScheme);
            }

            MissionManager.Instance.AddUnit(mechController);
        }

        Destroy(this);
    }
}
