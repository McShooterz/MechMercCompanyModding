using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AxisInputGroupUI : MonoBehaviour
{
    [SerializeField]
    ControlOptionsUI controlOptionsUI;

    [SerializeField]
    Button auxiliaryPosButton;

    [SerializeField]
    Button auxiliaryNegButton;

    [SerializeField]
    Text keyboardPosText;

    [SerializeField]
    Text keyboardNegText;

    [SerializeField]
    Text mouseText;

    [SerializeField]
    Text auxiliaryPosText;

    [SerializeField]
    Text auxiliaryNegText;

    InputManager.AxisGroup axisGroup;

    public void SetAxisGroup(InputManager.AxisGroup target)
    {
        axisGroup = target;

        SetText();
    }

    public void SetText()
    {
        keyboardPosText.text = axisGroup.positiveKey.ToString();
        keyboardNegText.text = axisGroup.negativeKey.ToString();

        mouseText.text = StaticHelper.GetMouseAxisName(axisGroup.mouseAxis);

        bool hasAuxiliaryDevice = InputManager.Instance.HasAuxiliaryDevice;

        auxiliaryPosText.text = axisGroup.AuxiliaryPositiveDisplay;
        auxiliaryNegText.text = axisGroup.AuxiliaryNegativeDisplay;

        auxiliaryPosButton.interactable = hasAuxiliaryDevice;
        auxiliaryNegButton.interactable = hasAuxiliaryDevice;
    }

    public void ClickKeyboardPosButton()
    {
        controlOptionsUI.OpenRebindWindowKeyPos(axisGroup, this);

        AudioManager.Instance.PlayButtonClick(0);
    }

    public void ClickKeyboardNegButton()
    {
        controlOptionsUI.OpenRebindWindowKeyNeg(axisGroup, this);

        AudioManager.Instance.PlayButtonClick(0);
    }

    public void ClickMouseButton()
    {
        controlOptionsUI.OpenRebindWindowMouseAxis(axisGroup, this);

        AudioManager.Instance.PlayButtonClick(0);
    }

    public void ClickAuxiliaryPositiveButton()
    {
        controlOptionsUI.OpenRebindWindowAuxiliaryPositive(axisGroup, this);

        AudioManager.Instance.PlayButtonClick(0);
    }

    public void ClickAuxiliaryNegativeButton()
    {
        controlOptionsUI.OpenRebindWindowAuxiliaryNegative(axisGroup, this);

        AudioManager.Instance.PlayButtonClick(0);
    }

    public void ClickKeyboardPosClearButton()
    {
        axisGroup.positiveKey = UnityEngine.InputSystem.Key.None;

        SetText();

        AudioManager.Instance.PlayButtonClick(0);
    }

    public void ClickKeyboardNegClearButton()
    {
        axisGroup.negativeKey = UnityEngine.InputSystem.Key.None;

        SetText();

        AudioManager.Instance.PlayButtonClick(0);
    }

    public void ClickMouseClearButton()
    {
        axisGroup.mouseAxis = MouseAxis.None;

        SetText();

        AudioManager.Instance.PlayButtonClick(0);
    }

    public void ClickAuxiliaryPositiveClearButton()
    {
        axisGroup.auxiliaryPositive = -1;

        SetText();

        AudioManager.Instance.PlayButtonClick(0);
    }

    public void ClickAuxiliaryNegativeClearButton()
    {
        axisGroup.auxiliaryNegative = -1;

        SetText();

        AudioManager.Instance.PlayButtonClick(0);
    }
}
