using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SalvageEntryUI : MonoBehaviour
{
    [SerializeField]
    SalvageSubScreen salvageSubScreen;

    [SerializeField]
    GameObject mechInfoGameObject;

    [SerializeField]
    GameObject componentInfoGameObject;

    [SerializeField]
    MechIconDamageUI mechIconDamageUI;

    [SerializeField]
    Text mechChassisNameText;

    [SerializeField]
    Image componentBackgroundImage;

    [SerializeField]
    Text componentNameText;

    [SerializeField]
    Text countText;

    [SerializeField]
    Text marketText;

    [SerializeField]
    Text tonnageText;

    [SerializeField]
    Toggle takeToggle;

    public Toggle TakeToggle { get => takeToggle; }

    public MechData Mech { get; private set; }

    public ComponentDefinition Component { get; private set; }

    public void Initialize(MechData mechData)
    {
        Mech = mechData;

        mechInfoGameObject.SetActive(true);
        componentInfoGameObject.SetActive(false);

        mechIconDamageUI.SetMech(Mech);

        mechChassisNameText.text = Mech.MechChassis.GetDisplayName();

        countText.text = GlobalDataManager.Instance.currentCareer.GetMechChassisCount(Mech.MechChassis).ToString();

        marketText.text = StaticHelper.FormatMoney(Mech.MarketValue);

        tonnageText.text = Mech.MechChassis.Tonnage.ToString();

        takeToggle.isOn = false;

        takeToggle.onValueChanged.AddListener(delegate { salvageSubScreen.UpdateSalvage(); });
    }

    public void Initialize(ComponentDefinition componentDefinition)
    {
        Component = componentDefinition;

        mechInfoGameObject.SetActive(false);
        componentInfoGameObject.SetActive(true);

        componentBackgroundImage.color = Component.Color;

        componentNameText.text = Component.GetDisplayName();
        componentNameText.color = Component.TextColor;

        countText.text = GlobalDataManager.Instance.currentCareer.inventory.GetComponentCount(Component).ToString();

        marketText.text = StaticHelper.FormatMoney(Component.MarketValue);

        tonnageText.text = Component.Weight.ToString("0.##");

        takeToggle.isOn = false;

        takeToggle.onValueChanged.AddListener(delegate { salvageSubScreen.UpdateSalvage(); });
    }
}
