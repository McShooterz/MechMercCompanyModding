using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using UnityEngine;
using UnityEngine.Audio;
using System.Reflection;
using System.Xml.Serialization;
#if UNITY_EDITOR
using UnityEditor;
#endif

public static class StaticHelper
{
    public static int RandomGeneratedSeed { get => Random.Range(-2147483648, 2147483647); }

    public static void SetCollidersLayer(int layer, Collider[] colliders)
    {
        for (int i = 0; i < colliders.Length; i++)
        {
            colliders[i].gameObject.layer = layer;
        }
    }

    public static int GetRandomIndexByWeight(float[] weights)
    {
        if (weights.Length == 0)
            return -1;

        float weightSum = 0;

        foreach (float weight in weights)
        {
            weightSum += weight;
        }

        float randomValue = Random.Range(0f, weightSum);

        for (int i = 0; i < weights.Length; i++)
        {
            if (weights[i] <= 0.0f)
                continue;

            randomValue -= weights[i];

            if (randomValue <= 0)
            {
                return i;
            }
        }

        // Return -1 for error
        return -1;
    }

    public static string ArrayToString<T>(T[] items) 
	{
		StringBuilder str = new StringBuilder("[");

		for (int i = 0; i < items.Length; i++) 
		{
			if (typeof(T).IsArray) 
			{
				// Todo: inner arrays
				str.Append($"\n\t{{{{ [ ... ] }}}},");
			}
			else 
			{
				str.Append($"\n\t{{{{ {items[i]} }}}},");
			}

		}

		str.Append("\n]");
		return str.ToString();
	}

    public static string AddColorTagToText(string text, string colorValue)
    {
        return "<color=#" + colorValue + ">" + text + "</color>";
    }

    public static string FormatMoney(int value)
    {
        return string.Format("{0:n0}", value) + " EUC";
    }

    public static string FormatTime(int value)
    {
        return (value / 60).ToString("0.") + ":" + (value % 60).ToString("00.");
    }

    public static string GetObjectiveStateText(ObjectiveState objectiveState)
    {
        switch (objectiveState)
        {
            case ObjectiveState.Active:
                {
                    return ResourceManager.Instance.GetLocalization("Active");
                }
            case ObjectiveState.Completed:
                {
                    return ResourceManager.Instance.GetLocalization("Completed");
                }
            case ObjectiveState.Failed:
                {
                    return ResourceManager.Instance.GetLocalization("Failed");
                }
            case ObjectiveState.Disabled:
                {
                    return ResourceManager.Instance.GetLocalization("Disabled");
                }
            default:
                {
                    return "";
                }
        }
    }

    public static float GetVolumeFromMixer(AudioMixer audioMixer, string paramName)
    {
        audioMixer.GetFloat(paramName, out float decibel);

        return DecibelToVolume(decibel);
    }

    public static float DecibelToVolume(float decibel)
    {
        if (decibel == -80.0f)
        {
            return 0.0f;
        }

        //return Mathf.Exp(volume / 20.0f);
        return Mathf.Pow(10.0f, 0.05f * decibel);
    }

    public static float VolumeToDecibel(float volume)
    {
        if (volume == 0.0f)
        {
            return -80.0f;
        }

        //return Mathf.Log(volume) * 20.0f
        return Mathf.Log10(volume) * 20.0f;
    }

    public static Sprite GetSpriteUI(Texture2D texture2D)
    {
        if (texture2D != null)
        {
            return Sprite.Create(texture2D, new Rect(0.0f, 0.0f, texture2D.width, texture2D.height), new Vector2(0.5f, 0.5f), 100.0f);
        }

        return null;
    }

    public static string UppercaseFirst(string s)
    {
        // Check for empty string.
        if (string.IsNullOrEmpty(s))
        {
            return string.Empty;
        }
        // Return char and concat substring.
        return char.ToUpper(s[0]) + s.Substring(1);
    }

    public static bool IsOnScreen(Vector3 screenPosition)
    {
        return screenPosition.x > 0 && screenPosition.x < Screen.width && screenPosition.y > 0 && screenPosition.y < Screen.height && screenPosition.z > 0;
    }

