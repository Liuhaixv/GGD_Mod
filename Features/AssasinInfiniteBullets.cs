using Handlers.GameHandlers.VotingHandlers;
using HarmonyLib;
using Il2CppSystem;
using Il2CppSystem.Collections.Generic;
using Managers;
using Managers.PlayerManagers;
using MelonLoader;
using UnhollowerBaseLib;
using UnityEngine;
using UnityEngine.UI;

namespace GGD_Hack.Features
{
#if Developer
    //刺客开始射击 
    //作用为在射击后重新激活点击射击的按钮
    //被AssassinPanelHandler_??????????___ShowPanel_b__0调用
    //48 83 EC 28 48 8B 41 18 48 85 C0 74 13 0F
    [HarmonyPatch(typeof(AssassinPanelHandler), nameof(AssassinPanelHandler.NJGGMKKANPF), new System.Type[] { typeof(IPLJDOHJOLM) })]
    public class AssasinInfiniteBullets
    {
        //射击后将按钮改为可交互
        static void Postfix(AssassinPanelHandler __instance, IPLJDOHJOLM __0)
        {
            try
            {
                VotingPanelHandler instance = VotingPanelHandler.Instance;
                if (instance == null)
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
#endif

    /*
    [HarmonyPatch(typeof(GameObject), nameof(GameObject.SetActive)]
    public class InfiniteBulletsPatch_v2
    {
        static bool Prefix(GameObject __instance)
        {
            if(__instance.name)
        }
    }*/


    /// <summary>
    /// [已废弃]
    /// 允许刺客狙击肉汁和大白鹅
    /// </summary>
    /*
    [HarmonyPatch(typeof(PlayerRolesManager), nameof(PlayerRolesManager.GetPossibleAssassinTargets))]
    public class EnableToShootAllRoles
    {
        static void Postfix(ref UnhollowerBaseLib.Il2CppStructArray<IPLJDOHJOLM> __result)
        {
            try
            {
                const int rolesNum = 18;

                UnhollowerBaseLib.Il2CppStructArray<IPLJDOHJOLM> temp = new UnhollowerBaseLib.Il2CppStructArray<IPLJDOHJOLM>(__result.Count + rolesNum);

                for (int i = 0; i < __result.Count; i++)
                {
                    temp[i] = __result[i];
                }
                temp[__result.Count + 0] = IPLJDOHJOLM.Cannibal;
                temp[__result.Count + 1] = IPLJDOHJOLM.Morphling;
                temp[__result.Count + 2] = IPLJDOHJOLM.Silencer;
                temp[__result.Count + 3] = IPLJDOHJOLM.Professional;
                temp[__result.Count + 4] = IPLJDOHJOLM.Spy;
                temp[__result.Count + 5] = IPLJDOHJOLM.Hitman;
                temp[__result.Count + 6] = IPLJDOHJOLM.Snitch;
                temp[__result.Count + 7] = IPLJDOHJOLM.Party;
                temp[__result.Count + 8] = IPLJDOHJOLM.Demolitionist;
                temp[__result.Count + 9] = IPLJDOHJOLM.IdentityThief;
                temp[__result.Count + 10] = IPLJDOHJOLM.Ninja;
                temp[__result.Count + 11] = IPLJDOHJOLM.Undertaker;
                temp[__result.Count + 12] = IPLJDOHJOLM.Invisibility;
                temp[__result.Count + 13] = IPLJDOHJOLM.SerialKiller;
                temp[__result.Count + 14] = IPLJDOHJOLM.Warlock;
                temp[__result.Count + 15] = IPLJDOHJOLM.EsperDuck;
                temp[__result.Count + 16] = IPLJDOHJOLM.Goose;
                temp[__result.Count + 17] = IPLJDOHJOLM.Bounty;

                //temp[__result.Count + 3] = IPLJDOHJOLM.None;

                __result = temp;
            }
            catch (System.Exception ex)
            {
                MelonLogger.Warning(ex.Message);
            }
        }
    }*/
}
