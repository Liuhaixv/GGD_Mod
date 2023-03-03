using APIs.Photon;
using GGD_Hack.GameData;
using HarmonyLib;
using Il2CppSystem;
using MelonLoader;
using System;
using System.Diagnostics;
using Enum = System.Enum;

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
                            return true;
                    }

                    MelonLogger.Msg("接收到事件: " + eventName);
                    MelonLogger.Msg(__0.ToStringFull());

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
