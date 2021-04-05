using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissionButton : MonoBehaviour
{
    [SerializeField]
    Text text;

    MissionDefinition missionDefinition;

    public delegate void CallBackFunction(MissionDefinition mission);
    public CallBackFunction callBackFunction;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Initialize(CallBackFunction callBackFunction, MissionDefinition mission)
    {
        this.callBackFunction = callBackFunction;
        missionDefinition = mission;

        text.text = missionDefinition.GetDisplayName();
    }

    public void ClickButton()
    {
        callBackFunction(missionDefinition);

        AudioManager.Instance.PlayButtonClick(0);
    }
}
