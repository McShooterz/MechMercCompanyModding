using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MechMarketEntryUI : MonoBehaviour
{
    [SerializeField]
    MechMarketListUI mechMarketListUI;

    [SerializeField]
    LayoutElement layoutElement;

    [SerializeField]
    Image background;

    [SerializeField]
    MechIconDamageUI mechIconDamageUI;

    [SerializeField]
    Text mechNameText;

    [SerializeField]
    Text mechValueText;

    [SerializeField]
    Text countText;

    [SerializeField]
    int index;

    public LayoutElement LayoutElement { get => layoutElement; }

    public Image Background { get => background; }

    public void Initialize(MechMarketEntry mechMarketEntry, int index)
    {
        mechNameText.text = mechMarketEntry.MechDesign.DesignName + "-" + mechMarketEntry.MechDesign.GetMechChassisDefinition().UnitClassDisplay.ToUpper();

        mechValueText.text = StaticHelper.FormatMoney(Mathf.CeilToInt(mechMarketEntry.MechDesign.GetMarketValue() * 1.25f));

        countText.text = mechMarketEntry.count.ToString();

        mechIconDamageUI.SetMech(null);

        this.index = index;
    }


    public void ClickButton()
    {
        AudioManager.Instance.PlayButtonClick(0);

        mechMarketListUI.SelectIndex(index);
    }
}
