using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDMenu : MonoBehaviour
{
    [SerializeField] private GameObject m_MenuContainer;
    [SerializeField] private GameObject m_SpellBookContainer;


    public void OnOpenOptions()
    {
        Time.timeScale = 0f;
        m_MenuContainer.SetActive(true);
    }

    public void OnCloseOptions()
    {
        Time.timeScale = 1f;
        m_MenuContainer.SetActive(false);
    }
    public void OnOpenSpellBook()
    {
        Time.timeScale = 0f;
        m_SpellBookContainer.SetActive(true);
    }

    public void OnCloseSpellBook()
    {
        Time.timeScale = 1f;
        m_SpellBookContainer.SetActive(false);
    }

}

