using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GenericDamageDisplay : BaseDamageDisplay
{
    [SerializeField]
    Slider healthBar;

    public void SetDisplay(float percentageValue)
    {
        healthBar.value = percentageValue;
        ColorBlock colorBlock = healthBar.colors;
        colorBlock.normalColor = GetArmorHealthColor(percentageValue);
        healthBar.colors = colorBlock;
    }
}
