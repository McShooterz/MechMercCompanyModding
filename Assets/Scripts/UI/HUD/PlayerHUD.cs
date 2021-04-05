using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using SickscoreGames.HUDNavigationSystem;

public class PlayerHUD : MonoBehaviour
{
    #region Variables
    public static PlayerHUD Instance { get; private set; }

    [SerializeField]
    GameObject pauseMenu;

    [Header("Components")]

    [SerializeField]
    GameObject elementsContainer;

    [SerializeField]
    DeathMessageUI deathMessageUI;

    [SerializeField]
    Image targetIndicator;

    [SerializeField]
    Image targetOffScreenIndicator;

    [SerializeField]
    Image navPointIndicator;

    [SerializeField]
    Text navPointText;

    [SerializeField]
    Image lockedOnIndicator;

    [SerializeField]
    CompassController compassController;

    [SerializeField]
    MiniMapController miniMapController;

    [SerializeField]
    RadarController radarController;

    [SerializeField]
    Image[] reticleParts;

    [SerializeField]
    Image hitIndicator;

    [SerializeField]
    Text rangeText;

    [SerializeField]
    Text shuttingDownText;

    [SerializeField]
    Text overrideEngagedText;

    [SerializeField]
    RectTransform zoomWindow;

    [SerializeField]
    Slider throttlePositiveBar;

    [SerializeField]
    Slider throttleNegativeBar;

    [SerializeField]
    Text speedText;

    [SerializeField]
    Slider heatBar;

    [SerializeField]
    GameObject heatWarningIndicator;

    [SerializeField]
    Text temperatureText;

    [SerializeField]
    Slider coolantBar;

    [SerializeField]
    Slider jumpJetBar;

    [SerializeField]
    MechDamageDisplay mechDamageDisplay;

    [SerializeField]
    WeaponsManagerUI weaponsManagerUI;

    [SerializeField]
    Slider torsoTwistBar;

    [SerializeField]
    RectTransform torsoTwistBarFill;

    [SerializeField]
    ObjectivesControllerUI objectivesControllerUI;

    [SerializeField]
    GameObject borderWarningPanel;

    [SerializeField]
    Text borderWarningText;

    [SerializeField]
    SquadInfoGroup squadInfoGroup;

    [SerializeField]
    GameObject missileWarningPanel;

    [SerializeField]
    Image weaponGroup1IndicatorImage;

    [SerializeField]
    Image weaponGroup2IndicatorImage;

    [SerializeField]
    Image weaponGroup3IndicatorImage;

    [SerializeField]
    Image weaponGroup4IndicatorImage;

    [SerializeField]
    Image weaponGroup5IndicatorImage;

    [SerializeField]
    Image weaponGroup6IndicatorImage;

    [SerializeField]
    Slider weaponGroup1IndicatorJamming;

    [SerializeField]
    Slider weaponGroup2IndicatorJamming;

    [SerializeField]
    Slider weaponGroup3IndicatorJamming;

    [SerializeField]
    Slider weaponGroup4IndicatorJamming;

    [SerializeField]
    Slider weaponGroup5IndicatorJamming;

    [SerializeField]
    Slider weaponGroup6IndicatorJamming;

    [SerializeField]
    GameObject objectiveUpdatedIndicator;

    [Header("Controls")]

    [SerializeField]
    bool openingHUD = false;

    [SerializeField]
    bool closingHUD = false;

    [SerializeField]
    bool openingZoomWindow = false;

    [SerializeField]
    bool closingZoomWindow = false;

    [Header("Misc")]

    [SerializeField]
    float zoomWindowOffsect;

    [SerializeField]
    float miniMapScale = 0.5f;

    public string navPointName;

    Coroutine ZoomWindowCoroutine;

    Coroutine hudCoroutine;

	private Vector2 zoomWindowSize;

    float objectiveUpdateShowTimer = 0.0f;
    #endregion

