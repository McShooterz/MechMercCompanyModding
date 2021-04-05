using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MissionManager))]
public class MissionManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        MissionManager missionManager = (MissionManager)target;

        KeyValuePair<string, Color> playerSpawnPointsValidity = missionManager.GetValidPlayerSpawnPoints();
        KeyValuePair<string, Color> landingZoneNavPointValidity = missionManager.GetValidLandingZoneNavPoint();
        KeyValuePair<string, Color> missionValidBattle = missionManager.GetValidMissionBattle();
        KeyValuePair<string, Color> missionValidSearchAndDestroy = missionManager.GetValidMissionSearchAndDestroy();
        KeyValuePair<string, Color> missionValidAssassination = missionManager.GetValidMissionAssassination();
        KeyValuePair<string, Color> missionValidConvoyDestroy = missionManager.GetValidMissionConvoyDestroy();
        KeyValuePair<string, Color> missionValidBattleSupport = missionManager.GetValidMissionBattleSupport();
        KeyValuePair<string, Color> missionValidRecon = missionManager.GetValidMissionRecon();
        KeyValuePair<string, Color> missionValidBaseDestroy = missionManager.GetValidMissionBaseDestroy();
        KeyValuePair<string, Color> missionValidBaseCapture = missionManager.GetValidMissionBaseCapture();
        KeyValuePair<string, Color> missionValidBaseDefend = missionManager.GetValidMissionBaseDefend();

        GUI.color = playerSpawnPointsValidity.Value;
        GUILayout.TextArea("Player Spawns: " + playerSpawnPointsValidity.Key);

        GUI.color = landingZoneNavPointValidity.Value;
        GUILayout.TextArea("Landing Zone Nav Point: " + landingZoneNavPointValidity.Key);

        GUI.color = missionValidBattle.Value;
        GUILayout.TextArea("Battle Mission: " + missionValidBattle.Key);

        GUI.color = missionValidSearchAndDestroy.Value;
        GUILayout.TextArea("Search and Destroy Mission: " + missionValidSearchAndDestroy.Key);

        GUI.color = missionValidAssassination.Value;
        GUILayout.TextArea("Assassination Mission: " + missionValidAssassination.Key);

        GUI.color = missionValidConvoyDestroy.Value;
        GUILayout.TextArea("Convoy Destroy Mission: " + missionValidConvoyDestroy.Key);

        GUI.color = missionValidBattleSupport.Value;
        GUILayout.TextArea("Battle Support Mission: " + missionValidBattleSupport.Key);

        GUI.color = missionValidRecon.Value;
        GUILayout.TextArea("Recon Mission: " + missionValidRecon.Key);

        GUI.color = missionValidBaseDestroy.Value;
        GUILayout.TextArea("Base Destroy Mission: " + missionValidBaseDestroy.Key);

        GUI.color = missionValidBaseCapture.Value;
        GUILayout.TextArea("Base Capture Mission: " + missionValidBaseCapture.Key);

        GUI.color = missionValidBaseDefend.Value;
        GUILayout.TextArea("Base Defend Mission: " + missionValidBaseDefend.Key);

        GUI.color = Color.white;

        if (GUILayout.Button("Auto Size Border Objects"))
        {
            missionManager.AutoSizeBorderObjects();
        }

        if (GUILayout.Button("Create Landing Zone Nav Point"))
        {
            missionManager.CreateLandingZoneNavPoint();
        }

        if (GUILayout.Button("Create Player Spawn Points"))
        {
            missionManager.CreatePlayerSpawnPoints();
        }

        if (GUILayout.Button("Create All Mission Setups"))
        {
            missionManager.CreateMissionSetupBattle();
            missionManager.CreateMissionSetupBattleSupport();
            missionManager.CreateMissionSetupAssassination();
            missionManager.CreateMissionSetupRecon();
            missionManager.CreateMissionSetupSearchAndDestroy();
            missionManager.CreateMissionSetupBaseDestroy();
            missionManager.CreateMissionSetupBaseCapture();
            missionManager.CreateMissionSetupBaseDefend();
            missionManager.CreateMissionSetupConvoyDestroy();
        }

        if (GUILayout.Button("Create Mission Setup Battle"))
        {
            missionManager.CreateMissionSetupBattle();
        }

        if (GUILayout.Button("Create Mission Setup Battle Support"))
        {
            missionManager.CreateMissionSetupBattleSupport();
        }

        if (GUILayout.Button("Create Mission Setup Assassination"))
        {
            missionManager.CreateMissionSetupAssassination();
        }

        if (GUILayout.Button("Create Mission Setup Recon"))
        {
            missionManager.CreateMissionSetupRecon();
        }

        if (GUILayout.Button("Create Mission Setup Search And Destroy"))
        {
            missionManager.CreateMissionSetupSearchAndDestroy();
        }

        if (GUILayout.Button("Create Mission Setup Base Destroy"))
        {
            missionManager.CreateMissionSetupBaseDestroy();
        }

        if (GUILayout.Button("Create Mission Setup Base Capture"))
        {
            missionManager.CreateMissionSetupBaseCapture();
        }

        if (GUILayout.Button("Create Mission Setup Base Defend"))
        {
            missionManager.CreateMissionSetupBaseDefend();
        }

        if (GUILayout.Button("Create Mission Setup Convoy Destroy"))
        {
            missionManager.CreateMissionSetupConvoyDestroy();
        }

        DrawDefaultInspector();
    }
}
