using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CockpitController : MonoBehaviour
{
    [SerializeField]
    GameObject rainEffect;

    [SerializeField]
    GameObject displayRoot;

    [SerializeField]
    GameObject heatWarningIndicator;

    [SerializeField]
    GameObject missileWarningIndicator;

    [SerializeField]
    GameObject overrideWarningIndicator;

    [SerializeField]
    Material reactorStatusIndicatorMaterial;

    [SerializeField]
    Material heatGaugeMaterial;

    [SerializeField]
    Material coolantGaugeMaterial;

    [SerializeField]
    Material throttlePositiveGaugeMaterial;

    [SerializeField]
    Material throttleNegativeGaugeMaterial;

    [SerializeField]
    Material jumpJetGaugeMaterial;

    [SerializeField]
    Material torsoTwistBarLeft;

    [SerializeField]
    Material torsoTwistBarRight;

    [SerializeField]
    MechDamageDisplay mechDamageDisplay;

    [SerializeField]
    EnemyTargetInformationHUD enemyTargetInformationHUD;

    [SerializeField]
    MiniMapController miniMapController;

    [SerializeField]
    RadarController radarController;

    [SerializeField]
    Text heatText;

    [SerializeField]
    Text speedText;

    [SerializeField]
    float miniMapScale = 0.5f;

    public GameObject RainEffect { get => rainEffect; }

    public GameObject DisplayRoot { get => displayRoot; }

    public GameObject HeatWarningIndicator { get { return heatWarningIndicator; } }

    public GameObject MissileWarningIndicator { get { return missileWarningIndicator; } }

    public GameObject OverrideWarningIndicator { get => overrideWarningIndicator; }

    public MechDamageDisplay MechDamageDisplay { get { return mechDamageDisplay; } }

    public EnemyTargetInformationHUD EnemyTargetInformationHUD { get { return enemyTargetInformationHUD; } }

    public RadarController RadarController { get { return radarController; } }

    void Awake()
    {
        heatWarningIndicator.SetActive(false);
        missileWarningIndicator.SetActive(false);
        overrideWarningIndicator.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetReactorStatus(UnitPowerState unitPowerState)
    {
        switch (unitPowerState)
        {
            case UnitPowerState.Normal:
                {
                    reactorStatusIndicatorMaterial.SetColor("_Color", Color.green);
                    reactorStatusIndicatorMaterial.SetFloat("_FlashScale", 0.0f);
                    break;
                }
            case UnitPowerState.Shutdown:
                {
                    reactorStatusIndicatorMaterial.SetColor("_Color", Color.red);
                    reactorStatusIndicatorMaterial.SetFloat("_FlashScale", 10.0f);
                    break;
                }
            default:
                {
                    reactorStatusIndicatorMaterial.SetColor("_Color", Color.yellow);
                    reactorStatusIndicatorMaterial.SetFloat("_FlashScale", 5.0f);
                    break;
                }
        }
    }

    public void SetHeatGauge(float value)
    {
        heatGaugeMaterial.SetFloat("_BarValue", value);
    }

    public void SetCoolantGauge(float value)
    {
        coolantGaugeMaterial.SetFloat("_BarValue", value);
    }

    public void SetHeatText(float value)
    {
        heatText.text = value.ToString("0.") + "K";
    }

    public void SetThrottleGauge(float value)
    {
        if (value > 0)
        {
            throttlePositiveGaugeMaterial.SetFloat("_BarValue", value);
            throttleNegativeGaugeMaterial.SetFloat("_BarValue", 0.0f);
        }
        else
        {
            throttlePositiveGaugeMaterial.SetFloat("_BarValue", 0.0f);
            throttleNegativeGaugeMaterial.SetFloat("_BarValue", Mathf.Abs(value));
        }
    }

    public void SetSpeedText(float value)
    {
        speedText.text = value.ToString("0.0") + "\n" + "KPH";
    }

    public void SetJumpJetGauge(float value)
    {
        jumpJetGaugeMaterial.SetFloat("_BarValue", value);
    }

    public void SetTorsoTwistBar(float value)
    {
        if (value > 0)
        {
            torsoTwistBarLeft.SetFloat("_BarValue", 0);
            torsoTwistBarRight.SetFloat("_BarValue", value);
        }
        else
        {
            torsoTwistBarLeft.SetFloat("_BarValue", Mathf.Abs(value));
            torsoTwistBarRight.SetFloat("_BarValue", 0);
        }
    }

    public void UpdateMiniMap(Transform reference)
    {
        //miniMapController.UpdateMinimap(reference, miniMapScale);
    }
}
