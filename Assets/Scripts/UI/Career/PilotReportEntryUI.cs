using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PilotReportEntryUI : MonoBehaviour
{
    [SerializeField]
    Image pilotImage;

    [SerializeField]
    Text pilotNameText;

    [SerializeField]
    Text pilotStatusText;

    [SerializeField]
    Text pilotGunnerySkillText;

    [SerializeField]
    Text pilotTotalKillsText;

    [SerializeField]
    Text pilotMechKillsText;

    [SerializeField]
    Text pilotGroundVehicleKillsText;

    public void SetPlayer(Career career)
    {
        Sprite playerPilotSprite = career.PlayerPilotSprite;

        if (playerPilotSprite != null)
        {
            pilotImage.sprite = playerPilotSprite;
        }
        else
        {
            pilotImage.enabled = false;
        }

        pilotNameText.text = career.callsign;

        pilotStatusText.text = "";
        pilotGunnerySkillText.text = "";

        int totalKills = career.playerMechKills + career.playerVehicleKills + career.PlayerMissionKills;
        int mechKills = career.playerMechKills + career.playerMissionMechKills;
        int vehicleKills = career.playerVehicleKills + career.playerMissionVehicleKills;

        pilotTotalKillsText.text = totalKills.ToString() + " (+" + career.PlayerMissionKills + ")";

        pilotMechKillsText.text = mechKills.ToString() + " (+" + career.playerMissionMechKills + ")";

        pilotGroundVehicleKillsText.text = vehicleKills.ToString() + " (+" + career.playerMissionVehicleKills + ")";
    }

    public void SetPilot(MechPilot mechPilot)
    {
        Sprite pilotSprite = mechPilot.Icon;

        if (pilotSprite != null)
        {
            pilotImage.sprite = pilotSprite;
        }
        else
        {
            pilotImage.enabled = false;
        }

        pilotNameText.text = mechPilot.displayName;

        pilotStatusText.text = mechPilot.PilotStatusDisplay;

        float gunnerySkillTotal = mechPilot.gunnerySkill + mechPilot.missionExperience;

        pilotGunnerySkillText.text = (gunnerySkillTotal * 100).ToString("0.###") + " (+" + (mechPilot.missionExperience * 100).ToString("0.###") + ")";

        int totalKills = mechPilot.mechKills + mechPilot.vehicleKills + mechPilot.MissionKills;
        int mechKills = mechPilot.mechKills + mechPilot.missionMechKills;
        int vehicleKills = mechPilot.vehicleKills + mechPilot.missionVehicleKills;

        pilotTotalKillsText.text = totalKills.ToString() + " (+" + mechPilot.MissionKills + ")";

        pilotMechKillsText.text = mechKills.ToString() + " (+" + mechPilot.missionMechKills + ")";

        pilotGroundVehicleKillsText.text = vehicleKills.ToString() + " (+" + mechPilot.missionVehicleKills + ")";
    }
}
