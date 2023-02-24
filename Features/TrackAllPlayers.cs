using Handlers.GameHandlers.PlayerHandlers;
using Handlers.GameHandlers.TaskHandlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GGD_Hack.Features
{
    public class TrackAllPlayers
    {
        /// <summary>
        /// 显示所有玩家的箭头指示
        /// </summary>
        public static void ShowAllPlayersArrow()
        {
            //1.遍历玩家
            //2.TaskTargetingHandler调用方法添加玩家箭头

            //停止所有箭头追踪，防止重复添加
            TaskTargetingHandler.Instance.StopAllTargeting();

            //1.遍历玩家
            Il2CppSystem.Collections.Generic.Dictionary<string, PlayerController> playersList = PlayerController.playersList;

            foreach(var playerEntry in playersList)
            {
                if(playerEntry.Value == null) continue;

                PlayerController playerController = playerEntry.Value;

                //不指示幽灵
                if (playerController.isGhost) continue;

                //添加箭头追踪
                TaskTargetingHandler.Instance.EBDNBLGNMPI(playerController, TargetHandler.OPHJIHLGMNA.None);
            }
        }
    }
}
