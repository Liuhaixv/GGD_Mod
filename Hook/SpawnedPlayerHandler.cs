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
            //禁用屏蔽服务器回滚玩家装扮
            static bool Prefix(SpawnedPlayerHandler __instance, PlayerProperties __0)
            {
                //商店解锁功能未启用
                if (UnlockAllItems.Enabled.Value == false)
                {
                    return true;
                }

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

                return true;
            }

            static bool isEmpty(string str)
            {
                return (str == null || str == "" || str.Contains("empty"));
            }
        }
    }
}
