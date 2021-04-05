using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeamController : MonoBehaviour
{
    [SerializeField]
    LineRenderer lineRenderer;

    [SerializeField]
    GameObject muzzleEffect;

    [SerializeField]
    GameObject impactEffect;

    public LineRenderer LineRenderer
    {
        get
        {
            return lineRenderer;
        }
    }

    public GameObject MuzzleEffect
    {
        get
        {
            return muzzleEffect;
        }
    }

    public GameObject ImpactEffect
    {
        get
        {
            return impactEffect;
        }
    }

	// Use this for initialization
	void Start ()
    {
        TurnOff();
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void TurnOff()
    {
        lineRenderer.SetPosition(0, Vector3.zero);
        lineRenderer.SetPosition(1, Vector3.zero);

        muzzleEffect.SetActive(false);
        impactEffect.SetActive(false);
    }
    public void SetScaling(BeamWeaponDefinition beamWeaponDefinition)
    {
        lineRenderer.widthMultiplier = beamWeaponDefinition.BeamWidth;

        if ((object)muzzleEffect != null)
        {
            muzzleEffect.transform.localScale = beamWeaponDefinition.MuzzleEffectScaleVector;
        }

        if ((object)impactEffect != null)
        {
            impactEffect.transform.localScale = beamWeaponDefinition.ImpactEffectScaleVector;
        }
    }

    public void SetBeamWidth(float beamWidth)
    {
        lineRenderer.widthMultiplier = beamWidth;
    }
}
