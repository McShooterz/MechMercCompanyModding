using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CareerSave
{
    public string UniqueIdentifier = "1";

    public string Callsign = "Test Callsign";

    public string CompanyName = "Test Company";

    public string PlayerPilotImageKey = "Pilot_Generic_1";

    public FactionLogo FactionLogo;

    public string CurrentScreen = "HomeBase";

    public int Funds = 3000000;

    public bool Deceased = false;

    public bool Retired = false;

    public bool Bankrupt = false;

    public int Reputation = 0;

    public int Infamy = 0;

    public bool Permadeath = false;

    public int MechNameNumber = 0;

    public int PlayerMechKills = 0;

    public int PlayerVehicleKills = 0;

    public int PlayerMissionMechKills = 0;

    public int PlayerMissionVehicleKills = 0;

    public float ContractPayModifier = 1.0f;

    public float MissionPayModifier = 1.0f;

    public float BountyPayModifier = 1.0f;

    public float RepairCostModifier = 1.0f;

    public bool LastMissionSuccessful;

    public System.Guid DutyRosterMechPlayer = System.Guid.Empty;

    public System.Guid[] DutyRosterMechs = new System.Guid[] { System.Guid.Empty, System.Guid.Empty, System.Guid.Empty, System.Guid.Empty, System.Guid.Empty, System.Guid.Empty, System.Guid.Empty, System.Guid.Empty, System.Guid.Empty, System.Guid.Empty, System.Guid.Empty };

    public System.Guid[] DutyRosterPilots = new System.Guid[] { System.Guid.Empty, System.Guid.Empty, System.Guid.Empty, System.Guid.Empty, System.Guid.Empty, System.Guid.Empty, System.Guid.Empty, System.Guid.Empty, System.Guid.Empty, System.Guid.Empty, System.Guid.Empty };

    public System.Guid[] DropshipMechs = new System.Guid[] { System.Guid.Empty, System.Guid.Empty, System.Guid.Empty, System.Guid.Empty, System.Guid.Empty, System.Guid.Empty, System.Guid.Empty, System.Guid.Empty, System.Guid.Empty, System.Guid.Empty, System.Guid.Empty, System.Guid.Empty };

    public GameDate GameDate;

    public InventorySave Inventory;

    public MechSave[] Mechs = new MechSave[0];

    public PilotSave[] Pilots = new PilotSave[0];

    public MechSave[] MechsSalvage = new MechSave[0];

    public string[] ComponentsSalvage = new string[0];

    public MechMarketSave[] MechMarketEntries = new MechMarketSave[0];

    public InventorySave MarketInventory;

    public PilotSave[] PilotsForHire = new PilotSave[0];

    public ContractSave[] AvailableContracts = new ContractSave[0];

    public ObjectiveSave[] LastMissionObjectives = new ObjectiveSave[0];

    public Sprite GetPlayerPilotSprite()
    {
        Texture2D texture2D = ResourceManager.Instance.GetPilotTexture2D(PlayerPilotImageKey);

        if (texture2D != null)
            return StaticHelper.GetSpriteUI(texture2D);

        return null;
    }
}
