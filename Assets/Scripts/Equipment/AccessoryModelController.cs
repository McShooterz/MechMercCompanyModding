using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccessoryModelController : MonoBehaviour
{
    [SerializeField]
    protected AudioSource audioSource;

    [SerializeField]
    protected Animator[] animators;

    public AudioSource AudioSource { get => audioSource; }

    public void InitializeAudioSource()
    {
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        audioSource.spatialBlend = 1.0f;
        audioSource.maxDistance = 100.0f;
        audioSource.outputAudioMixerGroup = AudioManager.Instance.audioMixerGroupWeapons;
    }

    public void SetAnimatorsState(bool state)
    {
        for (int i = 0; i < animators.Length; i++)
        {
            animators[i].enabled = state;
        }
    }
}
