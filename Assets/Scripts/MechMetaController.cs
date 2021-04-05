using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(CharacterController))]
public class MechMetaController : MonoBehaviour
{
    [Space(5f)]

    [Header("Objects")]

    [SerializeField]
    GameObject[] armLeft = new GameObject[1];

    [SerializeField]
    GameObject[] armRight = new GameObject[1];

    [SerializeField]
    GameObject armDestroyedLeft;

    [SerializeField]
    GameObject armDestroyedRight;

    [SerializeField]
    GameObject jointDestroyedLeft;

    [SerializeField]
    GameObject jointDestroyedRight;

    [SerializeField]
    ParticleSystem[] jumpJetThrusters;

    [Space(5f)]

    [Header("Renders")]

    [SerializeField]
    Renderer[] renderers;

    [Space(5f)]

    [Header("Audio")]

    [SerializeField]
    AudioSource audioSourceFeet;

    [SerializeField]
    AudioSource audioSourceEngine;

    [SerializeField]
    AudioSource audioSourceTorso;

    [SerializeField]
    AudioSource audioSourcePower;

    [SerializeField]
    AudioSource audioSourceSystems;

    [SerializeField]
    AudioSource audioSourceLockOnSystem;

    [SerializeField]
    AudioSource audioSourceJumpJet;

    [SerializeField]
    AudioSource audioSourceHeatWarning;

    [SerializeField]
    AudioSource audioSourceCoolant;

    [SerializeField]
    AudioSource audioSourceRadio;

    [SerializeField]
    AudioSource audioSourceMissileWarning;

    [Space(5f)]

    [Header("Joints")]

    [SerializeField]
    Transform torsoTransform;

    [SerializeField]
    Transform shoulderLeftTransform;

    [SerializeField]
    Transform shoulderRightTransform;

    [SerializeField]
    Transform armLeftTransform;

    [SerializeField]
    Transform armRightTransform;

    [Space(5f)]

    [Header("Hardpoints")]

    [SerializeField]
    Transform cockpitHardpoint;

    [SerializeField]
    Transform targetableHardPoint;

    [SerializeField]
    Transform[] headHardpoints;

    [SerializeField]
    Transform[] torsoCenterHardpoints;

    [SerializeField]
    Transform[] torsoLeftHardpoints;

    [SerializeField]
    Transform[] torsoRightHardpoints;

    [SerializeField]
    Transform[] armLeftHardpoints;

    [SerializeField]
    Transform[] armRightHardpoints;

    [SerializeField]
    Transform torsoLeftExplosionHardpoint;

    [SerializeField]
    Transform torsoRightExplosionHardpoint;

    [SerializeField]
    Transform legLeftExplosionHardpoint;

    [SerializeField]
    Transform legRightExplosionHardpoint;

    [Space(5f)]

    [Header("Colliders")]

    [SerializeField]
    Collider[] headColliders;

    [SerializeField]
    Collider[] torsoCenterColliders;

    [SerializeField]
    Collider[] torsoLeftColliders;

    [SerializeField]
    Collider[] torsoRightColliders;

    [SerializeField]
    Collider[] legLeftColliders;

    [SerializeField]
    Collider[] legRightColliders;

    [SerializeField]
    Collider[] armLeftColliders;

    [SerializeField]
    Collider[] armRightColliders;

    [SerializeField]
    Collider[] armDestroyedLeftColliders;

    [SerializeField]
    Collider[] armDestroyedRightColliders;

    [Space(5f)]

    [Header("Rigidbodies")]

    [SerializeField]
    Rigidbody[] rigidbodies;

    [Header("Misc")]

    [SerializeField]
    ParticleSystem[] coolantParticleSystems;

