using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MechColorButton : MonoBehaviour
{
    [SerializeField]
    MechPaintScreen mechPaintScreen;

    [SerializeField]
    int colorIndex;

    [SerializeField]
    Image highlightImage;

    [SerializeField]
    Image colorImage;

    public Image HighlightImage { get { return highlightImage; } }

    public Image ColorImage { get { return colorImage; } }

    public void ClickButton()
    {
        mechPaintScreen.SelectColorIndex(colorIndex);

        AudioManager.Instance.PlayButtonClick(0);
    }
}
