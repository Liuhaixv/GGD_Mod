using HarmonyLib;
using MelonLoader;
using System.Text;
using UnityEngine.Events;

namespace GGD_Hack.Hook
{
    public class UnityEvent_
    {
        [HarmonyPatch(typeof(UnityEvent), nameof(UnityEvent.Invoke) )]
        public class Invoke_
        {
            static void Postfix(UnityEngine.Events.UnityEvent __instance)
            {
                try
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine("--------------------");
                    sb.AppendLine("void UnityEngine.Events.UnityEvent::Invoke()");
                    sb.Append("- __instance: ").AppendLine(__instance.ToString());
                    if(__instance != null)
                    {
                        int eventCount = __instance.GetPersistentEventCount();
                        sb.Append("- total count: ").AppendLine(eventCount.ToString());
                        for (int i = 0; i < eventCount; i++)
                        {
                            //string methodName = __instance.GetPersistentMethodName(i);
                            //sb.Append("- method name: ").AppendLine(methodName.ToString());
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
