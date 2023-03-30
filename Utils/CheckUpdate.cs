#if false
using System;
using BestHTTP;
using UnityEngine.Networking;
using Handlers.CommonHandlers.UIHandlers;
using MelonLoader;
using Newtonsoft.Json;
using UnityEngine.Events;
using UnityEngine;
using System.Text;
using HarmonyLib;
using GGD_Hack.Features;
using UnityEngine.SceneManagement;

public class ModVersionInfo
{
    public string ModVersion { get; set; }
    public string ForceUpdateVersionsOlderThan { get; set; }
}

namespace GGD_Hack.Utils
{
    public class CheckUpdate
    {
        private static string LatestModVersionJsonUrl = "https://textbin.net/raw/yoazqk3ggf";
        private static bool hasIgnoredUpdate = false;
        private static ModVersionInfo latestVersionInfo = null;
        public static async void CheckLatestModVersion()
        {
            if (hasIgnoredUpdate) return;
#if Developer
            WriteCurrentModVersionJsonToDisk();
#endif
            if (latestVersionInfo != null)
            {
                CheckLocalVersionAndPrompt();
                return;
            }

            UnityWebRequest www = UnityWebRequest.Get(LatestModVersionJsonUrl);
            UnityWebRequestAsyncOperation unityWebRequestAsyncOperation = www.SendWebRequest();

            unityWebRequestAsyncOperation.add_completed(
                new System.Action<AsyncOperation>((operation) =>
                {
                    if (!operation.isDone) return;

                    if (www.result == UnityWebRequest.Result.Success)
                    {
                        if (latestVersionInfo != null)
                        {
                            return;
                        }

                        string response = www.downloadHandler.text;
                        MelonLogger.Msg("获取到最新版本信息:" + response);
                        latestVersionInfo = JsonConvert.DeserializeObject<ModVersionInfo>(response);

                        CheckLocalVersionAndPrompt();
                    }
                    else
                    {
                        MelonLogger.Msg(System.ConsoleColor.Red, "Error getting latest mod version: " + www.error);
                    }
                })
                );
        }

        public static void WriteCurrentModVersionJsonToDisk()
        {
            string currentModVersion = BuildInfo.Version;

            // 创建ModVersionInfo对象并设置LatestVersion属性
            ModVersionInfo versionInfo = new ModVersionInfo();
            versionInfo.ModVersion = currentModVersion;

            // 序列化ModVersionInfo为JSON字符串
            string json = JsonConvert.SerializeObject(versionInfo);

            // 获取应用程序集的目录，并将JSON字符串写入文件
            string filePath = MelonHandler.ModsDirectory + "/" + "mod-version.json";
            System.IO.File.WriteAllText(filePath, json);
        }

        private static void CheckLocalVersionAndPrompt()
        {
            //旧版本
            Version latestVersion = new Version(latestVersionInfo.ModVersion);
            Version atLeastVersion = new Version(latestVersionInfo.ForceUpdateVersionsOlderThan ?? "0.0.0");
            Version localVersion = new Version(BuildInfo.Version);

            //检查是否是旧版
            if (localVersion.CompareTo(latestVersion) < 0)
            {
                MelonLogger.Msg(System.ConsoleColor.Green, "New version available: " + latestVersionInfo.ModVersion);
                MelonLogger.Msg(System.ConsoleColor.Green, "检测到mod新版本可用: " + latestVersionInfo.ModVersion);

                //检查是否需要强制更新
                if (localVersion.CompareTo(atLeastVersion) < 0)
                {
                    OpenUpdatePrompt(latestVersionInfo.ModVersion, true);
                }
                else
                {
                    OpenUpdatePrompt(latestVersionInfo.ModVersion, false);
                }
            }
            else
            {
                hasIgnoredUpdate = true;
                MelonLogger.Msg(System.ConsoleColor.Green, "当前已是最新版本mod: " + BuildInfo.Version);
            }
        }

        private static void OpenUpdatePrompt(string latestModVersion, bool forceUpdate = false)
        {
            bool isChineseSystem = Utils.IsChineseSystem();

            string title = isChineseSystem ? "更新可用" : "Update mod";
            StringBuilder content = new StringBuilder();
            content.AppendLine(string.Format("<color=green>{0}</color>", isChineseSystem ? "新版本mod已可用!" : "New version of Mod is available"));
            content.AppendLine(string.Format("{0}: <color=yellow>{1}</color>", isChineseSystem ? "当前版本" : "Current version", BuildInfo.Version));
            content.AppendLine(string.Format("{0}: <color=green>{1}</color>", isChineseSystem ? "最新版本" : "Latest version", latestModVersion));
            if (forceUpdate)
            {
                if (isChineseSystem)
                    content.AppendLine("<align=left>检测到本地Mod版本过旧，本次Mod版本更新为<color=red>强制更新</color>，这意味着最新版本修复了一些严重漏洞或发布了一些重要功能...</align>");
                else
                    content.AppendLine("<align=left>Detected that the local Mod version is too old, this Mod version update is <color=red>mandatory</color>, which means that the latest version has fixed some serious vulnerabilities or released some important features...</align>");
            }
            content.AppendLine(isChineseSystem ? "<align=left>或许你不喜欢频繁下载新的版本，但更新将获得包括但不限于以下好处：\n<color=green>新功能</color>、更好的<color=green>稳定性</color>、更好的<color=green>性能表现</color>、旧版本bug的<color=green>修复</color>...</align>" : "<align=left>Maybe you don't like downloading new versions frequently, but updating will bring you benefits including but not limited to:\n<color=green>New features</color>, better <color=green>stability</color>, better <color=green>performance</color>, and <color=green>bug fixes</color> for the old version...</align>");

            string leftButtonText = (isChineseSystem ? "忽略更新 [不推荐]" : "Ignore [Are u sure?]");
            string rightButtonText = (isChineseSystem ? "立即下载" : "Update");

            UnityAction leftAction = new System.Action(() =>
            {
                hasIgnoredUpdate = true;
                GlobalPanelsHandler.Instance.ClosePanels();
            });

            //跳转下载mod地址
            UnityAction rightAction = new System.Action(() =>
            {
#if Developer
                System.Diagnostics.Process.Start("https://textbin.net/yoazqk3ggf");
#else
                System.Diagnostics.Process.Start("https://github.com/Liuhaixv/GGDH_ML/releases");
#endif
            });

            if (forceUpdate)
            {
                GlobalPanelsHandler.Instance.OpenOneButtonPromptPanel(
                    title,
                   content.ToString(),
                    rightButtonText,
                    rightAction);
            }
            else
            {
                GlobalPanelsHandler.Instance.OpenPromptPanel(
                        title,
                       content.ToString(),
                        leftButtonText,
                        leftAction,
                        rightButtonText,
                        rightAction
                    );
            }
        }

        [HarmonyPatch(typeof(SceneManager), nameof(SceneManager.Internal_SceneLoaded))]
        class SceneManager_
        {
            static void Postfix(Scene scene)
            {
                MelonLogger.Msg("正在检查版本更新");
                //检查mod更新
                CheckUpdate.CheckLatestModVersion();
            }
        }
    }
}
#endif