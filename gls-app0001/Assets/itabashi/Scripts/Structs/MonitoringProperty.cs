using UnityEngine;
using UnityEngine.Events;

namespace Utility
{
    [System.Serializable]
    public class MonitoringProperty<T>
    {
        [SerializeField]
        private T m_value;

        [SerializeField]
        public UnityEvent<T> OnChangedEvent;

        public T value
        {
            set
            {
                bool isEqual = m_value.Equals(value);

                m_value = value;

                if(!isEqual)
                {
                    OnChangedEvent?.Invoke(m_value);
                }
            }

            get { return m_value; }
        }


        public MonitoringProperty(T value)
        {
            m_value = value;
        }
    }
}