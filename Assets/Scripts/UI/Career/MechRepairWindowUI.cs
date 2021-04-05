using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MechRepairWindowUI : MonoBehaviour
{
    [SerializeField]
    Image headDamageImage;

    [SerializeField]
    Image torsoCenterDamageImage;

    [SerializeField]
    Image torsoLeftDamageImage;

    [SerializeField]
    Image torsoRightDamageImage;

    [SerializeField]
    Image armLeftDamageImage;

    [SerializeField]
    Image armRightDamageImage;

    [SerializeField]
    Image legLeftDamageImage;

    [SerializeField]
    Image legRightDamageImage;

    [SerializeField]
    Text mechStatusValueText;

    [SerializeField]
    Button acceptRepairsButton;

    [SerializeField]
    Toggle headRepairToggle;

    [SerializeField]
    Toggle torsoCenterRepairToggle;

    [SerializeField]
    Toggle torsoLeftRepairToggle;

    [SerializeField]
    Toggle torsoRightRepairToggle;

    [SerializeField]
    Toggle armLeftRepairToggle;

    [SerializeField]
    Toggle armRightRepairToggle;

    [SerializeField]
    Toggle legLeftRepairToggle;

    [SerializeField]
    Toggle legRightRepairToggle;

    [SerializeField]
    Text headRepairText;

    [SerializeField]
    Text torsoCenterRepairText;

    [SerializeField]
    Text torsoLeftRepairText;

    [SerializeField]
    Text torsoRightRepairText;

    [SerializeField]
    Text armLeftRepairText;

    [SerializeField]
    Text armRightRepairText;

    [SerializeField]
    Text legLeftRepairText;

    [SerializeField]
    Text legRightRepairText;

    [SerializeField]
    Text fundsValueText;

    [SerializeField]
    Text repairCostValueText;

    [SerializeField]
    Text fundsBalanceValueText;

    [SerializeField]
    int headRepairCost;

    [SerializeField]
    int torsoCenterRepairCost;

    [SerializeField]
    int torsoLeftRepairCost;

    [SerializeField]
    int torsoRightRepairCost;

    [SerializeField]
    int armLeftRepairCost;

    [SerializeField]
    int armRightRepairCost;

    [SerializeField]
    int legLeftRepairCost;

    [SerializeField]
    int legRightRepairCost;

    [SerializeField]
    int componentReplaceCost;

    [SerializeField]
    int repairTotalCost;

    public delegate void CallBack();
    public CallBack callBack;

    public MechData MechData { get; private set; }

    public void SetMech(MechData mechData)
    {
        MechData = mechData;

        Career career = GlobalDataManager.Instance.currentCareer;

        acceptRepairsButton.interactable = false;

        fundsValueText.text = StaticHelper.FormatMoney(career.funds);
        repairCostValueText.text = StaticHelper.FormatMoney(0);
        fundsBalanceValueText.text = StaticHelper.FormatMoney(career.funds);

        headRepairToggle.SetIsOnWithoutNotify(false);
        torsoCenterRepairToggle.SetIsOnWithoutNotify(false);
        torsoLeftRepairToggle.SetIsOnWithoutNotify(false);
        torsoRightRepairToggle.SetIsOnWithoutNotify(false);
        armLeftRepairToggle.SetIsOnWithoutNotify(false);
        armRightRepairToggle.SetIsOnWithoutNotify(false);
        legLeftRepairToggle.SetIsOnWithoutNotify(false);
        legRightRepairToggle.SetIsOnWithoutNotify(false);

        UpdateMechInfo();
    }

    void UpdateMechInfo()
    {
        Career career = GlobalDataManager.Instance.currentCareer;

        headDamageImage.color = BaseDamageDisplay.GetInternalHealthColor(MechData.InternalPercentHead);
        torsoCenterDamageImage.color = BaseDamageDisplay.GetInternalHealthColor(MechData.InternalPercentTorsoCenter);
        torsoLeftDamageImage.color = BaseDamageDisplay.GetInternalHealthColor(MechData.InternalPercentTorsoLeft);
        torsoRightDamageImage.color = BaseDamageDisplay.GetInternalHealthColor(MechData.InternalPercentTorsoRight);
        armLeftDamageImage.color = BaseDamageDisplay.GetInternalHealthColor(MechData.InternalPercentArmLeft);
        armRightDamageImage.color = BaseDamageDisplay.GetInternalHealthColor(MechData.InternalPercentArmRight);
        legLeftDamageImage.color = BaseDamageDisplay.GetInternalHealthColor(MechData.InternalPercentLegLeft);
        legRightDamageImage.color = BaseDamageDisplay.GetInternalHealthColor(MechData.InternalPercentLegRight);

        MechStatusType mechStatusType = MechData.MechStatus;

        mechStatusValueText.text = StaticHelper.GetMechStatusName(mechStatusType);
        mechStatusValueText.color = StaticHelper.GetMechStatusColor(mechStatusType);

        float valuePerInternal = MechData.MechChassis.ValuePerInternal;

        headRepairCost = Mathf.CeilToInt(MechData.HeadDamage * valuePerInternal * career.repairCostModifier);
        torsoCenterRepairCost = Mathf.CeilToInt(MechData.TorsoCenterDamage * valuePerInternal * career.repairCostModifier);
        torsoLeftRepairCost = Mathf.CeilToInt(MechData.TorsoLeftDamage * valuePerInternal * career.repairCostModifier);
        torsoRightRepairCost = Mathf.CeilToInt(MechData.TorsoRightDamage * valuePerInternal * career.repairCostModifier);
        armLeftRepairCost = Mathf.CeilToInt(MechData.ArmLeftDamage * valuePerInternal * career.repairCostModifier);
        armRightRepairCost = Mathf.CeilToInt(MechData.ArmRightDamage * valuePerInternal * career.repairCostModifier);
        legLeftRepairCost = Mathf.CeilToInt(MechData.LegLeftDamage * valuePerInternal * career.repairCostModifier);
        legRightRepairCost = Mathf.CeilToInt(MechData.LegRightDamage * valuePerInternal * career.repairCostModifier);

        SetToggleAndText(MechData.HeadDamage > 0, headRepairToggle, headRepairText);
        SetToggleAndText(MechData.TorsoCenterDamage > 0, torsoCenterRepairToggle, torsoCenterRepairText);
        SetToggleAndText(MechData.TorsoLeftDamage > 0, torsoLeftRepairToggle, torsoLeftRepairText);
        SetToggleAndText(MechData.TorsoRightDamage > 0, torsoRightRepairToggle, torsoRightRepairText);
        SetToggleAndText(MechData.ArmLeftDamage > 0, armLeftRepairToggle, armLeftRepairText);
        SetToggleAndText(MechData.ArmRightDamage > 0, armRightRepairToggle, armRightRepairText);
        SetToggleAndText(MechData.LegLeftDamage > 0, legLeftRepairToggle, legLeftRepairText);
        SetToggleAndText(MechData.LegRightDamage > 0, legRightRepairToggle, legRightRepairText);

        headRepairText.text = "Repair Head: " + StaticHelper.FormatMoney(headRepairCost);
        torsoCenterRepairText.text = "Repair Center Torso: " + StaticHelper.FormatMoney(torsoCenterRepairCost);
        torsoLeftRepairText.text = "Repair Left Torso: " + StaticHelper.FormatMoney(torsoLeftRepairCost);
        torsoRightRepairText.text = "Repair Right Torso: " + StaticHelper.FormatMoney(torsoRightRepairCost);
        armLeftRepairText.text = "Repair Left Arm: " + StaticHelper.FormatMoney(armLeftRepairCost);
        armRightRepairText.text = "Repair Right Arm: " + StaticHelper.FormatMoney(armRightRepairCost);
        legLeftRepairText.text = "Repair Left Leg: " + StaticHelper.FormatMoney(legLeftRepairCost);
        legRightRepairText.text = "Repair Right Leg: " + StaticHelper.FormatMoney(legRightRepairCost);

        List<ComponentDefinition> componentsDestroyed = MechData.ComponentsDestroyed;

        Dictionary<ComponentDefinition, int> componentsDestroyedDict = new Dictionary<ComponentDefinition, int>();

        foreach (ComponentDefinition component in componentsDestroyed)
        {
            if (componentsDestroyedDict.ContainsKey(component))
            {
                componentsDestroyedDict[component]++;
            }
            else
            {
                componentsDestroyedDict.Add(component, 1);
            }
        }

        componentReplaceCost = 0;

        foreach (KeyValuePair<ComponentDefinition, int> componentEntry in componentsDestroyedDict)
        {
            int inventoryCount = career.inventory.GetComponentCount(componentEntry.Key);

            if (componentEntry.Value < inventoryCount)
            {
                int difference = componentEntry.Value - inventoryCount;

                componentReplaceCost += difference * Mathf.CeilToInt(componentEntry.Key.MarketValue * 1.25f);
            }
        }
    }

    public void UpdateSelection()
    {
        AudioManager.Instance.PlayButtonClick(0);

        Career career = GlobalDataManager.Instance.currentCareer;

        repairTotalCost = 0;

        if (headRepairToggle.isOn)
            repairTotalCost += headRepairCost;

        if (torsoCenterRepairToggle.isOn)
            repairTotalCost += torsoCenterRepairCost;

        if (torsoLeftRepairToggle.isOn)
            repairTotalCost += torsoLeftRepairCost;

        if (torsoRightRepairToggle.isOn)
            repairTotalCost += torsoRightRepairCost;

        if (armLeftRepairToggle.isOn)
            repairTotalCost += armLeftRepairCost;

        if (armRightRepairToggle.isOn)
            repairTotalCost += armRightRepairCost;

        if (legLeftRepairToggle.isOn)
            repairTotalCost += legLeftRepairCost;

        if (legRightRepairToggle.isOn)
            repairTotalCost += legRightRepairCost;

        int balance = GlobalDataManager.Instance.currentCareer.funds - repairTotalCost;

        fundsValueText.text = StaticHelper.FormatMoney(career.funds);
        repairCostValueText.text = StaticHelper.FormatMoney(repairTotalCost);
        fundsBalanceValueText.text = StaticHelper.FormatMoney(balance);

        if (balance < 0)
        {
            fundsBalanceValueText.color = Color.red;
        }
        else
        {
            fundsBalanceValueText.color = Color.white;
        }

        acceptRepairsButton.interactable = balance > 0 && (headRepairToggle.isOn || torsoCenterRepairToggle.isOn || torsoLeftRepairToggle.isOn || torsoRightRepairToggle.isOn || armLeftRepairToggle.isOn || armRightRepairToggle.isOn || legLeftRepairToggle.isOn || legRightRepairToggle.isOn);
    }

    void SetToggleAndText(bool state, Toggle toggle, Text text)
    {
        if (state)
        {
            toggle.interactable = true;
            text.color = Color.white;
        }
        else
        {
            toggle.interactable = false;
            text.color = Color.grey;
        }
    }

    public void ClickAcceptRepairsButton()
    {
        AudioManager.Instance.PlayButtonClick(0);

        acceptRepairsButton.interactable = false;

        Career career = GlobalDataManager.Instance.currentCareer;

        GlobalDataManager.Instance.currentCareer.funds -= repairTotalCost;

        MechData.RepairIntenals(headRepairToggle.isOn, torsoCenterRepairToggle.isOn, torsoLeftRepairToggle.isOn, torsoRightRepairToggle.isOn, armLeftRepairToggle.isOn, armRightRepairToggle.isOn, legLeftRepairToggle.isOn, legRightRepairToggle.isOn);

        headRepairToggle.SetIsOnWithoutNotify(false);
        torsoCenterRepairToggle.SetIsOnWithoutNotify(false);
        torsoLeftRepairToggle.SetIsOnWithoutNotify(false);
        torsoRightRepairToggle.SetIsOnWithoutNotify(false);
        armLeftRepairToggle.SetIsOnWithoutNotify(false);
        armRightRepairToggle.SetIsOnWithoutNotify(false);
        legLeftRepairToggle.SetIsOnWithoutNotify(false);
        legRightRepairToggle.SetIsOnWithoutNotify(false);

        fundsValueText.text = StaticHelper.FormatMoney(career.funds);
        repairCostValueText.text = StaticHelper.FormatMoney(0);
        fundsBalanceValueText.text = StaticHelper.FormatMoney(career.funds);

        UpdateMechInfo();
    }

    public void ClickCloseButton()
    {
        AudioManager.Instance.PlayButtonClick(0);

        gameObject.SetActive(false);

        callBack();
    }
}
