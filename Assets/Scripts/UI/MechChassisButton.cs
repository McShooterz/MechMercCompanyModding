using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MechChassisButton : MonoBehaviour
{
    [SerializeField]
    Text mechNameText;

    [SerializeField]
    Text weightText;

    MechChassisDefinition mechChassisDefinition;

    public delegate void CallBackFunction(MechChassisDefinition mechChassisDefinition);
    public CallBackFunction callBackFunction;

    // Use this for initialization
    void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void SetMechChassisDefinition(MechChassisDefinition chassisDefinition)
    {
        mechChassisDefinition = chassisDefinition;
        mechNameText.text = mechChassisDefinition.GetDisplayName();
        weightText.text = mechChassisDefinition.Tonnage.ToString() + "T";
    }

    public void SetCallBackFunction(CallBackFunction function)
    {
        callBackFunction = function;
    }

    public void ClickButton()
    {
        callBackFunction(mechChassisDefinition);

        AudioManager.Instance.PlayButtonClick(0);
    }
}
