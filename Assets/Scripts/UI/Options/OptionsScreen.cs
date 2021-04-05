using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsScreen : MonoBehaviour
{
    [SerializeField]
    GameObject backButtonTarget;

    [SerializeField]
    GameOptionsUI gameOptionsUI;

    [SerializeField]
    AudioOptionsUI audioOptionsUI;

    [SerializeField]
    GraphicOptionsUI graphicOptionsUI;

    [SerializeField]
    ControlOptionsUI controlOptionsUI;

    void Awake()
    {
        gameOptionsUI.gameObject.SetActive(true);
        audioOptionsUI.gameObject.SetActive(false);
        graphicOptionsUI.gameObject.SetActive(false);
        controlOptionsUI.gameObject.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !controlOptionsUI.HasKeyBindingsOpen)
        {
            Close();
        }
    }

    void OnEnable()
    {
        gameOptionsUI.gameObject.SetActive(true);
        audioOptionsUI.gameObject.SetActive(false);
        graphicOptionsUI.gameObject.SetActive(false);
        controlOptionsUI.gameObject.SetActive(false);
    }

    void Close()
    {
        if (backButtonTarget != null)
        {
            ResourceManager.Instance.SaveAudioConfig();
            ResourceManager.Instance.SaveGameOptionsConfig();
            ResourceManager.Instance.SaveGraphicsConfig();
            ResourceManager.Instance.SaveControlConfig();

            gameOptionsUI.SaveCareerOptions();

            gameObject.SetActive(false);
            backButtonTarget.SetActive(true);
        }
        else
        {
            Debug.LogError("Error: No back target assigned");
        }
    }

    public void ButtonClickBack()
    {
        Close();

        AudioManager.Instance.PlayButtonClick(0);
    }

    public void ButtonClickGameOptions()
    {
        gameOptionsUI.gameObject.SetActive(true);
        audioOptionsUI.gameObject.SetActive(false);
        graphicOptionsUI.gameObject.SetActive(false);
        controlOptionsUI.gameObject.SetActive(false);

        AudioManager.Instance.PlayButtonClick(0);
    }

    public void ButtonClickAudioOptions()
    {
        gameOptionsUI.gameObject.SetActive(false);
        audioOptionsUI.gameObject.SetActive(true);
        graphicOptionsUI.gameObject.SetActive(false);
        controlOptionsUI.gameObject.SetActive(false);

        AudioManager.Instance.PlayButtonClick(0);
    }

    public void ButtonClickGraphicOptions()
    {
        gameOptionsUI.gameObject.SetActive(false);
        audioOptionsUI.gameObject.SetActive(false);
        graphicOptionsUI.gameObject.SetActive(true);
        controlOptionsUI.gameObject.SetActive(false);

        AudioManager.Instance.PlayButtonClick(0);
    }

    public void ButtonClickControlOptions()
    {
        gameOptionsUI.gameObject.SetActive(false);
        audioOptionsUI.gameObject.SetActive(false);
        graphicOptionsUI.gameObject.SetActive(false);
        controlOptionsUI.gameObject.SetActive(true);

        AudioManager.Instance.PlayButtonClick(0);
    }
}
