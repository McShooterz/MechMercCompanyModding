using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

[CustomEditor(typeof(PlacementSystem))]
[CanEditMultipleObjects]
public class PrefabPlacementEditor : Editor
{

    private SerializedProperty prefabs;
    private SerializedProperty layerMask;
    private SerializedProperty group;
    private SerializedProperty prefabTag;
    private SerializedProperty minRange, maxRange;
    private SerializedProperty randomizePrefab;
    private SerializedProperty toInstantiate;

    private SerializedProperty spread;
    private SerializedProperty radius;
    private SerializedProperty amount;
    private SerializedProperty size;
    private SerializedProperty positionOffset;

    private SerializedProperty canPlaceOver;
    private SerializedProperty canAlign;
    private SerializedProperty isRandomS;
    private SerializedProperty isRandomR;
    private SerializedProperty hideInHierarchy;

    private Vector3 lastPos;
    private Vector3 mousePos;
    private Quaternion mouseRot;
	private bool hasMouse = false;
	
    private void OnEnable()
    {
        prefabs = serializedObject.FindProperty("prefabs");
        layerMask = serializedObject.FindProperty("layerMask");
        prefabTag = serializedObject.FindProperty("prefabTag");
        group = serializedObject.FindProperty("group");
        minRange = serializedObject.FindProperty("minRange");
        maxRange = serializedObject.FindProperty("maxRange");
        randomizePrefab = serializedObject.FindProperty("randomizePrefab");
        toInstantiate = serializedObject.FindProperty ("toInstantiate");
        spread = serializedObject.FindProperty("spread");
        radius = serializedObject.FindProperty("radius");
        amount = serializedObject.FindProperty("amount");
        size = serializedObject.FindProperty("size");
        positionOffset = serializedObject.FindProperty("positionOffset");
        canPlaceOver = serializedObject.FindProperty("canPlaceOver");
        canAlign = serializedObject.FindProperty("canAlign");
        isRandomS = serializedObject.FindProperty("isRandomS");
        isRandomR = serializedObject.FindProperty("isRandomR");
        hideInHierarchy = serializedObject.FindProperty("hideInHierarchy");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
   
        EditorGUILayout.PropertyField(prefabs, new GUIContent ("Prefabs"), true);

		GUILayout.BeginHorizontal();
		{
			EditorGUILayout.PrefixLabel("Raycast Mask");
			int mask = layerMask.intValue;
			LayerMask tempMask = EditorGUILayout.MaskField(InternalEditorUtility.LayerMaskToConcatenatedLayersMask(mask), InternalEditorUtility.layers);
			mask = InternalEditorUtility.ConcatenatedLayersMaskToLayerMask(tempMask);
			layerMask.intValue = mask;
		}
		GUILayout.EndHorizontal();

		EditorGUILayout.ObjectField(group, typeof(Transform), new GUIContent("Group"));
        prefabTag.stringValue = EditorGUILayout.TagField(new GUIContent("Prefab Tag"), prefabTag.stringValue);
        EditorGUILayout.PropertyField(randomizePrefab, new GUIContent ("Randomize Prefab"));

        if (!randomizePrefab.boolValue)
		{
            EditorGUILayout.DelayedIntField(toInstantiate, new GUIContent("Prefab Index"));
		}
        else
        {
            EditorGUILayout.DelayedIntField(minRange, new GUIContent("minRange"));
            EditorGUILayout.DelayedIntField(maxRange, new GUIContent("maxRange"));
        }

        EditorGUILayout.PropertyField(canPlaceOver, new GUIContent("Override"));
        EditorGUILayout.Slider(spread, 0, 10, new GUIContent("Spread"));
        EditorGUILayout.Slider(radius, 0, 10, new GUIContent("Radius"));
        EditorGUILayout.IntSlider(amount, 1, 50, new GUIContent("Amount"));
        EditorGUILayout.Slider(size, 0.1f, 10, new GUIContent("Size"));
        EditorGUILayout.PropertyField(canAlign, new GUIContent("Align with normal"));
        EditorGUILayout.DelayedFloatField(positionOffset, new GUIContent("yOffset"));
        EditorGUILayout.PropertyField(isRandomS, new GUIContent("Randomize Size"));
        EditorGUILayout.PropertyField(isRandomR, new GUIContent("Randomize Rotation"));
        EditorGUILayout.PropertyField(hideInHierarchy, new GUIContent("Hide in Hierarchy"));

        serializedObject.ApplyModifiedProperties();
    }
    
