using System;
using System.Collections;
using UnityEngine;

public enum EElemental
{
    Air,
    Fire,
    Water
}

[Serializable]
public struct Player : ITarget
{
    public string Name;
    public int HitPoints;
    public int HitPointsMax;
    public int TensionPoints;
    public int TensionPointsMax;
    public float ActionPoints;
    public int ActionPointsMax;
    public int ActionPointPerSec;
    public float DistanceAttack;
    public float RefreshTime;
    public Vector2 DamageRange; // TODO: Temp, il faut changer ça dans le systeme de combat

    public Player(int hitPointsMax, float distanceAttack) 
    {
        Name = "Player";
        HitPointsMax = hitPointsMax;
        HitPoints = HitPointsMax;
        DistanceAttack = distanceAttack;
        TensionPoints = 0;
        TensionPointsMax = 100;
        ActionPoints = 0;
        ActionPointsMax = 100;
        ActionPointPerSec = 45;
        RefreshTime = 0.1f;
        DamageRange = new Vector2(3, 15);
    }

    public float GetHPRatio()
    {
        return (float)HitPoints / (float)HitPointsMax;
    }

    public float GetAPRatio()
    {
        return (float)ActionPoints / (float)ActionPointsMax;
    }

    public float GetTPRatio()
    {
        return (float) TensionPoints / (float)TensionPointsMax;
    }

    public int GetDamage()
    {
        return UnityEngine.Random.Range(Mathf.FloorToInt(DamageRange.x), Mathf.FloorToInt(DamageRange.y));
    }

    public void AddActionPoints()
    {
        ActionPoints += ActionPointPerSec * RefreshTime;
        ActionPoints = ActionPoints > ActionPointsMax ? ActionPointsMax : ActionPoints;
    }

    public bool IsAction()
    {
        return ActionPoints >= ActionPointsMax;
    }

    public void ConsumeActionPoints()
    {
        ActionPoints = 0;
    }

    public string DisplayDamage()
    {
        return $"{Name} {HitPoints}/{HitPointsMax}";
    }

    public string DisplayDescription()
    {
        return "Self";
    }

    public bool IsAlive()
    {
        return HitPoints > 0;
    }
}

public class PlayerController : ATargetController
{


    /**
     * **********************************************
     * Variables de configuration du Player 
     * **********************************************
     */
    private Player m_Player;
    
    private bool m_StackActionPoints;


    // Events
    public event Action<Player> OnNotifyInfoPlayer;
    public event Action OnAttackSelected;
    public event Action OnSpellSelected;

    public event Action<int> OnAttack;
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
        m_Player = new Player(20, 5f); // Temp
        SubscribeAllEvents();
        RefreshInfoHUD();
        StartCoroutine(AddActionPointsRoutine());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            TakeDamage(5);
        }

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
            RefreshInfoHUD();
        }
    }

    private void SubscribeAllEvents()
    {
        GameStateEvent.Instance.SubscribeTo(EGameState.Interaction, OnInterractionState);
        GameStateEvent.Instance.SubscribeTo(EGameState.Spell, OnSpellState);
        GameStateEvent.Instance.SubscribeTo(EGameState.Combo, OnComboState);
        GameStateEvent.Instance.SubscribeTo(EGameState.None, OnNoneState);
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
        SelectSphere.Instance.HideSphere();
    }

    private void OnSpellState(bool isEnterState)
    {

        if (isEnterState) 
        {
            ConsumeAction();
            ConsumeTension(UnityEngine.Random.Range(2, 15));
            OnSpell?.Invoke(m_Player.GetDamage(), EElemental.Fire);
        } 
        else m_StackActionPoints = true;
    }

    private void OnComboState(bool isEnterState)
    {

        if (isEnterState) {
            ConsumeAction();
            OnAttack?.Invoke(m_Player.GetDamage());
        } 
        else m_StackActionPoints = true;
    }

    private void ConsumeAction()
    {
        m_StackActionPoints = false;
        m_Player.ConsumeActionPoints();
        RefreshInfoHUD();
    }

    private void RefreshInfoHUD()
    {
        OnNotifyInfoPlayer?.Invoke(m_Player);
    }

    public void TakeDamage(int damage)
    {
        m_Player.HitPoints -= damage;
        // TODO : Add die
        RefreshInfoHUD();
    }

    public void AddTension(int tension)
    {
        m_Player.TensionPoints += tension;
        m_Player.TensionPoints = m_Player.TensionPoints > m_Player.TensionPointsMax ? m_Player.TensionPointsMax : m_Player.TensionPoints;

        RefreshInfoHUD();
    }

    public void ConsumeTension(int tension)
    {
        m_Player.TensionPoints -= tension;
        m_Player.TensionPoints = m_Player.TensionPoints < 0 ? 0 : m_Player.TensionPoints;

        RefreshInfoHUD();
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

    public bool IsInRange(Vector3 targetPosition)
    {
        float distance = Vector3.Distance(transform.position, targetPosition);
        //Debug.Log(distance);
        return distance <= m_Player.DistanceAttack;
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
       return transform.position - position;
    }

    public Vector3 GetDirectionTo(Vector3 position)
    {
        return position - transform.position;
    }
}
