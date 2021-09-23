using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProtoGameOver : MonoBehaviour
{
    [SerializeField]
    private Text m_gameOverText;

    // Start is called before the first frame update
    void Start()
    {
        m_gameOverText.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GameOver()
    {
        m_gameOverText.enabled = true;
    }
}
