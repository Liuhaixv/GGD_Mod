using Il2CppSystem.Diagnostics.Tracing;
using MelonLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using Handlers.CommonHandlers;
//绕过不能输入尖括号的限制
namespace GGD_Hack.Hook.BlockShit
{
    public class AntiFilterText
    {
        [HarmonyPatch(typeof(ChatPanelHandler), nameof(ChatPanelHandler.OnTyping))]
        class OnTyping_
        {
            static bool Prefix(Handlers.CommonHandlers.ChatPanelHandler __instance, ref string __state)
            {

                try
                {
                    if (__instance.messageToSend.m_Text == null || __instance.messageToSend.m_Text.Contains("<") || __instance.messageToSend.m_Text.Contains(">"))
                    {
                        __state = null;
                        return false;
                    }

                    __state = __instance.messageToSend.m_Text;

                }
                catch (System.Exception ex)
                {
                    MelonLoader.MelonLogger.Msg($"Exception in patch of void Handlers.CommonHandlers.ChatPanelHandler::OnTyping():\n{ex}");
                }
                return true;
            }

            static void Postfix(Handlers.CommonHandlers.ChatPanelHandler __instance, ref string __state)
            {
                try
                {
                    /*
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine("\"" + (__state != null ? __state : "") + '"' + "->\"" + __instance.messageToSend.m_Text + '"');
                    MelonLoader.MelonLogger.Msg(sb.ToString());
                    */

                    if (__state != null)
                        __instance.messageToSend.m_Text = __state;
                }
                catch (System.Exception ex)
                {
                    MelonLogger.Msg(ex.Message);
                }
            }
        }

        [HarmonyPatch(typeof(ChatPanelHandler), nameof(ChatPanelHandler.SendChatMessage))]
        class SendChatMessage_
        {
            static bool Prefix(Handlers.CommonHandlers.ChatPanelHandler __instance)
            {
                try
                {
                    Managers.MainManager.Instance.chatManager.SendChatMessageEvent(__instance.messageToSend.m_Text);

                }
                catch (System.Exception ex)
                {
                   MelonLogger.Warning($"Exception in patch of void Handlers.CommonHandlers.ChatPanelHandler::SendChatMessage():\n{ex}");
                }
                return false;
            }
        }
    }
}