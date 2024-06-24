using System;
using System.Collections;
using UnityEngine;

public class PlayerController : ATargetController
{

    [SerializeField] private Transform m_PlayerHand;

    public Transform PlayerHand { get => m_PlayerHand; }

    /**
     * **********************************************
     * Variables de configuration du Player 
     * **********************************************
     */
    private Player m_Player;
    
    private bool m_StackActionPoints;


    // Events
    public event Action OnAttackSelected;
    public event Action OnSpellSelected;

    public event Action<int, EElemental> OnSpell;

    private static PlayerController m_Instance;
    public static PlayerController Instance { get { return m_Instance; } }


    private void Awake()
    {
        if (m_Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        m_Instance = this;
    }

    protected override void AfterStart()
    {
        m_Player = new Player(20, 10f); // Temp
        SubscribeAllEvents();
        RefreshInfoHUD();
        StartCoroutine(AddActionPointsRoutine());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            AddTension(7);
        }

        if (Input.GetKeyDown(KeyCode.V))
        {
            ConsumeTension(25);
        }
    }

    private IEnumerator AddActionPointsRoutine()
    {
        m_StackActionPoints = true;
        while (true)
        {
            yield return new WaitUntil(() => m_StackActionPoints);

            m_Player.AddActionPoints();
            yield return new WaitForSeconds(m_Player.RefreshTime);
            RefreshInfoHUDAP();
        }
    }

    private void SubscribeAllEvents()
    {
        GameStateEvent.Instance.SubscribeTo(EGameState.Interaction, OnInterractionState);
        GameStateEvent.Instance.SubscribeTo(EGameState.Spell, OnSpellState);
        GameStateEvent.Instance.SubscribeTo(EGameState.Combo, OnComboState);
        GameStateEvent.Instance.SubscribeTo(EGameState.None, OnNoneState);
        GameEventSystem.Instance.SubscribeTo(EGameEvent.EnterRoom, EnterRoom);
        GameEventSystem.Instance.SubscribeTo(EGameEvent.DamageToPlayer, TakeDamage);
        GameEventSystem.Instance.SubscribeTo(EGameEvent.ComboDamageToEnemy, ComboDamageToEnemy);
    }

    private void EnterRoom(GameEventMessage message)
    {
        if (message.Contains<RoomController>(EGameEventMessage.Room, out RoomController room))
        {
            // TODO : changer pour la bonne porte
            TeleportTo(room.GetRandomDoor());
        }
    }
    private void TeleportTo(Vector3 newPosition)
    {
        transform.position = newPosition;
    }


    private void OnInterractionState(bool isEnterState)
    {

        if (isEnterState) SpawnSelectSphere();
    }

    private void OnNoneState(bool isEnterState)
    {

        if (isEnterState) DespawnSelectSphere();
    }

    private void SpawnSelectSphere()
    {
        SelectSphere.Instance.ShowSphere(m_Player.DistanceAttack);
    }

    private void DespawnSelectSphere()
    {
        SelectSphere.Instance.DespawnSphere();
    }

    private void OnSpellState(bool isEnterState)
    {

        if (isEnterState) 
        {
            AudioManager.Instance.Play(EAudio.MagicFire, transform.position);
            ConsumeAction();
            ConsumeTension(UnityEngine.Random.Range(2, 15));
            OnSpell?.Invoke(m_Player.GetDamage(), EElemental.Fire);
        } 
        else m_StackActionPoints = true;
    }

    private void OnComboState(bool isEnterState)
    {

        /*if (isEnterState) {
            AudioManager.Instance.Play(EAudio.Attack, transform.position);
            ConsumeAction();
            OnAttack?.Invoke(m_Player.GetDamage());
        } 
        else m_StackActionPoints = true;*/

        if (isEnterState) ConsumeAction();
        else m_StackActionPoints = true;
    }

    private void ConsumeAction()
    {
        m_StackActionPoints = false;
        m_Player.ConsumeActionPoints();
        RefreshInfoHUDAP();
    }

    private void RefreshInfoHUD()
    {
        RefreshInfoHUDHP();
        RefreshInfoHUDAP();
        RefreshInfoHUDTP();
    }

    private void RefreshInfoHUDHP()
    {
        GameEventSystem.Instance.TriggerEvent(EGameEvent.RefreshHUDHP, new GameEventMessage(EGameEventMessage.HudBarData, m_Player.GetHPData()));
    }

    private void RefreshInfoHUDAP()
    {
        GameEventSystem.Instance.TriggerEvent(EGameEvent.RefreshHUDAP, new GameEventMessage(EGameEventMessage.HudBarData, m_Player.GetAPData()));
    }

    private void RefreshInfoHUDTP()
    {
        GameEventSystem.Instance.TriggerEvent(EGameEvent.RefreshHUDTP, new GameEventMessage(EGameEventMessage.HudBarData, m_Player.GetTPData()));
    }

    private void TakeDamage(GameEventMessage message)
    {
        if (message.Contains<float>(EGameEventMessage.DamageAttack, out float damage))
        {
            // TODO : Calculer fablisse (?)
            m_Player.HitPoints -= damage;
        }
        // TODO : Add die
        //RefreshInfoHUD();
        RefreshInfoHUDHP();
    }

    private void ComboDamageToEnemy(GameEventMessage message)
    {
        if (message.Contains<ATargetController>(EGameEventMessage.TargetController, out ATargetController targetController))
        {
            AddTension(targetController.ReciveAttack(m_Player.GetDamage()));
        }
    }

    public void AddTension(int tension)
    {
        m_Player.TensionPoints += tension;
        m_Player.TensionPoints = m_Player.TensionPoints > m_Player.TensionPointsMax ? m_Player.TensionPointsMax : m_Player.TensionPoints;

        //RefreshInfoHUD();
        RefreshInfoHUDTP();
    }

    public void ConsumeTension(int tension)
    {
        m_Player.TensionPoints -= tension;
        m_Player.TensionPoints = m_Player.TensionPoints < 0 ? 0 : m_Player.TensionPoints;

        //RefreshInfoHUD();
        RefreshInfoHUDTP();
    }

    // TODO: jusqu'au comobo etre fait
    public void AttackSelected()
    {
        OnAttackSelected?.Invoke();
    }

    public void SpellSelected()
    {
        //OnAttack?.Invoke(m_Player.GetDamage());
        OnSpellSelected?.Invoke();
    }

    public bool IsAction()
    {
        return m_Player.IsAction();
    }

    public override ITarget GetTarget()
    {
        return m_Player;
    }

    public Vector3 GetDirectionFrom(Vector3 position)
    {
        Vector3 playerPos = transform.position;
        playerPos.y = 0;
        position.y = 0;
       return (position - playerPos).normalized;
    }

    public Vector3 GetDirectionTo(Vector3 position)
    {
        Vector3 playerPos = transform.position;
        playerPos.y = 0;
        position.y = 0;
        return (playerPos - position).normalized;
    }
}
