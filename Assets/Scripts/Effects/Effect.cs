using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum EEffect
{
    Hit,
    Torch,
    Explosion,
    Preparing
}

public class Effect : MonoBehaviour
{
    [SerializeField] private float m_LifeTimeInSeconds;
    public float LifeTimeInSeconds { get => m_LifeTimeInSeconds; set { m_LifeTimeInSeconds = value; } }
    [SerializeField] private EEffect m_Type;
    [SerializeField] private int m_VariationCount = 1;
    [SerializeField] private float m_FowardToScreen = 5f;
    public float FowardToScreen { get => m_FowardToScreen; set { m_FowardToScreen = value; } }
    [SerializeField] private Animator m_Animator;
    [SerializeField] private List<EAudio> m_SoundsEffects = new();
    public List<EAudio> SoundsEffects { get => m_SoundsEffects; set { m_SoundsEffects = value; } }

    private System.Action<Effect, Collider> OnTriggerEnterAction;

    public void DoEffect(Transform parent,System.Action<Effect, Collider> OnTriggerEnter = null)
    {
        SetParent(parent);

        // Show
        gameObject.SetActive(true);

        ChangeAnimationVariation();
        PlaySound();

        // Destroy object after life time 
        if (m_LifeTimeInSeconds > 0) Destroy(gameObject, m_LifeTimeInSeconds);

        if(OnTriggerEnter != null) OnTriggerEnterAction += OnTriggerEnter;
    }

    private void OnTriggerEnter(Collider other)
    {
        OnTriggerEnterAction?.Invoke(this, other);
    }

    private void PlaySound()
    {
        if (m_SoundsEffects.Count <= 0) return;
        
        // Play sound
        int idSound = Random.Range(0, m_SoundsEffects.Count);
        AudioManager.Instance.Play(m_SoundsEffects[idSound]);
    }

    private void ChangeAnimationVariation()
    {
        if (m_VariationCount <= 1) return;
        
        // Set random variation
        int idAnimation = Random.Range(0, m_VariationCount);
        m_Animator.SetInteger(GameParametres.AnimationEffect.INT_ID_ANIMATION, idAnimation);
    }

    private void SetParent(Transform parent)
    {
        // Set position
        transform.position = parent.position;
        transform.SetParent(parent);

        if(m_FowardToScreen != 0)
        {
            // Move foward for aways display
            Vector3 directionToCamera = -GetCinemachineBrain().transform.forward;
            transform.Translate(directionToCamera * m_FowardToScreen);
        }
    }

    private CinemachineBrain GetCinemachineBrain()
    {
        return CinemachineCore.Instance.GetActiveBrain(0);
    }

    public void DestroyIt(float delay = 0)
    {
        Destroy(gameObject, delay);
    }
}
