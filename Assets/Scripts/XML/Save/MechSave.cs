using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MechSave
{
    public System.Guid Guid = System.Guid.Empty;

    public string MechChassis = "";

    public string CustomName = "";

    public string DesignName = "";

    public MechPaintScheme MechPaintScheme;

    public ArmorType ArmorTypeHead;
    public ArmorType ArmorTypeTorsoCenter;
    public ArmorType ArmorTypeTorsoLeft;
    public ArmorType ArmorTypeTorsoRight;
    public ArmorType ArmorTypeArmLeft;
    public ArmorType ArmorTypeArmRight;
    public ArmorType ArmorTypeLegLeft;
    public ArmorType ArmorTypeLegRight;

    public int ArmorMaxHead;
    public int ArmorMaxTorsoCenter;
    public int ArmorMaxTorsoCenterRear;
    public int ArmorMaxTorsoLeft;
    public int ArmorMaxTorsoLeftRear;
    public int ArmorMaxTorsoRight;
    public int ArmorMaxTorsoRightRear;
    public int ArmorMaxLegLeft;
    public int ArmorMaxLegRight;
    public int ArmorMaxArmLeft;
    public int ArmorMaxArmRight;

    public float InternalHead;
    public float InternalTorsoCenter;
    public float InternalTorsoLeft;
    public float InternalTorsoRight;
    public float InternalArmLeft;
    public float InternalArmRight;
    public float InternalLegLeft;
    public float InternalLegRight;

    public ComponentSave[] ComponentsHead = new ComponentSave[0];
    public ComponentSave[] ComponentsTorsoCenter = new ComponentSave[0];
    public ComponentSave[] ComponentsTorsoLeft = new ComponentSave[0];
    public ComponentSave[] ComponentsTorsoRight = new ComponentSave[0];
    public ComponentSave[] ComponentsArmLeft = new ComponentSave[0];
    public ComponentSave[] ComponentsArmRight = new ComponentSave[0];
    public ComponentSave[] ComponentsLegLeft = new ComponentSave[0];
    public ComponentSave[] ComponentsLegRight = new ComponentSave[0];

    public MechChassisDefinition GetMechChassisDefinition()
    {
        return ResourceManager.Instance.GetMechChassisDefinition(MechChassis);
    }
}
