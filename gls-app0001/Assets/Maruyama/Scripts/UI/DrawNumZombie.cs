using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class DrawNumZombie : MonoBehaviour
{
    private AllEnemyGeneratorManager m_allGeneratorManager;

    [SerializeField]
    private string m_textString = "ゾンビの数: ";
    private Text m_text = null;

    private void Start()
    { 
        m_allGeneratorManager = FindObjectOfType<AllEnemyGeneratorManager>();
        m_text = GetComponent<Text>();
    }

    private void Update()
    {
        var numZombie = m_allGeneratorManager.GetNumAllZombie();
        var numAliveZombie = m_allGeneratorManager.GetNumAllActiveZombie();

        m_text.text = m_textString + numZombie.ToString() + "/" + numAliveZombie.ToString();
    }
}
