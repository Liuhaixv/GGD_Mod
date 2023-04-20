using MelonLoader;
using System;
using HarmonyLib;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using Managers;

//检测到ML后通常会调用UnityEngine_Diagnostics_Utils__ForceCrash(2)
//或者Environment.FailFast
namespace GGD_Hack.AntiAC
{
    public static class BypassMelonLoaderDllCheck
    {


        /// <summary>
        /// 2.18.00.02
        /// 禁止检测是否存在MelonLoader.dll
        /// 该类调用了Mono_Math_BigInteger$$GetBytes_6494408592
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

                return typeof(GEKKDAMDHNC).GetMethods()
                    .Where(method => method.ReturnType == typeof(string))//返回值为string的方法
                    .Cast<MethodBase>();
            }

            static void Postfix(ref string __result)
            {
                string base64Decoded = null;

                try
                {
                    base64Decoded = System.Text.Encoding.ASCII.GetString(System.Convert.FromBase64String(__result.Replace(",", "")));
                }
                catch (Exception ex)
                {
                    base64Decoded = null;
                }
                
                MelonLogger.Msg(System.ConsoleColor.Yellow, "游戏正在检查字符串:{0}Base64解码:\"{1}\"",
                                                 __result,
                                                 base64Decoded != null ? base64Decoded : "无"
                                                 );

                if (base64Decoded?.Contains( "MelonLoader") ?? false)
                {
                    MelonLogger.Msg("游戏即将检测到非法DLL: {0}",base64Decoded);
                    // 修改返回值
                    __result = System.Convert.ToBase64String(
                        System.Text.Encoding.ASCII.GetBytes("f" + base64Decoded));
                    MelonLogger.Msg("已成功修改DLL名称返回值!");
                }
            }
        }

        //[HarmonyPatch(typeof(Environment), nameof(Environment.FailFast), typeof(string))]
        class FailFastPatch
        {
            static bool Prefix(string message)
            {
                MelonLogger.Msg(System.ConsoleColor.Red, "阻止了FailFast: " + message);
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

        //2.20.00
        /// <summary>
        /// 这个类专门负责检查指定路径是否存在某些文件，然后ForceCrash
        /// </summary>
        ///48 83 EC 38 33 C9 E8 ?? ?? ?? ?? 33 C9 E8 ?? ?? ?? ?? 33
        [HarmonyPatch(typeof(DPNBEKDCLIE), nameof(DPNBEKDCLIE.KOLPGPMALKJ))]
        class PreloadCheckPatch
        {
            static bool Prefix()
            {
                return false;
            }
        }
    }
}