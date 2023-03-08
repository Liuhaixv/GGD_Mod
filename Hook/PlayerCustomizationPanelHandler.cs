using GGD_Hack.Features;
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
            static void Postfix(ref bool __result)
            {
                if(__result == false)
                {
                    if(UnlockAllItems.Enabled.Value == true)
                    {
                        __result = true;
                    }
                }
            }
        }

    }
}
