using HarmonyLib;

namespace GGD_Hack.AntiAC
{
    public class BypassEacChecking
    {
        //线索一：Unity.Debug.LogError打印了Anti-Cheat is not installed
        //调用了线索二反查出来的一个sub无名函数，在末尾调用了FailFastg
        //48 89 4C 24 08 53 56 57 48 81 EC C0 01 00 00 48 8B D9 80 3D ?? ?? ?? ?? ?? 75 73 48 8D 0D ?? ?? ?? ?? E8 ?? ?? ?? ?? 48 8D 0D ?? ?? ?? ?? E8 ?? ?? ?? ?? 48 8D 0D ?? ?? ?? ?? E8 ?? ?? ?? ?? 48 8D 0D ?? ?? ?? ?? E8 ?? ?? ?? ?? 48 8D 0D ?? ?? ?? ?? E8 ?? ?? ?? ?? 48 8D 0D ?? ?? ?? ?? E8 ?? ?? ?? ?? 48 8D 0D ?? ?? ?? ?? E8 ?? ?? ?? ?? 48 8D 0D ?? ?? ?? ?? E8 ?? ?? ?? ?? 48 8D 0D ?? ?? ?? ?? E8 ?? ?? ?? ?? C6 05 ?? ?? ?? ?? ?? 0F 57 C0 33 C0 0F 11 84 24 C0 00 00 00 0F 11 84 24 D0 00 00 00 0F 11 84 24 E0 00 00 00 0F 11 84 24 F0 00 00 00 0F 11 84 24 00 01 00 00 0F 11 84 24 10 01 00 00 48 89 84 24 20 01 00 00 0F 57 C9 0F 11 8C 24 30 01 00 00 0F 11 8C 24 40 01 00 00 0F 11 8C 24 50 01 00 00 0F 11 8C 24 60 01 00 00 0F 11 8C 24 70 01 00 00 0F 11 8C 24 80 01 00 00 0F 11 8C 24 90 01 00 00 0F 11 8C 24 A0 01 00 00 88 84 24 F0 01 00 00 45 33 C0 BA 58
        //ShaderCachingDx11.Start

        //线索二：检测到EAC未运行时会调用的FailFast函数，反查调用的函数
        //E8 ?? ?? ?? ?? 48 8D 0D ?? ?? ?? ?? E8 ?? ?? ?? ?? 48 8B C8 E8 ?? ?? ?? ?? 48 8B 5E 10
        //void __stdcall __noreturn System_Environment__FailFast(System_String_o *message, const MethodInfo *method)
        //{
        //System_Environment__FailFast_6495551328_0(message, 0i64, 0i64);
        //}

        //线索三：EAC Failed to run
        //bool __stdcall KALHKLDPOHI__JJLDLGEDBHG(
        //KALHKLDPOHI_o* DKJMCCOOLFF,
        //KALHKLDPOHI_o *BOLDMODIPCD,
        //const MethodInfo* method)
        //
        //本函数返回两个参数不相等
        //本函数必须返回true，也就是说两个参数必须不同，否则会视为异常，调用ShaderCachingDx11某函数然后FailFast
        //48 83 EC 28 48 85 C9 74 0F 45

        //线索四：Anti-Cheat is not installed
        //bool __stdcall KALHKLDPOHI__JHIEMJGCLEP(
        //KALHKLDPOHI_o* DKJMCCOOLFF,
        //KALHKLDPOHI_o* BOLDMODIPCD,
        //const MethodInfo* method)
        //
        //本函数返回两个参数相等
        //本函数必须返回false，也就是说两个参数必须不同，否则会视为异常
        //E8 ?? ?? ?? ?? 48 8B 9C 24 A0 01 00 00 84 C0

        //线索五：摧毁MainManager的GameObject
        //E8 ?? ?? ?? ?? 48 8B 5C 24 60 48 8B 74 24 68 48 8B 7C 24 70 48 83 C4 50 41 5E C3 48 C7 44 24 38 00 00 00 00

        //线索六：IDA中字符串
        //EOSSDK-Win64-Shipping
        //EOS_AntiCheatClient_AddNotifyClientIntegrityViolated

        //是否未安装反作弊？
        [HarmonyPatch(typeof(KALHKLDPOHI),nameof(KALHKLDPOHI.JHIEMJGCLEP))]
        class BypassAntiCheatNotInstalled
        {
            //必须返回false
            //KALHKLDPOHI__JHIEMJGCLEP
            //E8 ?? ?? ?? ?? 48 8B 9C 24 A0 01 00 00 84 C0
            static bool Prefix(ref bool __result)
            {
                __result = false;
                return false;
            }
        }

        //在该函数中ShaderCachingDx11$$Start
        //E8 ?? ?? ?? ?? 48 85 C0 0F 84 1D 03 00 00 48 8B 0D
        [HarmonyPatch(typeof(MMIPLNJKFNJ),nameof(MMIPLNJKFNJ.CKKGKAALBDL))]
        class BypassAntiCheatNotRunning
        {
            //必须返回true
            
            static void Postfix(ref ulong __result)
            {
                __result = ulong.MaxValue;
                //return false;
            }
        }

        //在该函数中ShaderCachingDx11$$Start
        //返回KALHKLDPOHI实例
        //E8 ?? ?? ?? ?? EB 03 48 8B C6 45 33 C0
        [HarmonyPatch(typeof(JLBGAABKFCE), nameof(JLBGAABKFCE.EIFOHALOMMD))]
        class FakeInstance
        {
            static bool Prefix(ref MMIPLNJKFNJ __result)
            {
                __result = new MMIPLNJKFNJ();
                return false;
            }
        }

        [HarmonyPatch(typeof(ShaderCachingDx11), nameof(ShaderCachingDx11.Update))]
        class ShaderCachingDx11_Update
        {
            static bool Prefix()
            {
                
                return false;
            }
        }
    }
}
