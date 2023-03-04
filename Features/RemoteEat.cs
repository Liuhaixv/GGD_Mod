using Handlers.GameHandlers;
using Handlers.GameHandlers.PlayerHandlers;
using MelonLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GGD_Hack.Features
{
    /// <summary>
    /// 远程吃尸体
    /// </summary>
    public class RemoteEat
    {
        public static void EatRandomBody()
        {
            bool foundValidBody = false;
            foreach (var entry in PlayerController.playersList)
            {
                string userId = entry.key;
                PlayerController playerController = entry.value;
                if(userId == null || playerController == null || playerController.timeOfDeath == 0) { continue; }

                BodyHandler body = Managers.MainManager.Instance.gameManager.BodyFromUserId(playerController.userId);
                if (body == null)
                {
                    continue;
                }
                MelonLogger.Msg("已找到有效尸体，正在远程消化尸体:" + playerController.nickname);
                PluginEventsManager.Eat(body.bodyUserId);
                foundValidBody = true;
            }
            if(!foundValidBody)
            {
                MelonLogger.Warning("未找到有效尸体" );
            }
        }
    }
}
