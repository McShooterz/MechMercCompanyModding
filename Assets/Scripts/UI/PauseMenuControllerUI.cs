using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuControllerUI : MonoBehaviour
{
    [SerializeField]
    GameObject playerHUD;

    [SerializeField]
    OptionsScreen optionsScreen;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            playerHUD.SetActive(true);
            gameObject.SetActive(false);
            return;
        }
    }

    void OnEnable()
    {
        Time.timeScale = 0.0f;
        AudioListener.pause = true;
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;

#if UNITY_EDITOR
        Cursor.lockState = CursorLockMode.None;
#endif
    }

    public void ButtonClickResume()
    {
        AudioManager.Instance.PlayButtonClick(0);

        playerHUD.SetActive(true);
        gameObject.SetActive(false);
    }

    public void ButtonClickOptions()
    {
        if (optionsScreen != null)
        {
            AudioManager.Instance.PlayButtonClick(0);

            optionsScreen.gameObject.SetActive(true);
            gameObject.SetActive(false);
        }
    }

    public void ButtonClickRestartMission()
    {
        AudioManager.Instance.PlayButtonClick(0);

        MapDefinition mapDefinition = GlobalData.Instance.MissionSetup.MapDefinition;

        if (mapDefinition != null)
        {
            LoadingScreen.Instance.LoadScene(mapDefinition.Scene);
        }
    }

    public void ButtonClickAbortMission()
    {
        AudioManager.Instance.PlayButtonClick(0);


    }

    public void ButtonClickEmergencyEject()
    {
        AudioManager.Instance.PlayButtonClick(0);


    }

    public void ButtonClickExitMission()
    {
        AudioManager.Instance.PlayButtonClick(0);

        LoadingScreen.Instance.LoadScene(GlobalDataManager.Instance.backSceneName);
    }

    public void ButtonClickMainMenu()
    {
        AudioManager.Instance.PlayButtonClick(0);

        LoadingScreen.Instance.LoadScene("MainMenu");
    }

    public void ButtonClickQuitApplication()
    {
        AudioManager.Instance.PlayButtonClick(0);

        Application.Quit();
    }
}
