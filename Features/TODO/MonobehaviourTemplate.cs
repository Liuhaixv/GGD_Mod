#if false
using MelonLoader;
using UnhollowerRuntimeLib;
using UnityEngine;

using IntPtr = System.IntPtr;

namespace GGD_Hack.Features
{
    [RegisterTypeInIl2Cpp]
    public class MonobehaviourTemplate : MonoBehaviour
    {
        public static MonobehaviourTemplate Instance;
        public static MelonPreferences_Entry<bool> Enabled = MelonPreferences.CreateEntry<bool>("GGDH", "Enable_" + nameof(MonobehaviourTemplate), true);

        public MonobehaviourTemplate(IntPtr ptr) : base(ptr)
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

        public MonobehaviourTemplate() : base(ClassInjector.DerivedConstructorPointer<MonobehaviourTemplate>()) => ClassInjector.DerivedConstructorBody(this);
        public static void Init()
        {
            GameObject ML_Manager = GameObject.Find("ML_Manager");
            if (ML_Manager == null)
            {
                ML_Manager = new GameObject("ML_Manager");
                DontDestroyOnLoad(ML_Manager);
            }

            if (ML_Manager.GetComponent<MonobehaviourTemplate>() == null)
            {
                Instance = ML_Manager.AddComponent<MonobehaviourTemplate>();
            }
        }
    }
}
#endif