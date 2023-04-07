using MelonLoader;
using UnhollowerRuntimeLib;
using UnityEngine;
using HarmonyLib;

using IntPtr = System.IntPtr;
using Handlers.LobbyHandlers;
using Handlers.GameHandlers.PlayerHandlers;
using Handlers.CommonHandlers;
using GGD_Hack.Events;

namespace GGD_Hack.Features
{
    [RegisterTypeInIl2Cpp]
    public class DeathAlarm : MonoBehaviour
    {
        public static DeathAlarm Instance;
        public static MelonPreferences_Entry<bool> Enabled = MelonPreferences.CreateEntry<bool>("GGDH", "Enable_" + nameof(DeathAlarm), true);

        public DeathAlarm(IntPtr ptr) : base(ptr)
        {
            IngameSettings.AddIngameSettingsEntry(
                               new IngameSettings.IngameSettingsEntry()
                               {
                                   entry = Enabled,
                                   name_cn = "死亡播报",
                                   name_eng = "Death Alarm"
                               }
                                          );
        }

        public DeathAlarm() : base(ClassInjector.DerivedConstructorPointer<DeathAlarm>()) => ClassInjector.DerivedConstructorBody(this);
        public static void Init()
        {
            GameObject ML_Manager = GameObject.Find("ML_Manager");
            if (ML_Manager == null)
            {
                ML_Manager = new GameObject("ML_Manager");
                DontDestroyOnLoad(ML_Manager);
            }

            if (ML_Manager.GetComponent<DeathAlarm>() == null)
            {
                Instance = ML_Manager.AddComponent<DeathAlarm>();
            }
        }

        private static void ShowPlayerDeathPanel(PlayerController player = null)
        {
            LobbySceneHandler.Instance?.CallTributePanel(player, false);

            //播放声音
            {
                SoundHandler instance = SoundHandler.Instance;
                instance.mainSFXAudioSource.PlayOneShot(instance.DndGooseDead);
            }
        }

        [HarmonyPatch(typeof(InGameEvents),nameof(InGameEvents.Receive_Kill))]
        class InGameEvents_Receive_Kill
        {
            static void Postfix(string killerUserId, string killedUserId, string stingerId)
            {
                if(!Enabled.Value) return;

                ShowPlayerDeathPanel(PlayerController.playersList?[killedUserId]);
            }
        }
    }
}