using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuScreen : MonoBehaviour
{
    [SerializeField]
    OptionsScreen optionsScreen;

    [SerializeField]
    EncyclopediaScreen encyclopediaScreen;

    [SerializeField]
    CreditsScreen creditsScreen;

    [SerializeField]
    GameObject changeLogWindow;

    [SerializeField]
    Text versionText;

    void Awake()
    {
        optionsScreen.gameObject.SetActive(false);
        encyclopediaScreen.gameObject.SetActive(false);
        creditsScreen.gameObject.SetActive(false);
        changeLogWindow.SetActive(false);

        GlobalDataManager.Instance.currentCareer = new Career();
    }

    void Start ()
    {
        versionText.text = Application.version + " - Unity " + Application.unityVersion;
        //versionText.text = ResourceManager.Instance.version;
    }

    void OnEnable()
    {
        Time.timeScale = 1.0f;
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;

#if UNITY_EDITOR
        Cursor.lockState = CursorLockMode.None;
#endif
    }

    public void ClickCareerButton()
    {
        AudioManager.Instance.PlayButtonClick(0);

        LoadingScreen.Instance.LoadScene("CareerSelection");
    }

    public void ClickInstantActionButton()
    {
        AudioManager.Instance.PlayButtonClick(0);

        LoadingScreen.Instance.LoadScene("InstantAction");
    }

    public void ClickOptionsButton()
    {
        gameObject.SetActive(false);
        optionsScreen.gameObject.SetActive(true);

        AudioManager.Instance.PlayButtonClick(0);
    }

    public void ClickEncyclopediaButton()
    {
        encyclopediaScreen.gameObject.SetActive(true);

        AudioManager.Instance.PlayButtonClick(0);
    }

    public void ClickCreditsButton()
    {
        creditsScreen.gameObject.SetActive(true);

        AudioManager.Instance.PlayButtonClick(0);
    }

    public void ClickQuitButton()
    {
        AudioManager.Instance.PlayButtonClick(1);

        Application.Quit();
    }

    public void ClickDiscordButton()
    {
        AudioManager.Instance.PlayButtonClick(1);

        Application.OpenURL("https://discord.gg/WX2BgP9");
    }

    public void ClickTrelloButton()
    {
        AudioManager.Instance.PlayButtonClick(1);

        Application.OpenURL("https://trello.com/b/YROgJVLd/mech-merc-company-development");
    }

    public void ClickChangeLogButton()
    {
        changeLogWindow.SetActive(true);

        AudioManager.Instance.PlayButtonClick(0);
    }

    public void ClickChangeLogCloseButton()
    {
        changeLogWindow.SetActive(false);

        AudioManager.Instance.PlayButtonClick(0);
    }
}
