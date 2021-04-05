using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InstantActionFactionMenu : MonoBehaviour
{
    [SerializeField]
    Transform content;

    [SerializeField]
    IndexButton indexButton;

    [SerializeField]
    IndexButton[] buttons;

    [SerializeField]
    Text factionNameText;

    [SerializeField]
    Text factionDescriptionText;

    [SerializeField]
    FactionLogoUI factionLogoUI;

    [SerializeField]
    Color buttonColorDefault;

    [SerializeField]
    Color buttonColorHighlight;

    [SerializeField]
    int selectedIndex = 0;

    List<FactionDefinition> factions;

    public delegate void CallBackFunction(FactionDefinition factionDefinition);
    public CallBackFunction callBackFunction;

    public void Initialize(CallBackFunction function, List<FactionDefinition> factionDefinitions)
    {
        selectedIndex = 0;

        callBackFunction = function;

        factions = factionDefinitions;

        for (int i = 1; i < buttons.Length; i++)
        {
            Destroy(buttons[i].gameObject);
        }

        if (factions.Count > 0)
        {
            buttons = new IndexButton[factions.Count];

            buttons[0] = indexButton;

            indexButton.gameObject.SetActive(true);
            indexButton.Initialize(0, factions[0].GetDisplayName(), SelectFaction);
            indexButton.BackgroundImage.color = buttonColorDefault;

            for (int i = 1; i < buttons.Length; i++)
            {
                GameObject indexButtonObject = Instantiate(indexButton.gameObject, content);

                IndexButton newIndexButton = indexButtonObject.GetComponent<IndexButton>();

                if (newIndexButton != null)
                {
                    newIndexButton.Initialize(i, factions[i].GetDisplayName(), SelectFaction);
                    buttons[i] = newIndexButton;
                }
                else if (indexButtonObject != null)
                {
                    Destroy(indexButtonObject);
                }
            }

            SelectFaction(0);
        }
        else
        {
            indexButton.gameObject.SetActive(false);
            buttons = new IndexButton[0];
        }
    }

    public void SelectFaction(int index)
    {
        buttons[selectedIndex].BackgroundImage.color = buttonColorDefault;

        selectedIndex = index;

        buttons[selectedIndex].BackgroundImage.color = buttonColorHighlight;

        FactionDefinition factionDefinition = factions[selectedIndex];

        if (factionDefinition != null)
        {
            factionNameText.text = factionDefinition.GetDisplayName();
            factionDescriptionText.text = factionDefinition.GetDescription();
            factionLogoUI.SetFactionLogo(factionDefinition.FactionLogo);
        }
        else
        {
            factionNameText.text = "";
            factionDescriptionText.text = "";
            factionLogoUI.SetFactionLogo(null);
        }
    }

    public void SelectFaction(FactionDefinition factionDefinition)
    {
        for (int i = 0; i < factions.Count; i++)
        {
            if (factionDefinition == factions[i])
            {
                SelectFaction(i);
                break;
            }
        }
    }

    public void ClickCancelButton()
    {
        gameObject.SetActive(false);

        AudioManager.Instance.PlayButtonClick(0);
    }

    public void ClickSelectButton()
    {
        callBackFunction(factions[selectedIndex]);

        gameObject.SetActive(false);

        AudioManager.Instance.PlayButtonClick(0);
    }
}
