using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CurrentConfig
{
    public static int AntiAliasingMode = 1;

    public static bool UseBloom = true;
    public static float BloomValue = 3.0f;

    public static bool UseAmbientOcclusion = true;
    public static float AmbientOcclusionValue = 1.0f;

	public static bool UseAutoExposure = false;

	public static bool UseChromaticAbberation = false;

	public static bool UseColorGrading = false;

	public static bool UseDepthOfField = false;

	public static bool UseGrain = false;

	public static bool UseMotionBlur = false;

	public static bool UseScreenSpaceReflections = false;

	public static bool UseVignette = false;
}
