using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionAbortSubScreen : MonoBehaviour
{
    void OnEnable()
    {
        
    }

    public void ClickAbortContractButton()
    {
        AudioManager.Instance.PlayButtonClick(1);

        GlobalDataManager.Instance.currentCareer.currentContract = null;

        GlobalDataManager.Instance.currentCareer.AdvanceWeek();

        LoadingScreen.Instance.LoadScene("HomeBase");
    }
}
