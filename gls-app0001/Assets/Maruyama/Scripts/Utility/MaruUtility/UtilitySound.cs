using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MaruUtility
{
    namespace Sound
    {
        [System.Serializable]
        public struct AudioClipParametor
        {
            public AudioClip clip;
            public float volume;

            public AudioClipParametor(AudioClip clip, float volume)
            {
                this.clip = clip;
                this.volume = volume;
            }
        }
    }
}


