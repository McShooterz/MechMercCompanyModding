using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MechHangarEntryUI : MonoBehaviour
{
    [SerializeField]
    MechHangarListUI mechHangarListUI;

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

    public Image Background { get => background; }

    public void Initialize(MechData mechData, int index)
    {
        mechIconDamageUI.SetMech(mechData);

        mechNameText.text = mechData.customName;

        MechStatusType mechStatusType = mechData.MechStatus;

        mechStatusText.text = StaticHelper.GetMechStatusName(mechStatusType);

        mechStatusText.color = StaticHelper.GetMechStatusColor(mechStatusType);

        this.index = index;
    }

    public void ClickButton()
    {
        AudioManager.Instance.PlayButtonClick(0);

        mechHangarListUI.SelectIndex(index);
    }
}
