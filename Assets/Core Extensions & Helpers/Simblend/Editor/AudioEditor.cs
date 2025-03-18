using UnityEditor;
using UnityEngine;
using System.IO;
using System.Text.RegularExpressions;

#if UNITY_EDITOR
public class AudioEditor : EditorWindow
{
    private AudioSource previewAudioSource;
    private bool isPlaying = false;
    private float[] trimmedSamples;
    private bool isAudioAdded = false;

    private AudioClip audioClip;
    private float startTrim = 0f;
    private float endTrim = 0f;
    private float fadeStartDuration = 0f;
    private float fadeEndDuration = 0f;
    private bool loopPreview = false;

    private Texture2D waveformTexture; // New field to store the waveform texture
    private const int waveformWidth = 500; // Width of the waveform
    private const int waveformHeight = 100; // Height of the waveform

    string folder => outputFolder == null ? "Assets/Simblend Default Output" : AssetDatabase.GetAssetPath(outputFolder) + "/" + Regex.IsMatch(outputFolder.name, @"^[a-zA-Z0-9_]+$");
    UnityEngine.Object outputFolder;

    [MenuItem("Bremsengine/Simblend Audio Editor")]
    public static void ShowWindow()
    {
        GetWindow<AudioEditor>("Simblend");
    }

    private void OnEnable()
    {
        if (previewAudioSource == null)
        {
            // Create an audio source for previewing the sound
            GameObject audioPreviewer = new GameObject("AudioPreviewer");
            previewAudioSource = audioPreviewer.AddComponent<AudioSource>();
            previewAudioSource.hideFlags = HideFlags.HideAndDontSave;
        }
    }

    private void OnDisable()
    {
        if (previewAudioSource != null)
        {
            DestroyImmediate(previewAudioSource.gameObject);
        }
    }

    private void OnGUI()
    {
        GUILayout.Label("Trim and Fade Audio Clip", EditorStyles.boldLabel);

        audioClip = (AudioClip)EditorGUILayout.ObjectField("Audio Clip", audioClip, typeof(AudioClip), false);
        outputFolder = EditorGUILayout.ObjectField("Output Folder", outputFolder, typeof(UnityEngine.Object), false);

        if (audioClip == null || outputFolder == null)
        {
            GUILayout.Label("Select an Audio Clip to Edit, and an Output Folder.\n" +
                "The output folder must be a folder even thought u can drag a file in the field.\n" +
                "my brain is too small to make it only take folders.");

            GUILayout.Space(25f);

            GUILayout.Label("epically copy pasted from Simblend.");
            GUILayout.Space(5);
            GUILayout.Label("Links:");
            GUILayout.TextField("https://github.com/SimblendGames/Simblend-Editor");
            GUILayout.TextField("https://www.youtube.com/watch?v=7mMdf_JuC3o");
            GUILayout.TextField("https://www.reddit.com/r/Unity2D/comments/1fg9j1o/i_found_myself_having_to_trim_audios_often_and/");
            return;
        }

        if (audioClip != null)
        {
            // Display waveform with the trimmed and faded section
            if (waveformTexture == null || GUILayout.Button("Generate Waveform"))
            {
                waveformTexture = DrawWaveform(audioClip, waveformWidth, waveformHeight, new Color(1, 0.5f, 0), startTrim, endTrim, fadeStartDuration, fadeEndDuration);
            }

            // Display waveform texture
            if (waveformTexture != null)
            {
                GUILayout.Label("Waveform Preview");
                GUILayout.Box(waveformTexture);

                if (isPlaying)
                {
                    //Display the playhead
                    Rect waveformRect = GUILayoutUtility.GetLastRect();
                    float playheadPosition = Mathf.Min(((previewAudioSource.time) / (previewAudioSource.clip.length / 2f)) * waveformRect.width, waveformRect.width);
                    Rect playheadRect = new Rect(playheadPosition, GUILayoutUtility.GetLastRect().y, 2, waveformHeight);
                    EditorGUI.DrawRect(playheadRect, Color.red);
                }

            }

            // Sliders to control startTrim and endTrim
            startTrim = EditorGUILayout.Slider("Start Trim", startTrim, 0f, audioClip.length);
            endTrim = EditorGUILayout.Slider("End Trim", endTrim, 0f, audioClip.length);
            fadeStartDuration = EditorGUILayout.Slider("Fade Start Duration", fadeStartDuration, 0f, endTrim - startTrim);
            fadeEndDuration = EditorGUILayout.Slider("Fade End Duration", fadeEndDuration, 0f, endTrim - startTrim);
            loopPreview = GUILayout.Toggle(loopPreview, "Loop Preview");


            // Update waveform texture when sliders or fade values are adjusted
            if (GUI.changed)
            {
                waveformTexture = DrawWaveform(audioClip, waveformWidth, waveformHeight, new Color(1, 0.5f, 0), startTrim, endTrim, fadeStartDuration, fadeEndDuration);
            }

            if (!isAudioAdded)
            {
                endTrim = audioClip.length;
                isAudioAdded = true;
            }

            // Add Play, Pause, Stop Buttons
            GUILayout.Space(10);
            GUILayout.BeginHorizontal();

            if (GUILayout.Button("Play"))
            {
                PlayPreview();
            }
            if (GUILayout.Button("Stop"))
            {
                StopPreview();
            }

            GUILayout.EndHorizontal();

            if (GUILayout.Button("Trim, Fade, and Save"))
            {
                TrimAndFadeAudioClip();
            }
        }
        else
        {
            isAudioAdded = false;
        }

        if (isPlaying)
        {
            Repaint();
        }
    }

