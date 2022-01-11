using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MaruUtility
{
    namespace Sound
    {
        [System.Serializable]
        public class AudioClipParametor
        {
            public AudioClip clip;
            public float volume;

            public float intervalTime;

            private GameTimer timer = new GameTimer();

            public AudioClipParametor()
                :this(null, 0.0f, 0.0f)
            {}

            public AudioClipParametor(AudioClip clip, float volume)
                :this(clip, volume, 0.0f)
            { }

            public AudioClipParametor(AudioClip clip, float volume, float intervalTime)
            {
                this.clip = clip;
                this.volume = volume;
                this.intervalTime = intervalTime;

                timer = new GameTimer();
                timer.ResetTimer(0.0f);
            }

            public void TimerUpdate()
            {
                timer.UpdateTimer();
            }

            public void SEPlayOneShot()
            {
                if (timer.IsTimeUp)
                {
                    Manager.GameAudioManager.Instance.SEPlayOneShot(clip, volume);
                    timer.ResetTimer(intervalTime);
                }
            }
        }
    }
}


