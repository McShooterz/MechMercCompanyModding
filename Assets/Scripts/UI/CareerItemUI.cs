using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CareerItemUI : MonoBehaviour
{
    [SerializeField]
    CareerSelectionScreen careerSelectionScreen;

    [SerializeField]
    LayoutElement layoutElement;

    [SerializeField]
    Image pilotImage;

    [SerializeField]
    Image companyLogo1Image;

    [SerializeField]
    Image companyLogo2Image;

    [SerializeField]
    Text callsignText;

    [SerializeField]
    Text companyNameText;

    [SerializeField]
    Text statusText;

    [SerializeField]
    Text dateText;

    [SerializeField]
    Text fundsText;

    [SerializeField]
    Text reputationText;

    [SerializeField]
    Text infamyText;

    [SerializeField]
    Button playButton;

    [SerializeField]
    CareerSave careerSave;

    public LayoutElement LayoutElement { get => layoutElement; }

    public void SetCareer(CareerSave newCareerSave)
    {
        careerSave = newCareerSave;

        callsignText.text = careerSave.Callsign;
        companyNameText.text = careerSave.CompanyName;

        if (careerSave.Deceased)
        {
            statusText.color = Color.red;
            statusText.text = "Deceased";
            playButton.interactable = false;
        }
        else if (careerSave.Retired)
        {
            statusText.color = Color.yellow;
            statusText.text = "Retired";
            playButton.interactable = false;
        }
        else if (careerSave.Bankrupt)
        {
            statusText.color = Color.red;
            statusText.text = "Bankrupt";
            playButton.interactable = false;
        }
        else
        {
            statusText.color = Color.white;
            statusText.text = "Active";
            playButton.interactable = true;
        }

        dateText.text = careerSave.GameDate.Display;

        fundsText.text = StaticHelper.FormatMoney(careerSave.Funds);

        reputationText.text = careerSave.Reputation.ToString();

        infamyText.text = careerSave.Infamy.ToString();

        Sprite pilotImageSprite = careerSave.GetPlayerPilotSprite();

        if (pilotImageSprite != null)
        {
            pilotImage.overrideSprite = pilotImageSprite;
            pilotImage.color = Color.white;
        }
        else
        {
            pilotImage.color = Color.clear;
        }

        Sprite companyLogo = careerSave.FactionLogo.GetLogo1Sprite();

        if (companyLogo != null)
        {
            companyLogo1Image.sprite = companyLogo;
            companyLogo1Image.color = careerSave.FactionLogo.Color1;
        }
        else
        {
            companyLogo1Image.enabled = false;
        }

        companyLogo = careerSave.FactionLogo.GetLogo2Sprite();

        if (companyLogo != null)
        {
            companyLogo2Image.sprite = companyLogo;
            companyLogo2Image.color = careerSave.FactionLogo.Color2;
        }
        else
        {
            companyLogo2Image.enabled = false;
        }
    }

    public void ClickPlay()
    {
        AudioManager.Instance.PlayButtonClick(0);

        Career career = new Career(careerSave);

        GlobalDataManager.Instance.currentCareer = career;
        LoadingScreen.Instance.LoadScene(career.currentScreen);
    }

    public void ClickDelete()
    {
        AudioManager.Instance.PlayButtonClick(0);

        ResourceManager.Instance.DeleteCareer(careerSave.UniqueIdentifier);

        careerSelectionScreen.BuildCareerList();
    }
}
