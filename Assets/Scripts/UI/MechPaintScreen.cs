using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MechPaintScreen : MonoBehaviour
{
    [SerializeField]
    GameObject backTarget;

    [SerializeField]
    UnitCustomizationScreen unitCustomizationScreen;

    [SerializeField]
    MechColorButton[] mechColorButtons;

    [SerializeField]
    Slider colorRedSlider;

    [SerializeField]
    Slider colorGreenSlider;

    [SerializeField]
    Slider colorBlueSlider;

    [SerializeField]
    Slider colorAlphaSlider;

    [SerializeField]
    Text colorRedValueText;

    [SerializeField]
    Text colorGreenValueText;

    [SerializeField]
    Text colorBlueValueText;

    [SerializeField]
    Text colorAlphaValueText;

    [SerializeField]
    MechSkinButtonList mechSkinButtonList;

    [SerializeField]
    Color[] mechPaintColors = new Color[3];

    [SerializeField]
    Color defaultColor;

    [SerializeField]
    Color selectedColor;

    [SerializeField]
    int selectedColorIndex = 0;

    [SerializeField]
    int selectedPaintLayer1 = 0;

    [SerializeField]
    int selectedPaintLayer2 = 0;

    [SerializeField]
    bool holdRotateLeft;

    [SerializeField]
    bool holdRotateRight;

    Texture2D[] mechSkins;

    Material currentMechMaterial;

    MechMetaController currentMechMetaController;

    void Awake()
    {
        gameObject.SetActive(false);
    }

    void Start()
    {
        SelectColorIndex(0);
    }

    void Update()
    {
        if (holdRotateLeft || Input.GetKey(KeyCode.A))
        {
            currentMechMetaController.gameObject.transform.RotateAround(currentMechMetaController.gameObject.transform.position, Vector3.up, 100f * Time.unscaledDeltaTime);
        }

        if (holdRotateRight || Input.GetKey(KeyCode.D))
        {
            currentMechMetaController.gameObject.transform.RotateAround(currentMechMetaController.gameObject.transform.position, Vector3.up, -100f * Time.unscaledDeltaTime);
        }
    }

    public void Initialize(string mechChassisDefinition, MechMetaController mechMetaController, Material mechMaterial)
    {
        currentMechMetaController = mechMetaController;
        currentMechMaterial = mechMaterial;

        List<Texture2D> mechSkinList = new List<Texture2D>();

        if (currentMechMaterial != null)
        {
            mechPaintColors[0] = currentMechMaterial.GetColor("_BaseColor");
            mechPaintColors[1] = currentMechMaterial.GetColor("_Layer1Color");
            mechPaintColors[2] = currentMechMaterial.GetColor("_Layer2Color");
        }

        SetButtonColor(0, mechPaintColors[0]);
        SetButtonColor(1, mechPaintColors[1]);
        SetButtonColor(2, mechPaintColors[2]);

        mechSkinList.Add(null);

        mechSkinList.AddRange(ResourceManager.Instance.GetUniqueMechSkinList(mechChassisDefinition));
        mechSkinList.AddRange(ResourceManager.Instance.GetUniversalMechSkinList());

        mechSkins = mechSkinList.ToArray();

        mechSkinButtonList.Initialize(mechSkins, SelectPaintIndex);

        string paintLayer1Name = "";
        string paintLayer2Name = "";

        Texture2D layer1Texture = null;
        Texture2D layer2Texture = null;

        if (currentMechMaterial != null)
        {
            layer1Texture = currentMechMaterial.GetTexture("_AlbedoLayer1") as Texture2D;
            layer2Texture = currentMechMaterial.GetTexture("_AlbedoLayer2") as Texture2D;
        }

        if (layer1Texture != null)
        {
            paintLayer1Name = layer1Texture.name;
        }

        if (layer2Texture != null)
        {
            paintLayer2Name = layer2Texture.name;
        }

        for (int i = 1; i < mechSkins.Length; i++)
        {
            if (mechSkins[i].name == paintLayer1Name)
            {
                selectedPaintLayer1 = i;
            }

            if (mechSkins[i].name == paintLayer2Name)
            {
                selectedPaintLayer2 = i;
            }
        }

        mechSkinButtonList.ChangeSelectedButton(selectedPaintLayer1);
    }

    public MechPaintScheme GetMechPaintScheme()
    {
        return new MechPaintScheme(currentMechMaterial);
    }

    void SetSlidersColor(Color targetColor)
    {
        colorRedSlider.SetValueWithoutNotify(targetColor.r);
        colorGreenSlider.SetValueWithoutNotify(targetColor.g);
        colorBlueSlider.SetValueWithoutNotify(targetColor.b);
        colorAlphaSlider.SetValueWithoutNotify(targetColor.a);

        colorRedValueText.text = colorRedSlider.value.ToString("0.0#");
        colorGreenValueText.text = colorGreenSlider.value.ToString("0.0#");
        colorBlueValueText.text = colorBlueSlider.value.ToString("0.0#");
        colorAlphaValueText.text = colorAlphaSlider.value.ToString("0.0#");
    }

    void SetButtonColor(int index, Color targetColor)
    {
        mechColorButtons[index].ColorImage.color = new Color(targetColor.r, targetColor.g, targetColor.b, 1.0f);
    }

    public void SelectColorIndex(int index)
    {
        selectedColorIndex = index;

        for (int i = 0; i < mechColorButtons.Length; i++)
        {
            if (i == selectedColorIndex)
            {
                mechColorButtons[i].HighlightImage.color = selectedColor;

                SetSlidersColor(mechPaintColors[i]);
            }
            else
            {
                mechColorButtons[i].HighlightImage.color = defaultColor;
            }
        }

        switch (selectedColorIndex)
        {
            case 0:
                {
                    mechSkinButtonList.gameObject.SetActive(false);
                    break;
                }
            case 1:
                {
                    mechSkinButtonList.gameObject.SetActive(true);
                    mechSkinButtonList.ChangeSelectedButton(selectedPaintLayer1);
                    break;
                }
            case 2:
                {
                    mechSkinButtonList.gameObject.SetActive(true);
                    mechSkinButtonList.ChangeSelectedButton(selectedPaintLayer2);
                    break;
                }
        }
    }

    public void SelectPaintIndex(int index)
    {
        if (currentMechMaterial != null)
        {
            if (selectedColorIndex == 1)
            {
                currentMechMaterial.SetTexture("_AlbedoLayer1", mechSkins[index]);
                selectedPaintLayer1 = index;
            }
            else
            {
                currentMechMaterial.SetTexture("_AlbedoLayer2", mechSkins[index]);
                selectedPaintLayer2 = index;
            }
        }
    }

    public void ClickBackButton()
    {
        backTarget.SetActive(true);
        gameObject.SetActive(false);

        AudioManager.Instance.PlayButtonClick(0);
    }

    public void ClickStartRotateRightButton()
    {
        holdRotateRight = true;

        AudioManager.Instance.PlayButtonClick(0);
    }

    public void ClickStartRotateLeftButton()
    {
        holdRotateLeft = true;

        AudioManager.Instance.PlayButtonClick(0);
    }

    public void StopRotateRight()
    {
        holdRotateRight = false;
    }

    public void StopRotateLeft()
    {
        holdRotateLeft = false;
    }

    public void OnValueChangedColor()
    {
        colorRedSlider.SetValueWithoutNotify((float)System.Math.Round(colorRedSlider.value, 2));
        colorGreenSlider.SetValueWithoutNotify((float)System.Math.Round(colorGreenSlider.value, 2));
        colorBlueSlider.SetValueWithoutNotify((float)System.Math.Round(colorBlueSlider.value, 2));
        colorAlphaSlider.SetValueWithoutNotify((float)System.Math.Round(colorAlphaSlider.value, 2));

        Color targetColor = new Color(colorRedSlider.value, colorGreenSlider.value, colorBlueSlider.value, colorAlphaSlider.value);

        mechPaintColors[selectedColorIndex] = targetColor;
        SetButtonColor(selectedColorIndex, mechPaintColors[selectedColorIndex]);

        colorRedValueText.text = colorRedSlider.value.ToString("0.0#");
        colorGreenValueText.text = colorGreenSlider.value.ToString("0.0#");
        colorBlueValueText.text = colorBlueSlider.value.ToString("0.0#");
        colorAlphaValueText.text = colorAlphaSlider.value.ToString("0.0#");

        if (currentMechMaterial != null)
        {
            switch (selectedColorIndex)
            {
                case 0:
                    {
                        currentMechMaterial.SetColor("_BaseColor", targetColor);
                        break;
                    }
                case 1:
                    {
                        currentMechMaterial.SetColor("_Layer1Color", targetColor);
                        break;
                    }
                case 2:
                    {
                        currentMechMaterial.SetColor("_Layer2Color", targetColor);
                        break;
                    }
            }
        }
    }
}