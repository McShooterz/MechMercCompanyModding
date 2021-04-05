using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InstantActionMapMenu : MonoBehaviour
{
    [SerializeField]
    Transform content;

    [SerializeField]
    IndexButton indexButton;

    [SerializeField]
    IndexButton[] buttons;

    [SerializeField]
    Text mapNameText;

    [SerializeField]
    Image mapImage;

    [SerializeField]
    Text CoolingModifierValueText;

    [SerializeField]
    Color buttonColorDefault;

    [SerializeField]
    Color buttonColorHighlight;

    List<MapDefinition> mapDefinitions;

    [SerializeField]
    int selectedIndex = 0;

    public delegate void CallBackFunction(MapDefinition mapDefinition);
    public CallBackFunction callBackFunction;

    public void Initialize(CallBackFunction function, List<MapDefinition> definitions)
    {
        selectedIndex = 0;

        callBackFunction = function;

        mapDefinitions = definitions;

        for (int i = 1; i < buttons.Length; i++)
        {
            Destroy(buttons[i].gameObject);
        }

        if (mapDefinitions.Count > 0)
        {
            buttons = new IndexButton[mapDefinitions.Count];

            buttons[0] = indexButton;

            indexButton.gameObject.SetActive(true);
            indexButton.Initialize(0, mapDefinitions[0].GetDisplayName(), SelectMap);
            indexButton.BackgroundImage.color = buttonColorDefault;

            for (int i = 1; i < buttons.Length; i++)
            {
                GameObject indexButtonObject = Instantiate(indexButton.gameObject, content);

                IndexButton newIndexButton = indexButtonObject.GetComponent<IndexButton>();

                if (newIndexButton != null)
                {
                    newIndexButton.Initialize(i, mapDefinitions[i].GetDisplayName(), SelectMap);
                    buttons[i] = newIndexButton;
                }
                else if (indexButtonObject != null)
                {
                    Destroy(indexButtonObject);
                }
            }

            SelectMap(0);
        }
        else
        {
            indexButton.gameObject.SetActive(false);
            buttons = new IndexButton[0];
        }
    }

    public void SelectMap(int index)
    {
        buttons[selectedIndex].BackgroundImage.color = buttonColorDefault;

        selectedIndex = index;

        buttons[selectedIndex].BackgroundImage.color = buttonColorHighlight;

        MapDefinition mapDefinition = mapDefinitions[selectedIndex];

        if (mapDefinition != null)
        {
            mapNameText.text = mapDefinition.GetDisplayName();

            mapImage.overrideSprite = StaticHelper.GetSpriteUI(mapDefinition.GetMapTexture());

            if (mapDefinition.CoolingModifier < 0.0f)
            {
                CoolingModifierValueText.text = (mapDefinition.CoolingModifier * 100.0f).ToString("0.") + "%";
            }
            else
            {
                CoolingModifierValueText.text = "+" + (mapDefinition.CoolingModifier * 100.0f).ToString("0.") + "%";
            }
        }
        else
        {
            mapNameText.text = "";
            CoolingModifierValueText.text = "";
            mapImage.overrideSprite = null;
        }
    }

    public void SelectMap(MapDefinition mapDefinition)
    {
        for (int i = 0; i < mapDefinitions.Count; i++)
        {
            if (mapDefinition == mapDefinitions[i])
            {
                SelectMap(i);
                break;
            }
        }
    }

    public void ClickCancelButton()
    {
        gameObject.SetActive(false);

        AudioManager.Instance.PlayButtonClick(0);
    }

    public void ClickSelectButton()
    {
        if (mapDefinitions.Count > 0)
        {
            callBackFunction(mapDefinitions[selectedIndex]);
        }

        gameObject.SetActive(false);

        AudioManager.Instance.PlayButtonClick(0);
    }
}
