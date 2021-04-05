using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponGrouping
{
    public bool WeaponGroup1;
    public bool WeaponGroup2;
    public bool WeaponGroup3;
    public bool WeaponGroup4;
    public bool WeaponGroup5;
    public bool WeaponGroup6;

    public WeaponGrouping()
    {

    }

    public WeaponGrouping(WeaponGrouping source)
    {
        if (source != null)
        {
            WeaponGroup1 = source.WeaponGroup1;
            WeaponGroup2 = source.WeaponGroup2;
            WeaponGroup3 = source.WeaponGroup3;
            WeaponGroup4 = source.WeaponGroup4;
            WeaponGroup5 = source.WeaponGroup5;
            WeaponGroup6 = source.WeaponGroup6;
        }
    }

    public void Toggle(int index)
    {
        switch (index)
        {
            case 0:
                {
                    WeaponGroup1 = !WeaponGroup1;
                    break;
                }
            case 1:
                {
                    WeaponGroup2 = !WeaponGroup2;
                    break;
                }
            case 2:
                {
                    WeaponGroup3 = !WeaponGroup3;
                    break;
                }
            case 3:
                {
                    WeaponGroup4 = !WeaponGroup4;
                    break;
                }
            case 4:
                {
                    WeaponGroup5 = !WeaponGroup5;
                    break;
                }
            case 5:
                {
                    WeaponGroup6 = !WeaponGroup6;
                    break;
                }
        }
    }
}
