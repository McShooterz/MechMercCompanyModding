using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseDamageDisplay : MonoBehaviour
{
    public static Color GetArmorHealthColor(float health)
    {
        if (health == 1f)
        {
            return ResourceManager.Instance.GameConstants.ArmorColorHealthFull;
        }
        else if (health > 0.5f)
        {
            return ResourceManager.Instance.GameConstants.GetArmorColorHealthFullToMedium((health - 0.5f) * 2f);
        }
        else if (health > 0f)
        {
            return ResourceManager.Instance.GameConstants.GetArmorColorHealthMediumToLow(health * 2f);
        }
        else
        {
            return ResourceManager.Instance.GameConstants.ArmorColorHealthDestroyed;
        }
    }

    public static Color GetInternalHealthColor(float health)
    {
        if (health == 1f)
        {
            return ResourceManager.Instance.GameConstants.InternalColorHealthFull;
        }
        else if (health > 0.5f)
        {
            return ResourceManager.Instance.GameConstants.GetInternalColorHealthFullToMedium((health - 0.5f) * 2f);
        }
        else if (health > 0f)
        {
            return ResourceManager.Instance.GameConstants.GetInternalColorHealthMediumToLow(health * 2f);
        }
        else
        {
            return ResourceManager.Instance.GameConstants.InternalColorHealthDestroyed;
        }
    }
}
