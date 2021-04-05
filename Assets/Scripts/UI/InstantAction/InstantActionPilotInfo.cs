using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InstantActionPilotInfo : MonoBehaviour
{
    [SerializeField]
    InstantActionScreen instantActionScreen;

    [SerializeField]
    Image pilotImage;

    [SerializeField]
    Button pilotButton;

    [SerializeField]
    Text pilotButtonText;

    [SerializeField]
    Button mechSelectionButton;

    [SerializeField]
    Text mechSelectionButtonText;

    [SerializeField]
    Button mechDesignButton;

    [SerializeField]
    Text mechDesignButtonText;

    [SerializeField]
    Button mechCustomizeButton;

    [SerializeField]
    bool player;

    [SerializeField]
    int indexSquadMate;

    public PilotDefinition pilotDefinition = null;

    public MechData MechData
    {
        get
        {
            if (player)
            {
                return GlobalDataManager.Instance.instantActionGlobalData.playerMechData;
            }

            return GlobalDataManager.Instance.instantActionGlobalData.squadMateMechDatas[indexSquadMate];
        }
    }

    void Awake()
    {
        if (pilotButton != null)
        {
            pilotButton.onClick.AddListener(delegate { ClickSelectPilotButton(); });
        }

        mechSelectionButton.onClick.AddListener(delegate { ClickSelectMechButton(); });

        mechDesignButton.onClick.AddListener(delegate { ClickSelectDesignButton(); });

        mechCustomizeButton.onClick.AddListener(delegate { ClickCustomizeButton(); });
    }

    public void SetPilotDefinition(PilotDefinition pilot)
    {
        pilotDefinition = pilot;

        if (pilotDefinition != null)
        {
            mechSelectionButton.interactable = true;
            mechDesignButton.interactable = true;
            mechCustomizeButton.interactable = true;

            pilotImage.overrideSprite = pilotDefinition.GetDisplayImage();

            if (pilotImage.overrideSprite != null)
            {
                pilotImage.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            }
        }
        else
        {
            mechSelectionButton.interactable = false;
            mechDesignButton.interactable = false;
            mechCustomizeButton.interactable = false;

            pilotImage.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
        }
    }

    public void SetMechData(MechData mechData)
    {
        if (player)
        {
            GlobalDataManager.Instance.instantActionGlobalData.playerMechData = mechData;
        }
        else
        {
            GlobalDataManager.Instance.instantActionGlobalData.squadMateMechDatas[indexSquadMate] = mechData;
        }
    }

    public void UpdateButtonTexts()
    {
        MechData mechData = MechData;

        if (pilotButton != null)
        {
            if (pilotDefinition != null)
            {
                pilotButtonText.text = pilotDefinition.GetDisplayName();
            }
            else
            {
                pilotButtonText.text = "NO PILOT";
            }
        }

        if (mechData != null)
        {
            mechSelectionButtonText.text = mechData.MechChassis.GetDisplayName();
            mechDesignButtonText.text = mechData.designName;
        }
        else
        {
            mechSelectionButtonText.text = "-";
            mechDesignButtonText.text = "-";
        }
    }

    public void ClickSelectPilotButton()
    {
        instantActionScreen.ClickPilotSelectButton(pilotDefinition, this);

        AudioManager.Instance.PlayButtonClick(0);
    }

    public void ClickSelectMechButton()
    {
        instantActionScreen.SelectMechChassis(this);

        AudioManager.Instance.PlayButtonClick(0);
    }

    public void ClickSelectDesignButton()
    {
        instantActionScreen.ClickDesignSelectButton(MechData);

        AudioManager.Instance.PlayButtonClick(0);
    }

    public void ClickCustomizeButton()
    {
        instantActionScreen.ClickCustomizeButton(MechData);

        AudioManager.Instance.PlayButtonClick(0);
    }
}
