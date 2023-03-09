using Handlers.LobbyHandlers;
using HarmonyLib;
using MelonLoader;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

using IntPtr = System.IntPtr;
namespace GGD_Hack.Features.EasterEgg
{
    public class MoneyHackButton
    {
        public static PlayerCustomizationPanelHandler playerCustomizationPanelHandler = null;

        private static EventTrigger.Entry gold = null;
        private static EventTrigger.Entry silver = null;

        public static void OnSilverBuyButtonClicked(BaseEventData data)
        {
            MelonLogger.Msg(System.ConsoleColor.Green, "Buy按钮被点击!");

            IncreaseSilver();
        }

        public static void OnGoldBuyButtonClicked(BaseEventData data)
        {
            MelonLogger.Msg(System.ConsoleColor.Green, "Buy按钮被点击!");

            IncreaseGold();
        }

        private static void IncreaseGold()
        {
            MelonLogger.Msg("金币已增加");
            playerCustomizationPanelHandler.goldBalance += 100;

            playerCustomizationPanelHandler.goldCurrencyText.text = playerCustomizationPanelHandler.goldBalance.ToString();

            playerCustomizationPanelHandler.goldCurrencyText.ForceMeshUpdate(true, true);
        }
        private static void IncreaseSilver()
        {
            MelonLogger.Msg("银币已增加");
            playerCustomizationPanelHandler.silverBalance += 1000;

            playerCustomizationPanelHandler.silverCurrencyText.text = playerCustomizationPanelHandler.silverBalance.ToString();

            playerCustomizationPanelHandler.silverCurrencyText.ForceMeshUpdate(true, true);
        }

        //通过Hook获取PlayerCustomizationPanelHandler的实例
        [HarmonyPatch(typeof(PlayerCustomizationPanelHandler), nameof(PlayerCustomizationPanelHandler.Update))]
        class PlayerCustomizationPanelHandler_Update
        {
            static void Postfix(Handlers.LobbyHandlers.PlayerCustomizationPanelHandler __instance)
            {
                try
                {
                    if (MoneyHackButton.gold == null)
                    {
                        //彩蛋
                        EventTrigger.Entry entry = new EventTrigger.Entry();
                        entry.eventID = EventTriggerType.PointerClick;
                        entry.callback.AddListener((UnityAction<BaseEventData>)OnGoldBuyButtonClicked);
                        MoneyHackButton.gold = entry;
                    }
                    if (MoneyHackButton.silver == null)
                    {
                        //彩蛋
                        EventTrigger.Entry entry = new EventTrigger.Entry();
                        entry.eventID = EventTriggerType.PointerClick;
                        entry.callback.AddListener((UnityAction<BaseEventData>)OnSilverBuyButtonClicked);
                        MoneyHackButton.silver = entry;
                    }

                    if (MoneyHackButton.playerCustomizationPanelHandler != __instance && __instance != null)
                    {
                        MoneyHackButton.playerCustomizationPanelHandler = __instance;
#if Developer
                        MelonLogger.Msg(System.ConsoleColor.Green, "成功Hook获取PlayerCustomizationPanelHandler");
#endif


                        //修改Buy button
                        GameObject[] buttons = GameObject.FindObjectsOfType<GameObject>().Where(obj => obj.name == "Buy").ToArray();
                        foreach (GameObject button in buttons)
                        {
                            string parentName = button.transform.parent.gameObject.name;
                            // Do something with button
                            EventTrigger eventTrigger = button.GetComponent<EventTrigger>();
                            if (eventTrigger != null)
                            {
                                if (parentName == "Gold")
                                {
                                    eventTrigger.triggers.Clear();
                                    eventTrigger.triggers.Add(MoneyHackButton.gold);
                                    MelonLogger.Msg(System.ConsoleColor.Green, "已添加彩蛋");

                                }
                                else if (parentName == "Silver")
                                {
                                    eventTrigger.triggers.Clear();
                                    eventTrigger.triggers.Add(MoneyHackButton.silver);
                                    MelonLogger.Msg(System.ConsoleColor.Green, "已添加彩蛋");
                                }
                            }
                        }
                    }
                }
                catch (System.Exception ex)
                {
                    MelonLogger.Warning($"Exception in patch of void Handlers.LobbyHandlers.PlayerCustomizationPanelHandler::Update():\n{ex}");
                }
            }
        }
    }
}
