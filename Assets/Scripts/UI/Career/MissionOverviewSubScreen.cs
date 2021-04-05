using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissionOverviewSubScreen : MonoBehaviour
{
    [SerializeField]
    FactionLogoUI employerFactionLogo;

    [SerializeField]
    Text missionOverviewText;


    void Start()
    {
        System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();

        Career career = GlobalDataManager.Instance.currentCareer;
        ContractData contractData = career.currentContract;
        MissionSetup missionSetup = contractData.CurrentMission;

        employerFactionLogo.SetFactionLogo(contractData.EmployerDefinition.FactionLogo);

        stringBuilder.AppendLine("Contract: " + contractData.ContractDefinition.GetDisplayName());

        stringBuilder.AppendLine("Employer: " + contractData.EmployerDefinition.GetDisplayName());

        stringBuilder.AppendLine("Difficulty: " + (contractData.DifficultyEstimated + 1).ToString());

        stringBuilder.AppendLine("Current Mission: " + missionSetup.DisplayName);

        stringBuilder.AppendLine("Mission Index: " + (contractData.MissionIndex + 1) + "/" + contractData.MissionSetups.Length.ToString());

        missionOverviewText.text = stringBuilder.ToString();
    }
}
