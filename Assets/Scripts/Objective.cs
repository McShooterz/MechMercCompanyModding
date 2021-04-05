using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Objective
{
    public string displayText = "";

    public ObjectiveType objectiveType = ObjectiveType.DestroyAllUnits;

    public ObjectiveState objectiveState = ObjectiveState.Active;

    public int pay = 0;

    public bool hidden = false;

    public bool optional = false;

    public bool instantFailMission = false;

    public bool instantCompleteMission = false;

    public int count = 0;

    public float range = 0.0f;

    public float startTime = 0.0f;

    public float timeLimit = 0.0f;

    public bool failAtEndOfTimer = false;

    public int navPointIndex = 0;

    public int navPointIndexSetOnSuccess = -1;

    public int navPointIndexSetOnFailure = -1;

    public bool alertEnemiesPlayerLocationOnSuccess = false;

    public bool showDebriefing = true;

    [System.NonSerialized]
    public UnitData[] units = new UnitData[0];

    [System.NonSerialized]
    public Objective[] objectivesToActivateOnSuccess = new Objective[0];

    [System.NonSerialized]
    public Objective[] objectivesToActivateOnFailure = new Objective[0];

    [System.NonSerialized]
    public Objective[] objectivesToDisableOnSuccess = new Objective[0];

    [System.NonSerialized]
    public Objective[] objectivesToDisableOnFailure = new Objective[0];

    [System.NonSerialized]
    public Objective[] objectivesToHideOnSuccess = new Objective[0];

    [System.NonSerialized]
    public Objective[] objectivesToHideOnFailure = new Objective[0];

    [System.NonSerialized]
    public Objective[] objectivesToUnHideOnSuccess = new Objective[0];

    [System.NonSerialized]
    public Objective[] objectivesToUnHideOnFailure = new Objective[0];

    [System.NonSerialized]
    public Objective[] objectivesToFailOnSuccess = new Objective[0];

    [System.NonSerialized]
    public Objective[] objectivesToFailOnFailure = new Objective[0];

    [System.NonSerialized]
    public Objective[] objectivesToCompleteOnSuccess = new Objective[0];

    [System.NonSerialized]
    public Objective[] objectivesToCompleteOnFailure = new Objective[0];

    public MissionEventBase[] missionEventsOnSuccess = new MissionEventBase[0];

    public MissionEventBase[] missionEventsOnFailure = new MissionEventBase[0];

    public AudioClip[] audioClipsToPlayOnActivation = new AudioClip[0];

    public AudioClip[] audioClipsToPlayOnSuccess = new AudioClip[0];

    public AudioClip[] audioClipsToPlayOnFailure = new AudioClip[0];

    public float Timer { get => Mathf.Ceil(timeLimit - (Time.time - startTime)); }

    public bool ActiveTimer { get => timeLimit > 0.0f && objectiveState == ObjectiveState.Active && !hidden; }

    public string DisplayText
    {
        get
        {
            string timeText = "";

            if (timeLimit > 0.0f && objectiveState == ObjectiveState.Active)
            {
                timeText = "(" + StaticHelper.FormatTime((int)Timer) + ")";
            }

            if (optional && objectiveState == ObjectiveState.Active)
            {
                return StaticHelper.AddColorTagToText("Optional" + timeText + ": " + displayText, ResourceManager.Instance.GameConstants.GetObjectiveColorValue(objectiveState));
            }

            return StaticHelper.AddColorTagToText(StaticHelper.GetObjectiveStateText(objectiveState) + timeText + ": " + displayText, ResourceManager.Instance.GameConstants.GetObjectiveColorValue(objectiveState));
        }
    }

    public string DebriefingDisplayText
    {
        get
        {
            if (objectiveState == ObjectiveState.Completed)
            {
                return "Successful: " + displayText;
            }

            return "Failed: " + displayText;
        }
    }

    public int MissionPay
    {
        get
        {
            if (objectiveState == ObjectiveState.Completed)
            {
                return pay;
            }

            return 0;
        }
    }

    public ObjectiveSave ObjectiveSave
    {
        get
        {
            return new ObjectiveSave()
            {
                DisplayText = displayText,
                ObjectiveState = objectiveState,
                Pay = pay,
            };
        }
    }

    public Objective() { }

    public Objective(ObjectiveSave objectiveSave)
    {
        displayText = objectiveSave.DisplayText;
        objectiveState = objectiveSave.ObjectiveState;
        pay = objectiveSave.Pay;
    }

    /// <summary>Checks the objective and returns true if objective state has changed.</summary>
    public bool Update()
    {
        if (objectiveState != ObjectiveState.Active)
            return false;

        if (timeLimit > 0.0f && Time.time > (timeLimit + startTime))
        {
            if (failAtEndOfTimer)
            {
                SetFailed();
            }
            else
            {
                SetCompleted();
            }

            return true;
        }

        switch (objectiveType)
        {
            case ObjectiveType.DestroyAllUnits:
                {
                    return UpdateDestroyAllUnits();
                }
            case ObjectiveType.ProtectAllUnits:
                {
                    return UpdateProtectAllUnits();
                }
            case ObjectiveType.ProtectAnyUnits:
                {
                    return UpdateProtectAnyUnits();
                }
            case ObjectiveType.ProtectCountUnits:
                {
                    return UpdateProtectUnitsCount();
                }
            case ObjectiveType.MoveToNavPoint:
                {
                    return UpdateMoveToNavPoint();
                }
            case ObjectiveType.PreventConvoyReachEnd:
                {
                    return UpdatePreventConvoyReachEnd();
                }
            case ObjectiveType.BreakContactWithEnemy:
                {
                    return UpdateBreakContactWithEnemy();
                }
            case ObjectiveType.RemainInRangeOfNavPoint:
                {
                    return UpdateRemainInRangeNavPoint();
                }
            case ObjectiveType.AvoidEnemyDetection:
                {
                    return UpdateAvoidEnemyDetection();
                }
        }

        return false;
    }

    public void SetCompleted()
    {
        objectiveState = ObjectiveState.Completed;
        hidden = false;
        ActivateObjectives(objectivesToActivateOnSuccess);
        DisableObjectives(objectivesToDisableOnSuccess);
        HideObjectives(objectivesToHideOnSuccess);
        UnhideObjectives(objectivesToUnHideOnSuccess);
        FailObjectives(objectivesToFailOnSuccess);
        CompleteObjectives(objectivesToCompleteOnSuccess);

        if (!GlobalData.Instance.ActiveMissionData.MissionOver)
        {
            TriggerMissionEvents(missionEventsOnSuccess);
            PlayAudioClips(audioClipsToPlayOnSuccess);
        }

        if (navPointIndexSetOnSuccess != -1)
        {
            MechControllerPlayer.Instance.SetNavPoint(navPointIndexSetOnSuccess, true);
        }

        if (alertEnemiesPlayerLocationOnSuccess)
        {
            MissionManager.Instance.AlertEnemiesToPlayerLocation();
        }
    }

    public void SetFailed()
    {
        objectiveState = ObjectiveState.Failed;
        hidden = false;
        ActivateObjectives(objectivesToActivateOnFailure);
        DisableObjectives(objectivesToDisableOnFailure);
        HideObjectives(objectivesToHideOnFailure);
        UnhideObjectives(objectivesToUnHideOnFailure);
        FailObjectives(objectivesToFailOnFailure);
        CompleteObjectives(objectivesToCompleteOnFailure);

        if (!GlobalData.Instance.ActiveMissionData.MissionOver)
        {
            TriggerMissionEvents(missionEventsOnFailure);
            PlayAudioClips(audioClipsToPlayOnFailure);
        }

        if (navPointIndexSetOnFailure != -1)
        {
            MechControllerPlayer.Instance.SetNavPoint(navPointIndexSetOnFailure, true);
        }
    }

    bool UpdateDestroyAllUnits()
    {
        for (int i = 0; i < units.Length; i++)
        {
            UnitData unit = units[i];

            if (!unit.isDestroyed && !unit.isDisabled)
            {
                return false;
            }
        }

        SetCompleted();
        return true;
    }

    bool UpdateProtectAllUnits()
    {
        for (int i = 0; i < units.Length; i++)
        {
            UnitData unit = units[i];

            if (unit.isDestroyed || unit.isDisabled)
            {
                SetFailed();
                return true;
            }
        }

        return false;
    }

    bool UpdateProtectAnyUnits()
    {
        for (int i = 0; i < units.Length; i++)
        {
            UnitData unit = units[i];

            if (!unit.isDestroyed && !unit.isDisabled)
            {
                return false;
            }
        }

        SetFailed();
        return true;
    }

    bool UpdateProtectUnitsCount()
    {
        int protectedCount = 0;

        for (int i = 0; i < units.Length; i++)
        {
            UnitData unit = units[i];

            if (!unit.isDestroyed && !unit.isDisabled)
            {
                protectedCount++;

                if (protectedCount >= count)
                {
                    return false;
                }
            }
        }

        SetFailed();
        return true;
    }

    bool UpdateMoveToNavPoint()
    {
        Vector3 Direction = MechControllerPlayer.Instance.transform.position - MissionManager.Instance.NavigationPoints[navPointIndex].position;

        if (Direction.magnitude < range)
        {
            SetCompleted();
            return true;
        }

        return false;
    }

    bool UpdatePreventConvoyReachEnd()
    {
        if (MissionManager.Instance.ConvoyController.convoyReachedEnd)
        {
            SetFailed();
            return true;
        }

        return false;
    }

    bool UpdateBreakContactWithEnemy()
    {
        bool inContactWithEnemy = MissionManager.Instance.PlayerInContactWithEnemy();

        if (timeLimit > 0)
        {
            if (inContactWithEnemy)
            {
                startTime = Time.time;
            }
        }
        else if (!inContactWithEnemy)
        {
            SetCompleted();
            return true;
        }

        return false;
    }

    bool UpdateRemainInRangeNavPoint()
    {
        Vector3 Direction = MechControllerPlayer.Instance.transform.position - MissionManager.Instance.NavigationPoints[navPointIndex].position;

        if (Direction.magnitude > range)
        {
            startTime = Time.time;
        }

        return false;
    }

    bool UpdateAvoidEnemyDetection()
    {
        if (MissionManager.Instance.PlayerInContactWithEnemy())
        {
            SetFailed();
            return true;
        }

        return false;
    }

    public void SetStartTime()
    {
        if (timeLimit > 0.0f)
        {
            startTime = Time.time;
        }
    }

    public void Activate()
    {
        if (objectiveState == ObjectiveState.Disabled)
        {
            objectiveState = ObjectiveState.Active;
        }

        SetStartTime();

        PlayAudioClips(audioClipsToPlayOnActivation);
    }

    void ActivateObjectives(Objective[] objectives)
    {
        Objective objective;

        for (int i = 0; i < objectives.Length; i++)
        {
            objective = objectives[i];

            if (objective.objectiveState == ObjectiveState.Disabled)
            {
                objective.Activate();
            }
        }
    }

    void DisableObjectives(Objective[] objectives)
    {
        Objective objective;

        for (int i = 0; i < objectives.Length; i++)
        {
            objective = objectives[i];

            if (objective.objectiveState != ObjectiveState.Failed)
            {
                objective.objectiveState = ObjectiveState.Disabled;
            }
        }
    }

    void HideObjectives(Objective[] objectives)
    {
        for (int i = 0; i < objectives.Length; i++)
        {
            objectives[i].hidden = true;
        }
    }

    void UnhideObjectives(Objective[] objectives)
    {
        for (int i = 0; i < objectives.Length; i++)
        {
            objectives[i].hidden = false;
        }
    }

    void FailObjectives(Objective[] objectives)
    {
        Objective objective;

        for (int i = 0; i < objectives.Length; i++)
        {
            objective = objectives[i];

            if (objective.objectiveState != ObjectiveState.Completed && objective.objectiveState != ObjectiveState.Failed)
            {
                objective.SetFailed();
            }
        }
    }

    void CompleteObjectives(Objective[] objectives)
    {
        Objective objective;

        for (int i = 0; i < objectives.Length; i++)
        {
            objective = objectives[i];

            if (objective.objectiveState != ObjectiveState.Completed && objective.objectiveState != ObjectiveState.Failed)
            {
                objective.SetCompleted();
            }
        }
    }

    void TriggerMissionEvents(MissionEventBase[] missionEvents)
    {
        MissionEventBase missionEvent;

        for (int i = 0; i < missionEvents.Length; i++)
        {
            missionEvent = missionEvents[i];

            if (missionEvent != null)
            {
                missionEvent.Trigger();
            }
        }
    }

    void PlayAudioClips(AudioClip[] audioClips)
    {
        AudioClip audioClip;

        for(int i = 0; i < audioClips.Length; i++)
        {
            audioClip = audioClips[i];

            MechControllerPlayer.Instance.CommandSystem.AudioClipsRadio.Enqueue(audioClip);
        }
    }

    public static Objective[] CreateObjectiveArray(int count)
    {
        Objective[] objectives = new Objective[count];

        for (int i = 0; i < count; i++)
        {
            objectives[i] = new Objective();
        }

        return objectives;
    }
}
