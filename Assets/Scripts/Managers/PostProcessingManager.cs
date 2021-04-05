using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class PostProcessingManager : MonoBehaviour
{
	public static PostProcessingManager Instance { get; private set; }

	[SerializeField]
    PostProcessVolume postProcessVolume;

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
        SetUseBloom(CurrentConfig.UseBloom);
        SetBloomValue(CurrentConfig.BloomValue);
        SetUseAmbientOcclusion(CurrentConfig.UseAmbientOcclusion);
        SetAmbientOcclusionValue(CurrentConfig.AmbientOcclusionValue);
        SetUseAutoExposure(CurrentConfig.UseAutoExposure);
        SetUseChromaticAbberation(CurrentConfig.UseChromaticAbberation);
        SetUseColorGrading(CurrentConfig.UseColorGrading);
        SetUseDepthOfField(CurrentConfig.UseDepthOfField);
        SetUseGrain(CurrentConfig.UseGrain);
        SetUseMotionBlur(CurrentConfig.UseMotionBlur);
        SetUseScreenSpaceReflections(CurrentConfig.UseScreenSpaceReflections);
        SetUseVignette(CurrentConfig.UseVignette);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetUseBloom(bool state)
    {
        Bloom bloom;
		
        if (postProcessVolume.profile.TryGetSettings(out bloom))
        {
			bloom.enabled.value = state;
        }
    }

    public void SetBloomValue(float value)
    {
        Bloom bloom;

        if (postProcessVolume.profile.TryGetSettings(out bloom))
        {
            bloom.intensity.value = value;
        }
    }

    public void SetUseAmbientOcclusion(bool state)
    {
        AmbientOcclusion ambientOcclusion;

        if (postProcessVolume.profile.TryGetSettings(out ambientOcclusion))
        {
            ambientOcclusion.enabled.value = state;
        }
    }

    public void SetAmbientOcclusionValue(float value) 
	{
		AmbientOcclusion ambientOcclusion;

		if (postProcessVolume.profile.TryGetSettings(out ambientOcclusion)) 
		{
			ambientOcclusion.intensity.value = value;
		}	
	}

	public void SetUseAutoExposure(bool state) 
	{
		AutoExposure autoExposure;

		if (postProcessVolume.profile.TryGetSettings(out autoExposure)) 
		{
			autoExposure.enabled.value = state;
		}
	}

	public void SetUseChromaticAbberation(bool state) 
	{
		ChromaticAberration chromaticAberration;

		if (postProcessVolume.profile.TryGetSettings(out chromaticAberration)) 
		{
			chromaticAberration.enabled.value = state;
		}
	}

	public void SetUseColorGrading(bool state) 
	{
		ColorGrading colorGrading;

		if (postProcessVolume.profile.TryGetSettings(out colorGrading)) 
		{
			colorGrading.enabled.value = state;
		}
	}

	
	public void SetUseDepthOfField(bool state) 
	{
		DepthOfField depthOfField;

		if (postProcessVolume.profile.TryGetSettings(out depthOfField)) 
		{
			depthOfField.enabled.value = state;
		}
	}

	public void SetUseGrain(bool state) 
	{
		Grain grain;

		if (postProcessVolume.profile.TryGetSettings(out grain)) 
		{
			grain.enabled.value = state;
		}
	}

	public void SetUseMotionBlur(bool state) 
	{
		MotionBlur motionBlur;

		if (postProcessVolume.profile.TryGetSettings(out motionBlur)) 
		{
			motionBlur.enabled.value = state;
		}
	}

	public void SetUseScreenSpaceReflections(bool state) 
	{
		ScreenSpaceReflections screenSpaceReflections;

		if (postProcessVolume.profile.TryGetSettings(out screenSpaceReflections)) 
		{
			screenSpaceReflections.enabled.value = state;
		}
	}

	public void SetUseVignette(bool state) 
	{
		Vignette vignette;

		if (postProcessVolume.profile.TryGetSettings(out vignette)) 
		{
			vignette.enabled.value = state;
		}
	}
}
