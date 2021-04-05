using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ModInfo
{
    public string DisplayName = "";
    public string Description = "";
    public string Author = "";
    public string ModVersion = "";
    public string BaseGameVersion = "";
    public string PreviewImagePath = "";
    public ulong SteamPublishedID = 0;

    Sprite previewImage = null;

    string modPath = "";

    string modType = "";

    public string ModPath { get { return modPath; } }

    public string ModType { get { return modType; } }

    public Sprite PreviewImage
    {
        get
        {
            if (previewImage == null)
            {
                string path = ModPath + "/" + PreviewImagePath;

                if (File.Exists(path))
                {
                    Texture2D previewImageTexture = ResourceManager.GetTextureFromFile(path);

                    if (previewImageTexture != null)
                    {
                        previewImage = StaticHelper.GetSpriteUI(previewImageTexture);
                    }
                }
            }

            return previewImage;
        }
    }

    public void SetModPath(string path)
    {
        modPath = path;
    }

    public void SetModType(string type)
    {
        modType = type;
    }
}
