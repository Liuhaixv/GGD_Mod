using Handlers.GameHandlers.PlayerHandlers;
using HarmonyLib;
using MelonLoader;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace GGD_Hack.Hook
{
    public class IncreaseSkillRange
    {
    }

#if Developer

    //[HarmonyPatch(typeof(Transform), "set_position")]
    public class Transform_
    {
        static void Postfix(UnityEngine.Transform __instance, UnityEngine.Vector3 __0)
        {
            try
            {
                //判断是否是本地玩家
                if (__instance.gameObject.GetComponent<LocalPlayer>() == null)
                {
                    //不是本地玩家
                    return;
                }

                //打印函数调用栈
                StackTrace stackTrace = new StackTrace();
                string stackTraceString = stackTrace.ToString();

                MelonLogger.Msg("检测到本地玩家坐标被修改：");
                MelonLogger.Msg(stackTraceString);

            }
            catch (System.Exception ex)
            {
                MelonLogger.Warning($"Exception in patch of void UnityEngine.Transform::set_position(UnityEngine.Vector3 value):\n{ex}");
            }
        }

    }
#endif

}
