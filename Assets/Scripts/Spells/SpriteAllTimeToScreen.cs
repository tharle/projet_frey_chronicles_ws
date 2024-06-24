using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteAllTimeToScreen : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.forward = GetCinemachineBrain().transform.forward;
    }

    private CinemachineBrain GetCinemachineBrain()
    {
        return CinemachineCore.Instance.GetActiveBrain(0);
    }
}