    public AudioSource AudioSourceFeet
    {
        get
        {
            if (audioSourceFeet == null)
            {
                audioSourceFeet = gameObject.AddComponent<AudioSource>();
                audioSourceFeet.volume = 2.0f;
                audioSourceFeet.spatialBlend = 1.0f;
                audioSourceFeet.maxDistance = 100.0f;
                audioSourceFeet.outputAudioMixerGroup = AudioManager.Instance.audioMixerGroupMechs;
            }

            return audioSourceFeet;
        }
    }
    public AudioSource AudioSourceEngine
    {
        get
        {
            if (audioSourceEngine == null)
            {
                audioSourceEngine = cockpitHardpoint.gameObject.AddComponent<AudioSource>();
                audioSourceEngine.loop = true;
                audioSourceEngine.spatialBlend = 1.0f;
                audioSourceEngine.maxDistance = 100.0f;
                audioSourceEngine.outputAudioMixerGroup = AudioManager.Instance.audioMixerGroupMechs;
            }

            return audioSourceEngine;
        }
    }

    public AudioSource AudioSourceTorso
    {
        get
        {
            if (audioSourceTorso == null)
            {
                audioSourceTorso = torsoTransform.gameObject.AddComponent<AudioSource>();
                audioSourceTorso.loop = true;
                audioSourceTorso.spatialBlend = 1.0f;
                audioSourceTorso.maxDistance = 100.0f;
                audioSourceTorso.outputAudioMixerGroup = AudioManager.Instance.audioMixerGroupMechs;
            }

            return audioSourceTorso;
        }
    }

    public AudioSource AudioSourcePower
    {
        get
        {
            if (audioSourcePower == null)
            {
                audioSourcePower = torsoTransform.gameObject.AddComponent<AudioSource>();
                audioSourceTorso.spatialBlend = 1.0f;
                audioSourceTorso.maxDistance = 100.0f;
            }

            return audioSourcePower;
        }
    }

    public AudioSource AudioSourceSystems
    {
        get
        {
            if (audioSourceSystems == null)
            {
                audioSourceSystems = gameObject.AddComponent<AudioSource>();
            }

            return audioSourceSystems;
        }
    }

    public AudioSource AudioSourceHeatWarning
    {
        get
        {
            if (audioSourceHeatWarning == null)
            {
                audioSourceHeatWarning = gameObject.AddComponent<AudioSource>();
                audioSourceHeatWarning.loop = true;
            }

            return audioSourceHeatWarning;
        }
    }

    public AudioSource AudioSourceLockOnSystem
    {
        get
        {
            if (audioSourceLockOnSystem == null)
            {
                audioSourceLockOnSystem = gameObject.AddComponent<AudioSource>();
                audioSourceLockOnSystem.loop = true;
            }

            return audioSourceLockOnSystem;
        }
    }

    public AudioSource AudioSourceJumpJet
    {
        get
        {
            if (audioSourceJumpJet == null)
            {
                audioSourceJumpJet = torsoTransform.gameObject.AddComponent<AudioSource>();
                audioSourceJumpJet.spatialBlend = 1.0f;
                audioSourceJumpJet.maxDistance = 100.0f;
                audioSourceJumpJet.clip = ResourceManager.Instance.GetAudioClip("JumpJetThrust");
                audioSourceJumpJet.loop = true;
                audioSourceJumpJet.Stop();
                audioSourceJumpJet.outputAudioMixerGroup = AudioManager.Instance.audioMixerGroupMechs;
            }

            return audioSourceJumpJet;
        }
    }

    public AudioSource AudioSourceCoolant
    {
        get
        {
            if (audioSourceCoolant == null)
            {
                audioSourceCoolant = torsoTransform.gameObject.AddComponent<AudioSource>();
                audioSourceCoolant.spatialBlend = 1.0f;
                audioSourceCoolant.maxDistance = 100.0f;
                audioSourceCoolant.clip = ResourceManager.Instance.GetAudioClip("CoolantRelease");
                audioSourceCoolant.loop = true;
                audioSourceCoolant.Stop();
                audioSourceCoolant.outputAudioMixerGroup = AudioManager.Instance.audioMixerGroupMechs;
            }

            return audioSourceCoolant;
        }
    }

    public AudioSource AudioSourceRadio
    {
        get
        {
            if (audioSourceRadio == null)
            {
                audioSourceRadio = gameObject.AddComponent<AudioSource>();
                audioSourceRadio.outputAudioMixerGroup = AudioManager.Instance.audioMixerGroupVoices;

            }

            return audioSourceRadio;
        }
    }

