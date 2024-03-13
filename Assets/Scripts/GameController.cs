using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    //public event Action<bool> OnInterractionState;

    private void Start()
    {
    }

    private void Update()
    {

    }

    public void OnInterationMode(bool inInterraction)
    {

        //OnInterractionState?.Invoke(inInterraction);
        //PlayerController.GetInstance().ShowInterractionMode(inInterraction);
    }

}
