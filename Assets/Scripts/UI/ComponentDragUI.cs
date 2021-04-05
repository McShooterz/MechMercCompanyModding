using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ComponentDragUI : MonoBehaviour
{
    [SerializeField]
    RectTransform backgroundRectTransform;

    [SerializeField]
    Image backgroundImage;

    [SerializeField]
    Text componentNameText;

    public RectTransform BackgroundRectTransform
    {
        get
        {
            return backgroundRectTransform;
        }
    }
    public Image BackgroundImage
    {
        get
        {
            return backgroundImage;
        }
    }
    public Text ComponentNameText
    {
        get
        {
            return componentNameText;
        }
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Input.mousePosition;
    }
}
