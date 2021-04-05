using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MechIconDamageUI : MonoBehaviour
{
    [SerializeField]
    Image headImage;

    [SerializeField]
    Image tosoCenterImage;

    [SerializeField]
    Image torsoLeftImage;

    [SerializeField]
    Image torsoRightImage;

    [SerializeField]
    Image armLeftImage;

    [SerializeField]
    Image armRightImage;

    [SerializeField]
    Image legLeftImage;

    [SerializeField]
    Image legRightImage;

    public void SetMech(MechData mechData)
    {
        if (mechData != null)
        {
            headImage.color = BaseDamageDisplay.GetInternalHealthColor(mechData.InternalPercentHead);
            tosoCenterImage.color = BaseDamageDisplay.GetInternalHealthColor(mechData.InternalPercentTorsoCenter);
            torsoLeftImage.color = BaseDamageDisplay.GetInternalHealthColor(mechData.InternalPercentTorsoLeft);
            torsoRightImage.color = BaseDamageDisplay.GetInternalHealthColor(mechData.InternalPercentTorsoRight);
            armLeftImage.color = BaseDamageDisplay.GetInternalHealthColor(mechData.InternalPercentArmLeft);
            armRightImage.color = BaseDamageDisplay.GetInternalHealthColor(mechData.InternalPercentArmRight);
            legLeftImage.color = BaseDamageDisplay.GetInternalHealthColor(mechData.InternalPercentLegLeft);
            legRightImage.color = BaseDamageDisplay.GetInternalHealthColor(mechData.InternalPercentLegRight);
        }
        else
        {
            headImage.color = BaseDamageDisplay.GetInternalHealthColor(1.0f);
            tosoCenterImage.color = BaseDamageDisplay.GetInternalHealthColor(1.0f);
            torsoLeftImage.color = BaseDamageDisplay.GetInternalHealthColor(1.0f);
            torsoRightImage.color = BaseDamageDisplay.GetInternalHealthColor(1.0f);
            armLeftImage.color = BaseDamageDisplay.GetInternalHealthColor(1.0f);
            armRightImage.color = BaseDamageDisplay.GetInternalHealthColor(1.0f);
            legLeftImage.color = BaseDamageDisplay.GetInternalHealthColor(1.0f);
            legRightImage.color = BaseDamageDisplay.GetInternalHealthColor(1.0f);
        }
    }
}
