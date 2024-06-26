using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    [SerializeField] GameObject m_Model;
    private bool m_IsShowing = true;

    public void Show(bool show)
    {
        m_IsShowing = show;
        m_Model.SetActive(show);
    }
}
