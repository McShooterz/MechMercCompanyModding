using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CachingScreen : MonoBehaviour
{
    [SerializeField]
    Transform cameraTransform;

    [SerializeField]
    AudioSource audioSource;

    [SerializeField]
    Text cachingGroupText;

    [SerializeField]
    Text cachingElementText;

    [SerializeField]
    GameObject[] beamPrefabs;

    [SerializeField]
    GameObject[] projectilePrefabs;

    [SerializeField]
    GameObject[] trailPrefabs;

    [SerializeField]
    GameObject[] effectPrefabs;

    [SerializeField]
    AudioClip[] audioClips;

    [SerializeField]
    int beamIndex = 0;

    [SerializeField]
    int projectileIndex = 0;

    [SerializeField]
    int tailIndex = 0;

    [SerializeField]
    int effectIndex = 0;

    [SerializeField]
    int audioClipIndex = 0;

    private void Awake()
    {
        Time.timeScale = 0.0f;
    }

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;

        beamPrefabs = ResourceManager.Instance.BeamPrefabs;
        projectilePrefabs = ResourceManager.Instance.ProjectilePrefabs;
        trailPrefabs = ResourceManager.Instance.TrailPrefabs;
        effectPrefabs = ResourceManager.Instance.EffectPrefabs;
        audioClips = ResourceManager.Instance.AudioClips;

        audioSource.volume = 0.001f;
    }

    // Update is called once per frame
    void Update()
    {
        if (beamPrefabs.Length > 0 && beamIndex < beamPrefabs.Length)
        {
            CachGameObject(beamPrefabs[beamIndex]);

            cachingGroupText.text = "Beam Prefabs - " + ((float)beamIndex / beamPrefabs.Length * 100f).ToString("0.") + "%";

            beamIndex++;
        }
        else if (projectilePrefabs.Length > 0 && projectileIndex < projectilePrefabs.Length)
        {
            CachGameObject(projectilePrefabs[projectileIndex]);

            cachingGroupText.text = "Projectile Prefabs - " + ((float)projectileIndex / projectilePrefabs.Length * 100f).ToString("0.") + "%";

            projectileIndex++;
        }
        else if (trailPrefabs.Length > 0 && tailIndex < trailPrefabs.Length)
        {
            CachGameObject(trailPrefabs[tailIndex]);

            cachingGroupText.text = "Trail Prefabs - " + ((float)tailIndex / trailPrefabs.Length * 100f).ToString("0.") + "%";

            tailIndex++;
        }
        else if (effectPrefabs.Length > 0 && effectIndex < effectPrefabs.Length)
        {
            CachGameObject(effectPrefabs[effectIndex]);

            cachingGroupText.text = "Effect Prefabs - " + ((float)effectIndex / effectPrefabs.Length * 100f).ToString("0.") + "%";

            effectIndex++;
        }
        else if (audioClips.Length > 0 && audioClipIndex < audioClips.Length)
        {
            audioSource.PlayOneShot(audioClips[audioClipIndex]);

            cachingElementText.text = audioClips[audioClipIndex].name;

            cachingGroupText.text = "Audio Clips - " + ((float)audioClipIndex / audioClips.Length * 100f).ToString("0.") + "%";

            audioClipIndex++;
        }
        else
        {
            audioSource.enabled = false;
            LoadingScreen.Instance.LoadScene("MainMenu");
        }
    }

    void CachGameObject(GameObject target)
    {
        Instantiate(target, cameraTransform.position + cameraTransform.forward, Quaternion.identity);

        cachingElementText.text = target.name;
    }
}
