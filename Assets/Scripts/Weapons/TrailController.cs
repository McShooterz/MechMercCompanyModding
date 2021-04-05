using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailController : MonoBehaviour
{
    [SerializeField]
    ParticleSystem[] particleSystems;

    [SerializeField]
    TrailRenderer[] trailRenderers;

    [SerializeField]
    float lifeTimer = Mathf.Infinity;
	
	// Update is called once per frame
	void Update ()
    {
        if (Time.time > lifeTimer)
        {
            gameObject.SetActive(false);
        }
	}

    void OnEnable()
    {
        lifeTimer = Mathf.Infinity;
    }

    public void SetLifeTimer(float lifeTime)
    {
        lifeTimer = Time.time + lifeTime;
    }

    public void SetScale(float scale)
    {
        foreach (TrailRenderer trailRenderer in trailRenderers)
        {
            trailRenderer.startWidth = scale;
        }
    }

    public void SetEmission(bool state)
    {
        foreach(ParticleSystem particleSystem in particleSystems)
        {
            ParticleSystem.EmissionModule emissionModule = particleSystem.emission;
            emissionModule.enabled = state;
        }
    }

    public void ClearTrailRenderers()
    {
        foreach (TrailRenderer trailRenderer in trailRenderers)
        {
            trailRenderer.Clear();
        }
    }
}
