using GGD_Hack.Events;
using Handlers.GameHandlers.PlayerHandlers;
using HarmonyLib;
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
    public class VentIndicator
    {
        public static void IndicateUsedVent(PlayerController playerController)
        {
            bool isChineseOS = Utils.Utils.IsChineseSystem();
            //修改名字
            {
                //TODO: 修改VotePrefabHandler的playerName
                string ventPrefix = isChineseOS ? "[使用过管道] " : "[Vented] ";
                if (!playerController.nickname.Contains(ventPrefix))
                {
                    playerController.nickname = string.Format("{0}{1}", ventPrefix, playerController.nickname);
                }
            }

            MelonLogger.Msg(System.ConsoleColor.Green, "已标记玩家{0}为使用过管道", playerController.nickname);
        }

        private static void HandleVent(string userId)
        {
            PlayerController player = PlayerController.playersList[userId];
            IndicateUsedVent(player);
        }

        [HarmonyPatch(typeof(InGameEvents), nameof(InGameEvents.Vent), typeof(string), typeof(string))]
        class OnForceExitVent
        {
            static void Postfix(string userId, string ventId)
            {
                HandleVent(userId);
            }
        }
    }
}
