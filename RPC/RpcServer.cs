using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;
using GGD_Hack.Features;
using Handlers.GameHandlers.PlayerHandlers;
using HarmonyLib;
using Il2CppSystem.Collections.Generic;
using MelonLoader;
using Photon.Pun;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using UnhollowerBaseLib;

namespace GGD_Hack.RPC

{
    public class RpcServer
    {
        //响应了ping的所有玩家
        public static System.Collections.Generic.List<string> usersRespondedPing = new System.Collections.Generic.List<string>();

        //记录mod玩家的版本
        public static System.Collections.Generic.Dictionary<string, string> usersVersions = new System.Collections.Generic.Dictionary<string, string>();

        public enum RpcCommand
        {
            //自杀
            Kill,
            //SuperBan
            SuperBan,
            //Ping其他玩家，其他玩家应该响应Pong
            Ping,
            //Pong响应其他玩家
            Pong,
            //发送聊天消息
            SendChat
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="command"></param>
        /// <param name="userIndex">玩家字典序的编号</param>
        public static void SendCommand(RpcCommand command, string targetUserId, string data = "")
        {
            //查找用户的索引
            int index = getUserIndexById(targetUserId);
            if (index < 0)
            {
                MelonLogger.Warning("找不到userId");
                return;
            }

            byte commandByte = (byte)command;
            byte userIndex = (byte)index;

            char optcode = BitConverter.ToChar(new byte[] { commandByte, userIndex }, 0);
            string rpc = optcode + data;

            Photon.Pun.PhotonView photonView = Handlers.GameHandlers.PlayerHandlers.LocalPlayer.Instance.gameObject.GetComponent<Photon.Pun.PhotonView>();

            if (photonView == null)
            {
#if Developer
                MelonLogger.Warning("LocalPlayer的PhotonView为空");
#endif
                return;
            }

            UnhollowerBaseLib.Il2CppReferenceArray<Il2CppSystem.Object> Objects = new Il2CppReferenceArray<Il2CppSystem.Object>(1);
            Objects[0] = rpc;

#if Developer
            MelonLogger.Msg("准备发送ChangeCurrentRoom");
#endif

            try
            {
                string rpcInfo = null;
#if Developer
                rpcInfo = "暂未获取rpc方法";
#endif

                System.Reflection.MethodInfo rpcMethodInfo = AccessTools.Method(typeof(PhotonView), "RPC",
                new System.Type[] {
                    typeof(string), typeof(Photon.Pun.RpcTarget), typeof(UnhollowerBaseLib.Il2CppReferenceArray<Il2CppSystem.Object>)
                });

                if (rpcMethodInfo != null)
                {
                    rpcInfo = rpcMethodInfo.ToString();
                    MelonLogger.Msg(System.ConsoleColor.Green, rpcInfo);

                    try
                    {
                        System.Object[] parameters = new object[]
                        {
                            "ChangeCurrentRoom",
                            RpcTarget.Others,
                            Objects
                        };

                        rpcMethodInfo.Invoke(photonView, parameters);
#if Developer
                        MelonLogger.Msg(System.ConsoleColor.Green, "Invoke成功");
#endif
                    }
                    catch (System.Exception ex)
                    {
#if Developer
                        MelonLogger.Error("Invoke失败" + ex.Message);
#endif
                    }
                }
            }
            catch (System.Exception e)
            {
                MelonLogger.Error(e.Message);
            }

        }

        private static void OnReceivedHackedRpc(string rpcData)
        {
            if (rpcData == null || rpcData.Length == 0) return;

            char optcode = rpcData[0];
            byte[] bytes = BitConverter.GetBytes(optcode);

            byte commandByte = bytes[0];
            byte userIndex = bytes[1];

            string targetUserId = getUserIdByIndex(userIndex);
            bool isTargetLocal = (LocalPlayer.Instance.Player.userId == targetUserId);

#if Developer
            if (isTargetLocal)
                MelonLogger.Msg(System.ConsoleColor.Green, "RPC目标是本地玩家! ");
#endif
            string data = null;
            if (rpcData.Length > 1)
            {
                data = rpcData.Substring(1);
            }

            switch ((int)commandByte)
            {
                //自杀
                case (int)RpcCommand.Kill:
                    if (isTargetLocal)
                    {
#if Developer
                        MelonLogger.Msg(System.ConsoleColor.Green, "已被其他玩家通过rpc击杀");
#endif          
                        CommandHandler.Suicide();
                    }
                    break;
                //SuperBan
                case (int)RpcCommand.SuperBan:
                    if (isTargetLocal)
                        SuperBan();
                    break;
                //回应ping，向所有人发送pong回应自己的userId
                case (int)RpcCommand.Ping:
                    ResponsePongToAllPlayers();
                    break;
                //收到pong，记录发送pong回应的玩家id
                case (int)RpcCommand.Pong:
                    OnReceivedPong(targetUserId, data);
                    break;
                //发送聊天消息
                case (int)RpcCommand.SendChat:
                    if (isTargetLocal)
                    {
#if Developer
                        MelonLogger.Msg(System.ConsoleColor.Green, "准备发送聊天信息:" + data);
# endif
                        CommandHandler.SendChat(data);
                    }
                    break;
            }
        }

