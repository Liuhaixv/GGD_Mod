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
using APIs.RestClient.DemoScene.Scripts.Model;
using System.Text.RegularExpressions;
using static System.Net.WebRequestMethods;
using System.Diagnostics;
using System.IO;
using System.Security.Policy;

namespace GGD_Hack.Features
{
    public class AutoUpdate
    {
        private static string GithubLatestReleaseUrl = "https://github.com/Liuhaixv/GGDH_ML/releases/latest";
        private static bool hasIgnoredUpdate = false;
        private static string modFileName = "GGD_Hack_Mod.dll";

        private static string latestVersionFromGithub = null;
        public static async void CheckLatestModVersion()
        {
            if (hasIgnoredUpdate) return;

            if (latestVersionFromGithub == null)
            {
                GetLatestVersionFromGithub(true);
            }
            else
            {
                CheckLocalVersionAndPrompt();
            }
        }

        private static void GetLatestVersionFromGithub(bool checkUpdate = false)
        {
            UnityWebRequest www = UnityWebRequest.Get(GithubLatestReleaseUrl);
            UnityWebRequestAsyncOperation unityWebRequestAsyncOperation = www.SendWebRequest();

            unityWebRequestAsyncOperation.add_completed(
                new System.Action<AsyncOperation>((operation) =>
                {
                    //已经获取过更新数据
                    if (latestVersionFromGithub != null)
                    {
                        return;
                    }

                    if (www.result == UnityWebRequest.Result.Success)
                    {
                        //获取github提供的重定向路径
                        //location: https://github.com/Liuhaixv/GGDH_ML/releases/tag/v1.5.2.1
                        string latestReleaseRedirectedUrl = www.url;

                        //从重定向路径中获取版本号
                        MelonLogger.Msg(System.ConsoleColor.Green, "latestReleaseRedirectedUrl: " + latestReleaseRedirectedUrl);

                        Match match = Regex.Match(latestReleaseRedirectedUrl, @"\/tag\/v(?<version>[\d\.]+)$");
                        if (match.Success)
                        {
                            latestVersionFromGithub = match.Groups["version"].Value;
                            MelonLogger.Msg(System.ConsoleColor.Green, "Latest version: " + latestVersionFromGithub);

                            if (checkUpdate)
                            {
                                CheckLocalVersionAndPrompt();
                            }
                            return;
                        }
                        else
                        {
                            MelonLogger.Error("Failed to extract version number from github latest release's redirect URL");
                        }
                    }
                    else
                    {
                        MelonLogger.Error("Error getting latest mod version: " + www.error);
                    }
                })
                );
        }

        private static void CheckLocalVersionAndPrompt()
        {
            //旧版本
            Version latestVersion = new Version(latestVersionFromGithub);
            Version localVersion = new Version(BuildInfo.Version);

            //检查是否是旧版
            if (localVersion.CompareTo(latestVersion) < 0)
            {
                MelonLogger.Msg(System.ConsoleColor.Green, "New version available: " + latestVersion.ToString());
                MelonLogger.Msg(System.ConsoleColor.Green, "检测到mod新版本可用: " + latestVersion.ToString());
                OpenUpdatePrompt(latestVersion.ToString());
            }
            else
            {
                if (!hasIgnoredUpdate)
                {
                    MelonLogger.Msg(System.ConsoleColor.Green, "当前已是最新版本mod: " + latestVersion.ToString());
                    hasIgnoredUpdate = true;
                }
            }
        }

