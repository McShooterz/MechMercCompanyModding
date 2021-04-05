using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(RandomGenMissionSetupBaseDefend))]

public class RandomGenMissionSetupBaseDefendEditor : Editor
{
    public override void OnInspectorGUI()
    {
        RandomGenMissionSetupBaseDefend randomGenMissionSetupBaseDefend = (RandomGenMissionSetupBaseDefend)target;

        KeyValuePair<string, Color> missionValidity = randomGenMissionSetupBaseDefend.GetMissionValidity();

        GUI.color = missionValidity.Value;
        GUILayout.TextArea("Mission Validity: " + missionValidity.Key);

        GUI.color = Color.white;

        DrawDefaultInspector();
    }
}