    public AudioSource AudioSourceMissileWarning
    {
        get
        {
            if (audioSourceMissileWarning == null)
            {
                audioSourceMissileWarning = gameObject.AddComponent<AudioSource>();
                audioSourceMissileWarning.clip = ResourceManager.Instance.GetAudioClip("MissileWarning");
                audioSourceMissileWarning.loop = true;
                audioSourceMissileWarning.Stop();
                audioSourceMissileWarning.outputAudioMixerGroup = AudioManager.Instance.audioMixerGroupMechs;
            }

            return audioSourceMissileWarning;
        }
    }

    public GameObject[] ArmLeft { get => armLeft; }

    public GameObject[] ArmRight { get => armRight; }

    public GameObject ArmDestroyedLeft { get => armDestroyedLeft; }

    public GameObject ArmDestroyedRight { get => armDestroyedRight; }

    public GameObject JointDestroyedLeft { get => jointDestroyedLeft; }

    public GameObject JointDestroyedRight { get => jointDestroyedRight; }

    public ParticleSystem[] JumpJetThrusters { get => jumpJetThrusters; }

    public Renderer[] Renderers { get => renderers; }

    public Transform TorsoTransform { get => torsoTransform; }

    public Transform ShoulderLeftTransform { get => shoulderLeftTransform; }

    public Transform ShoulderRightTransform { get => shoulderRightTransform; }

    public Transform ArmLeftTransform { get => armLeftTransform; }

    public Transform ArmRightTransform { get => armRightTransform; }

    public Transform CockpitHardpoint { get => cockpitHardpoint; }

    public Transform TargetableHardPoint { get => targetableHardPoint; }

    public Transform[] HeadHardpoints { get => headHardpoints; }

    public Transform[] TorsoCenterHardpoints { get => torsoCenterHardpoints; }


    public Transform[] TorsoLeftHardpoints { get => torsoLeftHardpoints; }


    public Transform[] TorsoRightHardpoints { get => torsoRightHardpoints; }

    public Transform[] ArmLeftHardpoints { get => armLeftHardpoints; }

    public Transform[] ArmRightHardpoints { get => armRightHardpoints; }

    public Transform TorsoLeftExplosionHardpoint { get => torsoLeftExplosionHardpoint; }

    public Transform TorsoRightExplosionHardpoint { get => torsoRightExplosionHardpoint; }

    public Transform LegLeftExplosionHardpoint { get => legLeftExplosionHardpoint; }

    public Transform LegRightExplosionHardpoint { get => legRightExplosionHardpoint; }

    public Rigidbody[] Rigidbodies { get => rigidbodies; }

    void Awake()
    {
        if (armDestroyedLeft != null)
            armDestroyedLeft.SetActive(false);

        if (ArmDestroyedRight != null)
        ArmDestroyedRight.SetActive(false);

        if (jointDestroyedLeft != null)
            jointDestroyedLeft.SetActive(false);

        if (jointDestroyedRight != null)
            jointDestroyedRight.SetActive(false);

        foreach (ParticleSystem jumpJetThruster in jumpJetThrusters)
        {
            if (jumpJetThruster != null)
                jumpJetThruster.gameObject.SetActive(false);
        }
    }

    public bool IsHeadCollider(Collider targetCollider)
    {
        return headColliders.Contains(targetCollider);
    }

    public bool IsTorsoCenterCollider(Collider targetCollider)
    {
        return torsoCenterColliders.Contains(targetCollider);
    }

    public bool IsTorsoLeftCollider(Collider targetCollider)
    {
        return torsoLeftColliders.Contains(targetCollider);
    }

    public bool IsTorsoRightCollider(Collider targetCollider)
    {
        return torsoRightColliders.Contains(targetCollider);
    }

    public bool IsArmLeftCollider(Collider targetCollider)
    {
        return armLeftColliders.Contains(targetCollider);
    }

    public bool IsArmRightCollider(Collider targetCollider)
    {
        return armRightColliders.Contains(targetCollider);
    }

