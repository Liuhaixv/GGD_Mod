using Handlers.GameHandlers.PlayerHandlers;
using Handlers.LobbyHandlers;
using Managers;
using MelonLoader;
using System;
using System.Collections.Generic;
using HarmonyLib;
using UnhollowerRuntimeLib;
using UnityEngine;
using static Il2CppSystem.Globalization.TimeSpanFormat;
using System.Text.RegularExpressions;
using System.Linq;
using Photon.Realtime;
using Photon.Pun.Demo.PunBasics;
using APIs.Photon;

//根据名称自动踢出玩家
namespace GGD_Hack.Features
{
    [RegisterTypeInIl2Cpp]
    public class AutoKicker : MonoBehaviour
    {
        public static MelonPreferences_Entry<bool> Enabled = MelonPreferences.CreateEntry<bool>("GGDH", "Enable_" + nameof(AutoKicker), true);
        public static AutoKicker Instance = null;

        private List<string> rules = new List<string>();
        private string fileName = "AutoKick.txt";
        //regex rules
        private List<string> defaultRules = new List<string>() {
                @".*[ 0123456789零一二三四五六七八九壹贰叁肆伍陆柒捌玖]{6,}.*",
                @".*[qQ扣][裙群].*",
                @".*千人[裙群].*",
                @".*全网最低.*",
                @".*稳定.*",
                @".*奔放.*",
                @".*辅助.*",
                @".*低价.*",
                @".*透视.*",
                @".*看身份.*",
                @".*看职业.*",
                @".*穿墙.*",
                @".*秒噶.*",
                @".*瞬刀.*"
          };

        public AutoKicker(IntPtr ptr) : base(ptr)
        {
            IngameSettings.AddIngameSettingsEntry(
                   new IngameSettings.IngameSettingsEntry()
                   {
                       entry = Enabled,
                       name_cn = "自动踢人",
                       name_eng = "AutoKicker"
                   }
                );
        }

        // Optional, only used in case you want to instantiate this class in the mono-side
        // Don't use this on MonoBehaviours / Components!
        public AutoKicker() : base(ClassInjector.DerivedConstructorPointer<AutoKicker>()) => ClassInjector.DerivedConstructorBody(this);

        public static void Init()
        {
            GameObject ML_Manager = GameObject.Find("ML_Manager");
            if (ML_Manager == null)
            {
                ML_Manager = new GameObject("ML_Manager");
                DontDestroyOnLoad(ML_Manager);
            }

            if (ML_Manager.GetComponent<AutoKicker>() == null)
            {
                Instance = ML_Manager.AddComponent<AutoKicker>();
            }
        }

        private void Start()
        {
            string ruleFilePath = MelonHandler.ModsDirectory + "/" + fileName;
            MelonLogger.Msg(System.ConsoleColor.Green, "自动踢人的规则文件路径：" + ruleFilePath);

            //if ruleFile exists, create one if not
            if (!System.IO.File.Exists(ruleFilePath))
            {
                MelonLogger.Msg(System.ConsoleColor.Green, "自动踢人的规则文件不存在，即将创建默认规则文件");
                rules = defaultRules;
            }
            else
            {
                rules = System.IO.File.ReadAllLines(ruleFilePath).Where(line => !line.StartsWith("//")).Select(line => line).ToList();
                //合并默认规则，检查是否缺少
                foreach (string defaultRule in defaultRules)
                {
                    if (!rules.Contains(defaultRule))
                    {
                        rules.Add(defaultRule);
                    }
                }
            }

            System.IO.File.WriteAllLines(ruleFilePath, new string[] {
                    "//You can define any regular expression to automatically kick out players whose nicknames or chat messages match the pattern",
                    "//你可以定义任何正则表达式来自动踢出昵称或发言符合规则的玩家",
                    "//以下是一些默认规则，可以自行修改"
            });

            System.IO.File.AppendAllLines(ruleFilePath, rules);
        }

        private bool IsStringForbidden(string str)
        {
            foreach (string rule in rules)
            {
                if (rule.StartsWith("//"))
                {
                    continue;
                }

                if (Regex.IsMatch(str, rule))
                {
                    return true;
                }
            }
            return false;
        }
        private void KickPlayer(string userId)
        {
            GGD_Hack.PluginEventsManager.Kick_Player(userId, "Super Banned");
        }

