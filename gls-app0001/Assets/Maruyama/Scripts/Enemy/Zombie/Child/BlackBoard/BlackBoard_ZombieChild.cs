using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackBoard_ZombieChild : MonoBehaviour, I_BlackBoard<BlackBoard_ZombieChild.Parametor>
{
    [System.Serializable]
    public struct Parametor
    {
        public StateNode_ZombieChild_Dyning.DyningType dyningType;
    }

    [SerializeField]
    private Parametor m_param = new Parametor();

    public ref Parametor Struct
    {
        get => ref m_param;
    }

    public Parametor GetStruct()
    {
        return m_param;
    }
}
