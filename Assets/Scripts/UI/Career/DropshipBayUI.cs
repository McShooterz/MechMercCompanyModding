using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DropshipBayUI : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    Image weightIconImage;

    [SerializeField]
    Text weightText;

    [SerializeField]
    GameObject mechFillObject;

    [SerializeField]
    MechIconDamageUI mechIconDamageUI;

    [SerializeField]
    Text mechnameText;

    [SerializeField]
    Text mechStatusText;

    [SerializeField]
    Color defaultColor;

    [SerializeField]
    Color activeColor;

    [SerializeField]
    Color warningColor;

    [SerializeField]
    int tonnageLimit;

    public delegate void CallBackSelect(MechData mechData);
    public CallBackSelect callBackSelect;

    public delegate void CallBackHover(DropshipBayUI dropshipBayUI);
    public CallBackHover callBackHover;

    public MechData MechData { get; private set; }

    public void SetMechData(MechData mechData)
    {
        MechData = mechData;

        if (mechData != null)
        {
            mechFillObject.SetActive(true);

            mechIconDamageUI.SetMech(mechData);

            weightIconImage.color = activeColor;

            weightText.text = mechData.MechChassis.Tonnage.ToString() + "/100T";

            mechnameText.text = mechData.customName;

            MechStatusType mechStatusType = mechData.MechStatus;

            mechStatusText.text = StaticHelper.GetMechStatusName(mechStatusType);

            mechStatusText.color = StaticHelper.GetMechStatusColor(mechStatusType);
        }
        else
        {
            mechFillObject.SetActive(false);

            weightIconImage.color = defaultColor;

            weightText.text = "0/100T";
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        callBackHover(this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        callBackHover(null);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (MechData != null)
        {
            callBackSelect(MechData);

            SetMechData(null);
        }
    }
}
