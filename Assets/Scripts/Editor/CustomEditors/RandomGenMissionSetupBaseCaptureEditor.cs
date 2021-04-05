using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(RandomGenMissionSetupBaseCapture))]

public class RandomGenMissionSetupBaseCaptureEditor : Editor
{
    public override void OnInspectorGUI()
    {
        RandomGenMissionSetupBaseCapture randomGenMissionSetupBaseCapture = (RandomGenMissionSetupBaseCapture)target;

        KeyValuePair<string, Color> missionValidity = randomGenMissionSetupBaseCapture.GetMissionValidity();

        GUI.color = missionValidity.Value;
        GUILayout.TextArea("Mission Validity: " + missionValidity.Key);

        GUI.color = Color.white;

        DrawDefaultInspector();
    }
}