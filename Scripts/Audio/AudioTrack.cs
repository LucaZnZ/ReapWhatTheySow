using System;
using UnityEngine;

namespace ZnZUtil.Audio
{
    [Serializable]
    public class AudioTrack
    {
        public string name;
        public AudioType type;
        public AudioClip audioClip;
        [Space] [Range(0, 1)] public float volume;
        [Range(0.1f, 3)] public float pitch;
        public bool loop;

        [HideInInspector] public AudioSource source;

        public void SetSource(AudioSource source)
        {
            this.source = source;
            UpdateSource();
        }

        public void UpdateSource()
        {
            InitSource(source);
        }

        public void InitSource(AudioSource source)
        {
            if (source == null) return;
            source.clip = audioClip;
            source.playOnAwake = false;
            source.volume = volume;
            source.pitch = pitch;
            source.loop = loop;
        }
    }
}