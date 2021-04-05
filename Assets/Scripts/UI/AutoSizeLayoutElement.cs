using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AutoSizeLayoutElement : MonoBehaviour
{
    [SerializeField]
    LayoutElement layoutElement;

    [SerializeField]
    float width;

    [SerializeField]
    float height;

    // Start is called before the first frame update
    void Start()
    {
        width = width / 1920.0f * Screen.width;
        height = height / 1080.0f * Screen.height;

        Destroy(this);
    }

    public void AutoFillLayoutElement()
    {
        layoutElement = GetComponent<LayoutElement>();

        if (layoutElement != null)
        {
            width = layoutElement.preferredWidth;
            height = layoutElement.preferredHeight;
        }
    }
}
