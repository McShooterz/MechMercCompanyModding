using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class CameraController : MonoBehaviour
{
    public static CameraController Instance { get; private set; }

    [SerializeField]
    Camera mainCamera;

    [SerializeField]
    Camera cockpitCamera;

    [SerializeField]
    Camera zoomCamera;

    [SerializeField]
    PostProcessLayer[] postProcessLayers;

    [SerializeField]
    Light nightVisionLight;

    Coroutine nightVisionBrightenCoroutine;

    Vector3 targetShakeRotation;

    Vector3 currentShakeRotation;

    public Camera MainCamera { get => mainCamera; }

    public Camera CockpitCamera { get => cockpitCamera; }

    public Camera ZoomCamera { get => zoomCamera; }

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
        }
    }

    // Update is called once per frame
    void Update()
    {
        currentShakeRotation = Vector3.Lerp(currentShakeRotation, targetShakeRotation, 10.0f * Time.deltaTime);
        targetShakeRotation = Vector3.Lerp(targetShakeRotation, Vector3.zero, 5.0f * Time.deltaTime);

        cockpitCamera.transform.localRotation = Quaternion.Euler(currentShakeRotation);
        mainCamera.transform.localRotation = Quaternion.Euler(currentShakeRotation);
    }

    void OnEnable()
    {
        SetAntiAliasingMode(CurrentConfig.AntiAliasingMode);

        //CameraPlay.CurrentCamera = mainCamera;
    }

    void OnDisable()
    {
        //CameraPlay.Colored_Switch = false;
    }

    public void ToggleNightVision(MechMetaController mechMetaController)
    {
        /*if (!CameraPlay.Colored_Switch)
        {
            CameraPlay.Colored_ON(Color.green);
            nightVisionLight.enabled = true;
            nightVisionLight.intensity = 0.0f;

            nightVisionBrightenCoroutine = StartCoroutine(BrightenNightVisionLight());

            AudioManager.Instance.PlayClip(mechMetaController.AudioSourceSystems, ResourceManager.Instance.GetAudioClip("ToggleLightAmplification"), false, false);
        }
        else
        {
            CameraPlay.Colored_OFF(Color.green, 0.5f);
            nightVisionLight.enabled = false;

            if (nightVisionBrightenCoroutine != null)
            {
                StopCoroutine(nightVisionBrightenCoroutine);
            }

            AudioManager.Instance.PlayClip(mechMetaController.AudioSourceSystems, ResourceManager.Instance.GetAudioClip("ToggleLightAmplification"), false, false);
        }*/
    }

    public void SetAntiAliasingMode(int mode)
    {
        for (int i = 0; i < postProcessLayers.Length; i++)
        {
            postProcessLayers[i].antialiasingMode = (PostProcessLayer.Antialiasing)mode;
        }
    }

    IEnumerator BrightenNightVisionLight()
    {
        while (nightVisionLight.intensity < 2.5f)
        {
            nightVisionLight.intensity += 1.5f * Time.deltaTime;

            yield return null;
        }

        nightVisionLight.intensity = 2.5f;
    }

    public void AddShakeRotation(Vector3 rotationChange)
    {
        targetShakeRotation += rotationChange;
    }
}