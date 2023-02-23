using MelonLoader;
using UnhollowerRuntimeLib;
using UnityEngine;
using System.Linq;

using IntPtr = System.IntPtr;
using GGD_Hack.Features;
using GGD_Hack.Hook;

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

        private static void RemoteKill(string[] strings)
        {
            if (strings.Length < 2)
            {
                MelonLogger.Warning("RemoteKill参数过少！");
                return;
            }

            string targetUserId = null;
            string killDelay = null;

            switch(strings.Length)
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
        private static void Suicide()
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
        private static void BindHookToSendFart(string[] strings)
        {
            if (strings.Length < 2)
            {
                MelonLogger.Warning("BindHook参数过少！");
                return;
            }
            string actionName = string.Join("\n", strings.Skip(1));

            System.Action action = null;

            switch(actionName)
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

        private static void ShowAllPlayersArrow()
        {
            UnityMainThreadDispatcher.Instance().Enqueue(new System.Action(() =>
            {
                GGD_Hack.Features.TrackAllPlayers.ShowAllPlayersArrow();
            }));
        }

        private static void SendChat(string[] strings)
        {
            if (strings.Length < 2)
            {
                MelonLogger.Warning("SendChat参数过少！");
            }
            string chatMessage = string.Join("\n", strings.Skip(1));

            UnityMainThreadDispatcher.Instance().Enqueue(new System.Action(() =>
            {
                Utils.Utils.SendTextMessage(chatMessage);
            }));
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
