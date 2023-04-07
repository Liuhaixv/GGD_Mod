#if false
using MelonLoader;
using UnhollowerRuntimeLib;
using UnityEngine;
using HarmonyLib;

using IntPtr = System.IntPtr;
using GGD_Hack.Events;

namespace GGD_Hack.Features
{
    [RegisterTypeInIl2Cpp]
    public class ForceEnableCelebrityDeath : MonoBehaviour
    {
        public static ForceEnableCelebrityDeath Instance;
        public static MelonPreferences_Entry<bool> Enabled = MelonPreferences.CreateEntry<bool>("GGDH", "Enable_" + nameof(ForceEnableCelebrityDeath), true);

        public ForceEnableCelebrityDeath(IntPtr ptr) : base(ptr)
        {
            IngameSettings.AddIngameSettingsEntry(
                               new IngameSettings.IngameSettingsEntry()
                               {
                                   entry = Enabled,
                                   name_cn = "强制提示网红死亡",
                                   name_eng = "Force Enable Celebrity Death Alarm"
                               }
                                          );
        }

        public ForceEnableCelebrityDeath() : base(ClassInjector.DerivedConstructorPointer<ForceEnableCelebrityDeath>()) => ClassInjector.DerivedConstructorBody(this);
        public static void Init()
        {
            GameObject ML_Manager = GameObject.Find("ML_Manager");
            if (ML_Manager == null)
            {
                ML_Manager = new GameObject("ML_Manager");
                DontDestroyOnLoad(ML_Manager);
            }

            if (ML_Manager.GetComponent<ForceEnableCelebrityDeath>() == null)
            {
                Instance = ML_Manager.AddComponent<ForceEnableCelebrityDeath>();
            }
        }

        [HarmonyPatch(typeof(InGameEvents),nameof(InGameEvents.Celebrity_Died))]
        class InGameEvents_Celebrity_Died
        {
            static void Postfix()
            {
                if (!Enabled.Value) return;

                Handlers.LobbyHandlers.LobbySceneHandler.Instance.CallTributePanel(null, true);
                Handlers.CommonHandlers.SoundHandler.Instance.PlayCelebrityDeath();
            }
        }
    }
}
#endif