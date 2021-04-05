using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(RandomGenMissionSetupBattle))]

public class RandomGenMissionSetupBattleEditor : Editor
{
    public override void OnInspectorGUI()
    {
        RandomGenMissionSetupBattle randomGenMissionSetupBattle = (RandomGenMissionSetupBattle)target;

        KeyValuePair<string, Color> missionValidity = randomGenMissionSetupBattle.GetMissionValidity();

        GUI.color = missionValidity.Value;
        GUILayout.TextArea("Mission Validity: " + missionValidity.Key);

        GUI.color = Color.white;

        DrawDefaultInspector();
    }
}