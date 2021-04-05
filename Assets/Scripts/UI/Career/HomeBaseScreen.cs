using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public sealed class HomeBaseScreen : CareerLocationScreen
{
    [SerializeField]
    Transform startingCameraPosition;

    [SerializeField]
    Transform mechHangarCameraPosition;

    [SerializeField]
    Text baseLocationText;

    [Header("Terminal Screens")]

    [SerializeField]
    CommanderTerminalScreen commanderTerminalScreen;

    [SerializeField]
    DropshipLaunchPadTerminalScreen dropshipLaunchPadTerminalScreen;

    [SerializeField]
    MechHangarTerminalScreen mechHangarTerminalScreen;

    [SerializeField]
    WarehouseTerminalScreen warehouseTerminalScreen;

    void Awake()
    {
        commanderTerminalScreen.gameObject.SetActive(false);
        dropshipLaunchPadTerminalScreen.gameObject.SetActive(false);
        mechHangarTerminalScreen.gameObject.SetActive(false);
        warehouseTerminalScreen.gameObject.SetActive(false);

        mainCamera.transform.position = startingCameraPosition.position;
        mainCamera.transform.rotation = startingCameraPosition.rotation;

        baseLocationText.text = "Central Hub";

        GlobalDataManager.Instance.currentCareer.currentScreen = "HomeBase";
    }

    // Start is called before the first frame update
    void Start()
    {
        if (GlobalDataManager.Instance.currentCareer.CustomizingMechData != null)
        {
            mainCamera.transform.position = mechHangarCameraPosition.position;
            mainCamera.transform.rotation = mechHangarCameraPosition.rotation;

            baseLocationText.text = "Mech Hangar";

            mechHangarTerminalScreen.gameObject.SetActive(true);
            gameObject.SetActive(false);

            int index = GlobalDataManager.Instance.currentCareer.Mechs.Values.ToList().IndexOf(GlobalDataManager.Instance.currentCareer.CustomizingMechData);

            mechHangarTerminalScreen.MechsSubScreen.MechDataListUI.SelectIndex(index);

            GlobalDataManager.Instance.currentCareer.CustomizingMechData = null;
        }
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    void OnEnable()
    {
        Time.timeScale = 1.0f;
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }

    protected override void Interact(InteractableBase interactable)
    {
        interactable.Interact();

        if (interactable is InteractableDoor)
        {
            InteractableDoor interactableDoor = interactable as InteractableDoor;

            baseLocationText.text = interactableDoor.TargetLocationName;

            mainCamera.transform.position = interactableDoor.TargetLocationTransform.position;
            mainCamera.transform.rotation = interactableDoor.TargetLocationTransform.rotation;
        }
        else if (interactable is InteractableTerminal)
        {
            InteractableTerminal interactableTerminal = interactable as InteractableTerminal;

            interactableTerminal.TargetScreen.SetActive(true);

            gameObject.SetActive(false);
        }
    }
}
