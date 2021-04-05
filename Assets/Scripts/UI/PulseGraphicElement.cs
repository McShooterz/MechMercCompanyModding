using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PulseGraphicElement : MonoBehaviour
{
    [SerializeField]
    Graphic targetGraphicElement;

    [SerializeField]
    float frequency;

    public Graphic TargetGraphicElement { get => targetGraphicElement; }

    public float Frequency
    {
        set
        {
            frequency = value;

            if (frequency == 0.0f)
            {
                targetGraphicElement.color = new Color(targetGraphicElement.color.r, targetGraphicElement.color.g, targetGraphicElement.color.b, 1.0f);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (frequency > 0.0f)
        {
            targetGraphicElement.color = new Color(targetGraphicElement.color.r, targetGraphicElement.color.g, targetGraphicElement.color.b, Mathf.PingPong(Time.unscaledTime * frequency, 1.0f));
        }
    }
}
