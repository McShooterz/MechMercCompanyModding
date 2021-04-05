using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MechDraggableUI : MonoBehaviour
{
    [SerializeField]
    MechIconDamageUI mechIconDamageUI;

    [SerializeField]
    Text mechNameText;

    [SerializeField]
    Text mechStatusText;

    public void SetMech(MechData mechData)
    {
        gameObject.SetActive(true);

        mechIconDamageUI.SetMech(mechData);

        mechNameText.text = mechData.customName;

        mechStatusText.text = mechData.MechStatusDisplay;

        switch (mechData.MechStatus)
        {
            case MechStatusType.Ready:
                {
                    mechStatusText.color = new Color(0.0f, 0.75f, 0.0f);
                    break;
                }
            case MechStatusType.Damaged:
                {
                    mechStatusText.color = new Color(1.0f, 0.8f, 0.2f);
                    break;
                }
            case MechStatusType.Crippled:
                {
                    mechStatusText.color = Color.red;
                    break;
                }
            default:
                {
                    mechStatusText.color = Color.black;
                    break;
                }
        }
    }
}
