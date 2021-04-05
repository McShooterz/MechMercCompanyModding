using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ResourceManager))]
public class ResourceManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        ResourceManager resourceManager = (ResourceManager)target;

        if (GUILayout.Button("Auto Fill Prefabs"))
        {
            resourceManager.AutoFillPrefabs();
        }

        DrawDefaultInspector();
    }
}
