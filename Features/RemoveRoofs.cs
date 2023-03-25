using Handlers.GameHandlers;
using HarmonyLib;
using MelonLoader;
using UnityEngine;

namespace GGD_Hack.Features
{
    public class RemoveRoofs
    {
        public static MelonPreferences_Entry<bool> Enabled = MelonPreferences.CreateEntry<bool>("GGDH", "Enable_" + nameof(RemoveRoofs), true);

        public static void RemoveAllRoofs()
        {
            //移除屋顶
            GameObject.Find("Roofs").SetActive(false);
        }

        //战争迷雾是游戏开始后才会执行的
        [HarmonyPatch(typeof(FogOfWarHandler), nameof(FogOfWarHandler.Start))]
        public class RemoveFOW_On_Start
        {
            static void Prefix(ref FogOfWarHandler __instance)
            {
                if (Enabled.Value)
                {
                    RemoveAllRoofs();
                }
            }
        }
    }
}
