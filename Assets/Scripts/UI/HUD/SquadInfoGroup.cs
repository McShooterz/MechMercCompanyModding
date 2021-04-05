using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SquadInfoGroup : MonoBehaviour
{
    [SerializeField]
    GameObject[] squadHeaders;

    [SerializeField]
    Image[] squadHeaderHighlights;

    [SerializeField]
    SquadMateInfoHUD[] squadMatesInfoCommand;

    [SerializeField]
    SquadMateInfoHUD[] squadMatesInfoSeconday;

    [SerializeField]
    SquadMateInfoHUD[] squadMatesInfoTertiary;

    [SerializeField]
    SquadMateInfoHUD[] squadMateInfosActive;

    [SerializeField]
    Text instructionText;

    [SerializeField]
    Color selectedColor;

    [SerializeField]
    Color subSelectColor;

    public GameObject[] SquadHeaders
    {
        get
        {
            return squadHeaders;
        }
    }

    public Image[] SquadHeaderHighlights
    {
        get
        {
            return squadHeaderHighlights;
        }
    }

    public SquadMateInfoHUD[] SquadMateInfoHUDsCommand
    {
        get
        {
            return squadMatesInfoCommand;
        }
    }

    public SquadMateInfoHUD[] SquadMateInfoHUDsSeconday
    {
        get
        {
            return squadMatesInfoSeconday;
        }
    }

    public SquadMateInfoHUD[] SquadMateInfoHUDsTertiary
    {
        get
        {
            return squadMatesInfoTertiary;
        }
    }

    public Text InstructionText
    {
        get
        {
            return instructionText;
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetPilots(MechController[] squadCommand, MechController[] squadSecondary, MechController[] squadTertiary)
    {
        List<SquadMateInfoHUD> squadMateInfosActiveList = new List<SquadMateInfoHUD>();

        if (squadCommand.Length > 0)
        {
            squadHeaderHighlights[0].color = Color.clear;

            for (int i = 0; i < squadMatesInfoCommand.Length; i++)
            {
                if (i < squadCommand.Length)
                {
                    squadMatesInfoCommand[i].HighLightImage.color = Color.clear;

                    squadMatesInfoCommand[i].PilotText.text = squadCommand[i].MechData.currentMechPilot.displayName.ToUpper();

                    squadMatesInfoCommand[i].MechText.text = squadCommand[i].MechData.customName.ToUpper();

                    squadMatesInfoCommand[i].OrderText.text = "FOLLOW";

                    squadMateInfosActiveList.Add(squadMatesInfoCommand[i]);
                }
                else
                {
                    squadMatesInfoCommand[i].gameObject.SetActive(false);
                }
            }
        }
        else
        {
            squadHeaders[0].SetActive(false);

            for (int i = 0; i < squadMatesInfoCommand.Length; i++)
            {
                squadMatesInfoCommand[i].gameObject.SetActive(false);
            }
        }

        if (squadSecondary.Length > 0)
        {
            squadHeaderHighlights[1].color = Color.clear;

            for (int i = 0; i < squadMatesInfoSeconday.Length; i++)
            {
                if (i < squadSecondary.Length)
                {
                    squadMatesInfoSeconday[i].HighLightImage.color = Color.clear;

                    squadMatesInfoSeconday[i].PilotText.text = squadSecondary[i].MechData.currentMechPilot.displayName.ToUpper();

                    squadMatesInfoSeconday[i].MechText.text = squadSecondary[i].MechData.customName.ToUpper();

                    squadMatesInfoSeconday[i].OrderText.text = "FOLLOW";

                    squadMateInfosActiveList.Add(squadMatesInfoSeconday[i]);
                }
                else
                {
                    squadMatesInfoSeconday[i].gameObject.SetActive(false);
                }
            }
        }
        else
        {
            squadHeaders[1].SetActive(false);

            for (int i = 0; i < squadMatesInfoSeconday.Length; i++)
            {
                squadMatesInfoSeconday[i].gameObject.SetActive(false);
            }
        }

        if (squadTertiary.Length > 0)
        {
            squadHeaderHighlights[2].color = Color.clear;

            for (int i = 0; i < squadMatesInfoTertiary.Length; i++)
            {
                if (i < squadTertiary.Length)
                {
                    squadMatesInfoTertiary[i].HighLightImage.color = Color.clear;

                    squadMatesInfoTertiary[i].PilotText.text = squadTertiary[i].MechData.currentMechPilot.displayName.ToUpper();

                    squadMatesInfoTertiary[i].MechText.text = squadTertiary[i].MechData.customName.ToUpper();

                    squadMatesInfoTertiary[i].OrderText.text = "FOLLOW";

                    squadMateInfosActiveList.Add(squadMatesInfoTertiary[i]);
                }
                else
                {
                    squadMatesInfoTertiary[i].gameObject.SetActive(false);
                }
            }
        }
        else
        {
            squadHeaders[2].SetActive(false);

            for (int i = 0; i < squadMatesInfoTertiary.Length; i++)
            {
                squadMatesInfoTertiary[i].gameObject.SetActive(false);
            }
        }

        squadMateInfosActive = squadMateInfosActiveList.ToArray();
    }

    public void UpdatePilots(MechController[] squadCommand, MechController[] squadSecondary, MechController[] squadTertiary)
    {
        UpdatePilot(squadCommand, squadMatesInfoCommand);

        UpdatePilot(squadSecondary, squadMatesInfoSeconday);

        UpdatePilot(squadTertiary, squadMatesInfoTertiary);
    }

    void UpdatePilot(MechController[] mechs, SquadMateInfoHUD[] squadMateInfoHUDs)
    {
        for (int i = 0; i < mechs.Length; i++)
        {
            if (mechs[i].IsDestroyed)
            {
                if (squadMateInfoHUDs[i].PilotText.color != Color.red)
                {
                    squadMateInfoHUDs[i].PilotText.color = Color.red;

                    squadMateInfoHUDs[i].MechText.color = Color.red;

                    squadMateInfoHUDs[i].OrderText.color = Color.red;

                    squadMateInfoHUDs[i].OrderText.text = "DESTROYED";

                    squadMateInfoHUDs[i].HighLightImage.enabled = false;
                }
            }
            else
            {
                if (mechs[i].currentOrderSquadAI == null)
                {
                    squadMateInfoHUDs[i].OrderText.text = "FOLLOW";
                }
                else if (mechs[i].currentOrderSquadAI is OrderAttackTarget)
                {
                    squadMateInfoHUDs[i].OrderText.text = "ATTACK TARGET";
                }
                else if (mechs[i].currentOrderSquadAI is OrderClosestEnemy)
                {
                    squadMateInfoHUDs[i].OrderText.text = "ATTACK CLOSEST ENEMY";
                }
                else if (mechs[i].currentOrderSquadAI is OrderMoveToNavPoint)
                {
                    squadMateInfoHUDs[i].OrderText.text = "MOVING TO NAV POINT";
                }
                else if (mechs[i].currentOrderSquadAI is OrderHoldPosition)
                {
                    squadMateInfoHUDs[i].OrderText.text = "HOLD POSITION";
                }
            }
        }
    }

    public void HightlightAllSquads(bool shouldHighlightCommandSquad, bool shouldHighlightSecondarySquad, bool shouldHighlightTertiarySquad)
    {
        if (shouldHighlightCommandSquad)
        {
            squadHeaderHighlights[0].color = selectedColor;

            foreach (SquadMateInfoHUD squadMateInfoHUD in squadMatesInfoCommand)
            {
                squadMateInfoHUD.HighLightImage.color = selectedColor;
            }
        }

        if (shouldHighlightSecondarySquad)
        {
            squadHeaderHighlights[1].color = selectedColor;

            foreach (SquadMateInfoHUD squadMateInfoHUD in squadMatesInfoSeconday)
            {
                squadMateInfoHUD.HighLightImage.color = selectedColor;
            }
        }

        if (shouldHighlightTertiarySquad)
        {
            squadHeaderHighlights[2].color = selectedColor;

            foreach (SquadMateInfoHUD squadMateInfoHUD in squadMatesInfoTertiary)
            {
                squadMateInfoHUD.HighLightImage.color = selectedColor;
            }
        }
    }

    public void HighlightSelectionSquad(int index)
    {
        switch (index)
        {
            case 0:
                {
                    squadHeaderHighlights[0].color = selectedColor;

                    foreach (SquadMateInfoHUD squadMateInfoHUD in squadMatesInfoCommand)
                    {
                        squadMateInfoHUD.HighLightImage.color = selectedColor;
                    }

                    break;
                }
            case 1:
                {
                    squadHeaderHighlights[1].color = selectedColor;

                    foreach (SquadMateInfoHUD squadMateInfoHUD in squadMatesInfoSeconday)
                    {
                        squadMateInfoHUD.HighLightImage.color = selectedColor;
                    }

                    break;
                }
            case 2:
                {
                    squadHeaderHighlights[2].color = selectedColor;

                    foreach (SquadMateInfoHUD squadMateInfoHUD in squadMatesInfoTertiary)
                    {
                        squadMateInfoHUD.HighLightImage.color = selectedColor;
                    }

                    break;
                }
        }
    }

    public void HighlightSelectedIndividual(int index)
    {
        squadMateInfosActive[index].HighLightImage.color = selectedColor;
    }

    public void HightlightSubSelectionSquad(int index)
    {
        switch (index)
        {
            case 0:
                {
                    squadHeaderHighlights[0].color = subSelectColor;

                    foreach (SquadMateInfoHUD squadMateInfoHUD in squadMatesInfoCommand)
                    {
                        squadMateInfoHUD.HighLightImage.color = subSelectColor;
                    }

                    break;
                }
            case 1:
                {
                    squadHeaderHighlights[1].color = subSelectColor;

                    foreach (SquadMateInfoHUD squadMateInfoHUD in squadMatesInfoSeconday)
                    {
                        squadMateInfoHUD.HighLightImage.color = subSelectColor;
                    }

                    break;
                }
            default:
                {
                    squadHeaderHighlights[2].color = subSelectColor;

                    foreach (SquadMateInfoHUD squadMateInfoHUD in squadMatesInfoTertiary)
                    {
                        squadMateInfoHUD.HighLightImage.color = subSelectColor;
                    }

                    break;
                }
        }
    }

    public void ClearHighlights()
    {
        foreach (Image image in SquadHeaderHighlights)
        {
            image.color = Color.clear;
        }

        foreach (SquadMateInfoHUD squadMateInfoHUD in squadMateInfosActive)
        {
            squadMateInfoHUD.HighLightImage.color = Color.clear;
        }
    }
}
