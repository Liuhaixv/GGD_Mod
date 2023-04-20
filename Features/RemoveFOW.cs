using MelonLoader;
using HarmonyLib;
using Handlers.GameHandlers;
using Handlers.GameHandlers.PlayerHandlers;
using UnityEngine;
using static MelonLoader.MelonLogger;
using System;
using System.Collections;
using Handlers.LobbyHandlers;
using UnhollowerRuntimeLib;

//TODO: 移除战争迷雾
//Remove fog of war
namespace GGD_Hack.Features
{
    [RegisterTypeInIl2Cpp]
    public class RemoveFOW : MonoBehaviour
    {
        public static RemoveFOW Instance;
        public static MelonPreferences_Entry<bool> Enabled = MelonPreferences.CreateEntry<bool>("GGDH", "Enable_" + nameof(RemoveFOW), true);

        private static float lastTimeHackedLayerMask = -1;

        public RemoveFOW(IntPtr ptr) : base(ptr)
        {
            IngameSettings.AddIngameSettingsEntry(
                               new IngameSettings.IngameSettingsEntry()
                               {
                                   entry = Enabled,
                                   name_cn = "移除战争迷雾",
                                   name_eng = "Remove Fog of War"
                               }
                                          );
        }

        // Optional, only used in case you want to instantiate this class in the mono-side
        // Don't use this on MonoBehaviours / Components!
        public RemoveFOW() : base(ClassInjector.DerivedConstructorPointer<RemoveFOW>()) => ClassInjector.DerivedConstructorBody(this);
        public static void Init()
        {
            GameObject ML_Manager = GameObject.Find("ML_Manager");
            if (ML_Manager == null)
            {
                ML_Manager = new GameObject("ML_Manager");
                DontDestroyOnLoad(ML_Manager);
            }

            if (ML_Manager.GetComponent<RemoveFOW>() == null)
            {
                Instance = ML_Manager.AddComponent<RemoveFOW>();
            }
        }

        public static void SetBaseViewDistance(float distance)
        {
            Handlers.GameHandlers.PlayerHandlers.LocalPlayer.Instance.fogOfWar.baseViewDistance = distance;
        }

        [HarmonyPatch(typeof(FogOfWarHandler), nameof(FogOfWarHandler.UpdateFieldOfView))]
        public class DelayedDisableUpdatingFOV
        {
            static bool Prefix(ref FogOfWarHandler __instance)
            {
                if (!Enabled.Value)
                {
                    return true;
                }
                //修改视野大小范围
                __instance.baseViewDistance = 100000.0f;

                //Not hacked yet
                if (__instance.layerMask != 0)
                {
                    lastTimeHackedLayerMask = UnityEngine.Time.time;

                    //修改layerMask防止遮挡视野
                    __instance.KPCALPEMKDI = 0;
                    __instance.layerMask = 0;

                    GameObject faded = __instance.gameObject.transform.Find("faded").gameObject;
                    if (faded != null)
                    {
                        faded.SetActive(false);
                    }

                    if (__instance.shader == null || __instance.shader.name != "empty_shader")
                    {
                        GameObject empty = new GameObject();
                        empty.name = "empty_shader";
                        __instance.shader = empty;
                    }
                    return true;
                }
                else
                {
                    //Hacked
                    //Should skip updating FOV?

                    //游戏未开始
                    if (LobbySceneHandler.Instance.gameStarted == false)
                    {
                        //Do not skip
                        return true;
                    }

                    if (UnityEngine.Time.time - lastTimeHackedLayerMask > 2)
                    {
                        //Skip
                        return false;
                    }
                    else
                    {
                        //Do not skip
                        return true;
                    }
                }
            }
        }
    }
}
