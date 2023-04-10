using MelonLoader;
using UnhollowerRuntimeLib;
using UnityEngine;
using HarmonyLib;

using IntPtr = System.IntPtr;
using Handlers.LobbyHandlers;
using Managers;
using Photon.Pun;
using System.Linq;
using Handlers.GameHandlers.PlayerHandlers;

namespace GGD_Hack.Features
{
    [RegisterTypeInIl2Cpp]
    public class AutoStartGame : MonoBehaviour
    {
        public static AutoStartGame Instance;
        public static MelonPreferences_Entry<bool> Enabled = MelonPreferences.CreateEntry<bool>("GGDH", "Enable_" + nameof(AutoStartGame), false);
        //游戏结束后重开游戏的延迟
        public static MelonPreferences_Entry<float> restartGameCooldownTime = MelonPreferences.CreateEntry<float>("GGDH", nameof(AutoStartGame) + "_" + nameof(restartGameCooldownTime), 10.0f);

        private static float lastTimeClickedStartGameButton = -1;
        private static float clickButtonInterval = 3.0f;
        private static float doNotStartGameBeforeTime = -1.0f;
        public AutoStartGame(IntPtr ptr) : base(ptr)
        {
            IngameSettings.AddIngameSettingsEntry(
                               new IngameSettings.IngameSettingsEntry()
                               {
                                   entry = Enabled,
                                   name_cn = "自动开始游戏",
                                   name_eng = "Auto Start Game"
                               }
                                          );
        }

        public AutoStartGame() : base(ClassInjector.DerivedConstructorPointer<AutoStartGame>()) => ClassInjector.DerivedConstructorBody(this);
        public static void Init()
        {
            GameObject ML_Manager = GameObject.Find("ML_Manager");
            if (ML_Manager == null)
            {
                ML_Manager = new GameObject("ML_Manager");
                DontDestroyOnLoad(ML_Manager);
            }

            if (ML_Manager.GetComponent<AutoStartGame>() == null)
            {
                Instance = ML_Manager.AddComponent<AutoStartGame>();
            }
        }

        [HarmonyPatch(typeof(LobbySceneHandler), nameof(LobbySceneHandler.Update))]
        class LobbySceneHandler_Update
        {
            static void Postfix(LobbySceneHandler __instance)
            {
                if (!Enabled.Value) return;

                //更新重开游戏延迟
                if ((byte)MainManager.Instance?.gameManager?.gameState == (byte)GameData.GameState.InGame) {
                    doNotStartGameBeforeTime = Time.time + restartGameCooldownTime.Value;
                }

                //判断是否允许重新开始游戏
                if (Time.time < doNotStartGameBeforeTime)
                {
                    return;
                }

                //判断游戏是否开始
                if (__instance.gameStarted) return;
                if ((byte)MainManager.Instance.gameManager.gameState != (byte)GameData.GameState.InLobby) { return; }

                //判断房间内人数
                if (PlayerController.playersList.Count < 5)
                {
                    return;
                }

                //判断自己是否为房主
                Photon.Realtime.Player localPlayer = Photon.Pun.PhotonNetwork.LocalPlayer;
                if (!MainManager.Instance.photonMasterManager.IsMasterClient(localPlayer))
                {
                    return;
                }

                //判断上一次点击间隔
                //开始游戏
                if (Time.time - lastTimeClickedStartGameButton < clickButtonInterval)
                {
                    return;
                }

                //开始游戏
                {
                    LobbySceneHandler.Instance.StartGame();
                    lastTimeClickedStartGameButton = Time.time;
                }
            }
        }
    }
}