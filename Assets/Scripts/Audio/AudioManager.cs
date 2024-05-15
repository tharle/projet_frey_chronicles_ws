using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum EAudio
{
    Attack,
    MagicFire
}
public class AudioManager
{
    #region Singleton
    private static AudioManager m_Instance;
    public static AudioManager Instance {
        get
        {
            if (m_Instance == null) m_Instance = new AudioManager();

            return m_Instance;
        }
    }
    #endregion

    private Dictionary<EAudio, AudioClip> m_AudioClips;
    private Dictionary<EAudio, AudioSource> m_AudioSourcePlaying;

    private AudioPool m_AudioPool;

    public AudioManager()
    {
        m_AudioPool = new AudioPool();
        m_AudioSourcePlaying = new Dictionary<EAudio, AudioSource>();
        LoadAllAudioClips();
    }

    private void LoadAllAudioClips()
    {
        m_AudioClips = BundleLoader.Instance.LoadSFX();
    }

    public void Play(EAudio audioClipId, Vector3 soundPosition, bool isLooping = false)
    {
        AudioSource audioSource;
        // TODO: BUG: le son de la magie remplace celui de attack dans le dictionnaire...
        /*if (m_AudioSourcePlaying.ContainsKey(audioClipId))
        {
          audioSource = m_AudioSourcePlaying[audioClipId];
        }*/
        //else
        //{
        audioSource = m_AudioPool.GetAvailable();
        audioSource.clip = m_AudioClips[audioClipId];
        audioSource.transform.position = soundPosition;
        // m_AudioSourcePlaying.Add(audioClipId, audioSource);
        //}

        if (!audioSource.isPlaying) 
        {
            audioSource.Play();
            audioSource.loop = isLooping;
        }
    }


    public void Stop(EAudio audioClipId)
    {
        if (m_AudioSourcePlaying.ContainsKey(audioClipId)) m_AudioSourcePlaying[audioClipId].Stop();
    }
}
