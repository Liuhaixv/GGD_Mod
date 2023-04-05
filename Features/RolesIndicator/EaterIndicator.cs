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
    public class EaterIndicator
    { 
        public static void IndicateAsEater(string userId)
        {
            PlayerController playerController = PlayerController.playersList[userId];

            bool isChineseOS = Utils.Utils.IsChineseSystem();
            //修改名字
            {
                //TODO: 修改VotePrefabHandler的playerName
                string rolePrefix = isChineseOS ? "[吃过尸体] " : "[Ate body] ";
                if(!playerController.nickname.Contains(rolePrefix))
                {
                    playerController.nickname = string.Format("{0}{1}", rolePrefix, playerController.nickname);                    
                }            
            }

            MelonLogger.Msg(System.ConsoleColor.Green, "已标记玩家{0}吃过尸体", playerController.nickname);
        }

        [HarmonyPatch(typeof(InGameEvents), nameof(InGameEvents.Eat), typeof(string), typeof(string))]
        class OnTurnInvisible
        {
            static void Postfix(string eater, string eated)
            {
                IndicateAsEater(eater);
            }
        }
    }
}
