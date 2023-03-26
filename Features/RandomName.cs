using MelonLoader;
using System;
using HarmonyLib;
using UnhollowerRuntimeLib;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Handlers.MenuSceneHandlers;
using static MelonLoader.MelonLogger;
using System.Text;
using Photon.Realtime;
using Handlers.GameHandlers.PlayerHandlers;

namespace GGD_Hack.Features
{
    [RegisterTypeInIl2Cpp]
    public class RandomName : MonoBehaviour
    {
        public static MelonPreferences_Entry<bool> Enabled = MelonPreferences.CreateEntry<bool>("GGDH", "Enable_" + nameof(RandomName), true);

        public static RandomName Instance = null;

        public static GameObject changeRandomNameButton = null;
        //存储许多玩家的昵称
        public static List<string> nicknames = new List<string>();

        public RandomName(IntPtr ptr) : base(ptr) { }

        public RandomName() : base(ClassInjector.DerivedConstructorPointer<RandomName>()) => ClassInjector.DerivedConstructorBody(this);

        public static void Init()
        {
            GameObject ML_Manager = GameObject.Find("ML_Manager");
            if (ML_Manager == null)
            {
                ML_Manager = new GameObject("ML_Manager");
                DontDestroyOnLoad(ML_Manager);
            }

            if (ML_Manager.GetComponent<RandomName>() == null)
            {
                Instance = ML_Manager.AddComponent<RandomName>();
            }

            //通过回调从刷新的房间数据中窃取房主的名字
            APIs.Photon.PhotonCallbacksAPI.AddOnRoomListUpdateListener((Il2CppSystem.Action<Il2CppSystem.Collections.Generic.List<Photon.Realtime.RoomInfo>>)StoleNicknameFromHost, true);
        }

        //调整按钮的位置，向上挪动
        //主持游戏、查找游戏
        public static bool CreateChangeRandomNameButton()
        {
            //已经添加过
            if (changeRandomNameButton != null)
            {
                return true;
            }

            // 查找Mode Select - Safe zone H
            GameObject safeZone = GameObject.Find("Mode Select - Safe zone H");

            // 查找Goose
            Transform goose = safeZone.transform.Find("Goose");

            // 查找Nick Name Input和Find
            Transform find = goose.Find("Find");
            Transform nicknameInput = safeZone.transform.Find("Nick Name Input");

            // 克隆Find按钮
            changeRandomNameButton = GameObject.Instantiate(find.gameObject, nicknameInput.transform);

            //修改按钮的UI背景大小
            List<string> ui = new List<string>() { "DropShadow", "Fill", "Shadow" , "Border" };
            foreach(var name in ui)
            {
                Transform transform = changeRandomNameButton.transform.Find(name);
                if (transform != null)
                {
                    transform.localScale = new Vector3(0.6f, 1, 1);
                }
            }   

            changeRandomNameButton.name = nameof(changeRandomNameButton);
            changeRandomNameButton.transform.localPosition = new Vector3(-850, -65, 0);

            //添加按钮监听器
            //GameObject.Destroy(changeColorButton.GetComponent<Button>());
            Button button = changeRandomNameButton.gameObject.GetComponent<Button>();
            if (button != null)
            {
               DestroyImmediate(button);
                button = changeRandomNameButton.gameObject.AddComponent<Button>();
            }
            button.onClick.AddListener(new System.Action(() =>
            {
                //修改输入框昵称
                nicknameInput.gameObject.GetComponent<TMP_InputField>().text = GetRandomNickname();
            }));

            // 修改Find(clone)的TextMeshProUGUI组件中的文本
            TextMeshProUGUI findCloneTMP = changeRandomNameButton.transform.Find("Font").GetComponent<TextMeshProUGUI>();
            findCloneTMP.text =( Utils.Utils.IsChineseSystem() ? "随机" : "Random");

            return true;
        }

        //通过刷新房间号窃取房主的名字
        private static void StoleNicknameFromHost(Il2CppSystem.Collections.Generic.List<Photon.Realtime.RoomInfo> roomInfos)
        {

            //遍历房间数据
            foreach (var room in roomInfos)
            {
                string hostNickname = null;
                ExitGames.Client.Photon.Hashtable customProperties = room.CustomProperties;
                Il2CppSystem.Object @object = customProperties["C1"];
                hostNickname = @object.ToString();

                //添加昵称
                AddNicknameToList(hostNickname);
            }
        }

        private static void AddNicknameToList(string nickname)
        {
            RandomName.nicknames.Add(nickname);
            //添加昵称
            if (!RandomName.nicknames.Contains(nickname))
            {
                RandomName.nicknames.Add(nickname);
            }
        }

        private static string GetRandomNickname()
        {
            if (nicknames.Count == 0) return null;

            return nicknames[UnityEngine.Random.RandomRangeInt(0, nicknames.Count)];

        }

        [HarmonyPatch(typeof(SceneManager), nameof(SceneManager.Internal_SceneLoaded))]
        class SceneManager_
        {
            private static void Postfix(Scene scene)
            {
                MelonLogger.Msg(System.ConsoleColor.Green, "场景"+scene.name+"已加载!");
                if (scene.name == "MenuScene")
                {
                    bool success = RandomName.CreateChangeRandomNameButton();

                    if (success)
                        MelonLogger.Msg(System.ConsoleColor.Green, "已成功添加随机昵称按钮!");
                    else
                        MelonLogger.Warning("添加随机昵称按钮失败!");
                }
            }
        }

        [HarmonyPatch(typeof(PlayerController), nameof(PlayerController.Start))]
        class StoleNameFromPlayerController
        {
            private static void Postfix(PlayerController __instance)
            {
                //添加昵称
                AddNicknameToList(__instance.nickname);
            }
        }
    }
}
