using Handlers.GameHandlers.PlayerHandlers;
using HarmonyLib;
using MelonLoader;
using UnhollowerRuntimeLib;
using UnityEngine;
using static MelonLoader.MelonLogger;
using IntPtr = System.IntPtr;
//在有炸弹的人头上显示一个炸弹
namespace GGD_Hack.Features
{
    [RegisterTypeInIl2Cpp]
    public class BombIndicator : MonoBehaviour
    {
        public static BombIndicator Instance;
        public static MelonPreferences_Entry<bool> Enabled = MelonPreferences.CreateEntry<bool>("GGDH", "Enable_" + nameof(BombIndicator), true);

        private const string bombSpriteName = "BombSprite";

        public static Sprite Bomb
        {
            get
            {
                return Utils.SpriteUtil.GetSpriteFromImageName("bomb.png");
            }
        }

        public BombIndicator(IntPtr ptr) : base(ptr)
        {
            /*
            IngameSettings.AddIngameSettingsEntry(
                        new IngameSettings.IngameSettingsEntry()
                        {
                            entry = Enabled,
                            name_cn = "显示炸弹指示",
                            name_eng = "Show Bomb Indicator"
                        }
                    );
            */
        }
        public BombIndicator() : base(ClassInjector.DerivedConstructorPointer<BombIndicator>()) => ClassInjector.DerivedConstructorBody(this);
        public static void Init()
        {
            GameObject ML_Manager = GameObject.Find("ML_Manager");
            if (ML_Manager == null)
            {
                ML_Manager = new GameObject("ML_Manager");
                DontDestroyOnLoad(ML_Manager);
            }

            if (ML_Manager.GetComponent<BombIndicator>() == null)
            {
                Instance = ML_Manager.AddComponent<BombIndicator>();
            }
        }

        [HarmonyPatch(typeof(PlayerController), nameof(PlayerController.Start))]
        public class AutoAddBombSprite
        {
            static void Postfix(PlayerController __instance)
            {
                if (!Enabled.Value) return;

                GameObject player = __instance.gameObject;

                GameObject bomb = new GameObject(bombSpriteName);
                bomb.transform.SetParent(player.transform);

                SpriteRenderer bombSpriteRenderer = bomb.AddComponent<SpriteRenderer>();
                bombSpriteRenderer.sprite = Bomb;

                //放在玩家头上
                //bomb.transform.localPosition = new Vector3(-0.8f, 0.2f, 1.0f);

                //放在手里
                {
                    bomb.transform.localPosition = new Vector3(0.05f, -1.1f, 1f);
                    bomb.transform.localScale = new Vector3(-0.3f, 0.3f, 1);
                }
            }
        }

        [HarmonyPatch(typeof(PlayerController), nameof(PlayerController.Update))]
        public class UpdateBombSprite
        {
            static void Postfix(PlayerController __instance)
            {
                if (!Enabled.Value) return;

                GameObject player = __instance.gameObject;

                GameObject bomb = player.transform.Find(bombSpriteName).gameObject;

                bomb.SetActive(__instance.hasBomb);
            }
        }
    }  
}
