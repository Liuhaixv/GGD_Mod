
using MelonLoader;
using UnhollowerRuntimeLib;
using UnityEngine;
using HarmonyLib;

using IntPtr = System.IntPtr;
using Handlers.GameHandlers.PlayerHandlers;
using Handlers.LobbyHandlers;
using Handlers.GameHandlers.TaskHandlers;
using Handlers.GameHandlers;

namespace GGD_Hack.Features
{
    [RegisterTypeInIl2Cpp]
    public class InfiniteSkillDistance : MonoBehaviour
    {
        public static InfiniteSkillDistance Instance;
        public static MelonPreferences_Entry<bool> Enabled = MelonPreferences.CreateEntry<bool>("GGDH", "Enable_" + nameof(InfiniteSkillDistance), true);

        public InfiniteSkillDistance(IntPtr ptr) : base(ptr)
        {
            IngameSettings.AddIngameSettingsEntry(
                               new IngameSettings.IngameSettingsEntry()
                               {
                                   entry = Enabled,
                                   name_cn = "无限距离技能: \n鸽子、殡仪、侦探、跟踪者、爆炸王、秃鹫、超能力",
                                   name_eng = "Skill Ignore Distance: \nPigeon, Mortician, Detective, Stalker, Demolitionist, Vulture, Esper"
                               }
                                          );
        }

        public InfiniteSkillDistance() : base(ClassInjector.DerivedConstructorPointer<InfiniteSkillDistance>()) => ClassInjector.DerivedConstructorBody(this);
        public static void Init()
        {
            GameObject ML_Manager = GameObject.Find("ML_Manager");
            if (ML_Manager == null)
            {
                ML_Manager = new GameObject("ML_Manager");
                DontDestroyOnLoad(ML_Manager);
            }

            if (ML_Manager.GetComponent<InfiniteSkillDistance>() == null)
            {
                Instance = ML_Manager.AddComponent<InfiniteSkillDistance>();
            }
        }

        //Handlers_GameHandlers_PlayerHandlers_LocalPlayer__TriggerEnter
        [HarmonyPatch(typeof(PlayerController), nameof(PlayerController.Update))]
        class PlayerController_Update
        {
            static void Postfix(PlayerController __instance)
            {
                if (!Enabled.Value)
                {
                    return;
                }

                //跳过本地玩家
                if (__instance.isLocal)
                {
                    return;
                }

                //调用其他玩家进入本地玩家范围的函数
                PlayerController localPlayerController = LocalPlayer.Instance?.Player;

                if (localPlayerController == null) { return; }

                if (!(LobbySceneHandler.Instance?.gameStarted ?? false))
                {
                    return;
                }

                if (localPlayerController.playerRole == null)
                {
                    return;
                }

                //仅限以下角色启用
                {
                    IPLJDOHJOLM playerRoleId = localPlayerController.playerRole.IJOICOIDMHC;
                    if (playerRoleId != IPLJDOHJOLM.Pigeon
                        && playerRoleId != IPLJDOHJOLM.Mortician
                        && playerRoleId != IPLJDOHJOLM.Detective
                        && playerRoleId != IPLJDOHJOLM.Stalker
                        && playerRoleId != IPLJDOHJOLM.Demolitionist
                        && playerRoleId != IPLJDOHJOLM.Vulture
                         && playerRoleId != IPLJDOHJOLM.DNDVulture
                         && playerRoleId != IPLJDOHJOLM.EsperDuck)
                    {
#if Developer
                        MelonLogger.Error("玩家角色不符合无限距离");
#endif
                        return;
                    }
                }

#if Developer
                //MelonLogger.Msg(System.ConsoleColor.Green, "即将遍历所有玩家");
#endif


                LocalPlayer.Instance.TriggerEnter(
                        __instance.gameObject.transform.Find("Colliders").gameObject.GetComponent<CircleCollider2D>()
                    );
            }
        }

        //尸体碰撞自己
        [HarmonyPatch(typeof(BodyHandler), nameof(BodyHandler.FixedUpdate))]
        public class BodyHandler_Update
        {
            static void Postfix(BodyHandler __instance)
            {
                if (!Enabled.Value)
                {
                    return;
                }

                //跳过专杀尸体，防止直接报警
                if (__instance.killedByProfessional)
                {
                    return;
                }

                PolygonCollider2D polygonCollider2D = __instance.gameObject.GetComponent<PolygonCollider2D>();
                if (polygonCollider2D != null)
                    LocalPlayer.Instance.TriggerEnter(__instance.gameObject.GetComponent<PolygonCollider2D>());
                // LocalPlayer.Instance.TriggerEnter(__instance.gameObject.GetComponent<BoxCollider2D>());
            }
        }
    }
}