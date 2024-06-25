﻿using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class ComboState : AGameState
{
    private bool m_Hit;
    private bool m_Timeout;
    private bool m_AttackWasPressed;
    private int m_ComboCounter;
    private Coroutine m_Coroutine;
    private ATargetController m_Target;
    public ComboState(GameStateController controller) : base(controller, EGameState.Combo)
    {
    }

    public override void OnEnter()
    {
        base.OnEnter();
        Debug.Log("Combo ENTER");
        SelectSphere.Instance.HideSphere();
        m_ComboCounter = 0;
        m_Hit = true;
        //m_Coroutine = m_Controller.StartCoroutine(DoComboRoutine());
        m_Timeout = false;
        GameEventSystem.Instance.SubscribeTo(EGameEvent.EnemyDie, OnEnemyDie);
        m_Target = DungeonTargetManager.Instance.TargetSelected;
        m_Controller.StartCoroutine(CastComboRoutine());
    }

    private void OnEnemyDie(GameEventMessage message)
    {
        m_Controller.ChangeState(EGameState.None);
    }

    public override void OnExit()
    {
        base.OnExit();
        GameEventSystem.Instance.TriggerEvent(EGameEvent.ComboInfoHUD, new GameEventMessage(EGameEventMessage.ComboValue, -1));
        GameEventSystem.Instance.UnsubscribeFrom(EGameEvent.EnemyDie, OnEnemyDie);
    }


    public override void UpdateState()
    {
        base.UpdateState();

        //m_Controller.ChangeState(EGameState.None);

        if (m_Timeout || !m_Target.IsAlive())
        {
            m_Controller.ChangeState(EGameState.None);
            return;
        }


        if(Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown((int)MouseButton.Left))
        {
            GameEventSystem.Instance.TriggerEvent(EGameEvent.ComboTimerToggle, new GameEventMessage(EGameEventMessage.ComboTimerToggle, false));
            if(m_Coroutine != null) m_Controller.StopCoroutine(m_Coroutine);
            if (!m_AttackWasPressed && m_Hit)
            {
                m_Hit = false;
                m_ComboCounter++;
                GameEventSystem.Instance.TriggerEvent(EGameEvent.ComboInfoHUD, new GameEventMessage(EGameEventMessage.ComboValue, m_ComboCounter));

                //m_Coroutine = m_Controller.StartCoroutine(DoComboRoutine());
                m_Controller.StartCoroutine(CastComboRoutine());
            }else if (!m_AttackWasPressed)
            {
                m_AttackWasPressed = true;
            }
            else if (m_Timeout && m_AttackWasPressed)
            {
                m_Controller.ChangeState(EGameState.None);
            }

        }
    }

    private IEnumerator CastComboRoutine()
    {
        // Animation
        PlayerAnimation.Instance.Attack();

        PlayerController.Instance.LookToTarget(m_Target);

        yield return new WaitForSeconds(0.3f);

        //Logic
        GameObject go = BundleLoader.Instance.Load<GameObject>(GameParametres.BundleNames.PREFAB_COMBO, "Attack");
        go.transform.position = PlayerController.Instance.PlayerHand.position;

        if(go.TryGetComponent<ProjectilCombo>(out ProjectilCombo projectil))
        {
            projectil.Lauch(m_Target, 5f, OnHit);
        }
    }

    private void OnHit(ATargetController target)
    {
        AudioManager.Instance.Play(EAudio.Attack, m_Controller.transform.position);
        GameEventSystem.Instance.TriggerEvent(EGameEvent.ComboDamageToEnemy, new GameEventMessage(EGameEventMessage.TargetController, target));

        if(!m_AttackWasPressed) m_Coroutine = m_Controller.StartCoroutine(DoComboRoutine());
        else m_Controller.ChangeState(EGameState.None);
    }

    private IEnumerator DoComboRoutine()
    {
        //m_Hit = false;
        //yield return new WaitForSeconds(Random.Range(0.5f, 2)); // TODO: Calculer à du HIT au ennemi
        m_Hit =true;
        m_AttackWasPressed = false;
        GameEventSystem.Instance.TriggerEvent(EGameEvent.ComboTimerToggle, new GameEventMessage(EGameEventMessage.ComboTimerToggle, m_Hit));
        yield return new WaitForSeconds(Random.Range(0.5f, 1)); // TODO: Calculer à partir de la TENSION
        m_Hit = false;
        GameEventSystem.Instance.TriggerEvent(EGameEvent.ComboTimerToggle, new GameEventMessage(EGameEventMessage.ComboTimerToggle, m_Hit));

        m_Timeout = true;
    }
}
