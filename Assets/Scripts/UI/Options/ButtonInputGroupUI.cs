using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonInputGroupUI : MonoBehaviour
{
    [SerializeField]
    ControlOptionsUI controlOptionsUI;

    [SerializeField]
    Button joystickButton;

    [SerializeField]
    Text keyboardText;

    [SerializeField]
    Text mouseText;

    [SerializeField]
    Text joystickText;

    InputManager.InputGroup buttonGroup;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetButtonGroup(InputManager.InputGroup target)
    {
        buttonGroup = target;

        SetText();
    }

    public void SetText()
    {
        keyboardText.text = buttonGroup.KeyDisplay;

        mouseText.text = buttonGroup.MouseDisplay;

        joystickText.text = buttonGroup.AuxiliaryDisplay;

        joystickButton.interactable = InputManager.Instance.HasAuxiliaryDevice;
    }

    public void ClickKeyboardButton()
    {
        controlOptionsUI.OpenRebindWindowKey(buttonGroup, this);

        AudioManager.Instance.PlayButtonClick(0);
    }

    public void ClickMouseButton()
    {
        controlOptionsUI.OpenRebindWindowMouse(buttonGroup, this);

        AudioManager.Instance.PlayButtonClick(0);
    }

    public void ClickJoystickButton()
    {
        controlOptionsUI.OpenRebindWindowJoystick(buttonGroup, this);

        AudioManager.Instance.PlayButtonClick(0);
    }

    public void ClickKeyboardClearButton()
    {
        buttonGroup.key = UnityEngine.InputSystem.Key.None;

        SetText();

        AudioManager.Instance.PlayButtonClick(0);
    }

    public void ClickMouseClearButton()
    {
        buttonGroup.mouseButton = -1;

        SetText();

        AudioManager.Instance.PlayButtonClick(0);
    }

    public void ClickJoystickClearButton()
    {
        buttonGroup.auxiliaryButton = -1;

        SetText();

        AudioManager.Instance.PlayButtonClick(0);
    }
}
