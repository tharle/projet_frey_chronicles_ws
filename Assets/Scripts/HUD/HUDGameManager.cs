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
        PlayerController.Instance.OnNotifyInfoPlayer += OnNotifyInfoPlayer;
    }

    private void OnNotifyInfoPlayer(Player player)
    {
        m_HPValue.text = (player.GetHPRatio() * 100).ToString("00");
        m_TPValue.text = (player.GetTPRatio() * 100).ToString("00");
        m_APValue.text = (player.GetAPRatio() * 100).ToString("00");
    }
}
