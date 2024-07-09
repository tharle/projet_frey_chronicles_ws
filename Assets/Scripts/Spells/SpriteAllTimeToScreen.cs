using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteAllTimeToScreen : MonoBehaviour
{
    void Update()
    {
        transform.forward = GetCinemachineBrain().transform.forward;
    }

    private CinemachineBrain GetCinemachineBrain()
    {
        return CinemachineCore.Instance.GetActiveBrain(0);
    }
}
