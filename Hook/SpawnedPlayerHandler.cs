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
            static bool Prefix(PlayerProperties __0)
            {
                if (UnlockAllItems.Enabled.Value)
                {   //判断PlayerProperties是否要设置全为empty
                    if (__0.hat == "empty_Hats" &&
                        __0.pet == "empty_Pets" &&
                        __0.clothes == "empty_Clothes"
                        )
                    {
                        return false;
                    }
                }

                return true;
            }
        }
    }
}
