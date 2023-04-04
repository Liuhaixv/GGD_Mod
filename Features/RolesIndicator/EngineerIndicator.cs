using GGD_Hack.Events;
using Handlers.GameHandlers.PlayerHandlers;
using HarmonyLib;
using Managers;
using MelonLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//行为检测判断玩家是否是鹈鹕
//1.监听玩家被吃事件
//2.获取被吃玩家最近的玩家
namespace GGD_Hack.Features.RolesIndicator
{
    public class EngineerIndicator
    {
        public static void IndicateAsEngineer(PlayerController playerController)
        {
            bool isChineseOS = Utils.Utils.IsChineseSystem();
            //修改名字
            {
                //TODO: 修改VotePrefabHandler的playerName
                string engineerPrefix = isChineseOS ? "[工程师] " : "[Engineer] ";
                if (!playerController.nickname.Contains(engineerPrefix))
                {
                    playerController.nickname = string.Format("{0}{1}", engineerPrefix, playerController.nickname);
                }
            }

            MelonLogger.Msg(System.ConsoleColor.Green, "已标记玩家{0}为工程师", playerController.nickname);
        }

        private static void HandleForceExitVent(string userId)
        {
            //跳过投票阶段，会误触，因为玩家掉线后会直接判定为强制退出管道
            if((byte)MainManager.Instance.gameManager.gameState == (byte)GameData.GameState.Voting)
            {
                MelonLogger.Msg(System.ConsoleColor.Green, "由于正在投票，忽略强制退出管道事件");
                return;
            }

            PlayerController player = PlayerController.playersList[userId];
            IndicateAsEngineer(player);
        }

        [HarmonyPatch(typeof(InGameEvents), nameof(InGameEvents.ForceExitVent), typeof(string), typeof(string))]
        class OnForceExitVent
        {
            static void Postfix(string userId, string ventId)
            {
                HandleForceExitVent(userId);
            }
        }
    }
}
