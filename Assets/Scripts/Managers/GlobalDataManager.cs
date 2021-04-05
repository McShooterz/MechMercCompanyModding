using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalDataManager : MonoBehaviour
{
    public static GlobalDataManager Instance { get; private set; }

    public string backSceneName;

    public MechData mechDataCustomizing;

    //public MechData mechDataPlayer;

    //public MechData[] SquadMates { get; set; } = new MechData[11];

    public Inventory inventoryCurrent;

    public InstantActionGlobalData instantActionGlobalData = new InstantActionGlobalData();

    public Career currentCareer;

    //public MissionData currentMissionData;

    void Awake()
    {
        //Make a Singleton
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        //This stays in every scene
        DontDestroyOnLoad(gameObject);
    }

    // Use this for initialization
    void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}
}
