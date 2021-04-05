using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(RandomGenMissionSetupSearchAndDestroy))]

public class RandomGenMissionSetupSearchAndDestroyEditor : Editor
{
    public override void OnInspectorGUI()
    {
        RandomGenMissionSetupSearchAndDestroy randomGenMissionSetupSearchAndDestroy = (RandomGenMissionSetupSearchAndDestroy)target;

        KeyValuePair<string, Color> missionValidity = randomGenMissionSetupSearchAndDestroy.GetMissionValidity();

        GUI.color = missionValidity.Value;
        GUILayout.TextArea("Mission Validity: " + missionValidity.Key);

        GUI.color = Color.white;

        DrawDefaultInspector();
    }
}