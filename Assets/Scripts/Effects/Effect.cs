using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum EEffect
{
    Hit
}
public class Effect : MonoBehaviour
{
    [SerializeField] private float m_LifeTimeInSeconds;
    [SerializeField] private EEffect m_Type;
    [SerializeField] private float m_FowardToScreen = 5f;

    public void DoEffect(Transform parent)
    {
        transform.parent = parent;
        transform.position = parent.transform.position;
        Vector3 directionToCamera = -GetCinemachineBrain().transform.forward;
        transform.Translate(directionToCamera * m_FowardToScreen);


        gameObject.SetActive(true);
        Destroy(gameObject, m_LifeTimeInSeconds);
    }

    private CinemachineBrain GetCinemachineBrain()
    {
        return CinemachineCore.Instance.GetActiveBrain(0);
    }
}
