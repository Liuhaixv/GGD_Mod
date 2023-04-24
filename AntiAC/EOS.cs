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

                return typeof(NNCHJGNDAIB).GetMethods()
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

        //[HarmonyPatch(typeof(NNCHJGNDAIB), nameof(NNCHJGNDAIB.PHFNHOJBOGO))]
        class EOS_AntiCheatClient_AddNotifyMessageToServer
        {
            static void Postfix()
            {
                MelonLogger.Msg(System.ConsoleColor.Green, "EOS:{0}", "EOS_AntiCheatClient_AddNotifyMessageToServer");
            }
        }

        //[HarmonyPatch(typeof(NNCHJGNDAIB), nameof(NNCHJGNDAIB.PHFNHOJBOGO))]
        class EOS_AntiCheatClient_AddNotifyPeerActionRequired
        {
            static void Postfix()
            {
                MelonLogger.Msg(System.ConsoleColor.Green, "EOS:{0}", "EOS_AntiCheatClient_AddNotifyPeerActionRequired");
            }
        }

        [HarmonyPatch(typeof(NNCHJGNDAIB), nameof(NNCHJGNDAIB.DIKBAIEBEMG))]
        class EOS_AntiCheatClient_ReceiveMessageFromServer
        {
            static void Postfix()
            {
                MelonLogger.Msg(System.ConsoleColor.Green, "EOS:{0}", "EOS_AntiCheatClient_ReceiveMessageFromServer");
            }
        }

        //[HarmonyPatch(typeof(NNCHJGNDAIB), nameof(NNCHJGNDAIB.DKBINPOOBEP))]
        class EOS_AntiCheatClient_RemoveNotifyMessageToServer
        {
            static void Postfix()
            {
                MelonLogger.Msg(System.ConsoleColor.Green, "EOS:{0}", "EOS_AntiCheatClient_RemoveNotifyMessageToServer");
            }
        }

        //EOS_HAntiCheatClient Handle
        //EOS_AntiCheatClient_BeginSessionOptions* Options
        [HarmonyPatch(typeof(NNCHJGNDAIB), nameof(NNCHJGNDAIB.GCGILGHGGLF))]
        class EOS_AntiCheatClient_BeginSession
        {
            static void Postfix(DJJFBMKJIBJ __result, JMDPPJDHNKC __1)
            {
                BeginSessionOptions options = __1;
                string result = __result.ToString();
                MelonLogger.Msg(System.ConsoleColor.Green, "EOS:{0}\n{1}\nResult:{2}", "EOS_AntiCheatClient_BeginSession", options, result);
            }
        }

        //[HarmonyPatch(typeof(NNCHJGNDAIB), nameof(NNCHJGNDAIB.DKBINPOOBEP))]
        class EOS_AntiCheatClient_EndSession
        {
            static void Postfix()
            {
                MelonLogger.Msg(System.ConsoleColor.Green, "EOS:{0}", "EOS_AntiCheatClient_EndSession");
            }
        }

        //[HarmonyPatch(typeof(NNCHJGNDAIB), nameof(NNCHJGNDAIB.DFOPFKEGKAB))]
        class EOS_AntiCheatClient_AddNotifyClientIntegrityViolated
        {
            static void Postfix()
            {
                MelonLogger.Msg(System.ConsoleColor.Green, "EOS:{0}", "EOS_AntiCheatClient_AddNotifyClientIntegrityViolated");
            }
        }
    }
}