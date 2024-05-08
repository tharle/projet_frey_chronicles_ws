using TMPro;
using UnityEngine;

public class HUDGameManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI m_HPValue;
    [SerializeField] TextMeshProUGUI m_TPValue;
    [SerializeField] TextMeshProUGUI m_APValue;

    private void Awake()
    {
        SubscribeAllNotifyEvents();
    }

    private void SubscribeAllNotifyEvents()
    {
        GameEventSystem.Instance.SubscribeTo(EGameEvent.RefreshInfoHUD, OnRefreshInfoHUD);
    }

    private void OnRefreshInfoHUD(GameEventMessage message)
    {
        if(message.Contains<Player>(EGameEventMessage.Player, out Player player))
        {
            m_HPValue.text = (player.GetHPRatio() * 100).ToString("00") + "%";
            m_TPValue.text = (player.GetTPRatio() * 100).ToString("00") + "%";
            m_APValue.text = (player.GetAPRatio() * 100).ToString("00") + "%";
        }
    }
}
