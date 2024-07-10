using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum EEffect
{
    Hit,
    Torch,
    Explosion
}

public class Effect : MonoBehaviour
{
    [SerializeField] private float m_LifeTimeInSeconds;
    [SerializeField] private EEffect m_Type;
    [SerializeField] private int m_VariationCount = 1;
    [SerializeField] private float m_FowardToScreen = 5f;
    [SerializeField] private Animator m_Animator;
    [SerializeField] private List<EAudio> m_SoundsEffects;

    public void DoEffect(Transform parent)
    {
        SetPosition(parent);

        // Show
        gameObject.SetActive(true);

        ChangeAnimationVariation();
        PlaySound();

        // Destroy object after life time 
        if (m_LifeTimeInSeconds > 0) Destroy(gameObject, m_LifeTimeInSeconds);
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

    private void SetPosition(Transform parent)
    {
        // Set position
        transform.parent = parent;
        transform.position = parent.transform.position;

        // Move foward for aways display
        Vector3 directionToCamera = -GetCinemachineBrain().transform.forward;
        transform.Translate(directionToCamera * m_FowardToScreen);
    }

    private CinemachineBrain GetCinemachineBrain()
    {
        return CinemachineCore.Instance.GetActiveBrain(0);
    }
}
