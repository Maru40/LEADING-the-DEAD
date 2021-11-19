using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace Player
{
    public class PlayerMeatPutter : MonoBehaviour
    {
        [SerializeField]
        private IntReactiveProperty m_haveMeatCount = new IntReactiveProperty(5);

        public int HaveMeatCount { private set => m_haveMeatCount.Value = value; get => m_haveMeatCount.Value; }

        [SerializeField]
        private Transform m_meatPutPoint = null;

        [SerializeField]
        private GameObject m_meatObjectPrefab;

        [SerializeField]
        private PlayerStatusManager m_statusManager = null;

        private GameControls m_gameControls = null;

        public System.IObservable<int> OnHaveMeatCountChanged => m_haveMeatCount;

        private void Awake()
        {
            m_gameControls = new GameControls();
            this.RegisterController(m_gameControls);

            m_gameControls.Player.UseMeat.performed += context => PutMeat();
        }

        public void PutMeat()
        {
            if(HaveMeatCount == 0 || !m_statusManager.isControllValid)
            {
                return;
            }

            --HaveMeatCount;

            Instantiate(m_meatObjectPrefab, m_meatPutPoint.position, Quaternion.identity);
        }
    }
}