using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CareerPauseMenu : MonoBehaviour
{
    [SerializeField]
    GameObject backTarget;

    [SerializeField]
    OptionsScreen optionsScreen;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ClickCloseButton();
        }
    }

    public void ClickCloseButton()
    {
        AudioManager.Instance.PlayButtonClick(0);

        gameObject.SetActive(false);

        backTarget.SetActive(true);
    }

    public void ClickOptionsButton()
    {
        AudioManager.Instance.PlayButtonClick(0);

        gameObject.SetActive(false);

        optionsScreen.gameObject.SetActive(true);
    }

    public void ClickMainMenuButton()
    {
        AudioManager.Instance.PlayButtonClick(0);

        ResourceManager.Instance.StoreCareer(GlobalDataManager.Instance.currentCareer.CareerSave);

        LoadingScreen.Instance.LoadScene("MainMenu");
    }

    public void ClickExitToDesktop()
    {
        AudioManager.Instance.PlayButtonClick(1);

        ResourceManager.Instance.StoreCareer(GlobalDataManager.Instance.currentCareer.CareerSave);

        Application.Quit();
    }
}
