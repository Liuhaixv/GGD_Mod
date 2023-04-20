#if false
using MelonLoader;
using UnhollowerRuntimeLib;
using UnityEngine;
using HarmonyLib;

using IntPtr = System.IntPtr;
using Il2CppSystem.Diagnostics;
//2.20.00

//弹出错误窗口，带代码48 89 5C 24 20 66 89 4C
namespace GGD_Hack.AntiAC
{
    [RegisterTypeInIl2Cpp]
    public class BypassIntegrityValidation : MonoBehaviour
    {
        public static BypassIntegrityValidation Instance;

        public BypassIntegrityValidation(IntPtr ptr) : base(ptr)
        {
        }

        public BypassIntegrityValidation() : base(ClassInjector.DerivedConstructorPointer<BypassIntegrityValidation>()) => ClassInjector.DerivedConstructorBody(this);
        public static void Init()
        {
            GameObject ML_Manager = GameObject.Find("ML_Manager");
            if (ML_Manager == null)
            {
                ML_Manager = new GameObject("ML_Manager");
                DontDestroyOnLoad(ML_Manager);
            }

            if (ML_Manager.GetComponent<BypassIntegrityValidation>() == null)
            {
                Instance = ML_Manager.AddComponent<BypassIntegrityValidation>();
            }
        }

        //48 89 5C 24 20 66 89 4C
        //[HarmonyPatch(typeof(MAHIKBIKKCD), nameof(MAHIKBIKKCD.NCFKMGBFCKL))]
        class IntegrityErrorPanel
        {
            static void Postfix()
            {
                MelonLogger.Error("完整性错误弹窗");
            }
        }

        //[HarmonyPatch(typeof(MAHIKBIKKCD), "NENKBKFEHLA")]
        class True0
        {
            static bool Prefix(ref bool __result)
            {
                __result = true;
                return true;
            }

            static void Postfix(ref bool __result)
            {
                __result = true; 
            }
        }

        //E8 ?? ?? ?? ?? 84 C0 74 07 33 C9 E8 ?? ?? ?? ?? 33 C9
        //[HarmonyPatch(typeof(MAHIKBIKKCD), "FDOPJEHLPHM")]
        class True1
        {
            static bool Prefix(ref bool __result)
            {
                __result = true;
                return true;
            }

            static void Postfix(ref bool __result)
            {
                __result = true;
            }
        }

        //必须返回true
        //48 89 5C 24 08 48 89 74 24 10 57 48 83 EC 20 80 3D ?? ?? ?? ?? ?? 75 13
        //有逻辑被执行
        //检查EAC是否运行
        [HarmonyPatch(typeof(MAHIKBIKKCD), "LGMJLPFDKIM")]
        class CheckIfEACRunning_32766
        {
            static bool Prefix(ref bool __result)
            {
                __result = true;
                return false;
            }
        }

        //必须返回true
        //Failed to obtain application configuration
        //48 89 7C 24 10 55 48 8D AC 24 70
        //[HarmonyPatch(typeof(MAHIKBIKKCD), "NKCHIINMLHO")]
        class ObtainApplicationConfiguration_32767
        {
            static bool Prefix(ref bool __result)
            {
                __result = true;
                return true;
            }

            static void Postfix(ref bool __result)
            {
                __result = true;
            }
        }

        //必须返回true
        //48 89 5C 24 20 56 48 83 EC 30 80 3D ?? ?? ?? ?? ?? 48 8B D9 75 43
        //[HarmonyPatch(typeof(MAHIKBIKKCD), "GEAFKOFAGHN")]
        class A_32765
        {
            static bool Prefix(ref bool __result)
            {
                __result = true;
                return true;
            }

            static void Postfix(ref bool __result)
            {
                __result = true;
            }
        }

        //必须返回true
        //48 89 5C 24 20 56 48 83 EC 30 80 3D ?? ?? ?? ?? ?? 48 8B D9 75 43
        //[HarmonyPatch(typeof(MAHIKBIKKCD), "OBAJCFOEHGE")]
        class HandleAntiCheatEvent_2
        {
            static bool Prefix()
            {
                MelonLogger.Msg(System.ConsoleColor.Green, "正在处理事件220");
                return true;
            }

            static void Postfix()
            {
                MelonLogger.Msg(System.ConsoleColor.Green, "已处理完事件220");
            }
        }        
        
        [HarmonyPatch(typeof(MAHIKBIKKCD), "GJAJMEGOHLP")]
        class IntegerityError2
        {
            static bool Prefix()
            {
                MelonLogger.Msg(System.ConsoleColor.Green, "错误代码2");
                return false;
            }
        }
        


        //必须返回true
        //40 53 48 83 EC 40 80 3D ?? ?? ?? ?? ?? 48 8B D9 75 67 48 8D 0D ?? ?? ?? ?? E8 ?? ?? ?? ?? 48 8D 0D ?? ?? ?? ?? E8 ?? ?? ?? ?? 48 8D 0D ?? ?? ?? ?? E8 ?? ?? ?? ?? 48 8D 0D ?? ?? ?? ?? E8 ?? ?? ?? ?? 48 8D 0D ?? ?? ?? ?? E8 ?? ?? ?? ?? 48 8D 0D ?? ?? ?? ?? E8 ?? ?? ?? ?? 48 8D 0D ?? ?? ?? ?? E8 ?? ?? ?? ?? 48 8D 0D ?? ?? ?? ?? E8 ?? ?? ?? ?? C6 05 ?? ?? ?? ?? ?? 80
        //bind violationCallbackDelegate
        //[HarmonyPatch(typeof(MAHIKBIKKCD), "MCIKPEOPEBJ")]
        class BindViolationCallbackDelegate_32765
        {
            static bool Prefix(ref bool __result)
            {
                __result = true;
                return true;
            }

            static void Postfix(ref bool __result)
            {
                __result = true;
            }
        }

        //48 83 EC 28 48 85 C9 74 0F 45 33 C0
        //[HarmonyPatch(typeof(FEOBCAGBPGB), nameof(FEOBCAGBPGB.DIPLALELGDG))]
        class PlatformInitialize_0
        {
            static bool Prefix(ref bool __result)
            {
                __result = true;
                return true;
            }

            static void Postfix(ref bool __result)
            {
                __result = true;
            }
        }
    }
}
#endif