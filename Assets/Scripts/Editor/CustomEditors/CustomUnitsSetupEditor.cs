using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CustomUnitsSetup))]
public class CustomUnitsSetupEditor : Editor
{
    public override void OnInspectorGUI()
    {
        CustomUnitsSetup customUnitsSetup = (CustomUnitsSetup)target;

        if (GUILayout.Button("Auto fill unit setups"))
        {
            customUnitsSetup.AutoFillUnitSetups();
        }

        DrawDefaultInspector();
    }
}
