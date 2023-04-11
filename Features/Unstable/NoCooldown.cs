#if false//TODO: 暂时禁用，计划只针对部分角色开放，如本地技能的角色
using MelonLoader;
using UnhollowerRuntimeLib;
using UnityEngine;
using HarmonyLib;

using IntPtr = System.IntPtr;
using GGD_Hack.Internal.Buttons;

namespace GGD_Hack.Features
{
    [RegisterTypeInIl2Cpp]
    public class NoCooldown : MonoBehaviour
    {
        public static NoCooldown Instance;
        public static MelonPreferences_Entry<bool> Enabled = MelonPreferences.CreateEntry<bool>("GGDH", "Enable_" + nameof(NoCooldown), false);

        public NoCooldown(IntPtr ptr) : base(ptr)
        {
            IngameSettings.AddIngameSettingsEntry(
                               new IngameSettings.IngameSettingsEntry()
                               {
                                   entry = Enabled,
                                   name_cn = "移除技能CD",
                                   name_eng = "No Cooldown"
                               }
                                          );
        }

        public NoCooldown() : base(ClassInjector.DerivedConstructorPointer<NoCooldown>()) => ClassInjector.DerivedConstructorBody(this);
        public static void Init()
        {
            GameObject ML_Manager = GameObject.Find("ML_Manager");
            if (ML_Manager == null)
            {
                ML_Manager = new GameObject("ML_Manager");
                DontDestroyOnLoad(ML_Manager);
            }

            if (ML_Manager.GetComponent<NoCooldown>() == null)
            {
                Instance = ML_Manager.AddComponent<NoCooldown>();
            }
        }

        [HarmonyPatch(typeof(PlayerButtonController), nameof(PlayerButtonController.RegisterCooldownButton))]
        class RegisterCooldownButton_
        {
            static void Prefix(PlayerButtonController __instance, UICooldownButton __result, string __0, JLACBPCNIOP __1, UnityEngine.Transform __2,ref float __3,bool __4)
            {
                if(!Enabled.Value) return;
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

                    //移除CD
                    __3 = 0.1f;

                    if (__4)
                    {
                        MelonLogger.Msg(System.ConsoleColor.Green, "已移除按钮CD");
                    
                    }
                }
                catch (System.Exception ex)
                {
                    MelonLogger.Warning(ex.Message);
                }
            }
        }
    }
}
#endif