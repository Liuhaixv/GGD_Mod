using Handlers.CommonHandlers;
using HarmonyLib;
using MelonLoader;
using UnityEngine.UI;
using UnityEngine;

namespace GGD_Hack.AntiSell
{
    [HarmonyPatch(typeof(ClientSettings), nameof(ClientSettings.Update))]
    public class ChangeVersionText
    {
        static void Postfix(Handlers.CommonHandlers.ClientSettings __instance)
        {
            try
            {
                __instance.PluginVersionText.text = Utils.Utils.IsChineseSystem() ? "本mod完全免费! 禁止售卖" : "This mod is 100% Free!";
                __instance.PluginVersionText.color = UnityEngine.Color.red;
                __instance.PluginVersionText.fontSize = 70;
                //__instance.PluginVersionText.SetOutlineColor(UnityEngine.Color.black);
            }
            catch (System.Exception ex)
            {
                MelonLogger.Msg("Exception in patch of void Handlers.CommonHandlers.ClientSettings::Update():\n{" + ex + "}");
            }
        }
    }
}