        private void HandlePlayerChatMessage(string userId, string message)
        {
            //功能启用检查
            if (!Enabled.Value)
            {
                return;
            }

            //判断是否本地玩家
            if (userId == LocalPlayer.Instance.Player.userId)
                return;

            //判断是否在房间内
            if (!LobbySceneHandler.InGameScene)
                return;

            //判断游戏已经开始
            if (LobbySceneHandler.Instance.gameStarted)
                return;

            //判断自己是否为房主
            Photon.Realtime.Player localPlayer = Photon.Pun.PhotonNetwork.LocalPlayer;
            if (!MainManager.Instance.photonMasterManager.IsMasterClient(localPlayer))
            {
                return;
            }

            //如果发言违规则踢出
            if (IsStringForbidden(message))
            {
                //获取nickname
                string nickname = GameObject.Find(userId)?.GetComponent<PlayerController>()?.nickname ?? "";

                MelonLogger.Msg(System.ConsoleColor.Green, "检测到违规发言:{0} ，正在踢出玩家：{1}", message, nickname);
                KickPlayer(userId);
            }
        }

        private void HandlePlayerNickname(string userId, string nickname)
        {
            //功能启用检查
            if (!Enabled.Value)
            {
                return;
            }

            //判断是否本地玩家
            {
                if (LocalPlayer.Instance == null || LocalPlayer.Instance.Player == null)
                {
                    return;
                }
                if (userId == LocalPlayer.Instance.Player.userId)
                    return;
            }

            //判断是否在房间内
            if (!LobbySceneHandler.InGameScene)
                return;

            //判断游戏已经开始
            if (LobbySceneHandler.Instance.gameStarted)
                return;

            //判断自己是否为房主
            Photon.Realtime.Player localPlayer = Photon.Pun.PhotonNetwork.LocalPlayer;
            if (!MainManager.Instance.photonMasterManager.IsMasterClient(localPlayer))
            {
                return;
            }

            //判断昵称是否违规
            bool nicknameBanned = false;
            nicknameBanned = IsStringForbidden(nickname);

            //如果昵称违规则踢出
            if (nicknameBanned)
            {
                MelonLogger.Msg(System.ConsoleColor.Green, "检测到违规昵称！正在踢出该玩家：{0}", nickname);
                KickPlayer(userId);
            }
        }

        //通过hook在Join事件处理违规昵称
        //[11:08:37.540] [[开发者专用版]_Liuhaixv's_GGD_Hack_mod] 接收到事件: Join
        //[11:08:37.540][[开发者专用版] _Liuhaixv's_GGD_Hack_mod] Event 255: {
        //(Byte)249=(Hashtable){(Byte)253=(String)Dln8X9xxA5Xj5eWMdryVjrANLa13:1, (String)friendNumber=(String)8531, (String)userId=(String)Dln8X9xxA5Xj5eWMdryVjrANLa13, (String)friendName=(String)ZZZZZZ, (String)isAuth=(Boolean)True, (String)lsp=(Vector2)(-48.31, 36.77), (Byte)255=(String)Q群:660720811}, (Byte)252=(Int32[])System.Int32[], (Byte)254=(Int32)205}
        [HarmonyPatch(typeof(PhotonEventAPI), nameof(PhotonEventAPI.OnEvent), typeof(ExitGames.Client.Photon.EventData))]
        class PlayerJoinedRoom
        {
            static void Postfix(ExitGames.Client.Photon.EventData __0)
            {
                int code = __0.Code;

                //判断是不是加入游戏的事件
                if (code != (int)GameData.EventDataCode.Join)
                {
                    return;
                }
                ExitGames.Client.Photon.ParameterDictionary parameters = __0.Parameters;
                ExitGames.Client.Photon.Hashtable hashTable = parameters.Get<ExitGames.Client.Photon.Hashtable>(249);
                string userId = hashTable["userId"].ToString();
                string nickname = hashTable[255].ToString();
                MelonLogger.Msg("玩家加入房间: " + nickname);
                if (Enabled.Value)
                {
                    Instance.HandlePlayerNickname(userId, nickname);
                }
            }
        }

        //hook收到消息的函数
        [HarmonyPatch(typeof(Handlers.PrefabAttachedHandlers.MessagePrefabHandler), nameof(Handlers.PrefabAttachedHandlers.MessagePrefabHandler.Initialize))]
        class OnReceivedMessage
        {
            static void Postfix(Handlers.PrefabAttachedHandlers.MessagePrefabHandler __instance)
            {
                string message = __instance.message;
                string userId = __instance.sender;
                Instance.HandlePlayerChatMessage(userId, message);
            }
        }
    }
}
