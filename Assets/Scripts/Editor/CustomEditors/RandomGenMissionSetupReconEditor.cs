using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(RandomGenMissionSetupRecon))]

public class RandomGenMissionSetupReconEditor : Editor
{
    public override void OnInspectorGUI()
    {
        RandomGenMissionSetupRecon randomGenMissionSetupRecon = (RandomGenMissionSetupRecon)target;

        KeyValuePair<string, Color> missionValidity = randomGenMissionSetupRecon.GetMissionValidity();

        GUI.color = missionValidity.Value;
        GUILayout.TextArea("Mission Validity: " + missionValidity.Key);

        GUI.color = Color.white;

        DrawDefaultInspector();
    }
}