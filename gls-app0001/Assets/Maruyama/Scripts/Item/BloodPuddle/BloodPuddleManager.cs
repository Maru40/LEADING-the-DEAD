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
    struct InkParametor
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
    float m_time = 5.0f;

    WaitTimer m_waitTimer;

    List<InkParametor> m_inkParams = new List<InkParametor>();

    void Awake()
    {
        m_waitTimer = GetComponent<WaitTimer>();
    }

    private void Start()
    {
        m_waitTimer.AddWaitTimer(GetType(), m_time, EndProcess);
    }

    /// <summary>
    /// 終了関数。
    /// </summary>
    void EndProcess()
    {
        InkErase();  //インクの消去

        Destroy(gameObject);
    }

    /// <summary>
    /// インクの消去
    /// </summary>
    void InkErase()
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
