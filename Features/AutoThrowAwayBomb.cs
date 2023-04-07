using Handlers.GameHandlers.PlayerHandlers;
using Handlers.LobbyHandlers;
using Il2CppSystem;
using MelonLoader;
using UnhollowerRuntimeLib;
using UnityEngine;
using IntPtr = System.IntPtr;

namespace GGD_Hack.Features
{
    [RegisterTypeInIl2Cpp]
    public class AutoThrowAwayBomb : MonoBehaviour
    {
        public static AutoThrowAwayBomb Instance;
        public static MelonPreferences_Entry<bool> Enabled = MelonPreferences.CreateEntry<bool>("GGDH", "Enable_" + nameof(AutoThrowAwayBomb), true);
        //public static MelonPreferences_Entry<float> ThrowAwayBombWhenLessThanSeconds = MelonPreferences.CreateEntry<float>("GGDH", nameof(AutoThrowAwayBomb) + "_" + nameof(ThrowAwayBombWhenLessThanSeconds), 1.0f);

        public AutoThrowAwayBomb(IntPtr ptr) : base(ptr)
        {
            IngameSettings.AddIngameSettingsEntry(
                               new IngameSettings.IngameSettingsEntry()
                               {
                                   entry = Enabled,
                                   name_cn = "自动扔掉手上的炸弹",
                                   name_eng = "Auto Throw Away Bomb"
                               }
                                          );
        }

        public AutoThrowAwayBomb() : base(ClassInjector.DerivedConstructorPointer<AutoThrowAwayBomb>()) => ClassInjector.DerivedConstructorBody(this);
        public static void Init()
        {
            GameObject ML_Manager = GameObject.Find("ML_Manager");
            if (ML_Manager == null)
            {
                ML_Manager = new GameObject("ML_Manager");
                DontDestroyOnLoad(ML_Manager);
            }

            if (ML_Manager.GetComponent<AutoThrowAwayBomb>() == null)
            {
                Instance = ML_Manager.AddComponent<AutoThrowAwayBomb>();
            }
        }

        private void Update()
        {
            if (!Enabled.Value) return;
            if (LocalPlayer.Instance?.Player == null) return;
            if (!LobbySceneHandler.Instance?.gameStarted ?? true) return;

            if (LocalPlayer.Instance.Player.hasBomb)
            {
                Features.Bomb.ThrowAwayBomb();
                LocalPlayer.Instance.Player.hasBomb = false;
            }
        }
    }
}