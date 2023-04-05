using Handlers.GameHandlers.SpecialBehaviour;
using Il2CppSystem.Collections.Generic;
using UnhollowerRuntimeLib;
using UnityEngine;
using HarmonyLib;

using IntPtr = System.IntPtr;
using MelonLoader;
using Handlers.GameHandlers.PlayerHandlers;
using static MelonLoader.MelonLogger;
using Handlers.LobbyHandlers;
using Il2CppSystem.Threading.Tasks;
using UnityEngine.UI;
using Photon.Pun.Demo.Procedural;
using TMPro;
using GGD_Hack.Events;

namespace GGD_Hack.Features
{
    [RegisterTypeInIl2Cpp]
    public class MinimapESP : MonoBehaviour
    {
#if Legit
        public static MelonPreferences_Entry<bool> Enabled = MelonPreferences.CreateEntry<bool>("GGDH", "Enable_" + nameof(MinimapESP), false);

#else
        public static MelonPreferences_Entry<bool> Enabled = MelonPreferences.CreateEntry<bool>("GGDH", "Enable_" + nameof(MinimapESP), true);
#endif

        //是否已经初始化所有玩家的点位
        public static bool instantiatedAllPlayers = false;

        public static MinimapESP Instance = null;

        //通过hook更新
        public static MiniMapHandler miniMapHandler = null;

        //玩家userId对应的GameObject，表示地图上的点
        private static Dictionary<string, GameObject> playersOnMinimap = new Dictionary<string, GameObject>();

        public MinimapESP(IntPtr ptr) : base(ptr)
        {
            IngameSettings.AddIngameSettingsEntry(
                new IngameSettings.IngameSettingsEntry()
                {
                    entry = Enabled,
                    name_cn = "Minimap ESP 透视",
                    name_eng = "Minimap ESP"
                }
                );
        }

        // Optional, only used in case you want to instantiate this class in the mono-side
        // Don't use this on MonoBehaviours / Components!
        public MinimapESP() : base(ClassInjector.DerivedConstructorPointer<MinimapESP>()) => ClassInjector.DerivedConstructorBody(this);

        public static void Init()
        {
            GameObject ML_Manager = GameObject.Find("ML_Manager");
            if (ML_Manager == null)
            {
                ML_Manager = new GameObject("ML_Manager");
                DontDestroyOnLoad(ML_Manager);
            }

            if (ML_Manager.GetComponent<MinimapESP>() == null)
            {
                Instance = ML_Manager.AddComponent<MinimapESP>();
            }
        }

        /// <summary>
        /// 初始化所有玩家在地图上的点位（除了本地玩家）
        /// </summary>
        private static void InstantiateAllPlayers()
        {

            MelonLogger.Msg("正在初始化所有玩家的minimap点位");
            //清空之前的玩家列表
            DestroyAllPlayers();

            //准备克隆本地玩家的点位
            //GameObject targetMe = Utils.GameInstances.FindGameObjectByPath("Canvas/MiniMap/Panel/Target Me");
            GameObject targetMe = LobbySceneHandler.Instance?.miniMapHandler?.gameObject.transform.Find("Panel/Target Me")?.gameObject ?? null;

            if (targetMe == null)
            {
                MelonLogger.Warning("[MinimapESP] 未找到Target Me对象，初始化所有玩家点位失败!");
                return;
            }

            //遍历
            foreach (var entry in PlayerController.playersList)
            {
                var playerController = entry.Value;

                //跳过null和本地玩家
                if (playerController == null || playerController.isLocal) continue;

                GameObject clone = Object.Instantiate(targetMe, targetMe.transform.parent);
                clone.name = playerController.userId;

                //摧毁动画闪烁效果
                DestroyImmediate(clone.GetComponent<Animator>());

                //修改点和名字间连线的颜色
                clone.transform.Find("Text").gameObject.GetComponent<TextMeshProUGUI>().color = Color.white;

                //修改颜色
                UpdatePlayerColorAndName(clone, playerController);

                playersOnMinimap.Add(playerController.userId, clone);
            }

            MelonLogger.Msg("已初始化所有玩家的minimap点位");
            MinimapESP.instantiatedAllPlayers = true;
        }

        /// <summary>
        /// 修改名字
        /// </summary>
        /// <param name="playerOnMinimap"></param>
        /// <param name="playerController"></param>
        private static void UpdatePlayerColorAndName(GameObject playerOnMinimap, PlayerController playerController)
        {
            Transform youTransform = playerOnMinimap.transform.Find("You");

            Image sphere = playerOnMinimap.GetComponent<Image>();

            if (youTransform == null)
            {
                MelonLogger.Warning("找不到 You");
            }

            if (sphere == null)
            {
                MelonLogger.Warning("找不到 Image");
            }

            TMPro.TextMeshProUGUI textMeshProUGUI = youTransform.gameObject.GetComponent<TMPro.TextMeshProUGUI>();

            //修改文字
            if (textMeshProUGUI != null)
            {
                //鹈鹕粉色
                if (playerController.isInPelican)
                {
                    Color pink = new Color(1.0f, 0f, 1.0f);

                    textMeshProUGUI.color = pink;
                    textMeshProUGUI.outlineColor = Color.white;
                    //sphere.color = pink;
                }
                else

                //死亡黑色
                if (playerController.timeOfDeath > 0)
                {
                    Color black = Color.black;

                    textMeshProUGUI.color = black;
                    textMeshProUGUI.outlineColor = Color.white;
                    //sphere.color = black;
                }
                else
                {
                    //默认白色
                    textMeshProUGUI.color = Color.white;
                    textMeshProUGUI.outlineColor = Color.black;
                }


                textMeshProUGUI.text = playerController.nickname;
                textMeshProUGUI.ForceMeshUpdate();
            }

            //修改显示范围
            textMeshProUGUI.rectTransform.anchorMax = new Vector2(10, 0.5f);
        }

