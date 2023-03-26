#if Developer
using CodeStage.AntiCheat.ObscuredTypes;
using HarmonyLib;
using MelonLoader;
using System.Text;
using static MelonLoader.MelonLogger;

namespace GGD_Hack.AntiAC
{
    public class DeobfuscateString
    {

        
        //TODO:打印被混淆的字符串
        //[HarmonyPatch(typeof(ObscuredString), ".ctor")]
        public class ObscuredString_
        {
            static void Postfix(CodeStage.AntiCheat.ObscuredTypes.ObscuredString __instance)
            {
                try
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine("\n--------------------");
                    sb.AppendLine("string CodeStage.AntiCheat.ObscuredTypes.ObscuredString::ToString()");
                    //sb.Append("obscured: ").AppendLine(__result);
                    sb.Append("decrypted: ").AppendLine(__instance.GetDecrypted());
                    MelonLogger.Msg(System.ConsoleColor.Red, sb.ToString());
                }
                catch (System.Exception ex)
                {
                    MelonLogger.Warning($"Exception in patch of string CodeStage.AntiCheat.ObscuredTypes.ObscuredString::ToString():\n{ex}");
                }
            }
        }
    }
}
#endif