        private static void OpenUpdatePrompt(string latestModVersion)
        {
            bool isChineseSystem = Utils.Utils.IsChineseSystem();

            string title = isChineseSystem ? "更新可用" : "Update mod";
            StringBuilder content = new StringBuilder();
            content.AppendLine(string.Format("<color=green>{0}</color>", isChineseSystem ? "新版本mod已可用!" : "New version of Mod is available"));
            content.AppendLine(string.Format("{0}: <color=yellow>{1}</color>", isChineseSystem ? "当前版本" : "Current version", BuildInfo.Version));
            content.AppendLine(string.Format("{0}: <color=green>{1}</color>", isChineseSystem ? "最新版本" : "Latest version", latestModVersion));

            //content.AppendLine(isChineseSystem ? "<align=left>或许你不喜欢频繁下载新的版本，但更新将获得包括但不限于以下好处：\n<color=green>新功能</color>、更好的<color=green>稳定性</color>、更好的<color=green>性能表现</color>、旧版本bug的<color=green>修复</color>...</align>" : "<align=left>Maybe you don't like downloading new versions frequently, but updating will bring you benefits including but not limited to:\n<color=green>New features</color>, better <color=green>stability</color>, better <color=green>performance</color>, and <color=green>bug fixes</color> for the old version...</align>");

            string rightButtonText = (isChineseSystem ? "立即下载" : "Update");

            //跳转下载mod地址
            UnityAction rightAction = new System.Action(() =>
            {
                string downloadUrl = string.Format(@"https://github.com/Liuhaixv/GGDH_ML/releases/download/v{0}/{1}", latestVersionFromGithub, modFileName);
                string modPath = System.Reflection.Assembly.GetExecutingAssembly().Location;
                OpenUpdateDownloadPanel(downloadUrl, modPath);
            });

            if (GlobalPanelsHandler.Instance == null)
            {
                MelonLogger.Error("GlobalPanelsHandler.Instance is null");
                return;
            }

            GlobalPanelsHandler.Instance.OpenOneButtonPromptPanel(
                title,
               content.ToString(),
                rightButtonText,
                rightAction);
        }

        /// <summary>
        /// 显示一个下载面板，自动下载url对应的文件，显示下载进度，在下载完成后将文件复制并覆盖到localPath的位置，然后自动重启游戏
        /// </summary>
        /// <param name="url"></param>
        /// <param name="localPath"></param>
        public static void OpenUpdateDownloadPanel(string url, string localPath, bool downloadFinished = false)
        {
            //实例未初始化
            if (GlobalPanelsHandler.Instance == null)
            {
                return;
            }

            bool isChineseSystem = Utils.Utils.IsChineseSystem();

            string title = null;

            //标题
            if (!downloadFinished)
            {
                title = isChineseSystem ? "正在下载更新中..." : "Downloading...";
            }
            else
            {
                title = isChineseSystem ? "更新完毕" : "Updated";
            }

            //正文
            StringBuilder content = new StringBuilder();
            if (!downloadFinished)
            {

                content.AppendLine(string.Format("{0}<color=yellow>{1}</color>",
                                        isChineseSystem ? "当前版本:" : "Current Version:",
                                        BuildInfo.Version)
                            );
                content.AppendLine(string.Format("{0}<color=green>{1}</color>",
                                                isChineseSystem ? "最新版本:" : "Latest Version:",
                                                latestVersionFromGithub)
                                    );

                content.AppendLine(string.Format("<align=left>{0}</align>",
                                               isChineseSystem ? "正在后台下载最新mod中...如果长时间未下载完毕有可能是加速器问题，可以考虑手动下载替换mod" : "Downloading latest mod in the background...")
                                   );

                content.AppendLine(string.Format("<align=left>{0}</align>",
                                    isChineseSystem ? "下载完毕后mod将自动替换旧版文件，重启游戏即可" : "The latest mod will automatically replace the old one, and the game needs to be restarted to take effect then")
                                    );
            }
            else
            {
                content.AppendLine(string.Format("<color=green>{0}</color>{1}",
                                    isChineseSystem ? "当前已是最新版本:" : "Mod is up to date now:",
                                    latestVersionFromGithub)
                        );
                content.AppendLine(string.Format("<size=150%><color=green>{0}</color></size>",
                                isChineseSystem ? "最新mod已经下载完毕！\n重启游戏后生效" : "The latest mod has been downloaded!\nRestart game to take effect")
                                );
            }

            string rightButtonText = null;
            if (!downloadFinished)
            {
                rightButtonText = isChineseSystem ? "正在下载中...请等待" : "Downloading...Please wait";
            }
            else
            {
                rightButtonText = isChineseSystem ? "关闭游戏" : "Quit game";
            }

            //跳转下载mod地址
            UnityAction rightAction = new System.Action(() =>
            {
                if (downloadFinished)
                {
                    QuitGame();
                }
            });

            GlobalPanelsHandler.Instance.OpenOneButtonPromptPanel(
                title,
               content.ToString(),
                rightButtonText,
                rightAction);

            //开始后台下载
            if (!downloadFinished)
            {
                DownloadLatestModAndReplaceOldMod(url, localPath, new System.Action(() =>
                {
                    OpenUpdateDownloadPanel("", "", true);
                }));
            }
        }

