using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public sealed class InstantActionPilotSelectionMenu : BaseScrollSelectionMenu
{
    [SerializeField]
    Text pilotNameText;

    [SerializeField]
    Text pilotDescriptionText;

    [SerializeField]
    Text pilotGunneryText;

    [SerializeField]
    Image pilotImage;

    PilotDefinition selectedPilotDefinition;

    public CallBackFunction<PilotDefinition> callBackFunction;

    public void SetCallBackFunction(CallBackFunction<PilotDefinition> function)
    {
        callBackFunction = function;
    }

    protected override void InitializeButtons<T>(T[] elements)
    {
        for (int i = 0; i < buttonsList.Count; i++)
        {
            (buttonsList[i] as InstantActionPilotButton).Initialize(elements[i] as PilotDefinition, SelectElement);
        }
    }

    public override void SelectElement<T>(T element, BaseScrollSelectableButton targetButton)
    {
        SelectButton(targetButton);

        selectedPilotDefinition = element as PilotDefinition;

        SetDisplays(selectedPilotDefinition);

        if (selectedPilotDefinition != null)
        {
            pilotNameText.text = selectedPilotDefinition.GetDisplayName();
            pilotDescriptionText.text = selectedPilotDefinition.GetBackStory();
            pilotGunneryText.text = "Gunnery Skill: " + selectedPilotDefinition.GunnerySkill.ToString();

            pilotImage.overrideSprite = selectedPilotDefinition.GetDisplayImage();

            if (pilotImage.overrideSprite != null)
            {
                pilotImage.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            }
        }
        else
        {
            pilotNameText.text = "NO PILOT";
            pilotDescriptionText.text = "";
            pilotGunneryText.text = "";

            pilotImage.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
        }
    }

    public void SelectElement(PilotDefinition pilotDefinition)
    {
        selectedPilotDefinition = pilotDefinition;

        SetDisplays(selectedPilotDefinition);

        foreach (BaseScrollSelectableButton button in buttonsList)
        {
            if ((button as InstantActionPilotButton).PilotDefinition == pilotDefinition)
            {
                SelectButton(button);

                break;
            }
        }
    }

    void SetDisplays(PilotDefinition pilotDefinition)
    {
        if (pilotDefinition != null)
        {
            pilotNameText.text = pilotDefinition.GetDisplayName();
            pilotDescriptionText.text = pilotDefinition.GetBackStory();
            pilotGunneryText.text = "Gunnery: " + pilotDefinition.GunnerySkill.ToString();
        }
        else
        {
            pilotNameText.text = "No Pilot";
            pilotDescriptionText.text = "";
            pilotGunneryText.text = "";

            pilotImage.overrideSprite = null;
        }
    }

    public override void ClickSelectButton()
    {
        callBackFunction(selectedPilotDefinition);

        gameObject.SetActive(false);

        AudioManager.Instance.PlayButtonClick(0);
    }
}
