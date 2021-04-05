using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoDefinition : Definition
{
    public string DisplayName = "";
    public string Description = "";
    public float MarketValue;

    public string GetDisplayName()
    {
        return ResourceManager.Instance.GetLocalization(DisplayName);
    }

    public string GetDescription()
    {
        return ResourceManager.Instance.GetLocalization(Description);
    }
}
