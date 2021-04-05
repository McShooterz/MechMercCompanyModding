using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissionTerminalScreen : MonoBehaviour
{
    [SerializeField]
    DropshipScreen dropshipScreen;

    [SerializeField]
    MissionOverviewSubScreen missionOverviewSubScreen;

    [SerializeField]
    MissionSubScreen missionSubScreen;

    [SerializeField]
    DutyRosterSubScreen dutyRosterSubScreen;

    [SerializeField]
    BriefingSubScreen briefingSubScreen;

    [SerializeField]
    MissionMechSubScreen missionMechSubScreen;

    [SerializeField]
    InventorySubScreen inventorySubScreen;

    [SerializeField]
    FinancesSubScreen financesSubScreen;

    [SerializeField]
    MissionAbortSubScreen missionAbortSubScreen;

    [SerializeField]
    Image overviewButtonBackground;

    [SerializeField]
    Image missionButtonBackground;

    [SerializeField]
    Image dutyRosterButtonBackground;

    [SerializeField]
    Image briefingButtonBackground;

    [SerializeField]
    Image mechsButtonBackground;

    [SerializeField]
    Image inventoryButtonBackground;

    [SerializeField]
    Image financesButtonBackground;

    [SerializeField]
    Image abortButtonBackground;

    [SerializeField]
    Text abortButtonText;

    [SerializeField]
    Color activeColor;

    public MissionMechSubScreen MissionMechSubScreen { get => missionMechSubScreen; }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ClickCloseButton();
        }
    }

    void OnEnable()
    {
        missionOverviewSubScreen.gameObject.SetActive(true);
        missionSubScreen.gameObject.SetActive(false);
        dutyRosterSubScreen.gameObject.SetActive(false);
        briefingSubScreen.gameObject.SetActive(false);
        missionMechSubScreen.gameObject.SetActive(false);
        inventorySubScreen.gameObject.SetActive(false);
        financesSubScreen.gameObject.SetActive(false);
        missionAbortSubScreen.gameObject.SetActive(false);

        overviewButtonBackground.color = activeColor;
        missionButtonBackground.color = Color.white;
        dutyRosterButtonBackground.color = Color.white;
        briefingButtonBackground.color = Color.white;
        mechsButtonBackground.color = Color.white;
        inventoryButtonBackground.color = Color.white;
        financesButtonBackground.color = Color.white;
        abortButtonBackground.color = Color.red;
        abortButtonText.color = Color.white;
    }

    public void ClickOverviewButton()
    {
        AudioManager.Instance.PlayButtonClick(0);

        dutyRosterSubScreen.gameObject.SetActive(false);
        missionOverviewSubScreen.gameObject.SetActive(true);
        missionSubScreen.gameObject.SetActive(false);
        briefingSubScreen.gameObject.SetActive(false);
        missionMechSubScreen.gameObject.SetActive(false);
        inventorySubScreen.gameObject.SetActive(false);
        financesSubScreen.gameObject.SetActive(false);
        missionAbortSubScreen.gameObject.SetActive(false);

        overviewButtonBackground.color = activeColor;
        missionButtonBackground.color = Color.white;
        dutyRosterButtonBackground.color = Color.white;
        briefingButtonBackground.color = Color.white;
        mechsButtonBackground.color = Color.white;
        inventoryButtonBackground.color = Color.white;
        financesButtonBackground.color = Color.white;
        abortButtonBackground.color = Color.red;
        abortButtonText.color = Color.white;
    }

    public void ClickMissionButton()
    {
        AudioManager.Instance.PlayButtonClick(0);

        dutyRosterSubScreen.gameObject.SetActive(false);
        missionOverviewSubScreen.gameObject.SetActive(false);
        missionSubScreen.gameObject.SetActive(true);
        briefingSubScreen.gameObject.SetActive(false);
        missionMechSubScreen.gameObject.SetActive(false);
        inventorySubScreen.gameObject.SetActive(false);
        financesSubScreen.gameObject.SetActive(false);
        missionAbortSubScreen.gameObject.SetActive(false);

        overviewButtonBackground.color = Color.white;
        missionButtonBackground.color = activeColor;
        dutyRosterButtonBackground.color = Color.white;
        briefingButtonBackground.color = Color.white;
        mechsButtonBackground.color = Color.white;
        inventoryButtonBackground.color = Color.white;
        financesButtonBackground.color = Color.white;
        abortButtonBackground.color = Color.red;
        abortButtonText.color = Color.white;
    }

    public void ClickDutyRosterButton()
    {
        AudioManager.Instance.PlayButtonClick(0);

        dutyRosterSubScreen.gameObject.SetActive(true);
        missionOverviewSubScreen.gameObject.SetActive(false);
        missionSubScreen.gameObject.SetActive(false);
        briefingSubScreen.gameObject.SetActive(false);
        missionMechSubScreen.gameObject.SetActive(false);
        inventorySubScreen.gameObject.SetActive(false);
        financesSubScreen.gameObject.SetActive(false);
        missionAbortSubScreen.gameObject.SetActive(false);

        overviewButtonBackground.color = Color.white;
        missionButtonBackground.color = Color.white;
        dutyRosterButtonBackground.color = activeColor;
        briefingButtonBackground.color = Color.white;
        mechsButtonBackground.color = Color.white;
        inventoryButtonBackground.color = Color.white;
        financesButtonBackground.color = Color.white;
        abortButtonBackground.color = Color.red;
        abortButtonText.color = Color.white;
    }

    public void ClickBriefingButton()
    {
        AudioManager.Instance.PlayButtonClick(0);

        dutyRosterSubScreen.gameObject.SetActive(false);
        missionOverviewSubScreen.gameObject.SetActive(false);
        missionSubScreen.gameObject.SetActive(false);
        briefingSubScreen.gameObject.SetActive(true);
        missionMechSubScreen.gameObject.SetActive(false);
        inventorySubScreen.gameObject.SetActive(false);
        financesSubScreen.gameObject.SetActive(false);
        missionAbortSubScreen.gameObject.SetActive(false);

        overviewButtonBackground.color = Color.white;
        missionButtonBackground.color = Color.white;
        dutyRosterButtonBackground.color = Color.white;
        briefingButtonBackground.color = activeColor;
        mechsButtonBackground.color = Color.white;
        inventoryButtonBackground.color = Color.white;
        financesButtonBackground.color = Color.white;
        abortButtonBackground.color = Color.red;
        abortButtonText.color = Color.white;
    }

    public void ClickMechButton()
    {
        AudioManager.Instance.PlayButtonClick(0);

        dutyRosterSubScreen.gameObject.SetActive(false);
        missionOverviewSubScreen.gameObject.SetActive(false);
        missionSubScreen.gameObject.SetActive(false);
        briefingSubScreen.gameObject.SetActive(false);
        missionMechSubScreen.gameObject.SetActive(true);
        inventorySubScreen.gameObject.SetActive(false);
        financesSubScreen.gameObject.SetActive(false);
        missionAbortSubScreen.gameObject.SetActive(false);

        overviewButtonBackground.color = Color.white;
        missionButtonBackground.color = Color.white;
        dutyRosterButtonBackground.color = Color.white;
        briefingButtonBackground.color = Color.white;
        mechsButtonBackground.color = activeColor;
        inventoryButtonBackground.color = Color.white;
        financesButtonBackground.color = Color.white;
        abortButtonBackground.color = Color.red;
        abortButtonText.color = Color.white;
    }

    public void ClickInventoryButton()
    {
        AudioManager.Instance.PlayButtonClick(0);

        dutyRosterSubScreen.gameObject.SetActive(false);
        missionOverviewSubScreen.gameObject.SetActive(false);
        missionSubScreen.gameObject.SetActive(false);
        briefingSubScreen.gameObject.SetActive(false);
        missionMechSubScreen.gameObject.SetActive(false);
        inventorySubScreen.gameObject.SetActive(true);
        financesSubScreen.gameObject.SetActive(false);
        missionAbortSubScreen.gameObject.SetActive(false);

        overviewButtonBackground.color = Color.white;
        missionButtonBackground.color = Color.white;
        dutyRosterButtonBackground.color = Color.white;
        briefingButtonBackground.color = Color.white;
        mechsButtonBackground.color = Color.white;
        inventoryButtonBackground.color = activeColor;
        financesButtonBackground.color = Color.white;
        abortButtonBackground.color = Color.red;
        abortButtonText.color = Color.white;
    }

    public void ClickFinancesButton()
    {
        AudioManager.Instance.PlayButtonClick(0);

        dutyRosterSubScreen.gameObject.SetActive(false);
        missionOverviewSubScreen.gameObject.SetActive(false);
        missionSubScreen.gameObject.SetActive(false);
        briefingSubScreen.gameObject.SetActive(false);
        missionMechSubScreen.gameObject.SetActive(false);
        inventorySubScreen.gameObject.SetActive(false);
        financesSubScreen.gameObject.SetActive(true);
        missionAbortSubScreen.gameObject.SetActive(false);

        overviewButtonBackground.color = Color.white;
        missionButtonBackground.color = Color.white;
        dutyRosterButtonBackground.color = Color.white;
        briefingButtonBackground.color = Color.white;
        mechsButtonBackground.color = Color.white;
        inventoryButtonBackground.color = Color.white;
        financesButtonBackground.color = activeColor;
        abortButtonBackground.color = Color.red;
        abortButtonText.color = Color.white;
    }

    public void ClickAbortButton()
    {
        AudioManager.Instance.PlayButtonClick(0);

        dutyRosterSubScreen.gameObject.SetActive(false);
        missionOverviewSubScreen.gameObject.SetActive(false);
        missionSubScreen.gameObject.SetActive(false);
        briefingSubScreen.gameObject.SetActive(false);
        missionMechSubScreen.gameObject.SetActive(false);
        inventorySubScreen.gameObject.SetActive(false);
        financesSubScreen.gameObject.SetActive(false);
        missionAbortSubScreen.gameObject.SetActive(true);

        overviewButtonBackground.color = Color.white;
        missionButtonBackground.color = Color.white;
        dutyRosterButtonBackground.color = Color.white;
        briefingButtonBackground.color = Color.white;
        mechsButtonBackground.color = Color.white;
        inventoryButtonBackground.color = Color.white;
        financesButtonBackground.color = Color.white;
        abortButtonBackground.color = activeColor;
        abortButtonText.color = Color.black;
    }

    public void ClickCloseButton()
    {
        AudioManager.Instance.PlayButtonClick(0);

        gameObject.SetActive(false);
        dropshipScreen.gameObject.SetActive(true);
    }
}
