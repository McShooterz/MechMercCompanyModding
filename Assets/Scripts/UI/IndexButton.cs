using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IndexButton : MonoBehaviour
{
    [SerializeField]
    Text nameText;

    [SerializeField]
    Image backgroundImage;

    [SerializeField]
    int index;

    public delegate void CallBackFunction(int index);
    public CallBackFunction callBackFunction;

    public Image BackgroundImage
    {
        get
        {
            return backgroundImage;
        }
    }

    public void Initialize(int indexValue, string elementName, CallBackFunction function)
    {
        index = indexValue;

        nameText.text = elementName;

        callBackFunction = function;
    }

    public void ClickButton()
    {
        AudioManager.Instance.PlayButtonClick(0);

        callBackFunction(index);
    }
}
