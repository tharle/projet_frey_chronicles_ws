using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class ComboState : AGameState
{
    private bool m_Hit;
    private bool m_Timeout;
    private int m_ComboCounter;
    private Coroutine m_Coroutine;
    public ComboState(GameStateController controller) : base(controller, EGameState.Combo)
    {
    }

    public override void OnEnter()
    {
        base.OnEnter();
        Debug.Log("Combo ENTER");
        SelectSphere.Instance.HideSphere();
        m_ComboCounter = 1;
        m_Hit = true;
        m_Coroutine = m_Controller.StartCoroutine(DoComboRoutine());
        m_Timeout = false;
    }

    public override void OnExit()
    {
        base.OnExit();
        GameEventSystem.Instance.TriggerEvent(EGameEvent.ComboInfoHUD, new GameEventMessage(EGameEventMessage.ComboValue, -1));
    }


    public override void UpdateState()
    {
        base.UpdateState();

        //m_Controller.ChangeState(EGameState.None);

        if (m_Timeout)
        {
            m_Controller.ChangeState(EGameState.None);
            return;
        }


        if(Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown((int)MouseButton.Left))
        {
            GameEventSystem.Instance.TriggerEvent(EGameEvent.ComboTimerToggle, new GameEventMessage(EGameEventMessage.ComboTimerToggle, false));
            m_Controller.StopCoroutine(m_Coroutine);
            if (m_Hit)
            {
                m_ComboCounter++;
                AudioManager.Instance.Play(EAudio.Attack, m_Controller.transform.position);
                GameEventSystem.Instance.TriggerEvent(EGameEvent.ComboDamageToEnemy, new GameEventMessage(EGameEventMessage.DamageElemental, EElemental.Fire));
                GameEventSystem.Instance.TriggerEvent(EGameEvent.ComboInfoHUD, new GameEventMessage (EGameEventMessage.ComboValue, m_ComboCounter));
                
                m_Coroutine = m_Controller.StartCoroutine(DoComboRoutine());
            }
            else 
            {
                m_Controller.ChangeState(EGameState.None);
            }

        }
    }

    private IEnumerator DoComboRoutine()
    {
        m_Hit = false;
        yield return new WaitForSeconds(Random.Range(0.5f, 2)); // TODO: Calculer à du HIT au ennemi
        m_Hit =true;
        GameEventSystem.Instance.TriggerEvent(EGameEvent.ComboTimerToggle, new GameEventMessage(EGameEventMessage.ComboTimerToggle, m_Hit));
        yield return new WaitForSeconds(Random.Range(0.5f, 1)); // TODO: Calculer à partir de la TENSION
        m_Hit = false;
        GameEventSystem.Instance.TriggerEvent(EGameEvent.ComboTimerToggle, new GameEventMessage(EGameEventMessage.ComboTimerToggle, m_Hit));

        m_Timeout = true;
    }
}
