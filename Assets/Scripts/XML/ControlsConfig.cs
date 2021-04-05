using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ControlsConfig
{
    public string AuxiliaryDeviceName = "";

    public bool UseMouse = true;

    public bool UseAuxiliaryDevice = false;

    public bool UseThrottleDecay = true;

    public float MouseSensitivity = 1.0f;

    public float AuxiliarySensitivity = 1.0f;

    public AxisGroup TorsoPitch = new AxisGroup()
    {

    };

    public AxisGroup TorsoYaw = new AxisGroup()
    {

    };

    public AxisGroup Turn = new AxisGroup()
    {
    };

    public AxisGroup Throttle = new AxisGroup()
    {
        KeyPos = Key.W,
        KeyNeg = Key.S,
    };

    public ButtonGroup FireGroup1 = new ButtonGroup()
    {
        Key = Key.Numpad1,
    };

    public ButtonGroup FireGroup2 = new ButtonGroup()
    {
        Key = Key.Numpad2,
    };

    public ButtonGroup FireGroup3 = new ButtonGroup()
    {
        Key = Key.Numpad3,
    };

    public ButtonGroup FireGroup4 = new ButtonGroup()
    {
        Key = Key.Numpad4,
    };

    public ButtonGroup FireGroup5 = new ButtonGroup()
    {
        Key = Key.Numpad5,
    };

    public ButtonGroup FireGroup6 = new ButtonGroup()
    {
        Key = Key.Numpad6,
    };

    public ButtonGroup Power = new ButtonGroup()
    {
        Key = Key.P,
    };

    public ButtonGroup ShutdownOverride = new ButtonGroup()
    {
        Key = Key.O,
    };

    public ButtonGroup JumpJet = new ButtonGroup()
    {
        Key = Key.Space,
    };

    public ButtonGroup ExternalCamera = new ButtonGroup()
    {
        Key = Key.C,
    };

    public ButtonGroup NextNavPoint = new ButtonGroup()
    {
        Key = Key.N,
    };

    public ButtonGroup FlushCoolant = new ButtonGroup()
    {
        Key = Key.F,
    };

    public ButtonGroup NextEnemyRadar = new ButtonGroup()
    {
        Key = Key.E,
    };

    public ButtonGroup ToggleZoom = new ButtonGroup()
    {
        Key = Key.Z,
    };

    public ButtonGroup TargetUnderReticle = new ButtonGroup()
    {
        Key = Key.Q,
    };

    public ButtonGroup LightAmplification = new ButtonGroup()
    {
        Key = Key.L,
    };

    public ButtonGroup SetThrottle0 = new ButtonGroup()
    {
        Key = Key.Digit1,
    };

    public ButtonGroup SetThrottle1 = new ButtonGroup()
    {
        Key = Key.Digit2,
    };

    public ButtonGroup SetThrottle2 = new ButtonGroup()
    {
        Key = Key.Digit3,
    };

    public ButtonGroup SetThrottle3 = new ButtonGroup()
    {
        Key = Key.Digit4,
    };

    public ButtonGroup SetThrottle4 = new ButtonGroup()
    {
        Key = Key.Digit5,
    };

    public ButtonGroup SetThrottle5 = new ButtonGroup()
    {
        Key = Key.Digit6,
    };

    public ButtonGroup SetThrottle6 = new ButtonGroup()
    {
        Key = Key.Digit7,
    };

    public ButtonGroup SetThrottle7 = new ButtonGroup()
    {
        Key = Key.Digit8,
    };

    public ButtonGroup SetThrottle8 = new ButtonGroup()
    {
        Key = Key.Digit9,
    };

    public ButtonGroup SetThrottle9 = new ButtonGroup()
    {
        Key = Key.Digit0,
    };

    public ButtonGroup Objectives = new ButtonGroup()
    {
        Key = Key.Tab,
    };

    public ButtonGroup AlignLegsToTorso = new ButtonGroup()
    {
        Key = Key.X,
    };

    public ButtonGroup Command1 = new ButtonGroup()
    {
        Key = Key.F1,
    };

    public ButtonGroup Command2 = new ButtonGroup()
    {
        Key = Key.F2,
    };

    public ButtonGroup Command3 = new ButtonGroup()
    {
        Key = Key.F3,
    };

    public ButtonGroup Command4 = new ButtonGroup()
    {
        Key = Key.F4,
    };

    public ButtonGroup Command5 = new ButtonGroup()
    {
        Key = Key.F5,
    };

    public ButtonGroup Command6 = new ButtonGroup()
    {
        Key = Key.F6,
    };

    public ButtonGroup Command7 = new ButtonGroup()
    {
        Key = Key.F7,
    };

    public ButtonGroup WeaponMenuUp = new ButtonGroup()
    {
        Key = Key.UpArrow,
    };

    public ButtonGroup WeaponMenuDown = new ButtonGroup()
    {
        Key = Key.DownArrow,
    };

    public ButtonGroup WeaponMenuLeft = new ButtonGroup()
    {
        Key = Key.LeftArrow,
    };

    public ButtonGroup WeaponMenuRight = new ButtonGroup()
    {
        Key = Key.RightArrow,
    };

    public ButtonGroup WeaponMenuSelect = new ButtonGroup()
    {
        Key = Key.NumpadEnter,
    };

    [System.Serializable]
    public class ButtonGroup
    {
        public Key Key = Key.None;
        public int Mouse = -1;
        public int Auxiliary = -1;
        public bool AuxiliaryInverted = false;
    }

    [System.Serializable]
    public class AxisGroup
    {
        public Key KeyPos = Key.None;
        public Key KeyNeg = Key.None;
        public MouseAxis MouseAxis = MouseAxis.None;
        public int AuxiliaryPos = -1;
        public int AuxiliaryNeg = -1;
    }
}
