using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CompanyOverviewSubScreen : MonoBehaviour
{
    [SerializeField]
    InputField callsignInputField;

    [SerializeField]
    InputField companyInputField;

    [SerializeField]
    Toggle permadeathToggle;

    [SerializeField]
    Image playerPilotImage;

    [SerializeField]
    Image companyLogo1Image;

    [SerializeField]
    Image companyLogo2Image;

    [SerializeField]
    Slider color1RedSlider;

    [SerializeField]
    Slider color1GreenSlider;

    [SerializeField]
    Slider color1BlueSlider;

    [SerializeField]
    Text color1RedValueText;

    [SerializeField]
    Text color1GreenValueText;

    [SerializeField]
    Text color1BlueValueText;

    [SerializeField]
    Color companyLogoColor1 = Color.white;

    [SerializeField]
    Slider color2RedSlider;

    [SerializeField]
    Slider color2GreenSlider;

    [SerializeField]
    Slider color2BlueSlider;

    [SerializeField]
    Text color2RedValueText;

    [SerializeField]
    Text color2GreenValueText;

    [SerializeField]
    Text color2BlueValueText;

    [SerializeField]
    Color companyLogoColor2 = Color.white;

    [SerializeField]
    Text companyText;

    [SerializeField]
    int pilotImageIndex;

    [SerializeField]
    int logo1Index;

    [SerializeField]
    int logo2Index;

    Sprite[] pilotImageSprites;

    List<Texture2D> logoTextures;

    void OnEnable()
    {
        if (pilotImageSprites == null)
        {
            Texture2D[] pilotTextures = ResourceManager.Instance.GetPilotTextures();
            pilotImageSprites = new Sprite[pilotTextures.Length];

            for (int i = 0; i < pilotImageSprites.Length; i++)
            {
                pilotImageSprites[i] = StaticHelper.GetSpriteUI(pilotTextures[i]);
            }

            playerPilotImage.enabled = pilotImageSprites.Length > 0;
        }

        if (logoTextures == null)
        {
            logoTextures = new List<Texture2D>();
            logoTextures.Add(null);
            logoTextures.AddRange(ResourceManager.Instance.GetLogoTextures());
        }

        Career career = GlobalDataManager.Instance.currentCareer;

        callsignInputField.text = career.callsign;
        companyInputField.text = career.companyName;
        permadeathToggle.SetIsOnWithoutNotify(career.permadeath);

        pilotImageIndex = 0;
        logo1Index = 0;
        logo2Index = 0;

        for (int i = 0; i < pilotImageSprites.Length; i++)
        {
            if (pilotImageSprites[i].texture.name == career.playerPilotImageKey)
            {
                pilotImageIndex = i;
                break;
            }
        }

        playerPilotImage.sprite = pilotImageSprites[pilotImageIndex];

        for (int i = 1; i < logoTextures.Count; i++)
        {
            string logoTextureName = logoTextures[i].name;

            if (logoTextureName == career.factionLogo.Logo1)
            {
                logo1Index = i;
            }
            
            if (logoTextureName == career.factionLogo.Logo2)
            {
                logo2Index = i;
            }
        }

        if (logo1Index != 0)
        {
            companyLogo1Image.sprite = StaticHelper.GetSpriteUI(logoTextures[logo1Index]);
        }
        else
        {
            companyLogo1Image.enabled = false;
        }

        if (logo2Index != 0)
        {
            companyLogo2Image.sprite = StaticHelper.GetSpriteUI(logoTextures[logo2Index]);
        }
        else
        {
            companyLogo2Image.enabled = false;
        }

        companyLogoColor1 = career.factionLogo.Color1;
        companyLogo1Image.color = companyLogoColor1;

        companyLogoColor2 = career.factionLogo.Color2;
        companyLogo2Image.color = companyLogoColor2;

        color1RedSlider.SetValueWithoutNotify(companyLogoColor1.r);
        color1GreenSlider.SetValueWithoutNotify(companyLogoColor1.g);
        color1BlueSlider.SetValueWithoutNotify(companyLogoColor1.b);
        color2RedSlider.SetValueWithoutNotify(companyLogoColor2.r);
        color2GreenSlider.SetValueWithoutNotify(companyLogoColor2.g);
        color2BlueSlider.SetValueWithoutNotify(companyLogoColor2.b);

        color1RedValueText.text = companyLogoColor1.r.ToString("0.0#");
        color1GreenValueText.text = companyLogoColor1.g.ToString("0.0#");
        color1BlueValueText.text = companyLogoColor1.b.ToString("0.0#");
        color2RedValueText.text = companyLogoColor2.r.ToString("0.0#");
        color2GreenValueText.text = companyLogoColor2.g.ToString("0.0#");
        color2BlueValueText.text = companyLogoColor2.b.ToString("0.0#");

        System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();

        if (career.currentContract != null)
        {
            stringBuilder.AppendLine("Current Contract: " + career.currentContract.ContractDefinition.GetDisplayName());
        }
        else
        {
            stringBuilder.AppendLine("Current Contract: <color=red>None</color>");
        }

        stringBuilder.AppendLine();
        stringBuilder.AppendLine("Company Value: " + StaticHelper.FormatMoney(career.CompanyValue));

        stringBuilder.AppendLine("Weekly Expenses: " + StaticHelper.FormatMoney(career.WeeklyExpenses));

        stringBuilder.AppendLine();

        stringBuilder.AppendLine("Reputation: " + career.reputation);
        stringBuilder.AppendLine("Infamy: " + career.infamy);

        companyText.text = stringBuilder.ToString();
    }

    void OnDisable()
    {
        Career career = GlobalDataManager.Instance.currentCareer;

        if (callsignInputField.text.Length > 0)
            career.callsign = callsignInputField.text;

        if (companyInputField.text.Length > 0)
            career.companyName = companyInputField.text;

        career.permadeath = permadeathToggle.isOn;

        career.playerPilotImageKey = pilotImageSprites[pilotImageIndex].texture.name;

        FactionLogo factionLogo = new FactionLogo();

        factionLogo.Logo1 = logoTextures[logo1Index].name;

        if (logoTextures[logo2Index] != null)
        {
            factionLogo.Logo2 = logoTextures[logo2Index].name;
        }

        factionLogo.Color1 = companyLogoColor1;
        factionLogo.Color2 = companyLogoColor2;

        career.factionLogo = factionLogo;
    }

    public void ClickPilotChangeLeftButton()
    {
        if (pilotImageSprites.Length == 0)
            return;

        pilotImageIndex--;

        if (pilotImageIndex < 0)
        {
            pilotImageIndex = pilotImageSprites.Length - 1;
        }

        playerPilotImage.overrideSprite = pilotImageSprites[pilotImageIndex];

        AudioManager.Instance.PlayButtonClick(0);
    }

    public void ClickPilotChangeRightButton()
    {
        if (pilotImageSprites.Length == 0)
            return;

        pilotImageIndex++;

        if (pilotImageIndex == pilotImageSprites.Length)
        {
            pilotImageIndex = 0;
        }

        playerPilotImage.overrideSprite = pilotImageSprites[pilotImageIndex];

        AudioManager.Instance.PlayButtonClick(0);
    }

    public void ClickCompanyLogo1ChangeLeftButton()
    {
        logo1Index--;

        if (logo1Index < 1)
        {
            logo1Index = logoTextures.Count - 1;
        }

        companyLogo1Image.overrideSprite = StaticHelper.GetSpriteUI(logoTextures[logo1Index]);

        AudioManager.Instance.PlayButtonClick(0);
    }

    public void ClickCompanyLogo1ChangeRightButton()
    {
        logo1Index++;

        if (logo1Index == logoTextures.Count)
        {
            logo1Index = 1;
        }

        companyLogo1Image.overrideSprite = StaticHelper.GetSpriteUI(logoTextures[logo1Index]);

        AudioManager.Instance.PlayButtonClick(0);
    }

    public void ClickCompanyLogo2ChangeLeftButton()
    {
        logo2Index--;

        if (logo2Index < 0)
        {
            logo2Index = logoTextures.Count - 1;
        }

        if (logoTextures[logo2Index] != null)
        {
            companyLogo2Image.overrideSprite = StaticHelper.GetSpriteUI(logoTextures[logo2Index]);
            companyLogo2Image.enabled = true;
        }
        else
        {
            companyLogo2Image.overrideSprite = null;
            companyLogo2Image.enabled = false;
        }

        AudioManager.Instance.PlayButtonClick(0);
    }

    public void ClickCompanyLogo2ChangeRightButton()
    {
        logo2Index++;

        if (logo2Index == logoTextures.Count)
        {
            logo2Index = 0;
        }

        if (logoTextures[logo2Index] != null)
        {
            companyLogo2Image.overrideSprite = StaticHelper.GetSpriteUI(logoTextures[logo2Index]);
            companyLogo2Image.enabled = true;
        }
        else
        {
            companyLogo2Image.overrideSprite = null;
            companyLogo2Image.enabled = false;
        }

        AudioManager.Instance.PlayButtonClick(0);
    }

    public void ClickClearCompanyLogo2Button()
    {
        logo2Index = 0;

        companyLogo2Image.overrideSprite = null;
        companyLogo2Image.enabled = false;

        AudioManager.Instance.PlayButtonClick(0);
    }

    public void OnValueChangedColor1Red()
    {
        color1RedSlider.SetValueWithoutNotify((float)System.Math.Round(color1RedSlider.value, 2));

        color1RedValueText.text = color1RedSlider.value.ToString("0.0#");

        companyLogoColor1.r = color1RedSlider.value;

        companyLogo1Image.color = companyLogoColor1;
    }

    public void OnValueChangedColor1Green()
    {
        color1GreenSlider.SetValueWithoutNotify((float)System.Math.Round(color1GreenSlider.value, 2));

        color1GreenValueText.text = color1GreenSlider.value.ToString("0.0#");

        companyLogoColor1.g = color1GreenSlider.value;

        companyLogo1Image.color = companyLogoColor1;
    }

    public void OnValueChangedColor1Blue()
    {
        color1BlueSlider.SetValueWithoutNotify((float)System.Math.Round(color1BlueSlider.value, 2));

        color1BlueValueText.text = color1BlueSlider.value.ToString("0.0#");

        companyLogoColor1.b = color1BlueSlider.value;

        companyLogo1Image.color = companyLogoColor1;
    }

    public void OnValueChangedColor2Red()
    {
        color2RedSlider.SetValueWithoutNotify((float)System.Math.Round(color2RedSlider.value, 2));

        color2RedValueText.text = color2RedSlider.value.ToString("0.0#");

        companyLogoColor2.r = color2RedSlider.value;

        companyLogo2Image.color = companyLogoColor2;
    }

    public void OnValueChangedColor2Green()
    {
        color2GreenSlider.SetValueWithoutNotify((float)System.Math.Round(color2GreenSlider.value, 2));

        color2GreenValueText.text = color2GreenSlider.value.ToString("0.0#");

        companyLogoColor2.g = color2GreenSlider.value;

        companyLogo2Image.color = companyLogoColor2;
    }

    public void OnValueChangedColor2Blue()
    {
        color2BlueSlider.SetValueWithoutNotify((float)System.Math.Round(color2BlueSlider.value, 2));

        color2BlueValueText.text = color2BlueSlider.value.ToString("0.0#");

        companyLogoColor2.b = color2BlueSlider.value;

        companyLogo2Image.color = companyLogoColor2;
    }
}
