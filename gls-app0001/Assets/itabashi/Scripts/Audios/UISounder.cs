using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Manager;

public class UISounder : MonoBehaviour, ISelectHandler, ISubmitHandler, ICancelHandler
{
    [SerializeField]
    private AudioSource m_audioSource;

    [SerializeField, CustomLabel("決定音")]
    private AudioClip m_submitSound;

    [SerializeField, CustomLabel("キャンセル音")]
    private AudioClip m_cancelSound;

    [SerializeField, CustomLabel("選択音")]
    private AudioClip m_selectSound;

    [SerializeField]
    private float m_seVolumeScale = 1.0f;

    private void OneShot(AudioClip audioClip)
    {
        if(!audioClip)
        {
            return;
        }

        if(m_audioSource)
        {
            m_audioSource.PlayOneShot(audioClip, m_seVolumeScale);
        }

        GameAudioManager.Instance.SEPlayOneShot(audioClip, m_seVolumeScale);
    }

    public void SubmitPlay() => OneShot(m_submitSound);

    public void CancelPlay() => OneShot(m_cancelSound);

    public void SelectPlay() => OneShot(m_selectSound);

    public void OnSubmit(BaseEventData baseEventData) => SubmitPlay();

    public void OnCancel(BaseEventData baseEventData) => CancelPlay();

    public void OnSelect(BaseEventData baseEventData) => SelectPlay();
}
