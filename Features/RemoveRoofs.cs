using Handlers.GameHandlers;
using HarmonyLib;
using MelonLoader;
using UnhollowerRuntimeLib;
using UnityEngine;
using static MelonLoader.MelonLogger;
using IntPtr = System.IntPtr;
namespace GGD_Hack.Features
{
    [RegisterTypeInIl2Cpp]
    public class RemoveRoofs : MonoBehaviour 
    {
        public static RemoveRoofs Instance;
        public static MelonPreferences_Entry<bool> Enabled = MelonPreferences.CreateEntry<bool>("GGDH", "Enable_" + nameof(RemoveRoofs), true);

        public RemoveRoofs(IntPtr ptr) : base(ptr)
        {
            IngameSettings.AddIngameSettingsEntry(
                               new IngameSettings.IngameSettingsEntry()
                               {
                    entry = Enabled,
                    name_cn = "移除屋顶",
                    name_eng = "Remove Roofs"
                }
                                          );
        }
        // Optional, only used in case you want to instantiate this class in the mono-side
        // Don't use this on MonoBehaviours / Components!
        public RemoveRoofs() : base(ClassInjector.DerivedConstructorPointer<RemoveRoofs>()) => ClassInjector.DerivedConstructorBody(this);
        public static void Init()
        {
            GameObject ML_Manager = GameObject.Find("ML_Manager");
            if (ML_Manager == null)
            {
                ML_Manager = new GameObject("ML_Manager");
                DontDestroyOnLoad(ML_Manager);
            }

            if (ML_Manager.GetComponent<RemoveRoofs>() == null)
            {
                Instance = ML_Manager.AddComponent<RemoveRoofs>();
            }
        }

        private static void RemoveAllRoofs()
        {
            //移除屋顶

            GameObject roofs = GameObject.Find("Roofs");
            if (roofs != null)
            {
                roofs.SetActive(false);
                MelonLogger.Msg(System.ConsoleColor.Green, "屋顶已移除");
            }
            else
            {
                MelonLogger.Msg(System.ConsoleColor.Green, "该地图未找到屋顶，无需移除");
            }

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
