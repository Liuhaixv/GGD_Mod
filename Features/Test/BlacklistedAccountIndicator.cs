using Handlers.CommonHandlers.UIHandlers;
using HarmonyLib;
using System.Collections;
using MelonLoader;
using Photon.Realtime;
using System.Text;
using UnhollowerRuntimeLib;
using UnityEngine;
using UnityEngine.SceneManagement;
using IntPtr = System.IntPtr;
using Handlers.LobbyHandlers;
using Handlers.MenuSceneHandlers;
//转圈圈封禁
namespace GGD_Hack.Features.Test
{
    [RegisterTypeInIl2Cpp]
    public class BlacklistedAccountIndicator : MonoBehaviour
    {
        public static BlacklistedAccountIndicator Instance;
        public static MelonPreferences_Entry<bool> Enabled = MelonPreferences.CreateEntry<bool>("GGDH", "Enable_" + nameof(BlacklistedAccountIndicator), true);

        private static bool showBan = false;
        private static float showTime = -1;
        public BlacklistedAccountIndicator(IntPtr ptr) : base(ptr)
        {/*
            IngameSettings.AddIngameSettingsEntry(
                               new IngameSettings.IngameSettingsEntry()
                               {
                                   entry = Enabled,
                                   name_cn = "箭头自动追踪尸体",
                                   name_eng = "Auto Track Bodies"
                               }
                                          );*/
        }

        public BlacklistedAccountIndicator() : base(ClassInjector.DerivedConstructorPointer<BlacklistedAccountIndicator>()) => ClassInjector.DerivedConstructorBody(this);
        public static void Init()
        {
            GameObject ML_Manager = GameObject.Find("ML_Manager");
            if (ML_Manager == null)
            {
                ML_Manager = new GameObject("ML_Manager");
                DontDestroyOnLoad(ML_Manager);
            }

            if (ML_Manager.GetComponent<BlacklistedAccountIndicator>() == null)
            {
                Instance = ML_Manager.AddComponent<BlacklistedAccountIndicator>();
            }
        }

        private void Update()
        {
            if (showBan && Time.time > showTime)
            {
                showBan = false;
                showTime = -1;
                ShowBan();
            }
        }

        private static void ShowBan()
        {
            bool isChineseSystem = Utils.Utils.IsChineseSystem();

            string warningText = isChineseSystem ? "你可能注意到了游戏一直在主菜单转圈，这说明你的账号极大可能已被服务器<color=black>拉黑</color>。\n你可以清理缓存或登出来重新登陆其他账号。" : "It seems that your account has been <color=black>banned</color>, you can clear cache and login with another account";

            string title = isChineseSystem ? "严重警告" : "Warning";
            string content = null;

            content = warningText;

            string leftButtonText = isChineseSystem ? "???" : "WTF";
            string rightButtonText = isChineseSystem ? "登出并关闭游戏" : "Logout and quit";



            GlobalPanelsHandler.Instance.OpenPromptPanel(
                  title,
                 content.ToString(),
                  leftButtonText,
                   new System.Action(() =>
                   {
                       //修改文字
                       GlobalPanelsHandler.Instance.ClosePanels();
                   }),
                  rightButtonText,
                  new System.Action(() =>
                  {
                      GlobalPanelsHandler.Instance.ClosePanels();
                      MenuSceneHandler.Instance.SignOut();
                      Application.Quit();
                  })
              );

            GameObject.Destroy(GameObject.Find("GlobalCanvas/FriendLoadingPanel"));
        }

        [HarmonyPatch(typeof(LoadBalancingClient), nameof(LoadBalancingClient.OnOperationResponse))]
        public class IndicateAsBanned
        {
            //[05:36:18.241][[开发者专用版] _Liuhaixv's_GGD_Hack_mod] 接收到事件: AppStats
            //[05:36:18.241][[开发者专用版] _Liuhaixv's_GGD_Hack_mod] Event 226: {(Byte)227=(Int32)771, (Byte)229=(Int32)8920, (Byte)228=(Int32)805}
            static void Prefix(Photon.Realtime.LoadBalancingClient __instance, ExitGames.Client.Photon.OperationResponse __0)
            {
                try
                {
                    //账号被拉黑
                    if (__0.ReturnCode == 32767)
                    {
                        MelonLogger.Msg(System.ConsoleColor.Green, "账号疑似已被拉黑");
                        showBan = true;
                        showTime = Time.time + 10.0f;
                    }
                }
                catch (System.Exception ex)
                {
                    MelonLogger.Warning($"Exception in patch of void Photon.Realtime.LoadBalancingClient::OnOperationResponse(ExitGames.Client.Photon.OperationResponse operationResponse):\n{ex}");
                }
            }
        }
    }
}
