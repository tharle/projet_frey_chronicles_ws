using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class ComboState : AGameState
{
    private float m_WaitAttack = 0.3f;
    private Vector2 m_WaitDamage = new Vector2 (.5f, .7f);
    private Vector2 m_WaitCombo = new Vector2 (0.2f, 0.4f);
    private bool m_WaitHit;
    private bool m_Timeout;
    private bool m_AttackWasPressed;
    private int m_ComboCounter;
    private Coroutine m_CurrentRoutine;
    private ATargetController m_Target;
    public ComboState(GameStateController controller) : base(controller, EGameState.Combo)
    {
    }

    public override void OnEnter()
    {
        base.OnEnter();
        Debug.Log("Combo ENTER");
        GameEventSystem.Instance.SubscribeTo(EGameEvent.EnemyDie, OnEnemyDie);
        SelectSphere.Instance.HideSphere();

        m_ComboCounter = 0;
        m_Timeout = false;
        m_Target = DungeonTargetManager.Instance.TargetSelected;
        m_Controller.StartCoroutine(DoAttack());

        m_WaitDamage = new Vector2(0.5f, 0.7f);
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

        if (m_Timeout || !m_Target.IsAlive())
        {
            m_Controller.ChangeState(EGameState.None);
            return;
        }

        
        if (m_AttackWasPressed) return;
        if(Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown((int)MouseButton.Left))
        {

            m_AttackWasPressed = true;

            if (m_WaitHit)
            {
                SetWaitHit(false);
                m_Controller.StopAllCoroutines();
                AddComboCounter();
                m_Controller.StartCoroutine(DoAttack());
            }
        }
    }

    private void SetWaitHit(bool value)
    {
        m_WaitHit = value;
        GameEventSystem.Instance.TriggerEvent(EGameEvent.ComboTimerToggle, new GameEventMessage(EGameEventMessage.ComboTimerToggle, value));
    }

    private void AddComboCounter()
    {
        m_ComboCounter++;
        GameEventSystem.Instance.TriggerEvent(EGameEvent.ComboInfoHUD, new GameEventMessage(EGameEventMessage.ComboValue, m_ComboCounter));
    }

    private IEnumerator DoAttack()
    {
        
        yield return null;
        m_CurrentRoutine = null;

        m_AttackWasPressed = false;
        yield return new WaitForSeconds(Random.Range(m_WaitDamage.x, m_WaitDamage.y));  //  give time for sounds and damage calculs

        // Animation
        PlayerAnimation.Instance.Attack();
        PlayerController.Instance.LookToTarget(m_Target);


        m_CurrentRoutine = m_Controller.StartCoroutine(DoComboRoutine());
    }

    private IEnumerator DoComboRoutine()
    {
        yield return new WaitForSeconds(Random.Range(0.1f, m_WaitAttack) );  //  give time for sounds and damage calculs
        if (!m_AttackWasPressed) 
        {
            SetWaitHit(true);
        }

        // In enemy
        Effect effect = EffectPoolManager.Instance.Get(EEffect.Hit);
        effect.DoEffect(m_Target.transform);
        GameEventSystem.Instance.TriggerEvent(EGameEvent.ComboDamageToEnemy, new GameEventMessage(EGameEventMessage.TargetController, m_Target));
        float delay = m_WaitCombo.y * (1.5f - PlayerController.Instance.TensionRatio());
        yield return new WaitForSeconds(delay); // TODO: Calculer à partir de la TENSION
        SetWaitHit(false);

        m_Timeout = true;
    }
}
