using MelonLoader;
using UnhollowerRuntimeLib;
using UnityEngine;
using HarmonyLib;

using IntPtr = System.IntPtr;

namespace GGD_Hack.Features
{
    [RegisterTypeInIl2Cpp]
    public class UpdateVotePrefabNameColor : MonoBehaviour
    {
        public static UpdateVotePrefabNameColor Instance;
        public static MelonPreferences_Entry<bool> Enabled = MelonPreferences.CreateEntry<bool>("GGDH", "Enable_" + nameof(UpdateVotePrefabNameColor), true);

        public UpdateVotePrefabNameColor(IntPtr ptr) : base(ptr)
        {/*
            IngameSettings.AddIngameSettingsEntry(
                               new IngameSettings.IngameSettingsEntry()
                               {
                                   entry = Enabled,
                                   name_cn = "箭头自动追踪尸体",
                                   name_eng = "Auto Track Bodies"
                               }
                                          );*/
        }

        public UpdateVotePrefabNameColor() : base(ClassInjector.DerivedConstructorPointer<UpdateVotePrefabNameColor>()) => ClassInjector.DerivedConstructorBody(this);
        public static void Init()
        {
            GameObject ML_Manager = GameObject.Find("ML_Manager");
            if (ML_Manager == null)
            {
                ML_Manager = new GameObject("ML_Manager");
                DontDestroyOnLoad(ML_Manager);
            }

            if (ML_Manager.GetComponent<UpdateVotePrefabNameColor>() == null)
            {
                Instance = ML_Manager.AddComponent<UpdateVotePrefabNameColor>();
            }
        }
    }
}