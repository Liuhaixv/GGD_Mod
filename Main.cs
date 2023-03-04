using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Crmf;
using GGD_Hack.Features;
using GGD_Hack.Hook;
using GGD_Hack.Utils;
using Il2CppSystem.Security.Cryptography;
using MelonLoader;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Windows.Forms;
using UnityEngine;

namespace GGD_Hack
{
    public static class BuildInfo
    {
#if Developer
        public const string Name = "[开发者专用版] Liuhaixv's GGD_Hack mod"; // Name of the Mod.  (MUST BE SET)
#else
        public const string Name = "Liuhaixv's GGD_Hack mod"; // Name of the Mod.  (MUST BE SET)
#endif
        public const string Description = "免费mod辅助 Free Mod for cheating"; // Description for the Mod.  (Set as null if none)
        public const string Author = "Liuhaixv"; // Author of the Mod.  (MUST BE SET)
        public const string Company = "Liuhaixv"; // Company that made the Mod.  (Set as null if none)
        public const string Version = "1.4.0"; // Version of the Mod.  (MUST BE SET)
        public const string gameVersion = "2.18.00.02";//version of the GGD
        public const string DownloadLink = "https://github.com/Liuhaixv/GGDH_ML"; // Download Link for the Mod.  (Set as null if none)
    }

    public class GGDHack_Mod : MelonMod
    {
        public override void OnInitializeMelon()
        {
            MelonLogger.Msg("OnInitializeMelon");
            //BypassAC.PatchAssembliesLoadCheck();

            //配置持久化
            MelonPreferences.CreateCategory("GGDH");
        }
        public override void OnLateInitializeMelon() // Runs after Game Initialization.
        {
            MelonLogger.Msg("OnLateInitializeMelon");

            TCPTestServer testServer = new TCPTestServer(29241);
            testServer.Start();

            CheckModVersion();
            WarningFree();
        }

        public override void OnSceneWasLoaded(int buildindex, string sceneName) // Runs when a Scene has Loaded and is passed the Scene's Build Index and Name.
        {
            //MelonLogger.Msg("OnSceneWasLoaded: " + buildindex.ToString() + " | " + sceneName);
            Init();
        }

        public override void OnSceneWasInitialized(int buildindex, string sceneName) // Runs when a Scene has Initialized and is passed the Scene's Build Index and Name.
        {
            //MelonLogger.Msg("OnSceneWasInitialized: " + buildindex.ToString() + " | " + sceneName);
        }

        public override void OnUpdate() // Runs once per frame.
        {
            //MelonLogger.Msg("OnUpdate");
        }

        public override void OnFixedUpdate() // Can run multiple times per frame. Mostly used for Physics.
        {
            //MelonLogger.Msg("OnFixedUpdate");
        }

        public override void OnLateUpdate() // Runs once per frame after OnUpdate and OnFixedUpdate have finished.
        {
            //MelonLogger.Msg("OnLateUpdate");
        }

        public override void OnGUI() // Can run multiple times per frame. Mostly used for Unity's IMGUI.
        {
            //MelonLogger.Msg("OnGUI");
        }

        public override void OnApplicationQuit() // Runs when the Game is told to Close.
        {
            //MelonLogger.Msg("OnApplicationQuit");
        }

        public override void OnPreferencesSaved() // Runs when Melon Preferences get saved.
        {
            //MelonLogger.Msg("OnPreferencesSaved");
        }

        public override void OnPreferencesLoaded() // Runs when Melon Preferences get loaded.
        {
            //MelonLogger.Msg("OnPreferencesLoaded");
        }

        /// <summary>
        /// 初始化所有需要用到的实例
        /// </summary>
        private void Init()
        {
            CommandHandler.Init();

            //Unity主线程调度
            UnityMainThreadDispatcher.Init();

            //初始化小地图点位绘制
            MinimapESP.Init();
            //初始化小地图传送
            MinimapTeleport.Init();
            //初始化远程杀人
            RemoteKillPlayer.Init();
            //初始化小地图坐标记录器
            MinimapRecorder.Init();
            //随机加入房间
            RandomJoinRoom.Init();

            UnlockAllItems.Init();

            SendFartHook.bindAction(CommandHandler.MoveShuttle);
        }

