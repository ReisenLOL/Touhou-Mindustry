using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;

namespace Core.Extensions
{
    #region Single Channel
    public static partial class AudioEngine
    {
        static Dictionary<ACWrapperEntry, AudioSource> singleChannels;
        private static bool TrySingleChannel(ACWrapperEntry a, out AudioSource source)
        {
            source = null;
            if (!singleChannels.ContainsKey(a))
            {
                source = singleChannels[a] = RequestChannel(a.ToString());
                source.outputAudioMixerGroup = SingleChannelsMixer;
               }
            else
            {
                source = singleChannels[a];
            }
            return source != null;
        }
    }

    #endregion
    #region Play Sound
    public static partial class AudioEngine
    {
        private static void PlayWrapper(AudioClipWrapper a, Vector2 position)
        {
            for (int i = 0; i < a.soundClips.Count; i++)
            {
                if (a.singleChannel && TrySingleChannel(a.Entries[i], out AudioSource s))
                {
                    s.transform.position = position;
                    s.PlayWrapper(a, i);
                    continue;
                }
                else
                {
                    SoundIteration = SoundQueue.Dequeue();
                    SoundQueue.Enqueue(SoundIteration);

                    SoundIteration.transform.position = position;
                    SoundIteration.PlayWrapper(a, i);
                }
            }
        }
        public static void Play(this AudioClipWrapper a, Vector2 position)
        {
            if (a == null)
                return;
            PlayWrapper(a, position);
        }
    }
    #endregion
    [DefaultExecutionOrder(5)]
    public static partial class AudioEngine
    {
        public static AudioMixerGroup DynamicChannelsMixer { get; private set; }
        public static AudioMixerGroup SingleChannelsMixer { get; private set; }
        const string DynamicChannelsKey = "Dynamic Channels";
        const string SingleChannelsKey = "Single Channels";
        const string AudioEngineAddressableKey = "Audio Engine";
        const string AudioEngine3DPlayerName = "3D Audio Channel";
        const string AudioEngine2DPlayerName = "2D Audio Channel";
        public static AudioSource Source3D;
        public static AudioSource Source2D;
        public const int SoundChannels = 32;
        static GameObject root;
        static Queue<AudioSource> SoundQueue;
        static List<AudioSource> SoundStack;
        static AudioSource SoundIteration;
        private static AudioSource RequestChannel(string name)
        {
            AudioSource source;
            GameObject g = new GameObject("Channel " + name);
            g.transform.SetParent(root.transform, false);
            source = g.AddComponent<AudioSource>();
            source.playOnAwake = false;
            source.loop = false;
            return source;
        }
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void Initialize()
        {
            singleChannels = new();
            SoundQueue = new();
            SoundStack = new();
            AudioSource iteration;
            root = new GameObject("Audio Engine");
            GameObject.DontDestroyOnLoad(root);
            for (int i = 0; i < SoundChannels; i++)
            {
                iteration = RequestChannel(i.ToString());
                SoundQueue.Enqueue(iteration);
                SoundStack.Add(iteration);
            }
        }
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        public static void AfterSceneLoad()
        {
            void DynamicsSetup(IList<AudioMixerGroup> mixers)
            {
                foreach (AudioMixerGroup group in mixers)
                {
                    if (group == null)
                        continue;

                    DynamicChannelsMixer = group;
                }
                if (DynamicChannelsMixer == null)
                {
                    Debug.LogWarning("Failed to find Mixer group for Audio Engine / Random Channels. See AudioEngine.cs to find the addressables string key for RandomChannelsKey");
                }
                foreach (var channel in SoundStack)
                {
                    channel.outputAudioMixerGroup = DynamicChannelsMixer;
                }
            }
            void SingleChannelsSetup(IList<AudioMixerGroup> mixers)
            {
                foreach (AudioMixerGroup group in mixers)
                {
                    if (group == null)
                        continue;

                    SingleChannelsMixer = group;
                }
                if (SingleChannelsMixer == null)
                {
                    Debug.LogWarning("Failed to find Mixer group for Audio Engine / Target Channels. See AudioEngine.cs to find the addressables string key for TargetChannelsKey");
                }
            }
            void SetupSources(IList<GameObject> sourceObjects)
            {
                List<AudioSource> sources = new();
                foreach (GameObject g in sourceObjects)
                {
                    if (g.GetComponent<AudioSource>() is AudioSource source and not null)
                    {
                        if (source == null)
                            continue;
                        sources.Add(source);
                        if (source.transform.name == AudioEngine3DPlayerName)
                        {
                            Source3D = source;
                        }
                        if (source.transform.name == AudioEngine2DPlayerName)
                        {
                            Source2D = source;
                        }
                    }
                }
            }
            AddressablesTools.LoadKeys<AudioMixerGroup>(DynamicChannelsKey, DynamicsSetup);
            AddressablesTools.LoadKeys<AudioMixerGroup>(SingleChannelsKey, SingleChannelsSetup);
            AddressablesTools.LoadKeys<GameObject>(AudioEngineAddressableKey, SetupSources);
        }
    }
}
