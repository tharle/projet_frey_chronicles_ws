using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{

    private void Start()
    {
    }

    private void Update()
    {

    }

    public void OnInterationMode(bool inInterraction)
    {
        PlayerController.GetInstance().ShowInterractionMode(inInterraction);
    }

}
