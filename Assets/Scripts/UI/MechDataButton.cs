using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MechDataButton : MonoBehaviour
{
    [SerializeField]
    Image backgroundImage;

    [SerializeField]
    LayoutElement layoutElement;

    [SerializeField]
    Text mechNameText;

    MechData mechData;

    public delegate void CallBackFunction(MechData mechData, MechDataButton mechDataButton);
    public CallBackFunction callBackFunction;

    public Image BackgroundImage
    {
        get
        {
            return backgroundImage;
        }
    }

    public LayoutElement LayoutElement
    {
        get
        {
            return layoutElement;
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

    public void Initialize(MechData data, CallBackFunction function)
    {
        mechData = data;
        callBackFunction = function;
    }

    public void UpdateButton(MechData data)
    {
        mechData = data;
    }

    public void ClickButton()
    {
        callBackFunction(mechData, this);

        AudioManager.Instance.PlayButtonClick(0);
    }
}
