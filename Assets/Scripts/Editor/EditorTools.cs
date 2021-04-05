using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;


public class EditorTools
{
    [MenuItem("Tools/Custom Tools/Add Managers To Scene")]
    public static void AddManagersToScene()
    {
        string managersPath = Application.dataPath + "/Prefabs/Managers/";

        if (Object.FindObjectOfType(typeof(ResourceManager)) == null)
        {
            FileInfo fileInfo = new FileInfo(managersPath + "ResourceManager.prefab");
            GameObject prefab = StaticHelper.GetAssetFromFile<GameObject>(fileInfo);

            if (prefab != null)
            {
                PrefabUtility.InstantiatePrefab(prefab);
            }
            else
            {
                Debug.Log("File not found: " + fileInfo.FullName);
            }
        }
        else
        {
            Debug.Log("ResourceManager already exists");
        }

        if (Object.FindObjectOfType(typeof(AudioManager)) == null)
        {
            FileInfo fileInfo = new FileInfo(managersPath + "AudioManager.prefab");
            GameObject prefab = StaticHelper.GetAssetFromFile<GameObject>(fileInfo);

            if (prefab != null)
            {
                PrefabUtility.InstantiatePrefab(prefab);
            }
            else
            {
                Debug.Log("File not found: " + fileInfo.FullName);
            }
        }
        else
        {
            Debug.Log("AudioManager already exists");
        }

        if (Object.FindObjectOfType(typeof(InputManager)) == null)
        {
            FileInfo fileInfo = new FileInfo(managersPath + "InputManager.prefab");
            GameObject prefab = StaticHelper.GetAssetFromFile<GameObject>(fileInfo);

            if (prefab != null)
            {
                PrefabUtility.InstantiatePrefab(prefab);
            }
            else
            {
                Debug.Log("File not found: " + fileInfo.FullName);
            }
        }
        else
        {
            Debug.Log("InputManager already exists");
        }

        if (Object.FindObjectOfType(typeof(PostProcessingManager)) == null)
        {
            FileInfo fileInfo = new FileInfo(managersPath + "PostProcessingManager.prefab");
            GameObject prefab = StaticHelper.GetAssetFromFile<GameObject>(fileInfo);

            if (prefab != null)
            {
                PrefabUtility.InstantiatePrefab(prefab);
            }
            else
            {
                Debug.Log("File not found: " + fileInfo.FullName);
            }
        }
        else
        {
            Debug.Log("PostProcessingManager already exists");
        }

        if (Object.FindObjectOfType(typeof(GlobalDataManager)) == null)
        {
            FileInfo fileInfo = new FileInfo(managersPath + "GlobalDataManager.prefab");
            GameObject prefab = StaticHelper.GetAssetFromFile<GameObject>(fileInfo);

            if (prefab != null)
            {
                PrefabUtility.InstantiatePrefab(prefab);
            }
            else
            {
                Debug.Log("File not found: " + fileInfo.FullName);
            }
        }
        else
        {
            Debug.Log("GlobalDataManager already exists");
        }
    }

    [MenuItem("Tools/Custom Tools/Remove Colliders From Selection")]
    public static void RemoveAllCollidersFromSelection()
    {
        foreach (GameObject selectedGameObject in Selection.gameObjects)
        {
            Collider[] colliders = selectedGameObject.GetComponentsInChildren<Collider>();

            foreach (Collider collider in colliders)
            {
                Object.DestroyImmediate(collider);
            }
        }
    }

    [MenuItem("Tools/Custom Tools/Remove Lights From Selection")]
    public static void RemoveAllLightsFromSelection()
    {
        foreach (GameObject selectedGameObject in Selection.gameObjects)
        {
            Light[] lights = selectedGameObject.GetComponentsInChildren<Light>();

            foreach (Light light in lights)
            {
                Object.DestroyImmediate(light);
            }
        }
    }
}
#endif
