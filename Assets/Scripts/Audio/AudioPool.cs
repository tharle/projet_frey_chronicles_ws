using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPool
{
    private List<AudioSource> m_AudioPool = new List<AudioSource>();

    public AudioSource GetAvailable()
    {
        AudioSource audio = m_AudioPool.Find(audio => !audio.isPlaying);
        if(audio == null)
        {
            // Pas d'audio disponible
            GameObject go = new GameObject("Audio Source");
            audio = go.AddComponent<AudioSource>();
            m_AudioPool.Add(audio);
        }

        return audio;

    }
}
