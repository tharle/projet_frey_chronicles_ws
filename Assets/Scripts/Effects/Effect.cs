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

    public void DoEffect(Transform parent)
    {
        // Set position
        transform.parent = parent;
        transform.position = parent.transform.position;

        // Move foward for aways display
        Vector3 directionToCamera = -GetCinemachineBrain().transform.forward;
        transform.Translate(directionToCamera * m_FowardToScreen);

        // Show
        gameObject.SetActive(true);


        // Set random variation
        if (m_VariationCount > 1 && m_Animator != null)
        {
            int idAnimation = Random.Range(0, m_VariationCount);
            m_Animator.SetInteger(GameParametres.AnimationEffect.INT_ID_ANIMATION, idAnimation);
        }


        
        // Destroy object after life time 
        if (m_LifeTimeInSeconds > 0) Destroy(gameObject, m_LifeTimeInSeconds);
    }

    private CinemachineBrain GetCinemachineBrain()
    {
        return CinemachineCore.Instance.GetActiveBrain(0);
    }
}
