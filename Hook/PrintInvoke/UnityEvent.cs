#if Developer
using HarmonyLib;
using MelonLoader;
using System.Text;
using UnityEngine.Events;

namespace GGD_Hack.Hook
{
    public class UnityEvent_
    {
        //[HarmonyPatch(typeof(UnityEvent), nameof(UnityEvent.Invoke))]
        public class Invoke_
        {
            static void Prefix(UnityEngine.Events.UnityEvent __instance)
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
                        int persistentCallsCount = 0;
                        try
                        {
                            persistentCallsCount = __instance.m_Calls.Count;
                        }
                        catch (System.Exception e)
                        {
                            MelonLogger.Warning("获取UnityEvent的m_PersistentCalls方法数量失败:" + e.Message);
                        }

                        sb.Append("- m_PersistentCalls count: ").AppendLine(persistentCallsCount.ToString());

                        PersistentCallGroup m_PersistentCalls = __instance.m_PersistentCalls;
                        for (int i = 0; i < persistentCallsCount; i++)
                        {
                            PersistentCall persistentCall = m_PersistentCalls.m_Calls[i];

                            sb.Append(string.Format("- method name: {0}:{1}", persistentCall.m_TargetAssemblyTypeName, persistentCall.m_MethodName)).AppendLine();
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