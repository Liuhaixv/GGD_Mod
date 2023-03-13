using BestHTTP;
using ExitGames.Client.Photon;
using HarmonyLib;
using MelonLoader;
using UnhollowerBaseLib;

using Managers.InfoManagers;
using Objects;
using Il2CppSystem.Collections.Generic;
using Managers;
using Managers.PlayerManagers;
using Handlers.LobbyHandlers;
using UnhollowerRuntimeLib;
using UnityEngine;

using IntPtr = System.IntPtr;
using GGD_Hack.Hook;
//using HTTPResponse = BestHTTP.HTTPResponse;

namespace GGD_Hack.Features
{
    [RegisterTypeInIl2Cpp]
    public class ShowAllUnlockables : MonoBehaviour
    {
        public static ShowAllUnlockables Instance;
        public static MelonPreferences_Entry<bool> Enabled;
        //更新皮肤的时间间隔
        //public static MelonPreferences_Entry<float> updateInterval = MelonPreferences.CreateEntry("GGDH", nameof(UnlockAllItems.updateInterval), 25.0f);

        public ShowAllUnlockables(IntPtr ptr) : base(ptr)
        {
            if (!MelonPreferences.HasEntry("GGDH", nameof(ShowAllUnlockables)))
            {
                Enabled = MelonPreferences.CreateEntry<bool>("GGDH", nameof(ShowAllUnlockables), true);
            }
            else
            {
                Enabled = MelonPreferences.GetEntry<bool>("GGDH", nameof(ShowAllUnlockables));
            }
        }

        // Optional, only used in case you want to instantiate this class in the mono-side
        // Don't use this on MonoBehaviours / Components!
        public ShowAllUnlockables() : base(ClassInjector.DerivedConstructorPointer<ShowAllUnlockables>()) => ClassInjector.DerivedConstructorBody(this);
        public static void Init()
        {
            GameObject ML_Manager = GameObject.Find("ML_Manager");
            if (ML_Manager == null)
            {
                ML_Manager = new GameObject("ML_Manager");
                DontDestroyOnLoad(ML_Manager);
            }

            if (ML_Manager.GetComponent<ShowAllUnlockables>() == null)
            {
                Instance = ML_Manager.AddComponent<ShowAllUnlockables>();
            }
        }

        /*
        public static void SetUpdateTime(float time)
        {
            updateInterval.Value = time;
        }*/

        /// <summary>
        /// 通知服务器更新玩家装扮
        /// </summary>
        public static void UpdateTempUserUnlockables()
        {
            PlayerPropertiesManager playerPropertiesManager = MainManager.Instance.playerPropertiesManager;
            //获取缓存属性
            Dictionary<string, string> dict = new Dictionary<string, string>();

            //empty_Farts={int32}
            //color={int32}
            //userId
            dict.Add("hat", playerPropertiesManager.tempHat);
            dict.Add("clothes", playerPropertiesManager.tempClothes);
            dict.Add("fart", playerPropertiesManager.tempFart);
            dict.Add("pet", playerPropertiesManager.tempPet);
            dict.Add("stinger", playerPropertiesManager.tempStinger);
            dict.Add("Banners", playerPropertiesManager.tempBanner);
            dict.Add("Cards", playerPropertiesManager.tempCard);

            //dict.Add("nickname", UnityEngine.Random.RandomRangeInt(1000, 9999).ToString());

            //Not Preferred
            //usedRoomCode
            //WarpSpeed
            //usedRoomCode
            //C6 = true
            //<color #FF0000> 
            //Not Preferred
            //usedRoomCode
            //Not Preferred
            //usedRoomCode
            //Not Preferred
            //usedRoomCode
            //Not Preferred
            //usedRoomCode
            //Not Preferred
            //usedRoomCode
            MainManager.Instance.playerPropertiesManager.ChangeUserProperties(dict);
        }

        private void Update()
        {
        }
    }

    //ggdUnlockables 所有可用物品
    //userUnlockables 玩家已拥有物品
    [HarmonyPatch(typeof(UnlockablesManager), nameof(UnlockablesManager.ProcessGGDDataAndContinue))]
    class ProcessGGDDataAndContinue_
    {

