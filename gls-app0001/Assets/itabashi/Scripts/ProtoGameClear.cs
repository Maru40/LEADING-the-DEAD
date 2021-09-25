using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProtoGameClear : MonoBehaviour
{
    [SerializeField]
    private Text m_clearText;

    // Start is called before the first frame update
    void Start()
    {
        m_clearText.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GameClear()
    {
        m_clearText.enabled = true;
        m_clearText.text = "CLEAR";
    }
}
