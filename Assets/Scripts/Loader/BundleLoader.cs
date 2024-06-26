using UnityEngine;
using System.IO;
using System;
using Unity.VisualScripting;
using System.Collections.Generic;

public class BundleLoader: MonoBehaviour
{

    private static BundleLoader m_Instance;
    public static BundleLoader Instance
    {
        get { 
            if (m_Instance == null) 
            {
                GameObject go = new GameObject("BundleLoader");
                m_Instance = go.AddComponent<BundleLoader>();
            } 

            return m_Instance;
        }
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
            if(Enum.TryParse(clip.name, out EAudio audioId))
            {
                AudioClip newClip = Instantiate(clip);
                newClip.name = clip.name;
                audioClipsBundle.Add(audioId, newClip);
            }

        }

        return audioClipsBundle;
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

    /*

    

    public Dictionary<ERank, Sprite> LoadAllRankStamps()
    {
        Dictionary<ERank, Sprite> rankStampBundle = new();

        string[] spriteNames = { nameof(ERank.S), nameof(ERank.A), nameof(ERank.B), nameof(ERank.C), nameof(ERank.NONE) };
        List<Sprite> sprites = LoadAll<Sprite>(GameParameters.BundleNames.SPRITE_STAMP, true, spriteNames);
        foreach (Sprite sprite in sprites)
        {
            ERank rankId = ERank.NONE;
            switch (sprite.name)
            {
                case nameof(ERank.S):
                    rankId = ERank.S;
                    break;
                case nameof(ERank.A):
                    rankId = ERank.A;
                    break;
                case nameof(ERank.B):
                    rankId = ERank.B;
                    break;
                case nameof(ERank.C):
                    rankId = ERank.C;
                    break;
                default:
                    rankId = ERank.NONE;
                    break;
            }

            Sprite newSprite = Instantiate(sprite);
            newSprite.name = sprite.name;
            rankStampBundle.Add(rankId, newSprite);
        }

        return rankStampBundle;
    }*/
}
