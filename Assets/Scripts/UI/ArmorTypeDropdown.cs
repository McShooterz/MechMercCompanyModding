using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ArmorTypeDropdown : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    SectionHeaderOneSided sectionHeader;

    [SerializeField]
    Button mainButton;

    [SerializeField]
    Text mainButtonText;

    [SerializeField]
    Button[] optionsButtons;

    void Awake()
    {
        for (int i = 0; i < optionsButtons.Length; i++)
        {
            optionsButtons[i].gameObject.SetActive(false);
        }
    }

    public void ClickMainButton()
    {
        ToggleOptions();

        AudioManager.Instance.PlayButtonClick(0);
    }

    public void SetLabel(ArmorType armorType)
    {
        switch (armorType)
        {
            case ArmorType.composite:
                {
                    ColorBlock colorBlock = mainButton.colors;
                    colorBlock.normalColor = Color.yellow;
                    mainButton.colors = colorBlock;
                    mainButtonText.color = Color.black;
                    mainButtonText.text = "Composite Armor";
                    break;
                }
            case ArmorType.reactive:
                {
                    ColorBlock colorBlock = mainButton.colors;
                    colorBlock.normalColor = Color.green;
                    mainButton.colors = colorBlock;
                    mainButtonText.color = Color.white;
                    mainButtonText.text = "Reactive Armor";
                    break;
                }
            case ArmorType.thermal:
                {
                    ColorBlock colorBlock = mainButton.colors;
                    colorBlock.normalColor = Color.red;
                    mainButton.colors = colorBlock;
                    mainButtonText.color = Color.white;
                    mainButtonText.text = "Thermal Armor";
                    break;
                }
            case ArmorType.stealth:
                {
                    ColorBlock colorBlock = mainButton.colors;
                    colorBlock.normalColor = new Color(0.334f, 0.334f, 0.334f);
                    mainButton.colors = colorBlock;
                    mainButtonText.color = Color.white;
                    mainButtonText.text = "Stealth Armor";
                    break;
                }
            default:
                {
                    ColorBlock colorBlock = mainButton.colors;
                    colorBlock.normalColor = new Color(0.85f, 0.85f, 0.85f);
                    mainButton.colors = colorBlock;
                    mainButtonText.color = Color.black;
                    mainButtonText.text = "Standard Armor";
                    break;
                }
        }
    }

    public void ClickStandardArmorButton()
    {
        ToggleOptions();
        SetLabel(ArmorType.standard);
        sectionHeader.ChangeArmorType(ArmorType.standard, 0);
        AudioManager.Instance.PlayButtonClick(0);
    }

    public void ClickCompositeArmorButton()
    {
        ToggleOptions();
        SetLabel(ArmorType.composite);
        sectionHeader.ChangeArmorType(ArmorType.composite, 0);
        AudioManager.Instance.PlayButtonClick(0);
    }

    public void ClickThermalArmorButton()
    {
        ToggleOptions();
        SetLabel(ArmorType.thermal);
        sectionHeader.ChangeArmorType(ArmorType.thermal, 0);
        AudioManager.Instance.PlayButtonClick(0);
    }

    public void ClickReactiveArmorButton()
    {
        ToggleOptions();
        SetLabel(ArmorType.reactive);
        sectionHeader.ChangeArmorType(ArmorType.reactive, 0);
        AudioManager.Instance.PlayButtonClick(0);
    }

    public void ClickStealthArmorButton()
    {
        ToggleOptions();
        SetLabel(ArmorType.stealth);
        sectionHeader.ChangeArmorType(ArmorType.stealth, 0);
        AudioManager.Instance.PlayButtonClick(0);
    }

    void ToggleOptions()
    {
        for (int i = 0; i < optionsButtons.Length; i++)
        {
            Button optionButton = optionsButtons[i];
            optionButton.gameObject.SetActive(!optionButton.gameObject.activeInHierarchy);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        switch (sectionHeader.ArmorType)
        {
            case ArmorType.standard:
                {
                    ToolTipDisplay.Instance.SetText(ResourceManager.Instance.GetLocalization("ToolTipArmorStandard"));
                    break;
                }
            case ArmorType.composite:
                {
                    ToolTipDisplay.Instance.SetText(ResourceManager.Instance.GetLocalization("ToolTipArmorComposite"));
                    break;
                }
            case ArmorType.reactive:
                {
                    ToolTipDisplay.Instance.SetText(ResourceManager.Instance.GetLocalization("ToolTipArmorReactive"));
                    break;
                }
            case ArmorType.thermal:
                {
                    ToolTipDisplay.Instance.SetText(ResourceManager.Instance.GetLocalization("ToolTipArmorThermal"));
                    break;
                }
            case ArmorType.stealth:
                {
                    ToolTipDisplay.Instance.SetText(ResourceManager.Instance.GetLocalization("ToolTipArmorStealth"));
                    break;
                }
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ToolTipDisplay.Instance.Clear();
    }
}