    public static Rect GetScreenRect(Bounds bounds, Camera camera)
    {
        Vector3 center = bounds.center;
        Vector3 extent = bounds.extents;

        Vector2 min = camera.WorldToScreenPoint(new Vector3(center.x - extent.x, center.y - extent.y, center.z - extent.z));
        Vector2 max = min;

        Vector2 point = min;
        min = new Vector2(min.x >= point.x ? point.x : min.x, min.y >= point.y ? point.y : min.y);
        max = new Vector2(max.x <= point.x ? point.x : max.x, max.y <= point.y ? point.y : max.y);

        point = camera.WorldToScreenPoint(new Vector3(center.x + extent.x, center.y - extent.y, center.z - extent.z));
        min = new Vector2(min.x >= point.x ? point.x : min.x, min.y >= point.y ? point.y : min.y);
        max = new Vector2(max.x <= point.x ? point.x : max.x, max.y <= point.y ? point.y : max.y);

        point = camera.WorldToScreenPoint(new Vector3(center.x - extent.x, center.y - extent.y, center.z + extent.z));
        min = new Vector2(min.x >= point.x ? point.x : min.x, min.y >= point.y ? point.y : min.y);
        max = new Vector2(max.x <= point.x ? point.x : max.x, max.y <= point.y ? point.y : max.y);

        point = camera.WorldToScreenPoint(new Vector3(center.x + extent.x, center.y - extent.y, center.z + extent.z));
        min = new Vector2(min.x >= point.x ? point.x : min.x, min.y >= point.y ? point.y : min.y);
        max = new Vector2(max.x <= point.x ? point.x : max.x, max.y <= point.y ? point.y : max.y);

        point = camera.WorldToScreenPoint(new Vector3(center.x - extent.x, center.y + extent.y, center.z - extent.z));
        min = new Vector2(min.x >= point.x ? point.x : min.x, min.y >= point.y ? point.y : min.y);
        max = new Vector2(max.x <= point.x ? point.x : max.x, max.y <= point.y ? point.y : max.y);

        point = camera.WorldToScreenPoint(new Vector3(center.x + extent.x, center.y + extent.y, center.z - extent.z));
        min = new Vector2(min.x >= point.x ? point.x : min.x, min.y >= point.y ? point.y : min.y);
        max = new Vector2(max.x <= point.x ? point.x : max.x, max.y <= point.y ? point.y : max.y);

        point = camera.WorldToScreenPoint(new Vector3(center.x - extent.x, center.y + extent.y, center.z + extent.z));
        min = new Vector2(min.x >= point.x ? point.x : min.x, min.y >= point.y ? point.y : min.y);
        max = new Vector2(max.x <= point.x ? point.x : max.x, max.y <= point.y ? point.y : max.y);

        point = camera.WorldToScreenPoint(new Vector3(center.x + extent.x, center.y + extent.y, center.z + extent.z));
        min = new Vector2(min.x >= point.x ? point.x : min.x, min.y >= point.y ? point.y : min.y);
        max = new Vector2(max.x <= point.x ? point.x : max.x, max.y <= point.y ? point.y : max.y);

        return new Rect(min.x, min.y, max.x - min.x, max.y - min.y);
    }

    public static string ResolutionToString(Resolution resolution)
    {
        return resolution.width.ToString() + " x " + resolution.height.ToString() + " " + resolution.refreshRate.ToString() + "hz";
    }

    public static string SplitLineToMultiline(string input, int rowLength)
    {
        StringBuilder result = new StringBuilder();
        StringBuilder line = new StringBuilder();

        //Stack<string> stack = new Stack<string>(input.Split(' '));
        Queue<string> queue = new Queue<string>(input.Split(' '));

        while (queue.Count > 0)
        {
            //string word = stack.Pop();
            string word = queue.Dequeue();
            if (word.Length > rowLength)
            {
                string head = word.Substring(0, rowLength);
                string tail = word.Substring(rowLength);

                word = head;
                //stack.Push(tail);
                queue.Enqueue(tail);
            }

            if (line.Length + word.Length > rowLength)
            {
                result.AppendLine(line.ToString());
                line.Length = 0;
            }

            line.Append(word + " ");
        }

        result.Append(line);
        return result.ToString();
    }

