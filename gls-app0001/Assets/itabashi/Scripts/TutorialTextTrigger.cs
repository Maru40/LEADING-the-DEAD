using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialTextTrigger : MonoBehaviour
{
    [SerializeField]
    private BoxCollider m_boxCollider;

    [SerializeField]
    private Text m_text;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDrawGizmos()
    {
        if(!m_boxCollider)
        {
            return;
        }

        Color color = Color.green;
        color.a = 0.5f;
        Gizmos.color = color;
        Gizmos.DrawCube(m_boxCollider.center, m_boxCollider.size);
    }
}
