using Handlers.CommonHandlers.UIHandlers;
using Handlers.LobbyHandlers;
using HarmonyLib;
using MelonLoader;
using System.Security.Policy;
using System.Text;
using UnityEngine.Events;

namespace GGD_Hack.AntiSell
{
    [HarmonyPatch(typeof(Handlers.LobbyHandlers.PlayerCustomizationPanelHandler), nameof(PlayerCustomizationPanelHandler.Start))]
    public class PopupFree
    {
        public static void OpenNoticePanel(int times = 0)
        {
            bool isChineseSystem = Utils.Utils.IsChineseSystem();

            string warningText = isChineseSystem ? "本项目完全免费，禁止任何人贩卖!" : "Never buy or sell this mod!";

            string title = isChineseSystem ? "声明" : "Notice";
            string content = null;
            switch (times)
            {
                case 0:
                    content = warningText;
                    break;
                case 1:
                    content = warningText + '\n'
                        + "<color=green>" + warningText + "</color>";
                    break;
                case 2:
                    content = warningText + '\n'
                        + "<color=green>" + warningText + "</color>" + '\n'
                        + "<color=red>" + warningText + "</color>"; ;
                    break;
                case 3:
                    content = warningText + '\n'
                        + "<color=green>" + warningText + "</color>" + '\n'
                        + "<color=red>" + warningText + "</color>" + '\n'
                        + "<color=blue>" + warningText + "</color>"; ;
                    break;
            }

            string button = isChineseSystem ? "太棒了!" : "Great!";

            bool closePanels = false;

            GlobalPanelsHandler.Instance.OpenOneButtonPromptPanel(
                title,
                content,
                button,
                new System.Action(() =>
                {
                    //修改文字
                    GlobalPanelsHandler.Instance.ClosePanels();
                    if(times < 3)
                    {
                        OpenNoticePanel(++times);
                    }
                }),
                closePanels);
        }

        public static void OpenFreeCheckboxPanel()
        {
            //弹出免费窗口
            bool isChineseSystem = Utils.Utils.IsChineseSystem();
            string title = isChineseSystem ? "警告！" : "Warning!!";
            string content = isChineseSystem ? "本mod项目为Github<color=green>100%免费</color>项目!\n如果你是从别处购买的请立刻<color=red>退款</color>并投诉卖家!" : "This mod is 100% free!\nIf you bought it somewhere else, you are scammed!";
            string leftButton = isChineseSystem ? "我已知晓" : "Understand";
            string checkBox = isChineseSystem ? "不再显示" : "Don't show again";
            string rightButton = isChineseSystem ? "拜访作者Liuhaixv" : "Visit author";
            bool closePanels = false;


            UnityAction leftButtonAction = new System.Action(() =>
            {
                bool isOn = GlobalPanelsHandler.Instance.checkboxToggle.isOn;
                if (isOn)
                {
                    MelonLogger.Msg("正在设置已提示过免费警告...");
                    GGDHack_Mod.HasWarnedFree = true;
                }

                //修改文字
                GlobalPanelsHandler.Instance.ClosePanels();
                OpenNoticePanel();
            });

            UnityAction rightButtonAction = new System.Action(() =>
            {
                bool isOn = GlobalPanelsHandler.Instance.checkboxToggle.isOn;
                System.Diagnostics.Process.Start("https://github.com/Liuhaixv");
            });


            GlobalPanelsHandler.Instance.OpenCheckboxPanel(
                title,
                content,
                leftButton,
                checkBox,
                leftButtonAction,
                rightButton,
                rightButtonAction,
                closePanels
                );
        }
        static void Postfix(Handlers.LobbyHandlers.PlayerCustomizationPanelHandler __instance)
        {
            try
            {
                if (!GGDHack_Mod.HasWarnedFree)
                    OpenFreeCheckboxPanel();
            }
            catch (System.Exception ex)
            {
                MelonLogger.Warning($"Exception in patch of void Handlers.LobbyHandlers.PlayerCustomizationPanelHandler::Start():\n{ex}");
            }
        }
    }
}
