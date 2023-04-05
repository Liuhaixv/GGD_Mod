using Handlers.GameHandlers.PlayerHandlers;
using Handlers.LobbyHandlers;
using Handlers.SettingsHandler;
using HarmonyLib;
using MelonLoader;

namespace GGD_Hack.Features.RolesIndicator
{
    //本地修改
    public class ForceEnableRoleReveal
    {
        [HarmonyPatch(typeof(PlayerController),nameof(PlayerController.Update))]
        class PlayerController_Update
        {
            static void Postfix(PlayerController __instance)
            {
                if(__instance.isLocal)
                {
                    return;
                }

                if(!LobbySceneHandler.Instance.gameStarted)
                {
                    return;
                }

                __instance.playerNameRoleText.enabled = true;
            }
        }
    }
}
