using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EElemental
{
    Air,
    Fire,
    Water
}

[Serializable]
public struct Player 
{
    public int HitPoints;
    public int HitPointsMax;
    public int TensionPoints;
    public int TensionPointsMax;
    public float DistanceAttack;
    public Vector2 DamageRange;

    public Player(int hitPointsMax, float distanceAttack) 
    {
        HitPointsMax = hitPointsMax;
        HitPoints = HitPointsMax;
        DistanceAttack = distanceAttack;
        TensionPoints = 0;
        TensionPointsMax = 100;
        DamageRange = new Vector2(3, 7);
    }

    public int GetDamage()
    {
        return UnityEngine.Random.Range(Mathf.FloorToInt(DamageRange.x), Mathf.FloorToInt(DamageRange.y));
    }
}

public class PlayerController : MonoBehaviour
{


    /**
     * **********************************************
     * Variables de configuration du Player 
     * **********************************************
     */
    Player m_Player;


    // Events
    public event Action<Player> OnNotifyInfoPlayer;
    public event Action OnAttackSelected;
    public event Action OnSpellSelected;

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

    private void Start()
    {
        m_Player = new Player(20, 5f);
        SubscribeAllEvents();
        RefreshInfoHUD();
    }

    private void SubscribeAllEvents()
    {
        GameStateEvent.Instance.SubscribeTo(EGameState.Interaction, OnInterractionMode);
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

    private void OnInterractionMode(bool isEnterState)
    {

        if (isEnterState) SpawnSelectSphere();
        else DespawnSelectSphere();
    }

    private void SpawnSelectSphere()
    {
        SelectSphere.Instance.ShowSphere(m_Player.DistanceAttack);
    }

    private void DespawnSelectSphere()
    {
        SelectSphere.Instance.HideSphere();
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
        //OnAttack?.Invoke(m_Player.GetDamage());
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
}
