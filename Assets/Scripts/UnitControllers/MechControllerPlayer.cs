using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class MechControllerPlayer : MechController
{
    public static MechControllerPlayer Instance { get; private set; }

    #region Variables
    [SerializeField]
    bool playerControlled;

    [SerializeField]
    CockpitController cockpitController;

    [SerializeField]
    WeaponController[] weaponControllersGroup1 = new WeaponController[0];

    [SerializeField]
    WeaponController[] weaponControllersGroup2 = new WeaponController[0];

    [SerializeField]
    WeaponController[] weaponControllersGroup3 = new WeaponController[0];

    [SerializeField]
    WeaponController[] weaponControllersGroup4 = new WeaponController[0];

    [SerializeField]
    WeaponController[] weaponControllersGroup5 = new WeaponController[0];

    [SerializeField]
    WeaponController[] weaponControllersGroup6 = new WeaponController[0];

    [SerializeField]
    bool externalCamera;

    [SerializeField]
    AudioClip lockingOnClip;

    [SerializeField]
    AudioClip lockedOnClip;

    [SerializeField]
    bool shutDownOverridden;

    [SerializeField]
    GameObject lastHoveredReticleObject;

    [SerializeField]
    bool cruiseControl;

    [SerializeField]
    float speedUpdateTimer;

    [SerializeField]
    float movementSpeed;

    [SerializeField]
    Vector3 previousPosition;

    [SerializeField]
    bool isInsideBounds;

    [SerializeField]
    float boundsCheckTimer;

    [SerializeField]
    float boundsTimer;

    [SerializeField]
    float missileWarningTimer;

    [SerializeField]
    float distanceTargeting = -1.0f;

    [SerializeField]
    FiringMode firingModeGroup1 = FiringMode.Standard;

    [SerializeField]
    FiringMode firingModeGroup2 = FiringMode.Standard;

    [SerializeField]
    FiringMode firingModeGroup3 = FiringMode.Standard;

    [SerializeField]
    FiringMode firingModeGroup4 = FiringMode.Standard;

    [SerializeField]
    FiringMode firingModeGroup5 = FiringMode.Standard;

    [SerializeField]
    FiringMode firingModeGroup6 = FiringMode.Standard;

    [SerializeField]
    int indexWeaponGroup1 = 0;

    [SerializeField]
    int indexWeaponGroup2 = 0;

    [SerializeField]
    int indexWeaponGroup3 = 0;

    [SerializeField]
    int indexWeaponGroup4 = 0;

    [SerializeField]
    int indexWeaponGroup5 = 0;

    [SerializeField]
    int indexWeaponGroup6 = 0;

    List<UnitController> alliedRadarTargets = new List<UnitController>();
    List<UnitController> neutralRadarTargets = new List<UnitController>();

    WaitForEndOfFrame waitForEndOfFrame = new WaitForEndOfFrame();

    [SerializeField]
    CommandSystem commandSystem;
#endregion

    public CommandSystem CommandSystem { get => commandSystem; }

    public CockpitController CockpitController { get => cockpitController; }

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();

        StartCoroutine(CalculateVelocity());

        previousPosition = transform.position;
    }

    protected override void Update()
    {
        if (Time.timeScale == 0.0f || IsDestroyed)
        {
            return;
        }

        base.Update();

        if (playerControlled)
        {
            UpdatePlayer();
        }
        else if (aiControlled)
        {
            UpdateAI();
        }

        if (isInLava)
        {
            if (!Cheats.godMode)
            {
                mechData.TakeDamageInternalLegLeft(2f * Time.deltaTime);
                mechData.TakeDamageInternalLegRight(2f * Time.deltaTime);
            }

            AddHeat(3000f * Time.deltaTime);

            currentThrottle = Mathf.Clamp(currentThrottle, -0.5f, 0.5f);
        }

        CheckGround();

        UpdateImpactForce();
    }

    public override void SetMechData(MechData data)
    {
        base.SetMechData(data);

        Instance = this;
        playerControlled = true;
        SetTeam(TeamType.Player);

        CameraController.Instance.transform.position = mechMetaController.CockpitHardpoint.transform.position;
        CameraController.Instance.transform.rotation = mechMetaController.CockpitHardpoint.transform.rotation;
        CameraController.Instance.transform.parent = mechMetaController.CockpitHardpoint;

        GameObject cockpitPrefab = mechData.CockpitPrefab;

        if (cockpitPrefab != null)
        {
            cockpitController = Instantiate(cockpitPrefab).GetComponent<CockpitController>();

            if (cockpitController != null)
            {
                cockpitController.transform.position = mechMetaController.CockpitHardpoint.transform.position;
                cockpitController.transform.rotation = mechMetaController.CockpitHardpoint.transform.rotation;
                cockpitController.transform.parent = mechMetaController.CockpitHardpoint;

                //cockpitController.SetMiniMapImage(mapProfile);
            }
            else
            {
                Debug.LogError("Cockpit Controller missing");
            }
        }

        mechMetaController.AudioSourceEngine.clip = ResourceManager.Instance.GetAudioClip("ReactorRunning");
        mechMetaController.AudioSourceEngine.Play();

        mechMetaController.AudioSourceTorso.clip = ResourceManager.Instance.GetAudioClip("TorsoMotor");
        mechMetaController.AudioSourceTorso.Play();

        mechMetaController.AudioSourceHeatWarning.clip = ResourceManager.Instance.GetAudioClip("HeatAlarm");
        mechMetaController.AudioSourceHeatWarning.Pause();

        AudioManager.Instance.PlayClip(mechMetaController.AudioSourceSystems, ResourceManager.Instance.GetAudioClip("ComputerStartup"), false, false);

        if (mechData.jumpCapacity == 0.0f)
        {
            // Need to hide jump bar
        }

        commandSystem = new CommandSystem(mechMetaController.AudioSourceSystems, mechMetaController.AudioSourceRadio);

        PlayerHUD.Instance.WeaponsManagerUI.InitializeWeaponUI(weaponControllersAll, equipmentControllers);

        SetFiringModeText();
        UpdateChainFireUI();

        PlayerHUD.Instance.WeaponsManagerUI.ClearChainFireSelection();

        lockingOnClip = ResourceManager.Instance.GetAudioClip("LockingOn");
        lockedOnClip = ResourceManager.Instance.GetAudioClip("LockedOn");
    }

    void UpdatePlayer()
    {
        if (InputManager.Instance.objectives.PressedThisFrame())
        {
            PlayerHUD.Instance.ToggleObjectivesWindow();
        }

        recoilHorizontalCurrent = Mathf.Lerp(recoilHorizontalCurrent, recoilHorizontalTarget, 35f * Time.deltaTime);
        recoilVerticalCurrent = Mathf.Lerp(recoilVerticalCurrent, recoilVerticalTarget, 35f * Time.deltaTime);

        recoilHorizontalTarget = Mathf.Lerp(recoilHorizontalTarget, 0.0f, 25f * Time.deltaTime);
        recoilVerticalTarget = Mathf.Lerp(recoilVerticalTarget, 0.0f, 25f * Time.deltaTime);

        cockpitVerticalCurrent = Mathf.Lerp(cockpitVerticalCurrent, cockpitVerticalTarget, 5.5f * Time.deltaTime);
        cockpitHorizontalCurrent = Mathf.Lerp(cockpitHorizontalCurrent, cockpitHorizontalTarget, 5.5f * Time.deltaTime);
        cockpitRotationCurrent = Mathf.Lerp(cockpitRotationCurrent, cockpitRotationTarget, 10f * Time.deltaTime);

        cockpitController.transform.localPosition = new Vector3(cockpitHorizontalCurrent, cockpitVerticalCurrent, 0f);
        cockpitController.transform.localRotation = Quaternion.identity;
        cockpitController.transform.RotateAround(cockpitController.transform.position, cockpitController.transform.forward, cockpitRotationCurrent);

        cockpitVerticalTarget = Mathf.Lerp(cockpitVerticalTarget, 0.0f, 3.5f * Time.deltaTime);
        cockpitHorizontalTarget = Mathf.Lerp(cockpitHorizontalTarget, 0.0f, 3.5f * Time.deltaTime);
        cockpitRotationTarget = Mathf.Lerp(cockpitRotationTarget, 0.0f, 5f * Time.deltaTime);

        cockpitController.SetReactorStatus(unitPowerState);

        if (unitPowerState != UnitPowerState.Shutdown)
        {
            UpdateTargetingPlayer();
            UpdateMissileLockonPlayer();
            UpdateHUD();

            if (InputManager.Instance.shutdownOverride.PressedThisFrame())
            {
                shutDownOverridden = !shutDownOverridden;
                cockpitController.OverrideWarningIndicator.SetActive(shutDownOverridden);

                if (shutDownOverridden && unitPowerState == UnitPowerState.ShuttingDown)
                {
                    unitPowerState = UnitPowerState.Normal;
                    mechMetaController.AudioSourcePower.Stop();
                    return;
                }
            }
            else if (InputManager.Instance.power.PressedThisFrame())
            {
                if (unitPowerState == UnitPowerState.ShuttingDown)
                {
                    unitPowerState = UnitPowerState.Normal;
                    mechMetaController.AudioSourcePower.Stop();
                    return;
                }
                else if (unitPowerState == UnitPowerState.StartingUp)
                {
                    unitPowerState = UnitPowerState.Shutdown;
                    mechMetaController.AudioSourcePower.Stop();
                    return;
                }
            }
        }

        if (unitPowerState == UnitPowerState.Normal)
        {
            UpdatePlayerMovement();

            if (mechData.HasJumpJets)
            {
                UpdatePlayerJumpJets();
            }

            UpdatePlayerCoolant();

            float zoomModifier = PlayerHUD.Instance.ZoomWindow.gameObject.activeInHierarchy ? 0.3f : 1.0f;

            float twistInput = Mathf.Clamp(InputManager.Instance.torsoYaw.GetValue() * zoomModifier, -2.0f, 2.0f);

            float pitchInput = Mathf.Clamp(InputManager.Instance.torsoPitch.GetValue() * zoomModifier, -2.0f, 2.0f);

            currentTorsoTwist += twistInput * mechData.torsoTwistSpeed * Time.deltaTime + recoilHorizontalCurrent;

            currentTorsoPitch -= pitchInput * mechData.torsoPitchSpeed * Time.deltaTime + recoilVerticalCurrent;

            float movementVolume = Mathf.Min(Mathf.Max(Mathf.Abs(twistInput), Mathf.Abs(pitchInput)), 0.5f);

            torsoMovementVolume = Mathf.Max(torsoMovementVolume, movementVolume);

            mechMetaController.AudioSourceTorso.volume = torsoMovementVolume;

            torsoMovementVolume -= Mathf.Lerp(torsoMovementVolume, 0.0f, 0.1f * Time.deltaTime);

            UpdatePlayerWeapons();

            if (InputManager.Instance.nextNavPoint.PressedThisFrame())
            {
                CycleNextNavigationPoint();

                AudioManager.Instance.PlayClip(mechMetaController.AudioSourceSystems, ResourceManager.Instance.GetAudioClip("CycleNavPoint"), false, false);
            }

            if (InputManager.Instance.toggleZoom.PressedThisFrame())
            {
                PlayerHUD.Instance.ToggleZoomWindow();

                AudioManager.Instance.PlayClip(mechMetaController.AudioSourceSystems, ResourceManager.Instance.GetAudioClip("ToggleZoom"), false, false);
            }

            if (InputManager.Instance.nextEnemyRadar.PressedThisFrame())
            {
                CycleNextEnemyTarget();
            }

            if (InputManager.Instance.lightAmplification.PressedThisFrame())
            {
                CameraController.Instance.ToggleNightVision(mechMetaController);
            }

            if (InputManager.Instance.weaponMenuUp.PressedThisFrame())
            {
                PlayerHUD.Instance.WeaponsManagerUI.RowChangeUp();
            }
            else if (InputManager.Instance.weaponMenuDown.PressedThisFrame())
            {
                PlayerHUD.Instance.WeaponsManagerUI.RowChangeDown();
            }
            else if (InputManager.Instance.weaponMenuRight.PressedThisFrame())
            {
                PlayerHUD.Instance.WeaponsManagerUI.ColumnChangeRight();
            }
            else if (InputManager.Instance.weaponMenuLeft.PressedThisFrame())
            {
                PlayerHUD.Instance.WeaponsManagerUI.ColumnChangeLeft();
            }
            else if (InputManager.Instance.weaponMenuSelect.PressedThisFrame())
            {
                PlayerHUD.Instance.WeaponsManagerUI.SelectElement();
            }

            if (missileWarningTimer > Time.time)
            {
                //PlayerHUD.instance.MissileWarningPanel.SetActive(true);
                cockpitController.MissileWarningIndicator.SetActive(true);

                if (!mechMetaController.AudioSourceMissileWarning.isPlaying)
                {
                    mechMetaController.AudioSourceMissileWarning.Play();
                }
            }
            else
            {
                //PlayerHUD.instance.MissileWarningPanel.SetActive(false);
                cockpitController.MissileWarningIndicator.SetActive(false);

                if (mechMetaController.AudioSourceMissileWarning.isPlaying)
                {
                    mechMetaController.AudioSourceMissileWarning.Pause();
                }
            }
        }

        if (heatCurrent > mechData.heatWarning)
        {
            //PlayerHUD.instance.HeatWarningIndicator.SetActive(true);
            cockpitController.HeatWarningIndicator.SetActive(true);

            if (!mechMetaController.AudioSourceHeatWarning.isPlaying)
            {
                mechMetaController.AudioSourceHeatWarning.Play();
            }
        }
        else
        {
            //PlayerHUD.instance.HeatWarningIndicator.SetActive(false);
            cockpitController.HeatWarningIndicator.SetActive(false);

            if (mechMetaController.AudioSourceHeatWarning.isPlaying)
            {
                mechMetaController.AudioSourceHeatWarning.Pause();
            }
        }

        if (InputManager.Instance.externalCamera.PressedThisFrame())
        {
            externalCamera = !externalCamera;

            if (externalCamera)
            {
                cockpitController.gameObject.SetActive(false);
                CameraController.Instance.transform.localPosition = new Vector3(0f, 2f, -25f);
                CameraController.Instance.CockpitCamera.gameObject.SetActive(false);
                //PlayerHUD.instance.SetHudCenterVisibility(false);
            }
            else
            {
                cockpitController.gameObject.SetActive(true);
                CameraController.Instance.transform.localPosition = Vector3.zero;
                CameraController.Instance.CockpitCamera.gameObject.SetActive(true);
                //PlayerHUD.instance.SetHudCenterVisibility(true);
            }
        }

        if (InputManager.Instance.power.PressedThisFrame())
        {
            if (unitPowerState == UnitPowerState.Shutdown && Time.time > shutdownTimer)
            {
                StartStartingUp();
            }
            else if (unitPowerState == UnitPowerState.Normal)
            {
                StartShuttingDown();
            }
        }

        if (!GlobalData.Instance.ActiveMissionData.MissionOver && Time.time > boundsCheckTimer)
        {
            boundsCheckTimer = Time.time + 1.0f;

            if (isInsideBounds)
            {
                isInsideBounds = MissionManager.Instance.PlayerIsInsideBounds(transform.position);

                if (!isInsideBounds)
                {
                    boundsTimer = Time.time;
                    PlayerHUD.Instance.SetBorderWarningWindow(true);
                    PlayerHUD.Instance.SetBorderWarningTime(10f);
                }
            }
            else
            {
                isInsideBounds = MissionManager.Instance.PlayerIsInsideBounds(transform.position);

                if (isInsideBounds)
                {
                    PlayerHUD.Instance.SetBorderWarningWindow(false);
                }
                else
                {
                    float outOfBoundsTime = Time.time - boundsTimer;

                    PlayerHUD.Instance.SetBorderWarningTime(10.0f - outOfBoundsTime);

                    if (outOfBoundsTime > 10.0f)
                    {
                        PlayerHUD.Instance.SetBorderWarningWindow(false);
                        MissionManager.Instance.SetMissionFailed();
                    }
                }
            }
        }

        commandSystem.Update(currentNavigationPoint, targetUnit);
    }

    public override void UpdateHeat()
    {
        if (heatCurrent > 0)
        {
            // Apply cooling
            heatCurrent -= GetCoolingRate() * Time.deltaTime;

            if (heatCurrent < 0f)
            {
                heatCurrent = 0f;
            }

            // Take heat related damage
            if (unitPowerState != UnitPowerState.Shutdown)
            {
                if (heatCurrent > mechData.heatLimit)
                {
                    TakeHeatDamage(ResourceManager.Instance.GameConstants.HeatDamageExtreme * Time.deltaTime);
                }
                if (heatCurrent > mechData.heatDamageMajor)
                {
                    TakeHeatDamage(ResourceManager.Instance.GameConstants.HeatDamageMajor * Time.deltaTime);
                }
                else if (heatCurrent > mechData.heatDamageMinor)
                {
                    TakeHeatDamage(ResourceManager.Instance.GameConstants.HeatDamageMinor * Time.deltaTime);
                }
            }

            // Check for shutdown or auto restart
            if (unitPowerState == UnitPowerState.Normal && heatCurrent > mechData.heatShutdown && !shutDownOverridden)
            {
                StartShuttingDown();

                autoRestart = true;
            }
            else if (autoRestart && unitPowerState == UnitPowerState.Shutdown && Time.time > shutdownTimer)
            {
                StartStartingUp();
            }
        }
        else
        {
            heatCurrent = 0;

            if (autoRestart && unitPowerState == UnitPowerState.Shutdown && Time.time > shutdownTimer)
            {
                StartStartingUp();
            }
        }
    }

    void UpdatePlayerJumpJets()
    {
        if (InputManager.Instance.jumpJet.ReleasedThisFrame())
        {
            jumpJetRechargeTimer = Time.time + 0.5f;

            mechMetaController.AudioSourceJumpJet.Pause();

            mechMetaController.SetJumpJetThrustersState(false);
        }
        else if (InputManager.Instance.jumpJet.IsPressed() && currentJumpJetFuel > 0f)
        {
            if (isGrounded && characterControllerMovement.y < 0)
            {
                characterControllerMovement.y = mechData.jumpTrust * Time.deltaTime;
            }

            if (InputManager.Instance.throttle.GetValue() > 0f)
            {
                characterControllerMovement += (transform.forward + Vector3.up + Vector3.up).normalized * mechData.jumpTrust * Time.deltaTime;
            }
            else if (InputManager.Instance.throttle.GetValue() < 0f)
            {
                characterControllerMovement += (-transform.forward + Vector3.up + Vector3.up).normalized * mechData.jumpTrust * Time.deltaTime;
            }
            else
            {
                characterControllerMovement.y += mechData.jumpTrust * Time.deltaTime;
            }

            AddHeat(mechData.jumpHeat * ResourceManager.Instance.GameConstants.JumpJetHeatMultiplier * Time.deltaTime);

            transform.Rotate(0, InputManager.Instance.turn.GetValue() * mechData.movementSpeedTurn * 2f * Time.deltaTime, 0);

            CameraController.Instance.AddShakeRotation(Random.onUnitSphere * 30.0f * Time.deltaTime);

            currentJumpJetFuel -= Time.deltaTime;

            if (currentJumpJetFuel <= 0)
            {
                jumpJetRechargeTimer = Time.time + 0.5f;

                mechMetaController.AudioSourceJumpJet.Pause();

                mechMetaController.SetJumpJetThrustersState(false);
            }
            else
            {
                if (!mechMetaController.AudioSourceJumpJet.isPlaying)
                {
                    mechMetaController.AudioSourceJumpJet.Play();
                }

                mechMetaController.SetJumpJetThrustersState(true);
            }
        }
        else
        {
            if (mechData.jumpCapacity > 0 && currentJumpJetFuel < mechData.jumpCapacity && Time.time > jumpJetRechargeTimer)
            {
                currentJumpJetFuel += mechData.jumpJetRecharge * Time.deltaTime;

                if (currentJumpJetFuel > mechData.jumpCapacity)
                {
                    currentJumpJetFuel = mechData.jumpCapacity;
                }
            }
        }
    }

    void UpdatePlayerMovement()
    {
        if (isGrounded || Time.time - airTimer < 1.0f)
        {
            float horizontal = InputManager.Instance.turn.GetValue();

            if (horizontal != 0)
            {
                transform.Rotate(0, horizontal * mechData.movementSpeedTurn * Time.deltaTime, 0);
                animationTurning = Mathf.Lerp(animationTurning, 1f, 4f * Time.deltaTime);
            }
            else if (InputManager.Instance.alignLegsToTorso.IsPressed())
            {
                if (currentTorsoTwist < -1f)
                {
                    float minTurnRate = Mathf.Min(mechData.movementSpeedTurn, mechData.torsoTwistSpeed, Mathf.Abs(currentTorsoTwist)) * Time.deltaTime;

                    currentTorsoTwist += minTurnRate;

                    transform.Rotate(0, -minTurnRate, 0);
                    animationTurning = Mathf.Lerp(animationTurning, 1f, 4f * Time.deltaTime);
                }
                else if (currentTorsoTwist > 1f)
                {
                    float minTurnRate = Mathf.Min(mechData.movementSpeedTurn, mechData.torsoTwistSpeed, Mathf.Abs(currentTorsoTwist)) * Time.deltaTime;

                    currentTorsoTwist -= minTurnRate;

                    transform.Rotate(0, minTurnRate, 0);
                    animationTurning = Mathf.Lerp(animationTurning, 1f, 4f * Time.deltaTime);
                }
                else
                {
                    animationTurning = Mathf.Lerp(animationTurning, 0f, 4f * Time.deltaTime);
                }
            }
            else
            {
                animationTurning = Mathf.Lerp(animationTurning, 0f, 4f * Time.deltaTime);
            }

            if (Time.time > collisionTimer)
            {
                float throttleInput = InputManager.Instance.throttle.GetValue();

                if (throttleInput != 0 && (throttleInput > 0 || mechData.HasBothLegs))
                {
                    targetThrottle = throttleInput;
                    cruiseControl = false;
                }
                else
                {
                    if (InputManager.Instance.setThrottle0.PressedThisFrame())
                    {
                        targetThrottle = 0;
                        cruiseControl = true;
                    }
                    else if (InputManager.Instance.setThrottle1.PressedThisFrame())
                    {
                        targetThrottle = 0.11f;
                        cruiseControl = true;
                    }
                    else if (InputManager.Instance.setThrottle2.PressedThisFrame())
                    {
                        targetThrottle = 0.22f;
                        cruiseControl = true;
                    }
                    else if (InputManager.Instance.setThrottle3.PressedThisFrame())
                    {
                        targetThrottle = 0.33f;
                        cruiseControl = true;
                    }
                    else if (InputManager.Instance.setThrottle4.PressedThisFrame())
                    {
                        targetThrottle = 0.44f;
                        cruiseControl = true;
                    }
                    else if (InputManager.Instance.setThrottle5.PressedThisFrame())
                    {
                        targetThrottle = 0.55f;
                        cruiseControl = true;
                    }
                    else if (InputManager.Instance.setThrottle6.PressedThisFrame())
                    {
                        targetThrottle = 0.66f;
                        cruiseControl = true;
                    }
                    else if (InputManager.Instance.setThrottle7.PressedThisFrame())
                    {
                        targetThrottle = 0.77f;
                        cruiseControl = true;
                    }
                    else if (InputManager.Instance.setThrottle8.PressedThisFrame())
                    {
                        targetThrottle = 0.88f;
                        cruiseControl = true;
                    }
                    else if (InputManager.Instance.setThrottle9.PressedThisFrame())
                    {
                        targetThrottle = 1.0f;
                        cruiseControl = true;
                    }
                    else if (!cruiseControl)
                    {
                        if (InputManager.Instance.useThrottleDecay)
                        {
                            targetThrottle = 0.0f;
                        }
                        else
                        {
                            targetThrottle = currentThrottle;
                        }
                    }
                }
            }
            else
            {
                targetThrottle = 0.0f;
            }
        }
        else
        {
            targetThrottle = 0.0f;
        }

        mechMetaController.AudioSourceEngine.volume = Mathf.Clamp(Mathf.Abs(currentThrottle), 0.1f, 0.65f);
    }

    void UpdatePlayerWeapons()
    {
        if (weaponControllersGroup1.Length > 0 && FireByMode(firingModeGroup1, InputManager.Instance.fireGroup1, weaponControllersGroup1, ref indexWeaponGroup1))
            return;

        if (weaponControllersGroup2.Length > 0 && FireByMode(firingModeGroup2, InputManager.Instance.fireGroup2, weaponControllersGroup2, ref indexWeaponGroup2))
            return;

        if (weaponControllersGroup3.Length > 0 && FireByMode(firingModeGroup3, InputManager.Instance.fireGroup3, weaponControllersGroup3, ref indexWeaponGroup3))
            return;

        if (weaponControllersGroup4.Length > 0 && FireByMode(firingModeGroup4, InputManager.Instance.fireGroup4, weaponControllersGroup4, ref indexWeaponGroup4))
            return;

        if (weaponControllersGroup5.Length > 0 && FireByMode(firingModeGroup5, InputManager.Instance.fireGroup5, weaponControllersGroup5, ref indexWeaponGroup5))
            return;

        if (weaponControllersGroup6.Length > 0 && FireByMode(firingModeGroup6, InputManager.Instance.fireGroup6, weaponControllersGroup6, ref indexWeaponGroup6))
            return;
    }

    bool FireByMode(FiringMode firingMode, InputManager.InputGroup inputGroup, WeaponController[] weaponControllers, ref int index)
    {
        if (firingMode == FiringMode.Standard)
        {
            return inputGroup.IsPressed() && FireWeaponGroup(weaponControllers, inputGroup.PressedThisFrame());
        }
        else
        {
            return ChainFireWeaponGroup(inputGroup, weaponControllers, ref index);
        }
    }

    bool FireWeaponGroup(WeaponController[] weaponControllers, bool firedThisFrame)
    {
        for (int i = 0; i < weaponControllers.Length; i++)
        {
            weaponControllers[i].Fire(firedThisFrame);

            if (!shutDownOverridden && heatCurrent > mechData.heatShutdown)
                return true;
        }

        return false;
    }

    bool ChainFireWeaponGroup(InputManager.InputGroup inputGroup, WeaponController[] weaponControllers, ref int weaponIndex)
    {
        if (inputGroup.IsPressed())
        {
            weaponControllers[weaponIndex].Fire(inputGroup.PressedThisFrame());

            if (!shutDownOverridden && heatCurrent > mechData.heatShutdown)
                return true;
        }
        else if (inputGroup.ReleasedThisFrame())
        {
            weaponIndex = GetNextValidWeaponIndex(weaponControllers, weaponIndex);

            UpdateChainFireUI();
        }

        return false;
    }

    int GetNextValidWeaponIndex(WeaponController[] weaponControllers, int weaponIndex)
    {
        int startingWeaponIndex = weaponIndex;

        for (int i = 0; i < weaponControllers.Length; i++)
        {
            weaponIndex++;

            if (weaponIndex == weaponControllers.Length)
            {
                weaponIndex = 0;
            }

            if (weaponControllers[weaponIndex].CanFire)
            {
                return weaponIndex;
            }
        }

        return startingWeaponIndex;
    }

    void UpdateTargetingPlayer()
    {
        Ray ray = new Ray(mechMetaController.CockpitHardpoint.position, mechMetaController.CockpitHardpoint.forward);
        if (Physics.Raycast(ray, out raycastHit, 400.0f, targetingLayerMask, QueryTriggerInteraction.Ignore))
        {
            targetingPoints[0] = raycastHit.point;
            distanceTargeting = (targetingPoints[0] - mechMetaController.CockpitHardpoint.position).magnitude;
            PlayerHUD.Instance.SetRangeText((distanceTargeting * 10f).ToString("0.") + "m");

            for (int i = 0; i < weaponControllersAll.Length; i++)
            {
                if (weaponControllersAll[i].IsDestroyed)
                {
                    PlayerHUD.Instance.WeaponsManagerUI.SetWeaponColorDestroyed(i);
                }
                else if (weaponControllersAll[i].Jammed)
                {
                    PlayerHUD.Instance.WeaponsManagerUI.SetWeaponColorJammed(i);
                }
                else if (weaponControllersAll[i].InEffectiveRange(distanceTargeting))
                {
                    PlayerHUD.Instance.WeaponsManagerUI.SetWeaponColorNormal(i);
                }
                else
                {
                    PlayerHUD.Instance.WeaponsManagerUI.SetWeaponColorOutOfRange(i);
                }
            }

            if (lastHoveredReticleObject != raycastHit.transform.root.gameObject)
            {
                lastHoveredReticleObject = raycastHit.transform.root.gameObject;

                lastHoveredTarget = lastHoveredReticleObject.GetComponent<UnitController>();

                if (lastHoveredTarget != null)
                {
                    if (lastHoveredTarget.IsDestroyed)
                    {
                        PlayerHUD.Instance.SetTargetingReticleColor(Color.green);
                    }
                    else if (MissionManager.Instance.EnemyUnits.Contains(lastHoveredTarget))
                    {
                        PlayerHUD.Instance.SetTargetingReticleColor(Color.red);
                    }
                    else if (MissionManager.Instance.AllyUnits.Contains(lastHoveredTarget))
                    {
                        PlayerHUD.Instance.SetTargetingReticleColor(Color.blue);
                    }
                    else
                    {
                        PlayerHUD.Instance.SetTargetingReticleColor(Color.yellow);
                    }
                }
                else if (lastHoveredReticleObject.layer == 20)
                {
                    PlayerHUD.Instance.SetTargetingReticleColor(Color.yellow);
                }
                else
                {
                    PlayerHUD.Instance.SetTargetingReticleColor(Color.green);
                }
            }
        }
        else
        {
            targetingPoints[0] = mechMetaController.CockpitHardpoint.position + mechMetaController.CockpitHardpoint.forward * 100f;
            distanceTargeting = -1.0f;
            PlayerHUD.Instance.SetRangeText("");

            for (int i = 0; i < weaponControllersAll.Length; i++)
            {
                if (weaponControllersAll[i].IsDestroyed)
                {
                    PlayerHUD.Instance.WeaponsManagerUI.SetWeaponColorDestroyed(i);
                }
                else if (weaponControllersAll[i].Jammed)
                {
                    PlayerHUD.Instance.WeaponsManagerUI.SetWeaponColorJammed(i);
                }
                else
                {
                    PlayerHUD.Instance.WeaponsManagerUI.SetWeaponColorOutOfRange(i);
                }
            }

            lastHoveredReticleObject = null;
            lastHoveredTarget = null;
            PlayerHUD.Instance.SetTargetingReticleColor(Color.green);
        }

        //Target convergence
        if (targetUnit != null && targetUnit != lastHoveredTarget)
        {
            Vector3 targetDirection = targetUnit.Bounds.center - mechMetaController.CockpitHardpoint.position;
            float targetDistance = targetDirection.magnitude;

            float rangeFactor = Mathf.Max(1.0f, targetDistance / 50.0f);

            float targetSize = Mathf.Sqrt(targetUnit.Bounds.size.x * targetUnit.Bounds.size.x + targetUnit.Bounds.size.y * targetUnit.Bounds.size.y);

            float angleThreshold = Mathf.Tan(targetSize / targetDistance) * rangeFactor * ((180 / Mathf.PI));
            float targetAngle = Vector3.Angle(targetDirection, mechMetaController.CockpitHardpoint.forward);

            if (targetAngle < angleThreshold)
            {
                targetingPoints[0] = mechMetaController.CockpitHardpoint.position + mechMetaController.CockpitHardpoint.forward * targetDistance;
            }
        }

        if (InputManager.Instance.targetUnderReticle.PressedThisFrame())
        {
            if (lastHoveredTarget != null)
            {
                float distance = (lastHoveredTarget.transform.position - transform.position).magnitude;

                if (lastHoveredTarget.GetRadarDetectionReduction() != 0)
                {
                    if (distance < GetRadarDetectionRange() - lastHoveredTarget.GetRadarDetectionReduction())
                    {
                        targetUnit = lastHoveredTarget;
                        AudioManager.Instance.PlayClip(mechMetaController.AudioSourceSystems, ResourceManager.Instance.GetAudioClip("ChangeTarget"), false, false);
                    }
                }
                else if (distance < GetRadarDetectionRange())
                {
                    targetUnit = lastHoveredTarget;
                    AudioManager.Instance.PlayClip(mechMetaController.AudioSourceSystems, ResourceManager.Instance.GetAudioClip("ChangeTarget"), false, false);
                }
            }
            else
            {
                targetUnit = null;
            }
        }
    }

    void UpdateMissileLockonPlayer()
    {
        if (maxLockOnRange > 0.0f)
        {
            Ray ray = new Ray(mechMetaController.CockpitHardpoint.position, mechMetaController.CockpitHardpoint.forward);

            if (Physics.Raycast(ray, out raycastHit, maxLockOnRange, aiVisionLayerMask, QueryTriggerInteraction.Ignore))
            {
                if (raycastHit.transform.root.gameObject != lastHoveredLockonObject)
                {
                    lastHoveredLockonObject = raycastHit.transform.gameObject;

                    if (lastHoveredLockonObject.layer == unitLayer)
                    {
                        UnitController unitController = lastHoveredLockonObject.GetComponent<UnitController>();

                        if (unitController.Team == TeamType.Enemy)
                        {
                            lastHoveredLockonTarget = unitController;
                            lockingOnValue = 0f;
                        }
                        else
                        {
                            lastHoveredLockonTarget = null;
                        }
                    }
                    else
                    {
                        lastHoveredLockonTarget = null;
                    }
                }
                else if (lastHoveredLockonTarget != null)
                {
                    float distance = (mechMetaController.CockpitHardpoint.position - raycastHit.point).magnitude;

                    if (distance < maxLockOnRange)
                    {
                        if (!lastHoveredLockonTarget.IsDestroyed)
                        {
                            if (currentLockedOnTarget != null && currentLockedOnTarget == lastHoveredLockonTarget)
                            {
                                lockedOnTimer = Time.time + 3.0f;
                                lockingOnValue = 0f;
                            }
                            else if (lockingOnValue < 1.0f)
                            {
                                lockingOnValue += GetLockValue(distance, 200f, GetTargetLockOnBonus());

                                if (lockingOnValue > 1.0f)
                                {
                                    currentLockedOnTarget = lastHoveredLockonTarget;
                                    lockedOnTimer = Time.time + 3.0f;
                                }
                            }
                        }
                        else
                        {
                            lockingOnValue = 0f;
                        }
                    }
                    else
                    {
                        lockingOnValue = 0f;
                    }
                }
            }
            else
            {
                lastHoveredLockonObject = null;
                lockingOnValue = 0.0f;
            }

            if (currentLockedOnTarget != null)
            {
                if (Time.time > lockedOnTimer || currentLockedOnTarget.IsDestroyed)
                {
                    currentLockedOnTarget = null;
                }
                else
                {
                    PlayLockedOnClip();
                }
            }
            else if (lastHoveredLockonTarget != null && lockingOnValue > 0.0f)
            {
                PlayLockingOnClip();
            }
            else
            {
                StopPlayingLockOnSystem();
            }
        }
        else
        {
            StopPlayingLockOnSystem();
            lockingOnValue = 0;
        }
    }

    void UpdatePlayerCoolant()
    {
        if (InputManager.Instance.flushCoolant.ReleasedThisFrame() || unitPowerState != UnitPowerState.Normal)
        {
            if (mechMetaController.AudioSourceCoolant.isPlaying)
            {
                mechMetaController.AudioSourceCoolant.Pause();
            }

            mechMetaController.SetCoolantEffect(false);
        }
        else if (InputManager.Instance.flushCoolant.IsPressed() && heatCurrent > 0 && coolant > 0)
        {
            coolant -= Time.deltaTime;
            heatCurrent -= ResourceManager.Instance.GameConstants.CoolantHeatReduction * Time.deltaTime;

            bool stopCoolant = false;

            if (coolant <= 0)
            {
                coolant = 0;
                stopCoolant = true;
            }

            if (heatCurrent <= 0)
            {
                heatCurrent = 0;
                stopCoolant = true;
            }

            if (stopCoolant)
            {
                if (mechMetaController.AudioSourceCoolant.isPlaying)
                {
                    mechMetaController.AudioSourceCoolant.Pause();
                }

                mechMetaController.SetCoolantEffect(false);
            }
            else if (!mechMetaController.AudioSourceCoolant.isPlaying)
            {
                mechMetaController.AudioSourceCoolant.Play();

                mechMetaController.SetCoolantEffect(true);
            }
        }
    }

    void UpdateHUD()
    {
        PlayerHUD.Instance.SetCompassDirection(CameraController.Instance.transform);

        //PlayerHUD.Instance.UpdateMiniMap(transform);
        //cockpitController.UpdateMiniMap(transform);

        PlayerHUD.Instance.SetThrottle(currentThrottle);
        cockpitController.SetThrottleGauge(currentThrottle);

        if (mechData.HasJumpJets)
        {
            float fuelRatio = currentJumpJetFuel / mechData.jumpCapacity;

            //PlayerHUD.Instance.SetJumpJetValue(fuelRatio);
            cockpitController.SetJumpJetGauge(fuelRatio);
        }
        else
        {
            //PlayerHUD.Instance.SetJumpJetValue(0f);
            cockpitController.SetJumpJetGauge(0.0f);
        }

        if (Time.time > speedUpdateTimer)
        {
            speedUpdateTimer = Time.time + 0.25f;

            PlayerHUD.Instance.SetSpeedText(movementSpeed);
            cockpitController.SetSpeedText(movementSpeed);
        }

        float heatRatio = heatCurrent / mechData.heatLimit;

        PlayerHUD.Instance.SetHeat(heatRatio);
        PlayerHUD.Instance.SetTempuratureText(heatCurrent);
        cockpitController.SetHeatGauge(heatRatio);
        cockpitController.SetHeatText(heatCurrent);

        PlayerHUD.Instance.SetDamageDisplay(this);
        cockpitController.MechDamageDisplay.SetDisplays(mechData);

        if (targetUnit != null)
        {
            if (targetUnit.IsDestroyed && lastHoveredReticleObject == targetUnit.gameObject)
            {
                lastHoveredReticleObject = null;
            }

            if (!targetUnit.CanBeDetectedByRadar)
            {
                targetUnit = null;
            }
        }

        //PlayerHUD.instance.RadarController.UpdateRadar(transform, -currentTorsoTwist, targetUnit, enemyRadarTargets, allyRadarTargets, currentNavigationPoint.transform);
        cockpitController.RadarController.UpdateRadar(transform, -currentTorsoTwist, targetUnit, groupIntel.EnemyRadarTargets, alliedRadarTargets, neutralRadarTargets, currentNavigationPoint);

        if (targetUnit != null)
        {
            Color color;
            string distance = ((targetUnit.transform.position - transform.position).magnitude * 10f).ToString("0.") + "m";

            if (MissionManager.Instance.EnemyUnits.Contains(targetUnit))
            {
                color = Color.red;
            }
            else if (MissionManager.Instance.AllyUnits.Contains(targetUnit))
            {
                color = Color.blue;
            }
            else
            {
                color = Color.yellow;
            }

            //PlayerHUD.instance.SetTargetInfo(targetUnit, color, distance);
            cockpitController.EnemyTargetInformationHUD.SetTargetInfo(targetUnit, color, distance);

            PlayerHUD.Instance.SetTargetIndicator(targetUnit, color);
        }
        else
        {
            PlayerHUD.Instance.SetTargetIndicator(targetUnit, Color.clear);
            cockpitController.EnemyTargetInformationHUD.SetTargetInfo(null, Color.white, "");
        }

        PlayerHUD.Instance.SetLockedOnIndicator(currentLockedOnTarget);

        if (currentNavigationPoint != null)
        {
            PlayerHUD.Instance.SetNavPointIndicator(currentNavigationPoint.position, transform.position);
        }

        for (int i = 0; i < weaponControllersAll.Length; i++)
        {
            PlayerHUD.Instance.WeaponsManagerUI.SetWeaponRecycleBar(i, weaponControllersAll[i].GetRefireBar());

            PlayerHUD.Instance.WeaponsManagerUI.SetWeaponJammingBar(i, weaponControllersAll[i].Jamming);

            if (weaponControllersAll[i] is ProjectileWeaponController)
            {
                ProjectileWeaponController projectileWeaponController = weaponControllersAll[i] as ProjectileWeaponController;
                ProjectileWeaponDefinition projectileWeaponDefinition = projectileWeaponController.ProjectileWeaponDefinition;

                if (projectileWeaponDefinition.RequiresAmmo)
                {
                    PlayerHUD.Instance.WeaponsManagerUI.SetWeaponAmmoText(i, projectileWeaponController.AmmoNameDisplay, projectileWeaponController.AmmoCountDisplay);
                }
            }
        }

        for (int i = 0; i < equipmentControllers.Length; i++)
        {
            EquipmentController equipmentController = equipmentControllers[i];

            if (equipmentController.IsDestroyed)
            {
                PlayerHUD.Instance.WeaponsManagerUI.SetEquipmentColorDestroyed(i);
            }
            else
            {
                PlayerHUD.Instance.WeaponsManagerUI.SetEquipmentBar(i, equipmentController.BarValue);

                if (equipmentController.RequiresAmmo)
                {
                    PlayerHUD.Instance.WeaponsManagerUI.SetEquipmentAmmoText(i, equipmentController.CurrentAmmo);
                }
            }
        }

        if (weaponControllersGroup1.Length > 0)
        {
            PlayerHUD.Instance.WeaponGroup1IndicatorImage.gameObject.SetActive(true);

            PlayerHUD.Instance.WeaponGroup1IndicatorImage.color = WeaponGroupActiveColor(weaponControllersGroup1);

            PlayerHUD.Instance.WeaponGroup1IndicatorJamming.value = WeaponGroupJamming(weaponControllersGroup1);
        }
        else
        {
            PlayerHUD.Instance.WeaponGroup1IndicatorImage.gameObject.SetActive(false);
        }

        if (weaponControllersGroup2.Length > 0)
        {
            PlayerHUD.Instance.WeaponGroup2IndicatorImage.gameObject.SetActive(true);

            PlayerHUD.Instance.WeaponGroup2IndicatorImage.color = WeaponGroupActiveColor(weaponControllersGroup2);

            PlayerHUD.Instance.WeaponGroup2IndicatorJamming.value = WeaponGroupJamming(weaponControllersGroup2);
        }
        else
        {
            PlayerHUD.Instance.WeaponGroup2IndicatorImage.gameObject.SetActive(false);
        }

        if (weaponControllersGroup3.Length > 0)
        {
            PlayerHUD.Instance.WeaponGroup3IndicatorImage.gameObject.SetActive(true);

            PlayerHUD.Instance.WeaponGroup3IndicatorImage.color = WeaponGroupActiveColor(weaponControllersGroup3);

            PlayerHUD.Instance.WeaponGroup3IndicatorJamming.value = WeaponGroupJamming(weaponControllersGroup3);
        }
        else
        {
            PlayerHUD.Instance.WeaponGroup3IndicatorImage.gameObject.SetActive(false);
        }

        if (weaponControllersGroup4.Length > 0)
        {
            PlayerHUD.Instance.WeaponGroup4IndicatorImage.gameObject.SetActive(true);

            PlayerHUD.Instance.WeaponGroup4IndicatorImage.color = WeaponGroupActiveColor(weaponControllersGroup4);

            PlayerHUD.Instance.WeaponGroup4IndicatorJamming.value = WeaponGroupJamming(weaponControllersGroup4);
        }
        else
        {
            PlayerHUD.Instance.WeaponGroup4IndicatorImage.gameObject.SetActive(false);
        }

        if (weaponControllersGroup5.Length > 0)
        {
            PlayerHUD.Instance.WeaponGroup5IndicatorImage.gameObject.SetActive(true);

            PlayerHUD.Instance.WeaponGroup5IndicatorImage.color = WeaponGroupActiveColor(weaponControllersGroup5);

            PlayerHUD.Instance.WeaponGroup5IndicatorJamming.value = WeaponGroupJamming(weaponControllersGroup5);
        }
        else
        {
            PlayerHUD.Instance.WeaponGroup5IndicatorImage.gameObject.SetActive(false);
        }

        if (weaponControllersGroup6.Length > 0)
        {
            PlayerHUD.Instance.WeaponGroup6IndicatorImage.gameObject.SetActive(true);

            PlayerHUD.Instance.WeaponGroup6IndicatorImage.color = WeaponGroupActiveColor(weaponControllersGroup6);

            PlayerHUD.Instance.WeaponGroup6IndicatorJamming.value = WeaponGroupJamming(weaponControllersGroup6);
        }
        else
        {
            PlayerHUD.Instance.WeaponGroup6IndicatorImage.gameObject.SetActive(false);
        }

        cockpitController.SetTorsoTwistBar(currentTorsoTwist / mechChassisDefinition.TorsoTwistMax);

        cockpitController.SetCoolantGauge(coolant / mechData.coolantMax);
    }

    protected override void UpdateAnimation()
    {
        if (isGrounded)
        {
            animator.SetBool("Flying", false);
            animator.applyRootMotion = true;

            if (!isGroundedLast) //Time.time - airTimer > 1.0f)
            {
                if (characterControllerMovementLast.magnitude > 8f)
                {
                    animator.SetTrigger("Landing");
                }

                float airTime = Time.time - airTimer;

                if (airTime > 0.3f)
                {
                    if (airTime > 1.0f)
                    {
                        AudioManager.Instance.PlayClip(mechMetaController.AudioSourceFeet, ResourceManager.Instance.GetAudioClip("MechLand"), true, false);
                    }

                    CameraController.Instance.AddShakeRotation(Vector3.right * characterControllerMovementLast.magnitude);
                }
            }
        }
        else if (Time.time - airTimer > 0.4f)
        {
            animator.SetBool("Flying", true);
            animator.speed = 1f;
        }

        if (currentThrottle > 0)
        {
            if (mechData.LegLeftDestroyed)
            {
                if (animationMovement < 3f)
                {
                    animationMovement += 3f * Time.deltaTime;

                    if (animationMovement > 3f)
                    {
                        animationMovement = 3f;
                    }
                }
            }
            else if (mechData.LegRightDestroyed)
            {
                if (animationMovement < 2f)
                {
                    animationMovement += 3f * Time.deltaTime;

                    if (animationMovement > 2f)
                    {
                        animationMovement = 2f;
                    }
                }
            }
            else
            {
                if (animationMovement < 1f)
                {
                    animationMovement += 3f * Time.deltaTime;

                    if (animationMovement > 1f)
                    {
                        animationMovement = 1f;
                    }
                }
            }

            animator.speed = currentThrottle * mechData.movementSpeedForward;
            animator.SetFloat("Turning", 0f);
        }
        else if (currentThrottle < 0)
        {
            if (animationMovement > -1f)
            {
                animationMovement -= 3f * Time.deltaTime;

                if (animationMovement < -1f)
                {
                    animationMovement = -1f;
                }
            }

            animator.speed = -currentThrottle * mechData.movementSpeedReverse;
            animator.SetFloat("Turning", 0f);
        }
        else
        {
            if (animationMovement > 0f)
            {
                animationMovement -= 3f * Time.deltaTime;

                if (animationMovement < 0f)
                {
                    animationMovement = 0f;
                }
            }
            else if (animationMovement < 0f)
            {
                animationMovement += 3f * Time.deltaTime;

                if (animationMovement > 0f)
                {
                    animationMovement = 0f;
                }
            }

            if (animationTurning < 0.01f)
                animationTurning = 0.0f;

            animator.SetFloat("Turning", animationTurning);
            animator.speed = 1f;
        }

        animator.SetFloat("Movement", animationMovement);
    }

    protected override void UpdateRadar()
    {
        if (Time.time > radarTimer)
        {
            radarTimer = Time.time + 0.5f;

            groupIntel.ClearEnemyRadarTargets();

            CheckRadarCanDetectUnits(MissionManager.Instance.EnemyUnits, groupIntel.EnemyRadarTargets);

            alliedRadarTargets.Clear();

            for (int i = 0; i < MissionManager.Instance.AllyUnits.Count; i++)
            {
                UnitController alliedUnit = MissionManager.Instance.AllyUnits[i];

                if (alliedUnit.CanBeDetectedByRadar)
                {
                    alliedRadarTargets.Add(alliedUnit);
                }
            }

            neutralRadarTargets.Clear();

            CheckRadarCanDetectUnits(MissionManager.Instance.NeutralUnits, neutralRadarTargets);
        }
    }

    public override void TakeDamage(Collider hitCollider, Vector2 direction, float damage, WeaponController weaponController)
    {
        if (IsDestroyed || Cheats.godMode)
        {
            return;
        }

        base.TakeDamage(hitCollider, direction, damage, weaponController);
    }

    public override void TakeDirectSplashDamage(Collider hitCollider, Vector2 direction, float damage, WeaponController weaponController)
    {
        if (IsDestroyed || Cheats.godMode)
        {
            return;
        }

        base.TakeDirectSplashDamage(hitCollider, direction, damage, weaponController);
    }

    public override void TakeIndirectSplashDamage(Vector2 direction, float damage, WeaponController weaponController)
    {
        if (IsDestroyed || Cheats.godMode)
        {
            return;
        }

        base.TakeIndirectSplashDamage(direction, damage, weaponController);
    }

    protected override void TakeDamageLegsInternal(float legDamage)
    {
        if (IsDestroyed || Cheats.godMode)
        {
            return;
        }

        mechData.TakeDamageInternalLegLeft(legDamage);
        mechData.TakeDamageInternalLegRight(legDamage);
    }

    protected override void BuildWeapons()
    {
        base.BuildWeapons();

        List<WeaponController> group1 = new List<WeaponController>();
        List<WeaponController> group2 = new List<WeaponController>();
        List<WeaponController> group3 = new List<WeaponController>();
        List<WeaponController> group4 = new List<WeaponController>();
        List<WeaponController> group5 = new List<WeaponController>();
        List<WeaponController> group6 = new List<WeaponController>();

        foreach (WeaponController weaponController in weaponControllersAll)
        {
            if (weaponController.weaponGrouping.WeaponGroup1)
            {
                group1.Add(weaponController);
            }

            if (weaponController.weaponGrouping.WeaponGroup2)
            {
                group2.Add(weaponController);
            }

            if (weaponController.weaponGrouping.WeaponGroup3)
            {
                group3.Add(weaponController);
            }

            if (weaponController.weaponGrouping.WeaponGroup4)
            {
                group4.Add(weaponController);
            }

            if (weaponController.weaponGrouping.WeaponGroup5)
            {
                group5.Add(weaponController);
            }

            if (weaponController.weaponGrouping.WeaponGroup6)
            {
                group6.Add(weaponController);
            }
        }

        weaponControllersGroup1 = group1.ToArray();
        weaponControllersGroup2 = group2.ToArray();
        weaponControllersGroup3 = group3.ToArray();
        weaponControllersGroup4 = group4.ToArray();
        weaponControllersGroup5 = group5.ToArray();
        weaponControllersGroup6 = group6.ToArray();
    }

    void CycleNextNavigationPoint()
    {
        Transform[] navigationPoints = MissionManager.Instance.NavigationPoints;

        if (navigationPoints.Length > 1 && currentNavigationPoint != null)
        {
            for (int i = 0; i < navigationPoints.Length; i++)
            {
                if (navigationPoints[i] == currentNavigationPoint)
                {
                    i++;

                    if (i == navigationPoints.Length)
                    {
                        i = 0;
                    }

                    currentNavigationPoint = navigationPoints[i];
                    PlayerHUD.Instance.navPointName = StaticHelper.GetNavPointName(i);

                    break;
                }
            }
        }
    }

    void CycleNextEnemyTarget()
    {
        if (groupIntel.EnemyRadarTargets.Count == 0)
        {
            targetUnit = null;
            return;
        }

        AudioManager.Instance.PlayClip(mechMetaController.AudioSourceSystems, ResourceManager.Instance.GetAudioClip("ChangeTarget"), false, false);

        if (targetUnit == null)
        {
            targetUnit = GetClosestEnemy();
            return;
        }
        else
        {
            int index = groupIntel.EnemyRadarTargets.IndexOf(targetUnit);

            if (index == -1)
            {
                targetUnit = GetClosestEnemy();
                return;
            }
            else
            {
                for (int i = 0; i < groupIntel.EnemyRadarTargets.Count; i++)
                {
                    index++;

                    if (index == groupIntel.EnemyRadarTargets.Count)
                    {
                        index = 0;
                    }

                    if (!groupIntel.EnemyRadarTargets[index].IsDestroyed)
                    {
                        targetUnit = groupIntel.EnemyRadarTargets[index];
                        return;
                    }
                }
            }
        }
    }

    public void SetPlayerControl(bool state)
    {
        playerControlled = state;
    }

    public override void AddHeat(float value)
    {
        if (Cheats.noHeat)
            return;

        if (unitPowerState != UnitPowerState.Shutdown)
        {
            heatCurrent += value;
        }
    }

    public override void AddHeatDamage(float value)
    {
        if (Cheats.noHeat)
            return;

        if (unitPowerState != UnitPowerState.Shutdown)
        {
            if (heatCurrent < mechData.heatDamageMajor)
            {
                heatCurrent += value;

                if (heatCurrent > mechData.heatDamageMajor)
                {
                    heatCurrent = mechData.heatDamageMajor;
                }
            }
        }
    }

    public override void Die()
    {
        base.Die();

        Career career = GlobalDataManager.Instance.currentCareer;

        if (career != null && career.IsReal)
        {
            if (mechData.HeadDestroyed)
                career.deceased = true;
        }

        GlobalData.Instance.ActiveMissionData.failed = true;

        PlayerHUD.Instance.SetHudVisibility(false);

        cockpitController.gameObject.SetActive(false);

        CameraController cameraController = CameraController.Instance;

        cameraController.transform.parent = null;
        DeathCameraController deathCameraController = cameraController.gameObject.AddComponent<DeathCameraController>();
        deathCameraController.SetTarget(mechMetaController.TorsoTransform.gameObject);
        cameraController.CockpitCamera.gameObject.SetActive(false);
        cameraController.ZoomCamera.gameObject.SetActive(false);

        string causeOfDestruction;
        string pilotStatus;

        switch (mechData.causeOfDestruction)
        {
            case MechCauseOfDestructionType.HeadDestroyed:
                {
                    causeOfDestruction = "Cockpit Destroyed";
                    break;
                }
            case MechCauseOfDestructionType.CenterTorsoDestroyed:
                {
                    causeOfDestruction = "Torso Destroyed";
                    break;
                }
            case MechCauseOfDestructionType.LegsDestroyed:
                {
                    causeOfDestruction = "Legs Destroyed";
                    break;
                }
            case MechCauseOfDestructionType.OverHeating:
                {
                    causeOfDestruction = "Over Heating";
                    break;
                }
            case MechCauseOfDestructionType.ReactorBreach:
                {
                    causeOfDestruction = "Reactor Breach";
                    break;
                }
            case MechCauseOfDestructionType.AmmoExplosion:
                {
                    causeOfDestruction = "Ammo Explosion";
                    break;
                }
            case MechCauseOfDestructionType.WeaponExplosion:
                {
                    causeOfDestruction = "Weapon Explosion";
                    break;
                }
            default:
                {
                    causeOfDestruction = "Internal Explosion";
                    break;
                }
        }

        if (mechData.pilotKilled)
        {
            pilotStatus = "Deceased";
        }
        else
        {
            pilotStatus = "Successful Ejection";
        }

        PlayerHUD.Instance.SetDeathMessage("Cause of Destruction: " + causeOfDestruction, "Pilot Status: " + pilotStatus);
    }

















    protected override void StartStartingUp()
    {
        base.StartStartingUp();

        PlayReactorStartupSound();
    }

    protected override void StartShuttingDown()
    {
        base.StartShuttingDown();

        PlayReactorShuttingDownSound();
    }

    protected override void StartUp()
    {
        base.StartUp();

        PlayerHUD.Instance.OpenHUD();
        cockpitController.DisplayRoot.SetActive(true);

        AudioManager.Instance.PlayClip(mechMetaController.AudioSourceSystems, ResourceManager.Instance.GetAudioClip("ComputerStartup"), false, false);
    }

    protected override void Shutdown()
    {
        base.Shutdown();

        PlayerHUD.Instance.CloseZoomWindow();
        PlayerHUD.Instance.CloseHUD();
        cockpitController.DisplayRoot.SetActive(false);

        AudioManager.Instance.PlayClip(mechMetaController.AudioSourcePower, ResourceManager.Instance.GetAudioClip("ReactorStop"), false, false);
        mechMetaController.AudioSourceEngine.volume = 0.0f;

        StopPlayingLockOnSystem();

        if (mechMetaController.AudioSourceHeatWarning.isPlaying)
        {
            mechMetaController.AudioSourceHeatWarning.Pause();
        }

        if (mechMetaController.AudioSourceMissileWarning.isPlaying)
        {
            mechMetaController.AudioSourceMissileWarning.Pause();
        }
    }

    protected override void TakeHeatDamage(float damage)
    {
        if (Cheats.godMode)
            return;

        mechData.TakeDamageInternalTorsoCenter(damage);
    }

    public void ToggleWeaponGroup(int weaponIndex, int groupIndex)
    {
        weaponControllersAll[weaponIndex].weaponGrouping.Toggle(groupIndex);

        PlayerHUD.Instance.WeaponsManagerUI.UpdateWeaponGrouping(weaponControllersAll);

        List<WeaponController> group1 = new List<WeaponController>();
        List<WeaponController> group2 = new List<WeaponController>();
        List<WeaponController> group3 = new List<WeaponController>();
        List<WeaponController> group4 = new List<WeaponController>();
        List<WeaponController> group5 = new List<WeaponController>();
        List<WeaponController> group6 = new List<WeaponController>();

        for (int i = 0; i < weaponControllersAll.Length; i++)
        {
            WeaponController weaponController = weaponControllersAll[i];

            if (weaponController.weaponGrouping.WeaponGroup1)
            {
                group1.Add(weaponController);
            }

            if (weaponController.weaponGrouping.WeaponGroup2)
            {
                group2.Add(weaponController);
            }

            if (weaponController.weaponGrouping.WeaponGroup3)
            {
                group3.Add(weaponController);
            }

            if (weaponController.weaponGrouping.WeaponGroup4)
            {
                group4.Add(weaponController);
            }

            if (weaponController.weaponGrouping.WeaponGroup5)
            {
                group5.Add(weaponController);
            }

            if (weaponController.weaponGrouping.WeaponGroup6)
            {
                group6.Add(weaponController);
            }
        }

        weaponControllersGroup1 = group1.ToArray();
        weaponControllersGroup2 = group2.ToArray();
        weaponControllersGroup3 = group3.ToArray();
        weaponControllersGroup4 = group4.ToArray();
        weaponControllersGroup5 = group5.ToArray();
        weaponControllersGroup6 = group6.ToArray();

        indexWeaponGroup1 = Mathf.Clamp(indexWeaponGroup1, 0, Mathf.Max(weaponControllersGroup1.Length - 1, 0));
        indexWeaponGroup2 = Mathf.Clamp(indexWeaponGroup2, 0, Mathf.Max(weaponControllersGroup2.Length - 1, 0));
        indexWeaponGroup3 = Mathf.Clamp(indexWeaponGroup3, 0, Mathf.Max(weaponControllersGroup3.Length - 1, 0));
        indexWeaponGroup4 = Mathf.Clamp(indexWeaponGroup4, 0, Mathf.Max(weaponControllersGroup4.Length - 1, 0));
        indexWeaponGroup5 = Mathf.Clamp(indexWeaponGroup5, 0, Mathf.Max(weaponControllersGroup5.Length - 1, 0));
        indexWeaponGroup6 = Mathf.Clamp(indexWeaponGroup6, 0, Mathf.Max(weaponControllersGroup6.Length - 1, 0));

        UpdateChainFireUI();
    }

    void UpdateChainFireUI()
    {
        int index1 = -1;
        int index2 = -1;
        int index3 = -1;
        int index4 = -1;
        int index5 = -1;
        int index6 = -1;

        if (weaponControllersGroup1.Length > 0 && firingModeGroup1 == FiringMode.Chain)
            index1 = System.Array.IndexOf(weaponControllersAll, weaponControllersGroup1[indexWeaponGroup1]);

        if (weaponControllersGroup2.Length > 0 && firingModeGroup2 == FiringMode.Chain)
            index2 = System.Array.IndexOf(weaponControllersAll, weaponControllersGroup2[indexWeaponGroup2]);

        if (weaponControllersGroup3.Length > 0 && firingModeGroup3 == FiringMode.Chain)
            index3 = System.Array.IndexOf(weaponControllersAll, weaponControllersGroup3[indexWeaponGroup3]);

        if (weaponControllersGroup4.Length > 0 && firingModeGroup4 == FiringMode.Chain)
            index4 = System.Array.IndexOf(weaponControllersAll, weaponControllersGroup4[indexWeaponGroup4]);

        if (weaponControllersGroup5.Length > 0 && firingModeGroup5 == FiringMode.Chain)
            index5 = System.Array.IndexOf(weaponControllersAll, weaponControllersGroup5[indexWeaponGroup5]);

        if (weaponControllersGroup6.Length > 0 && firingModeGroup6 == FiringMode.Chain)
            index6 = System.Array.IndexOf(weaponControllersAll, weaponControllersGroup6[indexWeaponGroup6]);

        PlayerHUD.Instance.WeaponsManagerUI.UpdateChainFireSelection(index1, index2, index3, index4, index5, index6);
    }

    public void CycleWeaponAmmo(int weaponIndex)
    {
        if (!weaponControllersAll[weaponIndex].IsDestroyed && weaponControllersAll[weaponIndex] is ProjectileWeaponController)
        {
            ProjectileWeaponController projectileWeaponController = weaponControllersAll[weaponIndex] as ProjectileWeaponController;

            if (projectileWeaponController.ProjectileWeaponDefinition.RequiresAmmo)
            {
                projectileWeaponController.CycleNextAmmoPool();
            }
        }
    }

    public void CycleEquipmentMode(int equipmentIndex)
    {
        EquipmentController equipmentController = equipmentControllers[equipmentIndex];

        if (!equipmentController.IsDestroyed && equipmentController.CycleMode())
        {
            mechData.RecalculateStats();

            UpdateMissileDefenseRange();

            PlayerHUD.Instance.WeaponsManagerUI.SetEquipmentStateName(equipmentIndex, equipmentController.CurrentMode.GetDisplayName());
        }
    }

    public void CycleFiringMode(int index)
    {
        switch (index)
        {
            case 0:
                {
                    firingModeGroup1 = firingModeGroup1.Next();
                    break;
                }
            case 1:
                {
                    firingModeGroup2 = firingModeGroup2.Next();
                    break;
                }
            case 2:
                {
                    firingModeGroup3 = firingModeGroup3.Next();
                    break;
                }
            case 3:
                {
                    firingModeGroup4 = firingModeGroup4.Next();
                    break;
                }
            case 4:
                {
                    firingModeGroup5 = firingModeGroup5.Next();
                    break;
                }
            case 5:
                {
                    firingModeGroup6 = firingModeGroup6.Next();
                    break;
                }
        }

        UpdateChainFireUI();

        SetFiringModeText();
    }

    void SetFiringModeText()
    {
        string firingMode1Text = StaticHelper.GetFiringModeDisplay(firingModeGroup1).ToUpper();
        string firingMode2Text = StaticHelper.GetFiringModeDisplay(firingModeGroup2).ToUpper();
        string firingMode3Text = StaticHelper.GetFiringModeDisplay(firingModeGroup3).ToUpper();
        string firingMode4Text = StaticHelper.GetFiringModeDisplay(firingModeGroup4).ToUpper();
        string firingMode5Text = StaticHelper.GetFiringModeDisplay(firingModeGroup5).ToUpper();
        string firingMode6Text = StaticHelper.GetFiringModeDisplay(firingModeGroup6).ToUpper();

        PlayerHUD.Instance.WeaponsManagerUI.SetFiringModeText(firingMode1Text, firingMode2Text, firingMode3Text, firingMode4Text, firingMode5Text, firingMode6Text);
    }

    Color WeaponGroupActiveColor(WeaponController[] weaponGroup)
    {
        bool canFire = false;
        bool inRange = false;

        if (distanceTargeting > 0.0f)
        {
            for (int i = 0; i < weaponGroup.Length; i++)
            {
                WeaponController weaponController = weaponGroup[i];

                if (weaponController.CanFire)
                {
                    canFire = true;

                    if (weaponController.InEffectiveRange(distanceTargeting))
                    {
                        inRange = true;
                        break;
                    }
                }
            }
        }
        else
        {
            for (int i = 0; i < weaponGroup.Length; i++)
            {
                WeaponController weaponController = weaponGroup[i];

                if (weaponController.CanFire)
                {
                    canFire = true;
                    break;
                }
            }
        }

        if (inRange)
        {
            return Color.green;
        }
        else if (canFire)
        {
            return new Color(0.0f, 0.75f, 0.0f, 1.0f);
        }
        else
        {
            return Color.grey;
        }
    }

    float WeaponGroupJamming(WeaponController[] weaponGroup)
    {
        float jammingMax = 0.0f;

        for (int i = 0; i < weaponGroup.Length; i++)
        {
            float jammig = weaponGroup[i].Jamming;

            if (jammig > jammingMax)
                jammingMax = jammig;
        }

        return jammingMax;
    }

    public void SetNavPoint(int navPointIndex, bool playSound)
    {
        Transform[] navPoints = MissionManager.Instance.NavigationPoints;

        if (navPointIndex > -1 && navPointIndex < navPoints.Length)
        {
            currentNavigationPoint = navPoints[navPointIndex];
            PlayerHUD.Instance.navPointName = StaticHelper.GetNavPointName(navPointIndex);
        }
        else
        {
            Debug.LogError("Error: Invalid nav point index");
        }

        if (playSound)
        {
            AudioManager.Instance.PlayClip(mechMetaController.AudioSourceSystems, ResourceManager.Instance.GetAudioClip("CycleNavPoint"), false, false);
        }
    }

    public void AddCameraShake(Vector3 shakeValue)
    {
        CameraController.Instance.AddShakeRotation(shakeValue * mechData.MechChassis.CameraShakeModifier);
    }

    public override void GetKill(UnitController killTarget)
    {
        if (killTarget.Team != TeamType.Enemy)
            return;

        mechData.GetKill(killTarget.UnitData);
    }

    public void SetCockpitVisibility(bool state)
    {
        cockpitController.gameObject.SetActive(state);
    }

    public void AddMissileWarning()
    {
        missileWarningTimer = Time.time + 0.1f;
    }

    void PlayReactorStartupSound()
    {
        AudioManager.Instance.PlayClip(mechMetaController.AudioSourcePower, ResourceManager.Instance.GetAudioClip("ReactorStartup"), false, false);
    }

    void PlayReactorShuttingDownSound()
    {
        AudioManager.Instance.PlayClip(mechMetaController.AudioSourcePower, ResourceManager.Instance.GetAudioClip("ReactorShutdown"), false, false);
    }

    void PlayLockingOnClip()
    {
        if (mechMetaController.AudioSourceLockOnSystem.clip != lockingOnClip)
        {
            mechMetaController.AudioSourceLockOnSystem.clip = lockingOnClip;
        }

        if (!mechMetaController.AudioSourceLockOnSystem.isPlaying)
        {
            mechMetaController.AudioSourceLockOnSystem.Play();
        }
    }

    void PlayLockedOnClip()
    {
        if (mechMetaController.AudioSourceLockOnSystem.clip != lockedOnClip)
        {
            mechMetaController.AudioSourceLockOnSystem.clip = lockedOnClip;
        }

        if (!mechMetaController.AudioSourceLockOnSystem.isPlaying)
        {
            mechMetaController.AudioSourceLockOnSystem.Play();
        }
    }

    void StopPlayingLockOnSystem()
    {
        if (mechMetaController.AudioSourceLockOnSystem.isPlaying)
        {
            mechMetaController.AudioSourceLockOnSystem.Stop();
        }
    }

    public override void PlayFootStep()
    {
        base.PlayFootStep();

        AddCockpitShakeVertical(0.3f);
    }

    public override void FootStepLeft()
    {
        PlayFootStep();

        AddCockpitShakeHorizontal(0.4f);
        AddCockpitShakeRotation(-0.4f);
    }

    public override void FootStepRight()
    {
        PlayFootStep();

        AddCockpitShakeHorizontal(-0.4f);
        AddCockpitShakeRotation(0.4f);
    }

    public void PlayObjectiveUpdated()
    {
        mechMetaController.AudioSourceSystems.PlayOneShot(ResourceManager.Instance.GetAudioClip("ObjectiveUpdated"));
    }

    protected override void CheckFallDamage()
    {
        if (characterControllerMovementLast.y < -8f && !Cheats.godMode)
        {
            float fallDamage = -(characterControllerMovementLast.y + 8);

            mechData.TakeDamageInternalLegLeft(fallDamage);
            mechData.TakeDamageInternalLegRight(fallDamage);
        }
    }


    IEnumerator CalculateVelocity()
    {
        while (enabled)
        {
            // Position at frame start
            previousPosition = transform.position;
            // Wait till it the end of the frame
            yield return waitForEndOfFrame;
            // Calculate velocity: Velocity = DeltaPosition / DeltaTime
            movementSpeed = ((previousPosition - transform.position) / Time.deltaTime).magnitude * 36f;
        }
    }
}
