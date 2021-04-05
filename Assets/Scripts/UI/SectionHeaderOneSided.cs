using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SectionHeaderOneSided : MonoBehaviour
{
    [SerializeField]
    protected UnitCustomizationScreen unitCustomizationScreen;

    [SerializeField]
    protected SectionSlotGroupsUI sectionSlotGroupsUI;

    [SerializeField]
    protected Image ammoIndicator;

    [SerializeField]
    protected ArmorTypeDropdown armorTypeDropdown;

    [SerializeField]
    protected Button armorIncreaseButton;

    [SerializeField]
    protected Button armorDecreaseButton;

    [SerializeField]
    protected Text armorValueText;

    [SerializeField]
    protected Text armorTonsValueText;

    [SerializeField]
    protected Text internalValueText;

    [SerializeField]
    protected ArmorType armorType;

    [SerializeField]
    protected int armorCurrent;

    [SerializeField]
    protected int armorMax;

    [SerializeField]
    protected float internalBase;

    [SerializeField]
    protected bool holdIncreaseArmor;

    [SerializeField]
    protected bool holdDecreaseArmor;

    [SerializeField]
    protected float holdTimer;

    [SerializeField]
    protected bool heldChanged;

    public ArmorType ArmorType { get => armorType; }

    public int ArmorPoints { get => armorCurrent; }

    public Text InternalValueText { get => internalValueText; }

    public bool HasAmmoIndicator { get => ammoIndicator != null; }

    protected virtual void Awake()
    {
        armorValueText.text = "0/0";
        armorTonsValueText.text = "0.0";
        internalValueText.text = "0";

        armorIncreaseButton.interactable = false;
        armorDecreaseButton.interactable = false;
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
	}

    public virtual void SetBaseValues(int armorLimit, float chassisInternalBase)
    {
        armorCurrent = 0;
        armorMax = armorLimit;
        internalBase = chassisInternalBase;

        internalValueText.text = internalBase.ToString("0.#");

        UpdateArmorButtons();
        UpdateArmorText();
    }

    public void ChangeArmorType(ArmorType newArmorType, int value)
    {
        armorType = newArmorType;

        armorCurrent = value;

        UpdateArmorButtons();
        UpdateArmorText();

        armorTypeDropdown.SetLabel(armorType);
    }

    public virtual void UpdateArmorText()
    {
        armorValueText.text = armorCurrent.ToString() + "/" + armorMax.ToString();
        armorTonsValueText.text = GetArmorWeight().ToString("0.0#");
    }

    public virtual float GetArmorWeight()
    {
        return armorCurrent * ResourceManager.Instance.GameConstants.GetArmorWeight(armorType);
    }

    public virtual void UpdateArmorButtons()
    {
        armorDecreaseButton.interactable = armorCurrent > 0;
        armorIncreaseButton.interactable = armorCurrent < armorMax;
    }

    protected virtual void IncreaseArmor()
    {
        armorCurrent++;
        unitCustomizationScreen.RecalculateArmor();
        unitCustomizationScreen.UpdateArmorButtons();
        UpdateArmorText();
    }

    protected void DecreaseArmor()
    {
        armorCurrent--;
        unitCustomizationScreen.RecalculateArmor();
        unitCustomizationScreen.UpdateArmorButtons();
        UpdateArmorText();
    }

    public virtual void ClickIncreaseArmor()
    {
        if (heldChanged)
        {
            return;
        }

        if (armorCurrent < armorMax)
        {
            IncreaseArmor();
        }

        AudioManager.Instance.PlayButtonClick(0);
    }

    public void ClickDecreaseArmor()
    {
        if (heldChanged)
        {
            return;
        }

        if (armorCurrent > 0)
        {
            DecreaseArmor();
        }

        AudioManager.Instance.PlayButtonClick(0);
    }

    public void StartIncreasingArmor()
    {
        holdIncreaseArmor = true;
        holdTimer = Time.unscaledTime + 0.3f;
        heldChanged = false;
    }

    public void StartDecreasingArmor()
    {
        holdDecreaseArmor = true;
        holdTimer = Time.unscaledTime + 0.3f;
        heldChanged = false;
    }

    public void StopIncreasingArmor()
    {
        holdIncreaseArmor = false;
    }

    public void StopDecreasingArmor()
    {
        holdDecreaseArmor = false;
    }

    public virtual void MaxArmor()
    {
        while (armorCurrent < armorMax)
        {
            armorCurrent++;
            unitCustomizationScreen.RecalculateArmor();
        }
    }

    public void SetAmmoIndicatorColor(Color color)
    {
        if (ammoIndicator != null)
        {
            ammoIndicator.color = color;
        }
    }
}
