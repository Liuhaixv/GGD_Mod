using MelonLoader;
using UnhollowerRuntimeLib;
using UnityEngine;
using HarmonyLib;

using IntPtr = System.IntPtr;
using Handlers.GameHandlers.PlayerHandlers;
using Handlers.LobbyHandlers;

namespace GGD_Hack.Features
{
    [RegisterTypeInIl2Cpp]
    public class ForceEnableRoleRevealText : MonoBehaviour
    {
        public static ForceEnableRoleRevealText Instance;
        public static MelonPreferences_Entry<bool> Enabled = MelonPreferences.CreateEntry<bool>("GGDH", "Enable_" + nameof(ForceEnableRoleRevealText), true);

        public ForceEnableRoleRevealText(IntPtr ptr) : base(ptr)
        {
            IngameSettings.AddIngameSettingsEntry(
                               new IngameSettings.IngameSettingsEntry()
                               {
                                   entry = Enabled,
                                   name_cn = "显示角色身份文字在姓名下方",
                                   name_eng = "Show role text below name"
                               }
                                          );
        }

        public ForceEnableRoleRevealText() : base(ClassInjector.DerivedConstructorPointer<ForceEnableRoleRevealText>()) => ClassInjector.DerivedConstructorBody(this);
        public static void Init()
        {
            GameObject ML_Manager = GameObject.Find("ML_Manager");
            if (ML_Manager == null)
            {
                ML_Manager = new GameObject("ML_Manager");
                DontDestroyOnLoad(ML_Manager);
            }

            if (ML_Manager.GetComponent<ForceEnableRoleRevealText>() == null)
            {
                Instance = ML_Manager.AddComponent<ForceEnableRoleRevealText>();
            }
        }

        [HarmonyPatch(typeof(PlayerController), nameof(PlayerController.Update))]
        class PlayerController_Update
        {
            static void Postfix(PlayerController __instance)
            {
                if (__instance.isLocal)
                {
                    return;
                }

                if (!LobbySceneHandler.Instance.gameStarted)
                {
                    return;
                }

                __instance.playerNameRoleText.enabled = true;
            }
        }
    }
}