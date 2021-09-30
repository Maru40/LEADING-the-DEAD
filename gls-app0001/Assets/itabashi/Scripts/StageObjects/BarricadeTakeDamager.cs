using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarricadeTakeDamager : MonoBehaviour, I_TakeDamage
{
    /// <summary>
    /// バリケードの耐久度
    /// </summary>
    [SerializeField]
    private int m_durability = 100;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(DamageData damageData)
    {
        Debug.Log($"{damageData.damage}ダメージ受けました");
        m_durability -= damageData.damage;

        m_durability = Mathf.Max(m_durability, 0);

        if (m_durability > 0)
        {
            return;
        }

        Debug.Log("破壊されました");

        Destroy(gameObject);
    }
}
