using UnityEngine;
using UniRx;

public class GameTimeManager
{
    private static BoolReactiveProperty m_isPause = new BoolReactiveProperty(false);

    public static System.IObservable<bool> isPauseOnChanged { get { return m_isPause; } }

    public static bool isPause { get { return m_isPause.Value; } }

    public static void Pause()
    {
        Time.timeScale = 0.0f;

        m_isPause.Value = true;
    }

    public static void UnPause()
    {
        Time.timeScale = 1.0f;

        m_isPause.Value = false;
    }
}
