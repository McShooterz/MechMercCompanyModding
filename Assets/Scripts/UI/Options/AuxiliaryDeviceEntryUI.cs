using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class AuxiliaryDeviceEntryUI : MonoBehaviour
{
    [SerializeField]
    ControlOptionsUI controlOptionsUI;

    [SerializeField]
    AuxiliaryDeviceSelectWindow auxiliaryDeviceSelectWindow;

    [SerializeField]
    Text auxiliaryDeviceNameText;

    InputDevice inputDevice;

    public void SetInputDevice(InputDevice device)
    {
        inputDevice = device;

        if (inputDevice != null)
        {
            auxiliaryDeviceNameText.text = inputDevice.displayName.ToUpper();
        }
        else
        {
            auxiliaryDeviceNameText.text = "NONE";
        }
    }

    public void ClickSelectButton()
    {
        InputManager.Instance.SetAuxiliaryDevice(inputDevice);

        controlOptionsUI.RefreshInputs();

        auxiliaryDeviceSelectWindow.gameObject.SetActive(false);

        AudioManager.Instance.PlayButtonClick(0);
    }
}
