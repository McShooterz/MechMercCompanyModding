using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FactionLogoUI : MonoBehaviour
{
    [SerializeField]
    Image companyLogo1;

    [SerializeField]
    Image companyLogo2;

    public void SetFactionLogo(FactionLogo factionLogo)
    {
        if (factionLogo != null)
        {
            Sprite sprite = factionLogo.GetLogo1Sprite();

            if (sprite != null)
            {
                companyLogo1.enabled = true;
                companyLogo1.sprite = sprite;
                companyLogo1.color = factionLogo.Color1;
            }
            else
            {
                companyLogo1.enabled = false;
            }

            sprite = factionLogo.GetLogo2Sprite();

            if (sprite != null)
            {
                companyLogo2.enabled = true;
                companyLogo2.sprite = sprite;
                companyLogo2.color = factionLogo.Color2;
            }
            else
            {
                companyLogo2.enabled = false;
            }
        }
        else
        {
            companyLogo1.enabled = false;
            companyLogo2.enabled = false;
        }
    }
}
