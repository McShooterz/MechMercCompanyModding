using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentDestructible : MonoBehaviour
{
    [SerializeField]
    GameObject[] objectsDisableOnDestroy = new GameObject[0];

    [SerializeField]
    GameObject[] objectsEnableOnDestroy = new GameObject[0];

    [SerializeField]
    AudioSource audioSource;

    [SerializeField]
    Rigidbody attachedRigidbody;

    [SerializeField]
    float health;

    [SerializeField]
    bool isDestroyed;

    [SerializeField]
    int destroyedLayer;

    [SerializeField]
    string destroyedAudioClipKey = "";

    [SerializeField]
    UnityEngine.Audio.AudioMixerGroup audioMixerGroup;

    public bool destroyOnCollision = false;

    [SerializeField]
    bool fallOnDestroyed = false;

    [SerializeField]
    float fallForceModifier = 0.0f;

    [SerializeField]
    float fallForceAngularModifier = 0.0f;

    [SerializeField]
    float cleanUpTime = 5.0f;

    float cleanUpTimer;

    public AudioSource AudioSource
    {
        get
        {
            if (audioSource == null)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
                audioSource.spatialBlend = 1.0f;
                audioSource.maxDistance = 100.0f;
                audioSource.outputAudioMixerGroup = audioMixerGroup;
            }

            return audioSource;
        }
    }

    Rigidbody AttachedRigidbody
    {
        get
        {
            if (attachedRigidbody == null)
            {
                attachedRigidbody = gameObject.AddComponent<Rigidbody>();
            }
            return attachedRigidbody;
        }
    }

    void Awake()
    {
        gameObject.layer = 20;
    }

    // Update is called once per frame
    void Update()
    {
        if (isDestroyed && Time.time > cleanUpTimer)
        {
            if (attachedRigidbody != null)
                Destroy(attachedRigidbody);

            if (audioSource != null)
                audioSource.enabled = false;

            enabled = false;
        }
    }

    public void TakeDamage(float damage)
    {
        if (isDestroyed)
        {
            return;
        }

        if (damage > health)
        {
            health = 0f;
            Die();
        }
        else
        {
            health -= damage;
        }
    }

    public void TakeDamage(float damage, Vector3 forceDirection)
    {
        if (isDestroyed)
        {
            return;
        }

        if (damage > health)
        {
            health = 0f;
            Die(forceDirection);
        }
        else
        {
            health -= damage;
        }
    }

    public void Die()
    {
        if (isDestroyed)
        {
            return;
        }

        isDestroyed = true;
        gameObject.layer = destroyedLayer;
        cleanUpTimer = Time.time + cleanUpTime;

        if (destroyedAudioClipKey != "")
        {
            AudioClip audioClip = ResourceManager.Instance.GetAudioClip(destroyedAudioClipKey);

            if (audioClip != null)
            {
                AudioManager.Instance.PlayClip(AudioSource, audioClip, true, true);
            }
        }

        if (fallOnDestroyed)
        {
            AttachedRigidbody.isKinematic = false;
            AttachedRigidbody.velocity = Random.onUnitSphere * fallForceModifier + Vector3.down;
            AttachedRigidbody.angularVelocity = Random.onUnitSphere * fallForceAngularModifier;
        }

        foreach (GameObject disableObject in objectsDisableOnDestroy)
        {
            disableObject.SetActive(false);
        }

        foreach (GameObject enableObject in objectsEnableOnDestroy)
        {
            enableObject.SetActive(true);
        }
    }

    public void Die(Vector3 deathForce)
    {
        if (isDestroyed)
        {
            return;
        }

        isDestroyed = true;
        gameObject.layer = destroyedLayer;
        cleanUpTimer = Time.time + cleanUpTime;

        if (destroyedAudioClipKey != "")
        {
            AudioClip audioClip = ResourceManager.Instance.GetAudioClip(destroyedAudioClipKey);

            if (audioClip != null)
            {
                AudioManager.Instance.PlayClip(AudioSource, audioClip, true, true);
            }
        }

        if (fallOnDestroyed)
        {
            AttachedRigidbody.isKinematic = false;
            AttachedRigidbody.velocity = (deathForce + Vector3.down) * fallForceModifier;
            AttachedRigidbody.angularVelocity = Random.onUnitSphere * fallForceAngularModifier;
        }

        foreach (GameObject disableObject in objectsDisableOnDestroy)
        {
            disableObject.SetActive(false);
        }

        foreach (GameObject enableObject in objectsEnableOnDestroy)
        {
            enableObject.SetActive(true);
        }
    }
}