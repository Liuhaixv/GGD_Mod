using Handlers.CommonHandlers.UIHandlers;
using Handlers.GameHandlers.PlayerHandlers;
using Handlers.MenuSceneHandlers;
using HarmonyLib;
using MelonLoader;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Events;

//屏蔽一堆答辩
//https://harmony.pardeike.net/articles/patching.html
namespace GGD_Hack.Hook
{
    class BlockShit
    {
        /// <summary>
        /// 附加公开游戏警告
        /// </summary>
        //[HarmonyPatch(typeof(MenuSceneHandler), nameof(MenuSceneHandler.AttachPublicGameWarning))]
        class AttachPublicGameWarningHook
        {
        }

        /// <summary>
        ///  附加教程警告
        /// </summary>
        //[HarmonyPatch(typeof(MenuSceneHandler), nameof(MenuSceneHandler.AttachTutorialUnfinishedWarning))]
        class AttachTutorialUnfinishedWarningHook
        {
        }

        class GlobalPanelsHandler_
        {
            [HarmonyPatch(typeof(GlobalPanelsHandler), nameof(GlobalPanelsHandler.OpenPromptPanel),
                typeof(string), typeof(string), typeof(string),
                typeof(UnityAction),
                typeof(string),
                typeof(UnityAction),
                typeof(bool)
                )]
            class OpenPromptPanel_
            {
#if Developer
                //打印弹窗
                static bool Prefix(string __0, string __1, string __2, UnityAction __3, string __4, UnityAction __5, bool __6)
                {
                    MelonLogger.Msg("======OpenPromptPanel======");
                    MelonLogger.Msg("Title: " + __0);
                    MelonLogger.Msg("Message: " + __1);
                    MelonLogger.Msg("RejectButtonText: " + __2);
                    MelonLogger.Msg("RejectAction: " + __3.Method.Name);
                    MelonLogger.Msg("ConfirmButtonText: " + __4);
                    MelonLogger.Msg("ConfirmAction: " + __5.Method.Name);
                    MelonLogger.Msg("bool: " + __6);
                    MelonLogger.Msg("===========================" + '\n');

                    return true;
                }
#endif                
                
                //跳过恶心的弹窗
                static bool Prefix(UnityAction __3, UnityAction __5)
                {
                    UnityAction reject = __3;
                    UnityAction confirm = __5;

                    if (__5.Method.Name.Contains("AttachPublicGameWarning") ||
                       __5.Method.Name.Contains("AttachHangingOutMatureWarning") ||
                       __5.Method.Name.Contains("AttachTutorialUnfinishedWarning")
                           )
                    {
                        MelonLogger.Msg("已屏蔽游戏弹窗");
                        confirm.Invoke();
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }                
            }
        }
    }
}
