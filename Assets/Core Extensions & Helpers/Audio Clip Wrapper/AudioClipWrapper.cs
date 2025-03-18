using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace Core.Extensions
{
    [CreateAssetMenu(fileName = "New Sound", menuName = "Audio/Audio Clip Wrapper")]
    public class AudioClipWrapper : ScriptableObject
    {
        public string SoundName = "Headhunter, Leather Belt";
        [Range(0f,1f)]
        [SerializeField] float soundVolume = 0.7f;
        [SerializeField] AudioMixerGroup audioMixerOverride;
        public void ApplyMixerOverride(AudioSource source)
        {
            if (audioMixerOverride != null)
            {
                source.outputAudioMixerGroup = audioMixerOverride;
            }
        }
        [field: SerializeField] public List<ACWrapperEntry> soundClips { get; private set; } = new();
        [field: SerializeField] public bool singleChannel { get; private set; } = false;
        [field: SerializeField] public bool Is3D { get; private set; }
        [ContextMenu("Play Sound")]
        public void EditorPlaySound()
        {
            this.Play(Vector2.zero);
        }
        public ACWrapperEntry[] Entries => soundClips.ToArray();
        public float GetVolume(int index)
        {
            if (soundClips[index] == null)
                return soundVolume * 0.7f;
            return soundClips[index].Volume * soundVolume;
        }
        private void OnValidate()
        {
            this.FindEnumerableError(nameof(soundClips), soundClips);
            this.FindStringError(nameof(SoundName) ,SoundName);
        }
        private void Awake()
        {
            this.FindEnumerableError(nameof(soundClips), soundClips);
            this.FindStringError(nameof(SoundName), SoundName);
        }
    }
    [System.Serializable]
    public class ACWrapperEntry
    {
        public AudioClip clip;
        [Range(0f,80f)]
        public float PitchVariancePercent = 5f;
        [Range(0.2f,3f)]
        public float PitchOrigin = 1f;
        [Range(0.01f,1f)]
        public float Volume = 0.7f;
        public bool Muted;
        public string name => clip.name;
    }
}
