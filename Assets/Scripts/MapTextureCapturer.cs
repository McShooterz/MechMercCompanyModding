using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapTextureCapturer : MonoBehaviour
{
#if UNITY_EDITOR
    [SerializeField]
    Camera targetCamera;

    [SerializeField]
    string mapName = "MapName";

    [SerializeField]
    int imageWidth = 2048;

    [SerializeField]
    int imageHeight = 2048;

    public void CaptureCameraImage()
    {
        if (targetCamera != null)
        {
            RenderTexture renderTexture = new RenderTexture(imageWidth, imageHeight, 24);
            targetCamera.targetTexture = renderTexture;
            Texture2D texture2D = new Texture2D(imageWidth, imageHeight, TextureFormat.RGB24, false);

            targetCamera.Render();

            RenderTexture.active = renderTexture;
            texture2D.ReadPixels(new Rect(0, 0, imageWidth, imageHeight), 0, 0);
            texture2D.Apply();

            string path = System.IO.Directory.GetParent(Application.dataPath).FullName + "/MapTextures/";

            System.IO.Directory.CreateDirectory(path);

            path = path + mapName + ".png";

            byte[] bytes = texture2D.EncodeToPNG();
            System.IO.File.WriteAllBytes(path, bytes);
            Debug.Log("Saved as: " + path);

            targetCamera.targetTexture = null;
            RenderTexture.active = null;
            DestroyImmediate(renderTexture);
        }
        else
        {
            Debug.LogWarning("Warning: No Target Camera assigned");
        }
    }
#endif
}