    public MiniMapController MiniMapController { get => miniMapController; }

    public RadarController RadarController { get => radarController; }

    public ObjectivesControllerUI ObjectivesControllerUI { get => objectivesControllerUI; }

    public Text OverrideEngagedText { get => overrideEngagedText; }

    public Text ShuttingDownText { get => shuttingDownText; }

    public SquadInfoGroup SquadInfoGroup { get => squadInfoGroup; }

    public RectTransform ZoomWindow { get => zoomWindow; }

    public GameObject HeatWarningIndicator { get => heatWarningIndicator; }

    public WeaponsManagerUI WeaponsManagerUI { get => weaponsManagerUI; }

    public GameObject MissileWarningPanel { get => missileWarningPanel; }

    public Image WeaponGroup1IndicatorImage { get => weaponGroup1IndicatorImage; }

    public Image WeaponGroup2IndicatorImage { get => weaponGroup2IndicatorImage; }

    public Image WeaponGroup3IndicatorImage { get => weaponGroup3IndicatorImage; }

    public Image WeaponGroup4IndicatorImage { get => weaponGroup4IndicatorImage; }

    public Image WeaponGroup5IndicatorImage { get => weaponGroup5IndicatorImage; }

    public Image WeaponGroup6IndicatorImage { get => weaponGroup6IndicatorImage; }

    public Slider WeaponGroup1IndicatorJamming { get => weaponGroup1IndicatorJamming; }

    public Slider WeaponGroup2IndicatorJamming { get => weaponGroup2IndicatorJamming; }

    public Slider WeaponGroup3IndicatorJamming { get => weaponGroup3IndicatorJamming; }

    public Slider WeaponGroup4IndicatorJamming { get => weaponGroup4IndicatorJamming; }

    public Slider WeaponGroup5IndicatorJamming { get => weaponGroup5IndicatorJamming; }

    public Slider WeaponGroup6IndicatorJamming { get => weaponGroup6IndicatorJamming; }

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

        zoomWindow.transform.localScale = new Vector3(0.0f, 0.0f, 1.0f);
        zoomWindow.gameObject.SetActive(false);

        deathMessageUI.gameObject.SetActive(false);
        objectivesControllerUI.gameObject.SetActive(false);
        borderWarningPanel.SetActive(false);
        hitIndicator.gameObject.SetActive(false);
        heatWarningIndicator.gameObject.SetActive(false);

        elementsContainer.transform.localScale = new Vector3(0.0f, 0.0f, 1.0f);

        navPointName = StaticHelper.GetNavPointName(0);

		zoomWindowSize = ZoomWindow.anchorMax - ZoomWindow.anchorMin;

