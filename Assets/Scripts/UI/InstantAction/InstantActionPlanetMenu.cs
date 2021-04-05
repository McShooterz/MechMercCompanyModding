using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InstantActionPlanetMenu : MonoBehaviour
{
    [SerializeField]
    Transform content;

    [SerializeField]
    IndexButton indexButton;

    [SerializeField]
    IndexButton[] buttons;

    [SerializeField]
    Text planetNameText;

    [SerializeField]
    Text planetDescriptionText;

    [SerializeField]
    Color buttonColorDefault;

    [SerializeField]
    Color buttonColorHighlight;

    [SerializeField]
    Transform previewCameraTransform;

    [SerializeField]
    GameObject planetPreview = null;

    List<PlanetDefinition> planetDefinitions;

    [SerializeField]
    int selectedIndex = 0;

    public delegate void CallBackFunction(PlanetDefinition planetDefinition);
    public CallBackFunction callBackFunction;

    public void Initialize(CallBackFunction function, List<PlanetDefinition> definitions)
    {
        callBackFunction = function;

        planetDefinitions = definitions;

        if (planetDefinitions.Count > 0)
        {
            buttons = new IndexButton[planetDefinitions.Count];

            buttons[0] = indexButton;

            indexButton.Initialize(0, planetDefinitions[0].GetDisplayName(), SelectPlanet);

            for (int i = 1; i < buttons.Length; i++)
            {
                GameObject indexButtonObject = Instantiate(indexButton.gameObject, content);

                IndexButton newIndexButton = indexButtonObject.GetComponent<IndexButton>();

                if (newIndexButton != null)
                {
                    newIndexButton.Initialize(i, planetDefinitions[i].GetDisplayName(), SelectPlanet);
                    buttons[i] = newIndexButton;
                }
                else if (indexButtonObject != null)
                {
                    Destroy(indexButtonObject);
                }
            }

            SelectPlanet(0);
        }
        else
        {
            indexButton.gameObject.SetActive(false);
        }
    }

    public void SelectPlanet(int index)
    {
        buttons[selectedIndex].BackgroundImage.color = buttonColorDefault;

        selectedIndex = index;

        buttons[selectedIndex].BackgroundImage.color = buttonColorHighlight;

        PlanetDefinition planetDefinition = planetDefinitions[selectedIndex];

        planetNameText.text = planetDefinition.GetDisplayName();
        planetDescriptionText.text = planetDefinition.GetDescription();

        if (planetPreview != null)
        {
            Destroy(planetPreview);
        }

        planetPreview = planetDefinition.GetPrefab();

        if (planetPreview)
        {
            planetPreview = Instantiate(planetPreview, previewCameraTransform.transform);

            planetPreview.transform.localScale = new Vector3(planetDefinition.Scale, planetDefinition.Scale, planetDefinition.Scale);

            planetPreview.transform.localPosition = new Vector3(0f, 0f, 2.5f);

            planetPreview.AddComponent<ObjectRotator>().SetRotationRate(100.0f);
        }
    }

    public void SelectPlanet(PlanetDefinition planetDefinition)
    {
        for (int i = 0; i < planetDefinitions.Count; i++)
        {
            if (planetDefinition == planetDefinitions[i])
            {
                SelectPlanet(i);
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
        if (planetDefinitions.Count > 0)
        {
            callBackFunction(planetDefinitions[selectedIndex]);
        }

        gameObject.SetActive(false);

        AudioManager.Instance.PlayButtonClick(0);
    }
}
