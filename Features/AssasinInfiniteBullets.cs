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
    [HarmonyPatch(typeof(AssassinPanelHandler),        nameof(AssassinPanelHandler.FGOIENBGLJJ),        new System.Type[] {typeof(OBBMCDJMMOK)  })]
    public class AssasinInfiniteBullets
    {
        //射击后将按钮改为可交互
        static void Postfix(AssassinPanelHandler __instance, OBBMCDJMMOK __0)
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


#if Developer
    [HarmonyPatch(typeof(PlayerRolesManager), nameof(PlayerRolesManager.GetPossibleAssassinTargets))]
    public class EnableToShootAllRoles
    {
        static void Postfix(ref UnhollowerBaseLib.Il2CppStructArray<OBBMCDJMMOK> __result)
        {
            try
            {
                UnhollowerBaseLib.Il2CppStructArray<OBBMCDJMMOK> temp = new UnhollowerBaseLib.Il2CppStructArray<OBBMCDJMMOK>(__result.Count + 2);

                for (int i = 0; i < __result.Count; i++)
                {
                    temp[i] = __result[i];
                }

                temp[__result.Count] = OBBMCDJMMOK.Bounty;
                temp[__result.Count + 1] = OBBMCDJMMOK.Goose;

                __result = temp;
            }
            catch (System.Exception ex)
            {
                MelonLogger.Warning(ex.Message);
            }
        }

    }
#endif
}
