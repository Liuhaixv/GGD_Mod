using Handlers.GameHandlers.PlayerHandlers;
using Handlers.GameHandlers.TaskHandlers;
using Handlers.LobbyHandlers;
using HarmonyLib;
using Managers;
using MelonLoader;

namespace GGD_Hack.Features.TODO
{
    public class OneTapCompleteTask
    {
        public static void DelayedCompleteTask(string taskID)
        {

        }

        //Canvas/GamePanel/TasksList/List/TaskUI Victorian(Clone)/
        //Handlers.GameHandlers.TaskHandlers.TaskPrefabHandler
        [HarmonyPatch(typeof(TaskObjectHandler),nameof(TaskObjectHandler.AssignTask))]
        class TaskObjectHandler_AssignTask
        {
            static void Postfix(Handlers.GameHandlers.TaskHandlers.TaskObjectHandler __instance, Handlers.GameHandlers.TaskHandlers.TaskPanelHandler __0)
            {
                try
                {
                    string taskID = __0.taskID;
                }
                catch (System.Exception ex)
                {
                   MelonLogger.Warning($"Exception in patch of void Handlers.GameHandlers.TaskHandlers.TaskObjectHandler::AssignTask(Handlers.GameHandlers.TaskHandlers.TaskPanelHandler PDPJBDGLOLB):\n{ex}");
                }
            }
        }

        //任务被点击
        [HarmonyPatch(typeof(TaskPrefabHandler), nameof(TaskPrefabHandler.TargetTask))]
        class TaskPrefabHandler_TargetTask
        {
            static void Postfix(TaskPrefabHandler __instance)
            {
                string taskText = __instance.baseText;
                Objects.GameTask task = __instance.task;

                PluginEventsManager.Precursor(true);

                LobbySceneHandler.Instance.tasksHandler.CompleteTask(task.taskId, false, false, true, true);
                //PluginEventsManager.Complete_Task(LocalPlayer.Instance.Player.userId, task.taskId);

                PluginEventsManager.Precursor(false);

                Handlers.CommonHandlers.SoundHandler.Instance.PlayTaskCompleteSFX();

                MelonLogger.Msg(System.ConsoleColor.Green, "已完成任务：" + taskText);
            }
        }
    }
}
