using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ModButton : MonoBehaviour
{
    [SerializeField]
    ModLoadingScreen modLoadingScreen;

    [SerializeField]
    Image backGround;

    [SerializeField]
    Text modNameText;

    [SerializeField]
    Text modTypeText;

    [SerializeField]
    GameObject check;

    public Image BackGround { get { return backGround; } }

    public ModInfo ModInfo { get; private set; }

    public bool IsActive { get { return check.activeInHierarchy; } }

    public void Initialize(ModLoadingScreen modLoadingScreen, ModInfo modInfo, bool state)
    {
        this.modLoadingScreen = modLoadingScreen;

        ModInfo = modInfo;

        modNameText.text = ModInfo.DisplayName;
        modTypeText.text = ModInfo.ModType;

        check.SetActive(state);
    }

    public void ClickModButton()
    {
        AudioManager.Instance.PlayButtonClick(0);

        modLoadingScreen.SelectModButton(this);
    }

    public void ClickToggleButton()
    {
        AudioManager.Instance.PlayButtonClick(0);

        check.SetActive(!check.activeInHierarchy);
    }

    public void ClickMoveUpButton()
    {
        AudioManager.Instance.PlayButtonClick(0);

        modLoadingScreen.MoveButtonUp(this);
    }

    public void ClickMoveDownButton()
    {
        AudioManager.Instance.PlayButtonClick(0);

        modLoadingScreen.MoveButtonDown(this);
    }
}