        OpenHUD();
    }

    // Use this for initialization
    void Start ()
    {
        CameraController.Instance.ZoomCamera.gameObject.SetActive(false);
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pauseMenu.SetActive(true);
            gameObject.SetActive(false);
            return;
        }

        if (hitIndicator.gameObject.activeInHierarchy)
        {
            float alpha = hitIndicator.color.a - Time.deltaTime * 1.5f;

            if (alpha < 0f)
            {
                hitIndicator.gameObject.SetActive(false);
            }
            else
            {
                hitIndicator.color = new Color(hitIndicator.color.r, hitIndicator.color.g, hitIndicator.color.b, alpha);
            }
        }

        if (objectiveUpdatedIndicator.activeSelf && Time.time > objectiveUpdateShowTimer)
        {
            objectiveUpdatedIndicator.SetActive(false);
        }
	}

    void OnEnable()
    {
        Time.timeScale = 1.0f;
        AudioListener.pause = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        zoomWindowOffsect = 512f * (1080f / Screen.height);

        if (openingHUD)
        {
            StartCoroutine(OpenHudCoroutine());
        }
        else if (closingHUD)
        {
            StartCoroutine(CloseHudCoroutine());
        }

        if (openingZoomWindow)
        {
            StartCoroutine(OpenZoomWindowCoroutine());
        }
        else if (closingZoomWindow)
        {
            StartCoroutine(CloseZoomWindowCoroutine());
        }

        MechControllerPlayer.Instance?.CommandSystem.UpdateInstructions();
    }

    void OnGUI()
    {
        if (Application.isEditor)
        {

        }
    }

    public void OpenHUD()
    {
        openingHUD = true;
        closingHUD = false;

        StartCoroutine(OpenHudCoroutine());
    }

    public void CloseHUD()
    {
        closingHUD = true;
        openingHUD = false;

        StartCoroutine(CloseHudCoroutine());
    }

    public void OpenZoomWindow()
    {
        openingZoomWindow = true;
        closingZoomWindow = false;

        zoomWindow.gameObject.SetActive(true);
        CameraController.Instance.ZoomCamera.gameObject.SetActive(true);

        StartCoroutine(OpenZoomWindowCoroutine());
    }

    public void CloseZoomWindow()
    {
        closingZoomWindow = true;
        openingZoomWindow = false;

        StartCoroutine(CloseZoomWindowCoroutine());
    }

    public void SetHudVisibility(bool state)
    {
        elementsContainer.SetActive(state);
    }

    //public void SetHudCenterVisibility(bool state)
    //{
        //centerElements.SetActive(state);
    //}

    public void SetDeathMessage(string deathMessageText, string pilotStatusText)
    {
        deathMessageUI.gameObject.SetActive(true);
        deathMessageUI.DeathMessageText.text = deathMessageText;
        deathMessageUI.PilotStatusText.text = pilotStatusText;
    }

    public void SetCompassDirection(Transform reference)
    {
        compassController.UpdateCompassBar(reference);
    }

    public void UpdateMiniMap(Transform reference)
    {
        //miniMapController.UpdateMinimap(reference, miniMapScale);
    }

    public void SetRangeText(string text)
    {
        rangeText.text = text;
    }

    public void SetThrottle(float value)
    {
        value = Mathf.Clamp(value, -1f, 1f);

        if (value > 0f)
        {
            throttleNegativeBar.value = 0f;
            throttlePositiveBar.value = value;
        }
        else if (value < 0f)
        {
            throttleNegativeBar.value = -value;
            throttlePositiveBar.value = 0f;
        }
        else
        {
            throttleNegativeBar.value = 0f;
            throttlePositiveBar.value = 0f;
        }
    }

    public void SetHeat(float value)
    {
        value = Mathf.Clamp(value, 0f, 1f);

        heatBar.value = value;
    }

    public void SetJumpJetValue(float value)
    {
        jumpJetBar.value = value;
    }

    public void SetSpeedText(float value)
    {
        speedText.text = value.ToString("0.#" + "KPH");
    }

    public void SetTempuratureText(float value)
    {
        temperatureText.text = value.ToString("0.") + "K";
    }

    public void SetTargetingReticleColor(Color color)
    {
        foreach (Image reticlePart in reticleParts)
        {
            reticlePart.color = color;
        }
    }

    public void ToggleZoomWindow()
    {
        if (zoomWindow.gameObject.activeInHierarchy)
        {
            CloseZoomWindow();
        }
        else
        {
            OpenZoomWindow();
        }
    }

    public void SetDamageDisplay(UnitController unitController)
    {
        if (unitController is MechController)
        {
            mechDamageDisplay.gameObject.SetActive(true);
            MechController mechController = unitController as MechController;

            mechDamageDisplay.SetDisplays(mechController.MechData);
        }
    }

    public void SetTargetIndicator(UnitController target, Color color)
    {
        if ((object)target != null)
        {
            Vector3 screenPosition = CameraController.Instance.MainCamera.WorldToScreenPoint(target.Bounds.center);

            if (StaticHelper.IsOnScreen(screenPosition))
            {
                targetIndicator.gameObject.SetActive(true);
                targetOffScreenIndicator.gameObject.SetActive(false);

                targetIndicator.color = color;

                Rect targetRect;

                if (zoomWindow.gameObject.activeInHierarchy && RectTransformUtility.RectangleContainsScreenPoint(zoomWindow, new Vector2(screenPosition.x, screenPosition.y)))
                {
                    targetRect = StaticHelper.GetScreenRect(target.Bounds, CameraController.Instance.ZoomCamera);
                    screenPosition = targetRect.center;

                    targetIndicator.rectTransform.sizeDelta = new Vector2(targetRect.width / 512 * zoomWindow.rect.width, targetRect.height / 512 * zoomWindow.rect.height);

                    if (!RectTransformUtility.RectangleContainsScreenPoint(zoomWindow, new Vector2(screenPosition.x, screenPosition.y)))
                    {
                        targetIndicator.gameObject.SetActive(false);
                        return;
                    }
                }
                else
                {
                    targetRect = StaticHelper.GetScreenRect(target.Bounds, CameraController.Instance.MainCamera);
                    targetIndicator.rectTransform.sizeDelta = new Vector2(targetRect.width, targetRect.height);
                }

                targetIndicator.transform.position = new Vector3(screenPosition.x, screenPosition.y, 0f);
            }
            else
            {
                targetIndicator.gameObject.SetActive(false);
                targetOffScreenIndicator.gameObject.SetActive(true);

                targetOffScreenIndicator.color = color;

                if (screenPosition.z < 0f)
                {
                    screenPosition.x = Screen.width - screenPosition.x;
                    screenPosition.y = Screen.height - screenPosition.y;
                }

                // calculate off-screen position/rotation
                Vector3 screenCenter = new Vector3(Screen.width, Screen.height, 0f) / 2f;
                screenPosition -= screenCenter;
                float angle = Mathf.Atan2(screenPosition.y, screenPosition.x);
                angle -= 90f * Mathf.Deg2Rad;
                float cos = Mathf.Cos(angle);
                float sin = -Mathf.Sin(angle);
                float cotangent = cos / sin;
                screenPosition = screenCenter + new Vector3(sin * 50f, cos * 50f, 0f);

                // is indicator inside the defined bounds?
                float offset = Mathf.Min(screenCenter.x, screenCenter.y);
                offset = Mathf.Lerp(0f, offset, 0.075f);
                Vector3 screenBounds = screenCenter - new Vector3(offset, offset, 0f);
                float boundsY = (cos > 0f) ? screenBounds.y : -screenBounds.y;
                screenPosition = new Vector3(boundsY / cotangent, boundsY, 0f);

                // when out of bounds, get point on appropriate side
                if (screenPosition.x > screenBounds.x) // out => right
                    screenPosition = new Vector3(screenBounds.x, screenBounds.x * cotangent, 0f);
                else if (screenPosition.x < -screenBounds.x) // out => left
                    screenPosition = new Vector3(-screenBounds.x, -screenBounds.x * cotangent, 0f);
                screenPosition += screenCenter;

                targetOffScreenIndicator.transform.position = new Vector3(screenPosition.x + transform.localPosition.x, screenPosition.y + transform.localPosition.y, 0f);
                targetOffScreenIndicator.transform.rotation = Quaternion.Euler(0f, 0f, angle * Mathf.Rad2Deg);
            }
        }
        else
        {
            targetIndicator.gameObject.SetActive(false);
            targetOffScreenIndicator.gameObject.SetActive(false);
        }
    }

    public void SetLockedOnIndicator(UnitController target)
    {
        if (target != null)
        {
            Vector3 screenPosition = CameraController.Instance.MainCamera.WorldToScreenPoint(target.Bounds.center);

            if (StaticHelper.IsOnScreen(screenPosition))
            {
                lockedOnIndicator.gameObject.SetActive(true);
                Rect targetRect;

                if (zoomWindow.gameObject.activeInHierarchy && RectTransformUtility.RectangleContainsScreenPoint(zoomWindow, new Vector2(screenPosition.x, screenPosition.y)))
                {
                    targetRect = StaticHelper.GetScreenRect(target.Bounds, CameraController.Instance.ZoomCamera);
                    screenPosition = targetRect.center;

                    if (!RectTransformUtility.RectangleContainsScreenPoint(zoomWindow, new Vector2(screenPosition.x, screenPosition.y)))
                    {
                        lockedOnIndicator.gameObject.SetActive(false);
                        return;
                    }
                }
                  
                lockedOnIndicator.transform.position = new Vector3(screenPosition.x + transform.localPosition.x, screenPosition.y + transform.localPosition.y, 0f);
            }
            else
            {
                lockedOnIndicator.gameObject.SetActive(false);
            }
        }
        else
        {
            lockedOnIndicator.gameObject.SetActive(false);
        }
    }

    public void SetNavPointIndicator(Vector3 navPointPosition, Vector3 origin)
    {
        float distance = (navPointPosition - origin).magnitude * 10f;

        navPointText.text = navPointName + " " + distance.ToString("#.") + "m";

        Vector3 screenPosition = CameraController.Instance.MainCamera.WorldToScreenPoint(navPointPosition);

        if (StaticHelper.IsOnScreen(screenPosition))
        {
            navPointIndicator.transform.position = new Vector3(screenPosition.x + transform.localPosition.x, screenPosition.y + transform.localPosition.y, 0f);
        }
        else
        {
            if (screenPosition.z < 0f)
            {
                screenPosition.x = Screen.width - screenPosition.x;
                screenPosition.y = Screen.height - screenPosition.y;
            }

            // calculate off-screen position/rotation
            Vector3 screenCenter = new Vector3(Screen.width, Screen.height, 0f) / 2f;
            screenPosition -= screenCenter;
            float angle = Mathf.Atan2(screenPosition.y, screenPosition.x);
            angle -= 90f * Mathf.Deg2Rad;
            float cos = Mathf.Cos(angle);
            float sin = -Mathf.Sin(angle);
            float cotangent = cos / sin;
            screenPosition = screenCenter + new Vector3(sin * 50f, cos * 50f, 0f);

            // is indicator inside the defined bounds?
            float offset = Mathf.Min(screenCenter.x, screenCenter.y);
            offset = Mathf.Lerp(0f, offset, 0.075f);
            Vector3 screenBounds = screenCenter - new Vector3(offset, offset, 0f);
            float boundsY = (cos > 0f) ? screenBounds.y : -screenBounds.y;
            screenPosition = new Vector3(boundsY / cotangent, boundsY, 0f);

            // when out of bounds, get point on appropriate side
            if (screenPosition.x > screenBounds.x) // out => right
                screenPosition = new Vector3(screenBounds.x, screenBounds.x * cotangent, 0f);
            else if (screenPosition.x < -screenBounds.x) // out => left
                screenPosition = new Vector3(-screenBounds.x, -screenBounds.x * cotangent, 0f);
            screenPosition += screenCenter;

            navPointIndicator.transform.position = new Vector3(screenPosition.x + transform.localPosition.x, screenPosition.y + transform.localPosition.y, 0f);
            //navPointIndicator.transform.rotation = Quaternion.Euler(0f, 0f, angle * Mathf.Rad2Deg);
        }
    }

    public void SetTorsoTwistBar(float value)
    {
        torsoTwistBar.value = Mathf.Abs(value);

        if (value < 0)
        {
            torsoTwistBarFill.rotation = Quaternion.Euler(new Vector3(0f, 0f, 180f));
        }
        else
        {
            torsoTwistBarFill.rotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));
        }
    }

    public void SetCoolantBar(float value)
    {
        coolantBar.value = value;
    }

    public void ToggleObjectivesWindow()
    {
        objectivesControllerUI.gameObject.SetActive(!objectivesControllerUI.gameObject.activeSelf);

        if (objectivesControllerUI.gameObject.activeSelf)
        {
            objectiveUpdatedIndicator.SetActive(false);
        }
    }

    public void SetObjectivesWindowVisibility(bool state)
    {
        objectivesControllerUI.gameObject.SetActive(state);

        if (state)
        {
            objectiveUpdatedIndicator.SetActive(false);
        }
    }

    IEnumerator OpenHudCoroutine()
    {
        while (openingHUD)
        {
            if (enabled)
            {
                float scale = elementsContainer.transform.localScale.x;

                if (scale > 0.99f)
                {
                    scale = 1.0f;
                    openingHUD = false;
                }
                else
                {
                    scale = Mathf.Lerp(scale, 1.0f, 15.0f * Time.deltaTime);
                }

                elementsContainer.transform.localScale = new Vector3(scale, scale, 1.0f);
            }

            yield return null;
        }
    }

    IEnumerator CloseHudCoroutine()
    {
        while (closingHUD)
        {
            if (enabled)
            {
                float scale = elementsContainer.transform.localScale.x;

                if (scale < 0.01f)
                {
                    scale = 0.0f;
                    closingHUD = false;
                }
                else
                {
                    scale = Mathf.Lerp(scale, 0.0f, 15.0f * Time.deltaTime);
                }

                elementsContainer.transform.localScale = new Vector3(scale, scale, 1.0f);
            }

            yield return null;
        }
    }

    IEnumerator OpenZoomWindowCoroutine()
    {
        while (openingZoomWindow)
        {
            if (enabled)
            {
                float scale = zoomWindow.transform.localScale.x;

                if (scale > 0.99f)
                {
                    scale = 1.0f;
                    openingZoomWindow = false;
                }
                else
                {
                    scale = Mathf.Lerp(scale, 1.0f, 15.0f * Time.deltaTime);
                }

				Vector2 size = zoomWindowSize * scale;
                CameraController.Instance.ZoomCamera.rect = new Rect(.5f - size.x / 2f, .5f - size.y / 2f, size.x, size.y);
				zoomWindow.transform.localScale = new Vector3(scale, scale, 1.0f);
				
            }

            yield return null;
        }
    }

    IEnumerator CloseZoomWindowCoroutine()
    {
        while (closingZoomWindow)
        {
            if (enabled)
            {
                float scale = zoomWindow.transform.localScale.x;

                if (scale < 0.01f)
                {
                    scale = 0.0f;
                    closingZoomWindow = false;
                    zoomWindow.gameObject.SetActive(false);
                    CameraController.Instance.ZoomCamera.gameObject.SetActive(false);
                }
                else
                {
                    scale = Mathf.Lerp(scale, 0.0f, 15.0f * Time.deltaTime);
                }

				Vector2 size = zoomWindowSize * scale;
                CameraController.Instance.ZoomCamera.rect = new Rect(.5f - size.x / 2f, .5f - size.y / 2f, size.x, size.y);
                zoomWindow.transform.localScale = new Vector3(scale, scale, 1.0f);
            }

            yield return null;
        }
    }

    public void SetBorderWarningWindow(bool state)
    {
        borderWarningPanel.SetActive(state);
    }

    public void SetBorderWarningTime(float warningTime)
    {
        warningTime = Mathf.CeilToInt(warningTime);

        borderWarningText.text = "RETURN TO MISSION AREA: " + warningTime.ToString("0.");
    }

    public void SetHitIndication()
    {
        hitIndicator.gameObject.SetActive(true);
        hitIndicator.color = new Color(hitIndicator.color.r, hitIndicator.color.g, hitIndicator.color.b, 1.0f);
    }

    public void SetObjectiveUpdated()
    {
        objectiveUpdatedIndicator.SetActive(true);
        objectiveUpdateShowTimer = Time.time + 8.0f;
    }
}
