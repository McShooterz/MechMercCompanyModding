using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MechPaintScheme
{
    public Color BaseColor = new Color(0.75f, 0.75f, 0.75f, 1.0f);

    public string PaintLayer1 = "";

    public Color PaintLayer1Color = Color.white;

    public string PaintLayer2 = "";

    public Color PaintLayer2Color = Color.white;

    public MechPaintScheme() { }

    public MechPaintScheme(Material mechMaterial)
    {
        if (mechMaterial != null)
        {
            BaseColor = mechMaterial.GetColor("_BaseColor");

            Texture texture = mechMaterial.GetTexture("_AlbedoLayer1");

            if (texture != null)
            {
                PaintLayer1 = texture.name;
                PaintLayer1Color = mechMaterial.GetColor("_Layer1Color");
            }
            else
            {
                PaintLayer1 = "";
            }

            texture = mechMaterial.GetTexture("_AlbedoLayer2");

            if (texture != null)
            {
                PaintLayer2 = texture.name;
                PaintLayer2Color = mechMaterial.GetColor("_Layer2Color");
            }
            else
            {
                PaintLayer2 = "";
            }
        }
    }

    public MechPaintScheme Clone()
    {
        return (MechPaintScheme)MemberwiseClone();
    }

    public Texture2D GetPaintLayer1()
    {
        if (PaintLayer1 != "")
        {
            return ResourceManager.Instance.GetMechSkinTexture(PaintLayer1);
        }

        return null;
    }

    public Texture2D GetPaintLayer2()
    {
        if (PaintLayer2 != "")
        {
            return ResourceManager.Instance.GetMechSkinTexture(PaintLayer2);
        }

        return null;
    }

    public void ApplyPropertiesToMaterial(Material mechMaterial)
    {
        mechMaterial.SetColor("_BaseColor", BaseColor);
        mechMaterial.SetTexture("_AlbedoLayer1", GetPaintLayer1());
        mechMaterial.SetColor("_Layer1Color", PaintLayer1Color);
        mechMaterial.SetTexture("_AlbedoLayer2", GetPaintLayer2());
        mechMaterial.SetColor("_Layer2Color", PaintLayer2Color);
    }
}
