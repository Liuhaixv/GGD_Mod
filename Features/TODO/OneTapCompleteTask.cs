using Handlers.GameHandlers.TaskHandlers;
using HarmonyLib;
using MelonLoader;

namespace GGD_Hack.Features.TODO
{
    public class OneTapCompleteTask
    {
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
    }
}
