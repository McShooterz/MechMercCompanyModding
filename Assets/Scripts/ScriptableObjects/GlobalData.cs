using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(menuName = "ScriptableObjects/GlobalData")]
public class GlobalData : SingletonScriptableObject<GlobalData>
{
    [SerializeField]
    MissionSetup missionSetup;

    [SerializeField]
    InstantActionGlobalData instantActionGlobalData;

    MissionData activeMissionData;

    public MechData PlayerMechMission { get; private set; }

    public MechData[] SquadMechsMission { get; private set; } = new MechData[11];

    public MechPilot[] SquadPilotsMission { get; private set; } = new MechPilot[11];

    public MechSave PlayerMechSetup { get; set; }

    public MechSave[] SquadMechsSetup { get; private set; } = new MechSave[11];

    public PilotSave[] SquadPilotsSetup { get; private set; } = new PilotSave[11];

    public MissionSetup MissionSetup { get => missionSetup; }

    public MissionData ActiveMissionData { get => activeMissionData; set => activeMissionData = value; }

    public InstantActionGlobalData InstantActionGlobalData { get => instantActionGlobalData; }

    void OnEnable()
    {
        Initialize();
    }

    public MechData GetPlayerMechMission()
    {
        PlayerMechMission = new MechData(PlayerMechSetup);

        PlayerMechMission.currentMechPilot = null;

        return PlayerMechMission;
    }

    public MechData[] GetSquadMateMechsMission()
    {
        for (int i = 0; i < 11; i++)
        {
            MechSave mechSetup = SquadMechsSetup[i];
            PilotSave pilotSetup = SquadPilotsSetup[i];

            if (mechSetup != null && pilotSetup != null)
            {
                SquadMechsMission[i] = new MechData(mechSetup);
                SquadPilotsMission[i] = new MechPilot(pilotSetup);
                SquadMechsMission[i].currentMechPilot = SquadPilotsMission[i];
            }
            else
            {
                SquadMechsMission[i] = null;
                SquadPilotsMission[i] = null;
            }
        }

        return SquadMechsMission;
    }

    public void SetPlayerMechSetup(MechData mechData)
    {
        if (mechData != null)
        {
            PlayerMechSetup = mechData.MechSave;
        }
        else
        {
            PlayerMechSetup = null;
        }
    }

    public void SetSquadMatesSetup(MechData[] mechDatas)
    {
        for (int i = 0; i < mechDatas.Length; i++)
        {
            MechData mechData = mechDatas[i];

            if (mechData != null)
            {
                SquadMechsSetup[i] = mechData.MechSave;
            }
            else
            {
                SquadMechsSetup[i] = null;
            }
        }
    }

    public void SetSquadPilotsSetup(MechPilot[] mechPilots)
    {
        for (int i = 0; i < 11; i++)
        {
            MechPilot mechPilot = mechPilots[i];

            if (mechPilot != null)
            {
                SquadPilotsSetup[i] = mechPilot.PilotSave;
            }
            else
            {
                SquadPilotsSetup[i] = null;
            }
        }
    }
}