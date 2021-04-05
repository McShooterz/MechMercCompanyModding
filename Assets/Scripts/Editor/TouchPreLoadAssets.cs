using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[InitializeOnLoad]
public class TouchPreLoadAssets
{
    static TouchPreLoadAssets()
    {
        PlayerSettings.GetPreloadedAssets();
    }
}
