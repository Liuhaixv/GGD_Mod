using Handlers.GameHandlers.PlayerHandlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GGD_Hack.Features
{
    public class Bomb
    {
        //将炸弹随机传递给一名其他玩家
        public static void ThrowAwayBomb()
        {
            //获取所有存活玩家
            List<string> alivePlayers = new List<string>();

            foreach(var entry in PlayerController.playersList)
            {
                string userId = entry.Key;
                PlayerController playerControllerplayerController = entry.Value;

                if(playerControllerplayerController != null && !playerControllerplayerController.isLocal && playerControllerplayerController.timeOfDeath == 0)
                {
                    alivePlayers.Add(userId);
                }
            }

            if(alivePlayers.Count> 0)
            {
                //随机选择一名玩家
                string chosenPlayer = alivePlayers[UnityEngine.Random.RandomRangeInt(0, alivePlayers.Count)];
                PluginEventsManager.Throw_Bomb(chosenPlayer);
            }
        }
    }
}
