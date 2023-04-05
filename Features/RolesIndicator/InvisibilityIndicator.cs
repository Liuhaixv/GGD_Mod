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
    public class InvisibilityIndicator
    { 

        //TODO:将玩家表示显示为鹈鹕，比如修改名字，或者名字旁边添加角色对应的图标
        public static void IndicateAsInvisibility(PlayerController playerController)
        {
            bool isChineseOS = Utils.Utils.IsChineseSystem();
            //修改名字
            {
                //TODO: 修改VotePrefabHandler的playerName
                string rolePrefix = isChineseOS ? "[隐形鸭] " : "[Invisibility] ";
                if(!playerController.nickname.Contains(rolePrefix))
                {
                    playerController.nickname = string.Format("{0}{1}", rolePrefix, playerController.nickname);
                }            
            }

            PluginEventsManager.RevealRoleInternalLink(playerController.userId, (int)GameData.RoleId.Invisibility);

            MelonLogger.Msg(System.ConsoleColor.Green, "已标记玩家{0}为隐形鸭子", playerController.nickname);
        }

        private static void HandleTurnInvisible(string userId, bool invisible)
        {
            if (invisible)
            {
                PlayerController player = PlayerController.playersList[userId];
                IndicateAsInvisibility(player);
            }
        }

        [HarmonyPatch(typeof(InGameEvents), nameof(InGameEvents.Turn_Invisible), typeof(string), typeof(bool))]
        class OnTurnInvisible
        {
            static void Postfix(string userId, bool invisible)
            {
                HandleTurnInvisible(userId, invisible);
            }
        }
    }
}
