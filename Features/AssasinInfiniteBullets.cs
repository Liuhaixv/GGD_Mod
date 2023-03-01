using Handlers.GameHandlers.VotingHandlers;
using HarmonyLib;
using Il2CppSystem;
using Il2CppSystem.Collections.Generic;
using Managers;
using Managers.PlayerManagers;
using MelonLoader;
using UnhollowerBaseLib;
using UnityEngine.UI;

namespace GGD_Hack.Features
{
    //刺客开始射击 TODO: 
    //[HarmonyPatch(typeof(AssassinPanelHandler), nameof(AssassinPanelHandler.BCFLIFKEILI))]
    public class AssasinInfiniteBullets
    {
        //射击后将按钮改为可交互
        static void Postfix(AssassinPanelHandler __instance, FJCKNAJLDIG __0)
        {
            try
            {

                VotingPanelHandler instance = VotingPanelHandler.Instance;
                if(instance == null)
                {
                    MelonLogger.Warning("[刺客无限子弹] 未找到VotingPanelHandler!");
                    return;
                }

                //获取刺客的点击小按钮
                Button button = instance.assassinButton.GetComponent<Button>();
                button.interactable = true;
            }
            catch (System.Exception ex)
            {
                MelonLogger.Warning(ex.Message);
            }
        }
    }

    [HarmonyPatch(typeof(PlayerRolesManager), nameof(PlayerRolesManager.GetPossibleAssassinTargets))]
    public class EnableToShootAllRoles
    {
        static void Postfix(ref UnhollowerBaseLib.Il2CppStructArray<FJCKNAJLDIG> __result)
        {
            try
            {
                UnhollowerBaseLib.Il2CppStructArray<FJCKNAJLDIG> temp = new UnhollowerBaseLib.Il2CppStructArray<FJCKNAJLDIG>(__result.Count + 2);

                for(int i = 0; i < __result.Count; i++)
                {
                    temp[i] = __result[i];
                }

                temp[__result.Count] = FJCKNAJLDIG.Bounty;
                temp[__result.Count + 1] = FJCKNAJLDIG.Goose;

                __result = temp;

                /*
                List<FJCKNAJLDIG> roleIds = new List<FJCKNAJLDIG>();

                 foreach(var role in __result)
                {
                    roleIds.Add(role);
                }

                roleIds.Add(FJCKNAJLDIG.Bounty);
                roleIds.Add(FJCKNAJLDIG.Goose);
                */
            }
            catch (System.Exception ex)
            {
                MelonLogger.Warning(ex.Message);
            }
        }

    }
}
