using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEditor;

[CustomEditor(typeof(MechMetaController))]
public class MechMetaControllerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        MechMetaController mechMetaController = (MechMetaController)target;

        if (GUILayout.Button("Auto Fill Rigidbodies"))
        {
            mechMetaController.AutoFillRigidbodies();
        }

        if (GUILayout.Button("Add Foot AudioSource"))
        {
            mechMetaController.AddFootAudioSource();
        }

        if (GUILayout.Button("Activate Rigidbodies"))
        {
            mechMetaController.ActivateRigidbodies();
        }

        DrawDefaultInspector();
    }
}
