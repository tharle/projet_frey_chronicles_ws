using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorchController : MonoBehaviour
{
    [SerializeField] Vector2 m_RangeInstansity = new Vector2(1f, 8f);
    private Light m_Light;
    private float m_Delay;

    void Start()
    {
        m_Light = GetComponent<Light>();
        m_Delay = Random.Range(0, 2f);

        StartCoroutine(FliLightRoutine());
    }

    private IEnumerator FliLightRoutine()
    {
        yield return new WaitForSeconds(m_Delay);

        while (true)
        {
            m_Light.intensity = Mathf.PingPong(2 * Time.deltaTime, 8f);
            //m_Light.intensity = Random.Range(m_RangeInstansity.x, m_RangeInstansity.y);
            //m_Light.intensity = Mathf.PingPong(m_RangeInstansity.x, m_RangeInstansity.y);
            yield return null;
        }
    }
}