    public static string GetUnitClassName(UnitClass unitClass)
    {
        switch (unitClass)
        {
            case UnitClass.MechUltraLight:
                {
                    return ResourceManager.Instance.GetLocalization("UnitClassUltraLight");
                }
            case UnitClass.MechLight:
                {
                    return ResourceManager.Instance.GetLocalization("UnitClassLight");
                }
            case UnitClass.MechMedium:
                {
                    return ResourceManager.Instance.GetLocalization("UnitClassMedium");
                }
            case UnitClass.MechHeavy:
                {
                    return ResourceManager.Instance.GetLocalization("UnitClassHeavy");
                }
            case UnitClass.MechAssault:
                {
                    return ResourceManager.Instance.GetLocalization("UnitClassAssault");
                }
            default:
                {
                    return "Error";
                }
        }
    }

    public static string GetMechStatusName(MechStatusType mechStatusType)
    {
        switch (mechStatusType)
        {
            case MechStatusType.Ready:
                {
                    return ResourceManager.Instance.GetLocalization("Ready");
                }
            case MechStatusType.Damaged:
                {
                    return ResourceManager.Instance.GetLocalization("Damaged");
                }
            case MechStatusType.Crippled:
                {
                    return ResourceManager.Instance.GetLocalization("Crippled");
                }
            case MechStatusType.InvalidDesign:
                {
                    return ResourceManager.Instance.GetLocalization("Invalid Design");
                }
            case MechStatusType.Repairing:
                {
                    return ResourceManager.Instance.GetLocalization("Repairing");
                }
            default:
                {
                    return "Error";
                }
        }
    }

    public static Color GetMechStatusColor(MechStatusType mechStatusType)
    {
        switch (mechStatusType)
        {
            case MechStatusType.Ready:
                {
                    return new Color(0.0f, 0.75f, 0.0f);
                }
            case MechStatusType.Damaged:
                {
                    return new Color(1.0f, 0.8f, 0.2f);
                }
            case MechStatusType.Crippled:
                {
                    return Color.red;
                }
            case MechStatusType.InvalidDesign:
                {
                    return Color.red;
                }
            default:
                {
                    return Color.black;
                }
        }
    }

    public static string GetBiomeName(BiomeType biomeType)
    {
        switch (biomeType)
        {
            case BiomeType.Arctic:
                {
                    return ResourceManager.Instance.GetLocalization("Arctic");
                }
            case BiomeType.Desert:
                {
                    return ResourceManager.Instance.GetLocalization("Desert");
                }
            case BiomeType.Desert_WhiteStone:
                {
                    return ResourceManager.Instance.GetLocalization("White Stone Desert");
                }
            case BiomeType.Forest_Coniferous:
                {
                    return ResourceManager.Instance.GetLocalization("Coniferous Forest");
                }
            case BiomeType.Forest_Deciduous:
                {
                    return ResourceManager.Instance.GetLocalization("Deciduous Forest");
                }
            case BiomeType.Forest_Tropical:
                {
                    return ResourceManager.Instance.GetLocalization("Tropical Forest");
                }
            case BiomeType.Grassland:
                {
                    return ResourceManager.Instance.GetLocalization("Grassland");
                }
            case BiomeType.Islands_Coniferous:
                {
                    return ResourceManager.Instance.GetLocalization("Coniferous Islands");
                }
            case BiomeType.Islands_Deciduous:
                {
                    return ResourceManager.Instance.GetLocalization("Deciduous Islands");
                }
            case BiomeType.Islands_Tropical:
                {
                    return ResourceManager.Instance.GetLocalization("Tropical Islands");
                }
            case BiomeType.Lunar:
                {
                    return ResourceManager.Instance.GetLocalization("Lunar");
                }
            case BiomeType.Martian:
                {
                    return ResourceManager.Instance.GetLocalization("Martian");
                }
            case BiomeType.Odia:
                {
                    return ResourceManager.Instance.GetLocalization("Odia");
                }
            case BiomeType.Savanna_Deciduous:
                {
                    return ResourceManager.Instance.GetLocalization("Deciduous Savanna");
                }
            case BiomeType.Savanna_Tropical:
                {
                    return ResourceManager.Instance.GetLocalization("Tropical Savanna");
                }
            case BiomeType.Swamp:
                {
                    return ResourceManager.Instance.GetLocalization("Swamp");
                }
            case BiomeType.Tundra:
                {
                    return ResourceManager.Instance.GetLocalization("Tundra");
                }
            case BiomeType.Volcanic:
                {
                    return ResourceManager.Instance.GetLocalization("Volcanic");
                }
            case BiomeType.Test:
                {
                    return ResourceManager.Instance.GetLocalization("Test");
                }
            default:
                {
                    return "Error";
                }
        }
    }

