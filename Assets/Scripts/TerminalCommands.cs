using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CommandTerminal
{
    public static class TerminalCommands
    {
        static FreeCameraController freeCameraController;
        static Vector3 originalCameraPosition;
        static Quaternion originalCameraRotation;
        static Transform originalParent;

        [RegisterCommand(Help = "Go to the main menu", MinArgCount = 0, MaxArgCount = 0)]
        public static void CommandMainMenu(CommandArg[] args)
        {
            LoadingScreen.Instance.LoadScene("MainMenu");
        }

        [RegisterCommand(Help = "Exits the mission", MinArgCount = 0, MaxArgCount = 0)]
        public static void CommandExitMission(CommandArg[] args)
        {
            LoadingScreen.Instance.LoadScene(GlobalDataManager.Instance.backSceneName);
        }

        [RegisterCommand(Help = "The player no longer takes damage", MinArgCount = 0, MaxArgCount = 0)]
        public static void CommandGodMode(CommandArg[] args)
        {
            Cheats.godMode = !Cheats.godMode;

            if (Cheats.godMode)
            {
                Terminal.Log("God Mode enabled");
            }
            else
            {
                Terminal.Log("God Mode disabled");
            }
        }

        [RegisterCommand(Help = "The player no longer has heat", MinArgCount = 0, MaxArgCount = 0)]
        public static void CommandNoHeat(CommandArg[] args)
        {
            Cheats.noHeat = !Cheats.noHeat;

            if (Cheats.noHeat)
            {
                Terminal.Log("No Heat enabled");
            }
            else
            {
                Terminal.Log("No Heat disabled");
            }
        }

        [RegisterCommand(Help = "The player's weapons no longer use ammo", MinArgCount = 0, MaxArgCount = 0)]
        public static void CommandUnlimitedAmmo(CommandArg[] args)
        {
            Cheats.unlimitedAmmo = !Cheats.unlimitedAmmo;

            if (Cheats.unlimitedAmmo)
            {
                Terminal.Log("Unlimited Ammo enabled");
            }
            else
            {
                Terminal.Log("Unlimited Ammo disabled");
            }
        }

        [RegisterCommand(Help = "Destroys the player's mech's head", MinArgCount = 0, MaxArgCount = 0)]
        public static void CommandSuicide(CommandArg[] args)
        {
            if (MechControllerPlayer.Instance != null)
            {
                MechControllerPlayer.Instance.MechData.DestroyHead();
            }
            else
            {
                Terminal.Log("Warning: No player mech");
            }
        }

        [RegisterCommand(Help = "Switches between in game and free camera", MinArgCount = 0, MaxArgCount = 0)]
        public static void CommandToggleFreeCamera(CommandArg[] args)
        {
            if (CameraController.Instance == null)
            {
                Terminal.Log("Warning, no player camera to enable free camera mode.");
                return;
            }

            if (freeCameraController == null)
            {
                freeCameraController = CameraController.Instance.gameObject.AddComponent<FreeCameraController>();
                freeCameraController.enabled = false;
            }

            if (!freeCameraController.enabled)
            {
                freeCameraController.enabled = true;

                PlayerHUD.Instance.SetHudVisibility(false);
                MechControllerPlayer.Instance.SetPlayerControl(false);
                var mech = MechControllerPlayer.Instance;
                mech?.SetCockpitVisibility(false);

                originalCameraPosition = freeCameraController.transform.localPosition;
                originalCameraRotation = freeCameraController.transform.localRotation;
                originalParent = freeCameraController.transform.parent;
                freeCameraController.transform.SetParent(null);

                Terminal.Log("Switching to free camera");
            }
            else
            {
                freeCameraController.enabled = false;

                PlayerHUD.Instance.SetHudVisibility(true);
                MechControllerPlayer.Instance.SetPlayerControl(true);
                var mech = MechControllerPlayer.Instance;
                mech?.SetCockpitVisibility(true);

                freeCameraController.transform.SetParent(originalParent);
                freeCameraController.transform.localPosition = originalCameraPosition;
                freeCameraController.transform.localRotation = originalCameraRotation;

                Terminal.Log("Switching to game camera");
            }
        }

        [RegisterCommand(Help = "Adds funds to current career", MinArgCount = 0, MaxArgCount = 1)]
        public static void CommandAddFunds(CommandArg[] args)
        {
            int funds = 500000;

            if (args.Length > 0)
            {
                int value = args[0].Int;

                if (value != 0)
                {
                    funds = value;
                }
            }

            Career career = GlobalDataManager.Instance.currentCareer;

            if (career != null && career.IsReal)
            {
                Terminal.Log("Adding " + StaticHelper.FormatMoney(funds) + " to funds");
                career.funds += funds;
            }
            else
            {
                Terminal.Log("Warning: No current career to apply funds.");
            }
        }

        [RegisterCommand(Help = "Adds reputation to current career", MinArgCount = 0, MaxArgCount = 1)]
        public static void CommandAddReputation(CommandArg[] args)
        {
            int reputation = 10;

            if (args.Length > 0)
            {
                int value = args[0].Int;

                if (value != 0)
                {
                    reputation = value;
                }
            }

            Career career = GlobalDataManager.Instance.currentCareer;

            if (career != null && career.IsReal)
            {
                Terminal.Log("Adding " + reputation.ToString() + " reputation");
                career.reputation += reputation;
            }
            else
            {
                Terminal.Log("Warning: No current career to apply reputation.");
            }
        }

        [RegisterCommand(Help = "Adds infamy to current career", MinArgCount = 0, MaxArgCount = 1)]
        public static void CommandAddInfamy(CommandArg[] args)
        {
            int infamy = 10;

            if (args.Length > 0)
            {
                int value = args[0].Int;

                if (value != 0)
                {
                    infamy = value;
                }
            }

            Career career = GlobalDataManager.Instance.currentCareer;

            if (career != null && career.IsReal)
            {
                Terminal.Log("Adding " + infamy.ToString() + " infamy");
                career.infamy += infamy;
            }
            else
            {
                Terminal.Log("Warning: No current career to apply infamy.");
            }
        }
    }
}
