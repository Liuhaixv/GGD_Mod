using HarmonyLib;
using Managers.ConnectionManagers;
using MelonLoader;
using System.Text;

namespace GGD_Hack.Features
{
    public class ChangeName
    {
        public static string overrideName = null;

        public static void OverrideName(string name)
        {
            overrideName = name;
        }

        public static void StopOverride()
        {
           overrideName = null;
        }

        [HarmonyPatch(typeof(RoomManager), nameof(RoomManager.GEEMGMMAHKK))]
        public class OverrideNamePatch
        {
            static void Prefix(Managers.ConnectionManagers.RoomManager __instance, Il2CppSystem.Collections.IEnumerator __result, string __0, ref string __1, string __2, string __3, bool __4)
            {
                try
                {
                    if(overrideName != null)
                    {
                        __1 = overrideName;
                    }

                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine("--------------------");
                    sb.AppendLine("Il2CppSystem.Collections.IEnumerator Managers.ConnectionManagers.RoomManager::GEEMGMMAHKK(string LOPKLMEJFGC, string GBNIEEGIBFD, string OMHOMNEPPKM, string CLDANIALKPF, bool IEHHFCLHFAP)");
                    sb.Append("- __instance: ").AppendLine(__instance.ToString());
                    sb.Append("- Parameter 0 'LOPKLMEJFGC': ").AppendLine(__0?.ToString() ?? "null");
                    sb.Append("- Parameter 1 'GBNIEEGIBFD': ").AppendLine(__1?.ToString() ?? "null");
                    sb.Append("- Parameter 2 'OMHOMNEPPKM': ").AppendLine(__2?.ToString() ?? "null");
                    sb.Append("- Parameter 3 'CLDANIALKPF': ").AppendLine(__3?.ToString() ?? "null");
                    sb.Append("- Parameter 4 'IEHHFCLHFAP': ").AppendLine(__4.ToString());
                    sb.Append("- Return value: ").AppendLine(__result?.ToString() ?? "null");
                    MelonLogger.Msg(sb.ToString());
                }
                catch (System.Exception ex)
                {
                    MelonLogger.Msg($"Exception in patch of Il2CppSystem.Collections.IEnumerator Managers.ConnectionManagers.RoomManager::GEEMGMMAHKK(string LOPKLMEJFGC, string GBNIEEGIBFD, string OMHOMNEPPKM, string CLDANIALKPF, bool IEHHFCLHFAP):\n{ex}");
                }
            }

        }
    }
}
