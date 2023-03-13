#if Developer
using HarmonyLib;
using MelonLoader;
using System.Text;
using UnityEngine.Events;

namespace GGD_Hack.Hook
{
    public class UnityEvent_
    {
        //TODO:bug [HarmonyPatch(typeof(UnityEvent), nameof(UnityEvent.Invoke))]
        public class Invoke_
        {
            static void Postfix(UnityEngine.Events.UnityEvent __instance)
            {
                try
                {
                    if (__instance == null) { return; }

                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine("--------------------");
                    sb.AppendLine("void UnityEngine.Events.UnityEvent::Invoke()");
                    sb.Append("- __instance: ").AppendLine(__instance.ToString());
                    if (__instance != null)
                    {
                        int eventCount = 0;
                        try
                        {
                            eventCount = __instance.GetPersistentEventCount();
                        }catch(System.Exception e)
                        {
                            MelonLogger.Warning("获取UnityEvent的方法数量失败");
                        }

                        sb.Append("- total count: ").AppendLine(eventCount.ToString());
                        for (int i = 0; i < eventCount; i++)
                        {
                            //string methodName = __instance.GetPersistentMethodName(i);
                            //UnityEngine.Object target = __instance.GetPersistentTarget(i);
                            //sb.Append("- method name: ").AppendLine(methodName.ToString());
                            //sb.Append("- target: ").AppendLine(target.ToString());
                        }
                    }
                    MelonLogger.Msg(sb.ToString());
                }
                catch (System.Exception ex)
                {
                    MelonLogger.Warning($"Exception in patch of void UnityEngine.Events.UnityEvent::Invoke():\n{ex}");
                }
            }

        }
    }
}
#endif