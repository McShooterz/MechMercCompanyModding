using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class MechSpawnDefinition : SpawnDefinition
{
    public string MechChassis = "";

    public string MechDesign = "";

    public string[] MechPaintSchemes = new string[0];

    public MechDesign GetDesign()
    {
        return ResourceManager.Instance.GetMechDesign(MechChassis, MechDesign);
    }

    public MechPaintScheme GetMechPaintScheme()
    {
        if (MechPaintSchemes.Length > 0)
        {
            MechPaintScheme mechPaintScheme = ResourceManager.Instance.GetMechPaintScheme(MechChassis, MechPaintSchemes[Random.Range(0, MechPaintSchemes.Length)]);

            if (mechPaintScheme != null)
            {
                return mechPaintScheme;
            }
        }

        Debug.Log("Mech Paint Scheme not found");

        return new MechPaintScheme();
    }
}
