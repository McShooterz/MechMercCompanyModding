using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InstantActionMissionMenu : MonoBehaviour
{
    [SerializeField]
    Transform content;

    [SerializeField]
    IndexButton indexButton;

    [SerializeField]
    IndexButton[] buttons;

    [SerializeField]
    Text missionTypeNameText;

    [SerializeField]
    Text missionTypeDescriptionText;

    [SerializeField]
    MissionType[] missions;

    [SerializeField]
    int selectedIndex = 0;

    [SerializeField]
    Color buttonColorDefault;

    [SerializeField]
    Color buttonColorHighlight;

    public delegate void CallBackFunction(MissionType missionType);
    public CallBackFunction callBackFunction;

    public void Initialize(CallBackFunction function, MissionType[] missionTypes, MissionType startingMissionType)
    {
        selectedIndex = 0;

        callBackFunction = function;

        missions = missionTypes;

        for (int i = 1; i < buttons.Length; i++)
        {
            Destroy(buttons[i].gameObject);
        }

        if (missions.Length > 0)
        {
            buttons = new IndexButton[missionTypes.Length];

            buttons[0] = indexButton;

            indexButton.gameObject.SetActive(true);
            indexButton.Initialize(0, StaticHelper.GetMissionTypeName(missions[0]), SelectMission);
            indexButton.BackgroundImage.color = buttonColorDefault;

            for (int i = 1; i < missions.Length; i++)
            {
                GameObject missionButtonObject = Instantiate(indexButton.gameObject, content);

                IndexButton newIndexButton = missionButtonObject.GetComponent<IndexButton>();

                if (newIndexButton != null)
                {
                    newIndexButton.Initialize(i, StaticHelper.GetMissionTypeName(missions[i]), SelectMission);
                    buttons[i] = newIndexButton;
                }
                else if (missionButtonObject != null)
                {
                    Destroy(missionButtonObject);
                }
            }

            int index = System.Array.IndexOf(missionTypes, startingMissionType);

            if (index != -1)
            {
                SelectMission(index);
            }
            else
            {
                SelectMission(0);
            }
        }
        else
        {
            indexButton.gameObject.SetActive(false);
            buttons = new IndexButton[0];
        }
    }

    public void SelectMission(int index)
    {
        buttons[selectedIndex].BackgroundImage.color = buttonColorDefault;

        selectedIndex = index;

        buttons[selectedIndex].BackgroundImage.color = buttonColorHighlight;

        missionTypeNameText.text = StaticHelper.GetMissionTypeName(missions[selectedIndex]);

        missionTypeDescriptionText.text = StaticHelper.GetMissionTypeDescription(missions[selectedIndex]);
    }

    public void SelectMission(MissionType missionType)
    {
        for (int i = 0; i < missions.Length; i++)
        {
            if (missionType == missions[i])
            {
                SelectMission(i);
                break;
            }
        }
    }

    public void ClickCancelButton()
    {
        gameObject.SetActive(false);

        AudioManager.Instance.PlayButtonClick(0);
    }

    public void ClickSelectButton()
    {
        callBackFunction(missions[selectedIndex]);

        gameObject.SetActive(false);

        AudioManager.Instance.PlayButtonClick(0);
    }
}
