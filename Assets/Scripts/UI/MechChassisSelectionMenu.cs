using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MechChassisSelectionMenu : MonoBehaviour
{
    [SerializeField]
    Transform content;

    [SerializeField]
    MechChassisButton firstMechChassisButton;

    [SerializeField]
    List<MechChassisButton> mechChassisButtons = new List<MechChassisButton>();

    [SerializeField]
    Text mechChassisInformationText;

    [SerializeField]
    Camera mechPreviewCamera;

    [SerializeField]
    GameObject mechPreview;

    [SerializeField]
    MechChassisDefinition selectedChassisDefinition;

    public delegate void CallBackFunction(MechChassisDefinition mechChassisDefinition);
    public CallBackFunction callBackFunction;

    // Use this for initialization
    void Start ()
    {

    }
	
	// Update is called once per frame
	void Update ()
    {
		if ((object)mechPreview != null)
        {
            mechPreview.transform.RotateAround(mechPreview.transform.position, Vector3.up, 100f * Time.unscaledDeltaTime);
        }
	}

    public void BuildDesignsList(MechChassisDefinition[] mechChassisDefinitions)
    {
        if (mechChassisDefinitions.Length > 0)
        {
            firstMechChassisButton.gameObject.SetActive(true);
            firstMechChassisButton.SetMechChassisDefinition(mechChassisDefinitions[0]);
            firstMechChassisButton.SetCallBackFunction(SelectMechChassis);
            mechChassisButtons.Add(firstMechChassisButton);

            for (int i = 1; i < mechChassisDefinitions.Length; i++)
            {
                GameObject mechChassisButtonObject = Instantiate(firstMechChassisButton.gameObject, content);

                MechChassisButton mechChassisButton = mechChassisButtonObject.GetComponent<MechChassisButton>();

                if (mechChassisButton != null)
                {
                    mechChassisButton.SetMechChassisDefinition(mechChassisDefinitions[i]);
                    mechChassisButton.SetCallBackFunction(SelectMechChassis);
                    mechChassisButtons.Add(mechChassisButton);
                }
                else if (mechChassisButtonObject != null)
                {
                    Destroy(mechChassisButtonObject);
                }
            }

            SelectMechChassis(mechChassisDefinitions[0]);
        }
        else
        {
            firstMechChassisButton.gameObject.SetActive(false);
        }
    }

    public void SetCallBackFunction(CallBackFunction callBackFunction)
    {
        this.callBackFunction = callBackFunction;
    }

    public void SelectMechChassis(MechChassisDefinition mechChassisDefinition)
    {
        selectedChassisDefinition = mechChassisDefinition;

        mechChassisInformationText.text = selectedChassisDefinition.GetDisplayInformation();

        if (mechPreview != null)
        {
            Destroy(mechPreview);
        }

        GameObject mechPreviewPrefab = selectedChassisDefinition.GetMechPrefab();

        if (mechPreviewPrefab != null)
        {
            mechPreview = Instantiate(mechPreviewPrefab, mechPreviewCamera.transform);

            mechPreview.transform.Rotate(new Vector3(0f, 180f, 0f));

            CharacterController characterController = mechPreview.GetComponent<CharacterController>();

            mechPreview.transform.localPosition = new Vector3(0f, -characterController.bounds.extents.y, 2.5f);

            MechMetaController mechMetaController = mechPreview.GetComponent<MechMetaController>();

            //Material mechMaterial = mechMetaController.Renderers[0].material;
            //mechMaterial.SetTexture("_MainTex", mechChassisDefinition.GetDefaultSkin());

            //foreach(Renderer renderer in mechMetaController.Renderers)
            //{
                //renderer.material = mechMaterial;
            //}
        }
    }

    void ClearMechChassisList()
    {
        for (int i = 1; i < mechChassisButtons.Count; i++)
        {
            Destroy(mechChassisButtons[i].gameObject);
        }

        mechChassisButtons.Clear();
    }

    public void ClickCancelButton()
    {
        gameObject.SetActive(false);

        AudioManager.Instance.PlayButtonClick(0);
    }

    public void ClickSelectButton()
    {
        callBackFunction(selectedChassisDefinition);

        AudioManager.Instance.PlayButtonClick(0);
    }
}
