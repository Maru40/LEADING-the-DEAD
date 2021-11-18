using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderFadeManager : MonoBehaviour
{
    [SerializeField]
    float m_fadeTime = 1.0f;

    Renderer m_render;

    private void Awake()
    {
        enabled = false;
        m_render = GetComponent<Renderer>();
    }

    void Start()
    {

    }

    void Update()
    {
        if(m_render == null) { 
            return;
        }

        FadeUpdate();
    }

    void FadeUpdate()
    {
        var color = m_render.material.color;
        color.a -= Time.deltaTime / m_fadeTime;

        m_render.material.color = color;

        if(color.a <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void FadeStart()
    {
        FadeStart(m_fadeTime);
    }

    public void FadeStart(float fadeTime)
    {
        enabled = true;
        m_fadeTime = fadeTime;
    }
}
