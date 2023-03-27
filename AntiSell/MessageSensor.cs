using APIs.Photon;
using GGD_Hack.GameData;
using Handlers.PrefabAttachedHandlers;
using HarmonyLib;
using Managers;
using Managers.ConnectionManagers;
using MelonLoader;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

//过滤发送的聊天消息
namespace GGD_Hack.AntiSell
{
    public class MessageSensor
    {
        public static bool IsSpamming
        {
            get
            {
                return spamDetector.isSpamming;
            }
        }
        public static string spammingReplacement = "免费mod群277392777";

        //屏蔽qq群号并篡改
        public static SensorMessageHacker sensorMessageHacker = new SensorMessageHacker(@"[ 0-9]{6,}", "277392777");

        //检测垃圾消息
        public static SpamDetector spamDetector = new SpamDetector(2, 12, 0.8);

        //自动将本地发送到服务器的消息通过正则匹配记录下，然后在收到服务器回调创建消息对象时篡改显示，实现本地无法知道消息被篡改的效果
        public class SensorMessageHacker
        {
            // 要替换的文本
            // 6位以上数字
            private string ruleRegex = null;

            // 要被替换成的文本
            private string replaceWith = null;

            //hacked -> raw
            public Dictionary<string, string> hackedStrings = new Dictionary<string, string>();

            public SensorMessageHacker(string regex, string replaceWith)
            {
                this.ruleRegex = regex;
                this.replaceWith = replaceWith;
            }

            //正则匹配message是否包含违规文本，有就替换为replaceWith
            //hackedString是替换后的文本,将hackedString映射raw，更新字典hackedStrings
            public string Hack(string message)
            {
                string raw = message;
                string hackedString = Regex.Replace(message, ruleRegex, replaceWith);
                if (raw != hackedString)
                {
                    hackedStrings[hackedString] = raw;
                }
                return hackedString;
            }

            //查找字典hackedStrings中是否存在改字符串对应的键，有就返回键对应的值
            public string Unhack(string possibleModifiedString)
            {
                if (hackedStrings.ContainsKey(possibleModifiedString))
                {
                    return hackedStrings[possibleModifiedString];
                }
                else
                {
                    return possibleModifiedString;
                }
            }
        }

        //修改发送给服务器的消息
        [HarmonyPatch(typeof(PhotonEventAPI), nameof(PhotonEventAPI.SendEventToPlugin))]
        public class SendChatMessageEvent_
        {
            /*
            static void Prefix(byte __0, ref Il2CppSystem.Object __1, bool __2)
            {
                try
                {
                    int eventCode = __0;

                    //检查是否是聊天消息
                    if(__0 != (int)EventDataCode.CHAT_MESSAGE)
                    {
                        return;
                    }

                    UnhollowerBaseLib.Il2CppStringArray strsFromObj = new UnhollowerBaseLib.Il2CppStringArray(__1.Pointer);

                }
                catch (System.Exception ex)
                {
                 }
            }*/

            static void Prefix(byte __0, ref Il2CppSystem.Object __1, bool __2)
            {
                try
                {                  
                    int eventCode = __0;

                    //检查是否是聊天消息
                    if (__0 != (int)EventDataCode.CHAT_MESSAGE)
                    {
                        return;
                    }

                    UnhollowerBaseLib.Il2CppStringArray strsFromObj = new UnhollowerBaseLib.Il2CppStringArray(__1.Pointer);

                    string raw = strsFromObj[0];
                    string hackedString = sensorMessageHacker.Hack(raw);
                    spamDetector.AddMessage(raw);

                    //判断是否是违规字符串
                    if (raw != hackedString)
                    {

                        StringBuilder sb = new StringBuilder();
                        sb.AppendLine("检测到违规词，正在修改...");
                        sb.AppendLine("原始消息：" + raw);
                        sb.AppendLine("和谐后：" + hackedString);
#if Developer
                        MelonLogger.Msg(System.ConsoleColor.Green, sb.ToString());
#endif
                        //修改字符串
                        {
#if Developer
                            MelonLogger.Msg(System.ConsoleColor.Green, "开发者免疫违规词消息和谐");
#endif
                            strsFromObj[0] = hackedString;
                            __1 = new Il2CppSystem.Object(strsFromObj.Pointer);

                        }

                        return;
                    }
                    else
                    {
                        //TODO:误伤
                        return;

                        //判断是否是垃圾消息
                        if (IsSpamming)
                        {
                            StringBuilder sb = new StringBuilder();
                            sb.AppendLine("检测到垃圾消息，正在修改...");
                            sb.AppendLine("原始消息：" + raw);
                            sb.AppendLine("和谐后：" + spammingReplacement);

#if Developer
                            MelonLogger.Msg(System.ConsoleColor.Green, sb.ToString());
#endif

                            //修改字符串
                            {
#if Developer
                                MelonLogger.Msg(System.ConsoleColor.Green, "开发者免疫垃圾消息和谐");
#else
                                strsFromObj[0] = spammingReplacement;
                                __1 = new Il2CppSystem.Object(strsFromObj.Pointer);
#endif
                            }

                            sensorMessageHacker.hackedStrings[spammingReplacement] = raw;
                        }
                    }
                }
                catch (System.Exception ex)
                {
                    MelonLogger.Warning("错误消息");
                }
            }

        }

        //修改本地显示，服务器回传的消息
        [HarmonyPatch(typeof(MessagePrefabHandler), nameof(MessagePrefabHandler.Initialize))]
        public class MessagePrefabHandler_
        {
            static void Prefix(Handlers.PrefabAttachedHandlers.MessagePrefabHandler __instance)
            {
                try
                {
                    string possibleHackedString = __instance.message;

                    //还原被篡改过的群号
                    __instance.message = sensorMessageHacker.Unhack(possibleHackedString);

                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine("--------------------");
                    sb.AppendLine("void Handlers.PrefabAttachedHandlers.MessagePrefabHandler::Initialize()");
                    sb.Append("- __instance: ").AppendLine(__instance.ToString());
                    if (possibleHackedString != __instance.message)
                    {
                        sb.AppendLine("真实文本：" + possibleHackedString);
                        sb.AppendLine("本地修改为：" + __instance.message);
                    }
#if Developer
                    MelonLogger.Msg(sb.ToString());
#endif
                }
                catch (System.Exception ex)
                {
                    MelonLogger.Warning($"Exception in patch of void Handlers.PrefabAttachedHandlers.MessagePrefabHandler::Initialize():\n{ex}");
                }
            }

        }
    }
}