    public static IList<T> ShuffleList<T>(IList<T> genericList)
    {
        int count = genericList.Count;

        while (count > 1)
        {
            count--;
            int index = Random.Range(0, count + 1);
            T tempValue = genericList[index];
            genericList[index] = genericList[count];
            genericList[count] = tempValue;
        }

        return genericList;
    }

    public static string GetMouseButtonName(int value)
    {
        switch(value)
        {
            case -1:
                {
                    return "None";
                }
            case 3:
                {
                    return "Button Left";
                }
            case 4:
                {
                    return "Button Left";
                }
            case 5:
                {
                    return "Button Right";
                }
            case 6:
                {
                    return "Button Middle";
                }
            default:
                {
                    return "Button " + value.ToString();
                }
        }
    }

    public static string GetMouseAxisName(MouseAxis mouseAxis)
    {
        switch (mouseAxis)
        {
            case MouseAxis.X:
                {
                    return "X Axis";
                }
            case MouseAxis.X_Inverted:
                {
                    return "Inverted X Axis";
                }
            case MouseAxis.Y:
                {
                    return "Y Axis";
                }
            case MouseAxis.Y_Inverted:
                {
                    return "Inverted Y Axis";
                }
            case MouseAxis.Scroll:
                {
                    return "Scroll";
                }
            case MouseAxis.Scroll_Inverted:
                {
                    return "Inverted Scroll";
                }
            default:
                {
                    return "None";
                }
        }
    }

    public static string GetOrdinal(int number)
    {
        if (number <= 0) return number.ToString();

        switch (number % 100)
        {
            case 11:
            case 12:
            case 13:
                return number + "th";
        }

        switch (number % 10)
        {
            case 1:
                return number + "st";
            case 2:
                return number + "nd";
            case 3:
                return number + "rd";
            default:
                return number + "th";
        }
    }

    public static string GetNavPointName(int index)
    {
        switch (index)
        {
            case 0:
                {
                    return "Alpha";
                }
            case 1:
                {
                    return "Beta";
                }
            case 2:
                {
                    return "Gamma";
                }
            case 3:
                {
                    return "Delta";
                }
            case 4:
                {
                    return "Epsilon";
                }
            case 5:
                {
                    return "Zeta";
                }
            case 6:
                {
                    return "Eta";
                }
            case 7:
                {
                    return "Theta";
                }
            case 8:
                {
                    return "Iota";
                }
            case 9:
                {
                    return "Kappa";
                }
            case 10:
                {
                    return "Lambda";
                }
            case 11:
                {
                    return "Mu";
                }
            case 12:
                {
                    return "Nu";
                }
            case 13:
                {
                    return "Xi";
                }
            case 14:
                {
                    return "Omicron";
                }
            case 15:
                {
                    return "Pi";
                }
            case 16:
                {
                    return "Rho";
                }
            case 17:
                {
                    return "Sigma";
                }
            case 18:
                {
                    return "Tau";
                }
            case 19:
                {
                    return "Upsilon";
                }
            case 20:
                {
                    return "Phi";
                }
            case 21:
                {
                    return "Chi";
                }
            case 22:
                {
                    return "Psi";
                }
            case 23:
                {
                    return "Omega";
                }
            default:
                {
                    return "Alpha";
                }
        }
    }

