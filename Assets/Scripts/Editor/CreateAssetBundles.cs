using UnityEngine;
using UnityEditor;
using System.IO;

public class CreateAssetBundles
{
    [MenuItem("Assets/Build AssetBundles")]
    static void BuildAllAssetBundles()
    {
        string assetBundleDirectory = Directory.GetParent(Application.dataPath).FullName + "/AssetBundles";

        Debug.Log("AssetBundle Directory: " + assetBundleDirectory);

        Directory.CreateDirectory(assetBundleDirectory);

        string[] files = Directory.GetFiles(assetBundleDirectory);

        // Delete old asset bundles
        for (int i = 0; i < files.Length; i++)
        {
            File.Delete(files[i]);
        }

        BuildPipeline.BuildAssetBundles(assetBundleDirectory, BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows);

        files = Directory.GetFiles(assetBundleDirectory);

        for (int i = 0; i < files.Length; i++)
        {
            string asset = files[i];

            if (!asset.Contains("."))
            {
                File.Move(asset, asset + ".assetBundle");
            }
            else
            {
                File.Delete(asset);
            }
        }
    }
}
