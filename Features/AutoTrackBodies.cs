using MelonLoader;
using UnhollowerRuntimeLib;
using UnityEngine;

using IntPtr = System.IntPtr;

namespace GGD_Hack.Features
{
    [RegisterTypeInIl2Cpp]
    public class AutoTrackBodies : MonoBehaviour
    {
        public static AutoTrackBodies Instance;
        public static MelonPreferences_Entry<bool> Enabled = MelonPreferences.CreateEntry<bool>("GGDH", "Enable_" + nameof(AutoTrackBodies), true);

        public AutoTrackBodies(IntPtr ptr) : base(ptr)
        {
            IngameSettings.AddIngameSettingsEntry(
                               new IngameSettings.IngameSettingsEntry()
                               {
                                   entry = Enabled,
                                   name_cn = "箭头自动追踪尸体",
                                   name_eng = "Auto Track Bodies"
                               }
                                          );
        }

        public AutoTrackBodies() : base(ClassInjector.DerivedConstructorPointer<AutoTrackBodies>()) => ClassInjector.DerivedConstructorBody(this);
        public static void Init()
        {
            GameObject ML_Manager = GameObject.Find("ML_Manager");
            if (ML_Manager == null)
            {
                ML_Manager = new GameObject("ML_Manager");
                DontDestroyOnLoad(ML_Manager);
            }

            if (ML_Manager.GetComponent<AutoTrackBodies>() == null)
            {
                Instance = ML_Manager.AddComponent<AutoTrackBodies>();
            }
        }
    }
}
