using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using AttributeObject;
using MaruUtility.UtilityDictionary;
using MaruUtility.Sound;

public class DamageSoundManager : MonoBehaviour
{
    [SerializeField]
    Ex_Dictionary<DamageType, AudioClipParametor> m_audioParamDictionary = new Ex_Dictionary<DamageType, AudioClipParametor>();

    private void Awake()
    {
        m_audioParamDictionary.InsertInspectorData();
    }

    public void Damaged(DamageData data)
    {
        if(m_audioParamDictionary.ContainsKey(data.type))
        {
            var param = m_audioParamDictionary[data.type];
            Manager.GameAudioManager.Instance.SEPlayOneShot(param.clip, param.volume);
        }
    }
}
