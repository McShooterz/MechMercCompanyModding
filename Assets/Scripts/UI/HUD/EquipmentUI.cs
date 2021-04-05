using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentUI : MonoBehaviour
{
    [SerializeField]
    GameObject stateHighlight;

    [SerializeField]
    Text stateText;

    [SerializeField]
    Text nameText;

    [SerializeField]
    Slider bar;

    [SerializeField]
    Text ammoText;

    public GameObject StateHighlight { get => stateHighlight; }

    public Text StateText { get => stateText; }

    public Text NameText { get => nameText; }

    public Slider Bar { get => bar; }

    public Text AmmoText { get => ammoText; }
}
