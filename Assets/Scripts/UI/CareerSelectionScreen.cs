using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CareerSelectionScreen : MonoBehaviour
{
    [SerializeField]
    GameObject createCareerWindow;

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
    Color companyLogo1Color = Color.white;

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
    Color companyLogo2Color = Color.white;

    [SerializeField]
    Button createCreateButton; 

    [SerializeField]
    Transform content;

    [SerializeField]
    CareerItemUI firstCareerItem;

    [SerializeField]
    List<CareerItemUI> careerItemUIs = new List<CareerItemUI>();

    Sprite[] pilotImageSprites;

    List<Texture2D> logoTextures = new List<Texture2D>();

    int pilotImageIndex = 0;
    int logo1Index = 0;
    int logo2Index = 0;

    void Awake()
    {
        createCareerWindow.SetActive(false);

        firstCareerItem.LayoutElement.preferredWidth = firstCareerItem.LayoutElement.preferredWidth / 1920f * Screen.width;
        firstCareerItem.LayoutElement.preferredHeight = firstCareerItem.LayoutElement.preferredHeight / 1080 * Screen.height;

        careerItemUIs.Add(firstCareerItem);
    }

    void Start()
    {
        BuildCareerList();

        Texture2D[] pilotTextures = ResourceManager.Instance.GetPilotTextures();
        pilotImageSprites = new Sprite[pilotTextures.Length];

        for (int i = 0; i < pilotImageSprites.Length; i++)
        {
            pilotImageSprites[i] = StaticHelper.GetSpriteUI(pilotTextures[i]);
        }

        if (pilotImageSprites.Length > 0)
        {
            playerPilotImage.color = Color.white;
        }

        logoTextures.Add(null);
        logoTextures.AddRange(ResourceManager.Instance.GetLogoTextures());
    }

    void OnEnable()
    {
        Time.timeScale = 1.0f;
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }

    public void BuildCareerList()
    {
        List<CareerSave> careers = ResourceManager.Instance.GetCareers();

        while (careerItemUIs.Count < careers.Count)
        {
            CareerItemUI careerItemUI = Instantiate(firstCareerItem.gameObject, content).GetComponent<CareerItemUI>();

            careerItemUIs.Add(careerItemUI);
        }

        for (int i = 0; i < careerItemUIs.Count; i++)
        {
            if (i < careers.Count)
            {
                careerItemUIs[i].gameObject.SetActive(true);

                careerItemUIs[i].SetCareer(careers[i]);
            }
            else
            {
                careerItemUIs[i].gameObject.SetActive(false);
            }
        }
    }

    public void ClickCreateNewCareer()
    {
        createCareerWindow.SetActive(true);

        callsignInputField.text = "";
        companyInputField.text = "";
        createCreateButton.interactable = false;

        permadeathToggle.SetIsOnWithoutNotify(false);

        pilotImageIndex = 0;
        logo1Index = 1;
        logo2Index = 0;

        if (pilotImageSprites.Length > 0)
        {
            playerPilotImage.overrideSprite = pilotImageSprites[0];
        }

        companyLogo1Color = Color.white;
        companyLogo2Color = Color.white;

        color1RedSlider.SetValueWithoutNotify(1.0f);
        color1GreenSlider.SetValueWithoutNotify(1.0f);
        color1BlueSlider.SetValueWithoutNotify(1.0f);
        color2RedSlider.SetValueWithoutNotify(1.0f);
        color2GreenSlider.SetValueWithoutNotify(1.0f);
        color2BlueSlider.SetValueWithoutNotify(1.0f);

        color1RedValueText.text = "1.0";
        color1GreenValueText.text = "1.0";
        color1BlueValueText.text = "1.0";
        color2RedValueText.text = "1.0";
        color2GreenValueText.text = "1.0";
        color2BlueValueText.text = "1.0";

        companyLogo1Image.overrideSprite = StaticHelper.GetSpriteUI(logoTextures[1]);
        companyLogo1Image.color = Color.white;

        companyLogo2Image.overrideSprite = null;
        companyLogo2Image.color = Color.white;
        companyLogo2Image.enabled = false;

        AudioManager.Instance.PlayButtonClick(0);
    }

    public void ClickMainMenu()
    {
        AudioManager.Instance.PlayButtonClick(0);

        LoadingScreen.Instance.LoadScene("MainMenu");

        AudioManager.Instance.PlayButtonClick(0);
    }

    public void ClickCancel()
    {
        createCareerWindow.SetActive(false);

        AudioManager.Instance.PlayButtonClick(0);
    }

    public void ClickCreateCareer()
    {
        string time = System.DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss");
        int index = time.IndexOf(" ");
        time = time.Remove(index, 1).Insert(index, "_");
        time = time.Replace("/", string.Empty);
        time = time.Replace(" ", string.Empty);
        time = time.Replace(':', '_');

        //print(time);

        Career newCareer = new Career(time);

        newCareer.uniqueIdentifier = time;
        newCareer.callsign = callsignInputField.text;
        newCareer.companyName = companyInputField.text;

        if (pilotImageSprites.Length > 0)
        {
            newCareer.playerPilotImageKey = pilotImageSprites[pilotImageIndex].texture.name;
        }

        newCareer.gameDate = ResourceManager.Instance.GameConstants.CareerStartingDate;
        newCareer.funds = ResourceManager.Instance.GameConstants.StartingCareerFunds;

        newCareer.permadeath = permadeathToggle.isOn;

        newCareer.factionLogo.Color1 = companyLogo1Color;
        newCareer.factionLogo.Color2 = companyLogo2Color;

        newCareer.factionLogo.Logo1 = logoTextures[logo1Index].name;

        if (logoTextures[logo2Index] != null)
        {
            newCareer.factionLogo.Logo2 = logoTextures[logo2Index].name;
        }

        MechData playerStartingMech = new MechData(ResourceManager.Instance.GetMechDesign("Anvil", "Anvil Prime"));

        newCareer.AddMechNew(playerStartingMech);
        newCareer.DropshipMechs = new MechData[] { playerStartingMech, null, null, null, null, null, null, null, null, null, null, null };
        newCareer.DutyRosterMechPlayer = playerStartingMech;

        foreach (string componentKey in ResourceManager.Instance.GameConstants.CareerStartingComponents)
        {
            ComponentDefinition componentDefinition = ResourceManager.Instance.GetComponentDefinition(componentKey);

            if (componentDefinition != null)
            {
                newCareer.inventory.AddComponent(componentDefinition);
            }
        }

        ResourceManager.Instance.StoreCareer(newCareer.CareerSave);

        BuildCareerList();

        createCareerWindow.SetActive(false);

        AudioManager.Instance.PlayButtonClick(0);
    }

    public void OnValueChangedInputField()
    {
        if (callsignInputField.text == "" || companyInputField.text == "")
        {
            createCreateButton.interactable = false;
        }
        else
        {
            createCreateButton.interactable = true;
        }
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

        companyLogo1Color.r = color1RedSlider.value;

        companyLogo1Image.color = companyLogo1Color;
    }

    public void OnValueChangedColor1Green()
    {
        color1GreenSlider.SetValueWithoutNotify((float)System.Math.Round(color1GreenSlider.value, 2));

        color1GreenValueText.text = color1GreenSlider.value.ToString("0.0#");

        companyLogo1Color.g = color1GreenSlider.value;

        companyLogo1Image.color = companyLogo1Color;
    }

    public void OnValueChangedColor1Blue()
    {
        color1BlueSlider.SetValueWithoutNotify((float)System.Math.Round(color1BlueSlider.value, 2));

        color1BlueValueText.text = color1BlueSlider.value.ToString("0.0#");

        companyLogo1Color.b = color1BlueSlider.value;

        companyLogo1Image.color = companyLogo1Color;
    }

    public void OnValueChangedColor2Red()
    {
        color2RedSlider.SetValueWithoutNotify((float)System.Math.Round(color2RedSlider.value, 2));

        color2RedValueText.text = color2RedSlider.value.ToString("0.0#");

        companyLogo2Color.r = color2RedSlider.value;

        companyLogo2Image.color = companyLogo2Color;
    }

    public void OnValueChangedColor2Green()
    {
        color2GreenSlider.SetValueWithoutNotify((float)System.Math.Round(color2GreenSlider.value, 2));

        color2GreenValueText.text = color2GreenSlider.value.ToString("0.0#");

        companyLogo2Color.g = color2GreenSlider.value;

        companyLogo2Image.color = companyLogo2Color;
    }

    public void OnValueChangedColor2Blue()
    {
        color2BlueSlider.SetValueWithoutNotify((float)System.Math.Round(color2BlueSlider.value, 2));

        color2BlueValueText.text = color2BlueSlider.value.ToString("0.0#");

        companyLogo2Color.b = color2BlueSlider.value;

        companyLogo2Image.color = companyLogo2Color;
    }
}