    private void PlayPreview()
    {
        if (previewAudioSource == null || audioClip == null)
            return;

        // If already playing, stop before playing again
        if (isPlaying)
        {
            StopPreview();
        }

        // Trim and fade the audio for preview
        trimmedSamples = TrimAndFadeAudioSamples(audioClip, startTrim, endTrim, fadeStartDuration, fadeEndDuration);
        AudioClip trimmedClip = AudioClip.Create("TrimmedClip", trimmedSamples.Length, audioClip.channels, audioClip.frequency, false);
        trimmedClip.SetData(trimmedSamples, 0);
        previewAudioSource.loop = loopPreview; // Add looping control
        previewAudioSource.clip = trimmedClip;
        previewAudioSource.Play();

        isPlaying = true;
    }

    private void StopPreview()
    {
        if (previewAudioSource == null || !isPlaying)
            return;

        previewAudioSource.Stop();
        isPlaying = false;
    }

    private float[] TrimAndFadeAudioSamples(AudioClip clip, float startTrim, float endTrim, float fadeStartDuration, float fadeEndDuration)
    {
        int startSample = Mathf.FloorToInt(startTrim * clip.frequency * clip.channels);
        int endSample = Mathf.FloorToInt(endTrim * clip.frequency * clip.channels);
        int trimSamples = endSample - startSample;

        float[] samples = new float[clip.samples * clip.channels];
        clip.GetData(samples, 0);

        float[] trimmedSamples = new float[trimSamples];
        System.Array.Copy(samples, startSample, trimmedSamples, 0, trimSamples);

        // Apply fade-in and fade-out
        int fadeInSampleCount = Mathf.FloorToInt(fadeStartDuration * clip.frequency * clip.channels);
        int fadeOutSampleCount = Mathf.FloorToInt(fadeEndDuration * clip.frequency * clip.channels);

        // Apply fade-in
        for (int i = 0; i < fadeInSampleCount && i < trimmedSamples.Length; i++)
        {
            float fadeFactor = (float)i / fadeInSampleCount;
            trimmedSamples[i] *= fadeFactor;
        }

        // Apply fade-out
        if (fadeEndDuration > 0)
        {

            // Ensure fade out does not exceed the length of the trimmed data
            int fadeOutStart = trimmedSamples.Length - fadeOutSampleCount;

            for (int i = 0; i < fadeOutSampleCount && i < trimmedSamples.Length; i++)
            {
                // Calculate the fade factor, which starts at 1 and decreases to 0
                float fadeFactor = 1f - ((float)i / fadeOutSampleCount);

                // Apply fade-out to the end of the trimmed data
                trimmedSamples[fadeOutStart + i] *= fadeFactor;
            }
        }

        return trimmedSamples;
    }

