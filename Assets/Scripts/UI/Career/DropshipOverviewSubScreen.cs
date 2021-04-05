using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DropshipOverviewSubScreen : MonoBehaviour
{
    [SerializeField]
    Text currentContractNameText;

    [SerializeField]
    Text weeksUntilContractValueText;

    [SerializeField]
    Text loadedMechsValueText;

    [SerializeField]
    Text dropshipTravelValueText;

    [SerializeField]
    Text contractTravelCoverageValueText;

    [SerializeField]
    Text weeklyExpensesValueText;

    [SerializeField]
    Text fundsValueText;

    [SerializeField]
    Text totalExpensesValueText;

    [SerializeField]
    Text balanceValueText;

    [SerializeField]
    Button launchButton;

    [SerializeField]
    int weeksUntilContract;

    [SerializeField]
    int totalExpenses;

    void OnEnable()
    {
        int loadedMechs = 0;
        Career career = GlobalDataManager.Instance.currentCareer;


        foreach (MechData mechData in career.DropshipMechs)
        {
            if (mechData != null)
                loadedMechs++;
        }

        loadedMechsValueText.text = loadedMechs.ToString();

        dropshipTravelValueText.text = StaticHelper.FormatMoney(150000);

        fundsValueText.text = StaticHelper.FormatMoney(career.funds);

        if (loadedMechs > 0)
        {
            loadedMechsValueText.color = Color.white;
        }
        else
        {
            loadedMechsValueText.color = Color.red;
        }

        if (career.currentContract != null)
        {
            ContractData contractData = career.currentContract;

            weeksUntilContract = career.gameDate.WeeksGreater(contractData.StartDate);


            currentContractNameText.text = contractData.ContractDefinition.GetDisplayName();
            currentContractNameText.color = Color.white;

            weeksUntilContractValueText.text = weeksUntilContract.ToString();

            contractTravelCoverageValueText.text = StaticHelper.FormatMoney(contractData.TravelCoverage);

            int weeklyExpenses = (weeksUntilContract + 1) * career.WeeklyExpenses;

            weeklyExpensesValueText.text = StaticHelper.FormatMoney(weeklyExpenses);

            int travelCost = Mathf.Max(150000 - contractData.TravelCoverage, 0);
            totalExpenses = travelCost + weeklyExpenses;

            totalExpensesValueText.text = StaticHelper.FormatMoney(totalExpenses);

            int balance = career.funds - totalExpenses;

            balanceValueText.text = StaticHelper.FormatMoney(balance);

            if (balance < 0)
            {
                balanceValueText.color = Color.red;
            }
            else
            {
                balanceValueText.color = Color.white;
            }

            launchButton.interactable = loadedMechs > 0 && balance >= 0;
        }
        else
        {
            launchButton.interactable = false;

            currentContractNameText.text = "None";
            currentContractNameText.color = Color.red;

            weeksUntilContractValueText.text = "N/A";

            contractTravelCoverageValueText.text = "N/A";

            weeklyExpensesValueText.text = "N/A";

            totalExpensesValueText.text = "N/A";

            balanceValueText.text = "N/A";
            balanceValueText.color = Color.white;
        }
    }

    public void ClickLaunchButton()
    {
        AudioManager.Instance.PlayButtonClick(1);

        ResourceManager.Instance.StoreCareer(GlobalDataManager.Instance.currentCareer.CareerSave);

        GlobalDataManager.Instance.currentCareer.funds -= totalExpenses;

        GlobalDataManager.Instance.currentCareer.gameDate.AddDays(7 * (weeksUntilContract + 1));

        LoadingScreen.Instance.LoadScene("Dropship");
    }
}