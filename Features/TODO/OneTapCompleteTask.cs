using Handlers.GameHandlers.PlayerHandlers;
using Handlers.GameHandlers.TaskHandlers;
using HarmonyLib;
using MelonLoader;
using UnityEngine.UI;

namespace GGD_Hack.Features
{
    public class OneTapCompleteTask
    {
        private static void CompleteTask(string taskId)
        {
            PluginEventsManager.Precursor(true);

            PluginEventsManager.Complete_Task(LocalPlayer.Instance.Player.userId, taskId);

            PluginEventsManager.Precursor(false);

            MelonLogger.Msg(System.ConsoleColor.Green, "已秒任务:" + taskId);
        }

        [HarmonyPatch(typeof(TaskPrefabHandler), nameof(TaskPrefabHandler.Update))]
        class TaskPrefabHandler_TargetTask
        {
            static void Postfix(TaskPrefabHandler __instance)
            {
                Button button = __instance.gameObject.transform.Find("Selector")?.GetComponent<Button>();

                if (button == null)
                {
                    return;
                }

                if (button.onClick.GetPersistentEventCount() == 1)
                {
                    UnityEngine.Events.PersistentCall persistentCall = button.onClick.m_PersistentCalls.m_Calls[0];
                    if (persistentCall.m_MethodName == "TargetTask")
                    {
                        button.onClick.RemoveAllListeners();
                        button.onClick.AddListener(
                            new System.Action(() =>
                            {
                                CompleteTask(__instance.task.taskId);
                            })
                            );
                    }
                }
            }
        }
    }
}