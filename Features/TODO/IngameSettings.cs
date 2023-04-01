using MelonLoader;
using System;
using System.Collections.Generic;
using HarmonyLib;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static GGD_Hack.Features.IngameSettings;

//修改游戏设置面板中的“账户”，篡改为“Mod”，在里面放一些关于mod设置的选项
namespace GGD_Hack.Features
{
    public class IngameSettings
    {
        public static List<IngameSettingsEntry> registeredEntries = new List<IngameSettingsEntry>();

        private static GameObject boolSettingPrefab = null;
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
                if (settingsEntry.entry.DisplayName == ingameSettingsEntry.entry.DisplayName)
                {
                    MelonLogger.Error("已经添加过设置项:" + ingameSettingsEntry.entry.DisplayName);
                    return;
                } 
            }

            registeredEntries.Add(ingameSettingsEntry);
            MelonLogger.Msg(System.ConsoleColor.Green, "已注册mod设置项:" + ingameSettingsEntry.entry.DisplayName);
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
        [HarmonyPatch(typeof(Handlers.CommonHandlers.ClientSettings), nameof(Handlers.CommonHandlers.ClientSettings.Update))]
        public class ClientSettings_Update
        {
            static void Postfix(Handlers.CommonHandlers.ClientSettings __instance)
            {
                UnhollowerBaseLib.Il2CppReferenceArray<ModularUIButtonHandler> modularUIButtonHandlers = __instance.GetVerticalTabs();

                ModularUIButtonHandler account = null;
                foreach (ModularUIButtonHandler modularUIButtonHandler in modularUIButtonHandlers)
                {
                    if ((modularUIButtonHandler.gameObject?.name ?? null) == "Account")
                    {
                        account = modularUIButtonHandler;
                        break;
                    }
                }

                if (account == null)
                {
                    MelonLogger.Error("未找到Account");
                    return;
                }

                TextMeshProUGUI textMeshProUGUI = account?.gameObject.transform.Find("Text")?.GetComponent<TextMeshProUGUI>();
                if (textMeshProUGUI == null)
                {
                    MelonLogger.Error("未找到Account->TextMeshProUGUI");
                    return;
                }
                textMeshProUGUI.text = "Mod";
                textMeshProUGUI.ForceMeshUpdate();

                if (account.gameObject.active == false)
                {
                    account.gameObject.SetActive(true);
                }
            }
        }

