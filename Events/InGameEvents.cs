﻿using MelonLoader;
using HarmonyLib;
using System;
using APIs.Photon;
using GGD_Hack.GameData;
using ExitGames.Client.Photon;
using Il2CppSystem.Runtime.InteropServices;
using UnhollowerBaseLib;

namespace GGD_Hack.Events
{
    public class InGameEvents
    {
        public static void OnGameStart()
        {
            MelonLogger.Msg(System.ConsoleColor.Green, "游戏开始");
            //TODO:
            throw new NotImplementedException();
        }

        public static void OnGameEnded()
        {
            MelonLogger.Msg(System.ConsoleColor.Green, "游戏结束");
            //TODO:
            throw new NotImplementedException();
        }

        public static void Pelican_Eat(string playerEaten, string pelican = null)
        {
            MelonLogger.Msg(System.ConsoleColor.Green, "鹈鹕吃人事件：鹈鹕：{0} 被吃的玩家: {1}", string.IsNullOrEmpty(pelican) ? "未知" : pelican, playerEaten);
        }
    }

    [HarmonyPatch(typeof(PhotonEventAPI), nameof(PhotonEventAPI.OnEvent), typeof(ExitGames.Client.Photon.EventData))]
    class PhotonEventAPI_OnEvent
    {
        static void Postfix(ExitGames.Client.Photon.EventData __0)
        {
            try
            {
                int code = __0.Code;
                //事件参数
                ParameterDictionary parameters = __0.Parameters;

                EventDataCode eventDataCode = (EventDataCode)code;

#if false
                MelonLogger.Msg(System.ConsoleColor.Green, "InGameEvents:" + eventDataCode.ToString());
#endif

                //打印事件
                switch (eventDataCode)
                {
                    case EventDataCode.PELICAN_EAT:
                        {  //[03:21:39.440] [[开发者专用版]_Liuhaixv's_GGD_Hack_mod] 接收到事件: PELICAN_EAT
                           //[03:21:39.441][[开发者专用版] _Liuhaixv's_GGD_Hack_mod] Event 78: {(Byte)245=(String[]){7GWsdxRFvkRYaH4qZqJBMtqHqH72,hgqoiMN88QVgFMdgHMh512CJS0H3}, (Byte)254=(Int32)0}
                           //7GWsdxRFvkRYaH4qZqJBMtqHqH72 是自己、鹈鹕
                           //hgqoiMN88QVgFMdgHMh512CJS0H3 被吃

                            //[03:45:56.043] [[开发者专用版]_Liuhaixv's_GGD_Hack_mod] 接收到事件: PELICAN_EAT
                            //[03:45:56.043][[开发者专用版] _Liuhaixv's_GGD_Hack_mod] Event 78: {(Byte)245=(String[]){,gmf2ssN689dDRprV28CuJOhDLLt2}, (Byte)254=(Int32)0}


                            Il2CppStringArray stringArray = parameters.Get<Il2CppStringArray>(245);
                            string pelicanUserId = stringArray[0];
                            string playerEaten = stringArray[1];

                            if (pelicanUserId != null)
                            {
                                InGameEvents.Pelican_Eat(playerEaten, pelicanUserId);
                            }
                            else
                            {
                                InGameEvents.Pelican_Eat(playerEaten);
                            }

                            MelonLogger.Msg(System.ConsoleColor.Green, parameters.ToStringFull());
                            foreach (var obj in parameters)
                            {
                                MelonLogger.Msg(System.ConsoleColor.Green, obj.ToString());
                            }
                            break;
                        }
                    case EventDataCode.AppStats:
                        {
                            break;
                        }
                }
            }
            catch (System.Exception e)
            {
                MelonLogger.Error(e.ToString());
            }
        }
    }
}
