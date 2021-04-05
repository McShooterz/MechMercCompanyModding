using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(RandomGenMissionSetupConvoyDestroy))]

public class RandomGenMissionSetupConvoyDestroyEditor : Editor
{
    public override void OnInspectorGUI()
    {
        RandomGenMissionSetupConvoyDestroy randomGenMissionSetupConvoyDestroy = (RandomGenMissionSetupConvoyDestroy)target;

        KeyValuePair<string, Color> missionValidity = randomGenMissionSetupConvoyDestroy.GetMissionValidity();

        GUI.color = missionValidity.Value;
        GUILayout.TextArea("Mission Validity: " + missionValidity.Key);

        GUI.color = Color.white;

        DrawDefaultInspector();
    }
}