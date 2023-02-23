using MelonLoader;
using System;
using HarmonyLib;

namespace GGD_Hack
{
    public static class BypassAC
    {
        /// <summary>
        /// 2.17.01
        /// 禁止检查Assemblies是否被提前加载
        /// </summary>
        public static void PatchAssembliesLoadCheck()
        {
            IntPtr intPtr = PatternScanner.OffsetToModule("GameAssembly.dll", 0x8D5FC0);
            MelonLogger.Msg("Start to patch assemblies' loading check at: 0x" + intPtr.ToString("X"));
            MemoryUtils.WriteBytes(intPtr, new byte[1] { 0x0 });

            //TODO:            
            //intPtr = PatternScanner.OffsetToModule("GameAssembly.dll", 0x7E6D70);
            //MelonLogger.Msg("Start to patch assemblies' FailFast at: 0x" + intPtr.ToString("X"));
            //MemoryUtils.WriteBytes(intPtr, new byte[1] { 0xC3 });
        }

        /// <summary>
        /// 禁止检测是否存在MelonLoader.dll
        /// </summary>
        [HarmonyPatch(typeof(FNNFIMKCHLH),nameof(FNNFIMKCHLH.MDBKDAJIDJE))]
        private static class MelonLoaderFileExsistsCheckPatch
        {
            //原返回值MelonLoader.dll
            static bool Prefix(ref string __result)
            {
                //篡改返回值
                __result = "LoaderMelon.dll";

                //跳过原函数的执行
                return false;
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
    }
}
