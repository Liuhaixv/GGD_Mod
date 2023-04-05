
using MelonLoader;
using UnhollowerRuntimeLib;
using UnityEngine;
using HarmonyLib;

using IntPtr = System.IntPtr;
using Handlers.GameHandlers.PlayerHandlers;

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
                                   name_cn = "无限距离技能: \n鸽子、殡仪、侦探、跟踪者、爆炸王",                                   
                                   name_eng = "Skill Ignore Distance: \nPigeon, Mortician, Detective, Stalker, Demolitionist"
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
        [HarmonyPatch(typeof(LocalPlayer),nameof(LocalPlayer.Update))]
        class LocalPlayer_Update
        {
            static void Postfix(LocalPlayer __instance)
            {
                if(!Enabled.Value)
                {
                    return;
                }

                //调用其他玩家进入本地玩家范围的函数
                LocalPlayer localplayer = LocalPlayer.Instance;

                foreach(var otherPlayer in PlayerController.playersList.Values)
                {
                    if (otherPlayer == null || otherPlayer.isLocal) continue;

                    localplayer.TriggerEnter(
                            otherPlayer.gameObject.transform.Find("Colliders").gameObject.GetComponent<CircleCollider2D>()
                        ) ;
                }
            }
        }
    }
}