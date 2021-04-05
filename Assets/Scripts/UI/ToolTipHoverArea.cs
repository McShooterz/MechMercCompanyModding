using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ToolTipHoverArea : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    string toolTipKey = "";

    public void OnPointerEnter(PointerEventData eventData)
    {
        ToolTipDisplay.Instance.SetText(ResourceManager.Instance.GetLocalization(toolTipKey));
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ToolTipDisplay.Instance.Clear();
    }
}
