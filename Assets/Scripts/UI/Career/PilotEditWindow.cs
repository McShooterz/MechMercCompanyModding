using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PilotEditWindow : MonoBehaviour
{
    [SerializeField]
    PersonnelSubScreen personnelSubScreen;

    [SerializeField]
    Image pilotIconImage;

    [SerializeField]
    InputField callsignInputField;

    [SerializeField]
    Text pilotVoiceNameText;

    [SerializeField]
    Button acceptButton;

    [SerializeField]
    int pilotIconIndex = 0;

    [SerializeField]
    int pilotVoiceIndex = 0;

    MechPilot currentPilot;

    List<Sprite> PilotIconSprites { get; set; }

    List<PilotVoiceProfileDefinition> PilotVoices { get; set; }

    public void SetPilot(MechPilot mechPilot)
    {
        currentPilot = mechPilot;

        callsignInputField.text = currentPilot.displayName;

        if (PilotIconSprites == null)
        {
            PilotIconSprites = new List<Sprite>();

            Texture2D[] pilotTextures = ResourceManager.Instance.GetPilotTextures();

            for (int i = 0; i < pilotTextures.Length; i++)
            {
                PilotIconSprites.Add(StaticHelper.GetSpriteUI(pilotTextures[i]));
            }
        }

        if (PilotVoices == null)
        {
            PilotVoices = ResourceManager.Instance.GetPilotVoiceProfileDefinitions();
        }

        if (PilotIconSprites.Count > 0)
        {
            pilotIconIndex = 0;

            for (int i = 0; i < PilotIconSprites.Count; i++)
            {
                if (currentPilot.Icon.texture.name == PilotIconSprites[i].texture.name)
                {
                    pilotIconIndex = i;
                    break;
                }
            }

            pilotIconImage.sprite = PilotIconSprites[pilotIconIndex];
        }
        else
        {
            pilotIconImage.sprite = null;
        }

        if (PilotVoices.Count > 0)
        {
            pilotVoiceIndex = 0;

            for (int i = 0; i < PilotVoices.Count; i++)
            {
                if (currentPilot.PilotVoiceProfile == PilotVoices[i])
                {
                    pilotVoiceIndex = i;
                    break;
                }
            }

            pilotVoiceNameText.text = PilotVoices[pilotVoiceIndex].GetDisplayName();
        }
        else
        {
            pilotVoiceNameText.text = "";
        } 
    }

    public void OnCallsignChanged()
    {
        acceptButton.interactable = callsignInputField.text != "";
    }

    public void ClickPilotIconLeftButton()
    {
        AudioManager.Instance.PlayButtonClick(0);

        if (PilotIconSprites.Count > 0)
        {
            pilotIconIndex--;

            if (pilotIconIndex < 0)
            {
                pilotIconIndex = PilotIconSprites.Count - 1;
            }

            pilotIconImage.sprite = PilotIconSprites[pilotIconIndex];
        }
    }

    public void ClickPilotIconRightButton()
    {
        AudioManager.Instance.PlayButtonClick(0);

        if (PilotIconSprites.Count > 0)
        {
            pilotIconIndex++;

            if (pilotIconIndex == PilotIconSprites.Count)
            {
                pilotIconIndex = 0;
            }

            pilotIconImage.sprite = PilotIconSprites[pilotIconIndex];
        }
    }

    public void ClickPilotVoiceLeftButton()
    {
        AudioManager.Instance.PlayButtonClick(0);

        if (PilotVoices.Count > 0)
        {
            pilotVoiceIndex--;

            if (pilotVoiceIndex < 0)
            {
                pilotVoiceIndex = PilotVoices.Count - 1;
            }

            pilotVoiceNameText.text = PilotVoices[pilotVoiceIndex].GetDisplayName();
        }
    }

    public void ClickPilotVoiceRightButton()
    {
        AudioManager.Instance.PlayButtonClick(0);

        if (PilotVoices.Count > 0)
        {
            pilotVoiceIndex++;

            if (pilotVoiceIndex == PilotVoices.Count)
            {
                pilotVoiceIndex = 0;
            }

            pilotVoiceNameText.text = PilotVoices[pilotVoiceIndex].GetDisplayName();
        }
    }

    public void ClickPilotVoicePlayButton()
    {
        if (PilotVoices.Count > 0)
        {
            AudioManager.Instance.PlayClipUI(PilotVoices[pilotVoiceIndex].GetConfirmOrder());
        }
        else
        {
            AudioManager.Instance.PlayButtonClick(0);
        }
    }

    public void ClickAcceptButton()
    {
        AudioManager.Instance.PlayButtonClick(0);

        currentPilot.displayName = callsignInputField.text;

        if (PilotIconSprites.Count > 0)
            currentPilot.Icon = PilotIconSprites[pilotIconIndex];

        if (PilotVoices.Count > 0)
            currentPilot.PilotVoiceProfile = PilotVoices[pilotVoiceIndex];

        personnelSubScreen.RefreshInfo();

        gameObject.SetActive(false);
    }

    public void ClickCancelButton()
    {
        AudioManager.Instance.PlayButtonClick(0);

        gameObject.SetActive(false);
    }
}
