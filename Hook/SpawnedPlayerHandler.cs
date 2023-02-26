using GGD_Hack.Features;
using Handlers.GameHandlers.PlayerHandlers;
using HarmonyLib;

namespace GGD_Hack.Hook
{
    public class SpawnedPlayerHandler_
    {

        [HarmonyPatch(typeof(SpawnedPlayerHandler), nameof(SpawnedPlayerHandler.UpdatePlayerProperties))]
        class UpdatePlayerProperties_
        {
            static bool Prefix(SpawnedPlayerHandler __instance, PlayerProperties __0)
            {
                /*
                 * public enum LobbySceneHandler.IAJMBLJCBIK // TypeDefIndex: 3815
                {
	                // Fields
	                public int value__; // 0x0
	                public const LobbySceneHandler.IAJMBLJCBIK NotReady = 0;
	                public const LobbySceneHandler.IAJMBLJCBIK Ready = 1;
	                public const LobbySceneHandler.IAJMBLJCBIK Spectator = 2;
                }
                 */

                //跳过非本地玩家
                /*
                if (!__instance.playerController.isLocal)
                {
                    return true;
                }


                if (__0.hat == "empty_Hats" &&
                        __0.pet == "empty_Pets" &&
                        __0.clothes == "empty_Clothes" &&
                        __0.banner == "empty_Banners" &&
                        __0.card == "empty_Cards" &&
                        __0.stinger == "empty_Stingers" &&
                        __0.fart == "empty_Farts"
                        )
                {

                    return false;
                }

                return true;
                */

                //禁止将装扮数据更新为空

                //if (UnlockAllItems.Enabled.Value)
                //{

                //有bug
                PlayerController playerController = __instance.playerController;

                if (!isEmpty(playerController.bannerId) && isEmpty(__0.banner))
                {
                    __0.banner = playerController.bannerId;
                }
                if (!isEmpty(playerController.cardId) && isEmpty(__0.card))
                {
                    __0.card = playerController.cardId;
                }
                if (!isEmpty(playerController.clothesId) && isEmpty(__0.clothes))
                {
                    __0.clothes = playerController.clothesId;
                }
                if (!isEmpty(playerController.hatId) && isEmpty(__0.hat))
                {
                    __0.hat = playerController.hatId;
                }
                if (!isEmpty(playerController.petId) && isEmpty(__0.pet))
                {
                    __0.pet = playerController.petId;
                }
                if (!isEmpty(playerController.stingerId) && isEmpty(__0.stinger))
                {
                    __0.stinger = playerController.stingerId;
                }

                //}

                return true;
            }

            static bool isEmpty(string str)
            {
                return (str == null || str == "" || str.Contains("empty"));
            }
        }
    }
}
