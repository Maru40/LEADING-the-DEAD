using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class DrawNumFindZombie : MonoBehaviour
{
    private AllEnemyGeneratorManager m_allGeneratorManager;

    [SerializeField]
    private string m_textString = "ゾンビの数: ";
    [SerializeField, Header("仮攻略数")]
    private float m_numClearZombie = 15.0f;
    private Text m_text = null;

    private void Start()
    {
        m_allGeneratorManager = FindObjectOfType<AllEnemyGeneratorManager>();
        m_text = GetComponent<Text>();
    }

    private void Update()
    {
        var numFindZombie = m_allGeneratorManager.GetNumFindPlayerZombie();
        var numAliveZombie = m_allGeneratorManager.GetNumAllActiveZombie();

        m_text.text = m_textString + numFindZombie.ToString() + "/" + m_numClearZombie;
    }
}
