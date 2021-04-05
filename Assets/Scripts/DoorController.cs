using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class DoorController : MonoBehaviour
{
    [SerializeField]
    Animator doorAnimator;

    [SerializeField]
    AudioSource audioSource;

    [SerializeField]
    string doorOpenAudioClipKey = "";

    [SerializeField]
    string doorCloseAudioClipKey = "";

    [SerializeField]
    AudioClip doorOpenAudioClip;

    [SerializeField]
    AudioClip doorCloseAudioClip;

    public Animator DoorAnimator { get => doorAnimator; }

    // Start is called before the first frame update
    void Start()
    {
        if (doorOpenAudioClip == null && doorOpenAudioClipKey != "")
        {
            doorOpenAudioClip = ResourceManager.Instance.GetAudioClip(doorOpenAudioClipKey);
        }

        if (doorCloseAudioClip == null && doorCloseAudioClipKey != "")
        {
            doorCloseAudioClip = ResourceManager.Instance.GetAudioClip(doorCloseAudioClipKey);
        }

        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.volume = 0.75f;
            audioSource.spatialBlend = 0.75f;
            audioSource.minDistance = 15.0f;
            audioSource.maxDistance = 100.0f;
        }
    }

    public void PlayOpenDoor()
    {
        if (doorOpenAudioClip != null)
        {
            audioSource.Stop();
            audioSource.PlayOneShot(doorOpenAudioClip);
        }
    }

    public void PlayCloseDoor()
    {
        if (doorCloseAudioClip != null)
        {
            audioSource.Stop();
            audioSource.PlayOneShot(doorCloseAudioClip);
        }
    }
}
