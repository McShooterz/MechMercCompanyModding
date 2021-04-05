using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(RandomGenMissionSetupBattleSupport))]

public class RandomGenMissionSetupBattleSupportEditor : Editor
{
    public override void OnInspectorGUI()
    {
        RandomGenMissionSetupBattleSupport randomGenMissionSetupBattleSupport = (RandomGenMissionSetupBattleSupport)target;

        KeyValuePair<string, Color> missionValidity = randomGenMissionSetupBattleSupport.GetMissionValidity();

        GUI.color = missionValidity.Value;
        GUILayout.TextArea("Mission Validity: " + missionValidity.Key);

        GUI.color = Color.white;

        DrawDefaultInspector();
    }
}