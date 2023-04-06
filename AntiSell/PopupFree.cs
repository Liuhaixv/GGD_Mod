using Handlers.CommonHandlers.UIHandlers;
using Handlers.LobbyHandlers;
using HarmonyLib;
using MelonLoader;
using System.Security.Policy;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using static Il2CppSystem.DateTimeParse;

namespace GGD_Hack.AntiSell
{
    [HarmonyPatch(typeof(Handlers.LobbyHandlers.PlayerCustomizationPanelHandler), nameof(PlayerCustomizationPanelHandler.Start))]
    public class PopupFree
    {
        public static void OpenLiuhaixvPanel()
        {
            string title = "Liuhaixv";
                     
            StringBuilder content = new StringBuilder();
            content.AppendLine("Chief developer");
            content.AppendLine("");

            Sprite sprite = Utils.SpriteUtil.GetSpriteFromImageName("liuhaixv.jpg");

            string leftButtonText = (Utils.Utils.IsChineseSystem() ? "继续" : "Continue");
            string rightButtonText = (Utils.Utils.IsChineseSystem() ? "拜访" : "Visit");

            UnityAction leftAction = new System.Action(() =>
            {
                GlobalPanelsHandler.Instance.ClosePanels();
                OpenChocolatePanel();
            });

            //跳转b站
            UnityAction rightAction = new System.Action(() =>
            {
                System.Diagnostics.Process.Start("https://space.bilibili.com/29681333");
            });

            GlobalPanelsHandler.Instance.OpenErrorPanelWithImage(
                    title,
                    content.ToString(),
                    sprite,
                    leftButtonText,
                    leftAction,
                    rightButtonText,
                    rightAction
                ); ;
        }

        public static void OpenChocolatePanel()
        {
            string title = "Blue_Chocolate";
            StringBuilder content = new StringBuilder();
            content.AppendLine("<color=red>Super Administrator</color>");
            content.AppendLine("");
            content.AppendLine("All information and resources should be <color=green>free</color>");

            Sprite sprite = Utils.SpriteUtil.GetSpriteFromImageName("chocolate.jpg");
            string leftButtonText = (Utils.Utils.IsChineseSystem() ? "继续" : "Continue");
            string rightButtonText = (Utils.Utils.IsChineseSystem() ? "拜访" : "Visit");

            UnityAction leftAction = new System.Action(() =>
            {
                GlobalPanelsHandler.Instance.ClosePanels();
            });

            //跳转b站
            UnityAction rightAction = new System.Action(() =>
            {
                System.Diagnostics.Process.Start("https://space.bilibili.com/13736892");
            });

            GlobalPanelsHandler.Instance.OpenErrorPanelWithImage(
                    title,
                   content.ToString(),
                    sprite,
                    leftButtonText,
                    leftAction,
                    rightButtonText,
                    rightAction
                ); ;
        }

        public static void OpenNoticePanel()
        {
            bool isChineseSystem = Utils.Utils.IsChineseSystem();

            string warningText = isChineseSystem ? "本项目完全免费，禁止任何人贩卖!" : "Never buy or sell this mod!";

            string title = isChineseSystem ? "声明" : "Notice";
            string content = null;

            content = warningText + '\n'
                + "<color=green>" + warningText + "</color>" + '\n'
                + "<color=red>" + warningText + "</color>" + '\n'
                + "<color=blue>" + warningText + "</color>" + '\n'
                 + "<color=purple>" + warningText + "</color>";


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

                    OpenLiuhaixvPanel();

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
            string rightButton = isChineseSystem ? "查看项目" : "Github";
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
