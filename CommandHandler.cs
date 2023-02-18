﻿using Handlers.GameHandlers.PlayerHandlers;
using Il2CppSystem;
using MelonLoader;
using System;
using UnhollowerRuntimeLib;
using UnityEngine;
using System.Linq;

namespace GGD_Hack
{
    [RegisterTypeInIl2Cpp]
    public class CommandHandler : MonoBehaviour
    {
        private static CommandHandler instance = null;

        public CommandHandler(System.IntPtr ptr) : base(ptr) { }

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

        public static void HandleMessage(System.String message)
        {

            string[] lines = message.Split(new string[] { "\n", "\r\n" }, System.StringSplitOptions.RemoveEmptyEntries);

            if (lines.Length == 0)
            {
                //空字符串
                MelonLogger.Warning("收到的指令为空！");
                return;
            }

            string command = lines[0];

            MelonLogger.Msg("正在处理command: " + message);

            switch (command)
            {
                case "SendFart":
                    MelonLogger.Msg("command命中: SendFart()");
                    SendFart();
                    break;
                case "SendChat":
                    MelonLogger.Msg("command命中: SendChat()");
                    SendChat(lines);
                    break;
                default:
                    MelonLogger.Msg("未知command指令");
                    break;
            }
        }

        private static void SendChat(string[] strings)
        {
            if (strings.Length <= 1)
            {
                MelonLogger.Warning("SendChat参数过少！");
            }
            string chatMessage = string.Join("\n", strings.Skip(1));

            UnityMainThreadDispatcher.Instance().Enqueue(new System.Action(() => { Utils.Utils.SendTextMessage(chatMessage); }));
        }

        private static void SendFart()
        {
            Handlers.GameHandlers.PlayerHandlers.LocalPlayer localPlayer = Utils.GameInstances.GetLocalPlayer();
            if (localPlayer != null)
            {
                MelonLogger.Msg(localPlayer.Player.nickname);

                //localPlayer.SendFart();
                UnityMainThreadDispatcher.Instance().Enqueue(new System.Action(localPlayer.SendFart));
            }
        }
    }
}
