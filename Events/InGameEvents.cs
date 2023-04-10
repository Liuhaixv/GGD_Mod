using MelonLoader;
using HarmonyLib;
using System;
using APIs.Photon;
using GGD_Hack.GameData;
using ExitGames.Client.Photon;
using UnhollowerBaseLib;
using GGD_Hack.Features;

namespace GGD_Hack.Events
{
    public class InGameEvents
    {
        public static void Start_Game()
        {
            MelonLogger.Msg(System.ConsoleColor.Green, "游戏开始事件");
        }

        public static void OnGameEnded()
        {
            MelonLogger.Msg(System.ConsoleColor.Green, "游戏结束");
            //TODO:
            throw new NotImplementedException();
        }

        public static void Vent(string userId, string ventId)
        {
            MelonLogger.Msg(System.ConsoleColor.Green, "钻管道事件：玩家: {0} 管道id: {1}", userId, ventId);
        }

        public static void ForceExitVent(string userId, string ventId)
        {
            MelonLogger.Msg(System.ConsoleColor.Green, "强制移出管道事件：玩家: {0} 管道id: {1}", userId, ventId);
        }

        public static void Pelican_Eat(string playerEaten, string pelican = null)
        {
            MelonLogger.Msg(System.ConsoleColor.Green, "鹈鹕吃人事件：鹈鹕：{0} 被吃的玩家: {1}", string.IsNullOrEmpty(pelican) ? "未知" : pelican, playerEaten);
        }

        public static void Turn_Invisible(string userId, bool invisible)
        {
            MelonLogger.Msg(System.ConsoleColor.Green, "隐身事件：玩家:{0} 隐身中:{1}", userId, invisible ? "是" : "否");
        }

        public static void Grab_Body(string undertakerUserId, string bodyUserId)
        {
            MelonLogger.Msg(System.ConsoleColor.Green, "开始拖拽尸体事件：丧葬者:{0} 被拖拽的尸体:{1}", undertakerUserId, bodyUserId);
        }

        public static void Receive_Kill(string killerUserId, string killedUserId, string stingerId)
        {
            MelonLogger.Msg(System.ConsoleColor.Green, "玩家被杀事件：击杀者:{0} 被杀者:{1} 死亡动画id:{2}", string.IsNullOrEmpty(killerUserId) ? "未知" : killerUserId, killedUserId, stingerId);
        }

        //食鸟鸭或秃鹫
        public static void Eat(string eater, string eated)
        {
            MelonLogger.Msg(System.ConsoleColor.Green, "尸体被吃事件：吃者:{0} 被吃者:{1}", eater, eated);
        }

        //变形
        public static void Morph(string from, string to)
        {
            MelonLogger.Msg(System.ConsoleColor.Green, "变形事件：From:{0} To:{1}", from, to);
        }

        public static void Task_Completed(string userId, string taskId, int i, string tokenName, int earnedTokenNums)
        {
            //TODO
        }

        public static void Whistleblow_Bomb()
        {
            //TODO
            MelonLogger.Msg(System.ConsoleColor.Green, "传递炸弹事件：{0}");
        }

        public static void Generate_Bomb(string userId)
        {
            MelonLogger.Msg(System.ConsoleColor.Green, "生成炸弹事件：{0}", userId);
        }

        public static void Celebrity_Died()
        {
            MelonLogger.Msg(System.ConsoleColor.Green, "网红死亡事件");
        }

