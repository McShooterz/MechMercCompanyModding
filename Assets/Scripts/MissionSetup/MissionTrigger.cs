using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
public class MissionTrigger : MonoBehaviour
{
    [SerializeField]
    Transform[] triggerTargets;

    [SerializeField]
    Collider attachedCollider;

    [SerializeField]
    Rigidbody attachedRigidbody;

    [SerializeField]
    MissionEventBase[] missionEventsToTrigger;

    [SerializeField]
    MissionTrigger[] missionTriggersToEnable;

    [SerializeField]
    int[] objectivesToActivate;

    [SerializeField]
    int[] objectivesToUnHide;

    [SerializeField]
    int[] objectivesToComplete;

    void Awake()
    {
        attachedCollider.isTrigger = true;
        attachedRigidbody.isKinematic = true;
        attachedRigidbody.useGravity = false;

        gameObject.layer = LayerMask.NameToLayer("Trigger");
    }

    void OnTriggerEnter(Collider other)
    {
        foreach (Transform triggerTarget in triggerTargets)
        {
            if (other.transform == triggerTarget)
            {
                gameObject.SetActive(false);

                foreach (MissionEventBase missionEvent in missionEventsToTrigger)
                {
                    missionEvent.Trigger();
                }

                foreach (MissionTrigger missionTrigger in missionTriggersToEnable)
                {
                    missionTrigger.gameObject.SetActive(true);
                }

                if (objectivesToActivate.Length > 0)
                {
                    foreach (int objectiveIndex in objectivesToActivate)
                    {
                        if (objectiveIndex > 0 && objectiveIndex < MissionManager.Instance.MissionData.objectives.Length)
                        {
                            MissionManager.Instance.MissionData.objectives[objectiveIndex].Activate();
                        }
                    }

                    PlayerHUD.Instance.SetObjectivesWindowVisibility(true);
                    PlayerHUD.Instance.ObjectivesControllerUI.RebuildObjectivesText();
                }

                if (objectivesToUnHide.Length > 0)
                {
                    foreach (int objectiveIndex in objectivesToUnHide)
                    {
                        if (objectiveIndex > 0 && objectiveIndex < MissionManager.Instance.MissionData.objectives.Length)
                        {
                            MissionManager.Instance.MissionData.objectives[objectiveIndex].hidden = false;
                        }
                    }

                    PlayerHUD.Instance.SetObjectivesWindowVisibility(true);
                    PlayerHUD.Instance.ObjectivesControllerUI.RebuildObjectivesText();
                }

                if (objectivesToComplete.Length > 0)
                {
                    foreach (int objectiveIndex in objectivesToComplete)
                    {
                        if (objectiveIndex > 0 && objectiveIndex < MissionManager.Instance.MissionData.objectives.Length)
                        {
                            MissionManager.Instance.MissionData.objectives[objectiveIndex].SetCompleted();
                        }
                    }

                    PlayerHUD.Instance.SetObjectivesWindowVisibility(true);
                    PlayerHUD.Instance.ObjectivesControllerUI.RebuildObjectivesText();
                }

                break;
            }
        }
    }
}