using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreditsScreen : MonoBehaviour
{
    [SerializeField]
    Text creditsText;

    [SerializeField]
    RectTransform creditsTextRectTransform;

    [SerializeField]
    Vector3 creditsStartingPosition;

    [SerializeField]
    float scrollSpeed = 30.0f;

    void Awake()
    {
        creditsStartingPosition = creditsTextRectTransform.position;

        creditsText.text = ResourceManager.Instance.GetCredits().GetDisplay();
    }

    // Update is called once per frame
    void Update()
    {
        creditsTextRectTransform.position += new Vector3(0.0f, scrollSpeed * Time.deltaTime, 0.0f);
    }

    void OnEnable()
    {
        creditsTextRectTransform.position = creditsStartingPosition;
    }

    public void ClickBackButton()
    {
        gameObject.SetActive(false);

        AudioManager.Instance.PlayButtonClick(0);
    }
}
