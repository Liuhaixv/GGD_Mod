using Handlers.GameHandlers.PlayerHandlers;
using HarmonyLib;
using UnityEngine;

//在有炸弹的人头上显示一个炸弹
namespace GGD_Hack.Features
{
    public class BombIndicator
    {
        public static Sprite Bomb
        {
            get
            {
                return Utils.SpriteUtil.GetSpriteFromImageName("bomb.png");
            }
        } 

        [HarmonyPatch(typeof(PlayerController),nameof(PlayerController.Start))]
        public class AutoAddBombSprite
        {
            static void Postfix(PlayerController __instance)
            {
                GameObject player = __instance.gameObject;

                GameObject bomb = new GameObject("BombSprite");
                bomb.transform.SetParent(player.transform);

                SpriteRenderer bombSpriteRenderer = bomb.AddComponent<SpriteRenderer>();
                bombSpriteRenderer.sprite = Bomb;

                //放在玩家头上
                //bomb.transform.localPosition = new Vector3(-0.8f, 0.2f, 1.0f);

                //放在手里
                {
                    bomb.transform.localPosition = new Vector3(0.1f, -1.1f, 1.0f);
                    bomb.transform.localScale = new Vector3(-0.3f, 0.3f, 1);
                }
            }
        }

        [HarmonyPatch(typeof(PlayerController), nameof(PlayerController.Update))]
        public class UpdateBombSprite
        {
            static void Postfix(PlayerController __instance)
            {
                GameObject player = __instance.gameObject;

                GameObject bomb = player.transform.Find("BombSprite").gameObject;

                bomb.SetActive(__instance.hasBomb);
            }
        }
    }
}
