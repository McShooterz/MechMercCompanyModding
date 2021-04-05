using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;

public class MechData : CombatUnitData
{
    public Guid guid = Guid.Empty;

    [Header("Mech Data")]

    #region Variables
    //public MechDesign mechDesign;

    public string designName;

    [HideInInspector]
    public MechController mechController;

    [HideInInspector]
    public MechPilot currentMechPilot;

    public MechPaintScheme mechPaintScheme;

    public MechCauseOfDestructionType causeOfDestruction;

    public bool pilotKilled = false;

    // Not used yet
    public MechSectionData mechSectionHead;
    public MechSectionTwoSidedData mechSectionTorsoCenter;
    public MechSectionTwoSidedData mechSectionTorsoLeft;
    public MechSectionTwoSidedData mechSectionTorsoRight;
    public MechSectionData mechSectionArmLeft;
    public MechSectionData mechSectionArmRight;
    public MechSectionData mechSectionLegLeft;
    public MechSectionData mechSectionLegRight;

    public ArmorType armorTypeHead;
    public ArmorType armorTypeTorsoCenter;
    public ArmorType armorTypeTorsoLeft;
    public ArmorType armorTypeTorsoRight;
    public ArmorType armorTypeArmLeft;
    public ArmorType armorTypeArmRight;
    public ArmorType armorTypeLegLeft;
    public ArmorType armorTypeLegRight;

    public ComponentData[] componentsHead = new ComponentData[0];
    public ComponentData[] componentsTorsoCenter = new ComponentData[0];
    public ComponentData[] componentsTorsoLeft = new ComponentData[0];
    public ComponentData[] componentsTorsoRight = new ComponentData[0];
    public ComponentData[] componentsArmLeft = new ComponentData[0];
    public ComponentData[] componentsArmRight = new ComponentData[0];
    public ComponentData[] componentsLegLeft = new ComponentData[0];
    public ComponentData[] componentsLegRight = new ComponentData[0];
    public ComponentData[] componentsAll = new ComponentData[0];

    public AmmoPool[] ammoPoolsHead = new AmmoPool[0];
    public AmmoPool[] ammoPoolsTorsoCenter = new AmmoPool[0];
    public AmmoPool[] ammoPoolsTorsoLeft = new AmmoPool[0];
    public AmmoPool[] ammoPoolsTorsoRight = new AmmoPool[0];
    public AmmoPool[] ammoPoolsArmLeft = new AmmoPool[0];
    public AmmoPool[] ammoPoolsArmRight = new AmmoPool[0];

    public float armorHead;
    public float armorTorsoCenter;
    public float armorTorsoCenterRear;
    public float armorTorsoLeft;
    public float armorTorsoLeftRear;
    public float armorTorsoRight;
    public float armorTorsoRightRear;
    public float armorLegLeft;
    public float armorLegRight;
    public float armorArmLeft;
    public float armorArmRight;

    int armorMaxHead;
    int armorMaxTorsoCenter;
    int armorMaxTorsoCenterRear;
    int armorMaxTorsoLeft;
    int armorMaxTorsoLeftRear;
    int armorMaxTorsoRight;
    int armorMaxTorsoRightRear;
    int armorMaxLegLeft;
    int armorMaxLegRight;
    int armorMaxArmLeft;
    int armorMaxArmRight;

    public float internalHead;
    public float internalTorsoCenter;
    public float internalTorsoLeft;
    public float internalTorsoRight;
    public float internalLegLeft;
    public float internalLegRight;
    public float internalArmLeft;
    public float internalArmRight;

    float internalMaxHead;
    float internalMaxTorsoCenter;
    float internalMaxTorsoLeft;
    float internalMaxTorsoRight;
    float internalMaxLegLeft;
    float internalMaxLegRight;
    float internalMaxArmLeft;
    float internalMaxArmRight;

    public float reactorPower;
    public float heatLimit;
    public float cooling;

    public float coolantMax = 0f;

    public float heatWarning;
    public float heatShutdown;
    public float heatDamageMinor;
    public float heatDamageMajor;

    public float jumpTrust;
    public float jumpCapacity;
    public float jumpJetRecharge;
    public float jumpHeat;

    public float recoilModifier;
    
    public float movementSpeedForward;
    public float movementSpeedReverse;
    public float movementSpeedTurn;

    public float accelerationForward;
    public float accelerationReverse;
    public float deacceleration;

    public float torsoTwistSpeed;
    public float torsoPitchSpeed;
    #endregion

    #region Properties
    public MechChassisDefinition MechChassis { get; private set; }

    public MechDesign MechDesign
    {
        get
        {
            List<ComponentGroup> componentGroupsHead = GetComponentGroups(componentsHead, MechChassis.HeadSlotGroups.Length);
            List<ComponentGroup> componentGroupsTorsoCenter = GetComponentGroups(componentsTorsoCenter, MechChassis.TorsoCenterSlotGroups.Length);
            List<ComponentGroup> componentGroupsTorsoLeft = GetComponentGroups(componentsTorsoLeft, MechChassis.TorsoLeftSlotGroups.Length);
            List<ComponentGroup> componentGroupsTorsoRight = GetComponentGroups(componentsTorsoRight, MechChassis.TorsoRightSlotGroups.Length);
            List<ComponentGroup> componentGroupsArmLeft = GetComponentGroups(componentsArmLeft, MechChassis.ArmLeftSlotGroups.Length);
            List<ComponentGroup> componentGroupsArmRight = GetComponentGroups(componentsArmRight, MechChassis.ArmRightSlotGroups.Length);
            List<ComponentGroup> componentGroupsLegLeft = GetComponentGroups(componentsLegLeft, MechChassis.LegLeftSlotGroups.Length);
            List<ComponentGroup> componentGroupsLegRight = GetComponentGroups(componentsLegRight, MechChassis.LegRightSlotGroups.Length);

            MechDesign mechDesign = new MechDesign()
            {
                DesignName = customName,

                MechChassisDefinition = MechChassis.Key,

                ArmorTypeHead = armorTypeHead,
                ArmorPointsHead = armorMaxHead,

                ArmorTypeTorsoCenter = armorTypeTorsoCenter,
                ArmorPointsTorsoCenter = armorMaxTorsoCenter,
                ArmorPointsTorsoCenterRear = armorMaxTorsoCenterRear,

                ArmorTypeTorsoLeft = armorTypeTorsoLeft,
                ArmorPointsTorsoLeft = armorMaxTorsoLeft,
                ArmorPointsTorsoLeftRear = armorMaxTorsoLeftRear,

                ArmorTypeTorsoRight = armorTypeTorsoRight,
                ArmorPointsTorsoRight = armorMaxTorsoRight,
                ArmorPointsTorsoRightRear = armorMaxTorsoRightRear,

                ArmorTypeArmLeft = armorTypeArmLeft,
                ArmorPointsArmLeft = armorMaxArmLeft,

                ArmorTypeArmRight = armorTypeArmRight,
                ArmorPointsArmRight = armorMaxArmRight,

                ArmorTypeLegLeft = armorTypeLegLeft,
                ArmorPointsLegLeft = armorMaxLegLeft,

                ArmorTypeLegRight = armorTypeLegRight,
                ArmorPointsLegRight = armorMaxLegRight,

                ComponentGroupsHead = componentGroupsHead.ToArray(),
                ComponentGroupsTorsoCenter = componentGroupsTorsoCenter.ToArray(),
                ComponentGroupsTorsoLeft = componentGroupsTorsoLeft.ToArray(),
                ComponentGroupsTorsoRight = componentGroupsTorsoRight.ToArray(),
                ComponentGroupsArmLeft = componentGroupsArmLeft.ToArray(),
                ComponentGroupsArmRight = componentGroupsArmRight.ToArray(),
                ComponentGroupsLegLeft = componentGroupsLegLeft.ToArray(),
                ComponentGroupsLegRight = componentGroupsLegRight.ToArray(),
            };

            return mechDesign;
        }
    }

    public GameObject MechPrefab { get => MechChassis.GetMechPrefab(); }

    public GameObject CockpitPrefab { get => MechChassis.GetCockpitPrefab(); }

    public float ArmorPercentHead { get => armorHead / armorMaxHead; }

    public float ArmorPercentTorsoCenter { get => armorTorsoCenter / armorMaxTorsoCenter; }

    public float ArmorPercentTorsoCenterRear { get => armorTorsoCenterRear / armorMaxTorsoCenterRear; }

    public float ArmorPercentTorsoLeft { get => armorTorsoLeft / armorMaxTorsoLeft; }

    public float ArmorPercentTorsoLeftRear { get => armorTorsoLeftRear / armorMaxTorsoLeftRear; }

    public float ArmorPercentTorsoRight { get => armorTorsoRight / armorMaxTorsoRight; }

    public float ArmorPercentTorsoRightRear { get => armorTorsoRightRear / armorMaxTorsoRightRear; }

    public float ArmorPercentArmLeft { get => armorArmLeft / armorMaxArmLeft; }

    public float ArmorPercentArmRight { get => armorArmRight / armorMaxArmRight; }

    public float ArmorPercentLegLeft { get => armorLegLeft / armorMaxLegLeft; }

    public float ArmorPercentLegRight { get => armorLegRight / armorMaxLegRight; }

    public float InternalPercentHead { get => internalHead / internalMaxHead; }

    public float InternalPercentTorsoCenter { get => internalTorsoCenter / internalMaxTorsoCenter; }

    public float InternalPercentTorsoLeft { get => internalTorsoLeft / internalMaxTorsoLeft; }

    public float InternalPercentTorsoRight { get => internalTorsoRight / internalMaxTorsoRight; }

    public float InternalPercentArmLeft { get => internalArmLeft / internalMaxArmLeft; }

    public float InternalPercentArmRight { get => internalArmRight / internalMaxArmRight; }

    public float InternalPercentLegLeft { get => internalLegLeft / internalMaxLegLeft; }

    public float InternalPercentLegRight { get => internalLegRight / internalMaxLegRight; }

    public float InternalTotal { get => Mathf.Max(internalHead, 0) + Mathf.Max(internalTorsoCenter, 0) + Mathf.Max(internalTorsoLeft, 0) + Mathf.Max(internalTorsoRight, 0) + Mathf.Max(internalArmLeft, 0) + Mathf.Max(internalArmRight, 0) + Mathf.Max(internalLegLeft, 0) + Mathf.Max(internalLegRight, 0); }

    public float InternalMaxTotal { get => internalMaxHead + internalMaxTorsoCenter + internalMaxTorsoLeft + internalMaxTorsoRight + internalMaxArmLeft + internalMaxArmRight + internalMaxLegLeft + internalMaxLegRight; }

    public float InternalPercentageTotal { get => InternalTotal / InternalMaxTotal; }

    public int TotalArmor { get => armorMaxHead + armorMaxTorsoCenter + armorMaxTorsoCenterRear + armorMaxTorsoLeft + armorMaxTorsoLeftRear + armorMaxTorsoRight + armorMaxTorsoRightRear + armorMaxArmLeft + armorMaxArmRight + armorMaxLegLeft + armorMaxLegRight; }

    public bool HeadDestroyed { get => internalHead <= 0; }

    public bool TorsoCenterDestroyed { get => internalTorsoCenter <= 0; }

    public bool TorsoLeftDestroyed { get => internalTorsoLeft <= 0; }

    public bool TorsoRightDestroyed { get => internalTorsoRight <= 0; }

    public bool ArmLeftDestroyed { get => internalArmLeft <= 0; }

    public bool ArmRightDestroyed { get => internalArmRight <= 0; }

    public bool LegLeftDestroyed { get => internalLegLeft <= 0; }

    public bool LegRightDestroyed { get => internalLegRight <= 0; }

    public bool HasBothLegs { get => internalLegLeft > 0 && internalLegRight > 0; }

