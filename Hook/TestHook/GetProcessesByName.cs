using HarmonyLib;
using MelonLoader;
using System.Text;

namespace GGD_Hack.Hook.TestHook
{
    public class GetProcessesByNameHook
    {
        [HarmonyPatch(typeof(System.Diagnostics.Process), nameof(System.Diagnostics.Process.GetProcessesByName), typeof(string), typeof(string))]
        class GetProcessesByName_
        {
            static void Postfix(System.Diagnostics.Process[] __result, string __0, string __1)
            {
                try
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine("--------------------");
                    sb.AppendLine("static System.Diagnostics.Process[] System.Diagnostics.Process::GetProcessesByName(string processName, string machineName)");
                    sb.Append("- Parameter 0 'processName': ").AppendLine(__0?.ToString() ?? "null");
                    sb.Append("- Parameter 1 'machineName': ").AppendLine(__1?.ToString() ?? "null");
                    sb.Append("- Return value: ").AppendLine(__result?.ToString() ?? "null");
                    MelonLogger.Msg(sb.ToString());
                }
                catch (System.Exception ex)
                {
                    MelonLogger.Warning($"Exception in patch of static System.Diagnostics.Process[] System.Diagnostics.Process::GetProcessesByName(string processName, string machineName):\n{ex}");
                }
            }
        }
    }
}
