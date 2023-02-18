using Handlers.GameHandlers.PlayerHandlers;
using MelonLoader;
using System;
using UnhollowerRuntimeLib;
using UnityEngine;

namespace GGD_Hack
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
            Handlers.GameHandlers.PlayerHandlers.LocalPlayer localPlayer = Utils.GameInstances.GetLocalPlayer();
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
    }
}
