using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CommanderTerminalScreen : MonoBehaviour
{
    [SerializeField]
    HomeBaseScreen homeBaseScreen;

    [SerializeField]
    CompanyOverviewSubScreen companyOverviewSubScreen;

    [SerializeField]
    ContractsSubScreen contractsSubScreen;

    [SerializeField]
    PersonnelSubScreen personnelSubScreen;

    [SerializeField]
    FinancesSubScreen financesSubScreen;

    [SerializeField]
    NewsSubScreen newsSubScreen;

    [SerializeField]
    Image overviewButtonBackground;

    [SerializeField]
    Image contractsButtonBackground;

    [SerializeField]
    Image personnelButtonBackground;

    [SerializeField]
    Image financesButtonBackground;

    [SerializeField]
    Image loansButtonBackground;

    [SerializeField]
    Image newsButtonBackground;

    [SerializeField]
    Color activeColor;

    Image[] buttonBackgrounds;

    void Awake()
    {
        buttonBackgrounds = new Image[] { overviewButtonBackground, contractsButtonBackground, personnelButtonBackground, financesButtonBackground, loansButtonBackground, newsButtonBackground };
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ClickClose();
        }
    }

    void OnEnable()
    {     
        contractsSubScreen.gameObject.SetActive(false);
        personnelSubScreen.gameObject.SetActive(false);
        financesSubScreen.gameObject.SetActive(false);
        newsSubScreen.gameObject.SetActive(false);

        companyOverviewSubScreen.gameObject.SetActive(true);

        SelectionButtonBackground(overviewButtonBackground);
    }

    void SelectionButtonBackground(Image targetBackground)
    {
        for (int i = 0; i < buttonBackgrounds.Length; i++)
        {
            if (buttonBackgrounds[i] == targetBackground)
            {
                buttonBackgrounds[i].color = activeColor;
            }
            else
            {
                buttonBackgrounds[i].color = Color.white;
            }
        }
    }

    public void ClickOverviewButton()
    {
        SelectionButtonBackground(overviewButtonBackground);

        companyOverviewSubScreen.gameObject.SetActive(true);
        contractsSubScreen.gameObject.SetActive(false);
        personnelSubScreen.gameObject.SetActive(false);
        financesSubScreen.gameObject.SetActive(false);
        newsSubScreen.gameObject.SetActive(false);

        AudioManager.Instance.PlayButtonClick(0);
    }

    public void ClickContractsButton()
    {
        SelectionButtonBackground(contractsButtonBackground);

        companyOverviewSubScreen.gameObject.SetActive(false);
        contractsSubScreen.gameObject.SetActive(true);
        personnelSubScreen.gameObject.SetActive(false);
        financesSubScreen.gameObject.SetActive(false);
        newsSubScreen.gameObject.SetActive(false);

        AudioManager.Instance.PlayButtonClick(0);
    }

    public void ClickFinancesButton()
    {
        SelectionButtonBackground(financesButtonBackground);

        companyOverviewSubScreen.gameObject.SetActive(false);
        contractsSubScreen.gameObject.SetActive(false);
        personnelSubScreen.gameObject.SetActive(false);
        financesSubScreen.gameObject.SetActive(true);
        newsSubScreen.gameObject.SetActive(false);

        AudioManager.Instance.PlayButtonClick(0);
    }

    public void ClickPersonnelButton()
    {
        SelectionButtonBackground(personnelButtonBackground);

        companyOverviewSubScreen.gameObject.SetActive(false);
        contractsSubScreen.gameObject.SetActive(false);
        personnelSubScreen.gameObject.SetActive(true);
        financesSubScreen.gameObject.SetActive(false);
        newsSubScreen.gameObject.SetActive(false);

        AudioManager.Instance.PlayButtonClick(0);
    }

    public void ClickNewsButton()
    {
        SelectionButtonBackground(newsButtonBackground);

        companyOverviewSubScreen.gameObject.SetActive(false);
        contractsSubScreen.gameObject.SetActive(false);
        personnelSubScreen.gameObject.SetActive(false);
        financesSubScreen.gameObject.SetActive(false);
        newsSubScreen.gameObject.SetActive(true);

        AudioManager.Instance.PlayButtonClick(0);
    }

    public void ClickClose()
    {
        companyOverviewSubScreen.gameObject.SetActive(false);
        homeBaseScreen.gameObject.SetActive(true);

        gameObject.SetActive(false);

        AudioManager.Instance.PlayButtonClick(0);
    }
}
