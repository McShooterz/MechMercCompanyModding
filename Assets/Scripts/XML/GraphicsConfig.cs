using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphicsConfig
{
    public int QualityLevel;
    public int ResolutionX;
    public int ResolutionY;
    public int RefreshRate = 60;
    public bool FullScreen;
    public int TextureQuality;
    public AnisotropicFiltering AnisotropicFiltering;
    public bool RealTimeReflectionProbes;
    public float DpiFactor;
    public float LodDistance;
    public int MaxLod;
    public int VSyncCount;
    public SkinWeights AnimationSkinWeights;
    public bool BillboardFacingQuality;
    public bool SoftParticles;
    public bool SoftVegetation;
    public ShadowQuality ShadowQuality;
    public float ShadowDistance;
    public ShadowResolution ShadowResolution;
    public ShadowProjection ShadowProjection;
    public ShadowmaskMode ShadowmaskMode;
    public int ShadowCascades;
    public float ShadowNearPlaneOffset;
    public int PixelLightCount;
    public int FrameLimit = 120;

    // Post Processing
    public int AntiAliasingMode = 1;

    public bool UseBloom = true;
    public float BloomValue = 3.0f;

    public bool UseAmbientOcclusion = true;
    public float AmbientOcclusionValue = 1.0f;
	
	public bool UseAutoExposure = false;

	public bool UseChromaticAbberation = false;

	public bool UseColorGrading = false;

	public bool UseDepthOfField = false;

	public bool UseGrain = false;

	public bool UseMotionBlur = false;

	public bool UseScreenSpaceReflections = false;

	public bool UseVignette = false;
}
