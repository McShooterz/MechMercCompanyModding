using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MechDraggableEntryUI : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    MechIconDamageUI mechIconDamageUI;

    [SerializeField]
    Text mechNameText;

    [SerializeField]
    Text mechStatusText;

    public delegate void CallBackSelect(MechData mechData);
    public CallBackSelect callBackSelect;

    public delegate void CallBackHover(MechData mechData);
    public CallBackHover callBackHover;

    public MechData MechData { get; private set; } = null;

    public void Initialize(MechData mechData, CallBackSelect callBack, CallBackHover callBackHover)
    {
        MechData = mechData;

        callBackSelect = callBack;

        this.callBackHover = callBackHover;

        SetDisplay(MechData);
    }

    public void Refresh()
    {
        if (MechData != null)
            SetDisplay(MechData);
    }

    void SetDisplay(MechData mechData)
    {
        mechIconDamageUI.SetMech(mechData);

        mechNameText.text = mechData.customName;

        MechStatusType mechStatusType = mechData.MechStatus;

        mechStatusText.text = StaticHelper.GetMechStatusName(mechStatusType);

        mechStatusText.color = StaticHelper.GetMechStatusColor(mechStatusType);
    }

    public void Clear()
    {
        MechData = null;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        gameObject.SetActive(false);

        callBackSelect(MechData);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        callBackHover?.Invoke(MechData);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        callBackHover?.Invoke(null);
    }
}
