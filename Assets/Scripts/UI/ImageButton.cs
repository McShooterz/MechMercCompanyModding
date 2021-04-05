using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageButton : MonoBehaviour
{
    [SerializeField]
    Image background;

    [SerializeField]
    Image image;

    [SerializeField]
    int index;

    public delegate void CallBackFunction(int index);
    public CallBackFunction callBackFunction;

    public Image Background
    {
        get
        {
            return background;
        }
    }

    public void Initialize(int index, Texture2D texture2D, CallBackFunction function)
    {
        this.index = index;

        if (texture2D != null)
        {
            image.color = Color.white;
            image.overrideSprite = Sprite.Create(texture2D, new Rect(0.0f, 0.0f, texture2D.width, texture2D.height), new Vector2(0.5f, 0.5f), 100.0f);
        }
        else
        {
            image.color = Color.clear;
        }

        callBackFunction = function;
    }

    public void ClickButton()
    {
        callBackFunction(index);

        AudioManager.Instance.PlayButtonClick(0);
    }
}
