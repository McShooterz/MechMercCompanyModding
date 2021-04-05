using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.Utilities;

public class ControlOptionsUI : MonoBehaviour
{
    #region Variables
    [SerializeField]
    AuxiliaryDeviceSelectWindow auxiliaryDeviceSelectWindow;

    [SerializeField]
    GameObject rebindKeyWindow;

    [SerializeField]
    GameObject rebindMouseWindow;

    [SerializeField]
    GameObject rebindAuxiliaryWindow;

    [SerializeField]
    GameObject rebindKeyPosWindow;

    [SerializeField]
    GameObject rebindKeyNegWindow;

    [SerializeField]
    GameObject rebindMouseAxisWindow;

    [SerializeField]
    GameObject rebindAuxiliaryPositiveWindow;

    [SerializeField]
    GameObject rebindAuxiliaryNegativeWindow;

    [SerializeField]
    Slider mouseSensitivitySlider;

    [SerializeField]
    Slider joystickSensitivitySlider;

    [SerializeField]
    Text auxiliaryDeviceNameText;

    [SerializeField]
    Text mouseSensitivityValueText;

    [SerializeField]
    Text joystickSensitivityValueText;

    [SerializeField]
    Toggle mouseUseToggle;

    [SerializeField]
    Toggle joystickUseToggle;

    [SerializeField]
    Toggle throttleDecayToggle;

    [SerializeField]
    AxisInputGroupUI torsoPitch;

    [SerializeField]
    AxisInputGroupUI torsoYaw;

    [SerializeField]
    AxisInputGroupUI turn;

    [SerializeField]
    AxisInputGroupUI throttle;

    [SerializeField]
    ButtonInputGroupUI fireGroup1;

    [SerializeField]
    ButtonInputGroupUI fireGroup2;

    [SerializeField]
    ButtonInputGroupUI fireGroup3;

    [SerializeField]
    ButtonInputGroupUI fireGroup4;

    [SerializeField]
    ButtonInputGroupUI fireGroup5;

    [SerializeField]
    ButtonInputGroupUI fireGroup6;

    [SerializeField]
    ButtonInputGroupUI power;

    [SerializeField]
    ButtonInputGroupUI shutdownOverride;

    [SerializeField]
    ButtonInputGroupUI jumpJet;

    [SerializeField]
    ButtonInputGroupUI externalCamera;

    [SerializeField]
    ButtonInputGroupUI nextNavPoint;

    [SerializeField]
    ButtonInputGroupUI flushCoolant;

    [SerializeField]
    ButtonInputGroupUI nextEnemyRadar;

    [SerializeField]
    ButtonInputGroupUI toggleZoom;

    [SerializeField]
    ButtonInputGroupUI targetUnderReticle;

    [SerializeField]
    ButtonInputGroupUI lightAmplification;

    [SerializeField]
    ButtonInputGroupUI setThrottle0;

    [SerializeField]
    ButtonInputGroupUI setThrottle1;

    [SerializeField]
    ButtonInputGroupUI setThrottle2;

    [SerializeField]
    ButtonInputGroupUI setThrottle3;

    [SerializeField]
    ButtonInputGroupUI setThrottle4;

    [SerializeField]
    ButtonInputGroupUI setThrottle5;

    [SerializeField]
    ButtonInputGroupUI setThrottle6;

    [SerializeField]
    ButtonInputGroupUI setThrottle7;

    [SerializeField]
    ButtonInputGroupUI setThrottle8;

    [SerializeField]
    ButtonInputGroupUI setThrottle9;

    [SerializeField]
    ButtonInputGroupUI objectives;

    [SerializeField]
    ButtonInputGroupUI alignLegsToTorso;

    [SerializeField]
    ButtonInputGroupUI command1;

    [SerializeField]
    ButtonInputGroupUI command2;

    [SerializeField]
    ButtonInputGroupUI command3;

    [SerializeField]
    ButtonInputGroupUI command4;

    [SerializeField]
    ButtonInputGroupUI command5;

    [SerializeField]
    ButtonInputGroupUI command6;

    [SerializeField]
    ButtonInputGroupUI command7;

    [SerializeField]
    ButtonInputGroupUI weaponMenuUp;

    [SerializeField]
    ButtonInputGroupUI weaponMenuDown;

    [SerializeField]
    ButtonInputGroupUI weaponMenuLeft;

    [SerializeField]
    ButtonInputGroupUI weaponMenuRight;

    [SerializeField]
    ButtonInputGroupUI weaponMenuSelect;

