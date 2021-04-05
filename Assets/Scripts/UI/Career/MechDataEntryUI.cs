using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MechDataEntryUI : MonoBehaviour
{
    [SerializeField]
    Image background;

    [SerializeField]
    MechIconDamageUI mechIconDamageUI;

    [SerializeField]
    Text mechNameText;

    [SerializeField]
    Text mechStatusText;

    [SerializeField]
    int index;

    public delegate void CallBackSelect(int index);
    public CallBackSelect callBackSelect;

    public Image Background { get => background; }

    public void Initialize(MechData mechData, int index, CallBackSelect callBack)
    {
        mechIconDamageUI.SetMech(mechData);

        mechNameText.text = mechData.customName;

        MechStatusType mechStatusType = mechData.MechStatus;

        mechStatusText.text = StaticHelper.GetMechStatusName(mechStatusType);

        mechStatusText.color = StaticHelper.GetMechStatusColor(mechStatusType);

        this.index = index;

        callBackSelect = callBack;
    }

    public void ClickButton()
    {
        AudioManager.Instance.PlayButtonClick(0);

        callBackSelect(index);
    }
}
