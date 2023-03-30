using HarmonyLib;
using MelonLoader;
using System.Collections.Generic;
using System.Text;

namespace GGD_Hack.AntiSell
{

    [HarmonyPatch(typeof(Gaggle.Translation.TranslationManager), nameof(Gaggle.Translation.TranslationManager.GetTranslation), typeof(string))]
    public class HackTranslation
    {
        private static Dictionary<string, string> hackedStrings = new Dictionary<string, string>() {
            { "MENU_PLAY","GGDHack Mod\nv" + BuildInfo.Version}
        };

        static void Postfix(Gaggle.Translation.TranslationManager __instance, string __result, string __0)
        {
            try
            {
                if (hackedStrings.ContainsKey(__0))
                {

                    Gaggle.Translation.Translation translation = null;
                    translation = __instance.Translations[__instance.CurrentLanguage];

                    if(translation != null)
                    {
                        int index = translation.Keys.IndexOf(__0);
                        translation.Values[index] = hackedStrings[__0];
                        __result = hackedStrings[__0];
#if Developer
                        MelonLogger.Msg(System.ConsoleColor.Green, "已修改翻译文本：{0} -> {1} -> {2}", index, __0, __result);
#endif
                    }
                }
                /*
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("--------------------");
                sb.AppendLine("string Gaggle.Translation.TranslationManager::GetTranslation(string GDFPBPKHPDH)");
                sb.Append("- __instance: ").AppendLine(__instance.ToString());
                sb.Append("- Parameter 0 'GDFPBPKHPDH': ").AppendLine(__0?.ToString() ?? "null");
                sb.Append("- Return value: ").AppendLine(__result?.ToString() ?? "null");
                MelonLogger.Msg(sb.ToString());
                */
            }
            catch (System.Exception ex)
            {
                MelonLogger.Warning($"Exception in patch of string Gaggle.Translation.TranslationManager::GetTranslation(string GDFPBPKHPDH):\n{ex}");
            }
        }

    }
}
