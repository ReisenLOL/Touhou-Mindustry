using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Extensions
{
    public static class AudioExtensions
    {
        public static bool TryPlayClip(this AudioSource a, AudioClip clip)
        {
            if (clip == null)
                return false;

            a.clip = clip;
            a.Play();
            return true;
        }
        public static void PlayWrapper(this AudioSource a, AudioClipWrapper sound, int index)
        {
            if (sound is AudioClipWrapper audio && audio.soundClips != null)
            {
                ACWrapperEntry entry = audio.soundClips[index];
                if (entry.Muted)
                    return;

                a.clip = entry.clip;
                a.pitch = entry.PitchOrigin.Spread(entry.PitchVariancePercent);
                a.volume = sound.GetVolume(index);
                sound.ApplyMixerOverride(a);
                a.Set3D(sound);
                a.Play();
            }
        }
        private static void Set3D(this AudioSource a, AudioClipWrapper w, float? maxDistance = null)
        {
            float _maxDistance;
            if (w.Is3D)
            {
                if (AudioEngine.Source3D is AudioSource a3D and not null)
                {
                    _maxDistance = (maxDistance ?? a3D.maxDistance).Max(20f);
                    a.rolloffMode = AudioRolloffMode.Custom;
                    a.SetCustomCurve(AudioSourceCurveType.CustomRolloff, a3D.GetCustomCurve(AudioSourceCurveType.CustomRolloff));
                    a.SetCustomCurve(AudioSourceCurveType.SpatialBlend, a3D.GetCustomCurve(AudioSourceCurveType.SpatialBlend));
                    a.SetCustomCurve(AudioSourceCurveType.Spread, a3D.GetCustomCurve(AudioSourceCurveType.Spread));
                    a.maxDistance = _maxDistance;
                    return;
                }
            }
            if (AudioEngine.Source2D is AudioSource a2D and not null)
            {
                a.rolloffMode = AudioRolloffMode.Custom;
                a.SetCustomCurve(AudioSourceCurveType.CustomRolloff, a2D.GetCustomCurve(AudioSourceCurveType.CustomRolloff));
                a.SetCustomCurve(AudioSourceCurveType.SpatialBlend, a2D.GetCustomCurve(AudioSourceCurveType.SpatialBlend));
                a.SetCustomCurve(AudioSourceCurveType.Spread, a2D.GetCustomCurve(AudioSourceCurveType.Spread));
            }
        }
    }
}
