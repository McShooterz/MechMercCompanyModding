using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SingletonScriptableObject<T> : ScriptableObject where T : ScriptableObject
{
    public static T Instance { get; private set; }

    protected void Initialize()
    {
        Instance = Resources.FindObjectsOfTypeAll<T>()[0];
    }
}
