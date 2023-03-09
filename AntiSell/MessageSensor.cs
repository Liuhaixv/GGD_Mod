using Handlers.PrefabAttachedHandlers;
using HarmonyLib;
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
        public static SensorMessageHacker sensorMessageHacker = new SensorMessageHacker(@"\d{6,}", "277392777");

        //检测垃圾消息
        public static SpamDetector spamDetector = new SpamDetector(3,12,  0.7);

        //自动将本地发送到服务器的消息通过正则匹配记录下，然后在收到服务器回调创建消息对象时篡改显示，实现本地无法知道消息被篡改的效果
        public class SensorMessageHacker
        {
            // 要替换的文本
            // 6位以上数字
            private string ruleRegex = null;

            // 要被替换成的文本
            private string replaceWith = null;

            //忽略字数少于这个值的文字
            private int ignoreMessagesLessThan = 99999;

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
                if(raw != hackedString)
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
        [HarmonyPatch(typeof(ChatManager),nameof(ChatManager.SendChatMessageEvent))]
        public class SendChatMessageEvent_
        {
            static void Prefix(Managers.ConnectionManagers.ChatManager __instance,ref string __0)
            {
                try
                {
                    spamDetector.AddMessage(__0);
                    string raw = __0;
                    string hackedString = sensorMessageHacker.Hack(__0);

                    //判断是否是违规字符串
                    if(raw != hackedString)
                    {
#if Developer
                        StringBuilder sb = new StringBuilder();
                        sb.AppendLine("检测到违规词，正在修改...");
                        sb.AppendLine("原始消息：" + raw);
                        sb.AppendLine("和谐后：" + hackedString);
                        MelonLogger.Msg(System.ConsoleColor.Green, sb.ToString());
#endif
                        __0 = hackedString;
                        return;
                    } else
                    {
                        //判断是否是垃圾消息
                        if(IsSpamming)
                        {
                            StringBuilder sb = new StringBuilder();
                            sb.AppendLine("检测到垃圾消息，正在修改...");
                            sb.AppendLine("原始消息：" + raw);
                            sb.AppendLine("和谐后：" + spammingReplacement);
                            MelonLogger.Msg(System.ConsoleColor.Green, sb.ToString());
                            __0 = spammingReplacement;
                            sensorMessageHacker.hackedStrings[spammingReplacement] = raw;
                        }
                    }
                }
                catch (System.Exception ex)
                {
                   MelonLogger.Warning($"Exception in patch of void Managers.ConnectionManagers.ChatManager::SendChatMessageEvent(string BGGNFHNIAOL):\n{ex}");
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
                    if(possibleHackedString != __instance.message)
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
