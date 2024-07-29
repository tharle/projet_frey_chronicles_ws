using UnityEngine;

public class DoorController : MonoBehaviour
{

    [SerializeField] private GameObject m_Arrow;
    [SerializeField] private GameObject m_Block;
    [SerializeField] private Animator m_Animator;
    private bool m_IsOpen = false;
    private EAudio[] m_DoorAudios = { EAudio.DoorOpen1, EAudio.DoorOpen2, EAudio.DoorOpen3, EAudio.DoorOpen4 };

    private bool m_IsBattleMode = false;

    private void Start()
    {
        m_Arrow.SetActive(true);
        m_Block.SetActive(false);
        SubscribeAll();
    }

    private void SubscribeAll()
    {
        GameEventSystem.Instance.SubscribeTo(EGameEvent.BattleMode, OnBattleMode);
    }

    private void OnBattleMode(GameEventMessage message)
    {
        if (message.Contains<bool>(EGameEventMessage.Enter, out bool enter))
        {
            m_Arrow.SetActive(!enter);
            m_Block.SetActive(enter);
            m_IsBattleMode = enter;
        }
    }

    public void OpenDoor()
    {
        if (m_IsBattleMode || m_IsOpen) return;

        PlaySound();

        m_IsOpen = true;
        m_Animator.SetTrigger(GameParametres.AnimationDungeon.TRIGGER_OPEN_DOOR);
        Destroy(GetComponent<BoxCollider>(), 0.5f);
    }

    private void PlaySound()
    {
        int i = Random.Range(0, m_DoorAudios.Length);

        AudioManager.Instance.Play(m_DoorAudios[i], transform.position);
    }


}
