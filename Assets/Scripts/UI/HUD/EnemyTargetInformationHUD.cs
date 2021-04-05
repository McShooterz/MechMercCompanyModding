using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyTargetInformationHUD : MonoBehaviour
{
    [SerializeField]
    Image targetInfoBackground;

    [SerializeField]
    Image damageDisplayBackground;

    [SerializeField]
    Text targetNameText;

    [SerializeField]
    Text targetDistanceText;

    [SerializeField]
    Image weaponDisplayBackground;

    [SerializeField]
    Text weaponDisplayText;

    [SerializeField]
    MechDamageDisplay mechDamageDisplay;

    [SerializeField]
    GroundVehicleDamageDisplay groundVehicleDamageDisplay;

    [SerializeField]
    TurretDamageDisplay turretDamageDisplay;

    [SerializeField]
    GenericDamageDisplay genericDamageDisplay;

    public void SetTargetInfo(UnitController unitController, Color color, string targetDistance)
    {
        if (unitController != null)
        {
            gameObject.SetActive(true);

            targetInfoBackground.color = color;
            damageDisplayBackground.color = color;
            targetNameText.color = color;
            targetDistanceText.color = color;
            weaponDisplayBackground.color = color;
            targetDistanceText.text = targetDistance;

            targetNameText.text = unitController.TargetingDisplayName;

            if (unitController is CombatUnitController)
            {
                weaponDisplayBackground.gameObject.SetActive(true);
                weaponDisplayText.text = unitController.GetWeaponsDisplay();
            }
            else
            {
                weaponDisplayBackground.gameObject.SetActive(false);
            }

            if (unitController is MechController)
            {
                MechController mechController = unitController as MechController;

                mechDamageDisplay.gameObject.SetActive(true);
                groundVehicleDamageDisplay.gameObject.SetActive(false);
                turretDamageDisplay.gameObject.SetActive(false);
                genericDamageDisplay.gameObject.SetActive(false);

                mechDamageDisplay.SetDisplays(mechController.MechData);
            }
            else if (unitController is GroundVehicleController)
            {
                GroundVehicleController groundVehicleController = unitController as GroundVehicleController;

                mechDamageDisplay.gameObject.SetActive(false);
                groundVehicleDamageDisplay.gameObject.SetActive(true);
                turretDamageDisplay.gameObject.SetActive(false);
                genericDamageDisplay.gameObject.SetActive(false);

                groundVehicleDamageDisplay.SetDisplays(groundVehicleController.GroundVehicleData);
            }
            else if (unitController is TurretUnitController)
            {
                TurretUnitController turretUnitController = unitController as TurretUnitController;

                mechDamageDisplay.gameObject.SetActive(false);
                groundVehicleDamageDisplay.gameObject.SetActive(false);
                turretDamageDisplay.gameObject.SetActive(true);
                genericDamageDisplay.gameObject.SetActive(false);

                turretDamageDisplay.SetDisplays(turretUnitController.TurretUnitData);
            }
            else if (unitController is BuildingController)
            {
                mechDamageDisplay.gameObject.SetActive(false);
                groundVehicleDamageDisplay.gameObject.SetActive(false);
                turretDamageDisplay.gameObject.SetActive(false);
                genericDamageDisplay.gameObject.SetActive(true);

                genericDamageDisplay.SetDisplay((unitController as BuildingController).BuildingData.HealthPercentage);
            }
            else
            {
                mechDamageDisplay.gameObject.SetActive(false);
                groundVehicleDamageDisplay.gameObject.SetActive(false);
                turretDamageDisplay.gameObject.SetActive(false);
                genericDamageDisplay.gameObject.SetActive(false);
            }
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
