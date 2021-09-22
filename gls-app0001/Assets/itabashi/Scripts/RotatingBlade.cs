using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingBlade : MonoBehaviour
{
    /// <summary>
    /// ê⁄êGÉ_ÉÅÅ[ÉW
    /// </summary>
    [SerializeField]
    private float m_touchDamage = 10.0f;

    /// <summary>
    /// àÍïbä‘Ç…âÒì]Ç∑ÇÈäpìx
    /// </summary>
    [SerializeField]
    private float m_rotateEulerPerSecond = 45.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(0.0f, m_rotateEulerPerSecond * Time.deltaTime, 0.0f));
    }

    private void OnCollisionEnter(Collision collision)
    {
        var takeDamageTarget = collision.gameObject.GetComponent<AttributeObject.TakeDamageObject>();

        takeDamageTarget?.TakeDamage(new AttributeObject.DamageData(m_touchDamage));
    }
}
