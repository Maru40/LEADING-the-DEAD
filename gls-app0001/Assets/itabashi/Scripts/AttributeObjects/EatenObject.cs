using UnityEngine;
using UnityEngine.Events;

public class EatenObject : MonoBehaviour
{
    [SerializeField,Min(0)]
    private float m_eatWeight = 1.0f;

    public float eatWeight => m_eatWeight;

    [SerializeField]
    private UnityEvent<AttributeObject.DamageData> m_eatenEvent;

    public void Eaten(in AttributeObject.DamageData eatenDamageData)
    {
        m_eatenEvent?.Invoke(eatenDamageData);
    }
}
