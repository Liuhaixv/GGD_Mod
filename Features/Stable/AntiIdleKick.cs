using MelonLoader;
using HarmonyLib;
using Handlers.LobbyHandlers;

//反挂机被踢
namespace GGD_Hack.Features
{
    public class AntiIdleKick
    {
        public static MelonPreferences_Entry<bool> Enabled = MelonPreferences.CreateEntry<bool>("GGDH", "Enable_" + nameof(AntiIdleKick), true);

        [HarmonyPatch(typeof(LobbySceneHandler),nameof(LobbySceneHandler.Start))]
        class LobbySceneHandle_Start
        {
            static void Postfix(LobbySceneHandler __instance)
            {
                if (Enabled.Value)
                {
                    float idleTime = 1000000000.0f;
                    MelonLogger.Msg(System.ConsoleColor.Green, "已修改房间内最大挂机允许时长为：" + idleTime);
                    __instance.inactiveTimeThreshold = idleTime;
                    __instance.privateInactiveTimeThreshold = idleTime;
                }
            }   
        }
    }
}
