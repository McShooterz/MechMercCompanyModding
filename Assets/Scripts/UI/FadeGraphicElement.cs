using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeGraphicElement : MonoBehaviour
{
    [SerializeField]
    Graphic targetGraphicElement;

    [SerializeField]
    float fadeRate;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        float alpha = targetGraphicElement.color.a - fadeRate * Time.deltaTime;

        if (alpha <= 0)
        {
            gameObject.SetActive(false);
            return;
        }

        targetGraphicElement.color = new Color(targetGraphicElement.color.r, targetGraphicElement.color.g, targetGraphicElement.color.b, alpha);
    }

    public void ResetFade()
    {
        gameObject.SetActive(true);

        targetGraphicElement.color = new Color(targetGraphicElement.color.r, targetGraphicElement.color.g, targetGraphicElement.color.b, 1f);
    }
}
