using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using static System.Runtime.CompilerServices.RuntimeHelpers;

public class CastSpell : MonoBehaviour
{
    [SerializeField] Transform m_Container;
    List<Rune> m_Runes;

    private List<ERune> m_CastedRunes = new();


    void Start()
    {
        LoadAllRunes();
    }

    private void LoadAllRunes()
    {
        List<RuneData> runes = BundleLoader.Instance.LoadAll<RuneData, ERune>(GameParametres.BundleNames.RUNES);
        m_Runes = new();
        foreach(var data in runes) m_Runes.Add(data.Value);
    }

    void Update()
    {
        foreach (Rune rune in m_Runes)
        {
            foreach(KeyCode keyCode in rune.KCodes) 
                if (Input.GetKeyDown(keyCode)) CastRune(rune);
        }

        if (Input.GetKeyDown(KeyCode.M)) Clean();
    }

    private void CastRune(Rune rune)
    {
        m_CastedRunes.Add(rune.Type);

        GameObject go = new GameObject(Enum.GetName(typeof(ERune), rune.Type));
        go.transform.position = Vector3.zero;
        go.transform.SetParent(m_Container, false);
        Image image = go.AddComponent<Image>();
        image.sprite = rune.Icon;
    }

    private void Clean()
    {
        m_CastedRunes = new();
        foreach (Transform runeCasted in m_Container.transform) Destroy(runeCasted.gameObject);
    }
}
