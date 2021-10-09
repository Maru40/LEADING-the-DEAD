using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public abstract class PlayerActionBase : MonoBehaviour
    {
        [SerializeField]
        protected PlayerStatusManager m_playerStatusManager;
    }
}
