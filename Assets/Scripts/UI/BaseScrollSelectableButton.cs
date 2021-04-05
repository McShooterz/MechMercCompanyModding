using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class BaseScrollSelectableButton : MonoBehaviour
{
    [SerializeField]
    Text mainText;

    [SerializeField]
    Image backgroundImage;

    public delegate void CallBackFunction<T, U>(T element, U button);

    public Text MainText
    {
        get
        {
            return mainText;
        }
    }

    public Image BackgroundImage
    {
        get
        {
            return backgroundImage;
        }
    }

    public abstract void ClickButton();
}
