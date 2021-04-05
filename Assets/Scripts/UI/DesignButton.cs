using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DesignButton : MonoBehaviour
{
    [SerializeField]
    Text text;

    //[SerializeField]
    //MechDesignSaveMenu mechDesignSaveMenu;

    MechDesign mechDesign;

    public delegate void CallBackFunction(string chassisKey, string designKey);
    public CallBackFunction callBackFunction;

    public Text Text
    {
        get
        {
            return text;
        }
    }

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void Initialize(CallBackFunction callBackFunction, MechDesign design)
    {
        this.callBackFunction = callBackFunction;
        mechDesign = design;

        text.text = mechDesign.DesignName;
    }

    public string GetDesignName()
    {
        return mechDesign.DesignName;
    }

    public void ClickButton()
    {
        callBackFunction(mechDesign.MechChassisDefinition, mechDesign.DesignName);

        AudioManager.Instance.PlayButtonClick(0);
    }
}
