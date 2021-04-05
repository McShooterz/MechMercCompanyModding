using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class CommandSystem
{
    [SerializeField]
    AudioSource audioSourceSystems;

    [SerializeField]
    AudioSource audioSourceRadio;

    [SerializeField]
    int squadSelection = -1;

    [SerializeField]
    int squadSubSelection = -1;

    [SerializeField]
    int squadMechSelection = -1;

    [SerializeField]
    float radioClipTimer;

    [SerializeField]
    float friendlyFireWarningTimer;

    [SerializeField]
    Queue<AudioClip> audioClipsRadio = new Queue<AudioClip>();

    public Queue<AudioClip> AudioClipsRadio
    {
        get
        {
            return audioClipsRadio;
        }
    }

    public bool CanAddFriendlyFireWarning
    {
        get
        {
            return Time.time > friendlyFireWarningTimer;
        }
    }

    public CommandSystem(AudioSource system, AudioSource radio)
    {
        audioSourceSystems = system;
        audioSourceRadio = radio;
    }

    public void Update(Transform currentNavigationPoint, UnitController targetUnit)
    {
        if (squadSubSelection != -1)
        {
            if (InputManager.Instance.command1.PressedThisFrame())
            {
                switch (squadSubSelection)
                {
                    case 0:
                        {
                            if (!MissionManager.Instance.SqaudCommandUnits[0].IsDestroyed)
                            {
                                ClearCommandSelection();
                                squadMechSelection = System.Array.IndexOf(MissionManager.Instance.SquadMateUnits, MissionManager.Instance.SqaudCommandUnits[0]);
                                PlayerHUD.Instance.SquadInfoGroup.HighlightSelectedIndividual(squadMechSelection);
                                PlayRadioClick();
                                SetOrdersInstructions();
                            }
                            break;
                        }
                    case 1:
                        {
                            if (!MissionManager.Instance.SqaudSecondaryUnits[0].IsDestroyed)
                            {
                                ClearCommandSelection();
                                squadMechSelection = System.Array.IndexOf(MissionManager.Instance.SquadMateUnits, MissionManager.Instance.SqaudSecondaryUnits[0]);
                                PlayerHUD.Instance.SquadInfoGroup.HighlightSelectedIndividual(squadMechSelection);
                                PlayRadioClick();
                                SetOrdersInstructions();
                            }
                            break;
                        }
                    case 2:
                        {
                            if (!MissionManager.Instance.SqaudTertiaryUnits[0].IsDestroyed)
                            {
                                ClearCommandSelection();
                                squadMechSelection = System.Array.IndexOf(MissionManager.Instance.SquadMateUnits, MissionManager.Instance.SqaudTertiaryUnits[0]);
                                PlayerHUD.Instance.SquadInfoGroup.HighlightSelectedIndividual(squadMechSelection);
                                PlayRadioClick();
                                SetOrdersInstructions();
                            }
                            break;
                        }
                }
            }
            else if (InputManager.Instance.command2.PressedThisFrame())
            {
                switch (squadSubSelection)
                {
                    case 0:
                        {
                            if (MissionManager.Instance.SqaudCommandUnits.Length > 1 && !MissionManager.Instance.SqaudCommandUnits[1].IsDestroyed)
                            {
                                ClearCommandSelection();
                                squadMechSelection = System.Array.IndexOf(MissionManager.Instance.SquadMateUnits, MissionManager.Instance.SqaudCommandUnits[1]);
                                PlayerHUD.Instance.SquadInfoGroup.HighlightSelectedIndividual(squadMechSelection);
                                PlayRadioClick();
                                SetOrdersInstructions();
                            }
                            break;
                        }
                    case 1:
                        {
                            if (MissionManager.Instance.SqaudSecondaryUnits.Length > 1 && !MissionManager.Instance.SqaudSecondaryUnits[1].IsDestroyed)
                            {
                                ClearCommandSelection();
                                squadMechSelection = System.Array.IndexOf(MissionManager.Instance.SquadMateUnits, MissionManager.Instance.SqaudSecondaryUnits[1]);
                                PlayerHUD.Instance.SquadInfoGroup.HighlightSelectedIndividual(squadMechSelection);
                                PlayRadioClick();
                                SetOrdersInstructions();
                            }
                            break;
                        }
                    case 2:
                        {
                            if (MissionManager.Instance.SqaudTertiaryUnits.Length > 1 && !MissionManager.Instance.SqaudTertiaryUnits[1].IsDestroyed)
                            {
                                ClearCommandSelection();
                                squadMechSelection = System.Array.IndexOf(MissionManager.Instance.SquadMateUnits, MissionManager.Instance.SqaudTertiaryUnits[1]);
                                PlayerHUD.Instance.SquadInfoGroup.HighlightSelectedIndividual(squadMechSelection);
                                PlayRadioClick();
                                SetOrdersInstructions();
                            }
                            break;
                        }
                }
            }
            else if (InputManager.Instance.command3.PressedThisFrame())
            {
                switch (squadSubSelection)
                {
                    case 0:
                        {
                            if (MissionManager.Instance.SqaudCommandUnits.Length > 2 && !MissionManager.Instance.SqaudCommandUnits[2].IsDestroyed)
                            {
                                ClearCommandSelection();
                                squadMechSelection = System.Array.IndexOf(MissionManager.Instance.SquadMateUnits, MissionManager.Instance.SqaudCommandUnits[2]);
                                PlayerHUD.Instance.SquadInfoGroup.HighlightSelectedIndividual(squadMechSelection);
                                PlayRadioClick();
                                SetOrdersInstructions();
                            }
                            break;
                        }
                    case 1:
                        {
                            if (MissionManager.Instance.SqaudSecondaryUnits.Length > 2 && !MissionManager.Instance.SqaudSecondaryUnits[2].IsDestroyed)
                            {
                                ClearCommandSelection();
                                squadMechSelection = System.Array.IndexOf(MissionManager.Instance.SquadMateUnits, MissionManager.Instance.SqaudSecondaryUnits[2]);
                                PlayerHUD.Instance.SquadInfoGroup.HighlightSelectedIndividual(squadMechSelection);
                                PlayRadioClick();
                                SetOrdersInstructions();
                            }
                            break;
                        }
                    case 2:
                        {
                            if (MissionManager.Instance.SqaudTertiaryUnits.Length > 2 && !MissionManager.Instance.SqaudTertiaryUnits[2].IsDestroyed)
                            {
                                ClearCommandSelection();
                                squadMechSelection = System.Array.IndexOf(MissionManager.Instance.SquadMateUnits, MissionManager.Instance.SqaudTertiaryUnits[2]);
                                PlayerHUD.Instance.SquadInfoGroup.HighlightSelectedIndividual(squadMechSelection);
                                PlayRadioClick();
                                SetOrdersInstructions();
                            }
                            break;
                        }
                }
            }
            else if (InputManager.Instance.command4.PressedThisFrame())
            {
                switch (squadSubSelection)
                {
                    case 1:
                        {
                            if (MissionManager.Instance.SqaudSecondaryUnits.Length > 3 && !MissionManager.Instance.SqaudSecondaryUnits[3].IsDestroyed)
                            {
                                ClearCommandSelection();
                                squadMechSelection = System.Array.IndexOf(MissionManager.Instance.SquadMateUnits, MissionManager.Instance.SqaudSecondaryUnits[3]);
                                PlayerHUD.Instance.SquadInfoGroup.HighlightSelectedIndividual(squadMechSelection);
                                PlayRadioClick();
                                SetOrdersInstructions();
                            }
                            break;
                        }
                    case 2:
                        {
                            if (MissionManager.Instance.SqaudTertiaryUnits.Length > 3 && !MissionManager.Instance.SqaudTertiaryUnits[3].IsDestroyed)
                            {
                                ClearCommandSelection();
                                squadMechSelection = System.Array.IndexOf(MissionManager.Instance.SquadMateUnits, MissionManager.Instance.SqaudTertiaryUnits[3]);
                                PlayerHUD.Instance.SquadInfoGroup.HighlightSelectedIndividual(squadMechSelection);
                                PlayRadioClick();
                                SetOrdersInstructions();
                            }
                            break;
                        }
                }
            }
        }
        else if (squadSelection != -1)
        {
            if (InputManager.Instance.command1.PressedThisFrame())
            {
                switch (squadSelection)
                {
                    case 0:
                        {
                            CommandFollow(MissionManager.Instance.SqaudCommandUnits);
                            break;
                        }
                    case 1:
                        {
                            CommandFollow(MissionManager.Instance.SqaudSecondaryUnits);
                            break;
                        }
                    case 2:
                        {
                            CommandFollow(MissionManager.Instance.SqaudTertiaryUnits);
                            break;
                        }
                    default:
                        {
                            CommandFollow(MissionManager.Instance.SquadMateUnits);
                            break;
                        }
                }
            }
            else if (InputManager.Instance.command2.PressedThisFrame())
            {
                switch (squadSelection)
                {
                    case 0:
                        {
                            CommandAttackTarget(MissionManager.Instance.SqaudCommandUnits, targetUnit);
                            break;
                        }
                    case 1:
                        {
                            CommandAttackTarget(MissionManager.Instance.SqaudSecondaryUnits, targetUnit);
                            break;
                        }
                    case 2:
                        {
                            CommandAttackTarget(MissionManager.Instance.SqaudTertiaryUnits, targetUnit);
                            break;
                        }
                    default:
                        {
                            CommandAttackTarget(MissionManager.Instance.SquadMateUnits, targetUnit);
                            break;
                        }
                }
            }
            else if (InputManager.Instance.command3.PressedThisFrame())
            {
                switch (squadSelection)
                {
                    case 0:
                        {
                            CommandAttackClosestTarget(MissionManager.Instance.SqaudCommandUnits);
                            break;
                        }
                    case 1:
                        {
                            CommandAttackClosestTarget(MissionManager.Instance.SqaudSecondaryUnits);
                            break;
                        }
                    case 2:
                        {
                            CommandAttackClosestTarget(MissionManager.Instance.SqaudTertiaryUnits);
                            break;
                        }
                    default:
                        {
                            CommandAttackClosestTarget(MissionManager.Instance.SquadMateUnits);
                            break;
                        }
                }
            }
            else if (InputManager.Instance.command4.PressedThisFrame())
            {
                switch (squadSelection)
                {
                    case 0:
                        {
                            CommandMoveToNavPoint(MissionManager.Instance.SqaudCommandUnits, currentNavigationPoint);
                            break;
                        }
                    case 1:
                        {
                            CommandMoveToNavPoint(MissionManager.Instance.SqaudSecondaryUnits, currentNavigationPoint);
                            break;
                        }
                    case 2:
                        {
                            CommandMoveToNavPoint(MissionManager.Instance.SqaudTertiaryUnits, currentNavigationPoint);
                            break;
                        }
                    default:
                        {
                            CommandMoveToNavPoint(MissionManager.Instance.SquadMateUnits, currentNavigationPoint);
                            break;
                        }
                }
            }
            else if (InputManager.Instance.command5.PressedThisFrame())
            {
                switch (squadSelection)
                {
                    case 0:
                        {
                            CommandHoldPosition(MissionManager.Instance.SqaudCommandUnits);
                            break;
                        }
                    case 1:
                        {
                            CommandHoldPosition(MissionManager.Instance.SqaudSecondaryUnits);
                            break;
                        }
                    case 2:
                        {
                            CommandHoldPosition(MissionManager.Instance.SqaudTertiaryUnits);
                            break;
                        }
                    default:
                        {
                            CommandHoldPosition(MissionManager.Instance.SquadMateUnits);
                            break;
                        }
                }
            }
            else if (InputManager.Instance.command6.PressedThisFrame())
            {
                ClearCommandSelection();
            }
        }
        else if (squadMechSelection != -1)
        {
            if (InputManager.Instance.command1.PressedThisFrame())
            {
                CommandFollow(new MechController[] { MissionManager.Instance.SquadMateUnits[squadMechSelection] });
            }
            else if (InputManager.Instance.command2.PressedThisFrame())
            {
                CommandAttackTarget(new MechController[] { MissionManager.Instance.SquadMateUnits[squadMechSelection] }, targetUnit);
            }
            else if (InputManager.Instance.command3.PressedThisFrame())
            {
                CommandAttackClosestTarget(new MechController[] { MissionManager.Instance.SquadMateUnits[squadMechSelection] });
            }
            else if (InputManager.Instance.command4.PressedThisFrame())
            {
                CommandMoveToNavPoint(new MechController[] { MissionManager.Instance.SquadMateUnits[squadMechSelection] }, currentNavigationPoint);
            }
            else if (InputManager.Instance.command5.PressedThisFrame())
            {
                CommandHoldPosition(new MechController[] { MissionManager.Instance.SquadMateUnits[squadMechSelection] });
            }
            else if (InputManager.Instance.command6.PressedThisFrame())
            {
                ClearCommandSelection();
            }
        }
        else
        {
            if (InputManager.Instance.command1.PressedThisFrame() && MissionManager.Instance.SquadMateUnits.Any(unit => !unit.IsDestroyed))
            {
                squadSelection = 3;
                PlayerHUD.Instance.SquadInfoGroup.HightlightAllSquads(MissionManager.Instance.SqaudCommandUnits.Any(unit => !unit.IsDestroyed), MissionManager.Instance.SqaudSecondaryUnits.Any(unit => !unit.IsDestroyed), MissionManager.Instance.SqaudTertiaryUnits.Any(unit => !unit.IsDestroyed));
                PlayRadioClick();
                SetOrdersInstructions();
            }
            else if (InputManager.Instance.command2.PressedThisFrame() && MissionManager.Instance.SqaudCommandUnits.Any(unit => !unit.IsDestroyed))
            {
                squadSelection = 0;
                PlayerHUD.Instance.SquadInfoGroup.HighlightSelectionSquad(squadSelection);
                PlayRadioClick();
                SetOrdersInstructions();
            }
            else if (InputManager.Instance.command3.PressedThisFrame() && MissionManager.Instance.SqaudSecondaryUnits.Any(unit => !unit.IsDestroyed))
            {
                squadSelection = 1;
                PlayerHUD.Instance.SquadInfoGroup.HighlightSelectionSquad(squadSelection);
                PlayRadioClick();
                SetOrdersInstructions();
            }
            else if (InputManager.Instance.command4.PressedThisFrame() && MissionManager.Instance.SqaudTertiaryUnits.Any(unit => !unit.IsDestroyed))
            {
                squadSelection = 2;
                PlayerHUD.Instance.SquadInfoGroup.HighlightSelectionSquad(squadSelection);
                PlayRadioClick();
                SetOrdersInstructions();
            }
            else if (InputManager.Instance.command5.PressedThisFrame() && MissionManager.Instance.SqaudCommandUnits.Length > 0 && MissionManager.Instance.SqaudCommandUnits.Any(unit => !unit.IsDestroyed))
            {
                squadSubSelection = 0;
                PlayerHUD.Instance.SquadInfoGroup.HightlightSubSelectionSquad(squadSubSelection);
                PlayRadioClick();
                SetSubSelectionInstructions();
            }
            else if (InputManager.Instance.command6.PressedThisFrame() && MissionManager.Instance.SqaudSecondaryUnits.Length > 0 && MissionManager.Instance.SqaudSecondaryUnits.Any(unit => !unit.IsDestroyed))
            {
                squadSubSelection = 1;
                PlayerHUD.Instance.SquadInfoGroup.HightlightSubSelectionSquad(squadSubSelection);
                PlayRadioClick();
                SetSubSelectionInstructions();
            }
            else if (InputManager.Instance.command7.PressedThisFrame() && MissionManager.Instance.SqaudTertiaryUnits.Length > 0 && MissionManager.Instance.SqaudTertiaryUnits.Any(unit => !unit.IsDestroyed))
            {
                squadSubSelection = 2;
                PlayerHUD.Instance.SquadInfoGroup.HightlightSubSelectionSquad(squadSubSelection);
                PlayRadioClick();
                SetSubSelectionInstructions();
            }
        }

        if (audioClipsRadio.Count > 0)
        {
            if (!audioSourceRadio.isPlaying && Time.time > radioClipTimer)
            {
                AudioClip audioClip = audioClipsRadio.Dequeue();

                audioSourceRadio.PlayOneShot(audioClip);

                radioClipTimer = Time.time + audioClip.length + 0.25f;
            }
        }
    }

    void CommandFollow(MechController[] mechControllers)
    {
        AudioClip audioClip = null;

        foreach (MechController mechController in mechControllers)
        {
            if (mechController.IsDestroyed)
                continue;

            mechController.SetSquadMateOrder(null);

            if (audioClip == null)
            {
                audioClip = mechController.MechData.currentMechPilot.PilotVoiceProfile.GetConfirmFollowOrder();

                if (audioClip != null)
                {
                    audioClipsRadio.Enqueue(audioClip);
                }
            }
        }

        ClearCommandSelection();
    }

    void CommandAttackTarget(MechController[] mechControllers, UnitController targetUnit)
    {
        AudioClip audioClip = null;

        if (targetUnit != null && MissionManager.Instance.EnemyUnits.Contains(targetUnit))
        {
            foreach (MechController mechController in mechControllers)
            {
                if (mechController.IsDestroyed)
                    continue;

                mechController.SetSquadMateOrder(new OrderAttackTarget(targetUnit));

                if (audioClip == null)
                {
                    audioClip = mechController.MechData.currentMechPilot.PilotVoiceProfile.GetConfirmAttackOrder();

                    if (audioClip != null)
                    {
                        audioClipsRadio.Enqueue(audioClip);
                    }
                }
            }
        }
        else
        {
            foreach (MechController mechController in mechControllers)
            {
                if (audioClip == null)
                {
                    audioClip = mechController.MechData.currentMechPilot.PilotVoiceProfile.GetRejectOrder();

                    if (audioClip != null)
                    {
                        audioClipsRadio.Enqueue(audioClip);
                    }
                }
            }
        }

        ClearCommandSelection();
    }

    void CommandAttackClosestTarget(MechController[] mechControllers)
    {
        AudioClip audioClip = null;

        foreach (MechController mechController in mechControllers)
        {
            if (mechController.IsDestroyed)
                continue;

            mechController.SetSquadMateOrder(new OrderClosestEnemy());

            if (audioClip == null)
            {
                audioClip = mechController.MechData.currentMechPilot.PilotVoiceProfile.GetConfirmOrder();

                if (audioClip != null)
                {
                    audioClipsRadio.Enqueue(audioClip);
                }
            }
        }

        ClearCommandSelection();
    }

    void CommandMoveToNavPoint(MechController[] mechControllers, Transform currentNavigationPoint)
    {
        AudioClip audioClip = null;

        foreach (MechController mechController in mechControllers)
        {
            if (mechController.IsDestroyed)
                continue;

            mechController.SetSquadMateOrder(new OrderMoveToNavPoint(currentNavigationPoint));

            if (audioClip == null)
            {
                audioClip = mechController.MechData.currentMechPilot.PilotVoiceProfile.GetConfirmMoveToNavPointOrder();

                if (audioClip != null)
                {
                    audioClipsRadio.Enqueue(audioClip);
                }
            }
        }

        ClearCommandSelection();
    }

    void CommandHoldPosition(MechController[] mechControllers)
    {
        AudioClip audioClip = null;

        foreach (MechController mechController in mechControllers)
        {
            if (mechController.IsDestroyed)
                continue;

            (mechController as MechController).SetSquadMateOrder(new OrderHoldPosition());

            if (audioClip == null)
            {
                audioClip = mechController.MechData.currentMechPilot.PilotVoiceProfile.GetConfirmHoldPositionOrder();

                if (audioClip != null)
                {
                    audioClipsRadio.Enqueue(audioClip);
                }
            }
        }

        ClearCommandSelection();
    }

    void ClearCommandSelection()
    {
        squadSelection = -1;
        squadMechSelection = -1;
        squadSubSelection = -1;

        PlayerHUD.Instance.SquadInfoGroup.ClearHighlights();

        PlayRadioClick();

        SetSelectionInstructions();
    }

    void PlayRadioClick()
    {
        AudioManager.Instance.PlayClip(audioSourceSystems, ResourceManager.Instance.GetAudioClip("RadioClick"), false, false);
    }

    public void UpdateInstructions()
    {
        if (squadSubSelection != -1 || squadMechSelection != -1)
        {
            SetOrdersInstructions();
        }
        else if (squadSubSelection != -1)
        {
            SetSubSelectionInstructions();
        }
        else
        {
            SetSelectionInstructions();
        }
    }

    void SetSelectionInstructions()
    {
        if (MissionManager.Instance.SquadMateUnits.Length > 0 && MissionManager.Instance.SquadMateUnits.Any(unit => !unit.IsDestroyed))
        {
            System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();

            bool commandSquadSelectable = MissionManager.Instance.SqaudCommandUnits.Length > 0 && MissionManager.Instance.SqaudCommandUnits.Any(unit => !unit.IsDestroyed);
            bool secondarySquadSelectable = MissionManager.Instance.SqaudSecondaryUnits.Length > 0 && MissionManager.Instance.SqaudSecondaryUnits.Any(unit => !unit.IsDestroyed);
            bool tertiarySquadSelectable = MissionManager.Instance.SqaudTertiaryUnits.Length > 0 && MissionManager.Instance.SqaudTertiaryUnits.Any(unit => !unit.IsDestroyed);

            if (InputManager.Instance.command1.key != UnityEngine.InputSystem.Key.None)
                stringBuilder.AppendLine(InputManager.Instance.command1.key.ToString() + " - Select All");

            if (commandSquadSelectable && InputManager.Instance.command2.key != UnityEngine.InputSystem.Key.None)
            {
                stringBuilder.AppendLine(InputManager.Instance.command2.key.ToString() + " - Select Command Squad");
            }

            if (secondarySquadSelectable && InputManager.Instance.command3.key != UnityEngine.InputSystem.Key.None)
            {
                stringBuilder.AppendLine(InputManager.Instance.command3.key.ToString() + " - Select Secondary Squad");
            }

            if (tertiarySquadSelectable && InputManager.Instance.command4.key != UnityEngine.InputSystem.Key.None)
            {
                stringBuilder.AppendLine(InputManager.Instance.command4.key.ToString() + " - Select Tertiary Squad");
            }

            if (commandSquadSelectable && InputManager.Instance.command5.key != UnityEngine.InputSystem.Key.None)
            {
                stringBuilder.AppendLine(InputManager.Instance.command5.key.ToString() + " - Select Member of Command Squad");
            }

            if (secondarySquadSelectable && InputManager.Instance.command6.key != UnityEngine.InputSystem.Key.None)
            {
                stringBuilder.AppendLine(InputManager.Instance.command6.key.ToString() + " - Select Member of Seconday Squad");
            }

            if (tertiarySquadSelectable && InputManager.Instance.command7.key != UnityEngine.InputSystem.Key.None)
            {
                stringBuilder.Append(InputManager.Instance.command7.key.ToString() + " - Select Member of Tertiary Squad");
            }

            PlayerHUD.Instance.SquadInfoGroup.InstructionText.text = stringBuilder.ToString().TrimEnd('\r', '\n');
        }
        else
        {
            PlayerHUD.Instance.SquadInfoGroup.InstructionText.text = "";
        }
    }

    void SetSubSelectionInstructions()
    {
        System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();

        switch (squadSubSelection)
        {
            case 0:
                {
                    if (!MissionManager.Instance.SqaudCommandUnits[0].IsDestroyed)
                    {
                        if (InputManager.Instance.command1.key != UnityEngine.InputSystem.Key.None)
                            stringBuilder.AppendLine(InputManager.Instance.command1.key.ToString() + " - Select 1st Pilot");
                    }

                    if (MissionManager.Instance.SqaudCommandUnits.Length > 1 && !MissionManager.Instance.SqaudCommandUnits[1].IsDestroyed)
                    {
                        if (InputManager.Instance.command2.key != UnityEngine.InputSystem.Key.None)
                            stringBuilder.AppendLine(InputManager.Instance.command2.key.ToString() + " - Select 2nd Pilot");
                    }

                    if (MissionManager.Instance.SqaudCommandUnits.Length > 2 && !MissionManager.Instance.SqaudCommandUnits[2].IsDestroyed)
                    {
                        if (InputManager.Instance.command3.key != UnityEngine.InputSystem.Key.None)
                            stringBuilder.Append(InputManager.Instance.command3.key.ToString() + " - Select 3rd Pilot");
                    }

                    break;
                }
            case 1:
                {
                    if (!MissionManager.Instance.SqaudSecondaryUnits[0].IsDestroyed)
                    {
                        if (InputManager.Instance.command1.key != UnityEngine.InputSystem.Key.None)
                            stringBuilder.AppendLine(InputManager.Instance.command1.key.ToString() + " - Select 1st Pilot");
                    }

                    if (MissionManager.Instance.SqaudSecondaryUnits.Length > 1 && !MissionManager.Instance.SqaudSecondaryUnits[1].IsDestroyed)
                    {
                        if (InputManager.Instance.command2.key != UnityEngine.InputSystem.Key.None)
                            stringBuilder.AppendLine(InputManager.Instance.command2.key.ToString() + " - Select 2nd Pilot");
                    }

                    if (MissionManager.Instance.SqaudSecondaryUnits.Length > 2 && !MissionManager.Instance.SqaudSecondaryUnits[2].IsDestroyed)
                    {
                        if (InputManager.Instance.command3.key != UnityEngine.InputSystem.Key.None)
                            stringBuilder.AppendLine(InputManager.Instance.command3.key.ToString() + " - Select 3rd Pilot");
                    }

                    if (MissionManager.Instance.SqaudSecondaryUnits.Length > 3 && !MissionManager.Instance.SqaudSecondaryUnits[3].IsDestroyed)
                    {
                        if (InputManager.Instance.command4.key != UnityEngine.InputSystem.Key.None)
                            stringBuilder.Append(InputManager.Instance.command4.key.ToString() + " - Select 4th Pilot");
                    }

                    break;
                }
            case 2:
                {
                    if (!MissionManager.Instance.SqaudTertiaryUnits[0].IsDestroyed)
                    {
                        if (InputManager.Instance.command1.key != UnityEngine.InputSystem.Key.None)
                            stringBuilder.AppendLine(InputManager.Instance.command1.key.ToString() + " - Select 1st Pilot");
                    }

                    if (MissionManager.Instance.SqaudTertiaryUnits.Length > 1 && !MissionManager.Instance.SqaudTertiaryUnits[1].IsDestroyed)
                    {
                        if (InputManager.Instance.command2.key != UnityEngine.InputSystem.Key.None)
                            stringBuilder.AppendLine(InputManager.Instance.command2.key.ToString() + " - Select 2nd Pilot");
                    }

                    if (MissionManager.Instance.SqaudTertiaryUnits.Length > 2 && !MissionManager.Instance.SqaudTertiaryUnits[2].IsDestroyed)
                    {
                        if (InputManager.Instance.command3.key != UnityEngine.InputSystem.Key.None)
                            stringBuilder.AppendLine(InputManager.Instance.command3.key.ToString() + " - Select 3rd Pilot");
                    }

                    if (MissionManager.Instance.SqaudTertiaryUnits.Length > 3 && !MissionManager.Instance.SqaudTertiaryUnits[3].IsDestroyed)
                    {
                        if (InputManager.Instance.command4.key != UnityEngine.InputSystem.Key.None)
                            stringBuilder.Append(InputManager.Instance.command4.key.ToString() + " - Select 4th Pilot");
                    }

                    break;
                }
        }

        PlayerHUD.Instance.SquadInfoGroup.InstructionText.text = stringBuilder.ToString().TrimEnd('\r', '\n');
    }

    void SetOrdersInstructions()
    {
        System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();

        if (InputManager.Instance.command1.key != UnityEngine.InputSystem.Key.None)
            stringBuilder.AppendLine(InputManager.Instance.command1.key.ToString() + " - Follow");
        if (InputManager.Instance.command2.key != UnityEngine.InputSystem.Key.None)
            stringBuilder.AppendLine(InputManager.Instance.command2.key.ToString() + " - Attack Target");
        if (InputManager.Instance.command3.key != UnityEngine.InputSystem.Key.None)
            stringBuilder.AppendLine(InputManager.Instance.command3.key.ToString() + " - Attack Closest Enemies");
        if (InputManager.Instance.command4.key != UnityEngine.InputSystem.Key.None)
            stringBuilder.AppendLine(InputManager.Instance.command4.key.ToString() + " - Move to Nav Point");
        if (InputManager.Instance.command5.key != UnityEngine.InputSystem.Key.None)
            stringBuilder.AppendLine(InputManager.Instance.command5.key.ToString() + " - Hold Position");
        if (InputManager.Instance.command6.key != UnityEngine.InputSystem.Key.None)
            stringBuilder.Append(InputManager.Instance.command6.key.ToString() + " - Cancel");

        PlayerHUD.Instance.SquadInfoGroup.InstructionText.text = stringBuilder.ToString().TrimEnd('\r', '\n');
    }

    public void AddFriendFireWarning(AudioClip audioClip)
    {
        if (audioClip != null)
        {
            audioClipsRadio.Enqueue(audioClip);
        }

        friendlyFireWarningTimer = Time.time + 5.0f;
    }
}
