using MelonLoader;
using UnhollowerRuntimeLib;
using UnityEngine;
using HarmonyLib;

using IntPtr = System.IntPtr;
using Handlers.GameHandlers.PlayerHandlers;
using Handlers.LobbyHandlers;
using Il2CppSystem.Collections.Generic;
using Handlers.GameHandlers.TaskHandlers;
using Objects;
using Managers;
using GGD_Hack.Events;

namespace GGD_Hack.Features
{
    //[09:47:19.527]PRECURSOR True
    //[09:47:22.806]COMPLETE_TASK System.String[]
    //[09:47:24.306]PRECURSOR False
    [RegisterTypeInIl2Cpp]
    public class AutoTasks : MonoBehaviour
    {
        public enum TasksState
        {
            //初始状态，判断是否有需要完成的任务
            Idle,
            //开始初始化任务
            Prepare,
            //等待x秒
            Doing,
            //任务完成后发送任务完成事件
            Finish,
            //通知服务器结束
            Ending,
            //完成任务后再次开始做任务的冷却时间
            Cooldowning
        }

        public static AutoTasks Instance;
        public static MelonPreferences_Entry<bool> Enabled = MelonPreferences.CreateEntry<bool>("GGDH", "Enable_" + nameof(AutoTasks), false);


        public static MelonPreferences_Entry<float> beforePrecursorTrueInterval = MelonPreferences.CreateEntry<float>("GGDH", nameof(AutoTasks) + "_" + nameof(beforePrecursorTrueInterval), 2.0f);
        public static MelonPreferences_Entry<float> taskInterval = MelonPreferences.CreateEntry<float>("GGDH", nameof(AutoTasks) + "_" + nameof(taskInterval), 10.0f);
        public static MelonPreferences_Entry<float> cooldownInterval = MelonPreferences.CreateEntry<float>("GGDH", nameof(AutoTasks) + "_" + nameof(cooldownInterval), 10.0f);
        public const float precursorAfterCompletingTaskInterval = 0.5f;
        public const float doTaskAfterGameStartedInterval = 6.0f;

        private TasksState state = TasksState.Idle;
        private static GameTask currentTask = null;

        private float beforePrecursorTrueTime = -1f;
        private float taskTime = -1f;
        private float cooldownWaitingTime = -1f;
        private float precursorAfterCompletingTaskWaitingTime = -1f;
        private float doTaskAfterGameStartedIntervalWaitingTime = -1f;

        private bool tpToTaskPosition = true;

        private static List<GameTask> tasksToFinish = new List<GameTask>();
        public AutoTasks(IntPtr ptr) : base(ptr)
        {
            IngameSettings.AddIngameSettingsEntry(
                               new IngameSettings.IngameSettingsEntry()
                               {
                                   entry = Enabled,
                                   name_cn = "瞬移自动任务(测试中)",
                                   name_eng = "Auto Tasks with TP(Testing)"
                               }
                                          );
        }

        public AutoTasks() : base(ClassInjector.DerivedConstructorPointer<AutoTasks>()) => ClassInjector.DerivedConstructorBody(this);
        public static void Init()
        {
            GameObject ML_Manager = GameObject.Find("ML_Manager");
            if (ML_Manager == null)
            {
                ML_Manager = new GameObject("ML_Manager");
                DontDestroyOnLoad(ML_Manager);
            }

            if (ML_Manager.GetComponent<AutoTasks>() == null)
            {
                Instance = ML_Manager.AddComponent<AutoTasks>();
            }
        }

        private void Update()
        {
            if (!Enabled.Value)
            {
                return;
            }

            if (
                //本地玩家未初始化？
                (LocalPlayer.Instance?.Player == null)
               //游戏未开始？
               || (!LobbySceneHandler.Instance?.gameStarted ?? true)
             )
            {
                tasksToFinish.Clear();
                return;
            }

            switch (state)
            {
                case TasksState.Idle:
                    currentTask = null;

                    //判断是否超过游戏开始时间+延迟
                    if(Time.time <= doTaskAfterGameStartedIntervalWaitingTime)
                    {
                        break;
                    }

                    //获取任务
                    {
                        /*
                        if (tasksToFinish.Count > 0)
                        {
                            //从tasksToFinish中获取第一个任务并移除，然后设置currentTask
                            currentTask = tasksToFinish[0];
                            tasksToFinish.RemoveAt(0);

                        }
                        else
                        {
#if Developer
                            MelonLogger.Error("当前没有任务需要完成");
#endif
                        }*/

                        GameObject tasksList = LobbySceneHandler.Instance.tasksListHandler.tasksList;

                        //获取所有子对象
                        for(int i = 0; i < tasksList.transform.childCount; i++)
                        {
                            Transform taskTransform = tasksList.transform.GetChild(i);
                            TaskPrefabHandler taskPrefabHandler = taskTransform.gameObject.GetComponent<TaskPrefabHandler>();
                            GameTask task = taskPrefabHandler?.task;

                            if(task == null)
                            {
                                break;
                            }

                            //无可交互点，可能是鹈鹕时刻或者猎鹰时刻
                            if(task?.taskObject?.interactable == null)
                            {
                                continue;
                            }

                            currentTask = task;
                            break;
                        }

                        if(currentTask != null)
                        {
                            MelonLogger.Msg(System.ConsoleColor.Green, "已获取到可用任务，即将开始做任务:{0}", currentTask.taskDisplayName);
                            beforePrecursorTrueTime = Time.time + beforePrecursorTrueInterval.Value;
                            state = TasksState.Prepare;
                        } else
                        {
#if Developer
                            MelonLogger.Error("当前没有任务需要完成");
#endif
                        }
                    }

                    break;
                case TasksState.Prepare:
                    PrepareToDoTask();
                    break;
                case TasksState.Doing:
                    DoingTask();
                    break;
                case TasksState.Finish:
                    FinishTask();
                    break;
                case TasksState.Ending:
                    EndTask();
                    break;
                case TasksState.Cooldowning:
                    Cooldown();
                    break;
            }
        }

