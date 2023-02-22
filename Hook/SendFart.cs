using GGD_Hack.Features;
using Handlers.GameHandlers.PlayerHandlers;
using Handlers.GameHandlers.SpecialBehaviour;
using HarmonyLib;
using Il2CppSystem;
using MelonLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GGD_Hack.Hook
{
    [HarmonyPatch(typeof(LocalPlayer), nameof(LocalPlayer.SendFart))]
    class SendFartHook
    {
        private static readonly Queue<System.Action> executionQueue = new Queue<System.Action>();

        //更新实例
        static void Postfix()
        {
            MelonLogger.Msg("正在放屁...");

            if (executionQueue.Count > 0)
            {
                foreach (var action in executionQueue)
                {
                    MelonLogger.Msg("正在执行action");
                    action.Invoke();
                }
            }                    
        }

        /// <summary>
        /// 清楚所有绑定的函数并指定新的Action
        /// </summary>
        /// <param name="action"></param>
        public static void bindAction(System.Action action)
        {
            lock (executionQueue)
            {
                if(action == null)
                {
                    return;
                }

                executionQueue.Clear();
                executionQueue.Enqueue(action);
            }
        }
    }
}
