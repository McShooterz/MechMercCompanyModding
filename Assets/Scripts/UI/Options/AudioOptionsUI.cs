using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class AudioOptionsUI : MonoBehaviour
{
    [Header("Sliders")]

    [SerializeField]
    Slider masterVolumeSlider;

    [SerializeField]
    Slider musicVolumeSlider;

    [SerializeField]
    Slider uiVolumeSlider;

    [SerializeField]
    Slider weaponsVolumeSlider;

    [SerializeField]
    Slider effectsVolumeSlider;

    [SerializeField]
    Slider ambienceVolumeSlider;

    [SerializeField]
    Slider voicesVolumeSlider;

    [SerializeField]
    Slider mechsVolumeSlider;

    [Header("Text Values")]

    [Space(10)]

    [SerializeField]
    Text masterVolumeText;

    [SerializeField]
    Text musicVolumeText;

    [SerializeField]
    Text uiVolumeText;

    [SerializeField]
    Text weaponsVolumeText;

    [SerializeField]
    Text effectsVolumeText;

    [SerializeField]
    Text ambienceVolumeText;

    [SerializeField]
    Text voicesVolumeText;

    [SerializeField]
    Text mechsVolumeText;

    [Space(15)]

    [Header("Audio Mixers")]

    [SerializeField]
    AudioMixer audioMixerMaster;

    [SerializeField]
    AudioMixer audioMixerGameplay;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnEnable()
    {
        masterVolumeSlider.SetValueWithoutNotify(AudioListener.volume);

        musicVolumeSlider.SetValueWithoutNotify((float)System.Math.Round(StaticHelper.GetVolumeFromMixer(audioMixerMaster, "MusicVolume"), 2));
        weaponsVolumeSlider.SetValueWithoutNotify((float)System.Math.Round(StaticHelper.GetVolumeFromMixer(audioMixerGameplay, "WeaponsVolume"), 2));
        effectsVolumeSlider.SetValueWithoutNotify((float)System.Math.Round(StaticHelper.GetVolumeFromMixer(audioMixerGameplay, "EffectsVolume"), 2));
        ambienceVolumeSlider.SetValueWithoutNotify((float)System.Math.Round(StaticHelper.GetVolumeFromMixer(audioMixerGameplay, "AmbienceVolume"), 2));
        uiVolumeSlider.SetValueWithoutNotify((float)System.Math.Round(StaticHelper.GetVolumeFromMixer(audioMixerMaster, "UIVolume"), 2));
        voicesVolumeSlider.SetValueWithoutNotify((float)System.Math.Round(StaticHelper.GetVolumeFromMixer(audioMixerGameplay, "VoicesVolume"), 2));
        mechsVolumeSlider.SetValueWithoutNotify((float)System.Math.Round(StaticHelper.GetVolumeFromMixer(audioMixerGameplay, "MechsVolume"), 2));

        masterVolumeText.text = GetVolumeText(masterVolumeSlider.value);
        musicVolumeText.text = GetVolumeText(musicVolumeSlider.value);
        uiVolumeText.text = GetVolumeText(uiVolumeSlider.value);
        weaponsVolumeText.text = GetVolumeText(weaponsVolumeSlider.value);
        effectsVolumeText.text = GetVolumeText(effectsVolumeSlider.value);
        ambienceVolumeText.text = GetVolumeText(ambienceVolumeSlider.value);
        voicesVolumeText.text = GetVolumeText(voicesVolumeSlider.value);
        mechsVolumeText.text = GetVolumeText(mechsVolumeSlider.value);
    }

    public void OnValueChangedMasterVolume()
    {
        masterVolumeSlider.SetValueWithoutNotify((float)System.Math.Round(masterVolumeSlider.value, 2));
        AudioListener.volume = masterVolumeSlider.value;

        masterVolumeText.text = GetVolumeText(masterVolumeSlider.value);
    }

    public void OnValueChangedMusicVolume()
    {
        musicVolumeSlider.SetValueWithoutNotify((float)System.Math.Round(musicVolumeSlider.value, 2));
        audioMixerMaster.SetFloat("MusicVolume", StaticHelper.VolumeToDecibel(musicVolumeSlider.value));

        musicVolumeText.text = GetVolumeText(musicVolumeSlider.value);
    }

    public void OnValueChangedUIVolume()
    {
        uiVolumeSlider.SetValueWithoutNotify((float)System.Math.Round(uiVolumeSlider.value, 2));
        audioMixerMaster.SetFloat("UIVolume", StaticHelper.VolumeToDecibel(uiVolumeSlider.value));

        uiVolumeText.text = GetVolumeText(uiVolumeSlider.value);
    }

    public void OnValueChangedWeaponsVolume()
    {
        weaponsVolumeSlider.SetValueWithoutNotify((float)System.Math.Round(weaponsVolumeSlider.value, 2));
        audioMixerGameplay.SetFloat("WeaponsVolume", StaticHelper.VolumeToDecibel(weaponsVolumeSlider.value));

        weaponsVolumeText.text = GetVolumeText(weaponsVolumeSlider.value);
    }

    public void OnValueChangedEffectsVolume()
    {
        effectsVolumeSlider.SetValueWithoutNotify((float)System.Math.Round(effectsVolumeSlider.value, 2));
        audioMixerGameplay.SetFloat("EffectsVolume", StaticHelper.VolumeToDecibel(effectsVolumeSlider.value));

        effectsVolumeText.text = GetVolumeText(effectsVolumeSlider.value);
    }

    public void OnValueChangedAmbienceVolume()
    {
        ambienceVolumeSlider.SetValueWithoutNotify((float)System.Math.Round(ambienceVolumeSlider.value, 2));
        audioMixerGameplay.SetFloat("AmbienceVolume", StaticHelper.VolumeToDecibel(ambienceVolumeSlider.value));

        ambienceVolumeText.text = GetVolumeText(ambienceVolumeSlider.value);
    }

    public void OnValueChangedVoicesVolume()
    {
        voicesVolumeSlider.SetValueWithoutNotify((float)System.Math.Round(voicesVolumeSlider.value, 2));
        audioMixerGameplay.SetFloat("VoicesVolume", StaticHelper.VolumeToDecibel(voicesVolumeSlider.value));

        voicesVolumeText.text = GetVolumeText(voicesVolumeSlider.value);
    }

    public void OnValueChangedMechsVolume()
    {
        mechsVolumeSlider.SetValueWithoutNotify((float)System.Math.Round(mechsVolumeSlider.value, 2));
        audioMixerGameplay.SetFloat("MechsVolume", StaticHelper.VolumeToDecibel(mechsVolumeSlider.value));

        mechsVolumeText.text = GetVolumeText(mechsVolumeSlider.value);
    }

    string GetVolumeText(float value)
    {
        return (value * 100).ToString("0.") + "%";
    }
}
