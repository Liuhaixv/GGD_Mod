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
    //48 89 5C 24 08 57 48 83 EC 20 80 3D ?? ?? ?? ?? ?? 0F B7 FA 48 8B D9 75 67 48 8D 0D ?? ?? ?? ?? E8 ?? ?? ?? ?? 48 8D 0D ?? ?? ?? ?? E8 ?? ?? ?? ?? 48 8D 0D ?? ?? ?? ?? E8 ?? ?? ?? ?? 48 8D 0D ?? ?? ?? ?? E8 ?? ?? ?? ?? 48 8D 0D ?? ?? ?? ?? E8 ?? ?? ?? ?? 48 8D 0D ?? ?? ?? ?? E8 ?? ?? ?? ?? 48 8D 0D ?? ?? ?? ?? E8 ?? ?? ?? ?? 48 8D 0D ?? ?? ?? ?? E8 ?? ?? ?? ?? C6 05 ?? ?? ?? ?? ?? 33 D2 48 8B CB E8 ?? ?? ?? ?? 48 8B 05 ?? ?? ?? ?? 83 B8 E0 00 00 00 00 75 0F 48 8B C8 E8 ?? ?? ?? ?? 48 8B 05 ?? ?? ?? ?? 48 8B 80 B8 00 00 00 48 8B 08 48 85 C9 0F 84 77 02 00 00 45 33 C9 45 33 C0 33
    [HarmonyPatch(typeof(AssassinPanelHandler), nameof(AssassinPanelHandler.CFFKHGAGENE), new System.Type[] { typeof(PFFIPCCNHBA) })]
    public class AssasinInfiniteBullets
    {
        //射击后将按钮改为可交互
        static void Postfix(AssassinPanelHandler __instance, PFFIPCCNHBA __0)
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
    /// 允许刺客狙击肉汁和大白鹅
    /// </summary>
    [HarmonyPatch(typeof(PlayerRolesManager), nameof(PlayerRolesManager.GetPossibleAssassinTargets))]
    public class EnableToShootAllRoles
    {
        static void Postfix(ref UnhollowerBaseLib.Il2CppStructArray<PFFIPCCNHBA> __result)
        {
            try
            {
                const int rolesNum = 19;

                UnhollowerBaseLib.Il2CppStructArray<PFFIPCCNHBA> temp = new UnhollowerBaseLib.Il2CppStructArray<PFFIPCCNHBA>(__result.Count + rolesNum);

                for (int i = 0; i < __result.Count; i++)
                {
                    temp[i] = __result[i];
                }
                temp[__result.Count + 0] = PFFIPCCNHBA.Cannibal;
                temp[__result.Count + 1] = PFFIPCCNHBA.Morphling;
                temp[__result.Count + 2] = PFFIPCCNHBA.Silencer;
                temp[__result.Count + 3] = PFFIPCCNHBA.Professional;
                temp[__result.Count + 4] = PFFIPCCNHBA.Spy;
                temp[__result.Count + 5] = PFFIPCCNHBA.Hitman;
                temp[__result.Count + 6] = PFFIPCCNHBA.Snitch;
                temp[__result.Count + 7] = PFFIPCCNHBA.Party;
                temp[__result.Count + 8] = PFFIPCCNHBA.Demolitionist;
                temp[__result.Count + 9] = PFFIPCCNHBA.IdentityThief;
                temp[__result.Count + 10] = PFFIPCCNHBA.Ninja;
                temp[__result.Count + 11] = PFFIPCCNHBA.Undertaker;
                temp[__result.Count + 12] = PFFIPCCNHBA.Invisibility;
                temp[__result.Count + 13] = PFFIPCCNHBA.SerialKiller;
                temp[__result.Count + 14] = PFFIPCCNHBA.Warlock;
                temp[__result.Count + 15] = PFFIPCCNHBA.EsperDuck;
                temp[__result.Count + 16] = PFFIPCCNHBA.Assassin;
                temp[__result.Count + 17] = PFFIPCCNHBA.Goose;
                temp[__result.Count + 18] = PFFIPCCNHBA.Bounty;

                //temp[__result.Count + 3] = PFFIPCCNHBA.None;

                __result = temp;
            }
            catch (System.Exception ex)
            {
                MelonLogger.Warning(ex.Message);
            }
        }
    }
}
