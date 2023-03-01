using HarmonyLib;
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
            static void Postfix(UnityEngine.Events.InvokableCall __instance)
            {

                UnityAction @delegate = __instance.Delegate;

                try
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine("--------------------");
                    sb.AppendLine("void UnityEngine.Events.InvokableCall::Invoke()");
                    sb.Append("- __instance: ").AppendLine(__instance.ToString());
                    sb.Append("- @delegate: ").AppendLine(@delegate.Method.Name);                

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
