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
    [HarmonyPatch(typeof(AssassinPanelHandler), nameof(AssassinPanelHandler.HDHMIAPOBPJ), new System.Type[] { typeof(CONFOOGKOGN) })]
    public class AssasinInfiniteBullets
    {
        //射击后将按钮改为可交互
        static void Postfix(AssassinPanelHandler __instance, CONFOOGKOGN __0)
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
        static void Postfix(ref UnhollowerBaseLib.Il2CppStructArray<CONFOOGKOGN> __result)
        {
            try
            {
                const int rolesNum = 18;

                UnhollowerBaseLib.Il2CppStructArray<CONFOOGKOGN> temp = new UnhollowerBaseLib.Il2CppStructArray<CONFOOGKOGN>(__result.Count + rolesNum);

                for (int i = 0; i < __result.Count; i++)
                {
                    temp[i] = __result[i];
                }
                temp[__result.Count + 0] = CONFOOGKOGN.Cannibal;
                temp[__result.Count + 1] = CONFOOGKOGN.Morphling;
                temp[__result.Count + 2] = CONFOOGKOGN.Silencer;
                temp[__result.Count + 3] = CONFOOGKOGN.Professional;
                temp[__result.Count + 4] = CONFOOGKOGN.Spy;
                temp[__result.Count + 5] = CONFOOGKOGN.Hitman;
                temp[__result.Count + 6] = CONFOOGKOGN.Snitch;
                temp[__result.Count + 7] = CONFOOGKOGN.Party;
                temp[__result.Count + 8] = CONFOOGKOGN.Demolitionist;
                temp[__result.Count + 9] = CONFOOGKOGN.IdentityThief;
                temp[__result.Count + 10] = CONFOOGKOGN.Ninja;
                temp[__result.Count + 11] = CONFOOGKOGN.Undertaker;
                temp[__result.Count + 12] = CONFOOGKOGN.Invisibility;
                temp[__result.Count + 13] = CONFOOGKOGN.SerialKiller;
                temp[__result.Count + 14] = CONFOOGKOGN.Warlock;
                temp[__result.Count + 15] = CONFOOGKOGN.EsperDuck;
                temp[__result.Count + 16] = CONFOOGKOGN.Goose;
                temp[__result.Count + 17] = CONFOOGKOGN.Bounty;

                //temp[__result.Count + 3] = CONFOOGKOGN.None;

                __result = temp;
            }
            catch (System.Exception ex)
            {
                MelonLogger.Warning(ex.Message);
            }
        }
    }*/
}
