using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MechHangarTerminalScreen : MonoBehaviour
{
    [SerializeField]
    HomeBaseScreen homeBaseScreen;

    [SerializeField]
    MechsSubScreen mechsSubScreen;

    [SerializeField]
    MechMarketSubScreen mechMarketSubScreen;

    [SerializeField]
    Image mechsButtonBackground;

    [SerializeField]
    Image marketButtonBackground;

    [SerializeField]
    Color activeColor;

    [SerializeField]
    Camera mechPreviewCamera;

    [SerializeField]
    GameObject mechPreviewObject;

    public MechsSubScreen MechsSubScreen { get => mechsSubScreen; }

    void Update()
    {
        if ((object)mechPreviewObject != null)
        {
            mechPreviewObject.transform.RotateAround(mechPreviewObject.transform.position, Vector3.up, 100f * Time.unscaledDeltaTime);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ClickClose();
        }
    }

    void OnEnable()
    {
        mechsSubScreen.gameObject.SetActive(false);
        mechMarketSubScreen.gameObject.SetActive(false);

        mechsButtonBackground.color = activeColor;
        marketButtonBackground.color = Color.white;

        mechsSubScreen.gameObject.SetActive(true);
    }

    public void SetPreviewMech(MechChassisDefinition mechChassis, MechPaintScheme mechPaintScheme)
    {
        if (mechPreviewObject != null)
        {
            Destroy(mechPreviewObject);
        }

        GameObject mechPreviewPrefab = mechChassis.GetMechPrefab();

        if ((object)mechPreviewPrefab != null)
        {
            mechPreviewObject = Instantiate(mechPreviewPrefab, mechPreviewCamera.transform);

            mechPreviewObject.transform.Rotate(new Vector3(0f, 180f, 0f));

            CharacterController characterController = mechPreviewObject.GetComponent<CharacterController>();

            mechPreviewObject.transform.localPosition = new Vector3(0f, -characterController.bounds.extents.y, 2.5f);

            if (mechPaintScheme != null)
            {
                MechMetaController mechMetaController = mechPreviewObject.GetComponent<MechMetaController>();

                if (mechMetaController != null)
                {
                    mechMetaController.ApplyMechPaintScheme(mechPaintScheme);
                }
            }
        }
    }

    public void ClearPreviewMech()
    {
        if ((object)mechPreviewObject != null)
        {
            Destroy(mechPreviewObject);
            mechPreviewObject = null;
        }
    }

    public void ClickMechsButton()
    {
        mechsSubScreen.gameObject.SetActive(true);
        mechMarketSubScreen.gameObject.SetActive(false);

        mechsButtonBackground.color = activeColor;
        marketButtonBackground.color = Color.white;

        AudioManager.Instance.PlayButtonClick(0);
    }

    public void ClickMarketButton()
    {
        mechsSubScreen.gameObject.SetActive(false);
        mechMarketSubScreen.gameObject.SetActive(true);

        mechsButtonBackground.color = Color.white;
        marketButtonBackground.color = activeColor;

        AudioManager.Instance.PlayButtonClick(0);
    }

    public void ClickClose()
    {
        homeBaseScreen.gameObject.SetActive(true);

        gameObject.SetActive(false);

        AudioManager.Instance.PlayButtonClick(0);
    }
}
