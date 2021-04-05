using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.Utilities;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }

    public bool useMouse;

    public bool useAuxiliary;

    public bool useThrottleDecay = true;

    public float mouseSensitivity = 1.0f;

    public float auxiliarySensitivity = 1.0f;

    [Header("Axis Groups")]

    public AxisGroup torsoPitch;

    public AxisGroup torsoYaw;

    public AxisGroup turn;

    public AxisGroup throttle;

    [Header("Input Groups")]

    public InputGroup fireGroup1;

    public InputGroup fireGroup2;

    public InputGroup fireGroup3;

    public InputGroup fireGroup4;

    public InputGroup fireGroup5;

    public InputGroup fireGroup6;

    public InputGroup power;

    public InputGroup shutdownOverride;

    public InputGroup jumpJet;

    public InputGroup externalCamera;

    public InputGroup nextNavPoint;

    public InputGroup flushCoolant;

    public InputGroup nextEnemyRadar;

    public InputGroup toggleZoom;

    public InputGroup targetUnderReticle;

    public InputGroup lightAmplification;

    public InputGroup setThrottle0;

    public InputGroup setThrottle1;

    public InputGroup setThrottle2;

    public InputGroup setThrottle3;

    public InputGroup setThrottle4;

    public InputGroup setThrottle5;

    public InputGroup setThrottle6;

    public InputGroup setThrottle7;

    public InputGroup setThrottle8;

    public InputGroup setThrottle9;

    public InputGroup objectives;

    public InputGroup alignLegsToTorso;

    public InputGroup command1;

    public InputGroup command2;

    public InputGroup command3;

    public InputGroup command4;

    public InputGroup command5;

    public InputGroup command6;

    public InputGroup command7;

    public InputGroup weaponMenuUp;

    public InputGroup weaponMenuDown;

    public InputGroup weaponMenuLeft;

    public InputGroup weaponMenuRight;

    public InputGroup weaponMenuSelect;

    Mouse currentMouse;

    public InputDevice currentAuxiliaryDevice;

    public InputControl[] mouseControls = new InputControl[0];

    public InputControl[] auxiliaryControls = new InputControl[0];

    public ControlsConfig ControlsConfig
    {
        get
        {
            string auxiliaryDeviceName = "";

            if (currentAuxiliaryDevice != null)
            {
                auxiliaryDeviceName = currentAuxiliaryDevice.displayName;
            }

            return new ControlsConfig()
            {
                AuxiliaryDeviceName = auxiliaryDeviceName,
                UseMouse = useMouse,
                UseAuxiliaryDevice = useAuxiliary,
                MouseSensitivity = mouseSensitivity,
                AuxiliarySensitivity = auxiliarySensitivity,
                UseThrottleDecay = useThrottleDecay,
                TorsoPitch = torsoPitch.AxisGroupSave,
                TorsoYaw = torsoYaw.AxisGroupSave,
                Turn = turn.AxisGroupSave,
                Throttle = throttle.AxisGroupSave,
                FireGroup1 = fireGroup1.ButtonGroupSave,
                FireGroup2 = fireGroup2.ButtonGroupSave,
                FireGroup3 = fireGroup3.ButtonGroupSave,
                FireGroup4 = fireGroup4.ButtonGroupSave,
                FireGroup5 = fireGroup5.ButtonGroupSave,
                FireGroup6 = fireGroup6.ButtonGroupSave,
                Power = power.ButtonGroupSave,
                ShutdownOverride = shutdownOverride.ButtonGroupSave,
                JumpJet = jumpJet.ButtonGroupSave,
                ExternalCamera = externalCamera.ButtonGroupSave,
                NextNavPoint = nextNavPoint.ButtonGroupSave,
                FlushCoolant = flushCoolant.ButtonGroupSave,
                NextEnemyRadar = nextEnemyRadar.ButtonGroupSave,
                ToggleZoom = toggleZoom.ButtonGroupSave,
                TargetUnderReticle = targetUnderReticle.ButtonGroupSave,
                LightAmplification = lightAmplification.ButtonGroupSave,
                SetThrottle0 = setThrottle0.ButtonGroupSave,
                SetThrottle1 = setThrottle1.ButtonGroupSave,
                SetThrottle2 = setThrottle2.ButtonGroupSave,
                SetThrottle3 = setThrottle3.ButtonGroupSave,
                SetThrottle4 = setThrottle4.ButtonGroupSave,
                SetThrottle5 = setThrottle5.ButtonGroupSave,
                SetThrottle6 = setThrottle6.ButtonGroupSave,
                SetThrottle7 = setThrottle7.ButtonGroupSave,
                SetThrottle8 = setThrottle8.ButtonGroupSave,
                SetThrottle9 = setThrottle9.ButtonGroupSave,
                Objectives = objectives.ButtonGroupSave,
                AlignLegsToTorso = alignLegsToTorso.ButtonGroupSave,
                Command1 = command1.ButtonGroupSave,
                Command2 = command2.ButtonGroupSave,
                Command3 = command3.ButtonGroupSave,
                Command4 = command4.ButtonGroupSave,
                Command5 = command5.ButtonGroupSave,
                Command6 = command6.ButtonGroupSave,
                Command7 = command7.ButtonGroupSave,
                WeaponMenuUp = weaponMenuUp.ButtonGroupSave,
                WeaponMenuDown = weaponMenuDown.ButtonGroupSave,
                WeaponMenuLeft = weaponMenuLeft.ButtonGroupSave,
                WeaponMenuRight = weaponMenuRight.ButtonGroupSave,
                WeaponMenuSelect = weaponMenuSelect.ButtonGroupSave,
            };
        }
    }

    public string AuxiliaryDeviceDisplayName
    {
        get
        {
            if (HasAuxiliaryDevice)
            {
                return currentAuxiliaryDevice.displayName.ToUpper();
            }

            return "NONE";
        }
    }

    public bool HasAuxiliaryDevice { get => currentAuxiliaryDevice != null; }

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

        DontDestroyOnLoad(gameObject);

        if (Keyboard.current == null)
        {
            Debug.LogError("Error: Keyboard not detected");
        }

        currentMouse = Mouse.current;

        if (currentMouse != null)
        {
            mouseControls = currentMouse.allControls.ToArray();
        }
        else
        {
            Debug.LogError("Error: Mouse not detected");
        }


    }

    public void LoadConfiguration(ControlsConfig controlsConfig)
    {
        SetAuxiliaryDevice(controlsConfig.AuxiliaryDeviceName);

        useMouse = controlsConfig.UseMouse;
        useAuxiliary = controlsConfig.UseAuxiliaryDevice;
        mouseSensitivity = controlsConfig.MouseSensitivity;
        auxiliarySensitivity = controlsConfig.AuxiliarySensitivity;
        useThrottleDecay = controlsConfig.UseThrottleDecay;

        torsoPitch.LoadAxisGroupSave(controlsConfig.TorsoPitch);
        torsoYaw.LoadAxisGroupSave(controlsConfig.TorsoYaw);
        turn.LoadAxisGroupSave(controlsConfig.Turn);
        throttle.LoadAxisGroupSave(controlsConfig.Throttle);

        fireGroup1.LoadButtonGroupSave(controlsConfig.FireGroup1);
        fireGroup2.LoadButtonGroupSave(controlsConfig.FireGroup2);
        fireGroup3.LoadButtonGroupSave(controlsConfig.FireGroup3);
        fireGroup4.LoadButtonGroupSave(controlsConfig.FireGroup4);
        fireGroup5.LoadButtonGroupSave(controlsConfig.FireGroup5);
        fireGroup6.LoadButtonGroupSave(controlsConfig.FireGroup6);
        power.LoadButtonGroupSave(controlsConfig.Power);
        shutdownOverride.LoadButtonGroupSave(controlsConfig.ShutdownOverride);
        jumpJet.LoadButtonGroupSave(controlsConfig.JumpJet);
        externalCamera.LoadButtonGroupSave(controlsConfig.ExternalCamera);
        nextNavPoint.LoadButtonGroupSave(controlsConfig.NextNavPoint);
        flushCoolant.LoadButtonGroupSave(controlsConfig.FlushCoolant);
        nextEnemyRadar.LoadButtonGroupSave(controlsConfig.NextEnemyRadar);
        toggleZoom.LoadButtonGroupSave(controlsConfig.ToggleZoom);
        targetUnderReticle.LoadButtonGroupSave(controlsConfig.TargetUnderReticle);
        lightAmplification.LoadButtonGroupSave(controlsConfig.LightAmplification);
        setThrottle0.LoadButtonGroupSave(controlsConfig.SetThrottle0);
        setThrottle1.LoadButtonGroupSave(controlsConfig.SetThrottle1);
        setThrottle2.LoadButtonGroupSave(controlsConfig.SetThrottle2);
        setThrottle3.LoadButtonGroupSave(controlsConfig.SetThrottle3);
        setThrottle4.LoadButtonGroupSave(controlsConfig.SetThrottle4);
        setThrottle5.LoadButtonGroupSave(controlsConfig.SetThrottle5);
        setThrottle6.LoadButtonGroupSave(controlsConfig.SetThrottle6);
        setThrottle7.LoadButtonGroupSave(controlsConfig.SetThrottle7);
        setThrottle8.LoadButtonGroupSave(controlsConfig.SetThrottle8);
        setThrottle9.LoadButtonGroupSave(controlsConfig.SetThrottle9);
        objectives.LoadButtonGroupSave(controlsConfig.Objectives);
        alignLegsToTorso.LoadButtonGroupSave(controlsConfig.AlignLegsToTorso);
        command1.LoadButtonGroupSave(controlsConfig.Command1);
        command2.LoadButtonGroupSave(controlsConfig.Command2);
        command3.LoadButtonGroupSave(controlsConfig.Command3);
        command4.LoadButtonGroupSave(controlsConfig.Command4);
        command5.LoadButtonGroupSave(controlsConfig.Command5);
        command6.LoadButtonGroupSave(controlsConfig.Command6);
        command7.LoadButtonGroupSave(controlsConfig.Command7);
        weaponMenuUp.LoadButtonGroupSave(controlsConfig.WeaponMenuUp);
        weaponMenuDown.LoadButtonGroupSave(controlsConfig.WeaponMenuDown);
        weaponMenuLeft.LoadButtonGroupSave(controlsConfig.WeaponMenuLeft);
        weaponMenuRight.LoadButtonGroupSave(controlsConfig.WeaponMenuRight);
        weaponMenuSelect.LoadButtonGroupSave(controlsConfig.WeaponMenuSelect);
    }

    public void SetAuxiliaryDevice(InputDevice inputDevice)
    {
        currentAuxiliaryDevice = inputDevice;

        if (currentAuxiliaryDevice != null)
        {
            auxiliaryControls = currentAuxiliaryDevice.allControls.ToArray();
        }
    }

    public void SetAuxiliaryDevice(string displayName)
    {
        ReadOnlyArray<InputDevice> devices = InputSystem.devices;

        for (int i = 0; i < devices.Count; i++)
        {
            InputDevice inputDevice = devices[i];

            if (inputDevice.displayName == displayName)
            {
                SetAuxiliaryDevice(inputDevice);
            }
        }
    }

    [System.Serializable]
    public class InputGroup
    {
        public Key key = Key.None;

        public int mouseButton = -1;

        public int auxiliaryButton = -1;

        public bool auxiliaryInverted = false;

        float auxiliaryChangeTimer = 0.0f;

        public ControlsConfig.ButtonGroup ButtonGroupSave
        {
            get
            {
                return new ControlsConfig.ButtonGroup()
                {
                    Key = key,
                    Mouse = mouseButton,
                    Auxiliary = auxiliaryButton,
                    AuxiliaryInverted = auxiliaryInverted,
                };
            }
        }

        bool ValidMouseButton { get => mouseButton != -1 && mouseButton < Instance.mouseControls.Length; }

        bool ValidAuxiliaryButton { get => auxiliaryButton != -1 && auxiliaryButton < Instance.auxiliaryControls.Length; }

        bool CanAuxiliaryChange { get => Time.time > auxiliaryChangeTimer; }

        public string KeyDisplay { get => key.ToString(); }
 
        public string MouseDisplay { get => StaticHelper.GetMouseButtonName(mouseButton); }

        public string AuxiliaryDisplay
        {
            get
            {
                if (Instance.currentAuxiliaryDevice != null && ValidAuxiliaryButton)
                {
                    InputControl inputControl = Instance.auxiliaryControls[auxiliaryButton];

                    if (inputControl != null)
                    {
                        if (inputControl is ButtonControl)
                        {
                            return inputControl.displayName;
                        }

                        if (auxiliaryInverted)
                        {
                            return "Inverted " + inputControl.displayName;
                        }

                        return inputControl.displayName;
                    }
                }

                return "None";
            }
        }

        public bool IsPressed()
        {
            if (!Application.isFocused || !CommandTerminal.Terminal.instance.IsClosed)
            {
                return false;
            }

            if (key != Key.None && Keyboard.current != null && Keyboard.current[key].isPressed)
            {
                return true;
            }

            if (Instance.useMouse && Instance.currentMouse != null && ValidMouseButton)
            {
                ButtonControl buttonControl = Instance.mouseControls[mouseButton] as ButtonControl;

                if (buttonControl != null && buttonControl.isPressed)
                {
                    return true;
                }
            }

            if (Instance.useAuxiliary && Instance.HasAuxiliaryDevice && ValidAuxiliaryButton)
            {
                InputControl inputControl = Instance.auxiliaryControls[auxiliaryButton];

                if (inputControl is ButtonControl)
                {
                    ButtonControl buttonControl = inputControl as ButtonControl;

                    if (buttonControl != null && buttonControl.isPressed)
                    {
                        return true;
                    }
                }
                else
                {
                    AxisControl axisControl = inputControl as AxisControl;

                    float inputValue = axisControl.ReadValue();
                    bool pressed = (!auxiliaryInverted && inputValue > 0.5f) || (auxiliaryInverted && inputValue < -0.5f);

                    if (axisControl != null && pressed)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public bool PressedThisFrame()
        {
            if (!Application.isFocused || !CommandTerminal.Terminal.instance.IsClosed)
            {
                return false;
            }

            if (key != Key.None && Keyboard.current != null && Keyboard.current[key].wasPressedThisFrame)
            {
                return true;
            }

            if (Instance.useMouse && mouseButton != -1 && Mouse.current != null)
            {
                ButtonControl buttonControl = Instance.mouseControls[mouseButton] as ButtonControl;

                if (buttonControl != null && buttonControl.wasPressedThisFrame)
                {
                    return true;
                }
            }

            if (Instance.useAuxiliary && Instance.HasAuxiliaryDevice && ValidAuxiliaryButton)
            {
                InputControl inputControl = Instance.auxiliaryControls[auxiliaryButton];

                if (inputControl is ButtonControl)
                {
                    ButtonControl buttonControl = inputControl as ButtonControl;

                    if (buttonControl != null && buttonControl.wasPressedThisFrame)
                    {
                        return true;
                    }
                }
                else if (CanAuxiliaryChange)
                {
                    AxisControl axisControl = inputControl as AxisControl;

                    if (axisControl != null)
                    {
                        float inputValue = axisControl.ReadValue();
                        float inputLastValue = axisControl.ReadValueFromPreviousFrame();

                        bool pressed = (!auxiliaryInverted && inputValue > 0.5f) || (auxiliaryInverted && inputValue < -0.5f);
                        bool pressedLastFrame = (!auxiliaryInverted && inputLastValue > 0.5f) || (auxiliaryInverted && inputLastValue < -0.5f);

                        if (pressed && !pressedLastFrame)
                        {
                            auxiliaryChangeTimer = Time.time + 0.1f;
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        public bool ReleasedThisFrame()
        {
            if (!Application.isFocused || !CommandTerminal.Terminal.instance.IsClosed)
            {
                return false;
            }

            if (key != Key.None && Keyboard.current != null && Keyboard.current[key].wasReleasedThisFrame)
            {
                return true;
            }

            if (Instance.useMouse && mouseButton != -1 && Mouse.current != null)
            {
                ButtonControl buttonControl = Instance.mouseControls[mouseButton] as ButtonControl;

                if (buttonControl != null && buttonControl.wasReleasedThisFrame)
                {
                    return true;
                }
            }

            if (Instance.useAuxiliary && Instance.HasAuxiliaryDevice && ValidAuxiliaryButton)
            {
                InputControl inputControl = Instance.auxiliaryControls[auxiliaryButton];

                if (inputControl is ButtonControl)
                {
                    ButtonControl buttonControl = inputControl as ButtonControl;

                    if (buttonControl != null && buttonControl.wasReleasedThisFrame)
                    {
                        return true;
                    }
                }
                else if(CanAuxiliaryChange)
                {
                    AxisControl axisControl = Instance.auxiliaryControls[auxiliaryButton] as AxisControl;

                    if (axisControl != null)
                    {
                        float inputValue = axisControl.ReadValue();
                        float inputLastValue = axisControl.ReadValueFromPreviousFrame();

                        bool pressed = (!auxiliaryInverted && inputValue > 0.5f) || (auxiliaryInverted && inputValue < -0.5f);
                        bool pressedLastFrame = (!auxiliaryInverted && inputLastValue > 0.5f) || (auxiliaryInverted && inputLastValue < -0.5f);

                        if (!pressed && pressedLastFrame)
                        {
                            auxiliaryChangeTimer = Time.time + 0.1f;
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        public void LoadButtonGroupSave(ControlsConfig.ButtonGroup buttonGroup)
        {
            key = buttonGroup.Key;
            mouseButton = buttonGroup.Mouse;
            auxiliaryButton = buttonGroup.Auxiliary;
            auxiliaryInverted = buttonGroup.AuxiliaryInverted;
        }
    }

    [System.Serializable]
    public class AxisGroup
    {
        public Key positiveKey = Key.None;

        public Key negativeKey = Key.None;

        public MouseAxis mouseAxis = MouseAxis.None;

        public int auxiliaryPositive = -1;

        public int auxiliaryNegative = -1;

        public ControlsConfig.AxisGroup AxisGroupSave
        {
            get
            {
                return new ControlsConfig.AxisGroup
                {
                    KeyPos = positiveKey,
                    KeyNeg = negativeKey,
                    MouseAxis = mouseAxis,
                    AuxiliaryPos = auxiliaryPositive,
                    AuxiliaryNeg = auxiliaryNegative,
                };
            }
        }

        public bool ValidAuxiliaryPositive { get => auxiliaryPositive > -1 && auxiliaryPositive < Instance.auxiliaryControls.Length; }

        public bool ValidAuxiliaryNegative { get => auxiliaryNegative > -1 && auxiliaryNegative < Instance.auxiliaryControls.Length; }

        public string AuxiliaryPositiveDisplay
        {
            get
            {
                if (Instance.HasAuxiliaryDevice && ValidAuxiliaryPositive)
                {
                    return Instance.auxiliaryControls[auxiliaryPositive].displayName;
                }

                return "None";
            }
        }

        public string AuxiliaryNegativeDisplay
        {
            get
            {
                if (Instance.HasAuxiliaryDevice && ValidAuxiliaryNegative)
                {
                    return Instance.auxiliaryControls[auxiliaryNegative].displayName;
                }

                return "None";
            }
        }

        public float GetValue()
        {
            if (!Application.isFocused || !CommandTerminal.Terminal.instance.IsClosed)
            {
                return 0.0f;
            }

            if (positiveKey != Key.None && Keyboard.current != null && Keyboard.current[positiveKey].isPressed)
            {
                return 1.0f;
            }

            if (negativeKey != Key.None && Keyboard.current != null && Keyboard.current[negativeKey].isPressed)
            {
                return -1.0f;
            }

            if (Instance.useAuxiliary && Instance.HasAuxiliaryDevice)
            {
                if (ValidAuxiliaryPositive)
                {
                    AxisControl axisControl = Instance.auxiliaryControls[auxiliaryPositive] as AxisControl;

                    float value = axisControl.ReadValue();

                    if (Mathf.Abs(value) > 0.1f)
                    {
                        return value;
                    }
                }

                if (ValidAuxiliaryNegative)
                {
                    AxisControl axisControl = Instance.auxiliaryControls[auxiliaryNegative] as AxisControl;

                    float value = axisControl.ReadValue();

                    if (Mathf.Abs(value) > 0.1f)
                    {
                        return -value;
                    }
                }
            }

            if (Instance.useMouse && Mouse.current != null)
            {
                switch (mouseAxis)
                {
                    case MouseAxis.X:
                        {
                            float value = Input.GetAxis("Mouse X") * Instance.mouseSensitivity;

                            return value;
                        }
                    case MouseAxis.X_Inverted:
                        {
                            float value = Input.GetAxis("Mouse X") * Instance.mouseSensitivity;

                            return -value;
                        }
                    case MouseAxis.Y:
                        {
                            float value = Input.GetAxis("Mouse Y") * Instance.mouseSensitivity;

                            return value;
                        }
                    case MouseAxis.Y_Inverted:
                        {
                            float value = Input.GetAxis("Mouse Y") * Instance.mouseSensitivity;

                            return -value;
                        }
                    case MouseAxis.Scroll:
                        {
                            return Input.mouseScrollDelta.y;
                        }
                    case MouseAxis.Scroll_Inverted:
                        {
                            return -Input.mouseScrollDelta.y;
                        }
                }
            }

            return 0.0f;
        }

        public void LoadAxisGroupSave(ControlsConfig.AxisGroup axisGroup)
        {
            positiveKey = axisGroup.KeyPos;
            negativeKey = axisGroup.KeyNeg;
            mouseAxis = axisGroup.MouseAxis;
            auxiliaryPositive = axisGroup.AuxiliaryPos;
            auxiliaryNegative = axisGroup.AuxiliaryNeg;
        }
    }
}