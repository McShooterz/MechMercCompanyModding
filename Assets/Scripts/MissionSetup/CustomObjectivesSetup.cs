using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomObjectivesSetup : MonoBehaviour
{
    [SerializeField]
    ObjectiveSetup[] objectivesetups = new ObjectiveSetup[0];

    public Objective[] GetObjectives()
    {
        Objective[] objectives = new Objective[objectivesetups.Length];

        // Create empty objectives so they can reference each other when building them out
        for (int i = 0; i < objectives.Length; i++)
        {
            objectives[i] = new Objective();
        }

        // Build objectives
        for (int i = 0; i < objectives.Length; i++)
        {
            Objective objective = objectives[i];
            ObjectiveSetup objectiveSetup = objectivesetups[i];

            objective.objectiveType = objectiveSetup.objectiveType;

            objective.displayText = objectiveSetup.displayText;

            objective.objectiveState = objectiveSetup.objectiveState;

            objective.hidden = objectiveSetup.startHidden;          

            objective.instantCompleteMission = objectiveSetup.instantCompleteMission;
            objective.instantFailMission = objectiveSetup.instantFailMission;

            objective.timeLimit = objectiveSetup.timeLimit;

            objective.failAtEndOfTimer = objectiveSetup.failAtEndOfTimer;

            objective.count = objectiveSetup.count;

            objective.navPointIndex = objectiveSetup.navPointIndex;

            objective.navPointIndexSetOnSuccess = objectiveSetup.navPointIndexSetOnSuccess;

            objective.navPointIndexSetOnFailure = objectiveSetup.navPointIndexSetOnFailure;

            List<UnitController> unitControllers = objectiveSetup.GetUnits();

            if (unitControllers.Count > 0)
            {
                objective.units = new UnitData[unitControllers.Count];

                for (int k = 0; k < objective.units.Length; k++)
                {
                    objective.units[k] = unitControllers[k].UnitData;
                }
            }

            if (objectiveSetup.objectivesToActivateOnFailure.Length > 0)
            {
                objective.objectivesToActivateOnFailure = BuildObjectiveArray(objectives, objectiveSetup.objectivesToActivateOnFailure);
            }

            if (objectiveSetup.objectivesToActivateOnSuccess.Length > 0)
            {
                objective.objectivesToActivateOnSuccess = BuildObjectiveArray(objectives, objectiveSetup.objectivesToActivateOnSuccess);
            }

            if (objectiveSetup.objectivesToCompleteOnFailure.Length > 0)
            {
                objective.objectivesToCompleteOnFailure = BuildObjectiveArray(objectives, objectiveSetup.objectivesToCompleteOnFailure);
            }

            if (objectiveSetup.objectivesToCompleteOnSuccess.Length > 0)
            {
                objective.objectivesToCompleteOnSuccess = BuildObjectiveArray(objectives, objectiveSetup.objectivesToCompleteOnSuccess);
            }

            if (objectiveSetup.objectivesToDisableOnFailure.Length > 0)
            {
                objective.objectivesToDisableOnFailure = BuildObjectiveArray(objectives, objectiveSetup.objectivesToDisableOnFailure);
            }

            if (objectiveSetup.objectivesToDisableOnSuccess.Length > 0)
            {
                objective.objectivesToDisableOnSuccess = BuildObjectiveArray(objectives, objectiveSetup.objectivesToDisableOnSuccess);
            }

            if (objectiveSetup.objectivesToFailOnFailure.Length > 0)
            {
                objective.objectivesToFailOnFailure = BuildObjectiveArray(objectives, objectiveSetup.objectivesToFailOnFailure);
            }

            if (objectiveSetup.objectivesToFailOnSuccess.Length > 0)
            {
                objective.objectivesToFailOnSuccess = BuildObjectiveArray(objectives, objectiveSetup.objectivesToFailOnSuccess);
            }

            if (objectiveSetup.objectivesToUnHideOnFailure.Length > 0)
            {
                objective.objectivesToUnHideOnFailure = BuildObjectiveArray(objectives, objectiveSetup.objectivesToUnHideOnFailure);
            }

            if (objectiveSetup.objectivesToUnHideOnSuccess.Length > 0)
            {
                objective.objectivesToUnHideOnSuccess = BuildObjectiveArray(objectives, objectiveSetup.objectivesToUnHideOnSuccess);
            }

            objective.missionEventsOnSuccess = objectiveSetup.missionEventsOnSuccess;
            objective.missionEventsOnFailure = objectiveSetup.missionEventsOnFailure;

            objective.audioClipsToPlayOnActivation = objectiveSetup.audioClipsToPlayOnActivation;

            objective.audioClipsToPlayOnSuccess = objectiveSetup.audioClipsToPlayOnSuccess;

            objective.audioClipsToPlayOnFailure = objectiveSetup.audioClipsToPlayOnFailure;

            if (objectiveSetup.objectiveState == ObjectiveState.Active)
            {
                objective.Activate();
            }
        }

        return objectives;
    }

    Objective[] BuildObjectiveArray(Objective[] sourceObjectives, ObjectiveSetup[] sourceObjectiveSetups)
    {
        Objective[] objectives = new Objective[sourceObjectiveSetups.Length];

        for (int i = 0; i < sourceObjectiveSetups.Length; i++)
        {
            objectives[i] = sourceObjectives[System.Array.IndexOf(objectivesetups, sourceObjectiveSetups[i])];
        }

        return objectives;
    }
}
