using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MarketMechDataButton : MonoBehaviour
{
    [SerializeField]
    Image backgroundImage;

    [SerializeField]
    LayoutElement layoutElement;

    [SerializeField]
    Text mechNameText;

    [SerializeField]
    Text costText;

    MechData mechData;

    public delegate void CallBackFunction(MechData mechData, MarketMechDataButton marketMechDataButton);
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

    public void Initialize(MechData data, int cost, CallBackFunction function)
    {
        mechData = data;
        callBackFunction = function;

        mechNameText.text = mechData.MechChassis.GetDisplayName();
        costText.text = StaticHelper.FormatMoney(cost);
    }

    public void ClickButton()
    {
        callBackFunction(mechData, this);

        AudioManager.Instance.PlayButtonClick(0);
    }
}
