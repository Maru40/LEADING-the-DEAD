using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEnder : MonoBehaviour
{
    public void GameEnd()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
