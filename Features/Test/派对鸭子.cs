using Handlers.GameHandlers.PlayerHandlers;
using Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace GGD_Hack.Features.Test
{
    public class 派对鸭子
    {
        public void 派对(PlayerController playerController_)
        {
            foreach (var entry in PlayerController.playersList)
            {
                string userId = entry.Key;
                PlayerController playerController = entry.Value;

                //跳过无效玩家
                if (userId == null || userId == "" || playerController == null || playerController.isLocal)
                {
                    continue;
                }

                //玩家未死亡
                if (playerController.timeOfDeath == 0)
                {
                    continue;
                }

                string userId_ = playerController.userId;
                MainManager.Instance.pluginEventsManager.OGINDGDMPAB(userId_);
            }

        }
    }
}
