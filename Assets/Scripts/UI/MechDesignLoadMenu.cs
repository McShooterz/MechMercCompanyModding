using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MechDesignLoadMenu : MonoBehaviour
{
    [SerializeField]
    Transform content;

    [SerializeField]
    Button selectButton;

    [SerializeField]
    DesignButton firstDesignButton;

    [SerializeField]
    List<DesignButton> designButtons = new List<DesignButton>();

    [SerializeField]
    MechDesignInformationUI mechDesignInformation;

    [SerializeField]
    MechDesign selectedMechDesign;

    public delegate void CallBackFunction(MechDesign mechDesign, bool doNotTakeFromInventory);
    public CallBackFunction callBackFunction;

    // Use this for initialization
    void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void BuildDesignsList(MechDesign[] mechDesigns)
    {
        ClearDesignsList();

        if (mechDesigns.Length > 0)
        {
            firstDesignButton.gameObject.SetActive(true);
            firstDesignButton.Initialize(SelectMechDesign, mechDesigns[0]);
            designButtons.Add(firstDesignButton);

            for (int i = 1; i < mechDesigns.Length; i++)
            {
                GameObject designButtonObject = Instantiate(firstDesignButton.gameObject, content);

                DesignButton designButton = designButtonObject.GetComponent<DesignButton>();

                if (designButton != null)
                {
                    designButton.Initialize(SelectMechDesign, mechDesigns[i]);
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

    public void SetCallBackFunction(CallBackFunction function)
    {
        callBackFunction = function;
    }

    public void SelectMechDesign(string chassisKey, string designKey)
    {
        selectedMechDesign = ResourceManager.Instance.GetMechDesign(chassisKey, designKey);

        mechDesignInformation.SetDesign(selectedMechDesign);

        selectButton.interactable = selectedMechDesign != null;
    }

    void ClearDesignsList()
    {
        for (int i = 1; i < designButtons.Count; i++)
        {
            if (designButtons[i].gameObject != null)
            {
                Destroy(designButtons[i].gameObject);
            }
        }

        designButtons.Clear();
    }

    public void ClickCancelButton()
    {
        gameObject.SetActive(false);
    }

    public void ClickSelectButton()
    {
        callBackFunction(selectedMechDesign, false);
    }
}
