using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AudioGroup
{
    public AudioGroupEntry[] AudioGroupEntries = new AudioGroupEntry[0];

    public AudioClip GetRandomClip()
    {
        if (AudioGroupEntries.Length > 0)
        {
            List<AudioClip> audioClips = new List<AudioClip>();
            List<float> randomWeights = new List<float>();

            for (int i = 0; i < AudioGroupEntries.Length; i++)
            {
                AudioGroupEntry audioGroupEntry = AudioGroupEntries[i];

                AudioClip audioClip = audioGroupEntry.GetAudioClip();

                if (audioClip != null)
                {
                    audioClips.Add(audioClip);
                    randomWeights.Add(audioGroupEntry.RandomWeight);
                }
            }

            if (audioClips.Count > 0)
            {
                return audioClips[StaticHelper.GetRandomIndexByWeight(randomWeights.ToArray())];
            }
        }

        return null;
    }

    public class AudioGroupEntry
    {
        public string AudioClipKey = "";

        public float RandomWeight = 1.0f;

        public AudioClip GetAudioClip()
        {
            return ResourceManager.Instance.GetAudioClip(AudioClipKey);
        }
    }
}
