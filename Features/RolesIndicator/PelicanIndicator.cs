using GGD_Hack.Events;
using Handlers.GameHandlers.PlayerHandlers;
using HarmonyLib;
using MelonLoader;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//行为检测判断玩家是否是鹈鹕
//1.监听玩家被吃事件
//2.获取被吃玩家最近的玩家
namespace GGD_Hack.Features.RolesIndicator
{
    public class PelicanIndicator
    {
        //TODO: test only
        public static void MarkAllPlayersAsPelican()
        {
            foreach (var player in Handlers.GameHandlers.PlayerHandlers.PlayerController.playersList.Values)
            {
                if (player != null && !player.isLocal)
                {
                    player.spawnedPlayerHandler?.SetPlayerRole(AEMEBPDOAPE.Pelican);
                }
            }
        }

        //TODO:将玩家表示显示为鹈鹕，比如修改名字，或者名字旁边添加角色对应的图标
        public static void IndicateAsPelican(PlayerController playerController)
        {
            bool isChineseOS = Utils.Utils.IsChineseSystem();
            //修改名字
            {
                //TODO: 修改VotePrefabHandler的playerName
                string rolePrefix = isChineseOS ? "[鹈鹕] " : "[Pelican] ";
                if (!playerController.nickname.Contains(rolePrefix))
                {
                    playerController.nickname = string.Format("{0}{1}", rolePrefix, playerController.nickname);
                }
            }

            //playerController.spawnedPlayerHandler.SetPlayerRole(AEMEBPDOAPE.Pelican);
            PluginEventsManager.RevealRoleInternalLink(playerController.userId, (int)GameData.RoleId.Pelican);

            MelonLogger.Msg(System.ConsoleColor.Green, "已标记玩家{0}为鹈鹕", playerController.nickname);
        }

        /// <summary>
        /// 获取除了userId玩家外的其他玩家，按照与该玩家的距离正序的List
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static List<PlayerController> GetOtherAlivePlayersNearby(string userId)
        {
            Vector3 position = PlayerController.playersList[userId].CIPMDPLLDAL;

            List<PlayerController> playersNearby = new List<PlayerController>();

            foreach (var Entry in PlayerController.playersList)
            {
                string nickname = Entry.key;
                PlayerController playerController = Entry.value;

                //玩家为空
                if (string.IsNullOrEmpty(nickname) || playerController == null)
                {
                    continue;
                }

                //已死亡
                if (playerController.timeOfDeath != 0)
                {
                    continue;
                }

                //排除自己
                if (playerController.userId == userId)
                {
                    continue;
                }

                playersNearby.Add(playerController);
            };

            //根据和本地玩家的距离升序排序        
            playersNearby.Sort((p1, p2) => Vector3.Distance(p1.CIPMDPLLDAL, position)
                                .CompareTo(Vector3.Distance(p2.CIPMDPLLDAL, position)));

            return playersNearby;
        }

        private static void HandlePelicanEat(string playerEaten, string pelican = null)
        {
            PlayerController pelicanPlayerController = null;

            if (!string.IsNullOrWhiteSpace(pelican))
            {
                pelicanPlayerController = PlayerController.playersList[pelican];
                IndicateAsPelican(pelicanPlayerController);
                return;
            }

            //未指定鹈鹕，根据死亡玩家附近玩家判断鹈鹕

            List<PlayerController> playersNearby = GetOtherAlivePlayersNearby(playerEaten);

            Vector3 eatenPosition = PlayerController.playersList[playerEaten].CIPMDPLLDAL;

            if (playersNearby == null || playersNearby.Count == 0)
            {
                MelonLogger.Error("HandlePelicanEat的playersNearby不能为空");
                return;
            }

            //根据一定范围内，才确定为鹈鹕，防止鹈鹕吃人时有多个玩家靠近鹈鹕，被错误标记为鹈鹕
            PlayerController nearstPlayer = playersNearby.ElementAt(0);
           
            if (playersNearby.Count == 1)
            {
                pelicanPlayerController = nearstPlayer;
            }
            else
            {
                float nearstDistance = Vector3.Distance(nearstPlayer.CIPMDPLLDAL, eatenPosition);

                PlayerController secondNearstPlayer =  playersNearby.ElementAt(1);

                float secondNearstDistance = Vector3.Distance(secondNearstPlayer.CIPMDPLLDAL, eatenPosition);

                if (secondNearstDistance - nearstDistance > 1f)
                {
                    pelicanPlayerController = nearstPlayer;
                }
                else
                {
                    pelicanPlayerController = null;
                    MelonLogger.Msg(System.ConsoleColor.Red, "鹈鹕吃人事件: 被吃的玩家: {0} 附近有多个玩家，无法判断鹈鹕", playerEaten);
                }
            }

            if (pelicanPlayerController)
            {
                MelonLogger.Msg(System.ConsoleColor.Green, "鹈鹕吃人事件: 已根据被吃玩家附近玩家判断出鹈鹕为：{0}",pelicanPlayerController.nickname);
                IndicateAsPelican(pelicanPlayerController);
            }
        }

        [HarmonyPatch(typeof(InGameEvents), nameof(InGameEvents.Pelican_Eat), typeof(string), typeof(string))]
        class OnPelicanEat
        {
            static void Postfix(string playerEaten, string pelican = null)
            {
                MelonLogger.Msg(System.ConsoleColor.Green, "Postfix: 鹈鹕吃人事件：鹈鹕：{0} 被吃的玩家: {1}", string.IsNullOrEmpty(pelican) ? "未知" : pelican, playerEaten);

                HandlePelicanEat(playerEaten, pelican);
            }
        }
    }
}
