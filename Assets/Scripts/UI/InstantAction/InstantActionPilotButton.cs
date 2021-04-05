using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantActionPilotButton : BaseScrollSelectableButton
{
    public CallBackFunction<PilotDefinition, InstantActionPilotButton> callBackFunction;

    public PilotDefinition PilotDefinition { get; private set; }

    public void Initialize(PilotDefinition definition, CallBackFunction<PilotDefinition, InstantActionPilotButton> function)
    {
        PilotDefinition = definition;
        callBackFunction = function;

        if (PilotDefinition != null)
        {
            MainText.text = PilotDefinition.GetDisplayName();
        }
        else
        {
            MainText.text = "NO PILOT";
        }
    }

    public override void ClickButton()
    {
        callBackFunction(PilotDefinition, this);

        AudioManager.Instance.PlayButtonClick(0);
    }
}
