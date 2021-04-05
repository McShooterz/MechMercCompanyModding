using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponUI : MonoBehaviour
{
    [SerializeField]
    Text group1Text;

    [SerializeField]
    Text group2Text;

    [SerializeField]
    Text group3Text;

    [SerializeField]
    Text group4Text;

    [SerializeField]
    Text group5Text;

    [SerializeField]
    Text group6Text;

    [SerializeField]
    Text weaponNameText;

    [SerializeField]
    Text rangeText;

    [SerializeField]
    Slider weaponRefireBar;

    [SerializeField]
    Slider jammingBar;

    [SerializeField]
    Text ammoTypeText;

    [SerializeField]
    Text ammoText;

    [SerializeField]
    GameObject highlightGroup1;

    [SerializeField]
    GameObject highlightGroup2;

    [SerializeField]
    GameObject highlightGroup3;

    [SerializeField]
    GameObject highlightGroup4;

    [SerializeField]
    GameObject highlightGroup5;

    [SerializeField]
    GameObject highlightGroup6;

    [SerializeField]
    GameObject highlightAmmo;

    [SerializeField]
    GameObject fireSelectGroup1;

    [SerializeField]
    GameObject fireSelectGroup2;

    [SerializeField]
    GameObject fireSelectGroup3;

    [SerializeField]
    GameObject fireSelectGroup4;

    [SerializeField]
    GameObject fireSelectGroup5;

    [SerializeField]
    GameObject fireSelectGroup6;

    public Text Group1Text { get => group1Text; }

    public Text Group2Text { get => group2Text; }

    public Text Group3Text { get => group3Text; }

    public Text Group4Text { get => group4Text; }

    public Text Group5Text { get => group5Text; }

    public Text Group6Text { get => group6Text; }

    public Text WeaponNameText { get => weaponNameText; }

    public Text RangeText { get => rangeText; }

    public Slider WeaponRefireBar { get => weaponRefireBar; }

    public Slider JammingBar { get => jammingBar; }

    public Text AmmoTypeText { get => ammoTypeText; }

    public Text AmmoText { get => ammoText; }

    public GameObject HighlightGroup1 { get => highlightGroup1; }

    public GameObject HighlightGroup2 { get => highlightGroup2; }

    public GameObject HighlightGroup3 { get => highlightGroup3; }

    public GameObject HighlightGroup4 { get => highlightGroup4; }

    public GameObject HighlightGroup5 { get => highlightGroup5; }

    public GameObject HighlightGroup6 { get => highlightGroup6; }

    public GameObject HighlightAmmo { get => highlightAmmo; }

    public void SetFireSelection(bool group1, bool group2,bool group3, bool group4, bool group5, bool group6)
    {
        fireSelectGroup1.SetActive(group1);
        fireSelectGroup2.SetActive(group2);
        fireSelectGroup3.SetActive(group3);
        fireSelectGroup4.SetActive(group4);
        fireSelectGroup5.SetActive(group5);
        fireSelectGroup6.SetActive(group6);
    }

    public void ClearFireSelection()
    {
        fireSelectGroup1.SetActive(false);
        fireSelectGroup2.SetActive(false);
        fireSelectGroup3.SetActive(false);
        fireSelectGroup4.SetActive(false);
        fireSelectGroup5.SetActive(false);
        fireSelectGroup6.SetActive(false);
    }
}
