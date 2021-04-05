using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ComponentMenuItem : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    LayoutElement layoutElement;

    [SerializeField]
    Image backgroundImage;

    [SerializeField]
    Text nameText;

    [SerializeField]
    Text slotText;

    [SerializeField]
    Text tonsText;

    [SerializeField]
    Text amountText;

    [SerializeField]
    Text valueText;

    Inventory targetInventory;

    public delegate void CallBackSelect(ComponentDefinition componentDefinition);
    public CallBackSelect callBackSelect;

    public delegate void CallBackHover(ComponentDefinition componentDefinition);
    public CallBackHover callBackHover;

    public ComponentDefinition ComponentDefinition { get; private set; }

    public Inventory TargetInventory { set => targetInventory = value; }

    public LayoutElement LayoutElement { get => layoutElement; }

    public void Initialize(KeyValuePair<ComponentDefinition, int> pair, CallBackSelect select, CallBackHover hover)
    {
        ComponentDefinition = pair.Key;

        string dispalyName = ComponentDefinition.GetDisplayName();

        gameObject.name = dispalyName;
        backgroundImage.color = ComponentDefinition.Color;
        nameText.text = dispalyName;
        slotText.text = ComponentDefinition.SlotSize.ToString();
        tonsText.text = ComponentDefinition.Weight.ToString("0.##");
        nameText.color = ComponentDefinition.TextColor;
        slotText.color = ComponentDefinition.TextColor;
        tonsText.color = ComponentDefinition.TextColor;
        amountText.color = ComponentDefinition.TextColor;
        amountText.text = pair.Value.ToString();

        valueText.text = "";

        callBackSelect = select;
        callBackHover = hover;
    }

    public void InitializeMarket(KeyValuePair<ComponentDefinition, int> pair, CallBackSelect select, CallBackHover hover)
    {
        ComponentDefinition = pair.Key;

        string dispalyName = ComponentDefinition.GetDisplayName();

        gameObject.name = dispalyName;
        backgroundImage.color = ComponentDefinition.Color;
        nameText.text = dispalyName;
        slotText.text = "";
        tonsText.text = "";
        nameText.color = ComponentDefinition.TextColor;
        amountText.color = ComponentDefinition.TextColor;
        amountText.text = pair.Value.ToString();

        valueText.text = StaticHelper.FormatMoney(Mathf.CeilToInt(ComponentDefinition.MarketValue * 1.25f)).ToString();
        valueText.color = ComponentDefinition.TextColor;

        callBackSelect = select;
        callBackHover = hover;
    }

    public void RefreshCount()
    {
        amountText.text = targetInventory.GetComponentCount(ComponentDefinition).ToString();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        callBackSelect(ComponentDefinition);
        RefreshCount();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        callBackHover(ComponentDefinition);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        callBackHover(null);
    }
}
