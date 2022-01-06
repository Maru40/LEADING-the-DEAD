using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Es.InkPainter;

/// <summary>
/// 血だまり管理
/// </summary>
[RequireComponent(typeof(WaitTimer))]
public class BloodPuddleManager : MonoBehaviour
{
    private struct InkParametor
    {
        public InkCanvas ink;
        public Brush brush;
        public Vector3 position;

        public InkParametor(InkCanvas ink, Brush brush, Vector3 position)
        {
            this.ink = ink;
            this.brush = brush;
            this.position = position;
        }
    }

    [SerializeField]
    private float m_time = 5.0f;

    [Header("BloodPuddleの見た目を管理するRenderManager群"), SerializeField]
    private List<RenderFadeManager> m_bloodPuddleRenderManagers = new List<RenderFadeManager>();

    [SerializeField]
    private VerticesAlphaManager m_verticesAlphaMangaer;

    private WaitTimer m_waitTimer;

    private List<InkParametor> m_inkParams = new List<InkParametor>();

    private void Awake()
    {
        m_waitTimer = GetComponent<WaitTimer>();
    }

    private void Start()
    {
        m_waitTimer.AddWaitTimer(GetType(), m_time, EndProcess);

        FadeStart();
        //m_verticesAlphaMangaer?.ChangeAlpha();
    }

    private void FadeStart()
    {
        foreach(var manager in m_bloodPuddleRenderManagers)
        {
            manager.FadeStart(m_time);
        }
    }

    /// <summary>
    /// 終了関数。
    /// </summary>
    private void EndProcess()
    {
        InkErase();  //インクの消去

        Destroy(gameObject);
    }

    /// <summary>
    /// インクの消去
    /// </summary>
    private void InkErase()
    {
        foreach (var param in m_inkParams)
        {
            param.ink.Erase(param.brush, param.position);
        }
    }

    //アクセッサ・プロパティ----------------------------------------------------------

    public void AddInk(InkCanvas ink, Brush brush, Vector3 position)
    {
        m_inkParams.Add(new InkParametor(ink, brush, position));
    }
}
