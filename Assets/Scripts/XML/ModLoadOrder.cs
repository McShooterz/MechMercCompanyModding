using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModLoadOrder
{
    public bool LoadMods = false;

    public ModEntry[] ModEntries = new ModEntry[0];

    [System.Serializable]
    public class ModEntry
    {
        public string Path = "";

        public bool IsActive = false;
    }
}
