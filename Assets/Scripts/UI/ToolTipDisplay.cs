using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToolTipDisplay : MonoBehaviour
{
    public static ToolTipDisplay Instance { private set; get; }

    [SerializeField]
    RectTransform rectTransform;

    [SerializeField]
    Text displayText;

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
            return;
        }

        if (rectTransform == null)
        {
            rectTransform = GetComponent<RectTransform>();
        }

        if (displayText == null)
        {
            displayText = GetComponentInChildren<Text>();
        }

        gameObject.SetActive(false);
    }

    void Update()
    {
        transform.position = Input.mousePosition;

        Vector3[] corners = new Vector3[4];
        rectTransform.GetWorldCorners(corners);

        float minY = Mathf.Min(corners[0].y, corners[1].y, corners[2].y, corners[3].y);
        float maxX = Mathf.Max(corners[0].x, corners[1].x, corners[2].x, corners[3].x);

        if (minY < 0f)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y + (minY * -1f), transform.position.z);
        }

        if (maxX > Screen.width)
        {
            transform.position = new Vector3(transform.position.x - (maxX - Screen.width), transform.position.y, transform.position.z);
        }
    }

    public void SetText(string text)
    {
        gameObject.SetActive(true);

        text = System.Text.RegularExpressions.Regex.Unescape(StaticHelper.SplitLineToMultiline(text, 40));

        displayText.text = text;

        LayoutRebuilder.ForceRebuildLayoutImmediate(rectTransform);
    }

    public void Clear()
    {
        gameObject.SetActive(false);
    }
}