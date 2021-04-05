using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PilotVoiceProfileDefinition : Definition
{
    public string DisplayName = "";

    public string[] ConfirmOrders = new string[0];

    public string[] RejectOrders = new string[0];

    public string[] ConfirmFollowOrders = new string[0];

    public string[] ConfirmAttackOrders = new string[0];

    public string[] ConfirmMoveToNavPointOrders = new string[0];

    public string[] ConfirmHoldPositionOrders = new string[0];

    public string[] Ejections = new string[0];

    public string[] EnemyDestroyed = new string[0];

    public string[] FriendlyFireWarnings = new string[0];

    public string GetDisplayName()
    {
        return ResourceManager.Instance.GetLocalization(DisplayName);
    }

    public AudioClip GetConfirmOrder()
    {
        return GetRandomAudioClipFromArray(ConfirmOrders);
    }

    public AudioClip GetRejectOrder()
    {
        return GetRandomAudioClipFromArray(RejectOrders);
    }

    public AudioClip GetConfirmFollowOrder()
    {
        return GetRandomAudioClipFromArray(ConfirmFollowOrders, ConfirmOrders);
    }

    public AudioClip GetConfirmAttackOrder()
    {
        return GetRandomAudioClipFromArray(ConfirmAttackOrders, ConfirmOrders);
    }

    public AudioClip GetConfirmMoveToNavPointOrder()
    {
        return GetRandomAudioClipFromArray(ConfirmMoveToNavPointOrders, ConfirmOrders);
    }

    public AudioClip GetConfirmHoldPositionOrder()
    {
        return GetRandomAudioClipFromArray(ConfirmHoldPositionOrders, ConfirmOrders);
    }

    public AudioClip GetEjection()
    {
        return GetRandomAudioClipFromArray(Ejections);
    }

    public AudioClip GetEnemyDestroyed()
    {
        return GetRandomAudioClipFromArray(EnemyDestroyed);
    }

    public AudioClip GetFriendlyFireWarning()
    {
        return GetRandomAudioClipFromArray(FriendlyFireWarnings);
    }

    AudioClip GetRandomAudioClipFromArray(string[] primaryArray)
    {
        if (primaryArray.Length > 0)
        {
            return ResourceManager.Instance.GetAudioClip(primaryArray[Random.Range(0, primaryArray.Length)]);
        }

        return null;
    }

    AudioClip GetRandomAudioClipFromArray(string[] primaryArray, string[] secondayArray)
    {
        if (primaryArray.Length > 0 && (secondayArray.Length == 0 || Random.Range(0, 1f) < 0.75f))
        {
            return ResourceManager.Instance.GetAudioClip(primaryArray[Random.Range(0, primaryArray.Length)]);
        }
        else if (secondayArray.Length > 0)
        {
            return ResourceManager.Instance.GetAudioClip(secondayArray[Random.Range(0, secondayArray.Length)]);
        }

        return null;
    }
}
