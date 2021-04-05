using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EncyclopediaScreen : MonoBehaviour
{
    [SerializeField]
    Text descriptionText;

    [SerializeField]
    Transform listContent;

    [SerializeField]
    IntButton firstButton;

    [SerializeField]
    Image image;

    [SerializeField]
    RawImage rawImage;

    [SerializeField]
    GameObject previewPrefab;

    [SerializeField]
    Camera previewPrefabCamera;

    EncyclopediaDefinition[] encyclopediaDefinitions;
    IntButton[] buttons;

    // Start is called before the first frame update
    void Start()
    {
        encyclopediaDefinitions = ResourceManager.Instance.GetEncyclopediaDefinitions();
        buttons = new IntButton[encyclopediaDefinitions.Length];

        if (encyclopediaDefinitions.Length > 0)
        {
            buttons[0] = firstButton;
            buttons[0].Initialize(encyclopediaDefinitions[0], 0, SelectEntry);
            SelectEntry(0);

            for (int i = 1; i < buttons.Length; i++)
            {
                buttons[i] = Instantiate(firstButton.gameObject, listContent).GetComponent<IntButton>();
                buttons[i].Initialize(encyclopediaDefinitions[i], i, SelectEntry);
            }
        }
        else
        {
            firstButton.gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if ((object)previewPrefab != null)
        {
            previewPrefab.transform.RotateAround(previewPrefab.transform.position, Vector3.up, 100f * Time.unscaledDeltaTime);
        }
    }

    public void SelectEntry(int index)
    {
        EncyclopediaDefinition encyclopediaDefinition = encyclopediaDefinitions[index];

        if ((object)previewPrefab != null)
        {
            Destroy(previewPrefab);
        }

        previewPrefab = encyclopediaDefinition.GetPrefab();

        if ((object)previewPrefab != null)
        {
            rawImage.gameObject.SetActive(true);
            image.gameObject.SetActive(false);

            previewPrefab = Instantiate(previewPrefab, previewPrefabCamera.transform);

            CharacterController characterController = previewPrefab.GetComponent<CharacterController>();

            if (characterController != null)
            {
                previewPrefab.transform.localPosition = new Vector3(0f, -characterController.bounds.extents.y, 2.5f);
            }
            else
            {
                Collider collider = previewPrefab.GetComponent<Collider>();

                if (collider != null)
                {
                    previewPrefab.transform.localPosition = new Vector3(0f, -collider.bounds.extents.y, 2.5f);
                }
            }
        }
        else
        {
            rawImage.gameObject.SetActive(false);
            image.gameObject.SetActive(true);
        }

        descriptionText.text = System.Text.RegularExpressions.Regex.Unescape(encyclopediaDefinition.GetDescription());
    }

    public void ClickBackButton()
    {
        gameObject.SetActive(false);

        AudioManager.Instance.PlayButtonClick(0);
    }
}
