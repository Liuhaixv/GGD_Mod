#if Developer
using HarmonyLib;
using MelonLoader;
using UnityEngine;

namespace GGD_Hack.Hook
{
    public class Debug_
    {
        [HarmonyPatch(typeof(Debug), nameof(Debug.Log), typeof(Il2CppSystem.Object))]
        class Log_
        {
            static void Postfix(Il2CppSystem.Object __0)
            {
                MelonLogger.Msg("<color=green>[UnityEngine.Debug.Log] </color>" + __0.ToString());
            }
        }

        [HarmonyPatch(typeof(Debug), nameof(Debug.LogWarning), typeof(Il2CppSystem.Object))]
        class LogWarning_
        {
            static void Postfix(Il2CppSystem.Object __0)
            {
                MelonLogger.Msg("<color=yellow>[UnityEngine.Debug.LogWarning] <color=yellow>" + __0.ToString());
            }
        }
    }
}
#endif