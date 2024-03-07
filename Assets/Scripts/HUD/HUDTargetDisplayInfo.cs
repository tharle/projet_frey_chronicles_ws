using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HUDTargetDisplayInfo : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI m_TargetDamage;
    [SerializeField] protected TextMeshProUGUI m_TargetDescription;

    [SerializeField] private GameObject TargetDisplayInfo;

    private void Start()
    {
        TargetDisplayInfo.SetActive(false);
        SubscribeAll();

    }

    private void SubscribeAll()
    {
        SelectSphere.Instance.OnTargetSelected = NotifyShowSelectTarget;
        GameStateEvent.Instance.SubscribeTo(EGameState.Interaction, OnInterractionMode);
    }


    private void NotifyShowSelectTarget(ITarget target)
    {
        //TargetDisplayInfo.SetActive(true);
        m_TargetDamage.text = target != null ?  target.DisplayDamage(): "No target in range";
        m_TargetDescription.text = target != null? target.DisplayDescription() : "";
    }

    private void OnInterractionMode(bool inIntration)
    {
        TargetDisplayInfo.SetActive(inIntration);
    }
}
