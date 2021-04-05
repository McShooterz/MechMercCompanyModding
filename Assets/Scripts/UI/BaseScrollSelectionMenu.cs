using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseScrollSelectionMenu : MonoBehaviour
{
    [SerializeField]
    protected Color buttonColorDefault;

    [SerializeField]
    protected Color buttonColorSelected;

    [SerializeField]
    protected Transform content;

    [SerializeField]
    protected BaseScrollSelectableButton firstButton;

    [SerializeField]
    protected List<BaseScrollSelectableButton> buttonsList = new List<BaseScrollSelectableButton>();

    public delegate void CallBackFunction<T>(T selectedElement);

    protected BaseScrollSelectableButton selectedButton;

    public void BuildButtonList<T, U>(T[] elements)
    {
        if (elements.Length > 0)
        {
            SelectElement(elements[0], firstButton);
            firstButton.gameObject.SetActive(true);
            buttonsList.Add(firstButton);

            for (int i = 1; i < elements.Length; i++)
            {
                GameObject createdButtonObject = Instantiate(firstButton.gameObject, content);

                U createdButton = createdButtonObject.GetComponent<U>();

                if (createdButton != null)
                {
                    buttonsList.Add(createdButton as BaseScrollSelectableButton);
                    (createdButton as BaseScrollSelectableButton).BackgroundImage.color = buttonColorDefault;
                }
                else if (createdButtonObject != null)
                {
                    Destroy(createdButtonObject);
                }
            }

            InitializeButtons(elements);
        }
        else
        {
            firstButton.gameObject.SetActive(false);
        }
    }

    protected void SelectButton(BaseScrollSelectableButton targetButton)
    {
        if (targetButton == selectedButton)
            return;

        if (selectedButton != null)
        {
            selectedButton.BackgroundImage.color = buttonColorDefault;
        }

        selectedButton = targetButton;

        selectedButton.BackgroundImage.color = buttonColorSelected;
    }

    protected abstract void InitializeButtons<T>(T[] elements);

    public abstract void SelectElement<T>(T element, BaseScrollSelectableButton targetButton);

    public void ClickCancelButton()
    {
        gameObject.SetActive(false);

        AudioManager.Instance.PlayButtonClick(0);
    }

    public abstract void ClickSelectButton();
}