    public bool IsCrippled { get => HeadDestroyed || TorsoCenterDestroyed || (LegLeftDestroyed && LegRightDestroyed); }

    public bool IsDamaged { get => internalHead < internalMaxHead || internalTorsoCenter < internalMaxTorsoCenter || internalTorsoLeft < internalMaxTorsoLeft || internalTorsoRight < internalMaxTorsoRight || internalArmLeft < internalMaxArmLeft || internalArmRight < internalMaxArmRight || internalLegLeft < internalMaxLegLeft || internalLegRight < internalMaxLegRight; }

    public bool HasValidDesign { get => reactorPower > 0; }

    public bool HasJumpJets { get => jumpTrust > 0.0f; }

    public float HeadDamage { get => Mathf.Max(internalMaxHead - Mathf.Max(internalHead, 0)); }

    public float TorsoCenterDamage { get => Mathf.Max(internalMaxTorsoCenter - Mathf.Max(internalTorsoCenter, 0), 0); }

    public float TorsoLeftDamage { get => Mathf.Max(internalMaxTorsoLeft - Mathf.Max(internalTorsoLeft, 0), 0); }

    public float TorsoRightDamage { get => Mathf.Max(internalMaxTorsoRight - Mathf.Max(internalTorsoRight, 0), 0); }

    public float ArmLeftDamage { get => Mathf.Max(internalMaxArmLeft - Mathf.Max(internalArmLeft, 0), 0); }

    public float ArmRightDamage { get => Mathf.Max(internalMaxArmRight - Mathf.Max(internalArmRight, 0), 0); }

    public float LegLeftDamage { get => Mathf.Max(internalMaxLegLeft - Mathf.Max(internalLegLeft, 0), 0); }

    public float LegRightDamage { get => Mathf.Max(internalMaxLegRight - Mathf.Max(internalLegRight, 0), 0); }

    public MechStatusType MechStatus
    {
        get
        {
            if (isDestroyed || IsCrippled)
                return MechStatusType.Crippled;

            if (!HasValidDesign)
                return MechStatusType.InvalidDesign;

            if (IsDamaged)
                return MechStatusType.Damaged;

            return MechStatusType.Ready;
        }
    }

    public string MechStatusDisplay { get => StaticHelper.GetMechStatusName(MechStatus); }

    public bool IsMissionReady
    {
        get
        {
            if (isDestroyed || IsCrippled || !HasValidDesign)
                return false;

            return true;
        }
    }

    public float RecoilModifier { get => recoilModifier; }

