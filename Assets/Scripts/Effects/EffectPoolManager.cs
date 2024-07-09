using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectPoolManager : MonoBehaviour
{
    #region Singleton
    private static EffectPoolManager m_Instance;
    public static EffectPoolManager Instance
    {
        get
        {
            if (m_Instance == null)
            {
                GameObject go = new GameObject("Effect Pool Manager");
                go.AddComponent<EffectPoolManager>();
            }

            return m_Instance;
        }
    }
    #endregion

    private Dictionary<EEffect, Effect> m_Effects = new Dictionary<EEffect, Effect>();

    private void Awake()
    {
        if (m_Instance != null)
        {
            Destroy(gameObject);
        }

        m_Instance = this;
    }

    public Effect Get(EEffect typeId)
    {
        if (!m_Effects.ContainsKey(typeId))
        {
            GameObject prefabEffect = BundleLoader.Instance.Load<GameObject>(GameParametres.BundleNames.EFFECTS, Enum.GetName(typeof(EEffect), typeId));
            prefabEffect.transform.parent = transform;
            prefabEffect.SetActive(false);
            if (prefabEffect.TryGetComponent<Effect>(out Effect effect))
            {
                m_Effects[typeId] = effect;
            }
        }

        return Instantiate(m_Effects[typeId]);
    }
}