        private static void DownloadLatestModAndReplaceOldMod(string url, string localFilePath, System.Action onCompleted)
        {
            UnityWebRequest www = UnityWebRequest.Get(url);
            UnityWebRequestAsyncOperation unityWebRequestAsyncOperation = www.SendWebRequest();

            unityWebRequestAsyncOperation.add_completed(
                new System.Action<AsyncOperation>((operation) =>
                {
                    if (www.result == UnityWebRequest.Result.Success)
                    {
                        MelonLogger.Msg(System.ConsoleColor.Green, "最新版mod已经下载完成");
                        byte[] bytes = www.downloadHandler.data;
                        try
                        {
                            //检查是否存在dll，并重命名为DeleteMePls
                            if (System.IO.File.Exists(localFilePath))
                            {
                                string dstFilePath = Path.Combine(Path.GetDirectoryName(localFilePath), "DeleteMePls");

                                //如果存在同名文件就删除
                                if (System.IO.File.Exists(dstFilePath))
                                {
                                    System.IO.File.Delete(dstFilePath);
                                }

                                System.IO.File.Move(localFilePath, dstFilePath);
                            }

                            string newFilePath = Path.Combine(Path.GetDirectoryName(localFilePath), modFileName);
                            System.IO.File.WriteAllBytes(newFilePath, bytes);
                            MelonLogger.Msg(System.ConsoleColor.Green, "最新版mod已经替换了旧版mod文件！" + newFilePath);
                            onCompleted.Invoke();
                        }
                        catch (Exception ex)
                        {
                            MelonLogger.Error("最新版mod已下载，但是替换mod时出错:" + ex.Message);
                        }
                    }
                    else
                    {
                        MelonLogger.Error("下载最新版mod出错...请检查加速器是否正常工作: " + www.error);
                        OpenDownloadFailedPanel();
                    }
                })
                );
        }

        //下载失败
        public static void OpenDownloadFailedPanel()
        {
            //实例未初始化
            if (GlobalPanelsHandler.Instance == null)
            {
                return;
            }

            bool isChineseSystem = Utils.Utils.IsChineseSystem();

            string title = null;

            //标题
            title = isChineseSystem ? "下载失败" : "Download Failed";


            //正文
            StringBuilder content = new StringBuilder();

            content.AppendLine(string.Format("{0}",
                                            isChineseSystem ? "下载失败！请手动下载最新mod替换..." : "Download failed! Please download latest mod manually")
                                );

            string rightButtonText = null;

            rightButtonText = isChineseSystem ? "手动下载" : "Download";

            //跳转下载mod地址
            UnityAction rightAction = new System.Action(() =>
            {
                System.Diagnostics.Process.Start("https://github.com/Liuhaixv/GGDH_ML/releases");
                QuitGame();
            });

            GlobalPanelsHandler.Instance.OpenOneButtonPromptPanel(
                title,
               content.ToString(),
                rightButtonText,
                rightAction);
        }

        private static void QuitGame()
        {
            //保存所有未保存的游戏数据
            PlayerPrefs.Save();
            Application.Quit();

            // C:/Program Files (x86)/Steam/steamapps/common/Goose Goose Duck/Goose Goose Duck_Data
#if Developer
            //MelonLogger.Msg(System.ConsoleColor.Green, "RestartGame()获取dataPath:" + Application.dataPath);
#endif
        }

        [HarmonyPatch(typeof(SceneManager), nameof(SceneManager.Internal_SceneLoaded))]
        class SceneManager_
        {
            static void Postfix(Scene scene)
            {
                MelonLogger.Msg("正在检查版本更新");
                //检查mod更新
                AutoUpdate.CheckLatestModVersion();
            }
        }
    }
}