using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MechDesignSaveMenu : MonoBehaviour
{
    [SerializeField]
    UnitCustomizationScreen unitCustomizationScreen;

    [SerializeField]
    GameObject overwriteWarningWindow;

    [SerializeField]
    InputField inputField;

    [SerializeField]
    Transform content;

    [SerializeField]
    DesignButton firstDesignButton;

    [SerializeField]
    List<DesignButton> designButtons;

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    void OnEnable()
    {
        overwriteWarningWindow.gameObject.SetActive(false);
    }

    public void BuildDesignsList(string mechChassisName)
    {
        MechDesign[] mechDesigns = ResourceManager.Instance.GetMechDesignList(mechChassisName);

        inputField.text = mechChassisName;

        ClearDesignsList();

        if (mechDesigns.Length > 0)
        {
            firstDesignButton.gameObject.SetActive(true);
            firstDesignButton.Initialize(SetDesignName, mechDesigns[0]);
            designButtons.Add(firstDesignButton);

            for (int i = 1; i < mechDesigns.Length; i++)
            {
                GameObject designButtonObject = Instantiate(firstDesignButton.gameObject, content);

                DesignButton designButton = designButtonObject.GetComponent<DesignButton>();

                if (designButton != null)
                {
                    designButton.Initialize(SetDesignName, mechDesigns[i]);
                    designButtons.Add(designButton);
                }
                else if (designButtonObject != null)
                {
                    Destroy(designButtonObject);
                }
            }
        }
        else
        {
            firstDesignButton.gameObject.SetActive(false);
        }
    }

    void ClearDesignsList()
    {
        for(int i = 1; i < designButtons.Count; i++)
        {
            if (designButtons[i].gameObject != null)
            {
                Destroy(designButtons[i].gameObject);
            }
        }

        designButtons.Clear();
    }

    public void SetDesignName(string chassisKey, string designKey)
    {
        inputField.text = designKey;
    }

    public void ClickSaveButton()
    {
        AudioManager.Instance.PlayButtonClick(0);

        if (inputField.text == "")
        {
            print("Input field empty");
            return;
        }

        foreach (DesignButton designButton in designButtons)
        {
            if (inputField.text == designButton.GetDesignName())
            {
                overwriteWarningWindow.SetActive(true);
                return;
            }
        }

        unitCustomizationScreen.SaveMechDesign(inputField.text);

        gameObject.SetActive(false);
    }

    public void ClickCancelButton()
    {
        gameObject.SetActive(false);

        AudioManager.Instance.PlayButtonClick(0);
    }

    public void ClickOverwriteCancelButton()
    {
        overwriteWarningWindow.gameObject.SetActive(false);

        AudioManager.Instance.PlayButtonClick(0);
    }

    public void ClickOverwriteSaveButton()
    {
        unitCustomizationScreen.SaveMechDesign(inputField.text);

        gameObject.SetActive(false);

        AudioManager.Instance.PlayButtonClick(0);
    }
}
