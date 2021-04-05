using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MarketComponentButton : MonoBehaviour
{
    [SerializeField]
    Image backgroundImage;

    [SerializeField]
    LayoutElement layoutElement;

    [SerializeField]
    Text componentNameText;

    [SerializeField]
    Text countText;

    [SerializeField]
    Text costText;

    ComponentDefinition componentDefinition;

    public delegate void CallBackFunction(ComponentDefinition componentDefinition, MarketComponentButton marketComponentButton);
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

    public ComponentDefinition ComponentDefinition
    {
        get
        {
            return componentDefinition;
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

    public void Initialize(ComponentDefinition definition, int count, int cost, CallBackFunction function)
    {
        componentDefinition = definition;
        callBackFunction = function;

        componentNameText.text = componentDefinition.GetDisplayName();
        countText.text = count.ToString();
        costText.text = StaticHelper.FormatMoney(cost);
    }

    public void ClickButton()
    {
        callBackFunction(componentDefinition, this);

        AudioManager.Instance.PlayButtonClick(0);
    }
}
