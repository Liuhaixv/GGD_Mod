#if Developer
using ExitGames.Client.Photon;
using Handlers.GameHandlers.PlayerHandlers;
using HarmonyLib;
using MelonLoader;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnhollowerBaseLib.Runtime;

namespace GGD_Hack.Hook
{
    public class PhotonNetwork_
    {
        /// https://doc-api.photonengine.com/en/pun/v2/class_photon_1_1_pun_1_1_photon_network.html#af05461cb7a83c79bbd206bb2ed2a07b8
        /// <summary>
        /// 在房间内发送完全可定制的事件。事件至少包含一个 EventCode (0..199)，并且可以包含内容。
        /// </summary>
        //[HarmonyPatch(typeof(PhotonNetwork), nameof(PhotonNetwork.RaiseEvent))]
        class RaiseEvent_
        {
            static bool Prefix(byte eventCode, Il2CppSystem.Object eventContent, RaiseEventOptions raiseEventOptions, SendOptions sendOptions)
            {
                try
                {
                    MelonLogger.Msg("============================");
                    MelonLogger.Msg("[RaiseEvent-eventCode] " + Convert.ToChar(eventCode).ToString());

                    string eventContentString = eventContent.ToString();

                    MelonLogger.Msg("[RaiseEvent-eventContent] " + eventContentString);
                    MelonLogger.Msg("============================");

                }
                catch(Exception e)
                {
                    MelonLogger.Error(e.ToString());
                }                

                return true;
            }
        }
    }
}
#endif