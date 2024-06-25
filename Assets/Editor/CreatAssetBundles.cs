using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Text;


public class CreatAssetBundles
{

    static Dictionary<string, string> bundles = new Dictionary<string, string>() 
    {
        { GameParametres.BundleNames.PREFAB_ENEMY, GameParametres.BundlePath.PREFAB_ENEMY },
        { GameParametres.BundleNames.PREFAB_COMBO, GameParametres.BundlePath.PREFAB_COMBO },
        { GameParametres.BundleNames.SFX, GameParametres.BundlePath.SFX },
    };

    [MenuItem("Tharle/Builld AssetBundles")]
    static void BuildAllAssetBundles()
    {
        List<AssetBundleBuild> assetBundleDefinitionList = new();
        foreach(string bundleName in bundles.Keys)
        {
            assetBundleDefinitionList.Add(CreateAssetBundle(bundleName, bundles[bundleName]));
        }

        // Create if not exist streaming Assets directory
        if (!Directory.Exists(Application.streamingAssetsPath))
        {
            Directory.CreateDirectory(Application.streamingAssetsPath);
        }

        // Build all bundles from 'BundleAssets' to streaming directory
        AssetBundleManifest manifest =  BuildPipeline.BuildAssetBundles(GameParametres.BundlePath.STREAMING_ASSETS, assetBundleDefinitionList.ToArray(), BuildAssetBundleOptions.None, EditorUserBuildSettings.activeBuildTarget);
        
        // Look at the results
        if (manifest != null)
        {
            foreach (var bundleName in manifest.GetAllAssetBundles())
            {
                string projectRelativePath = GameParametres.BundlePath.STREAMING_ASSETS + "/" + bundleName;
                Debug.Log($"Size of AssetBundle {projectRelativePath} is {((float)new FileInfo(projectRelativePath).Length / (1024)).ToString("0.00")} KB");
            }
        }
        else
        {
            Debug.Log("Build failed, see Console and Editor log for details");
        }
    }

    private static AssetBundleBuild CreateAssetBundle(string bundleName, string bundleUrl)
    {
        AssetBundleBuild ab = new();
        ab.assetBundleName = bundleName;
        ab.assetNames = RecursiveGetAllAssetsInDirectory(GameParametres.BundlePath.BUNDLE_ASSETS + bundleUrl).ToArray();
        return ab;
    }

    static List<string> RecursiveGetAllAssetsInDirectory(string path)
    {
        List<string> assets = new();
        // "Assets/BundleAssets/Sounds"
        foreach (string asset in Directory.GetFiles(path))
                assets.Add(asset);
        return assets;
    }
}
