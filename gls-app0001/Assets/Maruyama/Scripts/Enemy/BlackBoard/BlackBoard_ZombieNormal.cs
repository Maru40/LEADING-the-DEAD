using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Parametor = BlackBoard_ZombieNormal.Parametor;

public class BlackBoard_ZombieNormal : MonoBehaviour, I_BlackBoard<Parametor>
{
    [System.Serializable]
    public struct Parametor
    {
        public TargetManager targetManager;
        public StateNode_ZombieNormal_Attack.BlackBoardParametor attackParam;
    }

    [SerializeField]
    private Parametor m_param;

    private void Start()
    {
        if(m_param.targetManager == null)
        {
            m_param.targetManager = GetComponent<TargetManager>();
        }
    }

    public ref Parametor Struct
    {
        get => ref m_param;
    }

    public Parametor GetStruct()
    {
        return m_param;
    }
}
