using APIs.Photon;
using ExitGames.Client.Photon;
using HarmonyLib;
using MelonLoader;
using Photon.Realtime;
using System;

//OnPlayerPropertiesUpdate -> Handlers_GameHandlers_PlayerHandlers_SpawnedPlayerHandler__UpdatePlayerProperties

namespace GGD_Hack.Hook
{
    /// <summary>
    /// 服务器事件回调API
    /// </summary>
    public class PhotonCallbacksAPI_
    {
        /*
        [HarmonyPatch(typeof(PhotonCallbacksAPI), nameof(PhotonCallbacksAPI.OnPlayerPropertiesUpdate), typeof(Player), typeof(Hashtable))]
        class OnPlayerPropertiesUpdate_
        {
            static bool Postfix(byte eventCode, Il2CppSystem.Object eventContent, RaiseEventOptions raiseEventOptions, SendOptions sendOptions)
            {
                MelonLogger.Msg("[RaiseEvent-eventCode] " + Convert.ToChar(eventCode).ToString());

                string eventContentString = eventContent.ToString();
                               
                MelonLogger.Msg("[RaiseEvent-eventContent] " + eventContentString);

                return true;
            }
        }
        */
    }
}
