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
        float attackValue = Random.Range(0, 1);
        m_Animator.SetFloat(GameParametres.AnimationPlayer.FLOAT_ATTACK_ID, attackValue);
    }

    public void ResetAnimation() 
    {
        m_Animator.SetTrigger(GameParametres.AnimationPlayer.TRIGGER_TO_IDLE);
    }
}
