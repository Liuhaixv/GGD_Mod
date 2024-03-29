﻿using MelonLoader;
using UnhollowerRuntimeLib;
using UnityEngine;
using System.Linq;

using IntPtr = System.IntPtr;
using GGD_Hack.Features;
using GGD_Hack.Hook;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;
using Handlers.LobbyHandlers;

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

        public static bool HandleMessage(System.String message)
        {

            string[] lines = message.Split(new string[] { "\n", "\r\n" }, System.StringSplitOptions.RemoveEmptyEntries);

            if (lines.Length == 0)
            {
                //空字符串
                MelonLogger.Warning("收到的指令为空！");
                return true;
            }

            string command = lines[0];

            MelonLogger.Msg("正在处理command: " + message);

            switch (command)
            {
                //绑定功能
                case "BindHookToSendFart":
                    MelonLogger.Msg("command命中: BindHookToSendFart(System.Action action)");
                    BindHookToSendFart(lines);
                    break;
                //通用功能
                case "SendFart":
                    MelonLogger.Msg("command命中: SendFart()");
                    SendFart();
                    break;
                //发送聊天消息
                case "SendChat":
                    MelonLogger.Msg("command命中: SendChat(string message)");
                    SendChat(lines);
                    break;
                case "ShowAllPlayersArrow":
                    MelonLogger.Msg("command命中: ShowAllPlayersArrow()");
                    ShowAllPlayersArrow();
                    break;
                case "MoveShuttle":
                    MelonLogger.Msg("command命中: MoveShuttle()");
                    MoveShuttle();
                    break;
                case "Suicide":
                    MelonLogger.Msg("command命中: Suicide()");
                    Suicide();
                    break;
                case "RemoteKill":
                    MelonLogger.Msg("command命中: RemoteKill(string userId)");
                    RemoteKill(lines);
                    break;
                case "UnlockAllItems":
                    MelonLogger.Msg("command命中: UnlockAllItems()");
                    UnlockAllItems_();
                    break;
                case "UpdateTempUserUnlockables":
                    MelonLogger.Msg("command命中: UpdateTempUserUnlockables()");
                    UpdateTempUserUnlockables();
                    break;
                case "RingBell":
                    MelonLogger.Msg("command命中: RingBell()");
                    RingBell();
                    break;
                case "PickUpAllBodies":
                    MelonLogger.Msg("command命中: PickUpAllBodies()");
                    PickUpAllBodies();
                    break;
                case "RemoteEat":
                    MelonLogger.Msg("command命中: RemoteEat()");
                    RemoteEat();
                    break;
                case "ThrowAwayBomb":
                    MelonLogger.Msg("command命中: ThrowAwayBomb()");
                    ThrowAwayBomb();
                    break;
                case "StartGame":
                    MelonLogger.Msg("command命中: StartGame()");
                    StartGame();
                    break;
                //Test
                case "Flip":
                    MelonLogger.Msg("command命中：Flip()");
                    Flip();
                    break;

                //静音其他玩家
                case "SilencePlayer":
                    MelonLogger.Msg("command命中: SilencePlayer(string userId)");
                    SilencePlayer(lines);
                    break;
                //测试连接
                case "TestConnection":
                    MelonLogger.Msg("测试TCP服务器连接成功");
                    return true;
                default:
                    MelonLogger.Msg("未知command指令");
                    return false;
            }
            return true;
        }

        public static void StartGame()
        {
            UnityMainThreadDispatcher.Instance().Enqueue(new System.Action(() =>
            {
                LobbySceneHandler.Instance?.StartGame();
            }));            
        }

        public static void Flip()
        {
            UnityMainThreadDispatcher.Instance().Enqueue(new System.Action(() =>
            {
                Features.Test.RPC.Flip();
            }));
        }

        public static void ThrowAwayBomb()
        {
            UnityMainThreadDispatcher.Instance().Enqueue(new System.Action(() =>
            {
                Features.Bomb.ThrowAwayBomb();
            }));
        }

        public static void RemoteEat()
        {
            UnityMainThreadDispatcher.Instance().Enqueue(new System.Action(() =>
            {
                GGD_Hack.Features.RemoteEat.EatRandomBody();
            }));
        }

        /// <summary>
        /// 捡起所有尸体
        /// </summary>
        private static void PickUpAllBodies()
        {
            UnityMainThreadDispatcher.Instance().Enqueue(new System.Action(() =>
            {
                GGD_Hack.Features.ClearAllBodies.PickUpAllBodies();
            }));
        }

        private static void RingBell()
        {
            UnityMainThreadDispatcher.Instance().Enqueue(new System.Action(() =>
            {
                PluginEventsManager.RingBell();
            }));
        }

        /// <summary>
        /// 静音所有其他玩家
        /// </summary>
        private static void SilencePlayer(string[] lines)
        {
            if (lines.Length < 2)
            {
                MelonLogger.Warning("SilencePlayer参数过少！");
                return;
            }

            string targetUserId = null;

            switch (lines.Length)
            {
                case 2:
                    targetUserId = lines[1];
                    break;
                default:
                    MelonLogger.Warning("SilencePlayer参数过多！");
                    return;
            }

            UnityMainThreadDispatcher.Instance().Enqueue(new System.Action(() =>
            {
                //远程杀人
                PluginEventsManager.Silence(targetUserId);

                MelonLogger.Msg("正在执行静音玩家任务: " + targetUserId);
            }));
        }

        private static void UpdateTempUserUnlockables()
        {
            UnityMainThreadDispatcher.Instance().Enqueue(new System.Action(() =>
            {
                ShowAllUnlockables.UpdateTempUserUnlockables();
            }));
        }

        private static void UnlockAllItems_()
        {
            //TODO: 目前默认启用解锁所有物品功能
            ShowAllUnlockables.Enabled.Value = true;
        }

        private static void RemoteKill(string[] strings)
        {
            if (strings.Length < 2)
            {
                MelonLogger.Warning("RemoteKill参数过少！");
                return;
            }

            string targetUserId = null;
            string killDelay = null;

            switch (strings.Length)
            {
                case 2:
                    targetUserId = strings[1];
                    break;
                case 3:
                    targetUserId = strings[1];
                    killDelay = strings[2];
                    break;
                default:
                    MelonLogger.Warning("RemoteKill参数过多！");
                    return;
            }

            UnityMainThreadDispatcher.Instance().Enqueue(new System.Action(() =>
            {
                //远程杀人
                bool result = Features.RemoteKillPlayer.TeleportAndKill(targetUserId, killDelay);

                if (result)
                {
                    MelonLogger.Msg("正在进行远程击杀任务");
                }
                else
                {
                    MelonLogger.Warning("远程击杀失败！");
                }
            }));
        }

        /// <summary>
        /// 自杀
        /// </summary>
        /// <exception cref="System.NotImplementedException"></exception>
        public static void Suicide()
        {
            UnityMainThreadDispatcher.Instance().Enqueue(new System.Action(() =>
            {
                MiscFunctions.Suicide();
            }));
        }

        /// <summary>
        /// 绑定hook函数执行的方法
        /// </summary>
        /// <param name="command"></param>
        /// <exception cref="System.NotImplementedException"></exception>
        public static void BindHookToSendFart(string[] strings)
        {
            if (strings.Length < 2)
            {
                MelonLogger.Warning("BindHook参数过少！");
                return;
            }
            string actionName = string.Join("\n", strings.Skip(1));

            System.Action action = null;

            switch (actionName)
            {
                case "MoveShuttle":
                    action = new System.Action(() =>
                    {
                        UnityMainThreadDispatcher.Instance().Enqueue(new System.Action(() =>
                        {
                            MiscFunctions.MoveShuttle();
                        }));
                    });

                    break;
                default:
                    MelonLogger.Warning("未知Action name!");
                    break;
            }

            SendFartHook.bindAction(action);
        }

        public static void MoveShuttle()
        {
            UnityMainThreadDispatcher.Instance().Enqueue(new System.Action(() =>
            {
                MiscFunctions.MoveShuttle();
            }));
        }

        public static void ShowAllPlayersArrow()
        {
            UnityMainThreadDispatcher.Instance().Enqueue(new System.Action(() =>
            {
                GGD_Hack.Features.TrackAllPlayers.ShowAllPlayersArrow();
            }));
        }

        public static void SendChat(string message)
        {
            if (message == null)
            {
                MelonLogger.Warning("聊天消息不能为空!");
            }

            UnityMainThreadDispatcher.Instance().Enqueue(new System.Action(() =>
            {
                Utils.Utils.SendTextMessage(message);
            }));
        }

        public static void SendChat(string[] strings)
        {
            if (strings.Length < 2)
            {
                MelonLogger.Warning("SendChat参数过少！");
            }
            string chatMessage = string.Join("\n", strings.Skip(1));

            SendChat(chatMessage);
        }

        public static void SendFart()
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
