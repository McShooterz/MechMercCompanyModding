using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PilotReportSubScreen : MonoBehaviour
{
    [SerializeField]
    PilotReportEntryUI[] pilotReportEntryUIs;

    void Start()
    {
        Career career = GlobalDataManager.Instance.currentCareer;
        List<MechPilot> mechPilotsUsed = new List<MechPilot>();

        for (int i = 0; i < career.DutyRosterPilots.Length; i++)
        {
            MechPilot mechPilot = career.DutyRosterPilots[i];

            if (mechPilot != null)
                mechPilotsUsed.Add(mechPilot);
        }

        pilotReportEntryUIs[0].SetPlayer(career);

        for (int i = 1; i < pilotReportEntryUIs.Length; i++)
        {
            if (i < mechPilotsUsed.Count + 1)
            {
                pilotReportEntryUIs[i].SetPilot(mechPilotsUsed[i - 1]);
            }
            else
            {
                pilotReportEntryUIs[i].gameObject.SetActive(false);
            }
        }
    }
}
