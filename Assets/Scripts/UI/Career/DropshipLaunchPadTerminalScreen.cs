using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DropshipLaunchPadTerminalScreen : MonoBehaviour
{
    [SerializeField]
    HomeBaseScreen homeBaseScreen;

    [SerializeField]
    DropshipOverviewSubScreen dropshipOverviewSubScreen;

    [SerializeField]
    DropshipLoadingSubScreen dropshipLoadingSubScreen;

    [SerializeField]
    Image overviewButtonBackground;

    [SerializeField]
    Image loadingButtonBackground;

    [SerializeField]
    Color activeColor;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ClickClose();
        }
    }

    void OnEnable()
    {
        dropshipOverviewSubScreen.gameObject.SetActive(false);
        dropshipLoadingSubScreen.gameObject.SetActive(false);

        overviewButtonBackground.color = activeColor;
        loadingButtonBackground.color = Color.white;

        dropshipOverviewSubScreen.gameObject.SetActive(true);
    }

    public void ClickOverviewButton()
    {
        dropshipLoadingSubScreen.gameObject.SetActive(false);
        dropshipOverviewSubScreen.gameObject.SetActive(true);

        overviewButtonBackground.color = activeColor;
        loadingButtonBackground.color = Color.white;

        AudioManager.Instance.PlayButtonClick(0);
    }

    public void ClickLoadingButton()
    {
        dropshipOverviewSubScreen.gameObject.SetActive(false);
        dropshipLoadingSubScreen.gameObject.SetActive(true);

        overviewButtonBackground.color = Color.white;
        loadingButtonBackground.color = activeColor;

        AudioManager.Instance.PlayButtonClick(0);
    }

    public void ClickDropshipsButton()
    {


        AudioManager.Instance.PlayButtonClick(0);
    }

    public void ClickMarketButton()
    {


        AudioManager.Instance.PlayButtonClick(0);
    }

    public void ClickClose()
    {
        homeBaseScreen.gameObject.SetActive(true);

        gameObject.SetActive(false);

        AudioManager.Instance.PlayButtonClick(0);
    }
}
