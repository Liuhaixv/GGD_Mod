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

        private static float lastTimeClickedStartGameButton = -1;
        private static float clickButtonInterval = 3.0f;
        public AutoStartGame(IntPtr ptr) : base(ptr)
        {
            IngameSettings.AddIngameSettingsEntry(
                               new IngameSettings.IngameSettingsEntry()
                               {
                                   entry = Enabled,
                                   name_cn = "自动开始游戏(轮抽会无限重开)",
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

                //判断游戏是否开始
                if (__instance.gameStarted) return;

                //判断房间内人数
                if(PlayerController.playersList.Count < 5)
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