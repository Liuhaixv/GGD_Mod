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
    public class UndertakerIndicator
    {
        public static void IndicateAsUndertaker(PlayerController playerController)
        {
            bool isChineseOS = Utils.Utils.IsChineseSystem();
            //修改名字
            {
                //TODO: 修改VotePrefabHandler的playerName
                string rolePrefix = isChineseOS ? "[丧葬者] " : "[Undertaker] ";
                if (!playerController.nickname.Contains(rolePrefix))
                {
                    playerController.nickname = string.Format("{0}{1}", rolePrefix, playerController.nickname);
                }
            }

            PluginEventsManager.RevealRoleInternalLink(playerController.userId, (int)GameData.RoleId.Undertaker);

            MelonLogger.Msg(System.ConsoleColor.Green, "已标记玩家{0}为丧葬者", playerController.nickname);
        }

        private static void HandleGrabBody(string undertakerUserId, string bodyUserId)
        {
            PlayerController player = PlayerController.playersList[undertakerUserId];
            IndicateAsUndertaker(player);
        }

        [HarmonyPatch(typeof(InGameEvents), nameof(InGameEvents.Grab_Body), typeof(string), typeof(string))]
        class OnTurnInvisible
        {
            static void Postfix(string undertakerUserId, string bodyUserId)
            {
                HandleGrabBody(undertakerUserId, bodyUserId);
            }
        }
    }
}
