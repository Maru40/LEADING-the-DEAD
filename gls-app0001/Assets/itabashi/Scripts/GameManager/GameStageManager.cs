using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Manager
{
    public class GameStageManager : MonoBehaviour
    {
        [SerializeField]
        private List<StageData> m_stageDatas = new List<StageData>();

        private int m_stageIndex = -1;

        public int stageIndex => m_stageIndex;

        private bool m_isSelectedStage = false;

        public bool isSelectStage => m_isSelectedStage;

        public static GameStageManager Instance { private set; get; }

        public StageData currentStageData => m_isSelectedStage ? m_stageDatas[m_stageIndex] : null;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public bool CanIncrement() => m_stageIndex < m_stageDatas.Count - 1;

        public bool CanDecrement() => m_stageIndex >= 0;

        public void Increment()
        {
            if (m_stageIndex != m_stageDatas.Count - 1)
            {
                ++m_stageIndex;
            }

            m_isSelectedStage = m_stageIndex >= 0;
        }

        public void Decrement()
        {
            if (m_isSelectedStage)
            {
                --m_stageIndex;
            }

            m_isSelectedStage = m_stageIndex >= 0;
        }
    }
}
