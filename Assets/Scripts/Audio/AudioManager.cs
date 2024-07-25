using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum EAudio
{
    Attack1,
    Attack2,
    Attack3,
    Complete,
    GameOver,
    MagicFire,
    MusicAmbient_1,
    MusicFight,
    DoorOpen1,
    DoorOpen2,
    DoorOpen3,
    DoorOpen4
}
public class AudioManager : MonoBehaviour
{
    private Dictionary<EAudio, AudioClip> m_AudioClips = new();
    private AudioPool m_AudioPool;

    private BundleLoader m_Loader;

    private static AudioManager m_Instance;
    public static AudioManager Instance
    {
        get
        {
            if (m_Instance == null)
            {
                GameObject go = new GameObject("AudioManager");
                go.AddComponent<AudioManager>();
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

        m_AudioPool = new AudioPool();
        m_Loader = BundleLoader.Instance;
        

        m_Instance = this;
    }

    public AudioSource Play(EAudio audioClipId)
    {
        return Play(audioClipId, Camera.main.transform.position);
    }

    public AudioSource Play(EAudio audioClipId, Vector3 soundPosition, bool isLooping = false, float volume = 1f)
    {
        if (m_AudioClips == null || m_AudioClips.Count <= 0) 
        {
            m_AudioClips = new();
            m_AudioClips = m_Loader.LoadSFX();
        } 

        Debug.Log($"AUDIO: {Enum.GetName(typeof(EAudio), audioClipId)}");
        AudioSource audioSource;
        audioSource = m_AudioPool.GetAvailable(transform);

        if (audioSource == null) {
            audioSource = new AudioSource();
            isLooping = false;
        } 

        audioSource.clip = m_AudioClips[audioClipId];
        audioSource.transform.position = soundPosition;
        audioSource.volume = volume;

        if (!audioSource.isPlaying)
        {
            audioSource.Play();
            audioSource.loop = isLooping;
        }

        return audioSource;
    }


    public void StopAllLooping()
    {
        m_AudioPool.StopAllLooping();
    }
}