    public bool IsLegLeftCollider(Collider targetCollider)
    {
        return legLeftColliders.Contains(targetCollider);
    }

    public bool IsLegRightCollider(Collider targetCollider)
    {
        return legRightColliders.Contains(targetCollider);
    }

    public void DisableArmLeftColliders()
    {
        for (int i = 0; i < armDestroyedLeftColliders.Length; i++)
        {
            armDestroyedLeftColliders[i].enabled = false;
        }
    }

    public void DisableArmRightColliders()
    {
        for (int i = 0; i < armDestroyedRightColliders.Length; i++)
        {
            armDestroyedRightColliders[i].enabled = false;
        }
    }

    public Material ApplyMechPaintScheme(MechPaintScheme mechPaintScheme)
    {
        if (renderers.Length > 0)
        {
            Material mechMaterial = new Material(renderers[0].material);

            mechPaintScheme.ApplyPropertiesToMaterial(mechMaterial);

            foreach (Renderer renderer in renderers)
            {
                renderer.material = mechMaterial;
            }

            return mechMaterial;
        }

        return null;
    }

    public void DisableAudioSources()
    {
        if (audioSourceFeet != null)
            audioSourceFeet.enabled = false;

        if (audioSourceEngine != null)
            audioSourceEngine.enabled = false;

        if (audioSourceTorso != null)
            audioSourceTorso.enabled = false;

        if (audioSourceSystems != null)
            audioSourceSystems.enabled = false;

        if (audioSourceLockOnSystem != null)
            audioSourceLockOnSystem.enabled = false;

        if (audioSourceJumpJet != null)
            audioSourceJumpJet.enabled = false;

        if (audioSourceHeatWarning != null)
            audioSourceHeatWarning.enabled = false;

        if (audioSourceCoolant != null)
            audioSourceCoolant.enabled = false;

        if (audioSourceRadio != null)
            audioSourceRadio.enabled = false;

        if (audioSourceMissileWarning != null)
            audioSourceMissileWarning.enabled = false;
    }

    public void SetCollidersLayer(int layer)
    {
        StaticHelper.SetCollidersLayer(layer, headColliders);
        StaticHelper.SetCollidersLayer(layer, torsoCenterColliders);
        StaticHelper.SetCollidersLayer(layer, torsoLeftColliders);
        StaticHelper.SetCollidersLayer(layer, torsoRightColliders);
        StaticHelper.SetCollidersLayer(layer, armLeftColliders);
        StaticHelper.SetCollidersLayer(layer, armRightColliders);
        StaticHelper.SetCollidersLayer(layer, legLeftColliders);
        StaticHelper.SetCollidersLayer(layer, legRightColliders);
    }

    public void SetJumpJetThrustersState(bool state)
    {
        StaticHelper.SetParticleSystemsEmission(jumpJetThrusters, state);
    }

    public void CreateCoolantParticleSystems()
    {
        GameObject coolentEffectPrefab = ResourceManager.Instance.GetEffectPrefab("MechCoolantEffect");

        if (coolentEffectPrefab != null)
        {
            GameObject coolentEffectObject = Instantiate(coolentEffectPrefab);
            coolentEffectObject.transform.position = torsoTransform.position;
            coolentEffectObject.transform.parent = torsoTransform;

            coolantParticleSystems = coolentEffectObject.GetComponentsInChildren<ParticleSystem>();

            foreach (ParticleSystem particleSystem in coolantParticleSystems)
            {
                ParticleSystem.EmissionModule emissionModule = particleSystem.emission;
                emissionModule.enabled = false;
            }
        }
        else
        {
            coolantParticleSystems = new ParticleSystem[0];
        }
    }

    public void SetCoolantEffect(bool state)
    {
        StaticHelper.SetParticleSystemsEmission(coolantParticleSystems, state);
    }

#if UNITY_EDITOR
    [Header("Editor Gizmos")]

    [SerializeField]
    bool drawColliders = false;

    [SerializeField]
    Color collidersHeadColor = new Color(1.0f, 1.0f, 0.0f, 0.75f);

