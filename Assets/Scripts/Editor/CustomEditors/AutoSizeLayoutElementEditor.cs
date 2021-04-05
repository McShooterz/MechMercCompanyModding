using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(AutoSizeLayoutElement))]
public class AutoSizeLayoutElementEditor : Editor
{
    public override void OnInspectorGUI()
    {
        AutoSizeLayoutElement autoSizeLayoutElement = (AutoSizeLayoutElement)target;

        if (GUILayout.Button("Auto Fill Layout Element"))
        {
            autoSizeLayoutElement.AutoFillLayoutElement();
        }

        DrawDefaultInspector();
    }
}
