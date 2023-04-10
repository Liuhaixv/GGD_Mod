using MelonLoader;
using UnhollowerRuntimeLib;
using UnityEngine;
using HarmonyLib;

using IntPtr = System.IntPtr;

namespace GGD_Hack.Features
{
    [RegisterTypeInIl2Cpp]
    public class DecreaseCooldown : MonoBehaviour
    {
        public static DecreaseCooldown Instance;
        public static MelonPreferences_Entry<bool> Enabled = MelonPreferences.CreateEntry<bool>("GGDH", "Enable_" + nameof(DecreaseCooldown), true);
        public static MelonPreferences_Entry<float> DecreasedCooldown = MelonPreferences.CreateEntry<float>("GGDH", nameof(DecreaseCooldown) + "_" + nameof(DecreasedCooldown), 3.0f);

        public DecreaseCooldown(IntPtr ptr) : base(ptr)
        {
            IngameSettings.AddIngameSettingsEntry(
                               new IngameSettings.IngameSettingsEntry()
                               {
                                   entry = Enabled,
                                   name_cn =string.Format( "微调CD时间", DecreasedCooldown.Value),
                                   name_eng = string.Format("Decrease CD(Current value:{0} secs)", DecreasedCooldown.Value),
                               }
                                          );
        }

        public DecreaseCooldown() : base(ClassInjector.DerivedConstructorPointer<DecreaseCooldown>()) => ClassInjector.DerivedConstructorBody(this);
        public static void Init()
        {
            GameObject ML_Manager = GameObject.Find("ML_Manager");
            if (ML_Manager == null)
            {
                ML_Manager = new GameObject("ML_Manager");
                DontDestroyOnLoad(ML_Manager);
            }

            if (ML_Manager.GetComponent<DecreaseCooldown>() == null)
            {
                Instance = ML_Manager.AddComponent<DecreaseCooldown>();
            }
        }

        [HarmonyPatch(typeof(PlayerButtonController), nameof(PlayerButtonController.RegisterCooldownButton))]
        class RegisterCooldownButton_
        {
            static void Prefix(PlayerButtonController __instance, UICooldownButton __result, string __0, KLKBPINDIGM __1, UnityEngine.Transform __2, ref float __3, bool __4)
            {
                if (!Enabled.Value) return;
                //
                //  --------------------
                //    UICooldownButton PlayerButtonController::RegisterCooldownButton(string AKMFDDABJMI, JGEEBFIBHGD KIDEMMMFKPP, UnityEngine.Transform AJHOFGOFCKM, float PMPIAPCEKCO, bool LPAEPADMEFM)
                //    - __instance: UIButtons (PlayerButtonController)
                //     - Parameter 0 'AKMFDDABJMI': bomb
                //     - Parameter 1 'KIDEMMMFKPP': Use
                //     - Parameter 2 'AJHOFGOFCKM': UseAnchor (UnityEngine.RectTransform)
                //     - Parameter 3 'PMPIAPCEKCO': 3
                //     - Parameter 4 'LPAEPADMEFM': True
                //      - Return value: bomb (UICooldownButton)
                //  --------------------
                //      UICooldownButton PlayerButtonController::RegisterCooldownButton(string AKMFDDABJMI, JGEEBFIBHGD KIDEMMMFKPP, UnityEngine.Transform AJHOFGOFCKM, float PMPIAPCEKCO, bool LPAEPADMEFM)
                //      - __instance: UIButtons (PlayerButtonController)
                //       - Parameter 0 'AKMFDDABJMI': kill
                //       - Parameter 1 'KIDEMMMFKPP': Primary
                //       - Parameter 2 'AJHOFGOFCKM': PrimaryRoleActionAnchor (UnityEngine.RectTransform)
                //       - Parameter 3 'PMPIAPCEKCO': 10
                //       - Parameter 4 'LPAEPADMEFM': True
                //        - Return value: kill (UICooldownButton)

                try
                {
                    string gameObjectName = __0;
                    int keybind = (int)__1;
                    Transform transform = __2;
                    float cooldownTime = __3;
                    bool noCooldownsOnClient = __4;

                    //减少CD
                    __3 = __3 - DecreasedCooldown.Value;
                }
                catch (System.Exception ex)
                {
                    MelonLogger.Warning(ex.Message);
                }
            }
        }
    }
}