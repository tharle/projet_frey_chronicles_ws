using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    #region Singleton

    private static PlayerAnimation m_Instance;

    public static PlayerAnimation Instance
    {
        get
        {
            return m_Instance;
        }
    }
    #endregion

    private Animator m_Animator;


    private void Awake()
    {
        if (m_Instance != null) Destroy(gameObject);
        m_Instance = this;
    }

    private void Start()
    {
        m_Animator = GetComponent<Animator>();

    }

    public void ChangeVelocity(float velocity)
    {
        m_Animator.SetFloat(GameParametres.AnimationPlayer.FLOAT_VELOCITY, velocity);
    }

    public void Attack()
    {
        int attackId = Random.Range(0, 4);
        m_Animator.SetTrigger(GameParametres.AnimationPlayer.TRIGGER_ATTACK_ID + attackId);
    }

    public void ResetAnimation() 
    {
        m_Animator.SetTrigger(GameParametres.AnimationPlayer.TRIGGER_TO_IDLE);
    }

    public void InInterract()
    {
        m_Animator.SetTrigger(GameParametres.AnimationPlayer.TRIGGER_INTERRACT);
    }

    public void TakeDamage()
    {
        m_Animator.SetTrigger(GameParametres.AnimationPlayer.TRIGGER_HIT_STRONG);
    }

    public void Touch()
    {
        m_Animator.SetTrigger(GameParametres.AnimationPlayer.TRIGGER_TOUCH);
    }

    public void SpellCast()
    {
        m_Animator.SetTrigger(GameParametres.AnimationPlayer.TRIGGER_SPELL_CAST);
    }

    public void SpellPreparing()
    {
        m_Animator.SetTrigger(GameParametres.AnimationPlayer.TRIGGER_SPELL_PREPARING);
    }
}
