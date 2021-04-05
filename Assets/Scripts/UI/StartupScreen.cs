using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartupScreen : MonoBehaviour
{
    [SerializeField]
    Slider progressBar;

    [SerializeField]
    Text progressText;

    AsyncOperation asyncOperation;

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1.0f;
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;

        gameObject.SetActive(true);

        asyncOperation = SceneManager.LoadSceneAsync("ModLoadScreen");
    }

    // Update is called once per frame
    void Update()
    {
        progressBar.value = asyncOperation.progress;
        progressText.text = "Loading: " + Mathf.Round(asyncOperation.progress * 100f).ToString() + "%";
    }
}
