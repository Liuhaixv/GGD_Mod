using MelonLoader;
using HarmonyLib;
using Handlers.GameHandlers;
using Handlers.GameHandlers.PlayerHandlers;
using UnityEngine;
using static MelonLoader.MelonLogger;
using System;

//TODO: 移除战争迷雾
//Remove fog of war
namespace GGD_Hack.Features
{
    public class RemoveFOW
    {
        public static MelonPreferences_Entry<bool> Enabled = MelonPreferences.CreateEntry<bool>("GGDH", "Enable_" + nameof(RemoveFOW), true);

        public static void RemoveFogOfWar()
        {
            try
            {

                FogOfWarHandler fogOfWar = LocalPlayer.Instance.fogOfWar;

                fogOfWar.DGAFOPNFEPL = 0;
                fogOfWar.layerMask = 0;

                //移除屋顶
                GameObject.Find("Roofs").SetActive(false);
                MelonLogger.Msg(System.ConsoleColor.Green, "已清除战争迷雾");
            }
            catch(Exception ex)
            {
#if Developer
                MelonLogger.Error("移除战争迷雾失败!" + ex.Message  );
#endif
            }
        }

        public static void SetBaseViewDistance(float distance)
        {
            Handlers.GameHandlers.PlayerHandlers.LocalPlayer.Instance.fogOfWar.baseViewDistance = distance;
        }

        [HarmonyPatch(typeof(FogOfWarHandler),nameof(FogOfWarHandler.Start))]
        public class RemoveFOW_On_Start
        {
            static void Prefix(ref FogOfWarHandler __instance)
            {
                if (Enabled.Value)
                {
                    RemoveFogOfWar();
                }
            }
        }

        [HarmonyPatch(typeof(FogOfWarHandler), nameof(FogOfWarHandler.FixedUpdate))]
        public class RemoveFOW_On_FixedUpdate
        {
            static void Postfix(ref FogOfWarHandler __instance)
            {
                if (Enabled.Value)
                {
                    GameObject fadedGameObject = __instance.gameObject.transform.Find("faded").gameObject;
                    if (fadedGameObject != null)
                    {
                        fadedGameObject.SetActive(false);
                        //GameObject.DestroyImmediate(fadedGameObject);
                    }

                    SetBaseViewDistance(100000.0f);
                    if (__instance.shader == null || __instance.shader.name != "empty_shader")
                    {
                        GameObject empty = new GameObject();
                        empty.name = "empty_shader";
                        __instance.shader = empty;
                    }                  
                }
            }
        }
    }
}