    private Texture2D DrawWaveform(AudioClip clip, int width, int height, Color waveformColor, float startTrim, float endTrim, float fadeStartDuration, float fadeEndDuration)
    {
        Texture2D texture = new Texture2D(width, height);
        float[] samples = new float[clip.samples * clip.channels];
        clip.GetData(samples, 0);

        Color[] colors = new Color[width * height];
        for (int i = 0; i < colors.Length; i++)
        {
            colors[i] = new Color(0.2f, 0.2f, 0.2f); // Background color
        }

        // Calculate the range of samples based on trim
        int startSample = Mathf.FloorToInt(startTrim * clip.frequency * clip.channels);
        int endSample = Mathf.FloorToInt(endTrim * clip.frequency * clip.channels);
        int trimSamples = endSample - startSample;

        // Calculate fade-in and fade-out sample ranges
        int fadeInSampleCount = Mathf.FloorToInt(fadeStartDuration * clip.frequency * clip.channels);
        int fadeOutSampleCount = Mathf.FloorToInt(fadeEndDuration * clip.frequency * clip.channels);

        int packSize = (trimSamples / width) + 1; // Calculate packSize based on the trimmed range

        for (int i = 0; i < width; i++)
        {
            float max = 0;
            for (int j = 0; j < packSize; j++)
            {
                int index = startSample + (i * packSize) + j;
                if (index < samples.Length)
                {
                    float wavePeak = Mathf.Abs(samples[index]);

                    // Apply fade-in based on the exact sample range
                    int currentSampleIndex = startSample + (i * packSize);
                    if (currentSampleIndex < startSample + fadeInSampleCount)
                    {
                        float fadeFactor = (float)(currentSampleIndex - startSample) / fadeInSampleCount;
                        wavePeak *= fadeFactor;
                    }

                    // Apply fade-out based on the exact sample range
                    if (currentSampleIndex > endSample - fadeOutSampleCount)
                    {
                        float fadeFactor = (float)(endSample - currentSampleIndex) / fadeOutSampleCount;
                        wavePeak *= fadeFactor;
                    }

                    if (wavePeak > max) max = wavePeak;
                }
            }

            int heightPos = Mathf.FloorToInt(max * (height / 2));
            for (int j = 0; j < heightPos; j++)
            {
                colors[(height / 2 + j) * width + i] = waveformColor;
                colors[(height / 2 - j) * width + i] = waveformColor;
            }
        }

        texture.SetPixels(colors);
        texture.Apply();

        return texture;
    }

    private void TrimAndFadeAudioClip()
    {
        if (audioClip == null)
        {
            Debug.LogError("No audio clip selected for trimming.");
            return;
        }

        float length = endTrim - startTrim;
        if (length <= 0)
        {
            Debug.LogError("Invalid trim values. The end trim must be greater than the start trim.");
            return;
        }
        string path = Path.Combine(Path.GetDirectoryName(folder), audioClip.name + " Simblend.wav");

        // Ensure the directory exists
        string directory = Path.GetDirectoryName(path);
        if (!Directory.Exists(directory))
        {
            if (File.Exists(directory))
            {
                Debug.LogError("You probably put a file in the folder slot for Simblend Audio Editor, pleae put a folder instead.");
                return;
            }
            Directory.CreateDirectory(directory);
        }

        // Create trimmed and faded AudioClip
        AudioClip trimmedClip = TrimAndFadeClip(audioClip, startTrim, length, fadeStartDuration, fadeEndDuration);
        SaveAsWav(trimmedClip, path);

        Debug.Log($"Trimmed and faded audio clip saved to {path}");
        AssetDatabase.Refresh();
    }

    private AudioClip TrimAndFadeClip(AudioClip clip, float startTime, float length, float fadeStartDuration, float fadeEndDuration)
    {
        float[] data = new float[clip.samples * clip.channels];
        clip.GetData(data, 0);

        int startSample = Mathf.FloorToInt(startTime * clip.frequency * clip.channels);
        int endSample = Mathf.FloorToInt((startTime + length) * clip.frequency * clip.channels);

        float[] trimmedData = new float[endSample - startSample];
        System.Array.Copy(data, startSample, trimmedData, 0, trimmedData.Length);

        // Apply fade-in
        if (fadeStartDuration > 0)
        {
            int fadeStartSamples = Mathf.FloorToInt(fadeStartDuration * clip.frequency * clip.channels);
            for (int i = 0; i < fadeStartSamples && i < trimmedData.Length; i++)
            {
                float fadeFactor = (float)i / fadeStartSamples;
                trimmedData[i] *= fadeFactor;
            }
        }

        // Apply fade-out
        if (fadeEndDuration > 0)
        {
            int fadeOutSampleCount = Mathf.FloorToInt(fadeEndDuration * clip.frequency * clip.channels);
            // Ensure fade out does not exceed the length of the trimmed data
            int fadeOutStart = trimmedSamples.Length - fadeOutSampleCount;

            for (int i = 0; i < fadeOutSampleCount && i < trimmedSamples.Length; i++)
            {
                // Calculate the fade factor, which starts at 1 and decreases to 0
                float fadeFactor = 1f - ((float)i / fadeOutSampleCount);

                // Apply fade-out to the end of the trimmed data
                trimmedData[fadeOutStart + i] *= fadeFactor;
            }
        }

        AudioClip newClip = AudioClip.Create(clip.name + "_EDITED", trimmedData.Length / clip.channels, clip.channels, clip.frequency, false);
        newClip.SetData(trimmedData, 0);

        return newClip;
    }

    private void SaveAsWav(AudioClip clip, string path)
    {
        if (clip == null)
        {
            Debug.LogError("Clip is null, cannot save as WAV.");
            return;
        }

        byte[] wavData = WavUtility.FromAudioClip(clip);
        File.WriteAllBytes(path, wavData);
    }
}
#endif