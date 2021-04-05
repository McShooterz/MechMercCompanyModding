using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundVehicleDefinition : Definition
{
    public string Prefab = "";

    public string DisplayName = "";

    public UnitClass UnitClass;

    public GroundVehicleMovement GroundVehicleMovement = GroundVehicleMovement.Tracked;

    public float PhysicsMass = 1000.0f;

    public float PhysicsFriction = 0.18f;

    public float EngineForce;

    public float EngineTurnForce;

    public float MaxSpeed;

    public float HoverHeight;

    public float HoverForce;

    public float FrontArmor;

    public float RearArmor;

    public float LeftArmor;

    public float RightArmor;

    public ArmorType ArmorTypeAllSides = ArmorType.standard;

    public int InternalHealth;

    public TurretSetting[] TurretSettings = new TurretSetting[0];

    public string[] Components = new string[0];

    public float MechLegDamage = 10.0f;


    public GameObject GetPrefab()
    {
        return ResourceManager.Instance.GetGroundVehiclePrefab(Prefab);
    }

    public string GetDisplayName()
    {
        return ResourceManager.Instance.GetLocalization(DisplayName);
    }
}
