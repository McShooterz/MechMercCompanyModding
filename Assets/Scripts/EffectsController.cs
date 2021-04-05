using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectsController : MonoBehaviour
{
    [SerializeField]
    AudioSource audioSource;

    [SerializeField]
    Light lightSource;

    [SerializeField]
    AnimationCurve lightCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    [SerializeField]
    float lightIntensityMultiplier = 1.0f;

    [SerializeField]
    float disableTime = 4f;

    [SerializeField]
    float disableTimer = 0f;

    [SerializeField]
    float lightTimer = 0f;

    public AudioSource AudioSource { get => audioSource; }

    void Awake()
    {
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        audioSource.spatialBlend = 1.0f;
        audioSource.maxDistance = 100.0f;
        audioSource.outputAudioMixerGroup = AudioManager.Instance.audioMixerGroupEffects;
    }

	void Update ()
    {
        if (Time.time > disableTimer)
        {
            gameObject.SetActive(false);
        }

        if (lightSource != null)
        {
            lightSource.intensity = lightCurve.Evaluate(Time.time - lightTimer) * lightIntensityMultiplier;
        }
    }

    void OnEnable()
    {
        disableTimer = disableTime + Time.time;

        if (lightSource != null)
        {
            lightTimer = Time.time;
        }
    }
}
