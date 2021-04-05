using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(RandomGenMissionSetupAssassination))]

public class RandomGenMissionSetupAssassinationEditor : Editor
{
    public override void OnInspectorGUI()
    {
        RandomGenMissionSetupAssassination randomGenMissionSetupAssassination = (RandomGenMissionSetupAssassination)target;

        KeyValuePair<string, Color> missionValidity = randomGenMissionSetupAssassination.GetMissionValidity();

        GUI.color = missionValidity.Value;
        GUILayout.TextArea("Mission Validity: " + missionValidity.Key);

        GUI.color = Color.white;

        DrawDefaultInspector();
    }
}