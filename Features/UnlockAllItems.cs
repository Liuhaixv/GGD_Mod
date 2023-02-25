
using BestHTTP;
using ExitGames.Client.Photon;
using HarmonyLib;
using MelonLoader;
using Il2CppSystem;
using UnhollowerBaseLib;

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
                Enabled = MelonPreferences.CreateEntry<bool>("GGDH", nameof(UnlockAllItems), false);
            }
            else
                Enabled = MelonPreferences.GetEntry<bool>("GGDH", nameof(UnlockAllItems));
        }
    }

    [HarmonyPatch(typeof(OnRequestFinishedDelegate), nameof(OnRequestFinishedDelegate.Invoke))]
    public static class HTTPResponse_Data_Getter_Patch
    {
        static void Postfix(BestHTTP.OnRequestFinishedDelegate __instance, BestHTTP.HTTPRequest __0, BestHTTP.HTTPResponse __1)
        {
            //打印HTTPRequest的Uri
           
            
                //var uri = typeof(HTTPRequest).GetProperty("Uri").GetValue(__0, null);
                //string uriString = uri.GetType().GetMethod("ToString").Invoke(uri, null) as string;
                //MelonLogger.Msg("HTTPManager.SendRequest: " + uriString);
                MelonLogger.Msg("HTTPManager.SendRequest: " + __0.ToString());
            

            //MelonLogger.Msg(__0.Uri..DataAsText);
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
