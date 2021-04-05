using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class GraphicOptionsUI : MonoBehaviour
{
    #region Variables
    [Header("Components")]

    [SerializeField]
    Dropdown qualityLevelDropDown;

    [SerializeField]
    Dropdown resolutionDropDown;

    [SerializeField]
    Toggle fullScreenToggle;

    [SerializeField]
    Dropdown textureQualityDropDown;

    [SerializeField]
    Dropdown anisotropicFilteringDropDown;

    [SerializeField]
    Toggle realTimeReflectionProbesToggle;

    [SerializeField]
    Slider factorDPISlider;

    [SerializeField]
    Text factorDPIText;

    [SerializeField]
    Slider distanceLODSlider;

    [SerializeField]
    Text distanceLODText;

    [SerializeField]
    Dropdown maxLODDropDown;

    [SerializeField]
    Dropdown vSyncCountDropDown;

    [SerializeField]
    Dropdown animationBlendWeightsDropDown;

    [SerializeField]
    Toggle billboardFacingQualityToggle;

    [SerializeField]
    Dropdown shadowQualityDropDown;

    [SerializeField]
    Slider shadowDistanceSlider;

    [SerializeField]
    Text shadowDistanceText;

    [SerializeField]
    Dropdown shadowResolutionDropDown;

    [SerializeField]
    Dropdown shadowProjectionDropDown;

    [SerializeField]
    Dropdown shadowMaskModeDropDown;

    [SerializeField]
    Dropdown shadowCascadesDropDown;

    [SerializeField]
    Slider shadowNearPlaneOffsetSlider;

    [SerializeField]
    Text shadowNearPlaneOffsetText;

    [SerializeField]
    Dropdown pixelLightCountDropDown;

    [SerializeField]
    Toggle softParticlesToggle;

    [SerializeField]
    Toggle softVegetationToggle;

    [SerializeField]
    GameObject resolutionWarningPanel;

    [SerializeField]
    Text revertTimerText;

    [SerializeField]
    Slider frameLimitSlider;

    [SerializeField]
    Text frameLimitValueText;

    [Header("Post Processing")]

    [SerializeField]
    Dropdown antiAliasingModeDropDown;

    [SerializeField]
    Toggle bloomToggle;

    [SerializeField]
    Slider bloomSlider;

    [SerializeField]
    Text bloomValueText;

    [SerializeField]
    Toggle ambientOcclusionToggle;

    [SerializeField]
	Slider ambientOcclusionSlider;

    [SerializeField]
	Text ambientOcclusionValueText;

	[SerializeField]
	Toggle autoExpoureToggle;

	[SerializeField]
	Toggle chromaticAbberationToggle;
	
	[SerializeField]
	Toggle colorGradingToggle;
	
	[SerializeField]
	Toggle depthOfFieldToggle;

	[SerializeField]
	Toggle grainToggle;

	[SerializeField]
	Toggle motionBlurToggle;

	[SerializeField]
	Toggle screenSpaceReflectionToggle;

	[SerializeField]
	Toggle vignetteToggle;

    [Space(5)]

    [Header("Settings")]

    [SerializeField]
    int[] textureQualityValues = new int[] { 3, 2, 1, 0 };

    [SerializeField]
    int[] maxLODValues = new int[] { 2, 1, 0 };

    [SerializeField]
    AnisotropicFiltering[] anisotropicFilteringValues = new AnisotropicFiltering[] { AnisotropicFiltering.Disable, AnisotropicFiltering.Enable, AnisotropicFiltering.ForceEnable };

    [SerializeField]
    SkinWeights[] blendWeightsValues = new SkinWeights[] { SkinWeights.OneBone, SkinWeights.TwoBones, SkinWeights.FourBones };

    [SerializeField]
    ShadowQuality[] shadowQualityValues = new ShadowQuality[] { ShadowQuality.Disable, ShadowQuality.HardOnly, ShadowQuality.All };

    [SerializeField]
    ShadowResolution[] shadowResolutionValues = new ShadowResolution[] { ShadowResolution.Low, ShadowResolution.Medium, ShadowResolution.High, ShadowResolution.VeryHigh };

    [SerializeField]
    ShadowmaskMode[] shadowmaskModeValues = new ShadowmaskMode[] { ShadowmaskMode.Shadowmask, ShadowmaskMode.DistanceShadowmask };

    [SerializeField]
    ShadowProjection[] shadowProjectionValues = new ShadowProjection[] { ShadowProjection.CloseFit, ShadowProjection.StableFit };

    [SerializeField]
    int[] shadowCascadesValues = new int[] { 1, 2, 4 };

    [Header("Other")]

    [SerializeField]
    float revertResolutionTimer;

    [SerializeField]
    Resolution revertResolution;

    [SerializeField]
    bool revertFullScreen = false;
    #endregion

    void Awake()
    {
        resolutionWarningPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (resolutionWarningPanel.activeInHierarchy)
        {
            if (Time.unscaledTime > revertResolutionTimer)
            {
                Screen.SetResolution(revertResolution.width, revertResolution.height, revertFullScreen, revertResolution.refreshRate);
                SetResolutionDropDownUI();
                fullScreenToggle.SetIsOnWithoutNotify(revertFullScreen);
                resolutionWarningPanel.SetActive(false);
            }
            else
            {
                revertTimerText.text = (revertResolutionTimer - Time.unscaledTime).ToString("0.0");
            }
        }
    }

    void OnEnable()
    {
        resolutionWarningPanel.SetActive(false);

        int quality;

        if (ResourceManager.Instance.customGraphicsQuality)
        {
            quality = 6;
        }
        else
        {
            quality = QualitySettings.GetQualityLevel();
        }

        SetUIValues();

        qualityLevelDropDown.SetValueWithoutNotify(quality);

        if (quality != 6)
        {
            ResourceManager.Instance.customGraphicsQuality = false;
        }

        revertResolution = Screen.currentResolution;

        InitializeResolutionDropdown();
    }

    void SetUIValues()
    {
        switch (QualitySettings.masterTextureLimit)
        {
            case 0:
                {
                    textureQualityDropDown.SetValueWithoutNotify(3);
                    break;
                }
            case 1:
                {
                    textureQualityDropDown.SetValueWithoutNotify(2);
                    break;
                }
            case 2:
                {
                    textureQualityDropDown.SetValueWithoutNotify(1);
                    break;
                }
            case 3:
                {
                    textureQualityDropDown.SetValueWithoutNotify(0);
                    break;
                }
            default:
                {
                    print("Error");
                    break;
                }
        }

        switch (QualitySettings.anisotropicFiltering)
        {
            case AnisotropicFiltering.Disable:
                {
                    anisotropicFilteringDropDown.SetValueWithoutNotify(0);
                    break;
                }
            case AnisotropicFiltering.Enable:
                {
                    anisotropicFilteringDropDown.SetValueWithoutNotify(1);
                    break;
                }
            case AnisotropicFiltering.ForceEnable:
                {
                    anisotropicFilteringDropDown.SetValueWithoutNotify(2);
                    break;
                }
            default:
                {
                    print("Error");
                    break;
                }
        }

        switch (QualitySettings.maximumLODLevel)
        {
            case 0:
                {
                    maxLODDropDown.SetValueWithoutNotify(2);
                    break;
                }
            case 1:
                {
                    maxLODDropDown.SetValueWithoutNotify(1);
                    break;
                }
            case 2:
                {
                    maxLODDropDown.SetValueWithoutNotify(0);
                    break;
                }
            default:
                {
                    print("Error");
                    break;
                }
        }

        vSyncCountDropDown.SetValueWithoutNotify(QualitySettings.vSyncCount);

        switch (QualitySettings.skinWeights)
        {
            case SkinWeights.OneBone:
                {
                    animationBlendWeightsDropDown.SetValueWithoutNotify(0);
                    break;
                }
            case SkinWeights.TwoBones:
                {
                    animationBlendWeightsDropDown.SetValueWithoutNotify(1);
                    break;
                }
            case SkinWeights.FourBones:
                {
                    animationBlendWeightsDropDown.SetValueWithoutNotify(2);
                    break;
                }
            default:
                {
                    print("Error");
                    break;
                }
        }

        switch (QualitySettings.shadows)
        {
            case ShadowQuality.Disable:
                {
                    shadowQualityDropDown.SetValueWithoutNotify(0);
                    break;
                }
            case ShadowQuality.HardOnly:
                {
                    shadowQualityDropDown.SetValueWithoutNotify(1);
                    break;
                }
            case ShadowQuality.All:
                {
                    shadowQualityDropDown.SetValueWithoutNotify(2);
                    break;
                }
            default:
                {
                    print("Error");
                    break;
                }
        }

        switch (QualitySettings.shadowResolution)
        {
            case ShadowResolution.Low:
                {
                    shadowResolutionDropDown.SetValueWithoutNotify(0);
                    break;
                }
            case ShadowResolution.Medium:
                {
                    shadowResolutionDropDown.SetValueWithoutNotify(1);
                    break;
                }
            case ShadowResolution.High:
                {
                    shadowResolutionDropDown.SetValueWithoutNotify(2);
                    break;
                }
            case ShadowResolution.VeryHigh:
                {
                    shadowResolutionDropDown.SetValueWithoutNotify(3);
                    break;
                }
            default:
                {
                    print("Error");
                    break;
                }
        }

        switch (QualitySettings.shadowProjection)
        {
            case ShadowProjection.CloseFit:
                {
                    shadowProjectionDropDown.SetValueWithoutNotify(0);
                    break;
                }
            case ShadowProjection.StableFit:
                {
                    shadowProjectionDropDown.SetValueWithoutNotify(1);
                    break;
                }
            default:
                {
                    print("Error");
                    break;
                }
        }

        switch (QualitySettings.shadowmaskMode)
        {
            case ShadowmaskMode.Shadowmask:
                {
                    shadowMaskModeDropDown.SetValueWithoutNotify(0);
                    break;
                }
            case ShadowmaskMode.DistanceShadowmask:
                {
                    shadowMaskModeDropDown.SetValueWithoutNotify(1);
                    break;
                }
            default:
                {
                    print("Error");
                    break;
                }
        }

        switch (QualitySettings.shadowCascades)
        {
            case 1:
                {
                    shadowCascadesDropDown.SetValueWithoutNotify(0);
                    break;
                }
            case 2:
                {
                    shadowCascadesDropDown.SetValueWithoutNotify(1);
                    break;
                }
            case 4:
                {
                    shadowCascadesDropDown.SetValueWithoutNotify(2);
                    break;
                }
            default:
                {
                    print("Error");
                    break;
                }
        }

        pixelLightCountDropDown.SetValueWithoutNotify(QualitySettings.pixelLightCount);

        fullScreenToggle.SetIsOnWithoutNotify(Screen.fullScreen);

        realTimeReflectionProbesToggle.SetIsOnWithoutNotify(QualitySettings.realtimeReflectionProbes);

        factorDPISlider.SetValueWithoutNotify(QualitySettings.resolutionScalingFixedDPIFactor);
        factorDPIText.text = factorDPISlider.value.ToString("0.0");

        distanceLODSlider.SetValueWithoutNotify(QualitySettings.lodBias);
        distanceLODText.text = distanceLODSlider.value.ToString("0.0");

        billboardFacingQualityToggle.SetIsOnWithoutNotify(QualitySettings.billboardsFaceCameraPosition);
        softParticlesToggle.SetIsOnWithoutNotify(QualitySettings.softParticles);
        softVegetationToggle.SetIsOnWithoutNotify(QualitySettings.softVegetation);

        shadowDistanceSlider.SetValueWithoutNotify(QualitySettings.shadowDistance);
        shadowDistanceText.text = shadowDistanceSlider.value.ToString("0.0");

        shadowNearPlaneOffsetSlider.SetValueWithoutNotify(QualitySettings.shadowNearPlaneOffset);
        shadowNearPlaneOffsetText.text = shadowNearPlaneOffsetSlider.value.ToString("0.0");

        antiAliasingModeDropDown.SetValueWithoutNotify(CurrentConfig.AntiAliasingMode);

        bloomSlider.SetValueWithoutNotify(CurrentConfig.BloomValue);
        bloomValueText.text = bloomSlider.value.ToString("0.0");

		ambientOcclusionSlider.SetValueWithoutNotify(CurrentConfig.AmbientOcclusionValue);
		ambientOcclusionValueText.text = ambientOcclusionSlider.value.ToString("0.0");

		autoExpoureToggle.SetIsOnWithoutNotify(CurrentConfig.UseAutoExposure);
		chromaticAbberationToggle.SetIsOnWithoutNotify(CurrentConfig.UseChromaticAbberation);
		colorGradingToggle.SetIsOnWithoutNotify(CurrentConfig.UseColorGrading);
		depthOfFieldToggle.SetIsOnWithoutNotify(CurrentConfig.UseDepthOfField);
		grainToggle.SetIsOnWithoutNotify(CurrentConfig.UseGrain);
		motionBlurToggle.SetIsOnWithoutNotify(CurrentConfig.UseMotionBlur);
		screenSpaceReflectionToggle.SetIsOnWithoutNotify(CurrentConfig.UseScreenSpaceReflections);
		vignetteToggle.SetIsOnWithoutNotify(CurrentConfig.UseVignette);
		
        frameLimitSlider.SetValueWithoutNotify(Application.targetFrameRate);
        frameLimitValueText.text = frameLimitSlider.value.ToString();
    }

    void InitializeResolutionDropdown()
    {
        resolutionDropDown.options.Clear();

        foreach (Resolution resolution in Screen.resolutions)
        {
            resolutionDropDown.options.Add(new Dropdown.OptionData(StaticHelper.ResolutionToString(resolution)));
        }

        resolutionDropDown.captionText.text = StaticHelper.ResolutionToString(Screen.currentResolution);
        SetResolutionDropDownUI();
    }

    void SetResolutionDropDownUI()
    {
        for (int i = 0; i < Screen.resolutions.Length; i++)
        {
            if (Screen.currentResolution.width == Screen.resolutions[i].width && Screen.currentResolution.height == Screen.resolutions[i].height && Screen.currentResolution.refreshRate == Screen.resolutions[i].refreshRate)
            {
                resolutionDropDown.SetValueWithoutNotify(i);
                return;
            }
        }
    }

    public void OnChangeQualityLevel()
    {
        if (qualityLevelDropDown.value < 6)
        {
            int value = qualityLevelDropDown.value;
            QualitySettings.SetQualityLevel(value, false);
            SetUIValues();
            SetQualityDropDown(value);
            ResourceManager.Instance.customGraphicsQuality = false;
        }
        else
        {
            ResourceManager.Instance.customGraphicsQuality = true;
        }
    }

    public void OnChangeResolution()
    {
        if (resolutionWarningPanel.activeInHierarchy)
        {
            return;
        }

        revertResolutionTimer = Time.unscaledTime + 12f;
        revertResolution = Screen.currentResolution;
        revertFullScreen = Screen.fullScreen;

        //print("Revert Resolution: " + StaticHelper.ResolutionToString(revertResolution));

        resolutionWarningPanel.SetActive(true);

        Resolution resolution = Screen.resolutions[resolutionDropDown.value];
        Screen.SetResolution(resolution.width, resolution.height, fullScreenToggle.isOn, resolution.refreshRate);
    }

    public void OnChangeTextureQuality()
    {
        SetQualityDropDown(6);

        QualitySettings.masterTextureLimit = textureQualityValues[textureQualityDropDown.value];
    }

    public void OnChangeAnisotropicFiltering()
    {
        SetQualityDropDown(6);

        QualitySettings.anisotropicFiltering = anisotropicFilteringValues[anisotropicFilteringDropDown.value];
    }

    public void OnChangeRealTimeReflectionProbes()
    {
        SetQualityDropDown(6);

        QualitySettings.realtimeReflectionProbes = realTimeReflectionProbesToggle.isOn;
    }

    public void OnChangeDPIFactor()
    {
        SetQualityDropDown(6);

        QualitySettings.resolutionScalingFixedDPIFactor = factorDPISlider.value;

        factorDPIText.text = factorDPISlider.value.ToString("0.0");
    }

    public void OnChangeLevelOfDetailDistance()
    {
        SetQualityDropDown(6);

        QualitySettings.lodBias = distanceLODSlider.value;

        distanceLODText.text = distanceLODSlider.value.ToString("0.0");
    }

    public void OnChangeMaxLevelOfDetail()
    {
        QualitySettings.maximumLODLevel = maxLODValues[maxLODDropDown.value];
    }

    public void OnChangeVSyncCount()
    {
        SetQualityDropDown(6);

        QualitySettings.vSyncCount = vSyncCountDropDown.value;
    }

    public void OnChangeAnimationBlendWeights()
    {
        SetQualityDropDown(6);

        QualitySettings.skinWeights = blendWeightsValues[animationBlendWeightsDropDown.value];
    }

    public void OnChangeBillboardFacingQuality()
    {
        SetQualityDropDown(6);

        QualitySettings.billboardsFaceCameraPosition = billboardFacingQualityToggle.isOn;
    }

    public void OnChangeSoftParticles()
    {
        QualitySettings.softParticles = softParticlesToggle.isOn;
    }

    public void OnChangeSoftVegetation()
    {
        QualitySettings.softVegetation = softVegetationToggle.isOn;
    }

    public void OnChangeShadowQuality()
    {
        SetQualityDropDown(6);

        QualitySettings.shadows = shadowQualityValues[shadowQualityDropDown.value];
    }

    public void OnChangeShadowDistance()
    {
        SetQualityDropDown(6);

        QualitySettings.shadowDistance = shadowDistanceSlider.value;

        shadowDistanceText.text = shadowDistanceSlider.value.ToString("0.0");
    }

    public void OnChangeShadowResolution()
    {
        SetQualityDropDown(6);

        QualitySettings.shadowResolution = shadowResolutionValues[shadowResolutionDropDown.value];
    }

    public void OnChangeShadowProjection()
    {
        QualitySettings.shadowProjection = shadowProjectionValues[shadowProjectionDropDown.value];
    }

    public void OnChangeShadowMaskMode()
    {
        SetQualityDropDown(6);

        QualitySettings.shadowmaskMode = shadowmaskModeValues[shadowMaskModeDropDown.value];
    }

    public void OnChangeShadowCascades()
    {
        SetQualityDropDown(6);

        QualitySettings.shadowCascades = shadowCascadesValues[shadowCascadesDropDown.value];
    }

    public void OnChangeShadowNearPlaneOffset()
    {
        QualitySettings.shadowNearPlaneOffset = shadowNearPlaneOffsetSlider.value;

        shadowNearPlaneOffsetText.text = shadowNearPlaneOffsetSlider.value.ToString("0.0");
    }

    public void OnChangePixelLightCount()
    {
        SetQualityDropDown(6);

        QualitySettings.pixelLightCount = pixelLightCountDropDown.value;
    }

    public void OnChangeAntiAliasingMode()
    {
        CurrentConfig.AntiAliasingMode = antiAliasingModeDropDown.value;

		if (CameraController.Instance != null) 
		{
			CameraController.Instance.SetAntiAliasingMode(CurrentConfig.AntiAliasingMode);
		}
    }

    public void OnChangeUseBloom()
    {
        CurrentConfig.UseBloom = bloomToggle.isOn;

        PostProcessingManager.Instance.SetUseBloom(CurrentConfig.UseBloom);
    }

    public void OnChangeBloomValue()
    {
        bloomSlider.SetValueWithoutNotify((float)System.Math.Round(bloomSlider.value, 1));

        CurrentConfig.BloomValue = bloomSlider.value;
        bloomValueText.text = CurrentConfig.BloomValue.ToString("0.0");

        PostProcessingManager.Instance.SetBloomValue(CurrentConfig.BloomValue);
    }

    public void OnChangeUseAmbientOcclusion()
    {
        CurrentConfig.UseAmbientOcclusion = ambientOcclusionToggle.isOn;

        PostProcessingManager.Instance.SetUseAmbientOcclusion(CurrentConfig.UseAmbientOcclusion);
    }

    public void OnChangeAmbientOcclusionValue()
    {
        ambientOcclusionSlider.SetValueWithoutNotify((float)System.Math.Round(ambientOcclusionSlider.value, 1));
        CurrentConfig.AmbientOcclusionValue = ambientOcclusionSlider.value;
        ambientOcclusionValueText.text = CurrentConfig.AmbientOcclusionValue.ToString("0.0");

        PostProcessingManager.Instance.SetAmbientOcclusionValue(CurrentConfig.AmbientOcclusionValue);
    }

	public void OnChangeUseAutoExposure() 
	{
		CurrentConfig.UseAutoExposure = autoExpoureToggle.isOn;

        PostProcessingManager.Instance.SetUseAutoExposure(CurrentConfig.UseAutoExposure);
    }

	public void OnChangeUseChromaticAbberation()
	{
		CurrentConfig.UseChromaticAbberation = chromaticAbberationToggle.isOn;
        PostProcessingManager.Instance.SetUseChromaticAbberation(CurrentConfig.UseChromaticAbberation);
    }
	
	public void OnChangeUseColorGrading()
	{
		CurrentConfig.UseColorGrading = colorGradingToggle.isOn;
        PostProcessingManager.Instance.SetUseColorGrading(CurrentConfig.UseColorGrading);
    }

	public void OnChangeUseDepthOfField()
	{
		CurrentConfig.UseDepthOfField = depthOfFieldToggle.isOn;
        PostProcessingManager.Instance.SetUseDepthOfField(CurrentConfig.UseDepthOfField);
    }

	public void OnChangeUseGrain()
	{
		CurrentConfig.UseGrain = grainToggle.isOn;
        PostProcessingManager.Instance.SetUseGrain(CurrentConfig.UseGrain);
    }

	public void OnChangeUseMotionBlur()
	{
		CurrentConfig.UseMotionBlur = motionBlurToggle.isOn;
        PostProcessingManager.Instance.SetUseMotionBlur(CurrentConfig.UseMotionBlur);
    }

	public void OnChangeUseScreenSpaceReflection()
	{
		CurrentConfig.UseScreenSpaceReflections = screenSpaceReflectionToggle.isOn;
        PostProcessingManager.Instance.SetUseScreenSpaceReflections(CurrentConfig.UseScreenSpaceReflections);
    }
	
	public void OnChangeUseVignette()
	{
		CurrentConfig.UseVignette = vignetteToggle.isOn;
        PostProcessingManager.Instance.SetUseVignette(CurrentConfig.UseVignette);
    }

	public void OnChangeFrameLimitValue()
    {
        Application.targetFrameRate = (int)frameLimitSlider.value;

        frameLimitValueText.text = frameLimitSlider.value.ToString();
    }

    void SetQualityDropDown(int value)
    {
        qualityLevelDropDown.value = value;
        qualityLevelDropDown.captionText.text = qualityLevelDropDown.options[value].text;
    }

    public void ButtonClickKeepChange()
    {
        resolutionWarningPanel.SetActive(false);
    }

    public void ButtonClickRevert()
    {
        revertResolutionTimer = Time.unscaledTime - 1f;
    }
}