        public static void Server_Send_Role(Il2CppStringArray userIdArray, Il2CppStringArray roleIdArray, Il2CppStringArray taskIdArray)
        {
            //TODO
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
                    case EventDataCode.START_GAME:
                        {
                            InGameEvents.Start_Game();
                            break;
                        }
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

                            if (!string.IsNullOrWhiteSpace(pelicanUserId))
                            {
                                InGameEvents.Pelican_Eat(playerEaten, pelicanUserId);
                            }
                            else
                            {
                                InGameEvents.Pelican_Eat(playerEaten);
                            }

                            //MelonLogger.Msg(System.ConsoleColor.Green, parameters.ToStringFull());

                            break;
                        }
                    case EventDataCode.TURN_INVISIBLE:
                        {
                            //[04:33:00.297] [[开发者专用版]_Liuhaixv's_GGD_Hack_mod] 接收到事件: TURN_INVISIBLE
                            //[04:33:00.297] [[开发者专用版] _Liuhaixv's_GGD_Hack_mod] Event 77: {(Byte)245=(String[]){HRr2vu3eMTOXaOt9lPcMWN43fX22,true}, (Byte)254=(Int32)0}

                            Il2CppStringArray stringArray = parameters.Get<Il2CppStringArray>(245);
                            string userId = stringArray[0];
                            bool invisible = stringArray[1] == "true";

                            InGameEvents.Turn_Invisible(userId, invisible);
#if Developer
                            MelonLogger.Msg(System.ConsoleColor.Green, parameters.ToStringFull());
#endif
                            break;
                        }
                    case EventDataCode.CHAT_MESSAGE:
                        {
                            /*
                            //[08:19:39.574] [[开发者专用版]_Liuhaixv's_GGD_Hack_mod] 接收到事件: CHAT_MESSAGE
                            //[08:19:39.575] [[开发者专用版] _Liuhaixv's_GGD_Hack_mod] Event 66: {(Byte)245=(Object[])System.Object[], (Byte)254=(Int32)0}
                            Il2CppArrayBase<Il2CppSystem.Object> objArray = parameters.Get<Il2CppArrayBase<Il2CppSystem.Object>>(245);

                            MelonLogger.Msg(System.ConsoleColor.Green, "收到聊天消息");
                            foreach (Il2CppSystem.Object obj in objArray)
                            {
                                MelonLogger.Msg(System.ConsoleColor.Green, obj.ToString());
                            }

                            Il2CppStringArray il2CppStringArray = new Il2CppStringArray(objArray[1].Pointer);

                            foreach (var str in il2CppStringArray)
                            {
                                MelonLogger.Msg(System.ConsoleColor.Green, str);
                            }
                            */
                            break;
                        }
                    case EventDataCode.VENT:
                        {
                            //[10:27:04.863] [[开发者专用版]_Liuhaixv's_GGD_Hack_mod] 接收到事件: VENT
                            //[10:27:04.863] [[开发者专用版] _Liuhaixv's_GGD_Hack_mod] Event 7: {(Byte)245=(String[]){0coZbhOkb6QhJzDhYynkqknr32l1,4,false}, (Byte)254=(Int32)0}
                            Il2CppStringArray stringArray = parameters.Get<Il2CppStringArray>(245);
                            string userId = stringArray[0];
                            string ventId = stringArray[1];

                            InGameEvents.Vent(userId, ventId);

                            //MelonLogger.Msg(System.ConsoleColor.Green, parameters.ToStringFull());

                            break;
                        }
                    case EventDataCode.FORCE_EXIT_VENT:
                        {
                            //[10:26:53.975] [[开发者专用版]_Liuhaixv's_GGD_Hack_mod] 接收到事件: FORCE_EXIT_VENT
                            //[10:26:53.975] [[开发者专用版] _Liuhaixv's_GGD_Hack_mod] Event 120: {(Byte)245=(String[]){0coZbhOkb6QhJzDhYynkqknr32l1,4}, (Byte)254=(Int32)0}
                            MelonLogger.Msg(System.ConsoleColor.Green, parameters.ToStringFull());

                            Il2CppStringArray stringArray = parameters.Get<Il2CppStringArray>(245);
                            string userId = stringArray[0];
                            string ventId = stringArray[1];

                            InGameEvents.ForceExitVent(userId, ventId);

                            break;
                        }
                    case EventDataCode.GRAB_BODY:
                        //[13:21:00.182] [[开发者专用版]_Liuhaixv's_GGD_Hack_mod] 接收到事件: GRAB_BODY
                        //[13:21:00.183] [[开发者专用版] _Liuhaixv's_GGD_Hack_mod] Event 59: {(Byte)245=(String[]){1l4kqYawXVQs55e23TlT6KKyCEB3,7GWsdxRFvkRYaH4qZqJBMtqHqH72}, (Byte)254=(Int32)0}
                        {
                            //TODO:System.OverflowException
                            MelonLogger.Msg(System.ConsoleColor.Green, parameters.ToStringFull());

                            Il2CppStringArray stringArray = parameters.Get<Il2CppStringArray>(245);
                            string undertakerUserId = stringArray[0];
                            string bodyUserId = stringArray[1];

                            InGameEvents.Grab_Body(undertakerUserId, bodyUserId);

                            break;
                        }
                    case EventDataCode.RECEIVE_KILL:
                        {
                            //[10:50:16.013] [[开发者专用版] _Liuhaixv's_GGD_Hack_mod] 接收到事件: RECEIVE_KILL
                            //[10:50:16.013][[开发者专用版]_Liuhaixv's_GGD_Hack_mod] Event 3: {(Byte)245=(String[]){,INHFrQihlYUg8yvPmwHREre61632,100080148,1}, (Byte)254=(Int32)0}

                            MelonLogger.Msg(System.ConsoleColor.Green, parameters.ToStringFull());

                            Il2CppStringArray stringArray = parameters.Get<Il2CppStringArray>(245);
                            string killerUserId = stringArray[0];
                            string bodyUserId = stringArray[1];
                            string stingerId = stringArray[2];

                            InGameEvents.Receive_Kill(killerUserId, bodyUserId, stingerId);

                            break;
                        }
                    case EventDataCode.EAT:
                        {
                            //[15:10:51.439] [[开发者专用版]_Liuhaixv's_GGD_Hack_mod] 接收到事件: EAT
                            //[15:10:51.440] [[开发者专用版] _Liuhaixv's_GGD_Hack_mod] Event 19: {(Byte)245=(String[]){FNV2BrK1ZVagjkjDoC0oQa61TgZ2,wV6chtJmnSQG9wyIMS2sAkDwUAq2}, (Byte)254=(Int32)0}
                            MelonLogger.Msg(System.ConsoleColor.Green, parameters.ToStringFull());

                            Il2CppStringArray stringArray = parameters.Get<Il2CppStringArray>(245);
                            string eater = stringArray[0];
                            string eated = stringArray[1];

                            InGameEvents.Eat(eater, eated);

                            break;
                        }
                    case EventDataCode.MORPH:
                        {
                            //[15:40:36.278] [[开发者专用版]_Liuhaixv's_GGD_Hack_mod] 接收到事件: MORPH
                            //[15:40:36.278] [[开发者专用版]_Liuhaixv's_GGD_Hack_mod] Event 21: {(Byte)245=(String[]){qWFC7AOTSsZMGNq48SaFEGB2LTd2,JtCDSJJPGVWs1Hkt8VjQzEObrV83,false}, (Byte)254=(Int32)0}
                            MelonLogger.Msg(System.ConsoleColor.Green, parameters.ToStringFull());

                            Il2CppStringArray stringArray = parameters.Get<Il2CppStringArray>(245);
                            string from = stringArray[0];
                            string to = stringArray[1];

                            InGameEvents.Morph(from, to);

                            break;
                        }
                    case EventDataCode.WHISTLEBLOW_BOMB:
                        {
                            //TODO
                            MelonLogger.Msg(System.ConsoleColor.Green, parameters.ToStringFull());

                            Il2CppArrayBase<Il2CppSystem.Object> objArray = parameters.Get<Il2CppArrayBase<Il2CppSystem.Object>>(245);

                            MelonLogger.Msg(System.ConsoleColor.Green, "收到炸弹WHISTLEBLOW_BOMB");

                            break;
                        }
                    case EventDataCode.GENERATE_BOMB:
                        {
                            //[16:51:49.082] [[开发者专用版]_Liuhaixv's_GGD_Hack_mod] 接收到事件: GENERATE_BOMB
                            //[16:51:49.083] [[开发者专用版] _Liuhaixv's_GGD_Hack_mod] Event 62: {(Byte)245=(String)dVirhupARGcyHuwL1FEkIDBCU3n1, (Byte)254=(Int32)9}
                            //TODO
                            MelonLogger.Msg(System.ConsoleColor.Green, parameters.ToStringFull());

                            string userId = parameters.Get<string>(245);

                            MelonLogger.Msg(System.ConsoleColor.Green, "生成炸弹GENERATE_BOMB:");

                            InGameEvents.Generate_Bomb(userId);
                            break;
                        }
                    case EventDataCode.CELEBRITY_DIED:
                        {
                            InGameEvents.Celebrity_Died();
                            break;
                        }
                    case EventDataCode.SERVER_SEND_ROLE:
                        {
                            //TODO
                            MelonLogger.Msg(System.ConsoleColor.Green, parameters.ToStringFull());

                            Il2CppReferenceArray<Il2CppStringArray> stringArrayArray = parameters.Get<Il2CppReferenceArray<Il2CppStringArray>>(245);
                            Il2CppStringArray userIdArray = stringArrayArray[0];
                            Il2CppStringArray roleIdArray = stringArrayArray[1];
                            Il2CppStringArray taskIdArray = stringArrayArray[4];

                            InGameEvents.Server_Send_Role(userIdArray, roleIdArray, taskIdArray);

                            break;
                        }

                    case EventDataCode.AppStats:
                        {
                            //TODO
                            break;
                        }

                }
            }
            catch (System.Exception e)
            {
                MelonLogger.Error("InGameEvent发生异常：" + e.ToString());
            }
        }
    }
}
