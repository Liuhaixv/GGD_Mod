#if Developer
using HarmonyLib;
using MelonLoader;
using System;
using System.Diagnostics;
using UnityEngine;

using Debug = UnityEngine.Debug;

namespace GGD_Hack.Hook
{
    public class Debug_
    {
        [HarmonyPatch(typeof(Debug), nameof(Debug.Log), typeof(Il2CppSystem.Object))]
        class Log_
        {
            static void Postfix(Il2CppSystem.Object __0)
            {
                MelonLogger.Msg("[UnityEngine.Debug.Log] " + __0.ToString());
            }
        }

        [HarmonyPatch(typeof(Debug), nameof(Debug.LogWarning), typeof(Il2CppSystem.Object))]
        class LogWarning_
        {
            static void Postfix(Il2CppSystem.Object __0)
            {
                
                MelonLogger.Warning("[UnityEngine.Debug.LogWarning] " + __0.ToString());
            }
        }

        [HarmonyPatch(typeof(Debug), nameof(Debug.LogError), typeof(Il2CppSystem.Object))]
        class LogLogError_
        {
            static void Postfix(Il2CppSystem.Object __0)
            {
                try
                {
                    MelonLogger.Error("[UnityEngine.Debug.LogError] " + __0.ToString());

                    if (__0.ToString().Contains("Anti-Cheat is not installed") || __0.ToString().Contains("EAC is not running"))
                    {
                        //打印跟踪栈
                        throw new Exception("EAC错误");
                    }
                }catch(Exception ex)
                {
                    MelonLogger.Warning(ex.Message);
                    MelonLogger.Warning(ex.StackTrace);
                }
            }
        }

        //TODO[HarmonyPatch(typeof(Debug), nameof(Debug.LogException), typeof(Il2CppSystem.Object))]
        class LogException_
        {
            static void Postfix(Il2CppSystem.Exception __0)
            {
                MelonLogger.Warning("[UnityEngine.Debug.LogException] " + __0.Message);
            }
        }
    }
}
#endif