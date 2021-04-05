using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SectionHeaderTwoSided : SectionHeaderOneSided
{
    [SerializeField]
    Button armorIncreaseRearButton;

    [SerializeField]
    Button armorDecreaseRearButton;

    [SerializeField]
    Text armorRearValueText;

    [SerializeField]
    int armorRearCurrent;

    [SerializeField]
    bool holdIncreaseArmorRear;

    [SerializeField]
    bool holdDecreaseArmorRear;

    public int ArmorPointsRear
    {
        get
        {
            return armorRearCurrent;
        }
    }

    public int TotalArmor
    {
        get
        {
            return armorCurrent + armorRearCurrent;
        }
    }

    protected override void Awake()
    {
        base.Awake();

        armorRearValueText.text = "-/-";

        armorIncreaseRearButton.interactable = false;
        armorDecreaseRearButton.interactable = false;
    }

    // Use this for initialization
    void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (holdIncreaseArmor)
        {
            if (Time.unscaledTime > holdTimer)
            {
                if (armorCurrent < armorMax)
                {
                    IncreaseArmor();
                    holdTimer = Time.unscaledTime + 0.1f;
                    heldChanged = true;
                }
                else
                {
                    unitCustomizationScreen.UpdateArmorButtons();
                    holdIncreaseArmor = false;
                }
            }
        }
        else if (holdDecreaseArmor)
        {
            if (Time.unscaledTime > holdTimer)
            {
                if (armorCurrent > 0)
                {
                    DecreaseArmor();
                    holdTimer = Time.unscaledTime + 0.1f;
                    heldChanged = true;
                }
                else
                {
                    unitCustomizationScreen.UpdateArmorButtons();
                    holdDecreaseArmor = false;
                }
            }
        }
        else if (holdIncreaseArmorRear)
        {
            if (Time.unscaledTime > holdTimer)
            {
                if (armorRearCurrent < armorMax)
                {
                    IncreaseArmorRear();
                    holdTimer = Time.unscaledTime + 0.1f;
                    heldChanged = true;
                }
                else
                {
                    unitCustomizationScreen.UpdateArmorButtons();
                    holdIncreaseArmorRear = false;
                }
            }
        }
        else if (holdDecreaseArmorRear)
        {
            if (Time.unscaledTime > holdTimer)
            {
                if (armorRearCurrent > 0)
                {
                    DecreaseArmorRear();
                    holdTimer = Time.unscaledTime + 0.1f;
                    heldChanged = true;
                }
                else
                {
                    unitCustomizationScreen.UpdateArmorButtons();
                    holdDecreaseArmorRear = false;
                }
            }
        }
    }

    public override void SetBaseValues(int armorLimit, float chassisInternalBase)
    {
        base.SetBaseValues(armorLimit, chassisInternalBase);

        armorRearCurrent = 0;

        UpdateArmorButtons();

        UpdateArmorText();
    }

    public void ChangeArmorType(ArmorType newArmorType, int frontValue, int rearValue)
    {
        armorType = newArmorType;

        armorCurrent = frontValue;
        armorRearCurrent = rearValue;

        UpdateArmorButtons();

        UpdateArmorText();

        armorTypeDropdown.SetLabel(armorType);
    }

    public override float GetArmorWeight()
    {
        return TotalArmor * ResourceManager.Instance.GameConstants.GetArmorWeight(armorType);
    }

    public override void UpdateArmorText()
    {
        armorValueText.text = armorCurrent.ToString() + "/" + armorMax.ToString();
        armorRearValueText.text = armorRearCurrent.ToString() + "/" + armorMax.ToString();
        armorTonsValueText.text = GetArmorWeight().ToString("0.0#");
    }

    public override void UpdateArmorButtons()
    {
        armorDecreaseButton.interactable = armorCurrent > 0;
        armorIncreaseButton.interactable = armorCurrent < armorMax;

        armorDecreaseRearButton.interactable = armorRearCurrent > 0;
        armorIncreaseRearButton.interactable = armorRearCurrent < armorMax;
    }

    protected override void IncreaseArmor()
    {
        if (TotalArmor < armorMax)
        {
            armorCurrent++;
        }
        else if (armorCurrent < armorMax && armorRearCurrent > 0)
        {
            armorCurrent++;
            armorRearCurrent--;
        }
        else
        {
            return;
        }

        unitCustomizationScreen.RecalculateArmor();
        unitCustomizationScreen.UpdateArmorButtons();
        UpdateArmorText();
    }

    void IncreaseArmorRear()
    {
        if (TotalArmor < armorMax)
        {
            armorRearCurrent++;
        }
        else if (armorRearCurrent < armorMax && armorCurrent > 0)
        {
            armorRearCurrent++;
            armorCurrent--;
        }
        else
        {
            return;
        }

        unitCustomizationScreen.RecalculateArmor();
        unitCustomizationScreen.UpdateArmorButtons();
        UpdateArmorText();
    }

    void DecreaseArmorRear()
    {
        armorRearCurrent--;
        unitCustomizationScreen.RecalculateArmor();
        unitCustomizationScreen.UpdateArmorButtons();
        UpdateArmorText();
    }

    public override void ClickIncreaseArmor()
    {
        if (heldChanged)
        {
            return;
        }

        IncreaseArmor();

        AudioManager.Instance.PlayButtonClick(0);
    }

    public void ClickIncreaseArmorRear()
    {
        if (heldChanged)
        {
            return;
        }

        IncreaseArmorRear();

        AudioManager.Instance.PlayButtonClick(0);
    }

    public void ClickDecreaseArmorRear()
    {
        if (heldChanged)
        {
            return;
        }

        if (armorRearCurrent > 0)
        {
            DecreaseArmorRear();
        }

        AudioManager.Instance.PlayButtonClick(0);
    }

    public void StartIncreasingArmorRear()
    {
        holdIncreaseArmorRear = true;
        holdTimer = Time.unscaledTime + 0.3f;
        heldChanged = false;
    }

    public void StartDecreasingArmorRear()
    {
        holdDecreaseArmorRear = true;
        holdTimer = Time.unscaledTime + 0.3f;
        heldChanged = false;
    }

    public void StopIncreasingArmorRear()
    {
        holdIncreaseArmorRear = false;
    }

    public void StopDecreasingArmorRear()
    {
        holdDecreaseArmorRear = false;
    }

    public override void MaxArmor()
    {
        while (armorCurrent + armorRearCurrent < armorMax)
        {
            armorCurrent++;
            unitCustomizationScreen.RecalculateArmor();
        }
    }
}
