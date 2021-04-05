using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(RandomGenMissionSetupBaseDestroy))]

public class RandomGenMissionSetupBaseDestroyEditor : Editor
{
    public override void OnInspectorGUI()
    {
        RandomGenMissionSetupBaseDestroy randomGenMissionSetupBaseDestroy = (RandomGenMissionSetupBaseDestroy)target;

        KeyValuePair<string, Color> missionValidity = randomGenMissionSetupBaseDestroy.GetMissionValidity();

        GUI.color = missionValidity.Value;
        GUILayout.TextArea("Mission Validity: " + missionValidity.Key);

        GUI.color = Color.white;

        DrawDefaultInspector();
    }
}