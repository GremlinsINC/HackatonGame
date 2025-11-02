using UnityEngine;
using System.Collections.Generic;

public static class AudioMixerUtility
{
    public static AudioClip MixClips(List<AudioClip> clips, string name = "MixedClip")
    {
        if (clips == null || clips.Count == 0)
            return null;

        int maxSamples = 0;
        int channels = clips[0].channels;
        int frequency = clips[0].frequency;

        foreach (var clip in clips)
        {
            if (clip == null) continue;
            maxSamples = Mathf.Max(maxSamples, clip.samples);
        }

        float[] mixedData = new float[maxSamples * channels];
        float[] buffer = new float[maxSamples * channels];

        foreach (var clip in clips)
        {
            if (clip == null) continue;
            buffer = new float[clip.samples * clip.channels];
            clip.GetData(buffer, 0);

            for (int i = 0; i < buffer.Length; i++)
            {
                mixedData[i] += buffer[i];
            }
        }

        float max = 0f;
        foreach (float s in mixedData)
            if (Mathf.Abs(s) > max) max = Mathf.Abs(s);
        if (max > 1f)
        {
            for (int i = 0; i < mixedData.Length; i++)
                mixedData[i] /= max;
        }

        AudioClip mixedClip = AudioClip.Create(name, maxSamples, channels, frequency, false);
        mixedClip.SetData(mixedData, 0);
        return mixedClip;
    }
}

