using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MarketMechDesignButton : MonoBehaviour
{
    [SerializeField]
    Image backgroundImage;

    [SerializeField]
    LayoutElement layoutElement;

    [SerializeField]
    Text mechNameText;

    [SerializeField]
    Text countText;

    [SerializeField]
    Text costText;

    MechDesign mechDesign;

    public delegate void CallBackFunction(MechDesign mechDesign, MarketMechDesignButton marketMechDesignButton);
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

    public void Initialize(MechDesign design, int count, int cost, CallBackFunction function)
    {
        mechDesign = design;
        callBackFunction = function;

        mechNameText.text = mechDesign.DesignName;
        countText.text = count.ToString();
        costText.text = StaticHelper.FormatMoney(cost);
    }

    public void ClickButton()
    {
        callBackFunction(mechDesign, this);

        AudioManager.Instance.PlayButtonClick(0);
    }
}
