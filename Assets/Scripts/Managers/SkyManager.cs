using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyManager : MonoBehaviour
{
    //[SerializeField]
    //UnityEngine.AzureSky.AzureWeatherController azureWeatherController;

    //[SerializeField]
    //UnityEngine.AzureSky.AzureTimeController azureTimeController;

    [SerializeField]
    AudioSource thunderAudioSource;

    [SerializeField]
    AudioClip[] thunderAudioClips;

    [SerializeField]
    float thunderTimer = 0;

    WeatherDefinition weatherDefinition;

    void Update()
    {
        if (thunderTimer > Time.time)
        {
            Vector3 thunderPosition = Random.onUnitSphere * Random.Range(20.0f, 30.0f);
            thunderPosition.y = 20.0f;
            thunderAudioSource.transform.position = thunderPosition;
            thunderAudioSource.PlayOneShot(thunderAudioClips[Random.Range(0, thunderAudioClips.Length)]);

            thunderTimer = Time.time + weatherDefinition.RandomThunderTime;
        }
    }

    public void CreateSky(WeatherDefinition definition, float time, MechControllerPlayer mechControllerPlayer)
    {
        weatherDefinition = definition;
        GameObject skyPrefab = Instantiate(ResourceManager.Instance.SkyPrefab);
        /*
        azureWeatherController = skyPrefab.GetComponent<UnityEngine.AzureSky.AzureWeatherController>();

        if (azureWeatherController != null)
        {   
            azureTimeController = skyPrefab.GetComponent<UnityEngine.AzureSky.AzureTimeController>();

            if (azureTimeController != null)
            {
                azureTimeController.SetTimeline(time);
            }

            if (weatherDefinition != null)
            {
                UnityEngine.AzureSky.AzureWeatherProfile profile = weatherDefinition.GetSkyProfile();
                GameObject weatherEffectPrefab = weatherDefinition.GetWeatherEffectPrefab();
                AudioClip ambientAudio = weatherDefinition.GetAmbientAudioClip();         

                if (profile != null)
                {
                    azureWeatherController.overrideObject = profile.overrideObject;

                    azureWeatherController.m_defaultWeatherProfilesList.Clear();

                    azureWeatherController.m_defaultWeatherProfilesList.Add(profile);
                }
                else
                {
                    Debug.LogWarning("Profile not found: " + definition.SkyProfile);
                }

                if (weatherEffectPrefab != null)
                {
                    GameObject weatherEffect = Instantiate(weatherEffectPrefab);
                    ObjectFollower objectFollower = weatherEffect.AddComponent<ObjectFollower>();
                    objectFollower.SetFollowTarget(CameraController.Instance.gameObject);
                }

                if (ambientAudio != null)
                {
                    AudioSource audioSource = gameObject.AddComponent<AudioSource>();

                    audioSource.outputAudioMixerGroup = AudioManager.Instance.audioMixerGroupAmbience;
                    audioSource.clip = ambientAudio;
                    audioSource.volume = definition.AmbientAudioVolume;
                    audioSource.loop = true;
                    audioSource.Play();
                }

                thunderAudioClips = definition.GetThunderAudioClips();

                if (thunderAudioClips.Length > 0 && definition.ThunderFrequency > 0.0f)
                {
                    GameObject thunderAudioGameObject = new GameObject();
                    thunderAudioSource = thunderAudioGameObject.AddComponent<AudioSource>();
                    thunderAudioSource.spatialBlend = 1.0f;
                    thunderAudioSource.minDistance = 20.0f;

                    thunderTimer = Time.time + weatherDefinition.RandomThunderTime;
                }

                MechControllerPlayer.Instance.CockpitController.RainEffect.SetActive(definition.CockpitRainEffect);
            }
        }*/
    }
}