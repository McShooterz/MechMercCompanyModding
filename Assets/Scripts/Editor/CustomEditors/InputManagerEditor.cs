using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(InputManager))]
public class InputManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        InputManager inputManager = (InputManager)target;

        DrawDefaultInspector();
    }
}
