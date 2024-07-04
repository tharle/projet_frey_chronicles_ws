using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    [SerializeField] Transform m_Model;
    private bool m_IsShowing = true;

    public void Show(bool show)
    {
        m_IsShowing = show;
        ToogleAllRenderChildsInModel(show);
    }

    private void ToogleAllRenderChildsInModel(bool show)
    {
        foreach (Transform child in m_Model)
        {
            if (child.TryGetComponent<MeshRenderer>(out MeshRenderer renderer))
            {
                renderer.enabled = show;
            }
        }
    }
}
