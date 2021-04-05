using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public abstract class InteractableBase : MonoBehaviour
{
    [SerializeField]
    protected AudioSource audioSource;

    [SerializeField]
    protected string displayName;

    [SerializeField]
    protected AudioClip interactionAudioClip;

    [SerializeField]
    protected string interactionAudioClipKey = "";

    public string DisplayName { get => displayName; }

    protected virtual void Start()
    {
        if (interactionAudioClip == null && interactionAudioClipKey != "")
        {
            interactionAudioClip = ResourceManager.Instance.GetAudioClip(interactionAudioClipKey);
        }
    }

    public virtual void Interact()
    {
        if (interactionAudioClip != null)
        {
            audioSource.PlayOneShot(interactionAudioClip);
        }
    }

    public virtual void StartHover()
    {

    }

    public virtual void EndHover()
    {

    }
}
