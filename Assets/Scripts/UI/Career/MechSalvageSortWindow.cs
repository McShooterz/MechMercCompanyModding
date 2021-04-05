using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MechSalvageSortWindow : MonoBehaviour
{
    [SerializeField]
    MechDraggableListUI mechDraggableListUI;

    [SerializeField]
    DropshipBayUI[] dropshipBays;

    [SerializeField]
    MechDraggableUI mechDraggableUI;

    [SerializeField]
    DropshipBayUI currentDropshipBay;

    [SerializeField]
    HoveredInformationUI hoveredInformationUI;

    [SerializeField]
    Button finishButton;

    MechData SelectedMechData { get; set; } = null;

    bool ValidSorting
    {
        get
        {
            foreach (DropshipBayUI dropshipBayUI in dropshipBays)
            {
                if (dropshipBayUI.MechData != null)
                    return true;
            }

            return false;
        }
    }

    void Awake()
    {
        hoveredInformationUI.gameObject.SetActive(false);

        foreach (DropshipBayUI dropshipBayUI in dropshipBays)
        {
            dropshipBayUI.callBackSelect = SelectMechData;
            dropshipBayUI.callBackHover = SetHoveredDropshipBay;
        }
    }

    /*
    void OnEnable()
    {
        mechDraggableListUI.BuildList(GlobalDataManager.Instance.currentCareer.Mechs, SelectMechData, SetHoveredMechdata);

        for (int i = 0; i < 12; i++)
        {
            MechData mechData = GlobalDataManager.Instance.currentCareer.MechsDropshipLoaded[i];

            dropshipBays[i].SetMechData(mechData);

            mechDraggableListUI.RemoveMech(mechData);
        }
    }*/

    public void BuildList(List<MechData> mechsSalvaged)
    {
        List<MechData> mechDatas = new List<MechData>();

        MechData[] dropshipMechs = GlobalDataManager.Instance.currentCareer.DropshipMechs;

        foreach (MechData mechData in dropshipMechs)
        {
            if (mechData != null)
            {
                mechDatas.Add(mechData);
            }
        }

        mechDatas.AddRange(mechsSalvaged);

        mechDraggableListUI.BuildList(mechDatas, SelectMechData, SetHoveredMechdata);

        for (int i = 0; i < 12; i++)
        {
            MechData mechData = dropshipMechs[i];

            dropshipBays[i].SetMechData(mechData);

            mechDraggableListUI.RemoveMech(mechData);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (SelectedMechData != null)
        {
            if (Input.GetMouseButtonUp(0))
            {
                mechDraggableUI.gameObject.SetActive(false);

                if (currentDropshipBay != null && currentDropshipBay.MechData == null)
                {
                    currentDropshipBay.SetMechData(SelectedMechData);

                    AudioManager.Instance.PlayButtonClick(0);
                }
                else
                {
                    mechDraggableListUI.RestoreMech(SelectedMechData);

                    AudioManager.Instance.PlayButtonClick(0);
                }

                SelectedMechData = null;

                finishButton.interactable = ValidSorting;
            }
            else
            {
                mechDraggableUI.transform.position = Input.mousePosition;
            }
        }
    }

    public void SetHoveredMechdata(MechData mechData)
    {
        if (mechData != null)
        {
            hoveredInformationUI.gameObject.SetActive(true);

            hoveredInformationUI.SetText(mechData.HoveredDisplay);
        }
        else
        {
            hoveredInformationUI.gameObject.SetActive(false);
        }
    }

    public void SetHoveredDropshipBay(DropshipBayUI dropshipBayUI)
    {
        currentDropshipBay = dropshipBayUI;

        if (currentDropshipBay != null)
        {
            SetHoveredMechdata(currentDropshipBay.MechData);
        }
        else
        {
            SetHoveredMechdata(null);
        }
    }

    public void SelectMechData(MechData mechData)
    {
        AudioManager.Instance.PlayButtonClick(0);

        SelectedMechData = mechData;

        mechDraggableUI.SetMech(SelectedMechData);

        finishButton.interactable = ValidSorting;

        hoveredInformationUI.gameObject.SetActive(false);
    }

    public void ClickFinishButton()
    {
        AudioManager.Instance.PlayButtonClick(0);

        MechData[] mechs = new MechData[12];

        for (int i = 0; i < 12; i++)
        {
            mechs[i] = dropshipBays[i].MechData;
        }

        GlobalDataManager.Instance.currentCareer.DropshipMechs = mechs;

        gameObject.SetActive(false);
    }
}