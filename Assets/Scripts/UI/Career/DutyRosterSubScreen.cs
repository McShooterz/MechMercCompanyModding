using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class DutyRosterSubScreen : MonoBehaviour
{
    [SerializeField]
    MechDraggableListUI mechDraggableListUI;

    [SerializeField]
    PilotDraggableListUI pilotDraggableListUI;

    [SerializeField]
    DutyRosterSlotUI[] dutyRosterSlotUIs;

    [SerializeField]
    MechDraggableUI mechDraggableUI;

    [SerializeField]
    PilotDraggableUI pilotDraggableUI;

    [SerializeField]
    HoveredInformationUI hoveredInformationUI;

    [SerializeField]
    Image playerPilotImage;

    [SerializeField]
    Text playerNameText;

    [SerializeField]
    Image mechsButtonBackground;

    [SerializeField]
    Image pilotsButtonBackground;

    [SerializeField]
    Color activeColor;

    [SerializeField]
    DutyRosterSlotUI currentDutyRosterSlotUI;

    public DutyRosterSlotUI CurrentDutyRosterSlotUI { set => currentDutyRosterSlotUI = value; }

    MechData SelectedMechData { get; set; } = null;

    MechPilot SelectedMechPilot { get; set; } = null;

    void Awake()
    {
        hoveredInformationUI.gameObject.SetActive(false);
    }

    void Start()
    {
        List<MechData> mechDatas = new List<MechData>();

        foreach (MechData mechData in GlobalDataManager.Instance.currentCareer.DropshipMechs)
        {
            if (mechData != null)
                mechDatas.Add(mechData);
        }

        mechDraggableListUI.BuildList(mechDatas, SelectMech, SetHoveredMechData);

        pilotDraggableListUI.BuildList(GlobalDataManager.Instance.currentCareer.Pilots.Values.ToList(), SelectPilot, SetHoveredPilot);

        mechDraggableListUI.RemoveMech(GlobalDataManager.Instance.currentCareer.DutyRosterMechPlayer);
        dutyRosterSlotUIs[0].PlaceMech(GlobalDataManager.Instance.currentCareer.DutyRosterMechPlayer);

        for (int i = 0; i < 11; i++)
        {
            mechDraggableListUI.RemoveMech(GlobalDataManager.Instance.currentCareer.DutyRosterMechs[i]);
            pilotDraggableListUI.RemovePilot(GlobalDataManager.Instance.currentCareer.DutyRosterPilots[i]);
            dutyRosterSlotUIs[i + 1].PlaceMech(GlobalDataManager.Instance.currentCareer.DutyRosterMechs[i]);
            dutyRosterSlotUIs[i + 1].PlacePilot(GlobalDataManager.Instance.currentCareer.DutyRosterPilots[i]);
        }

        foreach (MechData mechData in GlobalDataManager.Instance.currentCareer.DutyRosterMechs)
        {
            mechDraggableListUI.RemoveMech(mechData);
        }

        foreach (MechPilot mechPilot in GlobalDataManager.Instance.currentCareer.DutyRosterPilots)
        {
            pilotDraggableListUI.RemovePilot(mechPilot);
        }

        mechDraggableListUI.gameObject.SetActive(true);
        pilotDraggableListUI.gameObject.SetActive(false);

        mechsButtonBackground.color = activeColor;
        pilotsButtonBackground.color = Color.white;

        Career career = GlobalDataManager.Instance.currentCareer;

        Sprite sprite;

        sprite = career.PlayerPilotSprite;

        if (sprite != null)
        {
            playerPilotImage.enabled = true;
            playerPilotImage.sprite = sprite;
        }
        else
        {
            playerPilotImage.enabled = false;
        }

        playerNameText.text = career.callsign;
    }

    void OnEnable()
    {
        mechDraggableListUI.RefreshList();

        for (int i = 0; i < 11; i++)
        {
            if (dutyRosterSlotUIs[0].MechData != null && dutyRosterSlotUIs[0].MechData.MechStatus == MechStatusType.InvalidDesign)
            {
                mechDraggableListUI.RestoreMech(dutyRosterSlotUIs[0].MechData);
                dutyRosterSlotUIs[0].PlaceMech(null);
            }
        }
    }

    void OnDisable()
    {
        GlobalDataManager.Instance.currentCareer.DutyRosterMechPlayer = dutyRosterSlotUIs[0].MechData;

        MechData[] mechDatas = new MechData[11];
        MechPilot[] mechPilots = new MechPilot[11];

        for (int i = 0; i < 11; i++)
        {
            mechDatas[i] = dutyRosterSlotUIs[i + 1].MechData;
            mechPilots[i] = dutyRosterSlotUIs[i + 1].MechPilot;
        }

        GlobalDataManager.Instance.currentCareer.DutyRosterMechs = mechDatas;
        GlobalDataManager.Instance.currentCareer.DutyRosterPilots = mechPilots;
    }

    void Update()
    {
        if (SelectedMechData != null)
        {
            if (Input.GetMouseButtonUp(0))
            {
                if (currentDutyRosterSlotUI == null || !currentDutyRosterSlotUI.PlaceMech(SelectedMechData))
                {
                    mechDraggableListUI.RestoreMech(SelectedMechData);
                }

                SelectedMechData = null;
                mechDraggableUI.gameObject.SetActive(false);

                AudioManager.Instance.PlayButtonClick(0);
            }
            else
            {
                mechDraggableUI.transform.position = Input.mousePosition;
            }
        }

        if (SelectedMechPilot != null)
        {
            if (Input.GetMouseButtonUp(0))
            {
                if (currentDutyRosterSlotUI == null || !currentDutyRosterSlotUI.PlacePilot(SelectedMechPilot))
                {
                    pilotDraggableListUI.RestorePilot(SelectedMechPilot);
                }

                SelectedMechPilot = null;
                pilotDraggableUI.gameObject.SetActive(false);

                AudioManager.Instance.PlayButtonClick(0);
            }
            else
            {
                pilotDraggableUI.transform.position = Input.mousePosition;
            }
        }
    }

    public void SetHoveredMechData(MechData mechData)
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

    public void SetHoveredPilot(MechPilot mechPilot)
    {
        if (mechPilot != null)
        {
            hoveredInformationUI.gameObject.SetActive(true);

            hoveredInformationUI.SetText(mechPilot.HoveredDisplay);
        }
        else
        {
            hoveredInformationUI.gameObject.SetActive(false);
        }
    }

    public void SelectMech(MechData mechData)
    {
        AudioManager.Instance.PlayButtonClick(0);

        SelectedMechData = mechData;

        mechDraggableUI.gameObject.SetActive(true);

        mechDraggableUI.SetMech(SelectedMechData);

        hoveredInformationUI.gameObject.SetActive(false);
    }

    public void SelectPilot(MechPilot mechPilot)
    {
        AudioManager.Instance.PlayButtonClick(0);

        SelectedMechPilot = mechPilot;

        pilotDraggableUI.gameObject.SetActive(true);

        pilotDraggableUI.SetPilot(SelectedMechPilot);

        hoveredInformationUI.gameObject.SetActive(false);
    }

    public void ClickMechButton()
    {
        AudioManager.Instance.PlayButtonClick(0);

        mechDraggableListUI.gameObject.SetActive(true);
        pilotDraggableListUI.gameObject.SetActive(false);

        mechsButtonBackground.color = activeColor;
        pilotsButtonBackground.color = Color.white;
    }

    public void ClickPilotButton()
    {
        AudioManager.Instance.PlayButtonClick(0);

        mechDraggableListUI.gameObject.SetActive(false);
        pilotDraggableListUI.gameObject.SetActive(true);

        mechsButtonBackground.color = Color.white;
        pilotsButtonBackground.color = activeColor;
    }
}
