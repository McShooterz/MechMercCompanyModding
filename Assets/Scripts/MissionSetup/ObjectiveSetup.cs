using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveSetup : MonoBehaviour
{
    public ObjectiveType objectiveType = ObjectiveType.DestroyAllUnits;

    public string displayText = "";

    public ObjectiveState objectiveState = ObjectiveState.Active;

    public bool startHidden = false;

    public bool instantFailMission = false;

    public bool instantCompleteMission = false;

    public int count;

    public float timeLimit;

    public bool failAtEndOfTimer;

    public int navPointIndex;

    public int navPointIndexSetOnSuccess = -1;

    public int navPointIndexSetOnFailure = -1;

    public GameObject[] units = new GameObject[0];

    public ObjectiveSetup[] objectivesToActivateOnSuccess = new ObjectiveSetup[0];

    public ObjectiveSetup[] objectivesToActivateOnFailure = new ObjectiveSetup[0];

    public ObjectiveSetup[] objectivesToDisableOnSuccess = new ObjectiveSetup[0];

    public ObjectiveSetup[] objectivesToDisableOnFailure = new ObjectiveSetup[0];

    public ObjectiveSetup[] objectivesToUnHideOnSuccess = new ObjectiveSetup[0];

    public ObjectiveSetup[] objectivesToUnHideOnFailure = new ObjectiveSetup[0];

    public ObjectiveSetup[] objectivesToFailOnSuccess = new ObjectiveSetup[0];

    public ObjectiveSetup[] objectivesToFailOnFailure = new ObjectiveSetup[0];

    public ObjectiveSetup[] objectivesToCompleteOnSuccess = new ObjectiveSetup[0];

    public ObjectiveSetup[] objectivesToCompleteOnFailure = new ObjectiveSetup[0];

    public MissionEventBase[] missionEventsOnSuccess = new MissionEventBase[0];

    public MissionEventBase[] missionEventsOnFailure = new MissionEventBase[0];

    public AudioClip[] audioClipsToPlayOnActivation = new AudioClip[0];

    public AudioClip[] audioClipsToPlayOnSuccess = new AudioClip[0];

    public AudioClip[] audioClipsToPlayOnFailure = new AudioClip[0];

    public List<UnitController> GetUnits()
    {
        List<UnitController> unitControllers = new List<UnitController>();

        foreach (GameObject unitObject in units)
        {
            UnitController unitController = unitObject.GetComponent<UnitController>();

            if (unitController != null)
            {
                unitControllers.Add(unitController);
            }
        }

        return unitControllers;
    }
}
