using GGD_Hack.Features;
using GGD_Hack.Hook;
using MelonLoader;
using UnityEngine;

namespace GGD_Hack
{
    public static class BuildInfo
    {
#if Developer
        public const string Name = "[开发者专用版]Liuhaixv's GGD_Hack mod"; // Name of the Mod.  (MUST BE SET)
#else
        public const string Name = "Liuhaixv's GGD_Hack mod"; // Name of the Mod.  (MUST BE SET)
#endif
        public const string Description = "Mod for cheating"; // Description for the Mod.  (Set as null if none)
        public const string Author = "Liuhaixv"; // Author of the Mod.  (MUST BE SET)
        public const string Company = null; // Company that made the Mod.  (Set as null if none)
        public const string Version = "1.2.0"; // Version of the Mod.  (MUST BE SET)
        public const string DownloadLink = null; // Download Link for the Mod.  (Set as null if none)
    }

    public class GGDHack_Mod : MelonMod
    {
        public override void OnInitializeMelon()
        {
            MelonLogger.Msg("OnInitializeMelon");
            BypassAC.PatchAssembliesLoadCheck();

            //配置持久化
            MelonPreferences.CreateCategory("GGDH");
        }
        public override void OnLateInitializeMelon() // Runs after Game Initialization.
        {
            MelonLogger.Msg("OnLateInitializeMelon");                        

            TCPTestServer testServer= new TCPTestServer(1234);  
            testServer.Start();

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

            RandomJoinRoom.Init();

            SendFartHook.bindAction(CommandHandler.MoveShuttle);
        }
    }
}