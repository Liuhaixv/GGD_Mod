using MelonLoader;
using UnhollowerRuntimeLib;
using UnityEngine;
using HarmonyLib;

using IntPtr = System.IntPtr;

namespace GGD_Hack.Features
{
    [RegisterTypeInIl2Cpp]
    public class AutoNavigateLocalPlayerToPosition : MonoBehaviour
    {
        public static AutoNavigateLocalPlayerToPosition Instance;
        public static MelonPreferences_Entry<bool> Enabled = MelonPreferences.CreateEntry<bool>("GGDH", "Enable_" + nameof(AutoNavigateLocalPlayerToPosition), true);

        public AutoNavigateLocalPlayerToPosition(IntPtr ptr) : base(ptr)
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

        public AutoNavigateLocalPlayerToPosition() : base(ClassInjector.DerivedConstructorPointer<AutoNavigateLocalPlayerToPosition>()) => ClassInjector.DerivedConstructorBody(this);
        public static void Init()
        {
            GameObject ML_Manager = GameObject.Find("ML_Manager");
            if (ML_Manager == null)
            {
                ML_Manager = new GameObject("ML_Manager");
                DontDestroyOnLoad(ML_Manager);
            }

            if (ML_Manager.GetComponent<AutoNavigateLocalPlayerToPosition>() == null)
            {
                Instance = ML_Manager.AddComponent<AutoNavigateLocalPlayerToPosition>();
            }
        }

        //加载地图中所有的碰撞体
        private void InitColliders()
        {

        }
    }
}