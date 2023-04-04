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
    public class MorphIndicator
    { 
        public static void IndicateAsMorphed(string userId)
        {
            PlayerController playerController = PlayerController.playersList[userId];

            bool isChineseOS = Utils.Utils.IsChineseSystem();
            //修改名字
            {
                //TODO: 修改VotePrefabHandler的playerName
                string MorphPrefix = isChineseOS ? "[变形过] " : "[Morphed] ";
                if(!playerController.nickname.Contains(MorphPrefix))
                {
                    playerController.nickname = string.Format("{0}{1}", MorphPrefix, playerController.nickname);
                }            
            }

            MelonLogger.Msg(System.ConsoleColor.Green, "已标记玩家{0}变形过", playerController.nickname);
        }

        [HarmonyPatch(typeof(InGameEvents), nameof(InGameEvents.Morph), typeof(string), typeof(string))]
        class onMorph
        {
            static void Postfix(string from, string to)
            {
                IndicateAsMorphed(from);
            }
        }
    }
}
