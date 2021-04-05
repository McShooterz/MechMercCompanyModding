using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapDefinition : Definition
{
    public string DisplayName = "";

    public string Scene = "";

    public string Texture = "";

    public string MusicAudioGroup = "";

    public BiomeType Biome = BiomeType.Forest_Coniferous;

    public MissionType[] MissionTypes = new MissionType[0];

    public float CoolingModifier = 0.0f;

    public string CustomMissionDefinition = "";

    public SkyTimeElement[] SkyTimeElements = new SkyTimeElement[] { new SkyTimeElement() };

    public SkyWeatherElement[] SkyWeatherElements = new SkyWeatherElement[0];

    public int RandomIndexSkyTime
    {
        get
        {
            if (SkyTimeElements.Length > 0)
            {
                float[] randomWeights = new float[SkyTimeElements.Length];

                for (int i = 0; i < randomWeights.Length; i++)
                {
                    randomWeights[i] = SkyTimeElements[i].RandomWeight;
                }

                return StaticHelper.GetRandomIndexByWeight(randomWeights);
            }

            return -1;
        }
    }

    public int RandomIndexSkyWeather
    {
        get
        {
            if (SkyWeatherElements.Length > 0)
            {
                float[] randomWeights = new float[SkyWeatherElements.Length];

                for (int i = 0; i < randomWeights.Length; i++)
                {
                    randomWeights[i] = SkyWeatherElements[i].RandomWeight;
                }

                return StaticHelper.GetRandomIndexByWeight(randomWeights);
            }

            return -1;
        }
    }

    public string GetDisplayName()
    {
        return ResourceManager.Instance.GetLocalization(DisplayName);
    }

    public Texture2D GetMapTexture()
    {
        return ResourceManager.Instance.GetMapTexture2D(Texture);
    }

    public AudioClip GetMissionMusic()
    {
        AudioGroup audioGroup = ResourceManager.Instance.GetAudioGroup(MusicAudioGroup);

        if (audioGroup != null)
        {
            return audioGroup.GetRandomClip();
        }
        else
        {
            Debug.LogError("Error: Audio group not found - " + MusicAudioGroup);
        }

        return null;
    }

    public MissionDefinition GetCustomMissionDefinition()
    {
        return ResourceManager.Instance.GetMissionDefinition(CustomMissionDefinition);
    }

    public class SkyTimeElement
    {
        public string DisplayName = "DayTimeNoon";

        public float Time = 12.0f;

        public float RandomWeight = 1.0f;

        public string GetDisplayName()
        {
            return ResourceManager.Instance.GetLocalization(DisplayName);
        }
    }

    public class SkyWeatherElement
    {
        public string DisplayName = "";

        public string WeatherDefinition = "";

        public float RandomWeight = 1.0f;

        public string GetDisplayName()
        {
            return ResourceManager.Instance.GetLocalization(DisplayName);
        }

        public WeatherDefinition GetWeatherDefinition()
        {
            return ResourceManager.Instance.GetWeatherDefinition(WeatherDefinition);
        }
    }
}