    private void OnSceneGUI()
    {
        Event current = Event.current;
        int controlId = GUIUtility.GetControlID(GetHashCode(), FocusType.Passive);

        UpdateMousePosition();
        if (hasMouse && (current.type == EventType.MouseDrag || current.type == EventType.MouseDown) && !current.alt && prefabs != null)
        {
            if (current.button == 0 && (lastPos == Vector3.zero || CanDraw()) && !current.shift)
            {
                lastPos = mousePos;

                for (int i = 0; i < amount.intValue; i++)
                {
					PrefabInstantiate();
					if (radius.floatValue == 0) 
					{
						// Only place one when radius is 0
						break;
					}
                }
            }
            else if (current.button == 0 && current.shift)
            {
                lastPos = mousePos;
                PrefabRemove();
            }
        
        }

        if (current.type == EventType.MouseUp) 
		{
            lastPos = Vector3.zero;
		}

        if (Event.current.type == EventType.Layout)
        {
            HandleUtility.AddDefaultControl(controlId);
        }

        SceneView.RepaintAll();
    }

    public bool CanDraw()
    { 
        float dist = Vector3.Distance(mousePos, lastPos);

        return (dist >= spread.floatValue / 2);
    }

    public void UpdateMousePosition()
    {
        Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray.origin, ray.direction, out hit, Mathf.Infinity, layerMask.intValue))
        {
            mousePos = hit.point;
            mouseRot = Quaternion.FromToRotation(Vector3.up, hit.normal);
			// Animate handle size/color for feedback purposes
			Color c1 = Color.blue; c1.a = .5f + .08f * Mathf.Sin(Time.time * 2.5f);
			Color c2 = Color.cyan;

            Handles.color = c1;
			Handles.DrawSolidDisc(mousePos, hit.normal, .05f - .04f * Mathf.Sin(Time.time * 2.5f) + radius.floatValue);
            Handles.color = c2;
            Handles.DrawWireDisc(mousePos, hit.normal, .01f + radius.floatValue);
			
			hasMouse = true;
        }
        else
        {
			hasMouse = false;
		}
    }

    public void PrefabInstantiate()
    {
		int index = randomizePrefab.boolValue ? Random.Range(minRange.intValue, maxRange.intValue + 1) : toInstantiate.intValue;
        float prefabSize = size.floatValue;

		Vector3 position = mousePos;
		Vector3 scale = Vector3.one * prefabSize;
		Quaternion rotation = canAlign.boolValue ? mouseRot : Quaternion.identity;
		HideFlags hideFlags = hideInHierarchy.boolValue ? HideFlags.HideInHierarchy : HideFlags.None; 
       
		// Adjust position by radius along x/z axis
		position += new Vector3(-.5f + Random.value, 0, -.5f + Random.value).normalized * Random.value * radius.floatValue;

		// Ensure we hit before instantiating a thing.
		// Need to adjust up by 1.0f (could be less) to ensure we will be able to hit terrain when radius=0
        RaycastHit hit;
        if (Physics.Raycast(position + Vector3.up * (1.0f + radius.floatValue), Vector3.down, out hit, Mathf.Infinity, layerMask.intValue)) 
		{
			GameObject created = PrefabUtility.InstantiatePrefab(prefabs.GetArrayElementAtIndex(index).objectReferenceValue as GameObject) as GameObject;
			created.hideFlags |= hideFlags;
			Transform ct = created.transform;
			ct.localScale = scale;
			ct.rotation = rotation;
			ct.position = hit.point;

			var groupObj = group.objectReferenceValue as Transform;

			if (groupObj != null) 
			{
				ct.SetParent(groupObj);
			}

			if (isRandomR.boolValue) 
			{
				ct.Rotate(0, Random.Range(0, 360), 0);
			}
			if (isRandomS.boolValue) 
			{
				// Do later: Create size adjustment ui
				ct.localScale = Vector3.Scale(ct.localScale, Vector3.one * Random.Range(.9f, 1.15f));
			}

			Undo.RegisterCreatedObjectUndo(created, "Prefab placement");
		}
    }

    private void PrefabRemove()
    {
        GameObject[] prefabsInRadius;

        prefabsInRadius = GameObject.FindGameObjectsWithTag (prefabTag.stringValue);

        foreach (GameObject p in prefabsInRadius)
        {
            float dist = Vector3.Distance(mousePos, p.transform.position);

			if (p != null && dist <= radius.floatValue / 2) 
			{
                Undo.DestroyObjectImmediate(p);
			}
        }
    }
}
