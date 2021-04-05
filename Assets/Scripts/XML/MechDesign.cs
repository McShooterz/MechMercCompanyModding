using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class MechDesign
{
    public string DesignName = "";

    public string MechChassisDefinition = "";

    public ArmorType ArmorTypeHead;
    public int ArmorPointsHead;

    public ArmorType ArmorTypeTorsoCenter;
    public int ArmorPointsTorsoCenter;
    public int ArmorPointsTorsoCenterRear;

    public ArmorType ArmorTypeTorsoLeft;
    public int ArmorPointsTorsoLeft;
    public int ArmorPointsTorsoLeftRear;

    public ArmorType ArmorTypeTorsoRight;
    public int ArmorPointsTorsoRight;
    public int ArmorPointsTorsoRightRear;

    public ArmorType ArmorTypeArmLeft;
    public int ArmorPointsArmLeft;

    public ArmorType ArmorTypeArmRight;
    public int ArmorPointsArmRight;

    public ArmorType ArmorTypeLegLeft;
    public int ArmorPointsLegLeft;

    public ArmorType ArmorTypeLegRight;
    public int ArmorPointsLegRight;

    public ComponentGroup[] ComponentGroupsHead = new ComponentGroup[0];
    public ComponentGroup[] ComponentGroupsTorsoCenter = new ComponentGroup[0];
    public ComponentGroup[] ComponentGroupsTorsoLeft = new ComponentGroup[0];
    public ComponentGroup[] ComponentGroupsTorsoRight = new ComponentGroup[0];
    public ComponentGroup[] ComponentGroupsArmLeft = new ComponentGroup[0];
    public ComponentGroup[] ComponentGroupsArmRight = new ComponentGroup[0];
    public ComponentGroup[] ComponentGroupsLegLeft = new ComponentGroup[0];
    public ComponentGroup[] ComponentGroupsLegRight = new ComponentGroup[0];

    public int TotalArmor { get => ArmorPointsHead + ArmorPointsTorsoCenter + ArmorPointsTorsoCenterRear + ArmorPointsTorsoLeft + ArmorPointsTorsoLeftRear + ArmorPointsTorsoRight + ArmorPointsTorsoRightRear + ArmorPointsArmLeft + ArmorPointsArmRight + ArmorPointsLegLeft + ArmorPointsLegRight; }

    public string Key { get => ResourceManager.Instance.GetMechDesignKey(MechChassisDefinition, this); }

    public List<ComponentDefinition> GetComponentDefinitionsFromComponentGroups(ComponentGroup[] componentGroups)
    {
        List<ComponentDefinition> componentDefinitions = new List<ComponentDefinition>();

        foreach (ComponentGroup componentGroup in componentGroups)
        {
            componentDefinitions.AddRange(componentGroup.GetComponents());
        }

        return componentDefinitions;
    }

    public List<ComponentDefinition> GetAllComponentDefinitions()
    {
        List<ComponentDefinition> componentDefinitions = new List<ComponentDefinition>();

        componentDefinitions.AddRange(GetComponentDefinitionsFromComponentGroups(ComponentGroupsHead));
        componentDefinitions.AddRange(GetComponentDefinitionsFromComponentGroups(ComponentGroupsTorsoCenter));
        componentDefinitions.AddRange(GetComponentDefinitionsFromComponentGroups(ComponentGroupsTorsoLeft));
        componentDefinitions.AddRange(GetComponentDefinitionsFromComponentGroups(ComponentGroupsTorsoRight));
        componentDefinitions.AddRange(GetComponentDefinitionsFromComponentGroups(ComponentGroupsArmLeft));
        componentDefinitions.AddRange(GetComponentDefinitionsFromComponentGroups(ComponentGroupsArmRight));
        componentDefinitions.AddRange(GetComponentDefinitionsFromComponentGroups(ComponentGroupsLegLeft));
        componentDefinitions.AddRange(GetComponentDefinitionsFromComponentGroups(ComponentGroupsLegRight));

        return componentDefinitions;
    }

    public MechChassisDefinition GetMechChassisDefinition()
    {
        return ResourceManager.Instance.GetMechChassisDefinition(MechChassisDefinition);
    }

    public string GetDisplayInformation()
    {
        MechChassisDefinition mechChassisDefinition = GetMechChassisDefinition();
        StringBuilder stringBuilder = new StringBuilder();

        float enginePower = 0f;
        float heatLimit = 0f;
        float heatLimitBonus = 0f;
        float cooling = 0f;
        float coolant = 0f;

        Dictionary<string, int> weapons = new Dictionary<string, int>();

        foreach(ComponentDefinition componentDefinition in GetAllComponentDefinitions())
        {
            if (componentDefinition.EnginePower > enginePower)
            {
                enginePower = componentDefinition.EnginePower;
            }

            if (componentDefinition.HeatLimit > heatLimit)
            {
                heatLimit = componentDefinition.HeatLimit;
            }

            heatLimitBonus += componentDefinition.HeatLimitBonus;
            cooling += componentDefinition.Cooling;
            coolant += componentDefinition.Coolant;

            WeaponDefinition weaponDefinition = componentDefinition.GetWeaponDefinition();

            if (weaponDefinition != null)
            {
                string weaponName = weaponDefinition.GetDisplayName();

                if (weapons.ContainsKey(weaponName))
                {
                    weapons[weaponName] += 1;
                }
                else
                {
                    weapons.Add(weaponName, 1);
                }
            }
        }

        stringBuilder.AppendLine("Total Armor: " + TotalArmor.ToString());

        stringBuilder.AppendLine("Reactor Power: " + enginePower.ToString());

        stringBuilder.AppendLine("Movement Speed: " + mechChassisDefinition.GetDisplaySpeedForward(enginePower).ToString("0.0") + " / " + mechChassisDefinition.GetDisplaySpeedReverse(enginePower).ToString("0.0") + " KPH");

        stringBuilder.AppendLine("Turn Speed: " + mechChassisDefinition.GetSpeedTurn(enginePower).ToString("0.#"));

        stringBuilder.AppendLine("Torso Speed: " + mechChassisDefinition.GetSpeedTorsoTwist(enginePower).ToString("0.#"));

        stringBuilder.AppendLine("Heat Limit: " + (heatLimit + heatLimitBonus).ToString("0.#") + "K");

        stringBuilder.AppendLine("Cooling: " + cooling.ToString("0.#") + "K/s");

        if (coolant > 0)
        {
            stringBuilder.AppendLine("Coolant: " + coolant.ToString("0.#") + "kl");
        }

        stringBuilder.AppendLine("");

        stringBuilder.AppendLine("Weapons:");

        foreach(KeyValuePair<string, int> keyVal in weapons)
        {
            stringBuilder.AppendLine(keyVal.Value.ToString() + " " + keyVal.Key);
        }

        return stringBuilder.ToString();
    }

    public int GetMarketValue()
    {
        int value = GetMechChassisDefinition().MarketValue;

        foreach (ComponentDefinition componentDefinition in GetAllComponentDefinitions())
        {
            value += componentDefinition.MarketValue;
        }

        return value;
    }

    public string GetComponentsDisplay()
    {
        StringBuilder stringBuilder = new StringBuilder();

        List<ComponentDefinition> headComponents = GetComponentDefinitionsFromComponentGroups(ComponentGroupsHead);
        List<ComponentDefinition> torsoCenterComponents = GetComponentDefinitionsFromComponentGroups(ComponentGroupsTorsoCenter);
        List<ComponentDefinition> torsoLeftComponents = GetComponentDefinitionsFromComponentGroups(ComponentGroupsTorsoLeft);
        List<ComponentDefinition> torsoRightComponents = GetComponentDefinitionsFromComponentGroups(ComponentGroupsTorsoRight);
        List<ComponentDefinition> armLeftComponents = GetComponentDefinitionsFromComponentGroups(ComponentGroupsArmLeft);
        List<ComponentDefinition> armRightComponents = GetComponentDefinitionsFromComponentGroups(ComponentGroupsArmRight);
        List<ComponentDefinition> legLeftComponents = GetComponentDefinitionsFromComponentGroups(ComponentGroupsLegLeft);
        List<ComponentDefinition> legRightComponents = GetComponentDefinitionsFromComponentGroups(ComponentGroupsLegRight);

        if (headComponents.Count > 0)
        {
            stringBuilder.AppendLine("Head Components:");

            foreach (ComponentDefinition componentDefinition in headComponents)
            {
                stringBuilder.AppendLine(componentDefinition.GetDisplayName());
            }

            stringBuilder.AppendLine("");
        }

        if (torsoCenterComponents.Count > 0)
        {
            stringBuilder.AppendLine("Torso Center Components:");

            foreach (ComponentDefinition componentDefinition in torsoCenterComponents)
            {
                stringBuilder.AppendLine(componentDefinition.GetDisplayName());
            }

            stringBuilder.AppendLine("");
        }

        if (torsoLeftComponents.Count > 0)
        {
            stringBuilder.AppendLine("Torso Left Components:");

            foreach (ComponentDefinition componentDefinition in torsoLeftComponents)
            {
                stringBuilder.AppendLine(componentDefinition.GetDisplayName());
            }

            stringBuilder.AppendLine("");
        }

        if (torsoRightComponents.Count > 0)
        {
            stringBuilder.AppendLine("Torso Right Components:");

            foreach (ComponentDefinition componentDefinition in torsoRightComponents)
            {
                stringBuilder.AppendLine(componentDefinition.GetDisplayName());
            }

            stringBuilder.AppendLine("");
        }

        if (armLeftComponents.Count > 0)
        {
            stringBuilder.AppendLine("Arm Left Components:");

            foreach (ComponentDefinition componentDefinition in armLeftComponents)
            {
                stringBuilder.AppendLine(componentDefinition.GetDisplayName());
            }

            stringBuilder.AppendLine("");
        }

        if (armRightComponents.Count > 0)
        {
            stringBuilder.AppendLine("Arm Right Components:");

            foreach (ComponentDefinition componentDefinition in armRightComponents)
            {
                stringBuilder.AppendLine(componentDefinition.GetDisplayName());
            }

            stringBuilder.AppendLine("");
        }

        if (legLeftComponents.Count > 0)
        {
            stringBuilder.AppendLine("Leg Left Components:");

            foreach (ComponentDefinition componentDefinition in legLeftComponents)
            {
                stringBuilder.AppendLine(componentDefinition.GetDisplayName());
            }

            stringBuilder.AppendLine("");
        }

        if (legRightComponents.Count > 0)
        {
            stringBuilder.AppendLine("Leg Right Components:");

            foreach (ComponentDefinition componentDefinition in legRightComponents)
            {
                stringBuilder.AppendLine(componentDefinition.GetDisplayName());
            }

            stringBuilder.AppendLine("");
        }

        return stringBuilder.ToString();
    }
}