    InputManager.InputGroup activeButtonGroup;
    InputManager.AxisGroup activeAxisGroup;

    ButtonInputGroupUI activeButtonInputGroupUI;
    AxisInputGroupUI activeAxisInputGroupUI;

    AxisInputGroupUI[] axisInputGroupUIs;
    ButtonInputGroupUI[] buttonInputGroupUIs;

    Key[] validKey = new Key[] 
    {
        Key.Space,
        Key.Enter,
        Key.Tab,
        Key.Backquote,
        Key.Quote,
        Key.Semicolon,
        Key.Comma,
        Key.Period,
        Key.Slash,
        Key.Backslash,
        Key.LeftBracket,
        Key.RightBracket,
        Key.Minus,
        Key.Equals,
        Key.A,
        Key.B,
        Key.C,
        Key.D,
        Key.E,
        Key.F,
        Key.G,
        Key.H,
        Key.I,
        Key.J,
        Key.K,
        Key.L,
        Key.M,
        Key.N,
        Key.O,
        Key.P,
        Key.Q,
        Key.R,
        Key.S,
        Key.T,
        Key.U,
        Key.V,
        Key.W,
        Key.X,
        Key.Y,
        Key.Z,
        Key.Digit1,
        Key.Digit2,
        Key.Digit3,
        Key.Digit4,
        Key.Digit5,
        Key.Digit6,
        Key.Digit7,
        Key.Digit8,
        Key.Digit9,
        Key.Digit0,
        Key.LeftShift,
        Key.RightShift,
        Key.LeftAlt,
        Key.RightAlt,
        Key.LeftCtrl,
        Key.RightCtrl,
        Key.LeftArrow,
        Key.RightArrow,
        Key.UpArrow,
        Key.DownArrow,
        Key.Backspace,
        Key.PageDown,
        Key.PageUp,
        Key.Home,
        Key.End,
        Key.Insert,
        Key.Delete,
        Key.CapsLock,
        Key.NumLock,
        Key.PrintScreen,
        Key.ScrollLock,
        Key.Pause,
        Key.NumpadEnter,
        Key.NumpadDivide,
        Key.NumpadMultiply,
        Key.NumpadPlus,
        Key.NumpadMinus,
        Key.NumpadPeriod,
        Key.NumpadEquals,
        Key.Numpad0,
        Key.Numpad1,
        Key.Numpad2,
        Key.Numpad3,
        Key.Numpad4,
        Key.Numpad5,
        Key.Numpad6,
        Key.Numpad7,
        Key.Numpad8,
        Key.Numpad9,
        Key.F1,
        Key.F2,
        Key.F3,
        Key.F4,
        Key.F5,
        Key.F6,
        Key.F7,
        Key.F8,
        Key.F9,
        Key.F10,
        Key.F11,
        Key.F12,
    };
    #endregion

    public bool HasKeyBindingsOpen { get => rebindKeyWindow.activeInHierarchy || rebindMouseWindow.activeInHierarchy || rebindAuxiliaryWindow.activeInHierarchy || rebindKeyPosWindow.activeInHierarchy || rebindKeyNegWindow.activeInHierarchy || rebindMouseAxisWindow.activeInHierarchy || rebindAuxiliaryPositiveWindow.activeInHierarchy || rebindAuxiliaryNegativeWindow.activeInHierarchy; }

    private void Awake()
    {
        axisInputGroupUIs = new AxisInputGroupUI[]
        {
            torsoPitch,
            torsoYaw,
            turn,
            throttle,
        };

        buttonInputGroupUIs = new ButtonInputGroupUI[]
        {
            fireGroup1,
            fireGroup2,
            fireGroup3,
            fireGroup4,
            fireGroup5,
            fireGroup6,
            power,
            shutdownOverride,
            jumpJet,
            externalCamera,
            nextNavPoint,
            flushCoolant,
            nextEnemyRadar,
            toggleZoom,
            targetUnderReticle,
            lightAmplification,
            setThrottle0,
            setThrottle1,
            setThrottle2,
            setThrottle3,
            setThrottle4,
            setThrottle5,
            setThrottle6,
            setThrottle7,
            setThrottle8,
            setThrottle9,
            objectives,
            alignLegsToTorso,
            command1,
            command2,
            command3,
            command4,
            command5,
            command6,
            command7,
            weaponMenuUp,
            weaponMenuDown,
            weaponMenuLeft,
            weaponMenuRight,
            weaponMenuSelect,
        };
    }

