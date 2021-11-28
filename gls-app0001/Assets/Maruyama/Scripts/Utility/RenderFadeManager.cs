using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

public class RenderFadeManager : MonoBehaviour
{
    public enum BlendMode
    {
        None = -1,
        Opaque,
        Cutout,
        Fade,
        Transparent,
    }

    struct InitParametor
    {
        public BlendMode blendMode;
        public Color color;

        public InitParametor(BlendMode blendMode, Color color)
        {
            this.blendMode = blendMode;
            this.color = color;
        }
    }

    InitParametor m_initParam = new InitParametor();

    [SerializeField]
    float m_fadeTime = 1.0f;

    System.Action m_endAction = null;

    bool m_isEnd = false;
    public bool IsEnd => m_isEnd;

    Renderer m_render;

    private void Awake()
    {
        enabled = false;
        m_render = GetComponent<Renderer>();

        var material = m_render.material;
        //初期化用のデータを取得
        if (material.IsKeywordEnabled("_Mode"))
        {
            m_initParam.blendMode = (BlendMode)material.GetFloat("_Mode");
        }
        m_initParam.color = material.color;
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
            //Destroy(gameObject);
            m_endAction?.Invoke();
            m_endAction = null;
            m_isEnd = true;
            enabled = false;
        }
    }

    public void FadeStart(Action action = null, BlendMode blendMode = BlendMode.Fade)
    {
        FadeStart(m_fadeTime, action, blendMode);
    }

    public void FadeStart(float fadeTime, Action action = null, BlendMode blendMode = BlendMode.Fade)
    {
        m_isEnd = false;
        enabled = true;
        m_fadeTime = fadeTime;

        ChangeBlendMode(m_render.material, blendMode);
    }

    /// <summary>
    /// 初期状態に戻す。
    /// </summary>
    public void ResetInit()
    {
        if (m_initParam.blendMode == BlendMode.None) {
            return;
        }
        var material = m_render.material;
        ChangeBlendMode(material, m_initParam.blendMode);
        material.color = m_initParam.color;

        m_isEnd = false;
        enabled = false;
    }

    /// <summary>
    /// ブレンドモードの変更
    /// </summary>
    /// <param name="material">マテリアル</param>
    /// <param name="blendMode">ブレンドモード</param>
    void ChangeBlendMode(Material material, BlendMode blendMode)
    {
        material.SetFloat("_Mode", (float)blendMode);

        switch (blendMode)
        {
            case BlendMode.Opaque:
                material.SetOverrideTag("RenderType", "");
                material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
                material.SetInt("_ZWrite", 1);
                material.DisableKeyword("_ALPHATEST_ON");
                material.DisableKeyword("_ALPHABLEND_ON");
                material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                material.renderQueue = -1;
                break;
            case BlendMode.Cutout:
                material.SetOverrideTag("RenderType", "TransparentCutout");
                material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
                material.SetInt("_ZWrite", 1);
                material.EnableKeyword("_ALPHATEST_ON");
                material.DisableKeyword("_ALPHABLEND_ON");
                material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                material.renderQueue = 2450;
                break;
            case BlendMode.Fade:
                material.SetOverrideTag("RenderType", "Transparent");
                material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                material.SetInt("_ZWrite", 0);
                material.DisableKeyword("_ALPHATEST_ON");
                material.EnableKeyword("_ALPHABLEND_ON");
                material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                material.renderQueue = 3000;
                break;
            case BlendMode.Transparent:
                material.SetOverrideTag("RenderType", "Transparent");
                material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                material.SetInt("_ZWrite", 0);
                material.DisableKeyword("_ALPHATEST_ON");
                material.DisableKeyword("_ALPHABLEND_ON");
                material.EnableKeyword("_ALPHAPREMULTIPLY_ON");
                material.renderQueue = 3000;
                break;
        }
    }
}
