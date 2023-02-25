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

//using HTTPResponse = BestHTTP.HTTPResponse;

namespace GGD_Hack.Features
{
    public static class UnlockAllItems
    {
        public static MelonPreferences_Entry<bool> Enabled;

        static UnlockAllItems()
        {
            if (!MelonPreferences.HasEntry("GGDH", nameof(UnlockAllItems)))
            {
                Enabled = MelonPreferences.CreateEntry<bool>("GGDH", nameof(UnlockAllItems), true);
            }
            else
                Enabled = MelonPreferences.GetEntry<bool>("GGDH", nameof(UnlockAllItems));
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

            //List<string> categoryIds = new List<string>();
            //所有物品
            foreach(UnlockableInfo unlockableInfo in unlockableInfos)
            {
               

                unlockableInfo.isAvailable = true;
                //unlockableInfo.isOnSale = true;
                unlockableInfo.isAvailableExpiringSoon = false;
                unlockableInfo.isOnSaleExpiringSoon = false;

                Unlockable rawUnlockable = unlockableInfo.rawUnlockable;

                rawUnlockable.requirements = null;
                rawUnlockable.recipes = null;
                //rawUnlockable.onSale = false;

                //categoryIds.Add(rawUnlockable.categoryId);
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
