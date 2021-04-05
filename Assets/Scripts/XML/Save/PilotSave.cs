using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PilotSave
{
    public System.Guid Guid = System.Guid.Empty;

    public string DisplayName = "";

    public float GunnerySkill;

    public int ContractValue;

    public int ContractWeeklySalary;

    public int ContractMissionPay;

    public int ContractDeathPayout;

    public GameDate ContractEndDate = new GameDate();

    public string PilotVoiceProfile = "";

    public string PilotIcon = "";

    public PilotStatusType PilotStatus = PilotStatusType.Ready;

    public float MissionExperience = 0.0f;

    public int MechKills = 0;

    public int VehicleKills = 0;

    public int MissionMechKills = 0;

    public int MissionVehicleKills = 0;

    public PilotVoiceProfileDefinition GetPilotVoiceProfile()
    {
        return ResourceManager.Instance.GetPilotVoiceProfile(PilotVoiceProfile);
    }

    public Sprite GetIcon()
    {
        Texture2D texture2D = ResourceManager.Instance.GetPilotTexture2D(PilotIcon);

        if (texture2D != null)
        {
            return StaticHelper.GetSpriteUI(texture2D);
        }

        return null;
    }
}
