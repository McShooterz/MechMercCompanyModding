using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ContractDefinition : Definition
{
    public string DisplayName;

    public string Description;

    public string Employer = "";

    public int ReputationRequired = 0;

    public int InfamyRequired = 0;

    public string GetDisplayName()
    {
        return ResourceManager.Instance.GetLocalization(DisplayName);
    }

    public string GetDescription()
    {
        return ResourceManager.Instance.GetLocalization(Description);
    }

    public FactionDefinition GetEmployer()
    {
        return ResourceManager.Instance.GetFactionDefinition(Employer);
    }
}
