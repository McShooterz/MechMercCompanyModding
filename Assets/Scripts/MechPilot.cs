using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MechPilot
{
    public System.Guid guid = System.Guid.Empty;

    public string displayName = "";

    public float gunnerySkill;

    public int contractValue;

    public int contractWeeklySalary;

    public int contractMissionPay;

    public int contractDeathPayout;

    public GameDate contractEndDate = new GameDate();

    public float missionExperience = 0.0f;

    public int mechKills = 0;

    public int vehicleKills = 0;

    public int missionMechKills = 0;

    public int missionVehicleKills = 0;

    public PilotVoiceProfileDefinition PilotVoiceProfile { get; set; }

    public Sprite Icon { get; set; }

    public PilotStatusType PilotStatus { get; set; } = PilotStatusType.Ready;

    public string GunnerySkillDisplay
    {
        get
        {
            string value = ((int)(gunnerySkill * 100)).ToString();

            if (gunnerySkill < 0.4f)
            {
                return value + " Rookie";
            }
            else if (gunnerySkill < 0.6f)
            {
                return value + " Regular";
            }
            else if (gunnerySkill < 0.8f)
            {
                return value + " Veteran";
            }
            else if (gunnerySkill < 0.95f)
            {
                return value + " Elite";
            }

            return value + " Legendary";
        }
    }

    public string PilotStatusDisplay
    {
        get
        {
            switch (PilotStatus)
            {
                case PilotStatusType.Ready:
                    {
                        return "Ready";
                    }
                case PilotStatusType.Injured:
                    {
                        return "Injured";
                    }
                case PilotStatusType.Deceased:
                    {
                        return "Deceased";
                    }
                default:
                    {
                        return "Ready";
                    }
            }
        }
    }

    public string CareerDisplay
    {
        get
        {
            System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();

            stringBuilder.AppendLine("Callsign: " + displayName);

            stringBuilder.AppendLine("Status: " + PilotStatusDisplay);

            stringBuilder.AppendLine("Gunnery Skill: " + GunnerySkillDisplay);

            stringBuilder.AppendLine("Weekly Salary: " + StaticHelper.FormatMoney(contractWeeklySalary));

            stringBuilder.AppendLine("Mission Pay: " + StaticHelper.FormatMoney(contractMissionPay));

            stringBuilder.AppendLine("KIA Payout: " + StaticHelper.FormatMoney(contractDeathPayout));

            int totalKills = TotalKills;

            if (totalKills > 0)
            {
                stringBuilder.AppendLine("Total Confirmed Kills: " + totalKills);

                if (mechKills > 0)
                    stringBuilder.AppendLine("Mech Kills: " + mechKills);

                if (vehicleKills > 0)
                    stringBuilder.AppendLine("Vehicle Kills: " + vehicleKills);
            }

            if (contractEndDate != null && contractEndDate.Year != 1)
            {
                stringBuilder.AppendLine();
                stringBuilder.AppendLine("Contract End: " + contractEndDate.Display);
            }

            return stringBuilder.ToString();
        }
    }

    public string HoveredDisplay
    {
        get
        {
            System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();

            stringBuilder.AppendLine(displayName);

            stringBuilder.AppendLine("Gunnery Skill: " + GunnerySkillDisplay);

            stringBuilder.AppendLine("Weekly Salary: " + StaticHelper.FormatMoney(contractWeeklySalary));

            stringBuilder.AppendLine("Mission Pay: " + StaticHelper.FormatMoney(contractMissionPay));

            stringBuilder.AppendLine("KIA Payout: " + StaticHelper.FormatMoney(contractDeathPayout));

            return stringBuilder.ToString();
        }
    }

    public int TotalKills { get => mechKills + vehicleKills; }

    public int MissionKills { get => missionMechKills + missionVehicleKills; }

    public PilotSave PilotSave
    {
        get
        {
            string voiceProfileKey = "";
            string iconKey = "";
            
            if (PilotVoiceProfile != null)
            {
                voiceProfileKey = PilotVoiceProfile.Key;
            }

            if (Icon != null)
            {
                iconKey = Icon.texture.name;
            }

            return new PilotSave()
            {
                Guid = guid,

                DisplayName = displayName,

                GunnerySkill = gunnerySkill,

                ContractValue = contractValue,

                ContractWeeklySalary = contractWeeklySalary,

                ContractMissionPay = contractMissionPay,

                ContractDeathPayout = contractDeathPayout,

                ContractEndDate = new GameDate(contractEndDate),

                PilotVoiceProfile = voiceProfileKey,

                PilotIcon = iconKey,

                PilotStatus = PilotStatus,

                MissionExperience = missionExperience,

                MechKills = mechKills,

                VehicleKills = vehicleKills,

                MissionMechKills = missionMechKills,

                MissionVehicleKills = missionVehicleKills,
            };
        }
    }

    public MechPilot(string name)
    {
        displayName = name;
        PilotVoiceProfile = ResourceManager.Instance.GetPilotVoiceProfile("Default");
    }

    public MechPilot(PilotDefinition pilotDefinition)
    {
        displayName = pilotDefinition.GetDisplayName();

        PilotVoiceProfile = pilotDefinition.GetPilotVoiceProfile();

        gunnerySkill = pilotDefinition.GunnerySkill / 100f;
    }

    public MechPilot(PilotSave pilotSave)
    {
        guid = pilotSave.Guid;

        displayName = pilotSave.DisplayName;

        gunnerySkill = pilotSave.GunnerySkill;

        contractValue = pilotSave.ContractValue;

        contractWeeklySalary = pilotSave.ContractWeeklySalary;

        contractMissionPay = pilotSave.ContractMissionPay;

        contractDeathPayout = pilotSave.ContractDeathPayout;

        contractEndDate = new GameDate(pilotSave.ContractEndDate);

        PilotVoiceProfile = pilotSave.GetPilotVoiceProfile();

        Icon = pilotSave.GetIcon();

        PilotStatus = pilotSave.PilotStatus;

        missionExperience = pilotSave.MissionExperience;

        mechKills = pilotSave.MechKills;

        vehicleKills = pilotSave.VehicleKills;

        missionMechKills = pilotSave.MissionMechKills;

        missionVehicleKills = pilotSave.MissionVehicleKills;
    }

    public void SetBasicSkill(BasicPilotSkillLevel basicPilotSkillLevel)
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
                    gunnerySkill = 0.4f;
                    break;
                }
        }
    }

    public void GenerateContractValues()
    {
        float baseValue = gunnerySkill * 100;

        baseValue *= baseValue;

        contractValue = (int)(baseValue * Random.Range(23.0f, 25.0f));

        contractWeeklySalary = (int)(baseValue * Random.Range(0.9f, 1.1f));

        contractMissionPay = (int)(baseValue * Random.Range(4.5f, 5.5f));

        contractDeathPayout = (int)(baseValue * Random.Range(90.0f, 125.0f));
    }

    public static List<MechPilot> GetRandomMechPilots(int count)
    {
        List<MechPilot> mechPilots = new List<MechPilot>();
        MechPilot mechPilot;
        string callsign;
        PilotVoiceProfileDefinition pilotVoiceProfile;
        Sprite pilotIconSprite;

        for (int i = 0; i < count; i++)
        {
            callsign = ResourceManager.Instance.GetRandomCallsign();

            if (callsign != "")
            {
                mechPilot = new MechPilot(callsign);

                mechPilot.gunnerySkill = Random.Range(0.2f, 0.6f);

                if (Random.Range(0, 2) == 0)
                {
                    pilotVoiceProfile = ResourceManager.Instance.GetRandomVoiceProfileMale();
                    pilotIconSprite = ResourceManager.Instance.GetRandomPilotMaleSprite();
                }
                else
                {
                    pilotVoiceProfile = ResourceManager.Instance.GetRandomVoiceProfileFemale();
                    pilotIconSprite = ResourceManager.Instance.GetRandomPilotFemaleSprite();
                }

                if (pilotVoiceProfile == null)
                {
                    mechPilot.PilotVoiceProfile = ResourceManager.Instance.GetPilotVoiceProfile("Default");
                }
                else
                {
                    mechPilot.PilotVoiceProfile = pilotVoiceProfile;
                }

                if (pilotIconSprite == null)
                {
                    mechPilot.Icon = ResourceManager.Instance.GetRandomPilotGenericSprite();
                }
                else
                {
                    mechPilot.Icon = pilotIconSprite;
                }

                mechPilots.Add(mechPilot);
            }
        }

        return mechPilots;
    }

    public void ApplyMissionKills()
    {
        gunnerySkill += missionExperience;
        mechKills += missionMechKills;
        vehicleKills += missionVehicleKills;

        missionExperience = 0;
        missionMechKills = 0;
        missionVehicleKills = 0;
    }
}