    [SerializeField]
    Color collidersTorsoCenterColor = new Color(0.0f, 1.0f, 0.0f, 0.75f);

    [SerializeField]
    Color collidersTorsoLeftColor = new Color(1.0f, 0.0f, 0.0f, 0.75f);

    [SerializeField]
    Color collidersTorsoRightColor = new Color(0.0f, 0.0f, 1.0f, 0.75f);

    [SerializeField]
    Color collidersArmLeftColor = new Color(0.0f, 0.5f, 1.0f, 0.75f);

    [SerializeField]
    Color collidersArmRightColor = new Color(1.0f, 0.5f, 0.0f, 0.75f);

    [SerializeField]
    Color collidersLegLeftColor = new Color(1.0f, 0.0f, 1.0f, 0.75f);

    [SerializeField]
    Color collidersLegRightColor = new Color(1.0f, 0.0f, 0.5f, 0.75f);

    [SerializeField]
    bool drawWeaponHardpoints = false;

    [SerializeField]
    Color weaponHardpointsColor = new Color(1.0f, 0.0f, 0.0f, 0.75f);

    public void AutoFillRigidbodies()
    {
        rigidbodies = transform.GetComponentsInChildren<Rigidbody>();

        foreach (Rigidbody rigidbody in rigidbodies)
        {
            rigidbody.isKinematic = true;
        }

        EditorUtility.SetDirty(this);
        EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
    }

    public void AddFootAudioSource()
    {
        if (audioSourceFeet == null)
        {
            audioSourceFeet = gameObject.AddComponent<AudioSource>();
            audioSourceFeet.spatialBlend = 1.0f;
            audioSourceFeet.maxDistance = 100.0f;
        }

        EditorUtility.SetDirty(this);
        EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
    }

    public void ActivateRigidbodies()
    {
        rigidbodies = transform.GetComponentsInChildren<Rigidbody>();

        foreach (Rigidbody rigidbody in rigidbodies)
        {
            rigidbody.isKinematic = false;

            rigidbody.velocity += Random.onUnitSphere;
        }
    }

    void OnDrawGizmos()
    {
        if (drawColliders)
        {
            DrawColliders(headColliders, collidersHeadColor);
            DrawColliders(torsoCenterColliders, collidersTorsoCenterColor);
            DrawColliders(torsoLeftColliders, collidersTorsoLeftColor);
            DrawColliders(torsoRightColliders, collidersTorsoRightColor);
            DrawColliders(armLeftColliders, collidersArmLeftColor);
            DrawColliders(armRightColliders, collidersArmRightColor);
            DrawColliders(legLeftColliders, collidersLegLeftColor);
            DrawColliders(legRightColliders, collidersLegRightColor);
        }

        if (drawWeaponHardpoints)
        {
            Color previousColor = Gizmos.color;
            Gizmos.color = weaponHardpointsColor;

            DrawWeaponHardpoints(headHardpoints);
            DrawWeaponHardpoints(torsoCenterHardpoints);
            DrawWeaponHardpoints(torsoLeftHardpoints);
            DrawWeaponHardpoints(torsoRightHardpoints);
            DrawWeaponHardpoints(armLeftHardpoints);
            DrawWeaponHardpoints(armRightHardpoints);

            Gizmos.color = previousColor;
        }
    }

    void DrawColliders(Collider[] colliders, Color color)
    {
        Color previousColor = Gizmos.color;
        Gizmos.color = color;

        foreach (Collider collider in colliders)
        {
            if (collider is SphereCollider)
            {
                SphereCollider sphereCollider = collider as SphereCollider;

                Gizmos.DrawSphere(sphereCollider.bounds.center, sphereCollider.bounds.extents.x);
            }
            else
            {
                Gizmos.DrawCube(collider.bounds.center, collider.bounds.size);
            }
        }

        Gizmos.color = previousColor;
    }

    void DrawWeaponHardpoints(Transform[] weaponHardpoints)
    {
        foreach (Transform hardpoint in weaponHardpoints)
        {
            Gizmos.DrawLine(hardpoint.position, hardpoint.position + hardpoint.forward);
        }
    }
#endif
}