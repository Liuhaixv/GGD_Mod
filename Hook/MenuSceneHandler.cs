using Handlers.GameHandlers.PlayerHandlers;
using Handlers.MenuSceneHandlers;
using HarmonyLib;
using MelonLoader;
using System.Collections.Generic;
using UnityEngine.Events;

//https://harmony.pardeike.net/articles/patching.html
namespace GGD_Hack.Hook
{
    class MenuSceneHandlerHook
    {
        /// <summary>
        /// 跳过公开游戏警告
        /// </summary>
        [HarmonyPatch(typeof(MenuSceneHandler), nameof(MenuSceneHandler.AttachPublicGameWarning))]
       class AttachPublicGameWarningHook
        {
            static bool Prefix(ref UnityAction __result)
            {
                __result = null;
                return false;
            }
        }

        /// <summary>
        ///  跳过教程警告
        /// </summary>
        [HarmonyPatch(typeof(MenuSceneHandler), nameof(MenuSceneHandler.AttachTutorialUnfinishedWarning))]
        class AttachTutorialUnfinishedWarningHook
        {
            static bool Prefix(ref UnityAction __result)
            {
                __result = null;
                return false;
            }
        }
    }
}
