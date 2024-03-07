using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{


    /**
     * **********************************************
     * Variables de configuration du Player 
     * **********************************************
     */
    [SerializeField] private float m_DistanceAttack = 5f;
    public float DistanceAttack { 
        get { return m_DistanceAttack; } 
        set {  m_DistanceAttack = value; } 
    }

    [SerializeField] private Vector2 m_DamageRange = new Vector2(3, 7);
    [SerializeField] private float m_HitPoints = 100f;
    [SerializeField]private float m_HitPointsMax = 100f;
    [SerializeField] private float m_TensionPoints = 0f;
    [SerializeField] private float m_TensionPointsMax = 100f;


    // Events
    public event Action<float> OnHitPoint;
    public event Action<float> OnTensionPoint;
    public event Action<float> OnAttack;

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
        SelectSphere.Instance.ShowSphere(m_DistanceAttack);
    }

    private void DespawnSelectSphere()
    {
        SelectSphere.Instance.HideSphere();
    }

    public void TakeDamage(float damage)
    {
        m_HitPoints -= damage;
        // TODO : Add die

        float ratio = m_HitPoints / m_HitPointsMax;
        OnHitPoint?.Invoke(ratio);
    }

    public void AddTension(float tension)
    {
        m_TensionPoints += tension;

        m_TensionPoints = m_TensionPoints > m_TensionPointsMax? m_TensionPointsMax : m_TensionPoints;

        float ratio = m_TensionPoints / m_TensionPointsMax;
        OnTensionPoint?.Invoke(ratio);
    }

    public void ConsumeTension(float tension)
    {
        m_TensionPoints -= tension;

        m_TensionPoints = m_TensionPoints < 0 ? 0 : m_TensionPoints;

        float ratio = m_TensionPoints / m_TensionPointsMax;
        OnTensionPoint?.Invoke(ratio);
    }

    // TODO: jusqu'au comobo etre fait
    public void AttackSelected()
    {
        OnAttack?.Invoke(RandomDamage());
    }

    private float RandomDamage()
    {
        return UnityEngine.Random.Range(m_DamageRange.x, m_DamageRange.y);
    }


}
