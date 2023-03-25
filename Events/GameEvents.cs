using MelonLoader;
using HarmonyLib;

namespace GGD_Hack.Events
{
    //TODO
    public  class GameEvents
    {
        public static void OnGameStart()
        {
            MelonLogger.Msg(System.ConsoleColor.Green, "游戏开始");
        }

        public static void OnGameEnded()
        {
            MelonLogger.Msg(System.ConsoleColor.Green, "游戏结束");
        }

        //TODO: [HarmonyPatch(typeof())]
        //GAME_ENDED
    }
}
