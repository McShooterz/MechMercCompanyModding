using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FinancesSubScreen : MonoBehaviour
{
    [SerializeField]
    Text financeText;

    void OnEnable()
    {
        Career career = GlobalDataManager.Instance.currentCareer;
        System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();

        int mechsValue = career.MechsValue;
        int inventoryValue = career.inventory.InventoryValue;
        int totalCompanyValue = career.funds + mechsValue + inventoryValue;

        int mechMaintenance = career.MechMaintenance;
        int pilotSalary = career.PilotWeeklySalary;
        int totalWeeklyExpenses = 20000 + mechMaintenance + pilotSalary;

        stringBuilder.AppendLine("<size=20><b>Company Value</b></size>");
        stringBuilder.AppendLine("Funds: " + StaticHelper.FormatMoney(career.funds));
        stringBuilder.AppendLine("Mechs: " + StaticHelper.FormatMoney(mechsValue));
        stringBuilder.AppendLine("Inventory: " + StaticHelper.FormatMoney(inventoryValue));
        stringBuilder.AppendLine("Total Company Value: " + StaticHelper.FormatMoney(totalCompanyValue));

        stringBuilder.AppendLine();

        stringBuilder.AppendLine("<size=20><b>Weekly Expenses</b></size>");
        stringBuilder.AppendLine("Base Maintenance: " + StaticHelper.FormatMoney(20000));
        stringBuilder.AppendLine("Mech Maintenance: " + StaticHelper.FormatMoney(mechMaintenance));
        stringBuilder.AppendLine("Pilot Salary: " + StaticHelper.FormatMoney(pilotSalary));
        stringBuilder.AppendLine("Total Weekly Expenses: " + StaticHelper.FormatMoney(totalWeeklyExpenses));

        financeText.text = stringBuilder.ToString();
    }
}