        //添加持久化注册设置项
        //TODO:
        //账号按钮被点击后会调用SetupAccountSettingsNavigation
        [HarmonyPatch(typeof(Handlers.CommonHandlers.ClientSettings), nameof(Handlers.CommonHandlers.ClientSettings.Start))]
        public class ClientSettings_Start
        {
            static void Postfix(Handlers.CommonHandlers.ClientSettings __instance)
            {
                GameObject contentContainer = __instance.gameObject.transform.Find("Frame (new)/Content Container")?.gameObject ?? null;
                GameObject accountContent = contentContainer?.gameObject.transform.Find("Panel - Account/Content - Account")?.gameObject ?? null;
                if (contentContainer == null)
                {
                    MelonLogger.Error("contentContainer为空");
                    return;
                }

                if (accountContent == null)
                {
                    MelonLogger.Error("accountContent为空");
                    return;
                }

                //拷贝“操作”设置中的滚动视图到Mod中
                GameObject modScrollView = contentContainer.transform.Find("Panel -  Controls - Done/Content - KbM/StandaloneContent/Keybinds/Scroll View")?.gameObject ?? null;

                if (modScrollView == null)
                {
                    MelonLogger.Error("scrollView未找到");
                    return;
                }

                modScrollView = GameObject.Instantiate(modScrollView, accountContent.transform);

                modScrollView.name = "Mod Scroll View";

                //调整大小
                {
                    RectTransform rectTransform = modScrollView.GetComponent<RectTransform>();

                    if (rectTransform == null)
                    {
                        MelonLogger.Error("rectTransform未找到");
                        return;
                    }

                    //锚点
                    rectTransform.anchorMax = new Vector2(1, 1);
                    //大小
                    rectTransform.anchoredPosition = new Vector2(-15, -80);
                    rectTransform.sizeDelta = new Vector2(43, -220);
                }

                //修改Mod Scroll View - Viewport - Content布局样式
                GameObject content = null;
                {
                    content = modScrollView.transform.Find("Viewport/Content")?.gameObject;

                    GridLayoutGroup gridLayoutGroup = content.GetComponent<GridLayoutGroup>();

                    //设置每行只能有一个元素
                    {
                        gridLayoutGroup.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
                        gridLayoutGroup.constraintCount = 1;
                    }

                    //设置单元格大小(不然会错乱格式)
                    {
                        gridLayoutGroup.cellSize = new Vector2(100, 100);
                    }
                }

                //修改Content中元素
                {
                    //克隆一个bool样式的设置选项游戏对象
                    if(IngameSettings.boolSettingPrefab == null)
                    {
                        //从“玩法设定”菜单中偷一个过来
                        GameObject fullScreenToggle = contentContainer.transform.Find("Panel -  Gameplay - Done (check fullscreen)/Content - Gameplay/Content/Output Label BG (4)/GameObject/Flash Toggle")?.gameObject ?? null;
                        boolSettingPrefab = UnityEngine.Object.Instantiate(fullScreenToggle) as GameObject;
                        boolSettingPrefab.name = "Mod's Bool Setting Prefab";

                        //移除原有功能
                        {
                            //重置Toggle组件
                            Toggle toggle = boolSettingPrefab.GetComponent<Toggle>();
                            GameObject.DestroyImmediate(toggle);
                            toggle = boolSettingPrefab.AddComponent<Toggle>();
                            toggle.graphic = boolSettingPrefab.transform.Find("Checkmark").gameObject.GetComponent<Image>();

                            //重置文字
                            Gaggle.Translation.TranslationHelper title = boolSettingPrefab.transform.Find("Title").gameObject.GetComponent<Gaggle.Translation.TranslationHelper>();
                            GameObject.DestroyImmediate(title);                            
                        }
                    }

                    //删除原有元素
                    for (int i = content.transform.childCount -1; i >= 0;i--)
                    {
                        GameObject.Destroy(content.transform.GetChild(i).gameObject);
                    }

                    //添加mod设置选项
                    foreach(IngameSettingsEntry ingameSettingsEntry in IngameSettings.registeredEntries)
                    {
                        
                        //只添加Bool类型的设置选项，因为暂时只支持开关类型的mod设置
                        if(ingameSettingsEntry.entry.BoxedValue  is bool)
                        {
                            MelonLogger.Msg(System.ConsoleColor.Green, "正在添加mod设置：" + ingameSettingsEntry.entry.DisplayName);
                            GameObject boolToggle = GameObject.Instantiate(boolSettingPrefab, content.transform);

                            //修改文字
                            {
                                TextMeshProUGUI title = boolToggle.transform.Find("Title").gameObject.GetComponent<TextMeshProUGUI>();
                                title.text = Utils.Utils.IsChineseSystem() ? ingameSettingsEntry.name_cn : ingameSettingsEntry.name_eng;
                                title.ForceMeshUpdate(true);
                            }

                            //注册点击事件
                            Toggle toggle = boolToggle.GetComponent<Toggle>();
                            {
                                toggle.onValueChanged.AddListener(
                                        new System.Action<bool>((v) => {
                                            bool newValue = toggle.isOn;
                                            ingameSettingsEntry.entry.BoxedValue = newValue;
                                        })
                                    );
                            }

                            //修改勾选值
                            {
                                toggle.isOn = (bool)ingameSettingsEntry.entry.BoxedValue;
                            }
                            
                        } else
                        {
                            MelonLogger.Warning("未添加mod设置选项，因为数据非bool类型：" + ingameSettingsEntry.entry.DisplayName);
                            continue;
                        }
                    }
                }
            }
        }
    }
}
