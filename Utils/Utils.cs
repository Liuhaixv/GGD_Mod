using System;
using Handlers.GameHandlers.PlayerHandlers;
using MelonLoader;
using UnityEngine;

namespace GGD_Hack.Utils
{
    public class Utils
    {
        //修改颜色
        public static void ChangeColor(int color)
        {
            Managers.MainManager.Instance.playerPropertiesManager.ChangeColor(color);
        }
        //发送聊天消息
        public static void SendTextMessage(string text)
        {
            try
            {
                //IntPtr intPtr = UnhollowerBaseLib.Il2CppStringArray.AllocateArray(2);
                //UnhollowerBaseLib.Il2CppStringArray strs = new UnhollowerBaseLib.Il2CppStringArray(intPtr);
                UnhollowerBaseLib.Il2CppStringArray strs = new UnhollowerBaseLib.Il2CppStringArray(2);

                strs[0] = text;
                strs[1] = "false";

                Il2CppSystem.Object obj = new Il2CppSystem.Object(strs.Pointer);

                APIs.Photon.PhotonEventAPI.SendEventToPlugin(66, obj, false);
            }
            catch (System.Exception ex)
            {
                MelonLogger.Error("ERROR!!: SendTextMessage" + ex.Message);
            }
        }

        //打印所有持久化设置
        public static void PrintAllPrefs()
        {
            MelonPreferences_Category melonPreferences_Category = MelonPreferences.GetCategory("GGDH");
            System.Collections.Generic.List<MelonPreferences_Entry> entries = melonPreferences_Category.Entries;
            foreach (var entry in entries)
            {
                MelonLogger.Msg("键名：{0} 默认值：{1} 当前值：{2}",
                    entry.DisplayName,
                    entry.GetDefaultValueAsString(),
                    entry.GetEditedValueAsString());
            }
        }

        //获取LocalPlayer
        public static Handlers.GameHandlers.PlayerHandlers.LocalPlayer GetLocalPlayer()
        {
            return LocalPlayer.Instance;
        }

        /// <summary>
        /// 无论是否激活
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static GameObject FindGameObjectByPath(string path)
        {
            // 分割路径字符串
            string[] pathElements = path.Split('/');

            // 从根开始遍历场景层次结构
            Transform currentTransform = null;
            for (int i = 0; i < pathElements.Length; i++)
            {
                // 获取当前层级的Transform
                if (currentTransform == null)
                {
                    // 如果是根节点
                    currentTransform = GameObject.Find(pathElements[i]).transform;
                }
                else
                {
                    // 如果不是根节点
                    currentTransform = currentTransform.Find(pathElements[i]);
                }

                // 如果找不到当前层级的Transform，返回 null
                if (currentTransform == null)
                {
                    return null;
                }
            }

            // 返回找到的 GameObject
            return currentTransform.gameObject;
        }

        public static bool IsChineseSystem()
        {
            SystemLanguage systemLanguage = UnityEngine.Application.systemLanguage;
#if Developer
            //MelonLogger.Msg("当前系统语言:" + systemLanguage);
#endif
            if (systemLanguage == SystemLanguage.Chinese ||
                systemLanguage == SystemLanguage.ChineseSimplified ||
                systemLanguage == SystemLanguage.ChineseTraditional)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
