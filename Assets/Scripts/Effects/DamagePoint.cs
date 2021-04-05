using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

/// <summary> Script used to record data to move to damagable surface shader </summary>
public class DamagePoint : MonoBehaviour {

	/// <summary> Effect strength </summary>
	public float power;
	/// <summary> Effect creation time (from Time.time) </summary>
	public float time;
	/// <summary> Effect Radius </summary>
	public float radius;
	/// <summary> Effect Laser Strength </summary>
	public float laser;
	/// <summary> Effect Burn Duration </summary>
	public float burnDur;

	/// <summary> Keep effect at 100% burn? </summary>
	public bool hot = false;

}