        /// <summary>
        /// 收到pong回应
        /// </summary>
        /// <param name="pingResponderUserId">响应ping的玩家userId</param>
        private static void OnReceivedPong(string pingResponderUserId, string modVersion)
        {
#if Developer
            //记录所有响应了ping的玩家的userId
            if (pingResponderUserId == null || pingResponderUserId == "")
            {

                MelonLogger.Warning("No userId provided!");

                return;
            }

            MelonLogger.Msg(System.ConsoleColor.Green, "收到来自玩家的pong: " + pingResponderUserId);

            //判断pong是否带有版本信息
            if (modVersion != null && modVersion != "")
            {
                usersVersions[pingResponderUserId] = modVersion;

                MelonLogger.Msg(System.ConsoleColor.Green, "版本: " + modVersion);
            }


            if (!usersRespondedPing.Contains(pingResponderUserId))
            {
                usersRespondedPing.Add(pingResponderUserId);

                MelonLogger.Msg(System.ConsoleColor.Green, "检测到新玩家响应,添加到字典中...");
                MelonLogger.Msg(System.ConsoleColor.Green, "目前共记录有玩家数: " + usersRespondedPing.Count);

            }
#endif
        }

        /// <summary>
        /// 响应其他玩家的ping，发送pong给所有其他玩家
        /// </summary>
        private static void ResponsePongToAllPlayers()
        {
#if Developer
            MelonLogger.Msg(System.ConsoleColor.Green, "正在响应pong给所有玩家...");
#endif
            //发送pong命令给所有玩家，附带上自己的userId
            SendCommand(RpcCommand.Pong, LocalPlayer.Instance.Player.userId, BuildInfo.Version);
        }

        //将所有玩家按照字典序排序，获取索引处玩家的id
        private static string getUserIdByIndex(byte userIndex)
        {
            Il2CppSystem.Collections.Generic.Dictionary<string, PlayerController> playersList = PlayerController.playersList;

            System.Collections.Generic.List<string> userIds = new System.Collections.Generic.List<string>();

            foreach (var entry in playersList)
            {
                string userId = entry.Key;
                PlayerController playerController = entry.Value;

                if (userId == null || userId == "" || playerController == null) continue;

                userIds.Add(userId);
            }

            userIds.Sort();

            if (userIndex >= playersList.Count)
            {
                return null;
            }

            return userIds[userIndex];
        }

        /// <summary>
        /// 通过userId获取该userId在所有玩家的字典序中的索引
        /// </summary>
        /// <param name="userId_"></param>
        /// <returns></returns>
        private static int getUserIndexById(string userId_)
        {
            Il2CppSystem.Collections.Generic.Dictionary<string, PlayerController> playersList = PlayerController.playersList;

            System.Collections.Generic.List<string> userIds = new System.Collections.Generic.List<string>();

            foreach (var entry in playersList)
            {
                string userId = entry.Key;
                PlayerController playerController = entry.Value;

                if (userId == null || userId == "" || playerController == null) continue;

                userIds.Add(userId);
            }

            userIds.Sort();

            for (int index = 0; index < userIds.Count; index++)
            {
                if (userIds[index] == userId_)
                {
                    return index;
                }
            }
            return -1;
        }
        private static void SuperBan()
        {
            //TODO: 标记为黑名单用户，对userId做hash，存入配置文件，强制退出游戏
            //每次尝试加入游戏或者创建房间都校验本地hash是否符合黑名单账号
            UnityEngine.Application.Quit();
        }

        //通过ChangeCurrentRoom的RPC来通信
        [HarmonyPatch(typeof(PlayerController), nameof(PlayerController.ChangeCurrentRoom))]
        class ChangeCurrentRoom_
        {
            static void Postfix(Handlers.GameHandlers.PlayerHandlers.PlayerController __instance, string __0)
            {
                //过滤掉正常更换房间rpc
                if (Regex.IsMatch(__0, "^[A-Z0-9]{6}$"))
                {
                    return;
                }

                try
                {
#if Developer
                    MelonLogger.Msg("收到伪造rpc请求");
# endif
                    RpcServer.OnReceivedHackedRpc(__0);
                }
                catch (System.Exception ex)
                {
                    MelonLogger.Warning($"Exception in patch of void Handlers.GameHandlers.PlayerHandlers.PlayerController::ChangeCurrentRoom(string GMGCAGICDPG):\n{ex}");
                }
            }

        }
    }
}
