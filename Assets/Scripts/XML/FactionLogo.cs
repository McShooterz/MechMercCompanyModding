using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FactionLogo
{
    public string Logo1 = "";

    public string Logo2 = "";

    public Color Color1 = Color.white;

    public Color Color2 = Color.white;

    public FactionLogo() { }

    public FactionLogo(FactionLogo factionLogo)
    {
        Logo1 = factionLogo.Logo1;
        Logo2 = factionLogo.Logo2;
        Color1 = factionLogo.Color1;
        Color2 = factionLogo.Color2;
    }

    public Texture2D GetLogo1Texture()
    {
        return ResourceManager.Instance.GetLogoTexture(Logo1);
    }

    public Texture2D GetLogo2Texture()
    {
        return ResourceManager.Instance.GetLogoTexture(Logo2);
    }

    public Sprite GetLogo1Sprite()
    {
        Texture2D texture2D = GetLogo1Texture();

        if (texture2D != null)
        {
            return StaticHelper.GetSpriteUI(texture2D);
        }

        return null;
    }

    public Sprite GetLogo2Sprite()
    {
        Texture2D texture2D = GetLogo2Texture();

        if (texture2D != null)
        {
            return StaticHelper.GetSpriteUI(texture2D);
        }

        return null;
    }
}
