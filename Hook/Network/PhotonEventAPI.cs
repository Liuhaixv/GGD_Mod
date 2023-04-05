using APIs.Photon;
using GGD_Hack.GameData;
using HarmonyLib;
using Il2CppSystem;
using Il2CppSystem.Collections;
using Il2CppSystem.Collections.Generic;
using MelonLoader;
using System;
using System.Diagnostics;
using Enum = System.Enum;
using Exception = System.Exception;
using Hashtable = Il2CppSystem.Collections.Hashtable;

//发送给服务器调用的api为Photon_Realtime_LoadBalancingClient__OpRaiseEvent ->
//      //向同一房间的其他玩家发送带有自定义代码/类型和任何内容的事件
//      bool __stdcall Photon_Realtime_LoadBalancingPeer__OpRaiseEvent(
//        Photon_Realtime_LoadBalancingPeer_o * this,
//        uint8_t eventCode,
//        Il2CppObject *customEventContent,
//        Photon_Realtime_RaiseEventOptions_o *raiseEventOptions,
//        ExitGames_Client_Photon_SendOptions_o sendOptions,
//        const MethodInfo *method)
//高一级有SendEventToPlugin

namespace GGD_Hack.Hook
{
    public class PhotonEventAPI_
    {
        /// <summary>
        /// 游戏开始
        /// </summary>
        [HarmonyPatch(typeof(PhotonEventAPI), nameof(PhotonEventAPI.OnEvent), typeof(ExitGames.Client.Photon.EventData))]
        class OnEvent_
        {
            static bool Prefix(ExitGames.Client.Photon.EventData __0)
            {
                try
                {
#if Developer
                    bool shouldBlockEvent = false;
                    int code = __0.Code;

                    string eventName = "";

                    //获取枚举的名字
                    if (System.Enum.IsDefined(typeof(EventDataCode), code))
                    {
                        eventName = Enum.GetName(typeof(EventDataCode), code);
                    }

                    //pass的事件
                    switch (code)
                    {
                        case (int)EventDataCode.UnreliableRead:
                        case (int)EventDataCode.ReliableRead:
                        case (int)EventDataCode.Rpc:
                            return true;
                    }

                    MelonLogger.Msg("========================");
                    MelonLogger.Msg("接收到事件: " + eventName);
                    MelonLogger.Msg(__0.ToStringFull());

                    //打印事件
                    switch (code)
                    {
                        case (int)EventDataCode.PropertiesChanged:
                            try
                            {
                                /*
                                ExitGames.Client.Photon.ParameterDictionary parameters = __0.Parameters;
                                MelonLogger.Msg("ParameterDictionary:" + parameters.ToString());
                                byte byteKey = 251;
                                ExitGames.Client.Photon.Hashtable hashTable = parameters.Get<ExitGames.Client.Photon.Hashtable>(byteKey);
                                MelonLogger.Msg("Hashtable:" + hashTable);
                                foreach(var entry in hashTable)
                                {
                                    string key = entry.Key.ToString();
                                    var playerProperties =  entry.Value;
                                    MelonLogger.Msg("hashTable键值对:" + key + "=" + entry.Value.ToString());

                                    //Print each player properties
                                    
                                        MelonLogger.Msg("playerProperties:" + playerProperties.ToString());
                                    break;
                                }
                                */

                            }
                            catch (Exception e)
                            {
                                MelonLogger.Warning(e.Message);
                            }
                            break;
                    }


                    //屏蔽事件
                    switch (code)
                    {   //反作弊
                        case (int)EventDataCode.AntiCheat:
                            //case (int)EventDataCode.PropertiesChanged:
                            //测试
                            //case 226:
                            //case (int)EventDataCode.RECEIVE_KILL:
                            shouldBlockEvent = true;
                            break;
                        case (int)EventDataCode.KICK_PLAYER:
                            //shouldBlockEvent = true;
                            break;
                    }

                    //打印追踪栈
                    switch (code)
                    {
                        //case (int)EventDataCode.PropertiesChanged:
                        case 666666:
                            StackTrace stackTrace = new StackTrace();
                            string stackTraceString = stackTrace.ToString();
                            MelonLogger.Warning(stackTraceString);
                            break;
                        default:
                            break;
                    }


                    //开始屏蔽事件
                    if (shouldBlockEvent)
                    {
                        MelonLogger.Warning("已屏蔽事件: " + eventName + '\n');
                        return false;
                    }
                    else
                    {
                        return true;
                    }
#else
                    return true;
#endif
                }
                catch (System.Exception e)
                {
                    MelonLogger.Error(e.ToString());
                    return true;
                }
            }
        }
    }
}
