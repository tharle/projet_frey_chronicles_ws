using UnityEngine;
using System.IO;
using System;
using Unity.VisualScripting;
using System.Collections.Generic;

public class BundleLoader : MonoBehaviour
{

    private static BundleLoader m_Instance;
    public static BundleLoader Instance
    {
        get
        {
            if (m_Instance == null)
            {
                GameObject go = new GameObject("BundleLoader");
                go.AddComponent<BundleLoader>();
            }

            return m_Instance;
        }
    }

    private void Awake()
    {
        if (m_Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        CleanData();

        m_Instance = this;
    }

    public T Load<T, E>(string bundleName, E type) where T : UnityEngine.Object where E : Enum
    {
        string assetName = Enum.GetName(typeof(E), type);

        return Load<T>(bundleName, assetName);
    }

    public T Load<T>(string bundleName, string assetName) where T : UnityEngine.Object
    {
        AssetBundle localAssetBundle = AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath, bundleName));

        if (localAssetBundle == null)
        {
            Debug.LogError("Failed to load AssetBundle!");
            return default(T);
        }

        T asset = Instantiate(localAssetBundle.LoadAsset<T>(assetName));
        asset.name = assetName;

        localAssetBundle.Unload(false);

        return asset;
    }

    public Dictionary<EAudio, AudioClip> LoadSFX()
    {
        Dictionary<EAudio, AudioClip> audioClipsBundle = new();

        string[] assetNames = Enum.GetNames(typeof(EAudio));
        List<AudioClip> audioClips = LoadAll<AudioClip>(GameParametres.BundleNames.SFX, false, assetNames);

        foreach (AudioClip clip in audioClips)
        {
            if (Enum.TryParse(clip.name, out EAudio audioId))
            {
                AudioClip newClip = Instantiate(clip);
                newClip.name = clip.name;
                audioClipsBundle.Add(audioId, newClip);
            }

        }

        return audioClipsBundle;
    }

    public List<T> LoadAll<T, E>(string bundleName, bool IsCallUnload = true) where T : UnityEngine.Object where E : Enum
    {
        string[] assetNames = Enum.GetNames(typeof(E));

        return LoadAll<T>(bundleName, IsCallUnload, assetNames);
    }

    private List<T> LoadAll<T>(string bundleName, bool IsCallUnload, params string[] assetNames) where T : UnityEngine.Object
    {
        AssetBundle localAssetBundle = AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath, bundleName));
        List<T> assets = new List<T>();

        if (localAssetBundle == null)
        {
            Debug.LogError("Failed to load AssetBundle!");
            return assets;
        }

        foreach (string assetName in assetNames)
        {
            T asset = localAssetBundle.LoadAsset<T>(assetName);
            assets.Add(asset);
        }

        if (IsCallUnload) localAssetBundle.Unload(false);

        return assets;
    }

    public void CleanData()
    {
        foreach (var assetBundle in AssetBundle.GetAllLoadedAssetBundles())
        {
            assetBundle.Unload(true);
        }
    }
}

