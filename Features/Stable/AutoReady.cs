using Handlers.LobbyHandlers;
using HarmonyLib;
using MelonLoader;
using UnhollowerRuntimeLib;
using UnityEngine;

using IntPtr = System.IntPtr;

namespace GGD_Hack.Features
{
    [RegisterTypeInIl2Cpp]
    public class AutoReady : MonoBehaviour
    {
        public static AutoReady Instance;
        public static MelonPreferences_Entry<bool> Enabled = MelonPreferences.CreateEntry<bool>("GGDH", "Enable_" + nameof(AutoReady), true);

        public AutoReady(IntPtr ptr) : base(ptr)
        {
            IngameSettings.AddIngameSettingsEntry(
                               new IngameSettings.IngameSettingsEntry()
                               {
                                   entry = Enabled,
                                   name_cn = "自动准备",
                                   name_eng = "Auto Ready"
                               }
                                          );
        }

        public AutoReady() : base(ClassInjector.DerivedConstructorPointer<AutoReady>()) => ClassInjector.DerivedConstructorBody(this);
        public static void Init()
        {
            GameObject ML_Manager = GameObject.Find("ML_Manager");
            if (ML_Manager == null)
            {
                ML_Manager = new GameObject("ML_Manager");
                DontDestroyOnLoad(ML_Manager);
            }

            if (ML_Manager.GetComponent<AutoReady>() == null)
            {
                Instance = ML_Manager.AddComponent<AutoReady>();
            }
        }

        [HarmonyPatch(typeof(LobbySceneHandler), nameof(LobbySceneHandler.Update))]
        public class LobbySceneHandler_Update
        {
            static void Postfix(LobbySceneHandler __instance)
            {
                if (!Enabled.Value)
                {
                    return;
                }

                if (__instance.gameStarted)
                {
                    return;
                }

                LobbySceneHandler.ANEPPLAPGFB readyState = __instance.readyState;
                if (readyState != LobbySceneHandler.ANEPPLAPGFB.Ready)
                {
                    //游戏会检测是否鼠标被点击
                    __instance.mouseClicked = true;
                    __instance.ChangeReadyState();
                }
            }
        }
    }
}
