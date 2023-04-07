using MelonLoader;
using UnhollowerRuntimeLib;
using UnityEngine;
using HarmonyLib;

using IntPtr = System.IntPtr;
using Handlers.GameHandlers.PlayerHandlers;
using Handlers.GameHandlers.TaskHandlers;
using UnityEngine.UI;

namespace GGD_Hack.Features
{
    [RegisterTypeInIl2Cpp]
    public class OneTapCompleteTask : MonoBehaviour
    {
        public static OneTapCompleteTask Instance;
        public static MelonPreferences_Entry<bool> Enabled = MelonPreferences.CreateEntry<bool>("GGDH", "Enable_" + nameof(OneTapCompleteTask), true);

        public OneTapCompleteTask(IntPtr ptr) : base(ptr)
        {
            IngameSettings.AddIngameSettingsEntry(
                               new IngameSettings.IngameSettingsEntry()
                               {
                                   entry = Enabled,
                                   name_cn = "任务列表秒任务",
                                   name_eng = "Tasklist onetap completing task"
                               }
                                          );
        }

        public OneTapCompleteTask() : base(ClassInjector.DerivedConstructorPointer<OneTapCompleteTask>()) => ClassInjector.DerivedConstructorBody(this);
        public static void Init()
        {
            GameObject ML_Manager = GameObject.Find("ML_Manager");
            if (ML_Manager == null)
            {
                ML_Manager = new GameObject("ML_Manager");
                DontDestroyOnLoad(ML_Manager);
            }

            if (ML_Manager.GetComponent<OneTapCompleteTask>() == null)
            {
                Instance = ML_Manager.AddComponent<OneTapCompleteTask>();
            }
        }


        private static void CompleteTask(TaskPrefabHandler instance)
        {
            PluginEventsManager.Precursor(true);

            Handlers.CommonHandlers.SoundHandler.Instance?.PlayTaskCompleteSFX();
            PluginEventsManager.Complete_Task(LocalPlayer.Instance.Player.userId, instance.task.taskId);

            PluginEventsManager.Precursor(false);

            MelonLogger.Msg(System.ConsoleColor.Green, "已秒任务: " + instance.task.taskDisplayName);
#if Developer
            MelonLogger.Msg(System.ConsoleColor.Green, "hasToBeAssigned:", instance.task.hasToBeAssigned);
            MelonLogger.Msg(System.ConsoleColor.Green, "isTaskShared:", instance.task.isTaskShared);
#endif
            GameObject.Destroy(instance.gameObject);
        }

        [HarmonyPatch(typeof(TaskPrefabHandler), nameof(TaskPrefabHandler.Update))]
        class TaskPrefabHandler_TargetTask
        {
            static void Postfix(TaskPrefabHandler __instance)
            {
                if (!Enabled.Value) return;

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
                                CompleteTask(__instance);
                            })
                            );
                    }
                }
            }
        }
    }
}