using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CareerLocationScreen : MonoBehaviour
{
    [SerializeField]
    protected CareerPauseMenu careerPauseMenu;

    [SerializeField]
    protected CareerInfoUI careerInfoUI;

    [SerializeField]
    protected Camera mainCamera;

    [SerializeField]
    protected Text interactableText;

    [SerializeField]
    InteractableBase lastHoveredInteractable;

    RaycastHit raycastHit;
    Ray ray;

    // Update is called once per frame
    protected virtual void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            gameObject.SetActive(false);
            careerPauseMenu.gameObject.SetActive(true);
            return;
        }

        ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out raycastHit, 300.0f))
        {
            if (lastHoveredInteractable == null || lastHoveredInteractable.gameObject != raycastHit.transform.gameObject)
            {
                InteractableBase interactableBase = raycastHit.transform.gameObject.GetComponent<InteractableBase>();

                if (interactableBase != null)
                {
                    lastHoveredInteractable = interactableBase;
                    lastHoveredInteractable.StartHover();
                    interactableText.text = lastHoveredInteractable.DisplayName;
                }
            }

            if (lastHoveredInteractable != null)
            {
                interactableText.enabled = true;
                interactableText.transform.position = Input.mousePosition;

                if (Input.GetMouseButtonDown(0))
                {
                    Interact(lastHoveredInteractable);
                }
            }
            else
            {
                interactableText.enabled = false;
            }
        }
        else
        {
            if (lastHoveredInteractable != null)
            {
                lastHoveredInteractable.EndHover();
                lastHoveredInteractable = null;
                interactableText.enabled = false;
            }
        }
    }

    protected virtual void Interact(InteractableBase interactable)
    {
        interactable.Interact();

        if (interactable is InteractableDoor)
        {
            InteractableDoor interactableDoor = interactable as InteractableDoor;

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
