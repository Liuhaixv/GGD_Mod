using Handlers.LobbyHandlers;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GGD_Hack.Hook
{
    public class PlayerCustomizationPanelHandler_
    {

        /// <summary>
        /// 跳过死亡动画
        /// </summary>
        [HarmonyPatch(typeof(PlayerCustomizationPanelHandler), nameof(PlayerCustomizationPanelHandler.IsOwnedOrFree))]
        class IsOwnedOrFree_
        {
            static bool Prefix(ref bool __result)
            {
                __result = true;
                //跳过执行
                return false;
            }
        }

    }
}
