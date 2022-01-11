using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using AttributeObject;
using MaruUtility.UtilityDictionary;
using MaruUtility.Sound;

public class DamageSoundManager : MonoBehaviour
{
    [SerializeField]
    private Ex_Dictionary<DamageType, List<AudioClipParametor>> m_audioParamsDictionary = 
        new Ex_Dictionary<DamageType, List<AudioClipParametor>>();

    private void Awake()
    {
        m_audioParamsDictionary.InsertInspectorData();
    }

    public void Damaged(DamageData data)
    {
        if(m_audioParamsDictionary.ContainsKey(data.type))
        {
            var audioParams = m_audioParamsDictionary[data.type];
            foreach (var param in audioParams)
            {
                Manager.GameAudioManager.Instance.SEPlayOneShot(param.clip, param.volume);
            }
        }
    }
}
