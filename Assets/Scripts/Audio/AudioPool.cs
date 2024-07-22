using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class AudioPool
{
    private List<AudioSource> m_AudioPool = new List<AudioSource>();

    public AudioSource GetAvailable(Transform parent)
    {
        CleanAllDestroyed();
        AudioSource audio = m_AudioPool.Find(audio => !audio.isPlaying && !audio.gameObject.IsDestroyed());
        if (audio == null)
        {
            // Pas d'audio disponible
            GameObject go = new GameObject("Audio Source");
            go.transform.SetParent(parent);
            audio = go.AddComponent<AudioSource>();
            m_AudioPool.Add(audio);
        }

        return audio;

    }

    public void StopAllLooping()
    {
        CleanAllDestroyed();
        foreach (AudioSource audio in m_AudioPool)
        {
            if (audio.loop) audio.Stop();
        }
    }

    private void CleanAllDestroyed()
    {
        if (m_AudioPool.Count <= 0) return;

        for (int i = m_AudioPool.Count - 1; i >= 0; i--)
        {
            if (m_AudioPool[i].IsDestroyed()) m_AudioPool.RemoveAt(i);
        }
    }
}