    public string HoveredDisplay
    {
        get
        {
            System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
            Dictionary<string, int> weapons = new Dictionary<string, int>();

            foreach (ComponentData componentData in componentsAll)
            {
                if (componentData.isDestroyed)
                    continue;

                WeaponDefinition weaponDefinition = componentData.ComponentDefinition.GetWeaponDefinition();

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

            stringBuilder.AppendLine("Chassis: " + MechChassis.GetDisplayName());

            stringBuilder.AppendLine("Tonnage: " + MechChassis.Tonnage.ToString() + "T");

            stringBuilder.AppendLine("Armor: " + TotalArmor.ToString());

            stringBuilder.AppendLine("Movement Speed: " + MechChassis.GetDisplaySpeedForward(reactorPower).ToString("0.0") + " / " + MechChassis.GetDisplaySpeedReverse(reactorPower).ToString("0.0") + " KPH");

            stringBuilder.AppendLine();

            if (weapons.Count > 0)
            {
                stringBuilder.AppendLine("Weapons:");

                foreach (KeyValuePair<string, int> keyVal in weapons)
                {
                    stringBuilder.AppendLine(keyVal.Value.ToString() + " " + keyVal.Key);
                }

                stringBuilder.AppendLine();
            }

            stringBuilder.AppendLine("Head: " + (InternalPercentHead).ToString("0." + "%"));

            stringBuilder.AppendLine("Center Torso: " + (InternalPercentTorsoCenter).ToString("0." + "%"));

            stringBuilder.AppendLine("Left Torso: " + (InternalPercentTorsoLeft).ToString("0." + "%"));

            stringBuilder.AppendLine("Right Torso: " + (InternalPercentTorsoRight).ToString("0." + "%"));

            stringBuilder.AppendLine("Left Arm: " + (InternalPercentArmLeft).ToString("0." + "%"));

            stringBuilder.AppendLine("Right Arm: " + (InternalPercentArmRight).ToString("0." + "%"));

            stringBuilder.AppendLine("Left Leg:" + (InternalPercentLegLeft).ToString("0." + "%"));

            stringBuilder.AppendLine("Right Leg" + (InternalPercentLegRight).ToString("0." + "%"));

            return stringBuilder.ToString();
        }
    }

    public override List<ComponentDefinition> ComponentsSalvagable
    {
        get
        {
            List<ComponentDefinition> components = new List<ComponentDefinition>();

            foreach (ComponentData componentData in componentsAll)
            {
                if (!componentData.isDestroyed)
                {
                    components.Add(componentData.ComponentDefinition);
                }
            }

            return components;
        }
    }

    public List<ComponentDefinition> ComponentsDestroyed
    {
        get
        {
            List<ComponentDefinition> components = new List<ComponentDefinition>();

            foreach (ComponentData componentData in componentsAll)
            {
                if (componentData.isDestroyed)
                {
                    components.Add(componentData.ComponentDefinition);
                }
            }

            return components;
        }
    }

    public List<ComponentData> ComponentDatasDestroyed { get => componentsAll.Where(componentdata => componentdata.isDestroyed).ToList(); }

    public int MarketValue { get => Mathf.CeilToInt(InternalPercentageTotal * MechChassis.MarketValue); }

    public int ComponentValue
    {
        get
        {
            int sum = 0;

            for (int i = 0; i < componentsAll.Length; i++)
            {
                ComponentData componentData = componentsAll[i];

                if (componentData.isDestroyed)
                    continue;

                sum += componentData.ComponentDefinition.MarketValue;
            }

            return sum;
        }
    }

    public MechSave MechSave
    {
        get
        {
            return new MechSave
            {
                Guid = guid,

                MechChassis = MechChassis.Key,

                CustomName = customName,

                DesignName = designName,

                MechPaintScheme = mechPaintScheme,

                ArmorTypeHead = armorTypeHead,
                ArmorTypeTorsoCenter = armorTypeTorsoCenter,
                ArmorTypeTorsoLeft = armorTypeTorsoLeft,
                ArmorTypeTorsoRight = armorTypeTorsoRight,
                ArmorTypeArmLeft = armorTypeArmLeft,
                ArmorTypeArmRight = armorTypeArmRight,
                ArmorTypeLegLeft = armorTypeLegLeft,
                ArmorTypeLegRight = armorTypeLegRight,

                ArmorMaxHead = armorMaxHead,
                ArmorMaxTorsoCenter = armorMaxTorsoCenter,
                ArmorMaxTorsoCenterRear = armorMaxTorsoCenterRear,
                ArmorMaxTorsoLeft = armorMaxTorsoLeft,
                ArmorMaxTorsoLeftRear = armorMaxTorsoLeftRear,
                ArmorMaxTorsoRight = armorMaxTorsoRight,
                ArmorMaxTorsoRightRear = armorMaxTorsoRightRear,
                ArmorMaxLegLeft = armorMaxLegLeft,
                ArmorMaxLegRight = armorMaxLegRight,
                ArmorMaxArmLeft = armorMaxArmLeft,
                ArmorMaxArmRight = armorMaxArmRight,

                InternalHead = internalHead,
                InternalTorsoCenter = internalTorsoCenter,
                InternalTorsoLeft = internalTorsoLeft,
                InternalTorsoRight = internalTorsoRight,
                InternalArmLeft = internalArmLeft,
                InternalArmRight = internalArmRight,
                InternalLegLeft = internalLegLeft,
                InternalLegRight = internalLegRight,

                ComponentsHead = ComponentSave.GetComponentSaves(componentsHead),
                ComponentsTorsoCenter = ComponentSave.GetComponentSaves(componentsTorsoCenter),
                ComponentsTorsoLeft = ComponentSave.GetComponentSaves(componentsTorsoLeft),
                ComponentsTorsoRight = ComponentSave.GetComponentSaves(componentsTorsoRight),
                ComponentsArmLeft = ComponentSave.GetComponentSaves(componentsArmLeft),
                ComponentsArmRight = ComponentSave.GetComponentSaves(componentsArmRight),
                ComponentsLegLeft = ComponentSave.GetComponentSaves(componentsLegLeft),
                ComponentsLegRight = ComponentSave.GetComponentSaves(componentsLegRight),
            };
        }
    }
    #endregion

    public MechData(MechDesign design)
    {
        MechChassis = design.GetMechChassisDefinition();
        customName = design.DesignName;

        mechPaintScheme = new MechPaintScheme();

        BuildFromDesign(design);
    }

    public MechData(MechSave mechSave)
    {
        guid = mechSave.Guid;

        MechChassis = mechSave.GetMechChassisDefinition();

        customName = mechSave.CustomName;
        designName = mechSave.DesignName;

        mechPaintScheme = mechSave.MechPaintScheme.Clone();

        armorTypeHead = mechSave.ArmorTypeHead;
        armorTypeTorsoCenter = mechSave.ArmorTypeTorsoCenter;
        armorTypeTorsoLeft = mechSave.ArmorTypeTorsoLeft;
        armorTypeTorsoRight = mechSave.ArmorTypeTorsoRight;
        armorTypeArmLeft = mechSave.ArmorTypeArmLeft;
        armorTypeArmRight = mechSave.ArmorTypeArmRight;
        armorTypeLegLeft = mechSave.ArmorTypeLegLeft;
        armorTypeLegRight = mechSave.ArmorTypeLegRight;

        armorMaxHead = mechSave.ArmorMaxHead;
        armorMaxTorsoCenter = mechSave.ArmorMaxTorsoCenter;
        armorMaxTorsoCenterRear = mechSave.ArmorMaxTorsoCenterRear;
        armorMaxTorsoLeft = mechSave.ArmorMaxTorsoLeft;
        armorMaxTorsoLeftRear = mechSave.ArmorMaxTorsoLeftRear;
        armorMaxTorsoRight = mechSave.ArmorMaxTorsoRight;
        armorMaxTorsoRightRear = mechSave.ArmorMaxTorsoRightRear;
        armorMaxArmLeft = mechSave.ArmorMaxArmLeft;
        armorMaxArmRight = mechSave.ArmorMaxArmRight;
        armorMaxLegLeft = mechSave.ArmorMaxLegLeft;
        armorMaxLegRight = mechSave.ArmorMaxLegRight;

        componentsHead = ComponentData.BuildComponentDatas(mechSave.ComponentsHead);
        componentsTorsoCenter = ComponentData.BuildComponentDatas(mechSave.ComponentsTorsoCenter);
        componentsTorsoLeft = ComponentData.BuildComponentDatas(mechSave.ComponentsTorsoLeft);
        componentsTorsoRight = ComponentData.BuildComponentDatas(mechSave.ComponentsTorsoRight);
        componentsArmLeft = ComponentData.BuildComponentDatas(mechSave.ComponentsArmLeft);
        componentsArmRight = ComponentData.BuildComponentDatas(mechSave.ComponentsArmRight);
        componentsLegLeft = ComponentData.BuildComponentDatas(mechSave.ComponentsLegLeft);
        componentsLegRight = ComponentData.BuildComponentDatas(mechSave.ComponentsLegRight);

        List<ComponentData> componentsAllList = new List<ComponentData>();

        componentsAllList.AddRange(componentsHead);
        componentsAllList.AddRange(componentsTorsoCenter);
        componentsAllList.AddRange(componentsTorsoLeft);
        componentsAllList.AddRange(componentsTorsoRight);
        componentsAllList.AddRange(componentsArmLeft);
        componentsAllList.AddRange(componentsArmRight);
        componentsAllList.AddRange(componentsLegLeft);
        componentsAllList.AddRange(componentsLegRight);

        componentsAll = componentsAllList.ToArray();

        ammoPoolsHead = BuildAmmoPools(componentsHead);
        ammoPoolsTorsoCenter = BuildAmmoPools(componentsTorsoCenter);
        ammoPoolsTorsoLeft = BuildAmmoPools(componentsTorsoLeft);
        ammoPoolsTorsoRight = BuildAmmoPools(componentsTorsoRight);
        ammoPoolsArmLeft = BuildAmmoPools(componentsArmLeft);
        ammoPoolsArmRight = BuildAmmoPools(componentsArmRight);

        float internalHeadBonus = 0;
        float internalTorsoCenterBonus = 0;
        float internalTorsoLeftBonus = 0;
        float internalTorsoRightBonus = 0;
        float internalArmLeftBonus = 0;
        float internalArmRightBonus = 0;
        float internalLegLeftBonus = 0;
        float internalLegRightBonus = 0;

        foreach (ComponentData componentData in componentsHead)
        {
            internalHeadBonus += componentData.ComponentDefinition.InternalBonus;
        }

        foreach (ComponentData componentData in componentsTorsoCenter)
        {
            internalTorsoCenterBonus += componentData.ComponentDefinition.InternalBonus;
        }

        foreach (ComponentData componentData in componentsTorsoLeft)
        {
            internalTorsoLeftBonus += componentData.ComponentDefinition.InternalBonus;
        }

        foreach (ComponentData componentData in componentsTorsoRight)
        {
            internalTorsoRightBonus += componentData.ComponentDefinition.InternalBonus;
        }

        foreach (ComponentData componentData in componentsArmLeft)
        {
            internalArmLeftBonus += componentData.ComponentDefinition.InternalBonus;
        }

        foreach (ComponentData componentData in componentsArmRight)
        {
            internalArmRightBonus += componentData.ComponentDefinition.InternalBonus;
        }

        foreach (ComponentData componentData in componentsLegLeft)
        {
            internalLegLeftBonus += componentData.ComponentDefinition.InternalBonus;
        }

        foreach (ComponentData componentData in componentsLegRight)
        {
            internalLegRightBonus += componentData.ComponentDefinition.InternalBonus;
        }

        internalMaxHead = MechChassis.HeadInternal + internalHeadBonus;
        internalMaxTorsoCenter = MechChassis.TorsoCenterInternal + internalTorsoCenterBonus;
        internalMaxTorsoLeft = MechChassis.TorsoLeftInternal + internalTorsoLeftBonus;
        internalMaxTorsoRight = MechChassis.TorsoRightInternal + internalTorsoRightBonus;
        internalMaxArmLeft = MechChassis.ArmLeftInternal + internalArmLeftBonus;
        internalMaxArmRight = MechChassis.ArmRightInternal + internalArmRightBonus;
        internalMaxLegLeft = MechChassis.LegLeftInternal + internalLegLeftBonus;
        internalMaxLegRight = MechChassis.LegRightInternal + internalLegRightBonus;

        internalHead = Mathf.Clamp(mechSave.InternalHead, 0.0f, internalMaxHead);
        internalTorsoCenter = Mathf.Clamp(mechSave.InternalTorsoCenter, 0.0f, internalMaxTorsoCenter);
        internalTorsoLeft = Mathf.Clamp(mechSave.InternalTorsoLeft, 0.0f, internalMaxTorsoLeft);
        internalTorsoRight = Mathf.Clamp(mechSave.InternalTorsoRight, 0.0f, internalMaxTorsoRight);
        internalArmLeft = Mathf.Clamp(mechSave.InternalArmLeft, 0.0f, internalMaxArmLeft);
        internalArmRight = Mathf.Clamp(mechSave.InternalArmRight, 0.0f, internalMaxArmRight);
        internalLegLeft = Mathf.Clamp(mechSave.InternalLegLeft, 0.0f, internalMaxLegLeft);
        internalLegRight = Mathf.Clamp(mechSave.InternalLegRight, 0.0f, internalMaxLegRight);

        RecalculateStats();
    }

    public void BuildFromDesign(MechDesign design)
    {
        designName = design.DesignName;

        // Set Armor
        armorTypeHead = design.ArmorTypeHead;
        armorTypeTorsoCenter = design.ArmorTypeTorsoCenter;
        armorTypeTorsoLeft = design.ArmorTypeTorsoLeft;
        armorTypeTorsoRight = design.ArmorTypeTorsoRight;
        armorTypeArmLeft = design.ArmorTypeArmLeft;
        armorTypeArmRight = design.ArmorTypeArmRight;
        armorTypeLegLeft = design.ArmorTypeLegLeft;
        armorTypeLegRight = design.ArmorTypeLegRight;

        armorHead = design.ArmorPointsHead;
        armorMaxHead = design.ArmorPointsHead;

        armorTorsoCenter = design.ArmorPointsTorsoCenter;
        armorTorsoCenterRear = design.ArmorPointsTorsoCenterRear;
        armorMaxTorsoCenter = design.ArmorPointsTorsoCenter;
        armorMaxTorsoCenterRear = design.ArmorPointsTorsoCenterRear;
        
        armorTorsoLeft = design.ArmorPointsTorsoLeft;
        armorTorsoLeftRear = design.ArmorPointsTorsoLeftRear;
        armorMaxTorsoLeft = design.ArmorPointsTorsoLeft;
        armorMaxTorsoLeftRear = design.ArmorPointsTorsoLeftRear;

        armorTorsoRight = design.ArmorPointsTorsoRight;
        armorTorsoRightRear = design.ArmorPointsTorsoRightRear;
        armorMaxTorsoRight = design.ArmorPointsTorsoRight;
        armorMaxTorsoRightRear = design.ArmorPointsTorsoRightRear;

        armorArmLeft = design.ArmorPointsArmLeft;
        armorMaxArmLeft = design.ArmorPointsArmLeft;

        armorArmRight = design.ArmorPointsArmRight;
        armorMaxArmRight = design.ArmorPointsArmRight;

        armorLegLeft = design.ArmorPointsLegLeft;
        armorMaxLegLeft = design.ArmorPointsLegLeft;

        armorLegRight = design.ArmorPointsLegRight;
        armorMaxLegRight = design.ArmorPointsLegRight;

        internalHead = MechChassis.HeadInternal;        
        internalTorsoCenter = MechChassis.TorsoCenterInternal;        
        internalTorsoLeft = MechChassis.TorsoLeftInternal;        
        internalTorsoRight = MechChassis.TorsoRightInternal;        
        internalArmLeft = MechChassis.ArmLeftInternal;        
        internalArmRight = MechChassis.ArmRightInternal;        
        internalLegLeft = MechChassis.LegLeftInternal;       
        internalLegRight = MechChassis.LegRightInternal;       

        componentsHead = ComponentData.BuildComponentDatas(design.ComponentGroupsHead);
        componentsTorsoCenter = ComponentData.BuildComponentDatas(design.ComponentGroupsTorsoCenter);
        componentsTorsoLeft = ComponentData.BuildComponentDatas(design.ComponentGroupsTorsoLeft);
        componentsTorsoRight = ComponentData.BuildComponentDatas(design.ComponentGroupsTorsoRight);
        componentsArmLeft = ComponentData.BuildComponentDatas(design.ComponentGroupsArmLeft);
        componentsArmRight = ComponentData.BuildComponentDatas(design.ComponentGroupsArmRight);
        componentsLegLeft = ComponentData.BuildComponentDatas(design.ComponentGroupsLegLeft);
        componentsLegRight = ComponentData.BuildComponentDatas(design.ComponentGroupsLegRight);

        List<ComponentData> componentsAllList = new List<ComponentData>();
        componentsAllList.AddRange(componentsHead);
        componentsAllList.AddRange(componentsTorsoCenter);
        componentsAllList.AddRange(componentsTorsoLeft);
        componentsAllList.AddRange(componentsTorsoRight);
        componentsAllList.AddRange(componentsArmLeft);
        componentsAllList.AddRange(componentsArmRight);
        componentsAllList.AddRange(componentsLegLeft);
        componentsAllList.AddRange(componentsLegRight);
        componentsAll = componentsAllList.ToArray();

        ammoPoolsHead = BuildAmmoPools(componentsHead);
        ammoPoolsTorsoCenter = BuildAmmoPools(componentsTorsoCenter);
        ammoPoolsTorsoLeft = BuildAmmoPools(componentsTorsoLeft);
        ammoPoolsTorsoRight = BuildAmmoPools(componentsTorsoRight);
        ammoPoolsArmLeft = BuildAmmoPools(componentsArmLeft);
        ammoPoolsArmRight = BuildAmmoPools(componentsArmRight);

        RecalculateStats();

        RepairIntenals(true, true, true, true, true, true, true, true);
    }

    public void RecalculateStats()
    {
        radarDetectionReduction = 0.0f;

        lockOnBonus = 0.0f;

        coolantMax = 0.0f;
        reactorPower = 0.0f;
        heatLimit = 0.0f;
        float heatLimitBonus = 0.0f;
        cooling = 0.0f;

        jumpTrust = 0.0f;
        jumpCapacity = 4.0f;
        jumpJetRecharge = 0.0f;

        recoilModifier = MechChassis.RecoilModifier;
        float recoilModifierBonus = 0.0f;

        movementSpeedForward = 0.0f;
        movementSpeedReverse = 0.0f;
        movementSpeedTurn = 0.0f;
        accelerationForward = 0.0f;
        accelerationReverse = 0.0f;
        deacceleration = 0.0f;

        if (armorTypeHead == ArmorType.stealth)
        {
            radarDetectionReduction += ResourceManager.Instance.GameConstants.StealthArmorSignatureReductionHead;
        }

        if (armorTypeTorsoCenter == ArmorType.stealth)
        {
            radarDetectionReduction += ResourceManager.Instance.GameConstants.StealthArmorSignatureReductionTorsoCenter;
        }

        if (armorTypeTorsoLeft == ArmorType.stealth)
        {
            radarDetectionReduction += ResourceManager.Instance.GameConstants.StealthArmorSignatureReductionTorsoSide;
        }

        if (armorTypeTorsoRight == ArmorType.stealth)
        {
            radarDetectionReduction += ResourceManager.Instance.GameConstants.StealthArmorSignatureReductionTorsoSide;
        }

        if (armorTypeLegLeft == ArmorType.stealth)
        {
            radarDetectionReduction += ResourceManager.Instance.GameConstants.StealthArmorSignatureReductionLeg;
        }

        if (armorTypeLegRight == ArmorType.stealth)
        {
            radarDetectionReduction += ResourceManager.Instance.GameConstants.StealthArmorSignatureReductionLeg;
        }

        if (armorTypeArmLeft == ArmorType.stealth)
        {
            radarDetectionReduction += ResourceManager.Instance.GameConstants.StealthArmorSignatureReductionArm;
        }

        if (armorTypeArmRight == ArmorType.stealth)
        {
            radarDetectionReduction += ResourceManager.Instance.GameConstants.StealthArmorSignatureReductionArm;
        }

        for (int i = 0; i < componentsAll.Length; i++)
        {
            ComponentData componentData = componentsAll[i];

            if (componentData.isDestroyed)
            {
                continue;
            }

            ComponentDefinition component = componentData.ComponentDefinition;

            heatLimitBonus += component.HeatLimitBonus;
            coolantMax += component.Coolant;
            cooling += component.Cooling;
            reactorPower += component.EnginePower;
            radarDetectionReduction += component.RadarSignatureReduction;
            radarDetectionRange += component.RadarRangeBonus;
            jumpTrust += component.JumpJetThrust;
            jumpCapacity += component.JumpJetCapacity;
            jumpJetRecharge += component.JumpJetRecharge;
            lockOnBonus += component.TargetLockOnBonus;

            if (component.HeatLimit > heatLimit)
            {
                heatLimit = component.HeatLimit;
            }

            if (component.RecoilModifier < 1.0f && component.RecoilModifier > recoilModifierBonus)
            {
                recoilModifierBonus = component.RecoilModifier;
            }

            if (componentData.EquipmentController != null)
            {
                EquipmentController equipmentController = componentData.EquipmentController;

                radarDetectionReduction += equipmentController.CurrentMode.RadarSignatureReduction;
                radarDetectionRange += equipmentController.CurrentMode.RadarRangeBonus;
                lockOnBonus += equipmentController.CurrentMode.TargetLockOnBonus;
            }
        }

        heatLimit += heatLimitBonus;

        heatWarning = heatLimit * ResourceManager.Instance.GameConstants.HeatWarningPercent;
        heatShutdown = heatLimit * ResourceManager.Instance.GameConstants.HeatShutDownPercent;
        heatDamageMinor = heatLimit * ResourceManager.Instance.GameConstants.HeatDamageMinorPercent;
        heatDamageMajor = heatLimit * ResourceManager.Instance.GameConstants.HeatDamageMajorPercent;

        if (recoilModifierBonus != 0.0f)
        {
            recoilModifier *= (1.0f - recoilModifierBonus);
        }

        MechChassis.SetPowerSettings(reactorPower, ref movementSpeedForward, ref movementSpeedReverse, ref accelerationForward, ref accelerationReverse, ref deacceleration, ref movementSpeedTurn, ref torsoTwistSpeed, ref torsoPitchSpeed);

        if (jumpTrust > 0.0f)
        {
            jumpHeat = jumpTrust;
            jumpTrust = jumpTrust / ((MechChassis.Tonnage + 100) * 0.035f) + 5.0f;
        }

        // Sqr radar

        internalMaxHead = MechChassis.HeadInternal;
        internalMaxTorsoCenter = MechChassis.TorsoCenterInternal;
        internalMaxTorsoLeft = MechChassis.TorsoLeftInternal;
        internalMaxTorsoRight = MechChassis.TorsoRightInternal;
        internalMaxArmLeft = MechChassis.ArmLeftInternal;
        internalMaxArmRight = MechChassis.ArmRightInternal;
        internalMaxLegLeft = MechChassis.LegLeftInternal;
        internalMaxLegRight = MechChassis.LegRightInternal;

        foreach (ComponentData componentData in componentsHead)
        {
            internalMaxHead += componentData.ComponentDefinition.InternalBonus;
        }

        foreach (ComponentData componentData in componentsTorsoCenter)
        {
            internalMaxTorsoCenter += componentData.ComponentDefinition.InternalBonus;
        }

        foreach (ComponentData componentData in componentsTorsoLeft)
        {
            internalMaxTorsoLeft += componentData.ComponentDefinition.InternalBonus;
        }

        foreach (ComponentData componentData in componentsTorsoRight)
        {
            internalMaxTorsoRight += componentData.ComponentDefinition.InternalBonus;
        }

        foreach (ComponentData componentData in componentsArmLeft)
        {
            internalMaxArmLeft += componentData.ComponentDefinition.InternalBonus;
        }

        foreach (ComponentData componentData in componentsArmRight)
        {
            internalMaxArmRight += componentData.ComponentDefinition.InternalBonus;
        }

        foreach (ComponentData componentData in componentsLegLeft)
        {
            internalMaxLegLeft += componentData.ComponentDefinition.InternalBonus;
        }

        foreach (ComponentData componentData in componentsLegRight)
        {
            internalMaxLegRight += componentData.ComponentDefinition.InternalBonus;
        }

        if (internalHead > internalMaxHead)
            internalHead = internalMaxHead;

        if (internalTorsoCenter > internalMaxTorsoCenter)
            internalTorsoCenter = internalMaxTorsoCenter;

        if (internalTorsoLeft > internalMaxTorsoLeft)
            internalTorsoLeft = internalMaxTorsoLeft;

        if (internalTorsoRight > internalMaxTorsoRight)
            internalTorsoRight = internalMaxTorsoRight;

        if (internalArmLeft > internalMaxArmLeft)
            internalArmLeft = internalMaxArmLeft;

        if (internalArmRight > internalMaxArmRight)
            internalArmRight = internalMaxArmRight;

        if (internalLegLeft > internalMaxLegLeft)
            internalLegLeft = internalMaxLegLeft;

        if (internalLegRight > internalMaxLegRight)
            internalLegRight = internalMaxLegRight;
    }

    public void ReloadAllAmmo()
    {
        foreach (ComponentData componentData in componentsHead)
        {
            componentData.ReloadAmmo();
        }

        foreach (ComponentData componentData in componentsTorsoCenter)
        {
            componentData.ReloadAmmo();
        }

        foreach (ComponentData componentData in componentsTorsoLeft)
        {
            componentData.ReloadAmmo();
        }

        foreach (ComponentData componentData in componentsTorsoRight)
        {
            componentData.ReloadAmmo();
        }

        foreach (ComponentData componentData in componentsArmLeft)
        {
            componentData.ReloadAmmo();
        }

        foreach (ComponentData componentData in componentsArmRight)
        {
            componentData.ReloadAmmo();
        }
    }

    public void TakeDamageHead(float damage, WeaponController weaponController)
    {
        float armorPiercingDamage = 0.0f;

        if (armorHead > 0)
        {
            float armorDamageReduction;

            if (weaponController is ProjectileWeaponController && (weaponController as ProjectileWeaponController).ProjectileDefinition.ArmorPiercing != 0.0f)
            {
                armorPiercingDamage = damage * (weaponController as ProjectileWeaponController).ProjectileDefinition.ArmorPiercing;
                damage -= armorPiercingDamage;
            }

            if (armorTypeHead == ArmorType.standard || armorTypeHead == ArmorType.stealth || (armorDamageReduction = ResourceManager.Instance.GameConstants.GetArmorDamageReduction(armorTypeHead, weaponController.GetDamageType())) == 0)
            {
                if (armorHead > damage)
                {
                    armorHead -= damage;
                    damage = 0.0f;
                }
                else
                {
                    damage -= armorHead;
                    armorHead = 0;
                }
            }
            else
            {
                float damageReduced = damage * (1.0f - armorDamageReduction);

                if (armorHead > damageReduced)
                {
                    armorHead -= damageReduced;
                    damage = 0.0f;
                }
                else
                {
                    float damageUsedPercentage = armorHead / damageReduced;
                    armorHead = 0f;
                    damage -= damage * damageUsedPercentage;
                }
            }
        }

        damage += armorPiercingDamage;

        if (damage > 0)
        {
            TakeDamageInternalHead(damage, weaponController);
        }
    }

    public void TakeDamageTorsoCenterFront(float damage, WeaponController weaponController)
    {
        float armorPiercingDamage = 0.0f;

        if (armorTorsoCenter > 0)
        {
            float armorDamageReduction;

            if (weaponController is ProjectileWeaponController && (weaponController as ProjectileWeaponController).ProjectileDefinition.ArmorPiercing != 0.0f)
            {
                armorPiercingDamage = damage * (weaponController as ProjectileWeaponController).ProjectileDefinition.ArmorPiercing;
                damage -= armorPiercingDamage;
            }

            if (armorTypeTorsoCenter == ArmorType.standard || armorTypeTorsoCenter == ArmorType.stealth || (armorDamageReduction = ResourceManager.Instance.GameConstants.GetArmorDamageReduction(armorTypeTorsoCenter, weaponController.GetDamageType())) == 0)
            {
                if (armorTorsoCenter > damage)
                {
                    armorTorsoCenter -= damage;
                    damage = 0.0f;
                }
                else
                {
                    damage -= armorTorsoCenter;
                    armorTorsoCenter = 0;
                }
            }
            else
            {
                float damageReduced = damage * (1.0f - armorDamageReduction);

                if (armorTorsoCenter > damageReduced)
                {
                    armorTorsoCenter -= damageReduced;
                    damage = 0.0f;
                }
                else
                {
                    float damageUsedPercentage = armorTorsoCenter / damageReduced;
                    armorTorsoCenter = 0f;
                    damage -= damage * damageUsedPercentage;
                }
            }
        }

        damage += armorPiercingDamage;

        if (damage > 0)
        {
            TakeDamageInternalTorsoCenter(damage, weaponController);
        }
    }

    public void TakeDamageTorsoCenterRear(float damage, WeaponController weaponController)
    {
        float armorPiercingDamage = 0.0f;

        if (armorTorsoCenterRear > 0)
        {
            float armorDamageReduction;

            if (weaponController is ProjectileWeaponController && (weaponController as ProjectileWeaponController).ProjectileDefinition.ArmorPiercing != 0.0f)
            {
                armorPiercingDamage = damage * (weaponController as ProjectileWeaponController).ProjectileDefinition.ArmorPiercing;
                damage -= armorPiercingDamage;
            }

            if (armorTypeTorsoCenter == ArmorType.standard || armorTypeTorsoCenter == ArmorType.stealth || (armorDamageReduction = ResourceManager.Instance.GameConstants.GetArmorDamageReduction(armorTypeTorsoCenter, weaponController.GetDamageType())) == 0)
            {
                if (armorTorsoCenterRear > damage)
                {
                    armorTorsoCenterRear -= damage;
                    damage = 0.0f;
                }
                else
                {
                    damage -= armorTorsoCenterRear;
                    armorTorsoCenterRear = 0;
                }
            }
            else
            {
                float damageReduced = damage * (1.0f - armorDamageReduction);

                if (armorTorsoCenterRear > damageReduced)
                {
                    armorTorsoCenterRear -= damageReduced;
                    damage = 0.0f;
                }
                else
                {
                    float damageUsedPercentage = armorTorsoCenterRear / damageReduced;
                    armorTorsoCenterRear = 0f;
                    damage -= damage * damageUsedPercentage;
                }
            }
        }

        damage += armorPiercingDamage;

        if (damage > 0)
        {
            TakeDamageInternalTorsoCenter(damage, weaponController);
        }
    }

    public void TakeDamageTorsoLeftFront(float damage, WeaponController weaponController)
    {
        float armorPiercingDamage = 0.0f;

        if (armorTorsoLeft > 0)
        {
            float armorDamageReduction;

            if (weaponController is ProjectileWeaponController && (weaponController as ProjectileWeaponController).ProjectileDefinition.ArmorPiercing != 0.0f)
            {
                armorPiercingDamage = damage * (weaponController as ProjectileWeaponController).ProjectileDefinition.ArmorPiercing;
                damage -= armorPiercingDamage;
            }

            if (armorTypeTorsoLeft == ArmorType.standard || armorTypeTorsoLeft == ArmorType.stealth || (armorDamageReduction = ResourceManager.Instance.GameConstants.GetArmorDamageReduction(armorTypeTorsoLeft, weaponController.GetDamageType())) == 0)
            {
                if (armorTorsoLeft > damage)
                {
                    armorTorsoLeft -= damage;
                    damage = 0.0f;
                }
                else
                {
                    damage -= armorTorsoLeft;
                    armorTorsoLeft = 0;
                }
            }
            else
            {
                float damageReduced = damage * (1.0f - armorDamageReduction);

                if (armorTorsoLeft > damageReduced)
                {
                    armorTorsoLeft -= damageReduced;
                    damage = 0.0f;
                }
                else
                {
                    float damageUsedPercentage = armorTorsoLeft / damageReduced;
                    armorTorsoLeft = 0f;
                    damage -= damage * damageUsedPercentage;
                }
            }
        }

        damage += armorPiercingDamage;

        if (damage > 0)
        {
            TakeDamageInternalTorsoLeft(damage, weaponController);
        }
    }

    public void TakeDamageTorsoLeftRear(float damage, WeaponController weaponController)
    {
        float armorPiercingDamage = 0.0f;

        if (armorTorsoLeftRear > 0)
        {
            float armorDamageReduction;

            if (weaponController is ProjectileWeaponController && (weaponController as ProjectileWeaponController).ProjectileDefinition.ArmorPiercing != 0.0f)
            {
                armorPiercingDamage = damage * (weaponController as ProjectileWeaponController).ProjectileDefinition.ArmorPiercing;
                damage -= armorPiercingDamage;
            }

            if (armorTypeTorsoLeft == ArmorType.standard || armorTypeTorsoLeft == ArmorType.stealth || (armorDamageReduction = ResourceManager.Instance.GameConstants.GetArmorDamageReduction(armorTypeTorsoLeft, weaponController.GetDamageType())) == 0)
            {
                if (armorTorsoLeftRear > damage)
                {
                    armorTorsoLeftRear -= damage;
                    damage = 0.0f;
                }
                else
                {
                    damage -= armorTorsoLeftRear;
                    armorTorsoLeftRear = 0;
                }
            }
            else
            {
                float damageReduced = damage * (1.0f - armorDamageReduction);

                if (armorTorsoLeftRear > damageReduced)
                {
                    armorTorsoLeftRear -= damageReduced;
                    damage = 0.0f;
                }
                else
                {
                    float damageUsedPercentage = armorTorsoLeftRear / damageReduced;
                    armorTorsoLeftRear = 0f;
                    damage -= damage * damageUsedPercentage;
                }
            }
        }

        damage += armorPiercingDamage;

        if (damage > 0)
        {
            TakeDamageInternalTorsoLeft(damage, weaponController);
        }
    }

    public void TakeDamageTorsoRightFront(float damage, WeaponController weaponController)
    {
        float armorPiercingDamage = 0.0f;

        if (armorTorsoRight > 0)
        {
            float armorDamageReduction;

            if (weaponController is ProjectileWeaponController && (weaponController as ProjectileWeaponController).ProjectileDefinition.ArmorPiercing != 0.0f)
            {
                armorPiercingDamage = damage * (weaponController as ProjectileWeaponController).ProjectileDefinition.ArmorPiercing;
                damage -= armorPiercingDamage;
            }

            if (armorTypeTorsoRight == ArmorType.standard || armorTypeTorsoRight == ArmorType.stealth || (armorDamageReduction = ResourceManager.Instance.GameConstants.GetArmorDamageReduction(armorTypeTorsoRight, weaponController.GetDamageType())) == 0)
            {
                if (armorTorsoRight > damage)
                {
                    armorTorsoRight -= damage;
                    damage = 0.0f;
                }
                else
                {
                    damage -= armorTorsoRight;
                    armorTorsoRight = 0;
                }
            }
            else
            {
                float damageReduced = damage * (1.0f - armorDamageReduction);

                if (armorTorsoRight > damageReduced)
                {
                    armorTorsoRight -= damageReduced;
                    damage = 0.0f;
                }
                else
                {
                    float damageUsedPercentage = armorTorsoRight / damageReduced;
                    armorTorsoRight = 0f;
                    damage -= damage * damageUsedPercentage;
                }
            }
        }

        damage += armorPiercingDamage;

        if (damage > 0)
        {
            TakeDamageInternalTorsoRight(damage, weaponController);
        }
    }

    public void TakeDamageTorsoRightRear(float damage, WeaponController weaponController)
    {
        float armorPiercingDamage = 0.0f;

        if (armorTorsoRightRear > 0)
        {
            float armorDamageReduction;

            if (weaponController is ProjectileWeaponController && (weaponController as ProjectileWeaponController).ProjectileDefinition.ArmorPiercing != 0.0f)
            {
                armorPiercingDamage = damage * (weaponController as ProjectileWeaponController).ProjectileDefinition.ArmorPiercing;
                damage -= armorPiercingDamage;
            }

            if (armorTypeTorsoRight == ArmorType.standard || armorTypeTorsoRight == ArmorType.stealth || (armorDamageReduction = ResourceManager.Instance.GameConstants.GetArmorDamageReduction(armorTypeTorsoRight, weaponController.GetDamageType())) == 0)
            {
                if (armorTorsoRightRear > damage)
                {
                    armorTorsoRightRear -= damage;
                    damage = 0.0f;
                }
                else
                {
                    damage -= armorTorsoRightRear;
                    armorTorsoRightRear = 0;
                }
            }
            else
            {
                float damageReduced = damage * (1.0f - armorDamageReduction);

                if (armorTorsoRightRear > damageReduced)
                {
                    armorTorsoRightRear -= damageReduced;
                    damage = 0.0f;
                }
                else
                {
                    float damageUsedPercentage = armorTorsoRightRear / damageReduced;
                    armorTorsoRightRear = 0f;
                    damage -= damage * damageUsedPercentage;
                }
            }
        }

        damage += armorPiercingDamage;

        if (damage > 0)
        {
            TakeDamageInternalTorsoRight(damage, weaponController);
        }
    }

    public void TakeDamageArmLeft(float damage, WeaponController weaponController)
    {
        float armorPiercingDamage = 0.0f;

        if (armorArmLeft > 0)
        {
            float armorDamageReduction;

            if (weaponController is ProjectileWeaponController && (weaponController as ProjectileWeaponController).ProjectileDefinition.ArmorPiercing != 0.0f)
            {
                armorPiercingDamage = damage * (weaponController as ProjectileWeaponController).ProjectileDefinition.ArmorPiercing;
                damage -= armorPiercingDamage;
            }

            if (armorTypeArmLeft == ArmorType.standard || armorTypeArmLeft == ArmorType.stealth || (armorDamageReduction = ResourceManager.Instance.GameConstants.GetArmorDamageReduction(armorTypeArmLeft, weaponController.GetDamageType())) == 0)
            {
                if (armorArmLeft > damage)
                {
                    armorArmLeft -= damage;
                    damage = 0.0f;
                }
                else
                {
                    damage -= armorArmLeft;
                    armorArmLeft = 0;
                }
            }
            else
            {
                float damageReduced = damage * (1.0f - armorDamageReduction);

                if (armorArmLeft > damageReduced)
                {
                    armorArmLeft -= damageReduced;
                    damage = 0.0f;
                }
                else
                {
                    float damageUsedPercentage = armorArmLeft / damageReduced;
                    armorArmLeft = 0f;
                    damage -= damage * damageUsedPercentage;
                }
            }
        }

        damage += armorPiercingDamage;

        if (damage > 0)
        {
            TakeDamageInternalArmLeft(damage, weaponController);
        }
    }

    public void TakeDamageArmRight(float damage, WeaponController weaponController)
    {
        float armorPiercingDamage = 0.0f;

        if (armorArmRight > 0)
        {
            float armorDamageReduction;

            if (weaponController is ProjectileWeaponController && (weaponController as ProjectileWeaponController).ProjectileDefinition.ArmorPiercing != 0.0f)
            {
                armorPiercingDamage = damage * (weaponController as ProjectileWeaponController).ProjectileDefinition.ArmorPiercing;
                damage -= armorPiercingDamage;
            }

            if (armorTypeArmRight == ArmorType.standard || armorTypeArmRight == ArmorType.stealth || (armorDamageReduction = ResourceManager.Instance.GameConstants.GetArmorDamageReduction(armorTypeArmRight, weaponController.GetDamageType())) == 0)
            {
                if (armorArmRight > damage)
                {
                    armorArmRight -= damage;
                    damage = 0.0f;
                }
                else
                {
                    damage -= armorArmRight;
                    armorArmRight = 0;
                }
            }
            else
            {
                float damageReduced = damage * (1.0f - armorDamageReduction);

                if (armorArmRight > damageReduced)
                {
                    armorArmRight -= damageReduced;
                    damage = 0.0f;
                }
                else
                {
                    float damageUsedPercentage = armorArmRight / damageReduced;
                    armorArmRight = 0f;
                    damage -= damage * damageUsedPercentage;
                }
            }
        }

        damage += armorPiercingDamage;

        if (damage > 0)
        {
            TakeDamageInternalArmRight(damage, weaponController);
        }
    }

    public void TakeDamageLegLeft(float damage, WeaponController weaponController)
    {
        float armorPiercingDamage = 0.0f;

        if (armorLegLeft > 0)
        {
            float armorDamageReduction;

            if (weaponController is ProjectileWeaponController && (weaponController as ProjectileWeaponController).ProjectileDefinition.ArmorPiercing != 0.0f)
            {
                armorPiercingDamage = damage * (weaponController as ProjectileWeaponController).ProjectileDefinition.ArmorPiercing;
                damage -= armorPiercingDamage;
            }

            if (armorTypeLegLeft == ArmorType.standard || armorTypeLegLeft == ArmorType.stealth || (armorDamageReduction = ResourceManager.Instance.GameConstants.GetArmorDamageReduction(armorTypeLegLeft, weaponController.GetDamageType())) == 0)
            {
                if (armorLegLeft > damage)
                {
                    armorLegLeft -= damage;
                    damage = 0.0f;
                }
                else
                {
                    damage -= armorLegLeft;
                    armorLegLeft = 0;
                }
            }
            else
            {
                float damageReduced = damage * (1.0f - armorDamageReduction);

                if (armorLegLeft > damageReduced)
                {
                    armorLegLeft -= damageReduced;
                    damage = 0.0f;
                }
                else
                {
                    float damageUsedPercentage = armorLegLeft / damageReduced;
                    armorLegLeft = 0f;
                    damage -= damage * damageUsedPercentage;
                }
            }
        }

        damage += armorPiercingDamage;

        if (damage > 0)
        {
            TakeDamageInternalLegLeft(damage, weaponController);
        }
    }

    public void TakeDamageLegLeft(float damage)
    {
        if (armorLegLeft > 0)
        {
            if (armorLegLeft > damage)
            {
                armorLegLeft -= damage;
                damage = 0.0f;
            }
            else
            {
                damage -= armorLegLeft;
                armorLegLeft = 0;
            }
        }

        if (damage > 0)
        {
            TakeDamageInternalLegLeft(damage);
        }
    }

    public void TakeDamageLegRight(float damage, WeaponController weaponController)
    {
        float armorPiercingDamage = 0.0f;

        if (armorLegRight > 0)
        {
            float armorDamageReduction;

            if (weaponController is ProjectileWeaponController && (weaponController as ProjectileWeaponController).ProjectileDefinition.ArmorPiercing != 0.0f)
            {
                armorPiercingDamage = damage * (weaponController as ProjectileWeaponController).ProjectileDefinition.ArmorPiercing;
                damage -= armorPiercingDamage;
            }

            if (armorTypeLegRight == ArmorType.standard || armorTypeLegRight == ArmorType.stealth || (armorDamageReduction = ResourceManager.Instance.GameConstants.GetArmorDamageReduction(armorTypeLegRight, weaponController.GetDamageType())) == 0)
            {
                if (armorLegRight > damage)
                {
                    armorLegRight -= damage;
                    damage = 0.0f;
                }
                else
                {
                    damage -= armorLegRight;
                    armorLegRight = 0;
                }
            }
            else
            {
                float damageReduced = damage * (1.0f - armorDamageReduction);

                if (armorLegRight > damageReduced)
                {
                    armorLegRight -= damageReduced;
                    damage = 0.0f;
                }
                else
                {
                    float damageUsedPercentage = armorLegRight / damageReduced;
                    armorLegRight = 0f;
                    damage -= damage * damageUsedPercentage;
                }
            }
        }

        damage += armorPiercingDamage;

        if (damage > 0)
        {
            TakeDamageInternalLegRight(damage, weaponController);
        }
    }

    public void TakeDamageLegRight(float damage)
    {
        if (armorLegRight > 0)
        {
            if (armorLegRight > damage)
            {
                armorLegRight -= damage;
                damage = 0.0f;
            }
            else
            {
                damage -= armorLegRight;
                armorLegRight = 0;
            }
        }

        if (damage > 0)
        {
            TakeDamageInternalLegRight(damage);
        }
    }

    void TakeDamageInternalHead(float damage, WeaponController weaponController)
    {
        if (damage >= internalHead)
        {
            internalHead = 0;

            bool ammoExplosion = false;

            DestroyComponents(componentsHead, ref ammoExplosion);

            RecalculateStats();

            if (ammoExplosion)
            {
                mechController.CreateAmmoExplosionHead();
            }
            else
            {
                mechController.CreateExplosionHead();
            }

            if (!isDestroyed)
            {
                causeOfDestruction = MechCauseOfDestructionType.HeadDestroyed;
                pilotKilled = true;

                mechController.Die();
            }
        }
        else
        {
            internalHead -= damage;

            if (weaponController.GetCriticalRole())
            {
                CritHead(damage, weaponController);
            }
        }
    }

    public void TakeDamageInternalTorsoCenter(float damage, WeaponController weaponController)
    {
        if (damage >= internalTorsoCenter)
        {
            bool ammoExplosion = false;

            damage += DestroyComponents(componentsTorsoCenter, ref ammoExplosion);

            RecalculateStats();

            if (ammoExplosion)
            {
                mechController.CreateAmmoExplosionTorsoCenter();
            }

            internalTorsoCenter -= damage;

            if (!isDestroyed)
            {
                causeOfDestruction = MechCauseOfDestructionType.CenterTorsoDestroyed;

                mechController.CreateDeathExplosion();
                mechController.Die();
            }
        }
        else
        {
            internalTorsoCenter -= damage;

            if (weaponController.GetCriticalRole())
            {
                CritTorsoCenter(damage, weaponController);
            }
        }
    }

    void TakeDamageInternalTorsoLeft(float damage, WeaponController weaponController)
    {
        if (internalTorsoLeft > 0)
        {
            if (damage >= internalTorsoLeft)
            {
                damage -= internalTorsoLeft;
                internalTorsoLeft = 0;

                bool ammoExplosion = false;

                float explosionDamage = DestroyComponents(componentsTorsoLeft, ref ammoExplosion);

                RecalculateStats();

                if (ammoExplosion)
                {
                    mechController.CreateAmmoExplosionTorsoLeft();
                }
                else
                {
                    mechController.CreateExplosionTorsoLeft();
                }

                DestroyArmLeft();

                if (damage > 0)
                {
                    TakeDamageTorsoCenterFront(damage * 0.75f, weaponController);
                }

                if (explosionDamage > 0)
                {
                    TakeComponentExplosionDamageTorsoCenter(explosionDamage * 0.4f, weaponController);
                }
            }
            else
            {
                internalTorsoLeft -= damage;

                if (weaponController.GetCriticalRole())
                {
                    CritTorsoLeft(damage, weaponController);
                }
            }
        }
        else
        {
            TakeDamageTorsoCenterFront(damage * 0.75f, weaponController);
        }
    }

    void TakeDamageInternalTorsoRight(float damage, WeaponController weaponController)
    {
        if (internalTorsoRight > 0)
        {
            if (damage >= internalTorsoRight)
            {
                damage -= internalTorsoRight;
                internalTorsoRight = 0;

                bool ammoExplosion = false;

                float explosionDamage = DestroyComponents(componentsTorsoRight, ref ammoExplosion);

                RecalculateStats();

                if (ammoExplosion)
                {
                    mechController.CreateAmmoExplosionTorsoRight();
                }
                else
                {
                    mechController.CreateExplosionTorsoRight();
                }

                DestroyArmRight();

                if (damage > 0)
                {
                    TakeDamageTorsoCenterFront(damage * 0.75f, weaponController);
                }

                if (explosionDamage > 0)
                {
                    TakeComponentExplosionDamageTorsoCenter(explosionDamage * 0.4f, weaponController);
                }
            }
            else
            {
                internalTorsoRight -= damage;

                if (weaponController.GetCriticalRole())
                {
                    CritTorsoRight(damage, weaponController);
                }
            }
        }
        else
        {
            TakeDamageTorsoCenterFront(damage * 0.75f, weaponController);
        }
    }

    void TakeDamageInternalArmLeft(float damage, WeaponController weaponController)
    {
        if (internalArmLeft > 0)
        {
            if (damage >= internalArmLeft)
            {
                damage -= internalArmLeft;
                internalArmLeft = 0;
                mechController.DestroyArmLeft();

                bool ammoExplosion = false;

                float explosionDamage = DestroyComponents(componentsArmLeft, ref ammoExplosion);

                RecalculateStats();

                if (ammoExplosion)
                {
                    mechController.CreateAmmoExplosionArmLeft();
                }
                else
                {
                    mechController.CreateExplosionArmLeft();
                }

                mechController.CalculateMaxLockOnRange();

                if (damage > 0)
                {
                    TakeDamageTorsoLeftFront(damage * 0.75f, weaponController);
                }

                if (explosionDamage > 0)
                {
                    TakeComponentExplosionDamageTorsoLeft(explosionDamage * 0.4f, weaponController);
                }
            }
            else
            {
                internalArmLeft -= damage;

                if (weaponController.GetCriticalRole())
                {
                    CritArmLeft(damage, weaponController);
                }
            }
        }
        else
        {
            TakeDamageTorsoLeftFront(damage * 0.75f, weaponController);
        }
    }

    void TakeDamageInternalArmRight(float damage, WeaponController weaponController)
    {
        if (internalArmRight > 0)
        {
            if (damage >= internalArmRight)
            {
                damage -= internalArmRight;
                internalArmRight = 0;
                mechController.DestroyArmRight();

                bool ammoExplosion = false;

                float explosionDamage = DestroyComponents(componentsArmRight, ref ammoExplosion);

                RecalculateStats();

                if (ammoExplosion)
                {
                    mechController.CreateAmmoExplosionArmRight();
                }
                else
                {
                    mechController.CreateExplosionArmRight();
                }

                mechController.CalculateMaxLockOnRange();

                if (damage > 0)
                {
                    TakeDamageTorsoRightFront(damage * 0.75f, weaponController);
                }

                if (explosionDamage > 0)
                {
                    TakeComponentExplosionDamageTorsoRight(explosionDamage * 0.4f, weaponController);
                }
            }
            else
            {
                internalArmRight -= damage;

                if (weaponController.GetCriticalRole())
                {
                    CritArmRight(damage, weaponController);
                }
            }
        }
        else
        {
            TakeDamageTorsoRightFront(damage * 0.75f, weaponController);
        }
    }

    void TakeDamageInternalLegLeft(float damage, WeaponController weaponController)
    {
        if (internalLegLeft > 0)
        {
            if (damage >= internalLegLeft)
            {
                damage -= internalLegLeft;
                internalLegLeft = 0;

                float explosionDamage = DestroyComponents(componentsLegLeft);

                RecalculateStats();

                mechController.CreateExplosionLegLeft();

                if (internalLegRight > 0)
                {
                    if (damage > 0)
                    {
                        TakeDamageTorsoCenterFront(damage * 0.75f, weaponController);
                    }

                    if (explosionDamage > 0)
                    {
                        TakeComponentExplosionDamageTorsoCenter(explosionDamage * 0.4f, weaponController);
                    }
                }
                else if (!isDestroyed)
                {
                    causeOfDestruction = MechCauseOfDestructionType.LegsDestroyed;
                    mechController.Die();
                }
            }
            else
            {
                internalLegLeft -= damage;

                if (weaponController.GetCriticalRole())
                {
                    CritLegLeft(damage, weaponController);
                }
            }
        }
        else
        {
            TakeDamageTorsoCenterFront(damage * 0.75f, weaponController);
        }
    }

    void TakeDamageInternalLegRight(float damage, WeaponController weaponController)
    {
        if (internalLegRight > 0)
        {
            if (damage >= internalLegRight)
            {
                damage -= internalLegRight;
                internalLegRight = 0;

                float explosionDamage = DestroyComponents(componentsLegRight);

                RecalculateStats();

                mechController.CreateExplosionLegRight();

                if (internalLegLeft > 0)
                {
                    if (damage > 0)
                    {
                        TakeDamageTorsoCenterFront(damage * 0.75f, weaponController);
                    }

                    if (explosionDamage > 0)
                    {
                        TakeComponentExplosionDamageTorsoCenter(explosionDamage * 0.4f, weaponController);
                    }
                }
                else
                {
                    causeOfDestruction = MechCauseOfDestructionType.LegsDestroyed;
                    mechController.Die();
                }
            }
            else
            {
                internalLegRight -= damage;

                if (weaponController.GetCriticalRole())
                {
                    CritLegRight(damage, weaponController);
                }
            }
        }
        else
        {
            TakeDamageTorsoCenterFront(damage * 0.75f, weaponController);
        }
    }

    public void TakeDamageInternalTorsoCenter(float damage)
    {
        if (damage >= internalTorsoCenter)
        {
            bool ammoExplosion = false;

            damage += DestroyComponents(componentsTorsoCenter, ref ammoExplosion);

            RecalculateStats();

            if (ammoExplosion)
            {
                mechController.CreateAmmoExplosionTorsoCenter();
            }

            internalTorsoCenter -= damage;

            if (!isDestroyed)
            {
                causeOfDestruction = MechCauseOfDestructionType.OverHeating;

                mechController.CreateDeathExplosion();
                mechController.Die();
            }
        }
        else
        {
            internalTorsoCenter -= damage;
        }
    }

    public void TakeDamageInternalLegLeft(float damage)
    {
        if (internalLegLeft > 0)
        {
            damage += DestroyComponents(componentsLegLeft);

            RecalculateStats();

            if (damage >= internalLegLeft)
            {
                internalLegLeft = 0;

                if (internalLegRight == 0 && !isDestroyed)
                {
                    causeOfDestruction = MechCauseOfDestructionType.LegsDestroyed;
                    mechController.Die();
                }
            }
            else
            {
                internalLegLeft -= damage;
            }
        }
    }

    public void TakeDamageInternalLegRight(float damage)
    {
        if (internalLegRight > 0)
        {
            damage += DestroyComponents(componentsLegRight);

            RecalculateStats();

            if (damage >= internalLegRight)
            {
                internalLegRight = 0;             

                if (internalLegLeft == 0 && !isDestroyed)
                {
                    causeOfDestruction = MechCauseOfDestructionType.LegsDestroyed;
                    mechController.Die();
                }
            }
            else
            {
                internalLegRight -= damage;
            }
        }
    }

    void CritHead(float damage, WeaponController weaponController)
    {
        ComponentData componentData = GetCriticalTarget(componentsHead);

        if (componentData != null)
        {
            float critDamage = damage;

            if (weaponController.GetCriticalDamageMulti() != 1.0f)
            {
                critDamage *= weaponController.GetCriticalDamageMulti();
            }

            if (componentData.TakeDamage(critDamage))
            {                
                componentData.SetDestroyed();
                RecalculateStats();

                if (componentData.ComponentDefinition.GetExplosionRole())
                {
                    bool ammoExplosion = componentData.AmmoCanExplode;
                    componentData.SetAmmo(0);

                    if (componentData.ComponentDefinition.ExplosionDamage > internalHead)
                    {
                        internalHead = 0;
                        DestroyComponents(componentsHead, ref ammoExplosion);

                        RecalculateStats();

                        if (!ammoExplosion)
                        {
                            mechController.CreateExplosionHead();
                        }
                        else
                        {
                            mechController.CreateAmmoExplosionHead();
                        }

                        if (!isDestroyed)
                        {
                            causeOfDestruction = MechCauseOfDestructionType.InternalExplosion;
                            mechController.Die();
                        }
                    }
                    else
                    {
                        internalHead -= componentData.ComponentDefinition.ExplosionDamage;

                        mechController.CreateExplosionHead();
                    }

                    if (ammoExplosion)
                    {
                        mechController.CreateAmmoExplosionHead();
                    }
                }
            }
        }
    }

    void CritTorsoCenter(float damage, WeaponController weaponController)
    {
        ComponentData componentData = GetCriticalTarget(componentsTorsoCenter);

        if (componentData != null)
        {
            float critDamage = damage;

            if (weaponController.GetCriticalDamageMulti() != 1.0f)
            {
                critDamage *= weaponController.GetCriticalDamageMulti();
            }

            if (componentData.TakeDamage(critDamage))
            {
                componentData.SetDestroyed();
                RecalculateStats();
                if (componentData.ComponentDefinition.GetExplosionRole())
                {
                    bool ammoExplosion = componentData.AmmoCanExplode;
                    float explosionDamage = componentData.ExplosionDamage;
                    componentData.SetAmmo(0);

                    if (explosionDamage > internalTorsoCenter)
                    {
                        internalTorsoCenter = 0;
                        DestroyComponents(componentsTorsoCenter, ref ammoExplosion);

                        RecalculateStats();

                        if (ammoExplosion)
                        {
                            mechController.CreateAmmoExplosionTorsoCenter();
                        }

                        if (!isDestroyed)
                        {
                            mechController.CreateDeathExplosion();
                            causeOfDestruction = MechCauseOfDestructionType.InternalExplosion;
                            mechController.Die();
                        }
                    }
                    else
                    {
                        internalTorsoCenter -= componentData.ComponentDefinition.ExplosionDamage;

                        mechController.CreateExplosionTorsoCenter();
                    }

                    if (ammoExplosion)
                    {
                        mechController.CreateAmmoExplosionTorsoCenter();
                    }
                }
            }
        }
    }

    void CritTorsoLeft(float damage, WeaponController weaponController)
    {
        ComponentData componentData = GetCriticalTarget(componentsTorsoLeft);

        if (componentData != null)
        {
            float critDamage = damage;

            if (weaponController.GetCriticalDamageMulti() != 1.0f)
            {
                critDamage *= weaponController.GetCriticalDamageMulti();
            }

            if (componentData.TakeDamage(critDamage))
            {
                componentData.SetDestroyed();
                RecalculateStats();
                if (componentData.ComponentDefinition.GetExplosionRole())
                {
                    float explosionDamage = componentData.ExplosionDamage;

                    bool ammoExplosion = componentData.AmmoCanExplode;
                    componentData.SetAmmo(0);

                    if (explosionDamage > internalTorsoLeft)
                    {
                        explosionDamage -= internalTorsoLeft;
                        internalTorsoLeft = 0;

                        explosionDamage += DestroyComponents(componentsTorsoLeft, ref ammoExplosion);

                        RecalculateStats();

                        if (!ammoExplosion)
                        {
                            mechController.CreateExplosionTorsoLeft();
                        }

                        DestroyArmLeft();

                        if (explosionDamage > 0)
                        {
                            TakeComponentExplosionDamageTorsoCenter(explosionDamage * 0.4f, weaponController);
                        }
                    }
                    else
                    {
                        internalTorsoLeft -= componentData.ComponentDefinition.ExplosionDamage;
                    }

                    if (ammoExplosion)
                    {
                        mechController.CreateAmmoExplosionTorsoLeft();
                    }
                    else
                    {
                        mechController.CreateExplosionTorsoLeft();
                    }
                }
                else
                {
                    componentData.SetAmmo(0);
                }
            }
        }
    }

    void CritTorsoRight(float damage, WeaponController weaponController)
    {
        ComponentData componentData = GetCriticalTarget(componentsTorsoRight);

        if (componentData != null)
        {
            float critDamage = damage;

            if (weaponController.GetCriticalDamageMulti() != 1.0f)
            {
                critDamage *= weaponController.GetCriticalDamageMulti();
            }

            if (componentData.TakeDamage(critDamage))
            {
                componentData.SetDestroyed();
                RecalculateStats();

                if (componentData.ComponentDefinition.GetExplosionRole())
                {
                    float explosionDamage = componentData.ExplosionDamage;

                    bool ammoExplosion = componentData.AmmoCanExplode;
                    componentData.SetAmmo(0);

                    if (explosionDamage > internalTorsoRight)
                    {
                        explosionDamage -= internalTorsoRight;
                        internalTorsoRight = 0;

                        explosionDamage += DestroyComponents(componentsTorsoRight, ref ammoExplosion);

                        RecalculateStats();

                        if (!ammoExplosion)
                        {
                            mechController.CreateExplosionTorsoRight();
                        }

                        DestroyArmRight();

                        if (explosionDamage > 0)
                        {
                            TakeComponentExplosionDamageTorsoCenter(explosionDamage * 0.4f, weaponController);
                        }
                    }
                    else
                    {
                        internalTorsoRight -= componentData.ComponentDefinition.ExplosionDamage;
                    }

                    if (ammoExplosion)
                    {
                        mechController.CreateAmmoExplosionTorsoRight();
                    }
                    else
                    {
                        mechController.CreateExplosionTorsoRight();
                    }
                }
                else
                {
                    componentData.SetAmmo(0);
                }
            }
        }
    }

    void CritArmLeft(float damage, WeaponController weaponController)
    {
        ComponentData componentData = GetCriticalTarget(componentsArmLeft);

        if (componentData != null)
        {
            float critDamage = damage;

            if (weaponController.GetCriticalDamageMulti() != 1.0f)
            {
                critDamage *= weaponController.GetCriticalDamageMulti();
            }

            if (componentData.TakeDamage(critDamage))
            {
                componentData.SetDestroyed();
                RecalculateStats();
                if (componentData.ComponentDefinition.GetExplosionRole())
                {
                    float explosionDamage = componentData.ExplosionDamage;

                    bool ammoExplosion = componentData.AmmoCanExplode;
                    componentData.SetAmmo(0);

                    if (explosionDamage > internalArmLeft)
                    {
                        explosionDamage -= internalArmLeft;
                        internalArmLeft = 0;

                        mechController.DestroyArmLeft();

                        explosionDamage += DestroyComponents(componentsArmLeft, ref ammoExplosion);

                        RecalculateStats();

                        if (explosionDamage > 0)
                        {
                            TakeComponentExplosionDamageTorsoLeft(explosionDamage * 0.4f, weaponController);
                        }
                    }
                    else
                    {
                        internalArmLeft -= componentData.ComponentDefinition.ExplosionDamage;
                    }

                    if (ammoExplosion)
                    {
                        mechController.CreateAmmoExplosionArmLeft();
                    }
                    else
                    {
                        mechController.CreateExplosionArmLeft();
                    }
                }
                else
                {
                    componentData.SetAmmo(0);
                }
            }
        }
    }

    void CritArmRight(float damage, WeaponController weaponController)
    {
        ComponentData componentData = GetCriticalTarget(componentsArmRight);

        if (componentData != null)
        {
            float critDamage = damage;

            if (weaponController.GetCriticalDamageMulti() != 1.0f)
            {
                critDamage *= weaponController.GetCriticalDamageMulti();
            }

            if (componentData.TakeDamage(critDamage))
            {
                componentData.SetDestroyed();
                RecalculateStats();
                if (componentData.ComponentDefinition.GetExplosionRole())
                {
                    float explosionDamage = componentData.ExplosionDamage;

                    bool ammoExplosion = componentData.AmmoCanExplode;
                    componentData.SetAmmo(0);

                    if (explosionDamage > internalArmRight)
                    {
                        explosionDamage -= internalArmRight;
                        internalArmRight = 0;

                        mechController.DestroyArmRight();

                        explosionDamage += DestroyComponents(componentsArmRight, ref ammoExplosion);

                        RecalculateStats();

                        if (explosionDamage > 0)
                        {
                            TakeComponentExplosionDamageTorsoRight(explosionDamage * 0.4f, weaponController);
                        }
                    }
                    else
                    {
                        internalArmRight -= componentData.ComponentDefinition.ExplosionDamage;
                    }

                    if (ammoExplosion)
                    {
                        mechController.CreateAmmoExplosionArmRight();
                    }
                    else
                    {
                        mechController.CreateExplosionArmRight();
                    }
                }
                else
                {
                    componentData.SetAmmo(0);
                }
            }
        }
    }

    void CritLegLeft(float damage, WeaponController weaponController)
    {
        ComponentData componentData = GetCriticalTarget(componentsLegLeft);

        if (componentData != null)
        {
            float critDamage = damage;

            if (weaponController.GetCriticalDamageMulti() != 1.0f)
            {
                critDamage *= weaponController.GetCriticalDamageMulti();
            }

            if (componentData.TakeDamage(critDamage))
            {
                componentData.SetDestroyed();
                RecalculateStats();
                if (componentData.ComponentDefinition.GetExplosionRole())
                {
                    float explosionDamage = componentData.ExplosionDamage;

                    if (explosionDamage > internalLegLeft)
                    {
                        explosionDamage -= internalLegLeft;
                        internalLegLeft = 0;

                        explosionDamage += DestroyComponents(componentsLegLeft);

                        RecalculateStats();

                        if (internalLegRight > 0 && explosionDamage > 0)
                        {
                            TakeComponentExplosionDamageTorsoCenter(explosionDamage * 0.4f, weaponController);
                        }
                        else
                        {
                            causeOfDestruction = MechCauseOfDestructionType.InternalExplosion;
                            mechController.Die();
                        }
                    }
                    else
                    {
                        internalLegLeft -= componentData.ComponentDefinition.ExplosionDamage;
                    }
                }
            }
        }
    }

    void CritLegRight(float damage, WeaponController weaponController)
    {
        ComponentData componentData = GetCriticalTarget(componentsLegRight);

        if (componentData != null)
        {
            float critDamage = damage;

            if (weaponController.GetCriticalDamageMulti() != 1.0f)
            {
                critDamage *= weaponController.GetCriticalDamageMulti();
            }

            if (componentData.TakeDamage(critDamage))
            {
                componentData.SetDestroyed();
                RecalculateStats();
                if (componentData.ComponentDefinition.GetExplosionRole())
                {
                    float explosionDamage = componentData.ExplosionDamage;

                    if (explosionDamage > internalLegRight)
                    {
                        explosionDamage -= internalLegRight;
                        internalLegRight = 0;

                        explosionDamage += DestroyComponents(componentsLegRight);

                        RecalculateStats();

                        if (internalLegLeft > 0 && explosionDamage > 0)
                        {
                            TakeComponentExplosionDamageTorsoCenter(explosionDamage * 0.4f, weaponController);
                        }
                        else
                        {
                            causeOfDestruction = MechCauseOfDestructionType.LegsDestroyed;
                            mechController.Die();
                        }
                    }
                    else
                    {
                        internalLegRight -= componentData.ComponentDefinition.ExplosionDamage;
                    }
                }
            }
        }
    }

    public void DestroyHead()
    {
        armorHead = 0;
        internalHead = 0;

        if (!isDestroyed)
        {
            mechController.CreateExplosionHead();
            causeOfDestruction = MechCauseOfDestructionType.HeadDestroyed;
            pilotKilled = true;
            mechController.Die();
        }
    }

    void TakeComponentExplosionDamageTorsoCenter(float damage, WeaponController weaponController)
    {
        if (damage >= internalTorsoCenter)
        {
            bool ammoExplosion = false;

            damage += DestroyComponents(componentsTorsoCenter, ref ammoExplosion);

            RecalculateStats();

            if (ammoExplosion)
            {
                mechController.CreateAmmoExplosionTorsoCenter();
            }

            internalTorsoCenter = 0;

            if (!isDestroyed)
            {
                mechController.CreateDeathExplosion();
                causeOfDestruction = MechCauseOfDestructionType.InternalExplosion;
                mechController.Die();
            }
        }
        else
        {
            internalTorsoCenter -= damage;
        }
    }

    void TakeComponentExplosionDamageTorsoLeft(float damage, WeaponController weaponController)
    {
        if (damage >= internalTorsoLeft)
        {
            damage -= internalTorsoLeft;
            internalTorsoLeft = 0;

            bool ammoExplosion = false;

            damage += DestroyComponents(componentsTorsoLeft, ref ammoExplosion);

            RecalculateStats();

            if (ammoExplosion)
            {
                mechController.CreateAmmoExplosionTorsoLeft();
            }
            else
            {
                mechController.CreateExplosionTorsoLeft();
            }

            DestroyArmLeft();

            if (damage > 0)
            {
                TakeComponentExplosionDamageTorsoCenter(damage * 0.4f, weaponController);
            }
        }
        else
        {
            internalTorsoLeft -= damage;
        }
    }

    void TakeComponentExplosionDamageTorsoRight(float damage, WeaponController weaponController)
    {
        if (damage >= internalTorsoRight)
        {
            damage -= internalTorsoRight;
            internalTorsoRight = 0;

            bool ammoExplosion = false;

            damage += DestroyComponents(componentsTorsoRight, ref ammoExplosion);

            RecalculateStats();

            if (ammoExplosion)
            {
                mechController.CreateAmmoExplosionTorsoRight();
            }
            else
            {
                mechController.CreateExplosionTorsoRight();
            }

            DestroyArmRight();

            if (damage > 0)
            {
                TakeComponentExplosionDamageTorsoCenter(damage * 0.4f, weaponController);
            }
        }
        else
        {
            internalTorsoRight -= damage;
        }
    }

    void DestroyArmLeft()
    {
        armorArmLeft = 0;

        if (internalArmLeft > 0)
        {
            internalArmLeft = 0;

            bool ammoExplosion = false;

            DestroyComponents(componentsArmLeft, ref ammoExplosion);

            RecalculateStats();

            if (ammoExplosion)
            {
                mechController.CreateAmmoExplosionArmLeft();
            }
            else
            {
                mechController.CreateExplosionArmLeft();
            }

            mechController.DestroyArmLeft();
        }

        mechController.CalculateMaxLockOnRange();
    }

    void DestroyArmRight()
    {
        armorArmRight = 0;

        if (internalArmRight > 0)
        {
            internalArmRight = 0;

            bool ammoExplosion = false;

            DestroyComponents(componentsArmRight,ref ammoExplosion);

            RecalculateStats();

            if (ammoExplosion)
            {
                mechController.CreateAmmoExplosionArmRight();
            }
            else
            {
                mechController.CreateExplosionArmRight();
            }

            mechController.DestroyArmRight();
        }

        mechController.CalculateMaxLockOnRange();
    }

    ComponentData GetCriticalTarget(ComponentData[] componentDatas)
    {
        List<ComponentData> validTargets = new List<ComponentData>();

        foreach (ComponentData componentData in componentDatas)
        {
            if (!componentData.isDestroyed)
            {
                validTargets.Add(componentData);
            }
        }

        if (validTargets.Count > 0)
        {
            if (validTargets.Count > 1)
            {
                float[] weights = new float[validTargets.Count];

                for (int i = 0; i < weights.Length; i++)
                {
                    weights[i] = validTargets[i].ComponentDefinition.SlotSize;
                }

                return validTargets[StaticHelper.GetRandomIndexByWeight(weights)];
            }

            return validTargets[0];
        }

        return null;
    }

    List<ComponentGroup> GetComponentGroups(ComponentData[] componentDatas, int groupCount)
    {
        List<ComponentGroup> componentGroups = new List<ComponentGroup>();

        for (int i = 0; i < groupCount; i++)
        {
            List<ComponentSaved> componentSaves = new List<ComponentSaved>();

            foreach (ComponentData componentData in componentDatas)
            {
                if (componentData.isDestroyed)
                    continue;

                if (componentData.groupIndex == i)
                {
                    ComponentSaved componentSaved = new ComponentSaved();
                    componentSaved.ComponentDefinition = componentData.ComponentDefinition.Key;

                    if (componentData.WeaponGrouping != null)
                    {
                        componentSaved.WeaponGrouping = new WeaponGrouping(componentData.WeaponGrouping);
                    }
                    else
                    {
                        componentSaved.WeaponGrouping = new WeaponGrouping();
                    }

                    componentSaves.Add(componentSaved);
                }
            }

            ComponentGroup componentGroup = new ComponentGroup();
            componentGroup.ComponentsSaved = componentSaves.ToArray();
            componentGroups.Add(componentGroup);
        }

        return componentGroups;
    }

    public void StripComponents()
    {
        componentsHead = new ComponentData[0];
        componentsTorsoCenter = new ComponentData[0];
        componentsTorsoLeft = new ComponentData[0];
        componentsTorsoRight = new ComponentData[0];
        componentsArmLeft = new ComponentData[0];
        componentsArmRight = new ComponentData[0];
        componentsLegLeft = new ComponentData[0];
        componentsLegRight = new ComponentData[0];

        ammoPoolsHead = new AmmoPool[0];
        ammoPoolsTorsoCenter = new AmmoPool[0];
        ammoPoolsTorsoLeft = new AmmoPool[0];
        ammoPoolsTorsoRight = new AmmoPool[0];
        ammoPoolsArmLeft = new AmmoPool[0];
        ammoPoolsArmRight = new AmmoPool[0];

        RecalculateStats();
    }

    public void RepairArmor()
    {
        if (internalHead > 0)
            armorHead = armorMaxHead;

        if (internalTorsoCenter > 0)
        {
            armorTorsoCenter = armorMaxTorsoCenter;
            armorTorsoCenterRear = armorMaxTorsoCenterRear;
        }

        if (internalTorsoLeft > 0)
        {
            armorTorsoLeft = armorMaxTorsoLeft;
            armorTorsoLeftRear = armorMaxTorsoLeftRear;
        }

        if (internalTorsoRight > 0)
        {
            armorTorsoRight = armorMaxTorsoRight;
            armorTorsoRightRear = armorMaxTorsoRightRear;
        }

        if (internalArmLeft > 0)
            armorArmLeft = armorMaxArmLeft;

        if (internalArmRight > 0)
            armorArmRight = armorMaxArmRight;

        if (internalLegLeft > 0)
            armorLegLeft = armorMaxLegLeft;

        if (internalLegRight > 0)
            armorLegRight = armorMaxLegRight;
    }

    public void RepairIntenals(bool repairHead, bool repairTorsoCenter, bool repairTorsoLeft, bool repairTorsoRight, bool repairArmLeft, bool repairArmRight, bool repairLegLeft, bool repairLegRight)
    {
        if (repairHead)
            internalHead = internalMaxHead;

        if (repairTorsoCenter)
            internalTorsoCenter = internalMaxTorsoCenter;

        if (repairTorsoLeft)
            internalTorsoLeft = internalMaxTorsoLeft;

        if (repairTorsoRight)
            internalTorsoRight = internalMaxTorsoRight;

        if (repairArmLeft)
            internalArmLeft = internalMaxArmLeft;

        if (repairArmRight)
            internalArmRight = internalMaxArmRight;

        if (repairLegLeft)
            internalLegLeft = internalMaxLegLeft;

        if (repairLegRight)
            internalLegRight = internalMaxLegRight;

        if (internalHead > 0 && internalTorsoCenter > 0 && (internalLegLeft > 0 || internalLegRight > 0))
            isDestroyed = false;
    }

    public void RepairComponents()
    {
        if (internalHead > 0)
            RepairComponets(componentsHead);

        if (internalTorsoCenter > 0)
            RepairComponets(componentsTorsoCenter);

        if (internalTorsoLeft > 0)
            RepairComponets(componentsTorsoLeft);

        if (internalTorsoRight > 0)
            RepairComponets(componentsTorsoRight);

        if (internalArmLeft > 0)
            RepairComponets(componentsArmLeft);

        if (internalArmRight > 0)
            RepairComponets(componentsArmRight);

        if (internalLegLeft > 0)
            RepairComponets(componentsLegLeft);

        if (internalLegRight > 0)
            RepairComponets(componentsLegRight);
    }

    void RepairComponets(ComponentData[] componentDatas)
    {
        foreach (ComponentData componentData in componentDatas)
        {
            if (!componentData.isDestroyed)
            {
                componentData.Repair();
            }
        }
    }

    public void GetKill(UnitData killedUnit)
    {
        if (killedUnit is MechData)
        {
            MechData killedMech = killedUnit as MechData;

            if (currentMechPilot != null)
            {
                currentMechPilot.missionMechKills++;

                currentMechPilot.missionExperience += ResourceManager.Instance.GameConstants.GetKillExperience(killedMech.MechChassis.UnitClass);
            }
            else if (mechController is MechControllerPlayer)
            {
                Career career = GlobalDataManager.Instance.currentCareer;

                if (career != null)
                {
                    career.playerMissionMechKills++;
                }
            }
        }
        else if (killedUnit is GroundVehicleData)
        {
            GroundVehicleData killedGroundVehicle = killedUnit as GroundVehicleData;

            if (currentMechPilot != null)
            {
                currentMechPilot.missionVehicleKills++;

                currentMechPilot.missionExperience += ResourceManager.Instance.GameConstants.GetKillExperience(killedGroundVehicle.groundVehicleDefinition.UnitClass);
            }
            else if (mechController is MechControllerPlayer)
            {
                Career career = GlobalDataManager.Instance.currentCareer;

                if (career != null)
                {
                    career.playerMissionVehicleKills++;
                }
            }
        }
    }

    public void ResetMissionData()
    {
        if (currentMechPilot != null)
        {
            currentMechPilot.missionExperience = 0;
        }
    }

    protected override void Die()
    {
        mechController.CreateDeathExplosion();
        mechController.Die();
    }

    [System.Serializable]
    public class MechSectionData
    {
        public ArmorType armorType;

        public ComponentData[] components;

        public AmmoPool[] ammoPools;

        public float armor;
        public int armorMax;

        public float internalHealth;
        public int internalHealthMax;

        public float ArmorPercentage { get => armor / armorMax; }

        public float InternalHealthPercentage { get => internalHealth / internalHealthMax; }

        public bool IsDestroyed { get => internalHealth <= 0; }

        ComponentData CriticalTarget
        {
            get
            {
                List<ComponentData> validTargets = new List<ComponentData>();

                foreach (ComponentData componentData in components)
                {
                    if (!componentData.isDestroyed)
                    {
                        validTargets.Add(componentData);
                    }
                }

                if (validTargets.Count > 0)
                {
                    if (validTargets.Count > 1)
                    {
                        float[] weights = new float[validTargets.Count];

                        for (int i = 0; i < weights.Length; i++)
                        {
                            weights[i] = validTargets[i].ComponentDefinition.SlotSize;
                        }

                        return validTargets[StaticHelper.GetRandomIndexByWeight(weights)];
                    }

                    return validTargets[0];
                }

                return null;
            }
        }

        public virtual void RepairArmor()
        {
            armor = armorMax;
        }

        public void RepairComponets()
        {
            foreach (ComponentData componentData in components)
            {
                if (!componentData.isDestroyed)
                {
                    componentData.Repair();
                }
            }
        }

        public void ReloadAmmo()
        {
            foreach (ComponentData componentData in components)
            {
                componentData.ReloadAmmo();
            }
        }
    }


    [System.Serializable]
    public class MechSectionTwoSidedData : MechSectionData
    {
        public float armorRear;
        public int armorRearMax;

        public float ArmorRearPercentage { get => armorRear / armorRearMax; }

        public override void RepairArmor()
        {
            armor = armorMax;
            armorRear = armorRearMax;
        }
    }
}