        //显示所有不可用物品
        static void Prefix(ref GGDDataBody __0)
        {
            //功能未启用
            if (ShowAllUnlockables.Enabled.Value == false)
            {
                return;
            }

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

            //不可用的物品的价格
            UnlockableRecipe recipeOfNotAvailable = new UnlockableRecipe();
            recipeOfNotAvailable.recipeCosts = new Il2CppReferenceArray<UnlockableCost>(1);

            UnlockableCost cost = new UnlockableCost();
            cost.cost = 9999;
            cost.currencyType = "gold";

            recipeOfNotAvailable.recipeCosts[0] = cost;

            //List<string> categoryIds = new List<string>();
            //所有物品
            foreach (UnlockableInfo unlockableInfo in unlockableInfos)
            {

                Unlockable rawUnlockable = unlockableInfo.rawUnlockable;


                //跳过名片和横幅
                /*
                if (rawUnlockable.type == "Banners" || rawUnlockable.type == "Cards")
                {
                    continue;
                }*/

                //跳过不可用宠物
                /*
                if (rawUnlockable.type == "Pets" && unlockableInfo.isAvailable == false)
                {
                    continue;
                }*/

                //为所有不可用的物品加上价格强制显示
                if (!unlockableInfo.isAvailable)
                {
                    //MelonLogger.Msg(System.ConsoleColor.Green,"物品不可以用，正在添加价格并显示");
                    rawUnlockable.recipes = new Il2CppReferenceArray<UnlockableRecipe>(1);
                    rawUnlockable.recipes[0] = recipeOfNotAvailable;
                    unlockableInfo.isAvailable = true;
                }


                rawUnlockable.startDate = availableTemplate.rawUnlockable.startDate;
                rawUnlockable.expirationDate = availableTemplate.rawUnlockable.expirationDate;
                rawUnlockable.saleStartDate = availableTemplate.rawUnlockable.saleStartDate;
                rawUnlockable.saleExpirationDate = availableTemplate.rawUnlockable.saleExpirationDate;

                //只屏蔽非横幅和卡片的解锁条件
                /*
                if (rawUnlockable.type != "Banners" && rawUnlockable.type != "Cards")
                    rawUnlockable.requirements = availableTemplate.rawUnlockable.requirements;
                */

                unlockableInfo.isAvailableExpiringSoon = availableTemplate.isAvailableExpiringSoon;
                unlockableInfo.isOnSale = availableTemplate.isOnSale;
                unlockableInfo.isOnSaleExpiringSoon = availableTemplate.isOnSaleExpiringSoon;
                unlockableInfo.timeUntilIsAvailableExpiring = availableTemplate.timeUntilIsAvailableExpiring;
                unlockableInfo.timeUntilIsOnSaleExpiring = availableTemplate.timeUntilIsOnSaleExpiring;
            }
        }

        //解锁物品
        /*
        static void Postfix(ref GGDDataBody __0)
        {            
            GGDDataBody dataBody = __0;

            UnlockablesInfoBody ggdUnlockables = dataBody.ggdUnlockables;
            if (ggdUnlockables == null) { MelonLogger.Error("ggdUnlockables不存在"); return; }
            Il2CppReferenceArray<UnlockableInfo> unlockableInfos = ggdUnlockables.unlockableInfo;
            if (unlockableInfos == null) { MelonLogger.Error("unlockableInfo不存在"); return; }
            dataBody.userUnlockables = new Dictionary<string, UserUnlockable>();
            Dictionary<string, UserUnlockable> userUnlockables = dataBody.userUnlockables;
            if (userUnlockables == null)
            {
                MelonLogger.Error("userUnlockables不存在"); return;
            }

            //所有物品
            foreach (UnlockableInfo unlockableInfo in unlockableInfos)
            {
                Unlockable rawUnlockable = unlockableInfo.rawUnlockable;

                //解锁所有横幅和卡片
                if (rawUnlockable.type == "Cards" || rawUnlockable.type == "Banners")
                {
                    UserUnlockable userUnlockable = new UserUnlockable();
                    userUnlockable.timestamp = 1677158242542;
                    userUnlockables[rawUnlockable.id.ToString()] = userUnlockable;
                }
            }
        }*/
    }

    [HarmonyPatch(typeof(PlayerCustomizationPanelHandler), nameof(PlayerCustomizationPanelHandler.IsOwnedOrFree))]
    class IsOwnedOrFree_
    {
        static void Postfix(ref bool __result)
        {
#if !Developer
            //只有开发者版本启用
            return;
#else
            if (ShowAllUnlockables.Enabled.Value == true)
            {
                // __result = false;
            }
#endif
        }
    }
}
