using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponsManagerUI : MonoBehaviour
{
    [SerializeField]
    WeaponUI prefabWeaponUI;

    [SerializeField]
    EquipmentUI prefabEquipmentUI;

    [SerializeField]
    FiringModeUI firingModeUI;

    [SerializeField]
    WeaponUI[] weaponUIs;

    [SerializeField]
    EquipmentUI[] equipmentUIs;

    [SerializeField]
    int rowIndex = 0;

    [SerializeField]
    int columnIndex = 0;

    int TotalRows
    {
        get
        {
            if (weaponUIs.Length > 0)
            {
                return weaponUIs.Length + equipmentUIs.Length + 1;
            }

            return equipmentUIs.Length;
        }
    }

    public void InitializeWeaponUI(WeaponController[] weaponControllers, EquipmentController[] equipmentControllers)
    {
        if (weaponControllers.Length > 0)
        {
            List<WeaponUI> weaponListUI = new List<WeaponUI>();

            Color groupDefault = ResourceManager.Instance.GameConstants.WeaponGroupColorDefault;
            Color groupActive = ResourceManager.Instance.GameConstants.WeaponGroupColorActive;

            for (int i = 0; i < weaponControllers.Length; i++)
            {
                WeaponController weaponController = weaponControllers[i];

                GameObject weaponGameObject = Instantiate(prefabWeaponUI.gameObject, transform);
                weaponGameObject.SetActive(true);

                WeaponUI weaponUI = weaponGameObject.GetComponent<WeaponUI>();
                weaponListUI.Add(weaponUI);

                weaponUI.WeaponNameText.text = weaponControllers[i].GetDisplayName();

                weaponUI.RangeText.text = (weaponControllers[i].GetRangeEffective() * 10f).ToString("0.") + "m";

                if (weaponController.weaponGrouping.WeaponGroup1)
                {
                    weaponUI.Group1Text.color = groupActive;
                }
                else
                {
                    weaponUI.Group1Text.color = groupDefault;
                }

                if (weaponController.weaponGrouping.WeaponGroup2)
                {
                    weaponUI.Group2Text.color = groupActive;
                }
                else
                {
                    weaponUI.Group2Text.color = groupDefault;
                }

                if (weaponController.weaponGrouping.WeaponGroup3)
                {
                    weaponUI.Group3Text.color = groupActive;
                }
                else
                {
                    weaponUI.Group3Text.color = groupDefault;
                }

                if (weaponController.weaponGrouping.WeaponGroup4)
                {
                    weaponUI.Group4Text.color = groupActive;
                }
                else
                {
                    weaponUI.Group4Text.color = groupDefault;
                }

                if (weaponController.weaponGrouping.WeaponGroup5)
                {
                    weaponUI.Group5Text.color = groupActive;
                }
                else
                {
                    weaponUI.Group5Text.color = groupDefault;
                }

                if (weaponController.weaponGrouping.WeaponGroup6)
                {
                    weaponUI.Group6Text.color = groupActive;
                }
                else
                {
                    weaponUI.Group6Text.color = groupDefault;
                }

                if (weaponController is BeamWeaponController || !(weaponController as ProjectileWeaponController).ProjectileWeaponDefinition.RequiresAmmo || (weaponControllers[i] as ProjectileWeaponController).AmmoEmpty)
                {
                    Destroy(weaponUI.AmmoText.gameObject);
                    Destroy(weaponUI.AmmoTypeText.gameObject);
                }
            }            

            weaponUIs = weaponListUI.ToArray();
        }

        if (equipmentControllers.Length > 0)
        {
            List<EquipmentUI> equipmentListUI = new List<EquipmentUI>();

            for (int i = 0; i < equipmentControllers.Length; i++)
            {
                EquipmentController equipmentController = equipmentControllers[i];

                GameObject equipmentGameObject = Instantiate(prefabEquipmentUI.gameObject, transform);
                equipmentGameObject.SetActive(true);

                EquipmentUI equipmentUI = equipmentGameObject.GetComponent<EquipmentUI>();
                equipmentListUI.Add(equipmentUI);

                equipmentUI.NameText.text = equipmentController.Definition.GetDisplayName();

                if (equipmentController.Definition.EquipmentModes.Length > 0)
                {
                    equipmentUI.StateText.text = equipmentController.CurrentMode.GetDisplayName();
                }
                else
                {
                    equipmentUI.StateText.text = "";
                }

                if (!equipmentController.RequiresAmmo)
                {
                    Destroy(equipmentUI.AmmoText.gameObject);
                }
            }

            equipmentUIs = equipmentListUI.ToArray();
        }

        if (weaponControllers.Length > 0)
        {
            FiringModeUI oldFiringModeUI = firingModeUI;

            firingModeUI = Instantiate(firingModeUI.gameObject, transform).GetComponent<FiringModeUI>();

            Destroy(oldFiringModeUI.gameObject);

            firingModeUI.StateHighlight1.SetActive(false);
            firingModeUI.StateHighlight2.SetActive(false);
            firingModeUI.StateHighlight3.SetActive(false);
            firingModeUI.StateHighlight4.SetActive(false);
            firingModeUI.StateHighlight5.SetActive(false);
            firingModeUI.StateHighlight6.SetActive(false);
        }
        else
        {
            firingModeUI.gameObject.SetActive(false);
        }

        Destroy(prefabWeaponUI.gameObject);
        Destroy(prefabEquipmentUI.gameObject);

        if (weaponControllers.Length > 0 || equipmentControllers.Length > 0)
        {
            SetHighlight(true, 0, 0);
        }
    }

    public void UpdateWeaponGrouping(WeaponController[] weaponControllers)
    {
        Color groupDefault = ResourceManager.Instance.GameConstants.WeaponGroupColorDefault;
        Color groupActive = ResourceManager.Instance.GameConstants.WeaponGroupColorActive;

        for (int i = 0; i < weaponControllers.Length; i++)
        {
            if (weaponControllers[i].weaponGrouping.WeaponGroup1)
            {
                weaponUIs[i].Group1Text.color = groupActive;
            }
            else
            {
                weaponUIs[i].Group1Text.color = groupDefault;
            }

            if (weaponControllers[i].weaponGrouping.WeaponGroup2)
            {
                weaponUIs[i].Group2Text.color = groupActive;
            }
            else
            {
                weaponUIs[i].Group2Text.color = groupDefault;
            }

            if (weaponControllers[i].weaponGrouping.WeaponGroup3)
            {
                weaponUIs[i].Group3Text.color = groupActive;
            }
            else
            {
                weaponUIs[i].Group3Text.color = groupDefault;
            }

            if (weaponControllers[i].weaponGrouping.WeaponGroup4)
            {
                weaponUIs[i].Group4Text.color = groupActive;
            }
            else
            {
                weaponUIs[i].Group4Text.color = groupDefault;
            }

            if (weaponControllers[i].weaponGrouping.WeaponGroup5)
            {
                weaponUIs[i].Group5Text.color = groupActive;
            }
            else
            {
                weaponUIs[i].Group5Text.color = groupDefault;
            }

            if (weaponControllers[i].weaponGrouping.WeaponGroup6)
            {
                weaponUIs[i].Group6Text.color = groupActive;
            }
            else
            {
                weaponUIs[i].Group6Text.color = groupDefault;
            }
        }
    }

    public void SetWeaponColorNormal(int index)
    {
        weaponUIs[index].WeaponNameText.color = ResourceManager.Instance.GameConstants.WeaponGroupColorDefault;
    }

    public void SetWeaponColorOutOfRange(int index)
    {
        weaponUIs[index].WeaponNameText.color = ResourceManager.Instance.GameConstants.WeaponColorOutOfRange;
    }

    public void SetWeaponColorDestroyed(int weaponIndex)
    {
        weaponUIs[weaponIndex].WeaponNameText.color = ResourceManager.Instance.GameConstants.WeaponColorDestroyed;
    }

    public void SetEquipmentStateName(int index, string name)
    {
        equipmentUIs[index].StateText.text = name;
    }

    public void SetEquipmentColorDestroyed(int index)
    {
        equipmentUIs[index].NameText.color = Color.red;
    }

    public void SetWeaponColorJammed(int weaponIndex)
    {
        weaponUIs[weaponIndex].WeaponNameText.color = new Color(1.0f, 0.64f, 0.0f, 1.0f);
    }

    public void SetWeaponRecycleBar(int index, float value)
    {
        weaponUIs[index].WeaponRefireBar.value = value;
    }

    public void SetEquipmentBar(int index, float value)
    {
        equipmentUIs[index].Bar.value = value;
    }

    public void SetWeaponJammingBar(int weaponIndex, float value)
    {
        weaponUIs[weaponIndex].JammingBar.value = value;
    }

    public void SetWeaponAmmoText(int weaponIndex, string ammoName, string ammoCount)
    {
        weaponUIs[weaponIndex].AmmoText.text = ammoCount;
        weaponUIs[weaponIndex].AmmoTypeText.text = ammoName;
    }

    public void SetEquipmentAmmoText(int index, string count)
    {
        equipmentUIs[index].AmmoText.text = count;
    }

    public void SetFiringModeText(string state1Text, string state2Text, string state3Text, string state4Text, string state5Text, string state6Text)
    {
        firingModeUI.State1Text.text = state1Text;
        firingModeUI.State2Text.text = state2Text;
        firingModeUI.State3Text.text = state3Text;
        firingModeUI.State4Text.text = state4Text;
        firingModeUI.State5Text.text = state5Text;
        firingModeUI.State6Text.text = state6Text;
    }

    public void UpdateChainFireSelection(int groupIndex1, int groupIndex2, int groupIndex3, int groupIndex4, int groupIndex5, int groupIndex6)
    {
        for (int i = 0; i < weaponUIs.Length; i++)
        {
            weaponUIs[i].SetFireSelection(i == groupIndex1, i == groupIndex2, i == groupIndex3, i == groupIndex4, i == groupIndex5, i == groupIndex6);
        }
    }

    public void ClearChainFireSelection()
    {
        for (int i = 0; i < weaponUIs.Length; i++)
        {
            weaponUIs[i].ClearFireSelection();
        }
    }

    public void RowChangeUp()
    {
        if (TotalRows > 1)
        {
            SetHighlight(false, rowIndex, columnIndex);

            rowIndex--;

            if (rowIndex < 0)
            {
                rowIndex = TotalRows - 1;
            }
            else if (rowIndex == TotalRows - 1 && columnIndex > 5)
            {
                columnIndex = 5;
            }

            SetHighlight(true, rowIndex, columnIndex);
        }
    }

    public void RowChangeDown()
    {
        if (TotalRows > 1)
        {
            SetHighlight(false, rowIndex, columnIndex);

            rowIndex++;

            if (rowIndex == TotalRows)
            {
                rowIndex = 0;
            }
            else if (rowIndex == TotalRows - 1 && columnIndex > 5)
            {
                columnIndex = 5;
            }

            SetHighlight(true, rowIndex, columnIndex);
        }
    }

    public void ColumnChangeRight()
    {
        if (weaponUIs.Length > 0)
        {
            SetHighlight(false, rowIndex, columnIndex);

            columnIndex++;

            if (rowIndex < weaponUIs.Length && columnIndex > 6)
            {
                columnIndex = 0;
            }
            else if (rowIndex == TotalRows - 1 && columnIndex > 5)
            {
                columnIndex = 0;
            }

            SetHighlight(true, rowIndex, columnIndex);
        }
    }

    public void ColumnChangeLeft()
    {
        if (weaponUIs.Length > 0)
        {
            SetHighlight(false, rowIndex, columnIndex);

            columnIndex--;

            if (columnIndex < 0)
            {
                if (rowIndex < weaponUIs.Length)
                {
                    columnIndex = 6;
                }
                else if (rowIndex == TotalRows - 1)
                {
                    columnIndex = 5;
                }
            }

            SetHighlight(true, rowIndex, columnIndex);
        }
    }

    public void SelectElement()
    {
        if (TotalRows > 0)
        {
            if (rowIndex < weaponUIs.Length)
            {
                if (columnIndex < 6)
                {
                    MechControllerPlayer.Instance.ToggleWeaponGroup(rowIndex, columnIndex);
                }
                else
                {
                    MechControllerPlayer.Instance.CycleWeaponAmmo(rowIndex);
                }
            }
            else if (rowIndex < weaponUIs.Length + equipmentUIs.Length)
            {
                MechControllerPlayer.Instance.CycleEquipmentMode(rowIndex - weaponUIs.Length);
            }
            else
            {
                MechControllerPlayer.Instance.CycleFiringMode(columnIndex);
            }
        }
    }

    void SetHighlight(bool state, int row, int column)
    {
        if (row < weaponUIs.Length)
        {
            switch (column)
            {
                case 0:
                    {
                        weaponUIs[row].HighlightGroup1.SetActive(state);
                        break;
                    }
                case 1:
                    {
                        weaponUIs[row].HighlightGroup2.SetActive(state);
                        break;
                    }
                case 2:
                    {
                        weaponUIs[row].HighlightGroup3.SetActive(state);
                        break;
                    }
                case 3:
                    {
                        weaponUIs[row].HighlightGroup4.SetActive(state);
                        break;
                    }
                case 4:
                    {
                        weaponUIs[row].HighlightGroup5.SetActive(state);
                        break;
                    }
                case 5:
                    {
                        weaponUIs[row].HighlightGroup6.SetActive(state);
                        break;
                    }
                case 6:
                    {
                        weaponUIs[row].HighlightAmmo.SetActive(state);
                        break;
                    }
            }
        }
        else if (row < weaponUIs.Length + equipmentUIs.Length)
        {
            equipmentUIs[row - weaponUIs.Length].StateHighlight.SetActive(state);
        }
        else
        {
            switch (column)
            {
                case 0:
                    {
                        firingModeUI.StateHighlight1.SetActive(state);
                        break;
                    }
                case 1:
                    {
                        firingModeUI.StateHighlight2.SetActive(state);
                        break;
                    }
                case 2:
                    {
                        firingModeUI.StateHighlight3.SetActive(state);
                        break;
                    }
                case 3:
                    {
                        firingModeUI.StateHighlight4.SetActive(state);
                        break;
                    }
                case 4:
                    {
                        firingModeUI.StateHighlight5.SetActive(state);
                        break;
                    }
                case 5:
                    {
                        firingModeUI.StateHighlight6.SetActive(state);
                        break;
                    }
            }
        }
    }
}
