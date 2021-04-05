using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public sealed class DropshipScreen : CareerLocationScreen
{
    [SerializeField]
    MissionTerminalScreen missionTerminalScreen;

    void Awake()
    {
        missionTerminalScreen.gameObject.SetActive(false);

        Career career = GlobalDataManager.Instance.currentCareer;

        career.currentScreen = "Dropship";

        career.CheckDutyRoster();
    }

    void Start()
    {
        if (GlobalDataManager.Instance.currentCareer.CustomizingMechData != null)
        {
            missionTerminalScreen.gameObject.SetActive(true);
            gameObject.SetActive(false);

            List<MechData> mechsDropship = GlobalDataManager.Instance.currentCareer.DropshipMechs.Where(mech => mech != null).ToList();

            int index = mechsDropship.IndexOf(GlobalDataManager.Instance.currentCareer.CustomizingMechData);

            if (index > -1)
            {
                missionTerminalScreen.ClickMechButton();            
                missionTerminalScreen.MissionMechSubScreen.MechDataListUI.SelectIndex(index);
            }
            else
            {
                print("Error: Index of customized not found");
            }

            GlobalDataManager.Instance.currentCareer.CustomizingMechData = null;
        }
    }

    protected override void Update()
    {
        base.Update();
    }

    void OnEnable()
    {
        Time.timeScale = 1.0f;
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }
}
