using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PilotDefinition : Definition
{
    public string DisplayName = "";

    public string RealName = "";

    public string BackStory = "";

    public string DisplayImage = "";

    public string VoiceProfile = "";

    public int GunnerySkill = 0;

    public string GetDisplayName()
    {
        return ResourceManager.Instance.GetLocalization(DisplayName);
    }

    public string GetBackStory()
    {
        return ResourceManager.Instance.GetLocalization(BackStory);
    }

    public Sprite GetDisplayImage()
    {
        Texture2D pilotImageTexture = ResourceManager.Instance.GetPilotTexture2D(DisplayImage);

        if (pilotImageTexture != null)
        {
            return StaticHelper.GetSpriteUI(pilotImageTexture);
        }

        return null;
    }

    public PilotVoiceProfileDefinition GetPilotVoiceProfile()
    {
        PilotVoiceProfileDefinition pilotVoiceProfile = ResourceManager.Instance.GetPilotVoiceProfile(VoiceProfile);

        if (pilotVoiceProfile == null)
        {
            pilotVoiceProfile = ResourceManager.Instance.GetPilotVoiceProfile("Default");
        }

        return pilotVoiceProfile;
    }
}
