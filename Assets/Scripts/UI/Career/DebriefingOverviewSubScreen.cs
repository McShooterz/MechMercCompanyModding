using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebriefingOverviewSubScreen : MonoBehaviour
{
    [SerializeField]
    SalvageSubScreen salvageSubScreen;

    [SerializeField]
    Text missionResultText;

    [SerializeField]
    Text overviewText;

    [SerializeField]
    Text salvageStateText;

    [SerializeField]
    PulseGraphicElement salvageStatePulseGraphicElement;

    [SerializeField]
    GameObject finishConfirmationWindow;

    [SerializeField]
    int totalIncome;

    [SerializeField]
    int totalExpenses;

    [SerializeField]
    int reputationChange;

    void OnEnable()
    {
        finishConfirmationWindow.SetActive(false);

        System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
        Career career = GlobalDataManager.Instance.currentCareer;
        ContractData contractData = GlobalDataManager.Instance.currentCareer.currentContract;
        MissionSetup missionSetup = contractData.CurrentMission;

        int missionPay = 0;
        int contractPay = 0;
        int weeklyExpenses = career.WeeklyExpenses;
        int pilotMissionPay = 0;
        int pilotDeathPayout = 0;
        int balance;

        string startingSpacing = "    ";

        if (career.lastMissionSuccessful)
        {
            missionResultText.text = "Mission Successful";
        }
        else
        {
            missionResultText.text = "Mission Failed";
        }

        stringBuilder.Append(startingSpacing);
        stringBuilder.AppendLine("Objectives:");

        for (int i = 0; i < career.lastMissionObjectives.Length; i++)
        {
            ObjectiveSave objectiveSave = career.lastMissionObjectives[i];

            stringBuilder.Append(startingSpacing);
            stringBuilder.Append(objectiveSave.DisplayText.PadRight(56));
            int objectivePay = Mathf.CeilToInt(objectiveSave.Pay * career.missionPayModifier);
            stringBuilder.AppendLine(StaticHelper.FormatMoney(objectivePay).PadLeft(15));
            missionPay += objectivePay;
        }

        int missionKills = career.PlayerMissionKills;

        for (int i = 0; i < career.DutyRosterPilots.Length; i++)
        {
            MechPilot mechPilot = career.DutyRosterPilots[i];

            if (mechPilot != null)
            {
                missionKills += mechPilot.MissionKills;

                pilotMissionPay += mechPilot.contractMissionPay;

                if (mechPilot.PilotStatus == PilotStatusType.Deceased)
                    pilotDeathPayout += mechPilot.contractDeathPayout;
            }
        }

        int bountyPay = Mathf.CeilToInt(missionKills * contractData.BountyKillPay * career.bountyPayModifier);

        if (bountyPay > 0)
        {
            stringBuilder.AppendLine();
            stringBuilder.Append(startingSpacing);
            stringBuilder.Append(("Confirmed Kill Bounties " + missionKills.ToString()).PadRight(56));
            stringBuilder.AppendLine(StaticHelper.FormatMoney(bountyPay).PadLeft(15));
        }

        bool onLastMission = contractData.OnLastMission;

        if (onLastMission)
        {
            contractPay = Mathf.CeilToInt(contractData.ContractPayActual * career.contractPayModifier);

            stringBuilder.AppendLine();
            stringBuilder.Append(startingSpacing);
            stringBuilder.Append(("Contract Compensation").PadRight(56));
            stringBuilder.AppendLine(StaticHelper.FormatMoney(contractPay).PadLeft(15));
        }

        stringBuilder.AppendLine();
        stringBuilder.Append(startingSpacing);
        stringBuilder.Append(("Weekly Expenses").PadRight(56));

        if (onLastMission)
        {
            stringBuilder.AppendLine(StaticHelper.FormatMoney(weeklyExpenses * 2).PadLeft(15));
        }
        else
        {
            stringBuilder.AppendLine(StaticHelper.FormatMoney(weeklyExpenses).PadLeft(15));
        }

        if (pilotMissionPay > 0)
        {
            stringBuilder.Append(startingSpacing);
            stringBuilder.Append(("Pilot Mission Pay").PadRight(56));
            stringBuilder.AppendLine(StaticHelper.FormatMoney(pilotMissionPay).PadLeft(15));
        }

        if (pilotDeathPayout > 0)
        {
            stringBuilder.Append(startingSpacing);
            stringBuilder.Append(("Pilot KIA Payouts").PadRight(56));
            stringBuilder.AppendLine(StaticHelper.FormatMoney(pilotDeathPayout).PadLeft(15));
        }

        stringBuilder.AppendLine();
        stringBuilder.Append(startingSpacing);
        stringBuilder.Append(("Funds").PadRight(56));
        stringBuilder.AppendLine(StaticHelper.FormatMoney(career.funds).PadLeft(15));

        totalIncome = missionPay + contractPay + bountyPay;

        stringBuilder.Append(startingSpacing);
        stringBuilder.Append(("Total Income").PadRight(56));
        stringBuilder.AppendLine(StaticHelper.FormatMoney(totalIncome).PadLeft(15));

        totalExpenses = weeklyExpenses + pilotMissionPay + pilotDeathPayout;

        stringBuilder.Append(startingSpacing);
        stringBuilder.Append(("Total Expenses").PadRight(56));

        if (onLastMission)
        {
            stringBuilder.AppendLine(StaticHelper.FormatMoney(totalExpenses + weeklyExpenses).PadLeft(15));
        }
        else
        {
            stringBuilder.AppendLine(StaticHelper.FormatMoney(totalExpenses).PadLeft(15));
        }

        balance = career.funds + totalIncome - totalExpenses;

        stringBuilder.Append(startingSpacing);
        stringBuilder.Append(("Funds Balance").PadRight(56));
        stringBuilder.AppendLine(StaticHelper.FormatMoney(balance).PadLeft(15));

        if (onLastMission)
        {
            reputationChange = contractData.ReputationFinal;
            int reputationFinal = career.reputation + reputationChange;

            stringBuilder.AppendLine();
            stringBuilder.Append(startingSpacing);
            stringBuilder.Append(("Reputation").PadRight(56));

            if (reputationFinal < 0)
            {
                stringBuilder.AppendLine((reputationFinal + "(" + reputationChange + ")").PadLeft(15));
            }
            else
            {
                stringBuilder.AppendLine((reputationFinal + "(+" + reputationChange + ")").PadLeft(15));
            }
        }

        overviewText.text = stringBuilder.ToString();

        if (career.lastMissionSuccessful)
        {
            if (salvageSubScreen.SalvageDone)
            {
                salvageStateText.text = "Done";
                salvageStateText.color = Color.green;
                salvageStatePulseGraphicElement.Frequency = 0;
            }
            else
            {
                salvageStateText.text = "Available";
                salvageStateText.color = Color.yellow;
                salvageStatePulseGraphicElement.Frequency = 5;
            }
        }
        else
        {
            salvageStateText.text = "None";
            salvageStateText.color = Color.white;
            salvageStatePulseGraphicElement.Frequency = 0;
        }
    }

    public void ClickFinishButton()
    {
        AudioManager.Instance.PlayButtonClick(0);

        finishConfirmationWindow.SetActive(true);
    }

    public void ClickAcceptFinishButton()
    {
        AudioManager.Instance.PlayButtonClick(1);

        Career career = GlobalDataManager.Instance.currentCareer;

        career.funds += totalIncome - totalExpenses;

        career.reputation += reputationChange;

        career.gameDate.AddDays(7);

        ContractData contractData = career.currentContract;

        if (career.deceased)
        {
            if (career.permadeath)
            {
                LoadingScreen.Instance.LoadScene("CareerSelection");
            }
            else
            {
                CareerSave careerSave = ResourceManager.Instance.GetCareer(career.uniqueIdentifier);

                GlobalDataManager.Instance.currentCareer = new Career(careerSave);

                LoadingScreen.Instance.LoadScene(GlobalDataManager.Instance.currentCareer.currentScreen);
            }
        }
        else
        {
            career.ApplyMissionKills();

            for (int i = 0; i < career.DutyRosterPilots.Length; i++)
            {
                MechPilot mechPilot = career.DutyRosterPilots[i];

                if (mechPilot != null)
                {
                    mechPilot.ApplyMissionKills();
                }
            }

            career.RemoveDeceasedPilots();

            if (contractData.OnLastMission)
            {
                career.currentContract = null;

                career.AdvanceWeek();

                career.currentScreen = "HomeBase";

                ResourceManager.Instance.StoreCareer(career.CareerSave);

                LoadingScreen.Instance.LoadScene("HomeBase");
            }
            else
            {
                contractData.IncrementMissionIndex();

                career.currentScreen = "Dropship";

                ResourceManager.Instance.StoreCareer(career.CareerSave);

                LoadingScreen.Instance.LoadScene("Dropship");
            }
        }
    }

    public void ClickCancelFinishButton()
    {
        AudioManager.Instance.PlayButtonClick(0);

        finishConfirmationWindow.SetActive(false);
    }
}