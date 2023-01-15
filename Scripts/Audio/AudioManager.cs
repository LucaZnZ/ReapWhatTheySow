using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

namespace ZnZUtil.Audio
{
    public class AudioManager : SingletonBase<AudioManager>
    {
        protected override SceneMode sceneMode => SceneMode.DontDestroyOnLoad;

        [SerializeField] private List<AudioTrack> tracks = new();
        [SerializeField] private string playOnStart;
        [SerializeField] private float masterModifier = 1, sfxModifier = 1, musicModifier = 1, voiceModifier = 1;
        [SerializeField] private int soundeffectSourceLimit = 20;

        private List<AudioSource> sources = new();

        // TODO make it possible to play audio at calling object
        // TODO remove unused sources

        public static void PlayAudioTrack(string name)
        {
            if (instance == null || !instance.TryPlayAudio(name))
                Debug.LogWarning($"AudioTrack {name} not found");
        }

        public static bool HasTrack(string name) => instance == null || instance.tracks.Any(t => t.name == name);

        public static void ValidateTrack(string name, Object obj)
        {
            if (!HasTrack(name))
                Debug.LogWarning($"AudioTrack {name} not found{(obj != null ? $" on object {obj.name}" : "")}");
        }

        private AudioSource CreateAudioSource(GameObject obj)
        {
            var source = obj.AddComponent<AudioSource>();
            sources.Add(source);
            return source;
        }

        private bool TryPlayAudio(string name)
        {
            var track = tracks.Find(t => t.name == name);
            if (track == null) return false;

            if (PlaySoundeffectAudio(track))
                return true;

            if (track.source == null)
                track.SetSource(CreateAudioSource(gameObject));
            track.source.Play();

            return true;
        }

        private bool PlaySoundeffectAudio(AudioTrack track)
        {
            if (track.type != AudioType.Soundeffect) return false;
            var list = GetComponents<AudioSource>().Where(s => s.clip = track.audioClip);
            if (list.Count() >= soundeffectSourceLimit) return false;
            var src = CreateAudioSource(gameObject);
            track.InitSource(src);
            src.Play();
            return true;
        }

        /************************************************** Modifiers *************************************************/
        public void SetMasterModifier(float mod) => masterModifier = mod;
        public void SetSFXModifier(float mod) => sfxModifier = mod;
        public void SetMusicModifier(float mod) => musicModifier = mod;
        public void SetVoiceModifier(float mod) => voiceModifier = mod;

        private float GetModifier(AudioType type)
        {
            return masterModifier * type switch
            {
                AudioType.Soundeffect => sfxModifier,
                AudioType.Music => musicModifier,
                AudioType.Voice => voiceModifier,
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };
        }

        private void OnValidate()
        {
            UpdateModifiersOnTracks();
            tracks.ForEach(t => t.UpdateSource());
        }

        private void UpdateModifiersOnTracks()
        {
            foreach (var track in tracks) track.volume *= GetModifier(track.type);
        }

        private void Start()
        {
            if (playOnStart is {Length: > 0})
                PlayAudioTrack(playOnStart);
        }

        private void Update()
        {
            foreach (var idleSource in sources.Where(s => s != null && !s.isPlaying))
                Destroy(idleSource);
        }
    }
}