    public static string GetMissionTypeName(MissionType missionType)
    {
        switch (missionType)
        {
            case MissionType.Battle:
                {
                    return ResourceManager.Instance.GetLocalization("MissionTypeBattle");
                }
            case MissionType.Assassination:
                {
                    return ResourceManager.Instance.GetLocalization("MissionTypeAssassination");
                }
            case MissionType.BaseCapture:
                {
                    return ResourceManager.Instance.GetLocalization("MissionTypeBaseCapture");
                }
            case MissionType.BaseDefend:
                {
                    return ResourceManager.Instance.GetLocalization("MissionTypeBaseDefend");
                }
            case MissionType.BaseDestroy:
                {
                    return ResourceManager.Instance.GetLocalization("MissionTypeBaseDestroy");
                }
            case MissionType.BattleSupport:
                {
                    return ResourceManager.Instance.GetLocalization("MissionTypeBattleSupport");
                }
            case MissionType.ConvoyCapture:
                {
                    return ResourceManager.Instance.GetLocalization("MissionTypeConvoyCapture");
                }
            case MissionType.ConvoyDestroy:
                {
                    return ResourceManager.Instance.GetLocalization("MissionTypeConvoyDestroy");
                }
            case MissionType.ConvoyEscort:
                {
                    return ResourceManager.Instance.GetLocalization("MissionTypeConvoyEscort");
                }
            case MissionType.SearchAndDestroy:
                {
                    return ResourceManager.Instance.GetLocalization("MissionTypeSearchAndDestroy");
                }
            case MissionType.Recon:
                {
                    return ResourceManager.Instance.GetLocalization("MissionTypeRecon");
                }
            default:
                {
                    return "Warning: MissionType name not found";
                }
        }
    }

    public static string GetMissionTypeDescription(MissionType missionType)
    {
        switch (missionType)
        {
            case MissionType.Battle:
                {
                    return ResourceManager.Instance.GetLocalization("MissionTypeBattleDesc");
                }
            case MissionType.Assassination:
                {
                    return ResourceManager.Instance.GetLocalization("MissionTypeAssassinationDesc");
                }
            case MissionType.BaseCapture:
                {
                    return ResourceManager.Instance.GetLocalization("MissionTypeBaseCaptureDesc");
                }
            case MissionType.BaseDefend:
                {
                    return ResourceManager.Instance.GetLocalization("MissionTypeBaseDefendDesc");
                }
            case MissionType.BaseDestroy:
                {
                    return ResourceManager.Instance.GetLocalization("MissionTypeBaseDestroyDesc");
                }
            case MissionType.BattleSupport:
                {
                    return ResourceManager.Instance.GetLocalization("MissionTypeBattleSupportDesc");
                }
            case MissionType.ConvoyCapture:
                {
                    return ResourceManager.Instance.GetLocalization("MissionTypeConvoyCaptureDesc");
                }
            case MissionType.ConvoyDestroy:
                {
                    return ResourceManager.Instance.GetLocalization("MissionTypeConvoyDestroyDesc");
                }
            case MissionType.ConvoyEscort:
                {
                    return ResourceManager.Instance.GetLocalization("MissionTypeConvoyEscortDesc");
                }
            case MissionType.SearchAndDestroy:
                {
                    return ResourceManager.Instance.GetLocalization("MissionTypeSearchAndDestroyDesc");
                }
            case MissionType.Recon:
                {
                    return ResourceManager.Instance.GetLocalization("MissionTypeReconDesc");
                }
            default:
                {
                    return "";
                }
        }
    }

    public static string GetWeaponClassificationDisplay(WeaponClassification weaponClassification)
    {
        switch (weaponClassification)
        {
            case WeaponClassification.Laser:
                {
                    return "Laser";
                }
            case WeaponClassification.ParticleCannon:
                {
                    return "Particle Cannon";
                }
            default:
                {
                    return "";
                }
        }
    }

