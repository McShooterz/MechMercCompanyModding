using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionDefinition : Definition
{
    public string DisplayName = "";

    public string Description = "";

    public string MusicName = "";

    public string EnemyFaction = "";

    public int DifficultyTier = 0;

    public int MissionPay = 0;

	public string GetDisplayName()
    {
        return ResourceManager.Instance.GetLocalization(DisplayName);
    }

    public string GetDescription()
    {
        return ResourceManager.Instance.GetLocalization(Description);
    }

    public AudioClip GetMissionMusic()
    {
        return ResourceManager.Instance.GetAudioClip(MusicName);
    }

    public FactionDefinition GetEnemyFaction()
    {
        return ResourceManager.Instance.GetFactionDefinition(EnemyFaction);
    }
}
