using HarmonyLib;
using Managers.GameManagers;
using MelonLoader;
using UnityEngine;

namespace GGD_Hack.Hook
{
    public class CheatingManager_
    {
        [HarmonyPatch(typeof(CheatingManager), nameof(CheatingManager.OnCheat))]
        public class OnCheat_
        {
            static bool Prefix(string __0)
            {
                MelonLogger.Error("已检测到作弊: " + __0);

                //关闭游戏
                Application.Quit();
                return false;
            }
        }
    }
}