    public static string GetWeaponModificationTypeDisplay(WeaponModificationType weaponModificationType)
    {
        switch (weaponModificationType)
        {
            case WeaponModificationType.Damage:
                {
                    return "Damage";
                }
            case WeaponModificationType.Heat:
                {
                    return "Heat";
                }
            case WeaponModificationType.RecycleTime:
                {
                    return "Recycle Time";
                }
            case WeaponModificationType.BeamDuration:
                {
                    return "Beam Duration";
                }
            case WeaponModificationType.BeamRecharge:
                {
                    return "Beam Recharge";
                }
            default:
                {
                    return "";
                }
        }
    }

    public static string GetFiringModeDisplay(FiringMode firingMode)
    {
        switch (firingMode)
        {
            case FiringMode.Standard:
                {
                    return "STD";
                }
            case FiringMode.Chain:
                {
                    return "CHN";
                }
            default:
                {
                    return "";
                }
        }
    }

    public static List<InventoryComponentEntry> GetInventoryComponentsFilteredByType(List<InventoryComponentEntry> source, ComponentType componentType)
    {
        List<InventoryComponentEntry> filteredComponents = new List<InventoryComponentEntry>();

        foreach (InventoryComponentEntry componentEntry in source)
        {
            if (componentEntry.componentDefinition.ComponentType == componentType)
            {
                filteredComponents.Add(componentEntry);
            }
        }

        return filteredComponents;
    }

    public static List<InventoryComponentEntry> GetInventoryComponentsFilteredBySize(List<InventoryComponentEntry> source, int size)
    {
        List<InventoryComponentEntry> filteredComponents = new List<InventoryComponentEntry>();

        foreach (InventoryComponentEntry componentEntry in source)
        {
            if (componentEntry.componentDefinition.SlotSize <= size)
            {
                filteredComponents.Add(componentEntry);
            }
        }

        return filteredComponents;
    }

    public static List<InventoryComponentEntry> GetInventoryComponentsFilteredByTypeAndSize(List<InventoryComponentEntry> source, ComponentType componentType, int size)
    {
        List<InventoryComponentEntry> filteredComponents = new List<InventoryComponentEntry>();

        foreach (InventoryComponentEntry componentEntry in source)
        {
            if (componentEntry.componentDefinition.ComponentType == componentType)
            {
                if (componentEntry.componentDefinition.SlotSize <= size)
                {
                    filteredComponents.Add(componentEntry);
                }
            }
            else
            {
                filteredComponents.Add(componentEntry);
            }
        }

        return filteredComponents;
    }

    public static void SetParticleSystemsEmission(ParticleSystem[] particleSystems, bool state)
    {
        for (int i = 0; i < particleSystems.Length; i++)
        {
            ParticleSystem particleSystem = particleSystems[i];

            ParticleSystem.EmissionModule emissionModule = particleSystem.emission;
            emissionModule.enabled = state;
        }
    }

    public static T Next<T>(this T src) where T : struct
    {
        if (!typeof(T).IsEnum)
        {
            throw new System.ArgumentException(string.Format("Argument {0} is not an Enum", typeof(T).FullName));
        }

        T[] Arr = (T[])System.Enum.GetValues(src.GetType());
        int j = System.Array.IndexOf(Arr, src) + 1;

        return (Arr.Length == j) ? Arr[0] : Arr[j];
    }

    public static class JsonHelper
    {
        public static T[] FromJson<T>(string json)
        {
            Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
            return wrapper.Items;
        }

        public static string ToJson<T>(T[] array)
        {
            Wrapper<T> wrapper = new Wrapper<T>();
            wrapper.Items = array;
            return JsonUtility.ToJson(wrapper);
        }

        public static string ToJson<T>(T[] array, bool prettyPrint)
        {
            Wrapper<T> wrapper = new Wrapper<T>();
            wrapper.Items = array;
            return JsonUtility.ToJson(wrapper, prettyPrint);
        }

        [System.Serializable]
        private class Wrapper<T>
        {
            public T[] Items;
        }
    }

#if UNITY_EDITOR
    public static T GetAssetFromFile<T>(FileInfo fileInfo) where T : class
    {
        string fullPath = fileInfo.FullName.Replace(@"\", "/");
        string assetPath = "Assets" + fullPath.Replace(Application.dataPath, "");
        return AssetDatabase.LoadAssetAtPath(assetPath, typeof(T)) as T;
    }
#endif
}
