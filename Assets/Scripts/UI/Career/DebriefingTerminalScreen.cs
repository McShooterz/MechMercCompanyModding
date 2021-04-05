using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebriefingTerminalScreen : MonoBehaviour
{
    [SerializeField]
    DebriefingOverviewSubScreen debriefingOverviewSubScreen;

    [SerializeField]
    SalvageSubScreen salvageSubScreen;

    [SerializeField]
    PilotReportSubScreen pilotReportSubScreen;

    [SerializeField]
    MechReportSubScreen mechReportSubScreen;

    [SerializeField]
    AftermathSubScreen aftermathSubScreen;

    [SerializeField]
    Button salvageButton;

    [SerializeField]
    Image overviewButtonBackground;

    [SerializeField]
    Image salvageButtonBackground;

    [SerializeField]
    Image pilotReportButtonBackground;

    [SerializeField]
    Image mechReportButtonBackground;

    [SerializeField]
    Image aftermathButtonBackground;

    [SerializeField]
    Color activeColor;

    void Awake()
    {
        debriefingOverviewSubScreen.gameObject.SetActive(true);
        salvageSubScreen.gameObject.SetActive(false);
        pilotReportSubScreen.gameObject.SetActive(false);
        mechReportSubScreen.gameObject.SetActive(false);
        aftermathSubScreen.gameObject.SetActive(false);

        overviewButtonBackground.color = activeColor;
        salvageButtonBackground.color = Color.white;
        pilotReportButtonBackground.color = Color.white;
        mechReportButtonBackground.color = Color.white;
        aftermathButtonBackground.color = Color.white;

        Career career = GlobalDataManager.Instance.currentCareer;

        salvageButton.interactable = career.lastMissionSuccessful;
    }

    void OnEnable()
    {
        Time.timeScale = 1.0f;
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }

    public void ClickOverviewButton()
    {
        AudioManager.Instance.PlayButtonClick(0);

        debriefingOverviewSubScreen.gameObject.SetActive(true);
        salvageSubScreen.gameObject.SetActive(false);
        pilotReportSubScreen.gameObject.SetActive(false);
        mechReportSubScreen.gameObject.SetActive(false);
        aftermathSubScreen.gameObject.SetActive(false);

        overviewButtonBackground.color = activeColor;
        salvageButtonBackground.color = Color.white;
        pilotReportButtonBackground.color = Color.white;
        mechReportButtonBackground.color = Color.white;
        aftermathButtonBackground.color = Color.white;
    }

    public void ClickSalvageButton()
    {
        AudioManager.Instance.PlayButtonClick(0);

        debriefingOverviewSubScreen.gameObject.SetActive(false);
        salvageSubScreen.gameObject.SetActive(true);
        pilotReportSubScreen.gameObject.SetActive(false);
        mechReportSubScreen.gameObject.SetActive(false);
        aftermathSubScreen.gameObject.SetActive(false);

        overviewButtonBackground.color = Color.white;
        salvageButtonBackground.color = activeColor;
        pilotReportButtonBackground.color = Color.white;
        mechReportButtonBackground.color = Color.white;
        aftermathButtonBackground.color = Color.white;
    }

    public void ClickPilotReportButton()
    {
        AudioManager.Instance.PlayButtonClick(0);

        debriefingOverviewSubScreen.gameObject.SetActive(false);
        salvageSubScreen.gameObject.SetActive(false);
        pilotReportSubScreen.gameObject.SetActive(true);
        mechReportSubScreen.gameObject.SetActive(false);
        aftermathSubScreen.gameObject.SetActive(false);

        overviewButtonBackground.color = Color.white;
        salvageButtonBackground.color = Color.white;
        pilotReportButtonBackground.color = activeColor;
        mechReportButtonBackground.color = Color.white;
        aftermathButtonBackground.color = Color.white;
    }

    public void ClickMechReportButton()
    {
        AudioManager.Instance.PlayButtonClick(0);

        debriefingOverviewSubScreen.gameObject.SetActive(false);
        salvageSubScreen.gameObject.SetActive(false);
        pilotReportSubScreen.gameObject.SetActive(false);
        mechReportSubScreen.gameObject.SetActive(true);
        aftermathSubScreen.gameObject.SetActive(false);

        overviewButtonBackground.color = Color.white;
        salvageButtonBackground.color = Color.white;
        pilotReportButtonBackground.color = Color.white;
        mechReportButtonBackground.color = activeColor;
        aftermathButtonBackground.color = Color.white;
    }

    public void ClickAftermathButton()
    {
        AudioManager.Instance.PlayButtonClick(0);

        debriefingOverviewSubScreen.gameObject.SetActive(false);
        salvageSubScreen.gameObject.SetActive(false);
        pilotReportSubScreen.gameObject.SetActive(false);
        mechReportSubScreen.gameObject.SetActive(false);
        aftermathSubScreen.gameObject.SetActive(true);

        overviewButtonBackground.color = Color.white;
        salvageButtonBackground.color = Color.white;
        pilotReportButtonBackground.color = Color.white;
        mechReportButtonBackground.color = Color.white;
        aftermathButtonBackground.color = activeColor;
    }
}
