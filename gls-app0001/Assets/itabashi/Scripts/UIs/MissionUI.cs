using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissionUI : MonoBehaviour
{
    [SerializeField]
    private MissionStamp m_missionStamp;

    public MissionStamp missionStamp => m_missionStamp;

    [SerializeField]
    private Text m_missionText;

    public Text missionText => m_missionText;

    public void SetMissionStatus(bool isAchieve, string missionTextString)
    {
        missionStamp.SetIsAchieve(isAchieve);
        missionText.text = missionTextString;
    }
}
