using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class VehicleData : CombatUnitData
{
    protected float gunnerySkill = 0.0f;

    public float GunnerySkill { get => gunnerySkill; }

    public void SetGunnerySkill(BasicPilotSkillLevel basicPilotSkillLevel)
    {
        switch (basicPilotSkillLevel)
        {
            case BasicPilotSkillLevel.Rookie:
                {
                    gunnerySkill = 0.2f;
                    break;
                }
            case BasicPilotSkillLevel.Regular:
                {
                    gunnerySkill = 0.4f;
                    break;
                }
            case BasicPilotSkillLevel.Veteran:
                {
                    gunnerySkill = 0.6f;
                    break;
                }
            case BasicPilotSkillLevel.Elite:
                {
                    gunnerySkill = 0.8f;
                    break;
                }
            case BasicPilotSkillLevel.Legendary:
                {
                    gunnerySkill = 1.0f;
                    break;
                }
            default:
                {
                    gunnerySkill = 0.0f;
                    break;
                }
        }
    }
}
