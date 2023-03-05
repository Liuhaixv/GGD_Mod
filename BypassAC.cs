﻿using MelonLoader;
using System;
using HarmonyLib;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using Managers;

namespace GGD_Hack
{
    public static class BypassAC
    {
       

        /// <summary>
        /// 2.18.00.02
        /// 禁止检测是否存在MelonLoader.dll
        /// 该类调用了Mono.Math.BigInteger$$GetBytes_6494408592
        /// </summary>
        /// 
        /// 修改所有方法
        /// https://harmony.pardeike.net/articles/annotations.html#patching-multiple-methods
        [HarmonyPatch]
        private static class MelonLoaderFileExsistsCheckPatch
        {
            static IEnumerable<MethodBase> TargetMethods()
            {
                //System.Collections.Generic.List<string> list = AccessTools.GetMethodNames(typeof(AKCCGGKHPIA));
                
                return AccessTools.TypeByName("BBNPKEHDLPJ").GetMethods()
                    .Where(method => method.ReturnType == typeof(string))//返回值为string的方法
                    .Cast<MethodBase>();
            }

            static void Postfix(ref string __result)
            {
                //MelonLogger.Msg("游戏正在检查非法DLL名称：" + __result);
                if (__result == "MelonLoader.dll")
                {
                    MelonLogger.Msg("游戏即将检测到非法DLL: MelonLoader.dll");
                    // 修改返回值
                    __result = "LodasMnfks.dll";
                    MelonLogger.Msg("已成功修改DLL名称返回值!");
                }
            }
        }

        [HarmonyPatch(typeof(Environment), nameof(Environment.FailFast), typeof(string), typeof(Exception))]
        class FailFastPatch
        {
            static bool Prefix(string message, Exception exception)
            {
                return false;
            }
        }


        /*
       ///在MainManager的Awake方法末尾中
       /// <summary>
       /// 2.18.00
       /// 禁止检查Assemblies是否被提前加载
       /// </summary>
       public static void PatchAssembliesLoadCheck()
       {
           IntPtr intPtr = PatternScanner.OffsetToModule("GameAssembly.dll", 0xA8E23E+2);
           MelonLogger.Msg("Start to patch assemblies' loading check at: 0x" + intPtr.ToString("X"));
           MemoryUtils.WriteBytes(intPtr, new byte[1] { 0x0 });

           //intPtr = PatternScanner.OffsetToModule("GameAssembly.dll", 0x7E6D70);
           //MelonLogger.Msg("Start to patch assemblies' FailFast at: 0x" + intPtr.ToString("X"));
           //MemoryUtils.WriteBytes(intPtr, new byte[1] { 0xC3 });
       }
       */

        //2.18.00.02
        ///在MainManager的Awake方法末尾中，调用了两次 
        ///<summary>
        /// 在MainManager的Awake方法末尾中，调用了两次。检查dll是否提前加载等操作
        /// </summary>
        [HarmonyPatch(typeof(CKNDHODLGLO), nameof(CKNDHODLGLO.OPCLGJIMENO))]
        class PreloadCheckPatch
        {
            static bool Prefix()
            {
                return false;
            }
        }
    }
}
