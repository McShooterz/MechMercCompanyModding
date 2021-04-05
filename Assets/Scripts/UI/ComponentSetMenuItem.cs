using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ComponentSetMenuItem : MonoBehaviour, IPointerDownHandler
{
    [SerializeField]
    LayoutElement layoutElement;

    [SerializeField]
    Image backgroundImage;

    [SerializeField]
    Text nameText;

    [SerializeField]
    Text expandText;

    [SerializeField]
    ComponentMenuItem[] componentMenuItems;

    [SerializeField]
    bool callapsed = false;

    public ComponentMenuItem[] ComponentMenuItems { get => componentMenuItems; }

    public ComponentSet ComponentSet { get; private set; }

    public LayoutElement LayoutElement { get => layoutElement; }

    public bool IsCallapsed { get => callapsed; }

    public void SetComponentSet(ComponentSet componentSet)
    {
        ComponentSet = componentSet;

        string name = componentSet.GetDisplayName();

        gameObject.name = name;
        nameText.text = name;
        backgroundImage.color = componentSet.Color;
        nameText.color = componentSet.TextColor;
        expandText.color = componentSet.TextColor;
    }

    public void SetComponentMenuItems(ComponentMenuItem[] items)
    {
        componentMenuItems = items;
    }

    public void Callapse()
    {
        callapsed = true;

        foreach (ComponentMenuItem componentMenuItem in componentMenuItems)
        {
            componentMenuItem.gameObject.SetActive(false);
        }

        expandText.text = "+";
    }

    public void Expand()
    {
        callapsed = false;

        foreach (ComponentMenuItem componentMenuItem in componentMenuItems)
        {
            componentMenuItem.gameObject.SetActive(true);
            componentMenuItem.RefreshCount();
        }

        expandText.text = "-";
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (callapsed)
        {
            Expand();
        }
        else
        {
            Callapse();
        }

        AudioManager.Instance.PlayButtonClick(0);
    }
}
