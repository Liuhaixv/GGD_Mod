using MelonLoader;
using System;
using System.Collections.Generic;
using HarmonyLib;
using TMPro;

//修改游戏设置面板中的“账户”，篡改为“Mod”，在里面放一些关于mod设置的选项
namespace GGD_Hack.Features
{
    public class IngameSettings
    {
        public static List<IngameSettingsEntry> registeredEntries;
        public struct IngameSettingsEntry
        {
            //设置类型
            public MelonPreferences_Entry entry;
            //中文描述
            public string name_cn;
            //英文描述
            public string name_eng;
        }

        //添加游戏内设置项
        public static void AddIngameSettingsEntry(IngameSettingsEntry ingameSettingsEntry)
        {
            //检查是否之前添加过
            foreach (IngameSettingsEntry settingsEntry in registeredEntries)
            {
                if (settingsEntry.entry == ingameSettingsEntry.entry)
                {
                    MelonLogger.Error("已经添加过设置项:" + ingameSettingsEntry.entry.DisplayName);
                    return;
                }
            }

            registeredEntries.Add(ingameSettingsEntry);
        }

        //将所有持久化设置注册到游戏内
        private static void RegisterAllMelonPreferencesIntoGameSettings()
        {
        }

        //该页面只有在设置菜单被打开时候才会每帧更新
        //可以用来修改设置菜单中“账户”的名字为“Mod”
        //Client Settings New New 
        //  Frame (New)
        //      BG
        //          Bottom
        //              Tabs
        //                  Account
        //                      Text -> TextMeshProUGUI
        [HarmonyPatch(typeof(Handlers.CommonHandlers.ClientSettings),nameof(Handlers.CommonHandlers.ClientSettings.Update))]
        public class ClientSettings_Update
        {
            static void Postfix(Handlers.CommonHandlers.ClientSettings __instance)
            {
                UnhollowerBaseLib.Il2CppReferenceArray<ModularUIButtonHandler> modularUIButtonHandlers = __instance.GetVerticalTabs();
                ModularUIButtonHandler account = null;
                foreach(ModularUIButtonHandler modularUIButtonHandler in modularUIButtonHandlers)
                {
                    if((modularUIButtonHandler.gameObject?.name ?? null) == "Account")
                    {
                        account = modularUIButtonHandler;
                        break;
                    }
                }

                if(account == null)
                {
                    MelonLogger.Error("未找到Account");
                    return;
                }

                TextMeshProUGUI textMeshProUGUI = account?.gameObject.transform.Find("Text")?.GetComponent<TextMeshProUGUI>();
                if(textMeshProUGUI == null)
                {
                    MelonLogger.Error("未找到Account->TextMeshProUGUI");
                    return;
                }
                textMeshProUGUI.text = "Mod";
                textMeshProUGUI.ForceMeshUpdate();

                if(account.gameObject.active == false)
                {
                    account.gameObject.SetActive(true);
                }
            }
        }

        //添加持久化注册设置项
        //TODO:
        [HarmonyPatch(typeof(Handlers.CommonHandlers.ClientSettings), nameof(Handlers.CommonHandlers.ClientSettings.Start))]
        public class ClientSettings_Start
        {
            static void Postfix(Handlers.CommonHandlers.ClientSettings __instance)
            {
                UnhollowerBaseLib.Il2CppReferenceArray<ModularUIButtonHandler> modularUIButtonHandlers = __instance.GetVerticalTabs();
                ModularUIButtonHandler account = null;
                foreach (ModularUIButtonHandler modularUIButtonHandler in modularUIButtonHandlers)
                {
#if Developer
                    MelonLogger.Msg(System.ConsoleColor.Green, "ClientSettings.Start: " + modularUIButtonHandler.gameObject.name);
#endif
                }
            }
        }
    }
}
