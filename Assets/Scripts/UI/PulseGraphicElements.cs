using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PulseGraphicElements : MonoBehaviour
{
    [SerializeField]
    Graphic[] targetGraphicElements;

    [SerializeField]
    float frequency;

    public float Frequency
    {
        set
        {
            frequency = value;

            if (frequency == 0.0f)
            {
                for (int i = 0; i < targetGraphicElements.Length; i++)
                {
                    Graphic targetGraphicElement = targetGraphicElements[i];

                    targetGraphicElement.color = new Color(targetGraphicElement.color.r, targetGraphicElement.color.g, targetGraphicElement.color.b, 1.0f);
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (frequency > 0.0f)
        {
            float alpha = Mathf.PingPong(Time.unscaledTime * frequency, 1.0f);

            for (int i = 0; i < targetGraphicElements.Length; i++)
            {
                Graphic targetGraphicElement = targetGraphicElements[i];

                targetGraphicElement.color = new Color(targetGraphicElement.color.r, targetGraphicElement.color.g, targetGraphicElement.color.b, alpha);
            }
        }
    }
}
