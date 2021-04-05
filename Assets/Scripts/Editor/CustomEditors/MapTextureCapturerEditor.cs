using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MapTextureCapturer))]
public class MapTextureCapturerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        MapTextureCapturer mapTextureCapturer = (MapTextureCapturer)target;

        if (GUILayout.Button("Save Map Texture"))
        {
            mapTextureCapturer.CaptureCameraImage();
        }

        DrawDefaultInspector();
    }
}
