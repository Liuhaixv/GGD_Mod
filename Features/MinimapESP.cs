using Handlers.GameHandlers.SpecialBehaviour;
using Il2CppSystem.Collections.Generic;
using UnhollowerRuntimeLib;
using UnityEngine;
using UnityEngine.PlayerLoop;
using HarmonyLib;

using IntPtr = System.IntPtr;
using MelonLoader;
using Handlers.GameHandlers.PlayerHandlers;
using static MelonLoader.MelonLogger;
using Handlers.LobbyHandlers;
using Il2CppSystem.Threading.Tasks;

namespace GGD_Hack.Features
{
    [RegisterTypeInIl2Cpp]
    public class MinimapESP : MonoBehaviour
    {
        //是否已经初始化所有玩家的点位
        public static bool instantiatedAllPlayers = false;

        public static MinimapESP Instance = null;

        //通过hook更新
        public static MiniMapHandler miniMapHandler = null;

        //玩家userId对应的GameObject，表示地图上的点
        private static Dictionary<string, GameObject> playersOnMinimap = new Dictionary<string, GameObject>();

        public MinimapESP(IntPtr ptr) : base(ptr) { }

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
            GameObject targetMe = Utils.GameInstances.FindGameObjectByPath("Canvas/MiniMap/Panel/Target Me");

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

                SetPlayerName(clone, playerController);

                //TODO: 修改颜色

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
        private static void SetPlayerName(GameObject playerOnMinimap, PlayerController playerController)
        {
            Transform youTransform = playerOnMinimap.transform.Find("You");

            if (youTransform == null)
            {
                MelonLogger.Warning("找不到 You");
            }

            TMPro.TextMeshProUGUI textMeshProUGUI = youTransform.gameObject.GetComponent<TMPro.TextMeshProUGUI>();

            //修改文字
            if (textMeshProUGUI != null)
            {
                //MelonLogger.Msg("修改玩家姓名为: " + playerController.nickname);
                textMeshProUGUI.SetText(playerController.nickname, true);
            }
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

            foreach (var player in MinimapESP.playersOnMinimap)
            {
                string userId = player.Key;
                GameObject gameObject = player.Value;

                if (gameObject == null) continue;

                if (!PlayerController.playersList.ContainsKey(userId)) continue;

                //删除幽灵点位
                if (PlayerController.playersList[userId].isGhost)
                {
                    Destroy(gameObject);
                    gameObject = null;
                    continue;
                }

                //获取PlayerController对应的坐标
                PlayerController playerController = PlayerController.playersList[userId];
                Vector3 position = playerController.PMPPIFBLAPL;

                //根据PlayerController的坐标计算出GameObject的坐标
                gameObject.transform.localPosition = new Vector3(
                    (float)(miniMapHandler.xFactor * position.x) + miniMapHandler.xOffset,
                    (float)(miniMapHandler.yFactor * position.y) + miniMapHandler.yOffset,
                    0.0f
                );

                //强制修改名称
                SetPlayerName(gameObject, playerController);
            }

            //MelonLogger.Msg("已经更新所有玩家minimap坐标");

            /*
             * this是MiniMapHandler
            v180.fields.x = (float)(this->fields.xFactor * this->fields.PFMBPLBLHNN.fields.x) + this->fields.xOffset;
            v180.fields.y = (float)(this->fields.yFactor * this->fields.PFMBPLBLHNN.fields.y) + this->fields.yOffset;
            v180.fields.z = 0.0;
            */
        }
    }


    [HarmonyPatch(typeof(MiniMapHandler),nameof(MiniMapHandler.Update))]
    class MiniMapHandlerUpdateHook
    {
        //更新实例
        static void Postfix(MiniMapHandler __instance)
        {
            if (MinimapESP.miniMapHandler == null)
            {
                MelonLogger.Msg("已成功Hook获取到MinimapHandler");
                MinimapESP.miniMapHandler = __instance;
                MinimapESP.instantiatedAllPlayers = false;
            }
        }
    }
}