        private void CheckModVersion()
        {
            //检查游戏版本
            {
                // Pause the game
                Time.timeScale = 0;

                if (BuildInfo.gameVersion != UnityEngine.Application.version)
                {
                    string eng = "Mod works only game version:" + BuildInfo.gameVersion +
                        "\nCurrent game version:" + UnityEngine.Application.version +
                        "\nMod out dated already!";
                    string cn = "Mod对应游戏版本:" + BuildInfo.gameVersion +
                        "\n当前游戏版本:" + UnityEngine.Application.version +
                        "\nMod已过期!";

                    string show = IsChineseSystem() ? cn : eng;

                    MyForms.MyMessageBox.Show(show);
                    UnityEngine.Application.Quit();
                }

                // Resume the game
                Time.timeScale = 1;
            }
        }

        /// <summary>
        /// 弹出免费警告
        /// </summary>
        private void WarningFree()
        {
            // Pause the game
            Time.timeScale = 0;

            //检查是否警告过
            //将游戏版本号和mod版本号相加后md5存储在Melon配置
            if (!HasWarnedFree)
            {
                string eng = "This mod was developed by Liuhaixv@github.com\n" +
                    "The mod is free, if you bought it somewhere, you are scammed";
                string cn = "本mod由Liuhaixv@github.com开发\n" +
                    "该mod完全免费，如果你从别处购买到，那你就是大冤种";
                string show = IsChineseSystem() ? cn : eng;
                MyForms.MyMessageBox.Show(show);
                HasWarnedFree = true;
            }

            // Resume the game
            Time.timeScale = 1;
        }


        /// <summary>
        /// 是否已经警告过
        /// </summary>
        private bool HasWarnedFree
        {
        get
            {
                string key = nameof(HasWarnedFree);
                string correctMd5 = MD5Util.GetMd5Hash(BuildInfo.Version + BuildInfo.gameVersion + UnityEngine.Application.version);
                bool result = false;

                MelonPreferences_Entry<string> value = MelonPreferences.GetEntry<string>("GGDH", key);
                if (value == null)
                {
                    value = MelonPreferences.CreateEntry<string>("GGDH", key, "");
                    result =  false;
                }
                result = correctMd5 == value.Value;
#if Developer
                MelonLogger.Msg("HasWarnedFree正确的版本MD5为:" + correctMd5);
                MelonLogger.Msg("HasWarnedFree配置读取的MD5为:" + value.Value);
                MelonLogger.Msg("验证结果:" + result);
#endif
                return result;
            }
            set
            {
                string key = nameof(HasWarnedFree);
                MelonLogger.Msg("正在设置已提示过免费警告:" + key);
                MelonPreferences_Entry<string> melonPreferences_Entry = MelonPreferences.GetEntry<string>("GGDH", key);
                if (melonPreferences_Entry == null)
                {
                    melonPreferences_Entry = MelonPreferences.CreateEntry<string>("GGDH", key, "");
                }

                if (value == true)
                {
                    string md5 = MD5Util.GetMd5Hash(BuildInfo.Version + BuildInfo.gameVersion + UnityEngine.Application.version);
                    melonPreferences_Entry.Value = md5;
                }
                else
                {
                    melonPreferences_Entry.Value = "";
                }
            }
        }

        private bool IsChineseSystem()
        {
            SystemLanguage systemLanguage = UnityEngine.Application.systemLanguage;
#if Developer
            MelonLogger.Msg("当前系统语言:" + systemLanguage);
#endif
            if (systemLanguage == SystemLanguage.Chinese ||
                systemLanguage == SystemLanguage.ChineseSimplified ||
                systemLanguage == SystemLanguage.ChineseTraditional)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}