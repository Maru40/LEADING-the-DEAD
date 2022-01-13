using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyEar : EarBase
{
    [System.Serializable]
    public struct Parametor
    {
        [Header("聞こえる距離")]
        public float range;
    }

    private Parametor m_param = new Parametor();

    private TargetManager m_targetManager;

    private void Awake()
    {
        m_targetManager = GetComponent<TargetManager>();
    }

    public override void Listen(FoundObject foundObject)
    {
        m_targetManager.SetNowTarget(GetType(), foundObject);
    }
}
