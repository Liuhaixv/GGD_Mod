using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//静音所有玩家
namespace GGD_Hack.Features
{
    public class SilenceAllPlayers
    {
        public static bool SilenceAllOtherPlayers()
        {
            foreach (var entry in Handlers.GameHandlers.PlayerHandlers.PlayerController.playersList)
            {
                string userId = entry.Key;
                Handlers.GameHandlers.PlayerHandlers.PlayerController playerController = entry.Value;

                //跳过无效玩家、死亡玩家、本地玩家
                if(userId == null || userId =="" || playerController == null || 
                    playerController.isLocal || playerController.timeOfDeath != 0 ||
                    playerController.isSilenced
                    )
                {
                    continue;
                }

                Managers.MainManager.Instance.pluginEventsManager.NDJOCHMBDND(userId);
            }

            return true;
        }
    }
}
