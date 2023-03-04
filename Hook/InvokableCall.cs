#if Developer
using HarmonyLib;
using Il2CppSystem.Reflection;
using MelonLoader;
using System.Text;
using UnhollowerBaseLib;
using UnityEngine.Events;

namespace GGD_Hack.Hook
{
    public class InvokableCall_
    {
        //string methodName = unityEvent.GetPersistentMethodName(0);
        //
        [HarmonyPatch(typeof(InvokableCall), nameof(InvokableCall.Invoke), new System.Type[] { })]
        public class Invoke_
        {

            static void Prefix(UnityEngine.Events.InvokableCall __instance)
            {

                UnityAction @delegate = __instance.Delegate;
                MethodInfo methodInfo = @delegate.Method;
                string methodName = methodInfo.Name;
                string namespaceName = methodInfo.DeclaringType.Namespace;
                string className = methodInfo.DeclaringType.Name;
                try
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine("--------------------");
                    sb.AppendLine("void UnityEngine.Events.InvokableCall::Invoke()");
                    sb.Append("- 方法名:   ").AppendLine(@delegate.Method.Name);
                    sb.Append("- 完整方法: ").AppendLine(string.Format("{0}.{1}.{2}",
                                                            namespaceName,
                                                            className,
                                                            methodName));

                    MelonLogger.Msg(sb.ToString());
                }
                catch (System.Exception ex)
                {
                    MelonLogger.Msg($"Exception in patch of void UnityEngine.Events.InvokableCall::Invoke():\n{ex}");
                }
            }

        }

    }
}
#endif