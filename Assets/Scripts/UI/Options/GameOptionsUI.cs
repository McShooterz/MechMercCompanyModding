using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOptionsUI : MonoBehaviour
{
    #region Variables


    [Header("Career Difficulty")]

    [SerializeField]
    GameObject careerDifficultyGroup;

    [SerializeField]
    Slider contractPaySlider;

    [SerializeField]
    Slider missionPaySlider;

    [SerializeField]
    Slider bountyPaySlider;

    [SerializeField]
    Slider repairCostsSlider;

    [SerializeField]
    Text contractPayValueText;

    [SerializeField]
    Text missionPayValueText;

    [SerializeField]
    Text bountyPayValueText;

    [SerializeField]
    Text repairCostsValueText;

    [Header("Cheats")]

    [SerializeField]
    Toggle cheatGodModeToggle;

    [SerializeField]
    Toggle cheatNoHeatToggle;

    [SerializeField]
    Toggle cheatUnlimitedAmmoToggle;

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnEnable()
    {
        Career career = GlobalDataManager.Instance.currentCareer;

        if (career.IsReal)
        {
            careerDifficultyGroup.SetActive(true);

            contractPaySlider.SetValueWithoutNotify((float)System.Math.Round(career.contractPayModifier, 2));

            missionPaySlider.SetValueWithoutNotify((float)System.Math.Round(career.missionPayModifier, 2));

            bountyPaySlider.SetValueWithoutNotify((float)System.Math.Round(career.bountyPayModifier, 2));

            repairCostsSlider.SetValueWithoutNotify((float)System.Math.Round(career.repairCostModifier, 2));

            contractPayValueText.text = (contractPaySlider.value * 100).ToString("0.") + "%";

            missionPayValueText.text = (missionPaySlider.value * 100).ToString("0.") + "%";

            bountyPayValueText.text = (bountyPaySlider.value * 100).ToString("0.") + "%";

            repairCostsValueText.text = (repairCostsSlider.value * 100).ToString("0.") + "%";
        }
        else
        {
            careerDifficultyGroup.SetActive(false);
        }

        cheatGodModeToggle.SetIsOnWithoutNotify(Cheats.godMode);

        cheatNoHeatToggle.SetIsOnWithoutNotify(Cheats.noHeat);

        cheatUnlimitedAmmoToggle.SetIsOnWithoutNotify(Cheats.unlimitedAmmo);
    }

    public void SaveCareerOptions()
    {
        Career career = GlobalDataManager.Instance.currentCareer;

        if (career.IsReal)
        {
            career.contractPayModifier = contractPaySlider.value;

            career.missionPayModifier = missionPaySlider.value;

            career.bountyPayModifier = bountyPaySlider.value;

            career.repairCostModifier = repairCostsSlider.value;
        }
    }

    public void OnValueChangedContractPay()
    {
        contractPaySlider.SetValueWithoutNotify((float)System.Math.Round(contractPaySlider.value, 2));

        contractPayValueText.text = (contractPaySlider.value * 100).ToString("0.") + "%";
    }

    public void OnValueChangedMissionPay()
    {
        missionPaySlider.SetValueWithoutNotify((float)System.Math.Round(missionPaySlider.value, 2));

        missionPayValueText.text = (missionPaySlider.value * 100).ToString("0.") + "%";
    }

    public void OnValueChangedBountyPay()
    {
        bountyPaySlider.SetValueWithoutNotify((float)System.Math.Round(bountyPaySlider.value, 2));

        bountyPayValueText.text = (bountyPaySlider.value * 100).ToString("0.") + "%";
    }

    public void OnValueChangedRepairCosts()
    {
        repairCostsSlider.SetValueWithoutNotify((float)System.Math.Round(repairCostsSlider.value, 2));

        repairCostsValueText.text = (repairCostsSlider.value * 100).ToString("0.") + "%";
    }

    public void OnCheatChangedGodMode()
    {
        Cheats.godMode = cheatGodModeToggle.isOn;
    }

    public void OnCheatChangedNoHeat()
    {
        Cheats.noHeat = cheatNoHeatToggle.isOn;
    }

    public void OnCheatChangeUnlimitedAmmo()
    {
        Cheats.unlimitedAmmo = cheatUnlimitedAmmoToggle.isOn;
    }
}
