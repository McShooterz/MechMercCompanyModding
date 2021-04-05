using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MechSkinButtonList : MonoBehaviour
{
    [SerializeField]
    Transform content;

    [SerializeField]
    ImageButton startingButton;

    [SerializeField]
    ImageButton[] mechSkinButtons;

    [SerializeField]
    GridLayoutGroup gridLayoutGroup;

    [SerializeField]
    int selectedIndex = 0;

    [SerializeField]
    float itemWidth;

    [SerializeField]
    Color defaultColor;

    [SerializeField]
    Color selectedColor;

    public delegate void CallBackFunction(int index);
    public CallBackFunction callBackFunction;

    void Awake()
    {
        itemWidth = itemWidth / 1920 * Screen.width;

        gridLayoutGroup.cellSize = new Vector2(itemWidth, itemWidth);
    }

    public void Initialize(Texture2D[] mechSkins, CallBackFunction function)
    {
        callBackFunction = function;

        mechSkinButtons = new ImageButton[mechSkins.Length];
        mechSkinButtons[0] = startingButton;
        mechSkinButtons[0].Initialize(0, mechSkins[0], SelectButton);

        for (int i = 1; i < mechSkinButtons.Length; i++)
        {
            mechSkinButtons[i] = Instantiate(startingButton, content).GetComponent<ImageButton>();
            mechSkinButtons[i].Initialize(i, mechSkins[i], SelectButton);
        }
    }

    public void ChangeSelectedButton(int index)
    {
        mechSkinButtons[selectedIndex].Background.color = defaultColor;

        selectedIndex = index;

        mechSkinButtons[selectedIndex].Background.color = selectedColor;
    }

    public void SelectButton(int targetIndex)
    {
        if (selectedIndex == targetIndex)
            return;

        if (mechSkinButtons.Length > 0)
        {
            targetIndex = Mathf.Clamp(targetIndex, 0, mechSkinButtons.Length - 1);

            ChangeSelectedButton(targetIndex);

            callBackFunction(selectedIndex);
        }
    }
}
