using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WeatherDefinition : Definition
{
    public string SkyProfile = "";

    public string WeatherEffectPrefab = "";

    public string AmbientAudioClip = "";

    public float AmbientAudioVolume = 1.0f;

    public bool CockpitRainEffect = false;

    public string[] ThunderAudioClips = new string[0];

    public float ThunderFrequency = 0.0f;

    public float ThunderVariance = 0.0f;

    public float RandomThunderTime { get => ThunderFrequency + Random.Range(-ThunderVariance, ThunderVariance); }

    /*public UnityEngine.AzureSky.AzureWeatherProfile GetSkyProfile()
    {
        return ResourceManager.Instance.GetSkyProfile(SkyProfile);
    }*/

    public GameObject GetWeatherEffectPrefab()
    {
        return ResourceManager.Instance.GetEffectPrefab(WeatherEffectPrefab);
    }

    public AudioClip GetAmbientAudioClip()
    {
        return ResourceManager.Instance.GetAudioClip(AmbientAudioClip);
    }

    public AudioClip[] GetThunderAudioClips()
    {
        List<AudioClip> thunderAudioClips = new List<AudioClip>();
        AudioClip audioClip;

        for (int i = 0; i < ThunderAudioClips.Length; i++)
        {
            audioClip = ResourceManager.Instance.GetAudioClip(ThunderAudioClips[i]);

            if (audioClip != null)
            {
                thunderAudioClips.Add(audioClip);
            }
        }

        return thunderAudioClips.ToArray();
    }
}
