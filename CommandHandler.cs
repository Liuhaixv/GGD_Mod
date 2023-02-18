using Handlers.GameHandlers.PlayerHandlers;
using MelonLoader;
using System;
using UnhollowerRuntimeLib;
using UnityEngine;
using System.Reflection;
using UnityEngine.PlayerLoop;
using static MelonLoader.MelonLogger;

namespace TestMod
{
    [RegisterTypeInIl2Cpp]
    public class CommandHandler : MonoBehaviour
    {
        private static CommandHandler instance = null;

        public CommandHandler(IntPtr ptr) : base(ptr) { }

        // Optional, only used in case you want to instantiate this class in the mono-side
        // Don't use this on MonoBehaviours / Components!
        public CommandHandler() : base(ClassInjector.DerivedConstructorPointer<CommandHandler>()) => ClassInjector.DerivedConstructorBody(this);

        public static void Init()
        {
            GameObject ML_Manager = GameObject.Find("ML_Manager");
            if (ML_Manager == null)
            {
                ML_Manager = new GameObject("ML_Manager");
                DontDestroyOnLoad(ML_Manager);
            }

            if (ML_Manager.GetComponent<CommandHandler>() == null)
            {

                instance = ML_Manager.AddComponent<CommandHandler>();
            }

        }

        public void Update()
        {
            //Handlers.GameHandlers.PlayerHandlers.LocalPlayer localPlayer = GetLocalPlayer();
            //localPlayer.SendFart();
        }

        public static void Handle(System.String command)
        {
            MelonLogger.Msg("正在处理command: " + command);

            if (command == "sendfart")
            {
                MelonLogger.Msg("command命中: SendFart()");
                SendFart();
            }
        }

        private static void SendFart()
        {
            Handlers.GameHandlers.PlayerHandlers.LocalPlayer localPlayer = GetLocalPlayer();
            if (localPlayer != null)
            {
                MelonLogger.Msg(localPlayer.Player.nickname);

                //localPlayer.SendFart();
                UnityMainThreadDispatcher unityMainThreadDispatcher = UnityMainThreadDispatcher.Instance();
                if (unityMainThreadDispatcher != null)
                {
                    unityMainThreadDispatcher.Enqueue(new System.Action(
                        () =>
                        {
                            localPlayer.SendFart();
                        }
                    ));
                }
            }
        }

        //获取LocalPlayer
        static Handlers.GameHandlers.PlayerHandlers.LocalPlayer GetLocalPlayer()
        {
            //通过tag查找玩家
            GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("Player");

            GameObject player = null;
            foreach (GameObject gameObject in gameObjects)
            {
                if (gameObject.GetComponent<Handlers.GameHandlers.PlayerHandlers.LocalPlayer>() == null)
                {
                    continue;
                }
                else
                {
                    player = gameObject;
                    break;
                }
            }

            //未找到玩家实例
            if (player == null)
            {
                return null;
            }

            return player.GetComponent<Handlers.GameHandlers.PlayerHandlers.LocalPlayer>();
        }
    }
}
