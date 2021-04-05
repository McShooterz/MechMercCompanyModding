using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponGroupingMenu : MonoBehaviour
{
    [SerializeField]
    WeaponGroupingEntry[] weaponGroupingEntries;

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void BuildWeaponsList(List<ComponentPlacedUI> componentPlacedUIs)
    {
        for (int i = 0; i < weaponGroupingEntries.Length; i++)
        {
            if (i < componentPlacedUIs.Count)
            {
                weaponGroupingEntries[i].gameObject.SetActive(true);
                weaponGroupingEntries[i].SetComponentPlacedUI(componentPlacedUIs[i]);
            }
            else
            {
                weaponGroupingEntries[i].gameObject.SetActive(false);
            }
        }
    }

    public void ClickBackButton()
    {
        gameObject.SetActive(false);
    }
}
