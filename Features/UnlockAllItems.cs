extern alias System_;

using BestHTTP;
using ExitGames.Client.Photon;
using HarmonyLib;
using MelonLoader;
using Il2CppSystem;
using System.Reflection;
using UnhollowerBaseLib;

using Uri = System_::System.Uri;
using Managers.InfoManagers;
using Objects;
using Il2CppSystem.Collections.Generic;
using Managers;
using Managers.PlayerManagers;
using Handlers.LobbyHandlers;
using UnhollowerRuntimeLib;
using UnityEngine;

using IntPtr = System.IntPtr;
//using HTTPResponse = BestHTTP.HTTPResponse;

namespace GGD_Hack.Features
{
    [RegisterTypeInIl2Cpp]
    public class UnlockAllItems : MonoBehaviour
    {
        public static UnlockAllItems Instance;
        public static MelonPreferences_Entry<bool> Enabled;

        private float lastUpdateTime = 0;
        //更新皮肤的时间间隔
        private float updateInterval = 10.0f;

        public UnlockAllItems(IntPtr ptr) : base(ptr)
        {
            if (!MelonPreferences.HasEntry("GGDH", nameof(UnlockAllItems)))
            {
                Enabled = MelonPreferences.CreateEntry<bool>("GGDH", nameof(UnlockAllItems), true);
            }
            else
                Enabled = MelonPreferences.GetEntry<bool>("GGDH", nameof(UnlockAllItems));
        }

        // Optional, only used in case you want to instantiate this class in the mono-side
        // Don't use this on MonoBehaviours / Components!
        public UnlockAllItems() : base(ClassInjector.DerivedConstructorPointer<UnlockAllItems>()) => ClassInjector.DerivedConstructorBody(this);
        public static void Init()
        {
            GameObject ML_Manager = GameObject.Find("ML_Manager");
            if (ML_Manager == null)
            {
                ML_Manager = new GameObject("ML_Manager");
                DontDestroyOnLoad(ML_Manager);
            }

            if (ML_Manager.GetComponent<UnlockAllItems>() == null)
            {
                Instance = ML_Manager.AddComponent<UnlockAllItems>();
            }
        }

        /// <summary>
        /// 通知服务器更新玩家装扮
        /// </summary>
        public static void UpdateTempUserUnlockables()
        {
            PlayerPropertiesManager playerPropertiesManager = MainManager.Instance.playerPropertiesManager;
            //获取缓存属性
            Dictionary<string, string> dict = new Dictionary<string, string>();

            dict.Add("hat", playerPropertiesManager.tempHat);
            dict.Add("clothes", playerPropertiesManager.tempClothes);
            dict.Add("fart", playerPropertiesManager.tempFart);
            dict.Add("pet", playerPropertiesManager.tempPet);
            dict.Add("stinger", playerPropertiesManager.tempStinger);
            dict.Add("Banners", playerPropertiesManager.tempBanner);
            dict.Add("Cards", playerPropertiesManager.tempCard);

            MainManager.Instance.playerPropertiesManager.ChangeUserProperties(dict);
        }

        private void Update()
        {
            //判断是否在房间内
            if (!LobbySceneHandler.InGameScene) return;

            //游戏已经开始
            if(LobbySceneHandler.Instance.gameStarted) return;

            if(Time.time - lastUpdateTime < updateInterval)
            {
                return;
            }

            lastUpdateTime = Time.time;

            //如果功能被启用
            if(Enabled.Value == true)
            {
                //更新玩家装扮
                UpdateTempUserUnlockables();
            }
        }
    }

