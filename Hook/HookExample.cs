using Handlers.GameHandlers.PlayerHandlers;
using HarmonyLib;
using MelonLoader;
using System.Collections.Generic;

/*https://harmony.pardeike.net/articles/patching.html
namespace GGD_Hack.Hook
{
    public class DeathStingerSplashHook
    {
        /// <summary>
        /// 跳过死亡动画
        /// </summary>
        [HarmonyPatch(typeof(DeathStingerSplash), nameof(DeathStingerSplash.StartDeathStinger))]
        class StartDeathStingerHook
        {
            static bool Prefix(ref DeathStingerSplash __instance)
            {
                __instance.CloseThisShitSequence();
                //跳过执行
                return false;
            }
        }
    }
}
*/