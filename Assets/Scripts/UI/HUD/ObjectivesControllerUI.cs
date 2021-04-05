using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class ObjectivesControllerUI : MonoBehaviour
{
    [SerializeField]
    RectTransform ContentRectTransform;

    [SerializeField]
    Text objectivesText;

    [SerializeField]
    MissionData currentMissionData;

    StringBuilder stringBuilder = new StringBuilder();

    bool hasTimerObjective = false;

    void Awake()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        currentMissionData = GlobalData.Instance.ActiveMissionData;

        RebuildObjectivesText();
    }

    // Update is called once per frame
    void Update()
    {
        hasTimerObjective = false;

        for (int i = 0; i < currentMissionData.objectives.Length; i++)
        {
            if (currentMissionData.objectives[i].ActiveTimer)
            {
                hasTimerObjective = true;
                break;
            }
        }

        if (hasTimerObjective)
        {
            RebuildObjectivesText();
        }
    }

    void OnEnable()
    {
        RebuildObjectivesText();
    }

    public void RebuildObjectivesText()
    {
        stringBuilder.Length = 0;
        stringBuilder.Capacity = 0;

        for(int i = 0; i < currentMissionData.objectives.Length; i++)
        {
            Objective objective = currentMissionData.objectives[i];

            if (objective.hidden)
            {
                continue;
            }

            stringBuilder.AppendLine("\n" + objective.DisplayText);
        }

        if (currentMissionData.MissionOver)
        {
            if (currentMissionData.successful)
            {
                stringBuilder.Append("\n<color=lime>Mission Successful</color>");
            }
            else
            {
                stringBuilder.Append("\n<color=red>Mission Failed</color>");
            }

            stringBuilder.Append("\nPress F10 to exit mission.");
        }

        if (InputManager.Instance.objectives.key != UnityEngine.InputSystem.Key.None)
        {
            stringBuilder.Append($"\n\nPress {InputManager.Instance.objectives.key} to close.");
        }

        objectivesText.text = stringBuilder.ToString();
        LayoutRebuilder.ForceRebuildLayoutImmediate(ContentRectTransform);
    }
}
