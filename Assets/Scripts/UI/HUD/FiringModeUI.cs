using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FiringModeUI : MonoBehaviour
{
    [SerializeField]
    GameObject stateHighlight1;

    [SerializeField]
    GameObject stateHighlight2;

    [SerializeField]
    GameObject stateHighlight3;

    [SerializeField]
    GameObject stateHighlight4;

    [SerializeField]
    GameObject stateHighlight5;

    [SerializeField]
    GameObject stateHighlight6;

    [SerializeField]
    Text state1Text;

    [SerializeField]
    Text state2Text;

    [SerializeField]
    Text state3Text;

    [SerializeField]
    Text state4Text;

    [SerializeField]
    Text state5Text;

    [SerializeField]
    Text state6Text;

    public GameObject StateHighlight1 { get => stateHighlight1; }

    public GameObject StateHighlight2 { get => stateHighlight2; }

    public GameObject StateHighlight3 { get => stateHighlight3; }

    public GameObject StateHighlight4 { get => stateHighlight4; }

    public GameObject StateHighlight5 { get => stateHighlight5; }

    public GameObject StateHighlight6 { get => stateHighlight6; }

    public Text State1Text { get => state1Text; }

    public Text State2Text { get => state2Text; }

    public Text State3Text { get => state3Text; }

    public Text State4Text { get => state4Text; }

    public Text State5Text { get => state5Text; }

    public Text State6Text { get => state6Text; }
}
