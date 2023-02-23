using Handlers.GameHandlers.PlayerHandlers;
using HarmonyLib;
using MelonLoader;
using System.Collections.Generic;

/*https://harmony.pardeike.net/articles/patching.html
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
                if (action == null)
                {
                    return;
                }

                executionQueue.Clear();
                executionQueue.Enqueue(action);
            }
        }
    }
}
*/