        private static void DestroyAllPlayers()
        {
            MelonLogger.Msg("正在销毁所有玩家minimap点位");
            foreach (var entry in MinimapESP.playersOnMinimap)
            {
                if (entry.Value != null)
                {
                    Destroy(entry.Value);
                }
            }

            MinimapESP.playersOnMinimap.Clear();
            instantiatedAllPlayers = false;

            MelonLogger.Msg("已经销毁所有玩家minimap点位");
        }



        /// <summary>
        /// 更新所有gameObjects的坐标，相对于LocalPlayer
        /// </summary>
        private void Update()
        {
            //检查功能是否启用
            if (!MinimapESP.Enabled.Value)
            {
                return;
            }

            if (miniMapHandler == null)
            {
                return;
            }

            if (!LobbySceneHandler.Instance.gameStarted)
            {
                if (MinimapESP.playersOnMinimap.Count != 0)
                {
                    MelonLogger.Msg("游戏结束，清除所有玩家的minimap点位");
                    DestroyAllPlayers();
                }

                return;
            }

            //游戏进行中
            if (!instantiatedAllPlayers)
            {
                MinimapESP.InstantiateAllPlayers();
                return;
            }

            //检测是否打开地图
            if (!GameObject.Find("Canvas/MiniMap")) return;

            foreach (var player in MinimapESP.playersOnMinimap)
            {
                string userId = player.Key;
                GameObject gameObject = player.Value;

                if (gameObject == null) continue;

                if (!PlayerController.playersList.ContainsKey(userId)) continue;

                //获取PlayerController对应的坐标
                PlayerController playerController = PlayerController.playersList[userId];
                Vector3 position = playerController.ACGBAGHOFIP;

                //玩家死亡
                if (playerController.timeOfDeath != 0)
                {
                    //没有标签就加上标签
                    if (!playerController.nickname.Contains("[尸体]") && !playerController.nickname.Contains("[Body]"))
                    {
                        if (Utils.Utils.IsChineseSystem())
                        {
                            playerController.nickname = "[尸体] " + playerController.nickname;
                        }
                        else
                        {
                            playerController.nickname = "[Body] " + playerController.nickname;
                        }
                    }
                    //获取尸体
                    Handlers.GameHandlers.BodyHandler bodyHandler = Managers.MainManager.Instance.gameManager.BodyFromUserId(playerController.userId);
                    //如果尸体存在只想尸体的坐标
                    if (bodyHandler != null)
                        position = bodyHandler.transform.position;
                    else
                    {
                        //如果尸体不存在就删除
                        Destroy(gameObject);
                        gameObject = null;
                        continue;
                    }
                }

                //根据PlayerController的坐标计算出GameObject的坐标
                gameObject.transform.localPosition = new Vector3(
                (float)(miniMapHandler.xFactor * position.x) + miniMapHandler.xOffset,
                (float)(miniMapHandler.yFactor * position.y) + miniMapHandler.yOffset,
                0.0f
                );

                //修改颜色
                UpdatePlayerColorAndName(gameObject, playerController);
            }

            //MelonLogger.Msg("已经更新所有玩家minimap坐标");

            /*
             * this是MiniMapHandler
            v180.fields.x = (float)(this->fields.xFactor * this->fields.PFMBPLBLHNN.fields.x) + this->fields.xOffset;
            v180.fields.y = (float)(this->fields.yFactor * this->fields.PFMBPLBLHNN.fields.y) + this->fields.yOffset;
            v180.fields.z = 0.0;
            */
        }

        [HarmonyPatch(typeof(InGameEvents), nameof(InGameEvents.Pelican_Eat))]
        class PelicanEatenPlayerESP
        {
            private static void OnPelicanEat(string playerEaten, string pelican = null)
            {
                PlayerController playerController = PlayerController.playersList[playerEaten];
                playerController.nickname = string.Format("[{0}] {1}", Utils.Utils.IsChineseSystem()?"被吃":"Eaten", playerController.nickname);
            }

            static void Postfix(string playerEaten, string pelican)
            {
                //检查功能是否启用
                if (MinimapESP.Enabled.Value == false)
                {
                    return;
                }

                OnPelicanEat(playerEaten, pelican);
            }
        }
    }

    [HarmonyPatch(typeof(MiniMapHandler), nameof(MiniMapHandler.Update))]
    class MiniMapHandlerUpdateHook
    {
        //更新实例
        static void Postfix(MiniMapHandler __instance)
        {
            if (MinimapESP.miniMapHandler == null)
            {
#if Developer
                MelonLogger.Msg(System.ConsoleColor.Green, "已成功Hook获取到MinimapHandler");
#endif
                MinimapESP.miniMapHandler = __instance;
                MinimapESP.instantiatedAllPlayers = false;
            }
        }
    }
}