        private void PrepareToDoTask()
        { 
            //瞬移到任务点
            if (tpToTaskPosition)
            {

                Vector3 taskPosition = currentTask.taskObject.interactable.gameObject.transform.position;
                LocalPlayer.Instance.gameObject.transform.position = taskPosition;
                LocalPlayer.Instance.disableMovement = true;
            }
            else
            {
                LocalPlayer.Instance.disableMovement = false;
            }

            if (Time.time > beforePrecursorTrueTime)
            {
                //方案一:只发送事件
                //PluginEventsManager.Precursor(true);
                //方案二:模拟打开面板
                //currentTask.taskPanel.OpenPanel();
                //方案三:调用可交互物的点击事件
                currentTask.taskObject.interactable.onClick.Invoke();

                state = TasksState.Doing;
                taskTime = Time.time + taskInterval.Value;
                MelonLogger.Msg(System.ConsoleColor.Green, "已经通知服务器正在做任务，{0}秒后任务将完成", taskInterval.Value);
            }
        }

        private void DoingTask()
        {
            //等待几秒后再发送任务完成
            if (Time.time > taskTime)
            {
                state = TasksState.Finish;
                MelonLogger.Msg(System.ConsoleColor.Green, "任务过程已模拟完毕，即将完成任务");
            }
        }

        private void FinishTask()
        {
            //检查是否正在投票
            if ((byte)MainManager.Instance.gameManager.gameState == (byte)GameData.GameState.Voting)
            {
                tasksToFinish.Add(currentTask);
                MelonLogger.Msg(System.ConsoleColor.Green, "检测到当前正在投票，任务回滚中");

                PluginEventsManager.Precursor(false);
                state = TasksState.Idle;
                return;
            }

            //摧毁任务的面板对象
            //Destroy(currentTask.taskUIInList.gameObject);

            //任务完成通知服务器
            {
                //方案1：PluginEventsManager.Complete_Task(LocalPlayer.Instance.Player.userId, currentTask.taskId);
                //方案2：currentTask.taskCompletionCallback.Invoke();
                //方案3
                //80 7B 21 00 75 64 48 8B 05 ?? ?? ?? ?? 83 B8 E0 00 00 00 00 75 0F 48 8B C8 E8 ?? ?? ?? ?? 48 8B 05 ?? ?? ?? ?? 48 8B 80 B8 00 00 00 48 8B 08 48 85 C9 74 3C 48 8B 49 40 48 85 C9 74 33 48 8B 53 18 45 33 C9 48 C7 44 24 30 00 00 00 00 45 33 C0 C6 44 24 28 01 C6 44 24 20 00 E8 ?? ?? ?? ?? C6 43 21 01
                //currentTask.taskPanel.LMCKOLKLKCE = false;
                currentTask.taskPanel.CompleteTask();

                //播放任务完成的声音
                //Handlers.CommonHandlers.SoundHandler.Instance?.PlayTaskCompleteSFX();
            }

            //设置在任务完成事件发送后一段时间再通知precursor
            precursorAfterCompletingTaskWaitingTime = Time.time + precursorAfterCompletingTaskInterval;
            MelonLogger.Msg(System.ConsoleColor.Green, "任务已完成，即将通知服务器");
            state = TasksState.Ending;
        }

        private void EndTask()
        {
            if (Time.time > precursorAfterCompletingTaskWaitingTime)
            {
                //方案一：只发送事件
                //PluginEventsManager.Precursor(false);
                //方案二：关闭面板
                currentTask.taskPanel.canClosePanel = true;
                currentTask.taskPanel.ClosePanel();

                //设置冷却时间
                cooldownWaitingTime = Time.time + cooldownInterval.Value;
                MelonLogger.Msg(System.ConsoleColor.Green, "任务已完成，即将进入冷却期等待{0}秒...", cooldownInterval);
                state = TasksState.Cooldowning;
            }
        }
        private void Cooldown()
        {
            if (Time.time > cooldownWaitingTime)
            {
                MelonLogger.Msg(System.ConsoleColor.Green, "冷却期已完成，即将进入Idle状态");
                state = TasksState.Idle;
            }
        }

        //[HarmonyPatch(typeof(TasksHandler), nameof(TasksHandler.AssignTask), typeof(GameTask), typeof(bool))]
        class TasksHandler_AssignTask
        {
            static void Postfix(GameTask __0, bool __1)
            {
                tasksToFinish.Add(__0);
                MelonLogger.Msg(System.ConsoleColor.Green, "已添加任务:{0} 到待完成任务列表，当前待做任务数量:{1}", __0.taskDisplayName, tasksToFinish.Count);
            }
        }

        [HarmonyPatch(typeof(InGameEvents),nameof(InGameEvents.Start_Game))]
        class InGameEvents_StartGame
        {
            static void Postfix()
            {
                Instance.doTaskAfterGameStartedIntervalWaitingTime = Time.time + doTaskAfterGameStartedInterval;
            }
        }
    }
}