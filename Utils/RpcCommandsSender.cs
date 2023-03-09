using Handlers.GameHandlers.PlayerHandlers;
using MelonLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GGD_Hack.RPC;

namespace GGD_Hack.Utils
{
    public class RpcCommandsSender
    {
        public static float lastPingTime = -1;
        public static float minPingInterval = 1.0f;

        public static void Kill(string userId)
        {
            RpcServer.SendCommand(RpcServer.RpcCommand.Kill, userId);
        }

        public static void SuperBan(string userId)
        {
            RpcServer.SendCommand(RpcServer.RpcCommand.SuperBan, userId);
        }

        public static void SendChat(string userId, string message)
        {
            RpcServer.SendCommand(RpcServer.RpcCommand.SendChat, userId, message);
        }

        public static void Ping()
        {
#if Developer
            MelonLogger.Msg(System.ConsoleColor.Green, "正在发送ping...");
#endif
            if (UnityEngine.Time.time - lastPingTime < minPingInterval)
            {
#if Developer
                MelonLogger.Msg(System.ConsoleColor.Red,"ping间隔过短");
#endif
                return;
            }
            lastPingTime = UnityEngine.Time.time;

            RpcServer.SendCommand(RpcServer.RpcCommand.Ping, LocalPlayer.Instance.Player.userId);
        }
    }
}
