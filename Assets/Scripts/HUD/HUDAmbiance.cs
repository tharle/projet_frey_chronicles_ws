using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDAmbiance : MonoBehaviour
{
    [SerializeField] GameObject m_BattleModeDisplay;
    [SerializeField] EAudio m_AmbianceMusic;
    private AudioSource m_MusicPlaying;
    private float m_Volume = 0.5f;

    private void Start()
    {
        GameEventSystem.Instance.SubscribeTo(EGameEvent.BattleMode, OnBattleMode);
        m_MusicPlaying = AudioManager.Instance.Play(EAudio.MusicAmbient_1, Camera.main.transform.position, true);
        m_MusicPlaying.volume = m_Volume;
    }

    private void OnBattleMode(GameEventMessage message)
    {
        if(message.Contains<bool>(EGameEventMessage.Enter, out bool enter))
        {
            m_BattleModeDisplay.SetActive(enter);

            if (m_MusicPlaying != null) {
                m_MusicPlaying?.Stop();
                if (enter) m_MusicPlaying = AudioManager.Instance.Play(EAudio.MusicFight, Camera.main.transform.position, true);
                else  m_MusicPlaying = AudioManager.Instance.Play(EAudio.MusicAmbient_1, Camera.main.transform.position, true);

                m_MusicPlaying.volume = m_Volume;
            }
        }
    }
}
