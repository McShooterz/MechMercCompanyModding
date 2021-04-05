using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class DropshipLoadingSubScreen : MonoBehaviour
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

    MechData SelectedMechData { get; set; } = null;

    void Awake()
    {
        hoveredInformationUI.gameObject.SetActive(false);

        foreach (DropshipBayUI dropshipBayUI in dropshipBays)
        {
            dropshipBayUI.callBackSelect = SelectMechData;
            dropshipBayUI.callBackHover = SetHoveredDropshipBay;
        }
    }

    void OnEnable()
    {
        mechDraggableListUI.BuildList(GlobalDataManager.Instance.currentCareer.Mechs.Values.ToList(), SelectMechData, SetHoveredMechdata);

        MechData[] dropshipMechs = GlobalDataManager.Instance.currentCareer.DropshipMechs;

        List<MechData> mechList = GlobalDataManager.Instance.currentCareer.Mechs.Values.ToList();

        for (int i = 0; i < 12; i++)
        {
            MechData mechData = dropshipMechs[i];

            dropshipBays[i].SetMechData(mechData);

            mechDraggableListUI.RemoveMech(mechData);
        }
    }

    void OnDisable()
    {
        Career career = GlobalDataManager.Instance.currentCareer;
        MechData[] dropshipMechs = new MechData[12];
        //List<MechData> dropshipMechsList = new List<MechData>();

        for (int i = 0; i < 12; i++)
        {
            dropshipMechs[i] = dropshipBays[i].MechData;
        }

        MechData playerMech = career.DutyRosterMechPlayer;

        if (playerMech != null && !dropshipMechs.Contains(playerMech))
        {
            career.DutyRosterMechPlayer = null;
        }

        MechData[] dutyRosterMechs = career.DutyRosterMechs;

        for (int i = 0; i < 11; i++)
        {
            MechData mechData = dutyRosterMechs[i];
            if (mechData != null && !dropshipMechs.Contains(mechData))
            {
                dutyRosterMechs[i] = null;
            }
        }

        career.DutyRosterMechs = dutyRosterMechs;
        career.DropshipMechs = dropshipMechs;
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
            }
            else
            {
                mechDraggableUI.transform.position = Input.mousePosition;
            }
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

    public void SelectMechData(MechData mechData)
    {
        AudioManager.Instance.PlayButtonClick(0);

        SelectedMechData = mechData;

        mechDraggableUI.SetMech(SelectedMechData);

        hoveredInformationUI.gameObject.SetActive(false);
    }
}
