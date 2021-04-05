using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [SerializeField]
    AudioSource audioSourceUI;

    [SerializeField]
    AudioSource audioSourceMusic;

    public AudioMixerGroup audioMixerGroupMusic;

    public AudioMixerGroup audioMixerGroupUI;

    public AudioMixerGroup audioMixerGroupAmbience;

    public AudioMixerGroup audioMixerGroupEffects;

    public AudioMixerGroup audioMixerGroupMechs;

    public AudioMixerGroup audioMixerGroupVoices;

    public AudioMixerGroup audioMixerGroupWeapons;

    [SerializeField]
    public float pitchMin = 0.95f;

    [SerializeField]
    public float pitchMax = 1.05f;

    [SerializeField]
    int soundEffectLimit = 5;

    [SerializeField]
    int soundEffectCount = 0;

    [SerializeField]
    AudioClip[] buttonClicks;

    public float RandomPitch
    {
        get
        {
            return Random.Range(pitchMin, pitchMax);
        }
    }

    AudioSource AudioSourceMusic
    {
        get
        {
            if (audioSourceMusic == null)
            {
                audioSourceMusic = gameObject.AddComponent<AudioSource>();
                audioSourceMusic.outputAudioMixerGroup = audioMixerGroupMusic;
                audioSourceMusic.loop = true;
                audioSourceMusic.ignoreListenerPause = true;
            }

            return audioSourceMusic;
        }
    }

    AudioSource AudioSourceUI
    {
        get
        {
            if (audioSourceUI == null)
            {
                audioSourceUI = gameObject.AddComponent<AudioSource>();
                audioSourceUI.outputAudioMixerGroup = audioMixerGroupUI;
                audioSourceUI.ignoreListenerPause = true;
            }

            return audioSourceUI;
        }
    }

    void Awake()
    {
        //Make a Singleton
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        //This stays in every scene
        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayClipUI(AudioClip audioClip)
    {
        AudioSourceUI.PlayOneShot(audioClip);
    }

    public void PlayClipUI(AudioClip audioClip, float volume)
    {
        AudioSourceUI.PlayOneShot(audioClip, volume);
    }

    public void PlayClipMusic(AudioClip audioClip)
    {
        AudioSourceMusic.clip = audioClip;
        audioSourceMusic.Play();
    }

    public void PlayClip(AudioSource audioSource, AudioClip audioClip, bool useRandomPitch, bool limited)
    {
        if (limited)
        {
            if (soundEffectCount > soundEffectLimit)
            {
                return;
            }

            if (soundEffectCount == 0)
            {
                StartCoroutine(ResetSoundEffectCount());
            }

            soundEffectCount++;
        }

        if (useRandomPitch)
        {
            audioSource.pitch = RandomPitch;
        }

        audioSource.PlayOneShot(audioClip);
    }

    public void PlayClip(AudioSource audioSource, AudioClip audioClip, float volume, bool useRandomPitch, bool limited)
    {
        if (limited)
        {
            if (soundEffectCount > soundEffectLimit)
            {
                return;
            }

            if (soundEffectCount == 0)
            {
                StartCoroutine(ResetSoundEffectCount());
            }

            soundEffectCount++;
        }


        if (useRandomPitch)
        {
            audioSource.pitch = RandomPitch;
        }

        audioSource.PlayOneShot(audioClip, volume);
    }

    public void PlayButtonClick(int index)
    {
        if (index < 0)
        {
            index = 0;
        }
        else if (index >= buttonClicks.Length)
        {
            index = buttonClicks.Length - 1;
        }

        PlayClipUI(buttonClicks[index]);
    }

    public void StopPlayingMusic()
    {
        if (audioSourceMusic != null)
        {
            audioSourceMusic.Stop();
        }
    }

    IEnumerator ResetSoundEffectCount()
    {
        yield return new WaitForSeconds(0.07f);
        soundEffectCount = 0;
    }
}
