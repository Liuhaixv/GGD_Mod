#if false
using HarmonyLib;
using GGD_Hack.EOS;
using MelonLoader;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace GGD_Hack.AntiAC
{
    public class EOS
    {
        //[HarmonyPatch]
        private static class AllMethods
        {
            static IEnumerable<MethodBase> TargetMethods()
            {
                //System.Collections.Generic.List<string> list = AccessTools.GetMethodNames(typeof(AKCCGGKHPIA));

                return typeof(LMDCNGHFDAI).GetMethods()
                    //.Where(method => method.ReturnType == typeof(string))//返回值为string的方法
                    .Cast<MethodBase>();
            }

            static void Postfix()
            {
                StackTrace stackTrace = new StackTrace();
                string stackTraceString = stackTrace.ToString();
                MelonLogger.Msg(System.ConsoleColor.Green, "EOS:{0}", stackTraceString);
            }
        }

        [HarmonyPatch(typeof(OJKAAKLKJAI), nameof(OJKAAKLKJAI.PHFNHOJBOGO))]
        class EOS_AntiCheatClient_AddNotifyMessageToServer
        {
            static void Postfix()
            {
                MelonLogger.Msg(System.ConsoleColor.Green, "EOS:{0}", "EOS_AntiCheatClient_AddNotifyMessageToServer");
            }
        }

        [HarmonyPatch(typeof(OJKAAKLKJAI), nameof(OJKAAKLKJAI.PDEFPLPMPEL))]
        class EOS_AntiCheatClient_ReceiveMessageFromServer
        {
            static void Postfix()
            {
                MelonLogger.Msg(System.ConsoleColor.Green, "EOS:{0}", "EOS_AntiCheatClient_ReceiveMessageFromServer");
            }
        }

        [HarmonyPatch(typeof(OJKAAKLKJAI), nameof(OJKAAKLKJAI.DKBINPOOBEP))]
        class EOS_AntiCheatClient_RemoveNotifyMessageToServer
        {
            static void Postfix()
            {
                MelonLogger.Msg(System.ConsoleColor.Green, "EOS:{0}", "EOS_AntiCheatClient_RemoveNotifyMessageToServer");
            }
        }

        //EOS_HAntiCheatClient Handle
        //EOS_AntiCheatClient_BeginSessionOptions* Options
        [HarmonyPatch(typeof(OJKAAKLKJAI), nameof(OJKAAKLKJAI.GNOFJOJHLJD))]
        class EOS_AntiCheatClient_BeginSession
        {
            static void Postfix(CHLHFLEEPBB __1)
            {
                BeginSessionOptions options = __1;
                MelonLogger.Msg(System.ConsoleColor.Green, "EOS:{0}\n{1}", "EOS_AntiCheatClient_BeginSession", options);
            }
        }

        [HarmonyPatch(typeof(OJKAAKLKJAI), nameof(OJKAAKLKJAI.DFOPFKEGKAB))]
        class EOS_AntiCheatClient_AddNotifyClientIntegrityViolated
        {
            static void Postfix()
            {
                MelonLogger.Msg(System.ConsoleColor.Green, "EOS:{0}", "EOS_AntiCheatClient_AddNotifyClientIntegrityViolated");
            }
        }
    }
}
#endif