    void Start()
    {
        rebindKeyWindow.SetActive(false);
        rebindMouseWindow.SetActive(false);
        rebindAuxiliaryWindow.SetActive(false);
        rebindKeyPosWindow.SetActive(false);
        rebindKeyNegWindow.SetActive(false);
        rebindMouseAxisWindow.SetActive(false);
        rebindAuxiliaryPositiveWindow.SetActive(false);
        rebindAuxiliaryNegativeWindow.SetActive(false);
        auxiliaryDeviceSelectWindow.gameObject.SetActive(false);
    }

    void Update()
    {
        if (rebindKeyWindow.activeInHierarchy)
        {
            if (Keyboard.current != null)
            {
                if (Keyboard.current.escapeKey.wasPressedThisFrame)
                {
                    rebindKeyWindow.SetActive(false);
                }
                else
                {
                    foreach(Key key in validKey)
                    {
                        if (Keyboard.current[key].wasPressedThisFrame)
                        {
                            activeButtonGroup.key = key;
                            activeButtonInputGroupUI.SetText();
                            rebindKeyWindow.SetActive(false);
                            break;
                        }
                    }
                }
            }
            else
            {
                rebindKeyWindow.SetActive(false);
            }
        }

        if (rebindMouseWindow.activeInHierarchy)
        {
            if (Mouse.current != null)
            {
                if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
                {
                    rebindMouseWindow.SetActive(false);
                }
                else
                {
                    for (int i = 0; i < Mouse.current.allControls.Count; i++)
                    {
                        ButtonControl buttonControl = Mouse.current.allControls[i] as ButtonControl;

                        if (buttonControl != null && buttonControl.wasPressedThisFrame)
                        {
                            activeButtonGroup.mouseButton = i;
                            activeButtonInputGroupUI.SetText();
                            rebindMouseWindow.SetActive(false);
                            break;
                        }
                    }
                }
            }
            else
            {
                rebindMouseWindow.SetActive(false);
            }
        }

        if (rebindAuxiliaryWindow.activeInHierarchy)
        {
            if (InputManager.Instance.HasAuxiliaryDevice)
            {
                if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
                {
                    rebindAuxiliaryWindow.SetActive(false);
                }
                else
                {
                    ReadOnlyArray<InputControl> allControls = InputManager.Instance.auxiliaryControls;

                    for (int i = 0; i < allControls.Count; i++)
                    {
                        InputControl inputControl = allControls[i] as InputControl;

                        if (inputControl is ButtonControl)
                        {
                            ButtonControl buttonControl = inputControl as ButtonControl;

                            if (buttonControl != null && buttonControl.isPressed)
                            {
                                activeButtonGroup.auxiliaryButton = i;
                                activeButtonGroup.auxiliaryInverted = false;
                                activeButtonInputGroupUI.SetText();
                                rebindAuxiliaryWindow.SetActive(false);
                                break;
                            }
                        }
                        else
                        {
                            AxisControl axisControl = allControls[i] as AxisControl;

                            if (axisControl != null && Mathf.Abs(axisControl.ReadValue()) > 0.5f)
                            {
                                if (axisControl.ReadValue() > 0.5f)
                                {
                                    activeButtonGroup.auxiliaryButton = i;
                                    activeButtonGroup.auxiliaryInverted = false;
                                    activeButtonInputGroupUI.SetText();
                                    rebindAuxiliaryWindow.SetActive(false);
                                    break;
                                }
                                else if (axisControl.ReadValue() < -0.5f)
                                {
                                    activeButtonGroup.auxiliaryButton = i;
                                    activeButtonGroup.auxiliaryInverted = true;
                                    activeButtonInputGroupUI.SetText();
                                    rebindAuxiliaryWindow.SetActive(false);
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                rebindAuxiliaryWindow.SetActive(false);
            }
        }

        if (rebindKeyPosWindow.activeInHierarchy)
        {
            if (Keyboard.current != null)
            {
                if (Keyboard.current.escapeKey.wasPressedThisFrame)
                {
                    rebindKeyPosWindow.SetActive(false);
                }
                else
                {
                    foreach (Key key in validKey)
                    {
                        if (Keyboard.current[key].wasPressedThisFrame)
                        {
                            activeAxisGroup.positiveKey = key;
                            activeAxisInputGroupUI.SetText();
                            rebindKeyPosWindow.SetActive(false);
                            break;
                        }
                    }
                }
            }
            else
            {
                rebindKeyPosWindow.SetActive(false);
            }
        }

        if (rebindKeyNegWindow.activeInHierarchy)
        {
            if (Keyboard.current != null)
            {
                if (Keyboard.current.escapeKey.wasPressedThisFrame)
                {
                    rebindKeyNegWindow.SetActive(false);
                }
                else
                {
                    foreach (Key key in validKey)
                    {
                        if (Keyboard.current[key].wasPressedThisFrame)
                        {
                            activeAxisGroup.negativeKey = key;
                            activeAxisInputGroupUI.SetText();
                            rebindKeyNegWindow.SetActive(false);
                            break;
                        }
                    }
                }
            }
            else
            {
                rebindKeyNegWindow.SetActive(false);
            }
        }

        if (rebindMouseAxisWindow.activeInHierarchy)
        {
            if (Mouse.current != null)
            {
                if (Keyboard.current.escapeKey.wasPressedThisFrame)
                {
                    rebindMouseAxisWindow.SetActive(false);
                }
                else
                {
                    if (Input.GetAxis("Mouse X") > 0.5f)
                    {
                        activeAxisGroup.mouseAxis = MouseAxis.X;
                        activeAxisInputGroupUI.SetText();
                        rebindMouseAxisWindow.SetActive(false);
                    }
                    else if (Input.GetAxis("Mouse X") < -0.5f)
                    {
                        activeAxisGroup.mouseAxis = MouseAxis.X_Inverted;
                        activeAxisInputGroupUI.SetText();
                        rebindMouseAxisWindow.SetActive(false);
                    }
                    else if (Input.GetAxis("Mouse Y") > 0.5f)
                    {
                        activeAxisGroup.mouseAxis = MouseAxis.Y;
                        activeAxisInputGroupUI.SetText();
                        rebindMouseAxisWindow.SetActive(false);
                    }
                    else if (Input.GetAxis("Mouse Y") < -0.5f)
                    {
                        activeAxisGroup.mouseAxis = MouseAxis.Y_Inverted;
                        activeAxisInputGroupUI.SetText();
                        rebindMouseAxisWindow.SetActive(false);
                    }
                    else if (Input.mouseScrollDelta.y > 0.5f)
                    {
                        activeAxisGroup.mouseAxis = MouseAxis.Scroll;
                        activeAxisInputGroupUI.SetText();
                        rebindMouseAxisWindow.SetActive(false);
                    }
                    else if (Input.mouseScrollDelta.y < -0.5f)
                    {
                        activeAxisGroup.mouseAxis = MouseAxis.Scroll_Inverted;
                        activeAxisInputGroupUI.SetText();
                        rebindMouseAxisWindow.SetActive(false);
                    }
                }
            }
            else
            {
                rebindMouseAxisWindow.SetActive(false);
            }
        }

        if (rebindAuxiliaryPositiveWindow.activeInHierarchy)
        {
            if (InputManager.Instance.HasAuxiliaryDevice)
            {
                if (Keyboard.current.escapeKey.wasPressedThisFrame)
                {
                    rebindAuxiliaryPositiveWindow.SetActive(false);
                }
                else
                {
                    ReadOnlyArray<InputControl> allControls = InputManager.Instance.auxiliaryControls;

                    for (int i = 0; i < allControls.Count; i++)
                    {
                        AxisControl axisControl = allControls[i] as AxisControl;

                        if (axisControl != null && Mathf.Abs(axisControl.ReadValue()) > 0.5f)
                        {
                            activeAxisGroup.auxiliaryPositive = i;
                            activeAxisInputGroupUI.SetText();
                            rebindAuxiliaryPositiveWindow.SetActive(false);
                        }
                    }
                }
            }
            else
            {
                rebindAuxiliaryPositiveWindow.SetActive(false);
            }
        }

        if (rebindAuxiliaryNegativeWindow.activeInHierarchy)
        {
            if (InputManager.Instance.HasAuxiliaryDevice)
            {
                if (Keyboard.current.escapeKey.wasPressedThisFrame)
                {
                    rebindAuxiliaryNegativeWindow.SetActive(false);
                }
                else
                {
                    ReadOnlyArray<InputControl> allControls = InputManager.Instance.auxiliaryControls;

                    for (int i = 0; i < allControls.Count; i++)
                    {
                        AxisControl axisControl = allControls[i] as AxisControl;

                        if (axisControl != null && Mathf.Abs(axisControl.ReadValue()) > 0.5f)
                        {
                            activeAxisGroup.auxiliaryNegative = i;
                            activeAxisInputGroupUI.SetText();
                            rebindAuxiliaryNegativeWindow.SetActive(false);
                        }
                    }
                }
            }
            else
            {
                rebindAuxiliaryNegativeWindow.SetActive(false);
            }
        }
    }

    void OnEnable()
    {
        auxiliaryDeviceNameText.text = InputManager.Instance.AuxiliaryDeviceDisplayName;

        mouseSensitivitySlider.SetValueWithoutNotify(InputManager.Instance.mouseSensitivity);
        mouseSensitivityValueText.text = InputManager.Instance.mouseSensitivity.ToString("0.0");

        joystickSensitivitySlider.SetValueWithoutNotify(InputManager.Instance.auxiliarySensitivity);
        joystickSensitivityValueText.text = InputManager.Instance.auxiliarySensitivity.ToString("0.0");

        mouseUseToggle.SetIsOnWithoutNotify(InputManager.Instance.useMouse);

        joystickUseToggle.SetIsOnWithoutNotify(InputManager.Instance.useAuxiliary);

        throttleDecayToggle.SetIsOnWithoutNotify(InputManager.Instance.useThrottleDecay);

        torsoPitch.SetAxisGroup(InputManager.Instance.torsoPitch);

        torsoYaw.SetAxisGroup(InputManager.Instance.torsoYaw);

        turn.SetAxisGroup(InputManager.Instance.turn);

        throttle.SetAxisGroup(InputManager.Instance.throttle);

        fireGroup1.SetButtonGroup(InputManager.Instance.fireGroup1);

        fireGroup2.SetButtonGroup(InputManager.Instance.fireGroup2);

        fireGroup3.SetButtonGroup(InputManager.Instance.fireGroup3);

        fireGroup4.SetButtonGroup(InputManager.Instance.fireGroup4);

        fireGroup5.SetButtonGroup(InputManager.Instance.fireGroup5);

        fireGroup6.SetButtonGroup(InputManager.Instance.fireGroup6);

        power.SetButtonGroup(InputManager.Instance.power);

        shutdownOverride.SetButtonGroup(InputManager.Instance.shutdownOverride);

        jumpJet.SetButtonGroup(InputManager.Instance.jumpJet);

        externalCamera.SetButtonGroup(InputManager.Instance.externalCamera);

        nextNavPoint.SetButtonGroup(InputManager.Instance.nextNavPoint);

        flushCoolant.SetButtonGroup(InputManager.Instance.flushCoolant);

        nextEnemyRadar.SetButtonGroup(InputManager.Instance.nextEnemyRadar);

        toggleZoom.SetButtonGroup(InputManager.Instance.toggleZoom);

        targetUnderReticle.SetButtonGroup(InputManager.Instance.targetUnderReticle);

        lightAmplification.SetButtonGroup(InputManager.Instance.lightAmplification);

        setThrottle0.SetButtonGroup(InputManager.Instance.setThrottle0);

        setThrottle1.SetButtonGroup(InputManager.Instance.setThrottle1);

        setThrottle2.SetButtonGroup(InputManager.Instance.setThrottle2);

        setThrottle3.SetButtonGroup(InputManager.Instance.setThrottle3);

        setThrottle4.SetButtonGroup(InputManager.Instance.setThrottle4);

        setThrottle5.SetButtonGroup(InputManager.Instance.setThrottle5);

        setThrottle6.SetButtonGroup(InputManager.Instance.setThrottle6);

        setThrottle7.SetButtonGroup(InputManager.Instance.setThrottle7);

        setThrottle8.SetButtonGroup(InputManager.Instance.setThrottle8);

        setThrottle9.SetButtonGroup(InputManager.Instance.setThrottle9);

        objectives.SetButtonGroup(InputManager.Instance.objectives);

        alignLegsToTorso.SetButtonGroup(InputManager.Instance.alignLegsToTorso);

        command1.SetButtonGroup(InputManager.Instance.command1);

        command2.SetButtonGroup(InputManager.Instance.command2);

        command3.SetButtonGroup(InputManager.Instance.command3);

        command4.SetButtonGroup(InputManager.Instance.command4);

        command5.SetButtonGroup(InputManager.Instance.command5);

        command6.SetButtonGroup(InputManager.Instance.command6);

        command7.SetButtonGroup(InputManager.Instance.command7);

        weaponMenuUp.SetButtonGroup(InputManager.Instance.weaponMenuUp);

        weaponMenuDown.SetButtonGroup(InputManager.Instance.weaponMenuDown);

        weaponMenuLeft.SetButtonGroup(InputManager.Instance.weaponMenuLeft);

        weaponMenuRight.SetButtonGroup(InputManager.Instance.weaponMenuRight);

        weaponMenuSelect.SetButtonGroup(InputManager.Instance.weaponMenuSelect);
    }

    public void RefreshInputs()
    {
        auxiliaryDeviceNameText.text = InputManager.Instance.AuxiliaryDeviceDisplayName;

        for (int i = 0; i < axisInputGroupUIs.Length; i++)
        {
            axisInputGroupUIs[i].SetText();
        }

        for (int i = 0; i < buttonInputGroupUIs.Length; i++)
        {
            buttonInputGroupUIs[i].SetText();
        }
    }

    public void OpenRebindWindowKey(InputManager.InputGroup buttonGroup, ButtonInputGroupUI buttonInputGroupUI)
    {
        rebindKeyWindow.SetActive(true);
        activeButtonGroup = buttonGroup;
        activeButtonInputGroupUI = buttonInputGroupUI;
    }

    public void OpenRebindWindowMouse(InputManager.InputGroup buttonGroup, ButtonInputGroupUI buttonInputGroupUI)
    {
        rebindMouseWindow.SetActive(true);
        activeButtonGroup = buttonGroup;
        activeButtonInputGroupUI = buttonInputGroupUI;
    }

    public void OpenRebindWindowJoystick(InputManager.InputGroup buttonGroup, ButtonInputGroupUI buttonInputGroupUI)
    {
        rebindAuxiliaryWindow.SetActive(true);
        activeButtonGroup = buttonGroup;
        activeButtonInputGroupUI = buttonInputGroupUI;
    }

    public void OpenRebindWindowKeyPos(InputManager.AxisGroup axisGroup, AxisInputGroupUI axisInputGroupUI)
    {
        rebindKeyPosWindow.SetActive(true);
        activeAxisGroup = axisGroup;
        activeAxisInputGroupUI = axisInputGroupUI;
    }

    public void OpenRebindWindowKeyNeg(InputManager.AxisGroup axisGroup, AxisInputGroupUI axisInputGroupUI)
    {
        rebindKeyNegWindow.SetActive(true);
        activeAxisGroup = axisGroup;
        activeAxisInputGroupUI = axisInputGroupUI;
    }

    public void OpenRebindWindowMouseAxis(InputManager.AxisGroup axisGroup, AxisInputGroupUI axisInputGroupUI)
    {
        rebindMouseAxisWindow.SetActive(true);
        activeAxisGroup = axisGroup;
        activeAxisInputGroupUI = axisInputGroupUI;
    }

    public void OpenRebindWindowAuxiliaryPositive(InputManager.AxisGroup axisGroup, AxisInputGroupUI axisInputGroupUI)
    {
        rebindAuxiliaryPositiveWindow.SetActive(true);
        activeAxisGroup = axisGroup;
        activeAxisInputGroupUI = axisInputGroupUI;
    }

    public void OpenRebindWindowAuxiliaryNegative(InputManager.AxisGroup axisGroup, AxisInputGroupUI axisInputGroupUI)
    {
        rebindAuxiliaryNegativeWindow.SetActive(true);
        activeAxisGroup = axisGroup;
        activeAxisInputGroupUI = axisInputGroupUI;
    }

    public void OnChangeMouseSensitivity()
    {
        InputManager.Instance.mouseSensitivity = mouseSensitivitySlider.value;

        mouseSensitivityValueText.text = mouseSensitivitySlider.value.ToString("0.0");
    }

    public void OnChangeJoystickSensitivity()
    {
        InputManager.Instance.auxiliarySensitivity = joystickSensitivitySlider.value;

        joystickSensitivityValueText.text = joystickSensitivitySlider.value.ToString("0.0");
    }

    public void OnChangeUseMouse()
    {
        InputManager.Instance.useMouse = mouseUseToggle.isOn;
    }

    public void OnChangeUseJoystick()
    {
        InputManager.Instance.useAuxiliary = joystickUseToggle.isOn;
    }

    public void OnChangeThrottleDecay()
    {
        InputManager.Instance.useThrottleDecay = throttleDecayToggle.isOn;
    }

    public void ClickSelectAuxiliaryDevice()
    {
        auxiliaryDeviceSelectWindow.gameObject.SetActive(true);

        AudioManager.Instance.PlayButtonClick(0);
    }
}