    //ggdUnlockables 所有可用物品
    //userUnlockables 玩家已拥有物品
    [HarmonyPatch(typeof(UnlockablesManager), nameof(UnlockablesManager.ProcessGGDDataAndContinue))]
    class ProcessGGDDataAndContinue_
    {
        static void Prefix(ref GGDDataBody __0)
        {
            MelonLogger.Msg("开始处理GGDDataBody");

            GGDDataBody dataBody = __0;

            UnlockablesInfoBody ggdUnlockables = dataBody.ggdUnlockables;
            Il2CppReferenceArray<UnlockableInfo> unlockableInfos = ggdUnlockables.unlockableInfo;

            UnlockableInfo availableTemplate = null;
            //获取一个可用物品作为模板
            foreach (UnlockableInfo unlockableInfo in unlockableInfos)
            {
                if (unlockableInfo.isAvailable)
                {
                    availableTemplate = unlockableInfo;
                    break;
                }
            }
            /*
            foreach (var recipe in availableTemplate.rawUnlockable.recipes)
            {
                foreach(var recipeCost in recipe.recipeCosts)
                {
                    recipeCost.cost = 999;
                    recipeCost.currencyType = "gold";
                }                
            }
            */

            //List<string> categoryIds = new List<string>();
            //所有物品
            foreach (UnlockableInfo unlockableInfo in unlockableInfos)
            {

                Unlockable rawUnlockable = unlockableInfo.rawUnlockable;

                //跳过不可用宠物
                if (rawUnlockable.type == "Pets" && unlockableInfo.isAvailable == false)
                {
                    continue;
                }

                rawUnlockable.recipes = null;
                rawUnlockable.startDate = availableTemplate.rawUnlockable.startDate;
                rawUnlockable.expirationDate = availableTemplate.rawUnlockable.expirationDate;
                rawUnlockable.saleStartDate = availableTemplate.rawUnlockable.saleStartDate;
                rawUnlockable.saleExpirationDate = availableTemplate.rawUnlockable.saleExpirationDate;
                rawUnlockable.requirements = availableTemplate.rawUnlockable.requirements;

                unlockableInfo.isAvailable = availableTemplate.isAvailable;
                unlockableInfo.isAvailableExpiringSoon = availableTemplate.isAvailableExpiringSoon;
                unlockableInfo.isOnSale = availableTemplate.isOnSale;
                unlockableInfo.isOnSaleExpiringSoon = availableTemplate.isOnSaleExpiringSoon;
                unlockableInfo.timeUntilIsAvailableExpiring = availableTemplate.timeUntilIsAvailableExpiring;
                unlockableInfo.timeUntilIsOnSaleExpiring = availableTemplate.timeUntilIsOnSaleExpiring;
            }
        }
    }


    /*
    //BestHttp 2.5.4
    //[HarmonyPatch(typeof(HTTPManager), nameof(HTTPManager.SendRequest), typeof(string), typeof(HTTPMethods), typeof(bool), typeof(bool), typeof(OnRequestFinishedDelegate))]
    [HarmonyPatch(typeof(HTTPResponse), nameof(HTTPResponse.))]
    class HTTPManager_SendRequest_Patch
    {
        static bool Prefix(HTTPRequest __instance, OnRequestFinishedDelegate value)
        {
            // 创建一个新的回调函数，将原始回调函数的响应数据以Base64编码的形式传递给新的回调函数
            OnRequestFinishedDelegate newCallback = (req, resp) =>
            {
                byte[] data = resp.Data;
                string dataBase64 = Convert.ToBase64String(data);
                value(req, new HTTPResponse(resp.StatusCode, resp.Headers, dataBase64));
            };

            // 调用原始的SetCallback函数，将新的回调函数传递给它
            __instance.SetCallback(newCallback);

            // 返回false，继续调用原始的Callback函数
            return false;
        }

        static void Prefix(HTTPRequest __result)
        {
            //打印HTTPRequest的Uri
            var uri = typeof(HTTPRequest).GetProperty("Uri").GetValue(__result, null);

            string uriString = uri.GetType().GetMethod("ToString").Invoke(uri, null) as string;

            MelonLogger.Msg("HTTPManager.SendRequest: " + uriString);

            string stateName = Enum.GetName(typeof(HTTPRequestStates), __result.State);
            MelonLogger.Msg("State : " + stateName);

            //是否是玩家装扮的json
            if (uriString.Contains("fetchGGDUserDataVer4"))
            {
                //string responseString = System.Text.Encoding.UTF8.GetString(request.Response.DataAsText);

                //MelonLogger.Msg(request.Response.DataAsText);
            }
        }

        static string ChangeJson(string raw)
        {
            return "";
        }
    }*/
}
