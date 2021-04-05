using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class AuxiliaryDeviceSelectWindow : MonoBehaviour
{
    [SerializeField]
    GameObject auxiliaryDeviceEntryPrefab;

    [SerializeField]
    List<AuxiliaryDeviceEntryUI> auxiliaryDeviceEntryUIs;

    [SerializeField]
    Transform content;

    void Awake()
    {
        InputSystem.onDeviceChange += BuildEntriesList;

        auxiliaryDeviceEntryPrefab.SetActive(false);
    }

    void OnEnable()
    {
        BuildEntriesList();
    }

    void BuildEntriesList()
    {
        ReadOnlyArray<InputDevice> devices = InputSystem.devices;

        List<InputDevice> validDevices = new List<InputDevice>();
        validDevices.Add(null);

        //Get valid devices
        for (int i = 0; i < devices.Count; i++)
        {
            InputDevice potentialDevice = devices[i];

            if (!(potentialDevice is Mouse) && !(potentialDevice is Keyboard))
            {
                validDevices.Add(potentialDevice);
            }
        }

        //Resize
        while (auxiliaryDeviceEntryUIs.Count < validDevices.Count + 1)
        {
            auxiliaryDeviceEntryUIs.Add(Instantiate(auxiliaryDeviceEntryPrefab, content).GetComponent<AuxiliaryDeviceEntryUI>());
        }

        //Assign devices
        for (int i = 0; i < auxiliaryDeviceEntryUIs.Count; i++)
        {
            AuxiliaryDeviceEntryUI auxiliaryDeviceEntryUI = auxiliaryDeviceEntryUIs[i];
            bool deviceExits = i < validDevices.Count;
            auxiliaryDeviceEntryUI.gameObject.SetActive(deviceExits);

            if (deviceExits)
            {
                auxiliaryDeviceEntryUI.gameObject.SetActive(true);
                auxiliaryDeviceEntryUI.SetInputDevice(validDevices[i]);
            }
        }
    }

    void BuildEntriesList(InputDevice device, InputDeviceChange inputDeviceChange)
    {
        BuildEntriesList();
    }

    public void ClickCancelButton()
    {
        gameObject.SetActive(false);
    }
}
