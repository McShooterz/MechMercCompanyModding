using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

/// <summary> System that records damage source data and passes information to attached DamageableSurfaces </summary>
[ExecuteInEditMode]
public class DamageDataSystem : MonoBehaviour 
{
	
	Renderer[] renderers;
	SkinnedMeshRenderer[] smRenderers;
	DamagePoint[] damagePoints;
	
	Vector4[] damageData1;
	Vector4[] damageData2;

	void OnEnable() 
	{
		damageData1 = new Vector4[100];
		damageData2 = new Vector4[100];
	}
	
	void Update() 
	{

		/// Brute force version to make testing easier. 
		/// Todo: Optimize (get renderers once OnEnable, create interface for adding DamagePoints)
		damagePoints = GetComponentsInChildren<DamagePoint>();
		renderers = GetComponentsInChildren<Renderer>();
		smRenderers = GetComponentsInChildren<SkinnedMeshRenderer>();
		
		for (int i = 0; i < damagePoints.Length && i < 100; i++)
		{
			var point = damagePoints[i];
			Vector3 p = point.transform.position;

			/// Pack data for damage system
			damageData1[i] = new Vector4(p.x, p.y, p.z, point.power);
			damageData2[i] = new Vector4(point.hot ? Time.time : point.time, point.radius, point.laser, point.burnDur);

			/// Hot point for testing, sets hit time to current time to keep burn glow at 100%
			if (point.hot) { point.time = Time.time; }
		}

		/// Pipe data to renderer materials
		/// Todo: Investigate using MaterialPropertyBlock since lots of data is shared
		foreach (var renderer in renderers) 
		{
			/// Todo: Use instanced materials per damage system (Initialize during OnEnable in gameplay)
			var mat = renderer.sharedMaterial;
			SendData(mat);
		}
		
		foreach (var renderer in smRenderers) 
		{
			/// Todo: Use instanced materials per damage system (Initialize during OnEnable in gameplay)
			var mat = renderer.sharedMaterial;
			SendData(mat);
		}

	}

	private void SendData(Material mat) {
		mat.SetFloat("_RTime", Time.time);
		mat.SetMatrix("_W2O", transform.worldToLocalMatrix);
		mat.SetInt("_DamageDataLength", damagePoints.Length);
		mat.SetVectorArray("_DamageData1", damageData1);
		mat.SetVectorArray("_DamageData2", damageData2);
	}
}
