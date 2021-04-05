using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingScreen : MonoBehaviour
{
    public static LoadingScreen Instance { get; private set; }

    [SerializeField]
    Image loadingScreenImage;

    [SerializeField]
    Slider progressBar;

    [SerializeField]
    Text progressText;

    [SerializeField]
    Text hintText;

    AsyncOperation asyncOperation;

    void Awake()
    {
        //Make a Singleton
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }

        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (asyncOperation != null)
        {
            progressBar.value = asyncOperation.progress;
            progressText.text = "Loading: " + Mathf.Round(asyncOperation.progress * 100f).ToString() + "%";
        }
        else
        {
            progressBar.value = 0f;
            progressText.text = "Not Loading!";
        }
    }

    void OnEnable()
    {
        AudioManager.Instance.StopPlayingMusic();

        if (ResourceManager.Instance != null)
        {
            Texture2D loadingScreenTexture = ResourceManager.Instance.GetRandomLoadingScreenTexture();

            if (loadingScreenTexture != null)
            {
                loadingScreenImage.sprite = StaticHelper.GetSpriteUI(loadingScreenTexture);
            }

            hintText.text = ResourceManager.Instance.GameConstants.GetRandomHint();
        }
        else
        {
            hintText.text = "";
        }
    }

    public void LoadScene(string sceneName)
    {
        gameObject.SetActive(true);

        Scene targetScene = SceneManager.GetSceneByName(sceneName);

        Time.timeScale = 0.0f;
        Cursor.lockState = CursorLockMode.None;

        if (targetScene != null)
        {
            asyncOperation = SceneManager.LoadSceneAsync(sceneName);
        }
        else
        {
            Debug.LogError("Error: Scene not found: " + sceneName);

            asyncOperation = SceneManager.LoadSceneAsync("MainMenu");
        }
    }
}
