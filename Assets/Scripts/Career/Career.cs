using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class Career
{
    public string uniqueIdentifier = "1";

    public string callsign = "Test Callsign";

    public string companyName = "Test Company";

    public string playerPilotImageKey = "Pilot_Generic_1";

    public FactionLogo factionLogo;

    public string currentScreen = "HomeBase";

    public int funds = 999999999;

    public bool deceased = false;

    public bool retired = false;

    public bool bankrupt = false;

    public int reputation = 0;

    public int infamy = 0;

    public bool lastMissionSuccessful;

    public bool permadeath = false;

    int mechNameNumber = 0;

    public int playerMechKills = 0;

    public int playerVehicleKills = 0;

    public int playerMissionMechKills = 0;

    public int playerMissionVehicleKills = 0;

    public float contractPayModifier = 1.0f;

    public float missionPayModifier = 1.0f;

    public float bountyPayModifier = 1.0f;

    public float repairCostModifier = 1.0f;

    public GameDate gameDate;

    public Inventory inventory;

    public ContractData currentContract;

    public Dictionary<System.Guid, MechPilot> Pilots { get; set; } = new Dictionary<System.Guid, MechPilot>();

    public List<MechData> MechsSalvage { get; private set; } = new List<MechData>();

    public List<ComponentDefinition> ComponentsSalvage { get; set; } = new List<ComponentDefinition>();

    public ObjectiveSave[] lastMissionObjectives = new ObjectiveSave[0];

    public List<MechMarketEntry> MechMarketEntries { get; } = new List<MechMarketEntry>();

    public Inventory MarketInventory { get; private set; } = new Inventory();

    public List<MechPilot> PilotsForHire { get; private set; } = new List<MechPilot>();

    public MechData CustomizingMechData { get; set; }

    public Dictionary<System.Guid, MechData> Mechs { get; } = new Dictionary<System.Guid, MechData>();

    System.Guid[] dropshipMechs = new System.Guid[12];

    System.Guid dutyRosterMechPlayer = System.Guid.Empty;

    System.Guid[] dutyRosterMechs = new System.Guid[11];

    System.Guid[] dutyRosterPilots = new System.Guid[11];

    public List<ContractData> AvailableContracts { get; private set; } = new List<ContractData>();

    public int MechNameNumber { get => ++mechNameNumber; }

    public int WeeklyExpenses
    {
        get
        {
            int expenses = 20000;

            expenses += MechMaintenance;
            expenses += PilotWeeklySalary;

            return expenses;
        }
    }

    public int MechMaintenance
    {
        get
        {
            int maintenance = 0;

            foreach (MechData mechData in Mechs.Values)
            {
                maintenance += mechData.MechChassis.Maintenance;
            }

            return maintenance;
        }
    }

    public int PilotWeeklySalary
    {
        get
        {
            int weeklySalary = 0;

            foreach (MechPilot mechPilot in Pilots.Values)
            {
                weeklySalary += mechPilot.contractWeeklySalary;
            }

            return weeklySalary;
        }
    }

    public int MechsValue
    {
        get
        {
            int sum = 0;

            foreach (MechData mechData in Mechs.Values)
            {
                sum += mechData.MarketValue;
                sum += mechData.ComponentValue;
            }

            return sum;
        }
    }

    public int CompanyValue
    {
        get
        {
            int sum = funds;

            sum += MechsValue;
            sum += inventory.InventoryValue;

            return sum;
        }
    }

    public Sprite PlayerPilotSprite
    {
        get
        {
            Texture2D pilotTexture2D = ResourceManager.Instance.GetPilotTexture2D(playerPilotImageKey);

            if (pilotTexture2D != null)
            {
                return StaticHelper.GetSpriteUI(pilotTexture2D);
            }

            return null;
        }
    }

    public int PlayerMissionKills { get => playerMissionMechKills + playerMissionVehicleKills; }

    public bool IsReal { get; private set; } = false;

    public MechData DutyRosterMechPlayer 
    { 
        get => Mechs.TryGetValueOrDefault(dutyRosterMechPlayer);

        set
        {
            if (value != null)
            {
                dutyRosterMechPlayer = value.guid;
            }
            else
            {
                dutyRosterMechPlayer = System.Guid.Empty;
            }
        }
    }

    public MechData[] DutyRosterMechs
    {
        get
        {
            MechData[] mechDatas = new MechData[11];

            for (int i = 0; i < 11; i++)
            {
                mechDatas[i] = Mechs.TryGetValueOrDefault(dutyRosterMechs[i]);
            }

            return mechDatas;
        }
        set
        {
            for (int i = 0; i < 11; i++)
            {
                MechData mechData = value[i];

                if (mechData != null)
                {
                    dutyRosterMechs[i] = mechData.guid;
                }
                else
                {
                    dutyRosterMechs[i] = System.Guid.Empty;
                }
            }
        }
    }

    public MechPilot[] DutyRosterPilots
    {
        get
        {
            MechPilot[] pilots = new MechPilot[11];

            for (int i = 0; i < 11; i++)
            {
                pilots[i] = Pilots.TryGetValueOrDefault(dutyRosterPilots[i]);
            }

            return pilots;
        }
        set
        {
            for (int i = 0; i < 11; i++)
            {
                MechPilot pilot = value[i];

                if (pilot != null)
                {
                    dutyRosterMechs[i] = pilot.guid;
                }
                else
                {
                    dutyRosterMechs[i] = System.Guid.Empty;
                }
            }
        }
    }

    public bool DutyRosterValid
    {
        get
        {
            MechData mechDataPlayer = DutyRosterMechPlayer;

            if (mechDataPlayer == null || !mechDataPlayer.IsMissionReady)
                return false;

            MechData[] dutyRosterMechsCopy = DutyRosterMechs;
            MechPilot[] dutyRosterPilotsCopy = DutyRosterPilots;

            for (int i = 0; i < 11; i++)
            {
                MechData mechData = dutyRosterMechsCopy[i];
                MechPilot pilot = dutyRosterPilotsCopy[i];

                if (mechData != null)
                {
                    if (!mechData.IsMissionReady)
                        return false;

                    if (pilot == null || pilot.PilotStatus != PilotStatusType.Ready)
                        return false;
                }
                else if (pilot != null)
                    return false;
            }

            return true;
        }
    }

    public MechData[] DropshipMechs
    {
        get
        {
            MechData[] mechDatas = new MechData[12];

            for (int i = 0; i < 12; i++)
            {
                mechDatas[i] = Mechs.TryGetValueOrDefault(dropshipMechs[i]);
            }

            return mechDatas;
        }
        set
        {
            for (int i = 0; i < 12; i++)
            {
                MechData mechData = value[i];

                if (mechData != null)
                {
                    dropshipMechs[i] = mechData.guid;
                }
                else
                {
                    dropshipMechs[i] = System.Guid.Empty;
                }
            }
        }
    }

    public CareerSave CareerSave
    {
        get
        {
            CareerSave careerSave = null;

            try
            {
                List<MechSave> mechSaves = new List<MechSave>();

                foreach (MechData mech in Mechs.Values)
                {
                    MechSave mechSave = mech.MechSave;

                    mechSaves.Add(mechSave);
                }

                List<PilotSave> pilotSaves = new List<PilotSave>();

                foreach (MechPilot mechPilot in Pilots.Values)
                {
                    PilotSave pilotSave = mechPilot.PilotSave;

                    pilotSaves.Add(pilotSave);
                }

                List<MechSave> mechsSalvage = new List<MechSave>();

                foreach (MechData mechdata in MechsSalvage)
                {
                    mechsSalvage.Add(mechdata.MechSave);
                }

                List<string> componentsSalvage = new List<string>();

                foreach (ComponentDefinition componentDefinition in ComponentsSalvage)
                {
                    componentsSalvage.Add(componentDefinition.Key);
                }

                List<MechMarketSave> mechMarketSaves = new List<MechMarketSave>();

                foreach (MechMarketEntry mechMarketEntry in MechMarketEntries)
                {
                    mechMarketSaves.Add(mechMarketEntry.MechMarketSave);
                }

                List<PilotSave> pilotsForHire = new List<PilotSave>();

                foreach (MechPilot mechPilot in PilotsForHire)
                {
                    pilotsForHire.Add(mechPilot.PilotSave);
                }

                List<ContractSave> contractSaves = new List<ContractSave>();

                foreach (ContractData contractData in AvailableContracts)
                {
                    ContractSave contractSave = contractData.ContractSave;

                    if (currentContract == contractData)
                        contractSave.IsCareerCurrent = true;

                    contractSaves.Add(contractSave);
                }

                careerSave = new CareerSave()
                {
                    UniqueIdentifier = uniqueIdentifier,
                    Callsign = callsign,
                    CompanyName = companyName,
                    PlayerPilotImageKey = playerPilotImageKey,
                    FactionLogo = factionLogo,
                    CurrentScreen = currentScreen,
                    Funds = funds,
                    Deceased = deceased,
                    Retired = retired,
                    Bankrupt = bankrupt,
                    Reputation = reputation,
                    Infamy = infamy,
                    Permadeath = permadeath,
                    MechNameNumber = mechNameNumber,
                    GameDate = gameDate,
                    Inventory = inventory.InventorySave,
                    PlayerMechKills = playerMechKills,
                    PlayerVehicleKills = playerVehicleKills,
                    PlayerMissionMechKills = playerMissionMechKills,
                    PlayerMissionVehicleKills = playerMissionVehicleKills,
                    ContractPayModifier = contractPayModifier,
                    MissionPayModifier = missionPayModifier,
                    BountyPayModifier = bountyPayModifier,
                    RepairCostModifier = repairCostModifier,
                    LastMissionObjectives = lastMissionObjectives,
                    LastMissionSuccessful = lastMissionSuccessful,

                    DutyRosterMechPlayer = dutyRosterMechPlayer,
                    DutyRosterMechs = dutyRosterMechs,
                    DutyRosterPilots = dutyRosterPilots,
                    DropshipMechs = dropshipMechs,

                    Mechs = mechSaves.ToArray(),
                    Pilots = pilotSaves.ToArray(),
                    MechsSalvage = mechsSalvage.ToArray(),
                    ComponentsSalvage = componentsSalvage.ToArray(),
                    MechMarketEntries = mechMarketSaves.ToArray(),
                    MarketInventory = MarketInventory.InventorySave,
                    PilotsForHire = pilotsForHire.ToArray(),
                    AvailableContracts = contractSaves.ToArray(),
                };
            }
            catch(System.NullReferenceException exception)
            {
                Debug.LogError("Error: Failed to build Save");
                Debug.LogError("Error: " + exception.ToString());
            }

            return careerSave;
        }
    }

    public Career()
    {

    }

    public Career(string id)
    {
        IsReal = true;

        uniqueIdentifier = id;

        factionLogo = new FactionLogo();

        inventory = new Inventory();

        RebuildMarket();

        RebuildAvailableContracts();
    }

    public Career(CareerSave careerSave)
    {
        IsReal = true;

        uniqueIdentifier = careerSave.UniqueIdentifier;

        callsign = careerSave.Callsign;

        companyName = careerSave.CompanyName;

        playerPilotImageKey = careerSave.PlayerPilotImageKey;

        factionLogo = new FactionLogo(careerSave.FactionLogo);

        currentScreen = careerSave.CurrentScreen;

        funds = careerSave.Funds;

        deceased = careerSave.Deceased;

        retired = careerSave.Retired;

        bankrupt = careerSave.Bankrupt;

        reputation = careerSave.Reputation;

        infamy = careerSave.Infamy;

        permadeath = careerSave.Permadeath;

        mechNameNumber = careerSave.MechNameNumber;

        playerMechKills = careerSave.PlayerMechKills;

        playerVehicleKills = careerSave.PlayerVehicleKills;

        playerMissionMechKills = careerSave.PlayerMissionMechKills;

        playerMissionVehicleKills = careerSave.PlayerMissionVehicleKills;

        contractPayModifier = careerSave.ContractPayModifier;

        missionPayModifier = careerSave.MissionPayModifier;

        bountyPayModifier = careerSave.BountyPayModifier;

        repairCostModifier = careerSave.RepairCostModifier;

        lastMissionObjectives = careerSave.LastMissionObjectives;

        lastMissionSuccessful = careerSave.LastMissionSuccessful;

        gameDate = new GameDate(careerSave.GameDate);

        inventory = new Inventory(careerSave.Inventory);

        dutyRosterMechPlayer = careerSave.DutyRosterMechPlayer;

        System.Array.Copy(careerSave.DutyRosterMechs, dutyRosterMechs, 11);

        System.Array.Copy(careerSave.DutyRosterPilots, dutyRosterPilots, 11);

        System.Array.Copy(careerSave.DropshipMechs, dropshipMechs, 12);

        for (int i = 0; i < careerSave.Mechs.Length; i++)
        {
            MechSave mechSave = careerSave.Mechs[i];
            MechData mechData = new MechData(mechSave);

            if (mechData.guid == System.Guid.Empty)
            {
                mechData.guid = System.Guid.NewGuid();
            }

            Mechs[mechData.guid] = mechData;
        }

        for (int i = 0; i < careerSave.Pilots.Length; i++)
        {
            PilotSave pilotSave = careerSave.Pilots[i];
            MechPilot mechPilot = new MechPilot(pilotSave);

            if (mechPilot.guid == System.Guid.Empty)
            {
                mechPilot.guid = System.Guid.NewGuid();
            }

            Pilots[mechPilot.guid] = mechPilot;
        }

        for (int i = 0; i < careerSave.MechsSalvage.Length; i++)
        {
            MechsSalvage.Add(new MechData(careerSave.MechsSalvage[i]));
        }

        for (int i = 0; i < careerSave.ComponentsSalvage.Length; i++)
        {
            ComponentDefinition componentDefinition = ResourceManager.Instance.GetComponentDefinition(careerSave.ComponentsSalvage[i]);

            if (componentDefinition != null)
            {
                ComponentsSalvage.Add(componentDefinition);
            }
        }

        for (int i = 0; i < careerSave.MechMarketEntries.Length; i++)
        {
            MechMarketEntries.Add(new MechMarketEntry(careerSave.MechMarketEntries[i]));
        }

        MarketInventory = new Inventory(careerSave.MarketInventory);

        for (int i = 0; i < careerSave.PilotsForHire.Length; i++)
        {
            PilotsForHire.Add(new MechPilot(careerSave.PilotsForHire[i]));
        }

        for (int i = 0; i < careerSave.AvailableContracts.Length; i++)
        {
            ContractSave contractSave = careerSave.AvailableContracts[i];
            ContractData contractData = new ContractData(contractSave);

            if (contractSave.IsCareerCurrent)
                currentContract = contractData;

            AvailableContracts.Add(contractData);
        }
    }

    public void AdvanceWeek()
    {
        gameDate.AddDays(7);
        RebuildMarket();
        RebuildAvailableContracts();

        funds -= WeeklyExpenses;
    }

    public void RebuildAvailableContracts()
    {
        List<ContractRandomDefinition> contractRandomDefinitions = ResourceManager.Instance.GetContractRandomDefinitions();

        int contractCount;
        int existingCount;

        // Remove expired contracts
        if (currentContract != null)
        {
            if (currentContract.StartDate.GreaterThan(gameDate))
            {
                currentContract = null;
            }
        }

        for (int i = 0; i < AvailableContracts.Count; i++)
        {
            if (AvailableContracts[i].StartDate.GreaterThan(gameDate))
            {
                AvailableContracts.RemoveAt(i);
                i--;
            }
        }

        // Add new contracts
        foreach (ContractRandomDefinition contractRandomDefinition in contractRandomDefinitions)
        {
            contractCount = contractRandomDefinition.RandomContractCount;

            existingCount = 0;

            foreach (ContractData availableContract in AvailableContracts)
            {
                if (availableContract.ContractDefinition == contractRandomDefinition)
                {
                    existingCount++;
                }
            }

            for (int i = existingCount; i < contractCount; i++)
            {
                AvailableContracts.Add(new ContractData(contractRandomDefinition));
            }
        }
    }

    public void AddMechNew(MechData mechData)
    {
        mechData.guid = System.Guid.NewGuid();
        mechData.customName = mechData.MechChassis.GetDisplayName() + " " + MechNameNumber.ToString();

        Mechs.Add(mechData.guid, mechData);
    }

    public void RemoveMech(MechData mechData)
    {
        if (dutyRosterMechPlayer == mechData.guid)
        {
            dutyRosterMechPlayer = System.Guid.Empty;
        }

        for (int i = 0; i < 11; i++)
        {
            if (dutyRosterMechs[i] == mechData.guid)
            {
                dutyRosterMechs[i] = System.Guid.Empty;
                break;
            }
        }

        for (int i = 0; i < 12; i++)
        {
            if (dropshipMechs[i] == mechData.guid)
            {
                dropshipMechs[i] = System.Guid.Empty;
                break;
            }
        }

        Mechs.Remove(mechData.guid);
    }

    public void AddPilotNew(MechPilot mechPilot)
    {
        mechPilot.guid = System.Guid.NewGuid();
        Pilots.Add(mechPilot.guid, mechPilot);
    }

    public void RemovePilot(MechPilot mechPilot)
    {
        for (int i = 0; i < 11; i++)
        {
            if (dutyRosterPilots[i] == mechPilot.guid)
            {
                dutyRosterPilots[i] = System.Guid.Empty;
                break;
            }
        }

        Pilots.Remove(mechPilot.guid);
    }

    public int GetMechChassisCount(MechChassisDefinition mechChassisDefinition)
    {
        int sum = 0;

        foreach (MechData mechData in Mechs.Values)
        {
            if (mechData.MechChassis == mechChassisDefinition)
                sum++;
        }

        return sum;
    }

    public void BuildSalvageList()
    {
        MissionData missionData = GlobalData.Instance.ActiveMissionData;

        if (ComponentsSalvage == null)
            ComponentsSalvage = new List<ComponentDefinition>();

        ComponentsSalvage.Clear();
        MechsSalvage.Clear();

        if (missionData.successful)
        {
            List<UnitData> enemiesDestroyed = missionData.GetDestroyedEnemies();

            /*
            for (int i = 0; i < DutyRosterMechs.Length; i++)
            {
                MechData dutyRosterMech = DutyRosterMechs[i];

                if (dutyRosterMech != null && dutyRosterMech.isDestroyed)
                {
                    ComponentsSalvage.AddRange(dutyRosterMech.ComponentsSalvagable);
                    MechsSalvage.Add(dutyRosterMech);
                    dutyRosterMech.StripComponents();
                    RemoveMech(dutyRosterMech);
                }
            }*/

            foreach (UnitData enemyUnit in enemiesDestroyed)
            {
                ComponentsSalvage.AddRange(enemyUnit.ComponentsSalvagable);

                if (enemyUnit is MechData)
                {
                    MechData enemyMech = enemyUnit as MechData;
                    enemyMech.StripComponents();
                    MechsSalvage.Add(enemyUnit as MechData);
                }
            }

            MechsSalvage = MechsSalvage.OrderByDescending(mech => mech.MechChassis.MarketValue).ToList();
            ComponentsSalvage = ComponentsSalvage.OrderByDescending(component => component.MarketValue).ToList();
        }
        else
        {
            // Removed destroyed mechs
            MechData mechDataPlayer = Mechs[dutyRosterMechPlayer];         
            
            if (mechDataPlayer != null && mechDataPlayer.isDestroyed)
            {
                RemoveMech(mechDataPlayer);
            }

            for (int i = 0; i < 11; i++)
            {
                MechData dutyRosterMech = Mechs[dutyRosterMechs[i]];

                if (dutyRosterMech != null && dutyRosterMech.isDestroyed)
                {
                    RemoveMech(dutyRosterMech);
                }
            }
        }
    }

    public void RemoveDeceasedPilots()
    {
        List<MechPilot> pilotList = Pilots.Values.ToList();

        for (int i = 0; i < Pilots.Count; i++)
        {
            MechPilot pilot = pilotList[i];

            if (pilot.PilotStatus == PilotStatusType.Deceased)
            {
                RemovePilot(pilot);
                i--;
            }
        }
    }

    public void RebuildMarket()
    {
        MechMarketEntries.Clear();

        foreach (MechChassisDefinition mechChassisDefinition in ResourceManager.Instance.GetMechChassisDefinitions())
        {
            int count = mechChassisDefinition.GetRandomMarketCount();

            if (count > 0)
            {
                MechDesign mechDesign = mechChassisDefinition.GetDefaultDesign();

                if (mechDesign != null)
                {
                    MechMarketEntry mechMarketEntry = new MechMarketEntry()
                    {
                        MechDesign = mechDesign,
                        count = count,
                    };

                    MechMarketEntries.Add(mechMarketEntry);
                }
            }
        }

        MarketInventory.Clear();

        foreach (ComponentDefinition componentDefinition in ResourceManager.Instance.GetComponentDefinitions())
        {
            int count = componentDefinition.GetRandomMarketCount();

            if (count > 0)
            {
                MarketInventory.AddComponent(componentDefinition, count);
            }
        }

        PilotsForHire = MechPilot.GetRandomMechPilots(Random.Range(3, 7));

        foreach (MechPilot mechPilot in PilotsForHire)
        {
            mechPilot.GenerateContractValues();
        }
    }

    public void PurchaseMech(MechMarketEntry mechMarketEntry)
    {
        mechMarketEntry.count--;

        if (mechMarketEntry.count == 0)
        {
            MechMarketEntries.Remove(mechMarketEntry);
        }

        funds -= Mathf.CeilToInt(mechMarketEntry.MechDesign.GetMarketValue() * 1.25f);

        AddMechNew(new MechData(mechMarketEntry.MechDesign));
    }

    public void CheckDutyRoster()
    {
        MechStatusType mechStatusType;

        MechData playerMech = Mechs.TryGetValueOrDefault(dutyRosterMechPlayer);

        MechData[] dutyRosterMechsCopy = DutyRosterMechs;
        MechPilot[] dutyRosterPilotsCopy = DutyRosterPilots;

        if (playerMech != null)
        {
            mechStatusType = playerMech.MechStatus;

            if (mechStatusType != MechStatusType.Ready && mechStatusType != MechStatusType.Damaged)
                dutyRosterMechPlayer = System.Guid.Empty;
        }

        for (int i = 0; i < 11; i++)
        {
            MechData mechData = dutyRosterMechsCopy[i];

            if (mechData != null)
            {
                mechStatusType = mechData.MechStatus;

                if (mechStatusType != MechStatusType.Ready && mechStatusType != MechStatusType.Damaged)
                    dutyRosterMechs[i] = System.Guid.Empty;
            }
        }

        for (int i = 0; i < 11; i++)
        {
            MechPilot pilot = dutyRosterPilotsCopy[i];

            if (pilot != null && pilot.PilotStatus != PilotStatusType.Ready)
                dutyRosterPilots[i] = System.Guid.Empty;
        }
    }

    public void ApplyMissionKills()
    {
        playerMechKills += playerMissionMechKills;
        playerVehicleKills += playerMissionVehicleKills;

        playerMissionMechKills = 0;
        playerMissionVehicleKills = 0;
    }
}
