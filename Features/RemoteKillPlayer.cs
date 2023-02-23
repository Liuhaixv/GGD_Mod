using Cinemachine;
using Handlers.GameHandlers.PlayerHandlers;
using Handlers.GameHandlers.SpecialBehaviour;
using MelonLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnhollowerRuntimeLib;
using UnityEngine;
using static MelonLoader.MelonLogger;

//1.获取目标玩家的坐标
//2.在x毫秒内持续传送到玩家附近
//3.发送Kill命令击杀玩家
//4.传送回原始位置
namespace GGD_Hack.Features
{
    /// <summary>
    /// 远程杀人
    /// </summary>
    [RegisterTypeInIl2Cpp]
    public class RemoteKillPlayer : MonoBehaviour
    {
        public static RemoteKillPlayer Instance = null;

        private PlayerController killTarget = null;//要击杀的目标
        private Vector3 localPlayerPositionBeforeTeleport = Vector3.zero;//传送前的位置

        private bool hasStartedToKill = false;//标志是否已经开始击杀任务
        private float killTimer = 0;
        private float teleportTimer = 0;
        static private float killDelay = 0.5f;//击杀延迟 ms
        static private float teleportDuration = 0.5f;//传送回到原来位置的延迟

        public RemoteKillPlayer(IntPtr ptr) : base(ptr) { }

        // Optional, only used in case you want to instantiate this class in the mono-side
        // Don't use this on MonoBehaviours / Components!
        public RemoteKillPlayer() : base(ClassInjector.DerivedConstructorPointer<RemoteKillPlayer>()) => ClassInjector.DerivedConstructorBody(this);

        public static void Init()
        {
            GameObject ML_Manager = GameObject.Find("ML_Manager");
            if (ML_Manager == null)
            {
                ML_Manager = new GameObject("ML_Manager");
                DontDestroyOnLoad(ML_Manager);
            }

            if (ML_Manager.GetComponent<RemoteKillPlayer>() == null)
            {
                Instance = ML_Manager.AddComponent<RemoteKillPlayer>();
            }
        }

        public static bool TeleportAndKill(string userId, string _killDelay = null)
        {
            //当前已经有击杀任务正在执行
            if (Instance.hasStartedToKill)
            {
                MelonLogger.Warning("当前已经有击杀任务正在执行!");
                return false;
            }

            if (_killDelay == null)
            {
                return TeleportAndKill(userId);
            }

            killDelay = float.Parse(_killDelay);

            return TeleportAndKill(userId);
        }

        private static bool TeleportAndKill(string userId)
        {
            //开始击杀任务
            Instance.hasStartedToKill = true;

            //判断目标玩家是否存在
            var playersList = PlayerController.playersList;
            if (!playersList.ContainsKey(userId) || playersList[userId] == null)
            {
                Instance.hasStartedToKill = false;
                return false;
            }

            //保存tp前的本地玩家位置
            Instance.localPlayerPositionBeforeTeleport = LocalPlayer.Instance.transform.position;
            Instance.killTarget = playersList[userId];

            //禁止相机移动
            Camera.main.GetComponent<CinemachineBrain>().enabled = false;

            return true;
        }
        private void Update()
        {
            //判断是否正在进行击杀任务
            if (!hasStartedToKill)
            {
                return;
            }

            //目标仍然存在
            if (killTarget != null)
            {
                //增加击杀延迟计时器时间
                killTimer += Time.deltaTime;

                //未到击杀时间
                if (killTimer < killDelay)
                {
                    //瞬移到目标身上
                    LocalPlayer.Instance.transform.position = killTarget.transform.position;
                }
                else
                {
                    //猎杀时刻
                    MelonLogger.Msg("正在击杀目标: " + killTarget.nickname);
                    try
                    {
                        if(killTarget.timeOfDeath == 0)
                        {
                            LocalPlayer.Instance.Kill(killTarget);
                        }
                    }
                    catch (Exception e)
                    {
                        MelonLogger.Warning("击杀目标" + killTarget.nickname + "失败!!");
                        MelonLogger.Warning(e.ToString());
                    }
                    MelonLogger.Msg("已击杀目标: " + killTarget.nickname);

                    //重置计时器和击杀目标
                    killTimer = 0;
                    killTarget = null;
                }
            }
            else
            {
                //目标已消失，开始传送回到原始位置


                //持续传送的时间尚未结束
                if (teleportTimer < teleportDuration)
                {
                    //移动回到原始坐标
                    LocalPlayer.Instance.transform.position = localPlayerPositionBeforeTeleport;

                    //恢复相机移动
                    Camera.main.GetComponent<CinemachineBrain>().enabled = true;

                    teleportTimer += Time.deltaTime;
                }
                else
                {
                    //传送回原始位置的时间已结束


                    //设置已完成击杀任务
                    hasStartedToKill = false;
                    teleportTimer = 0;
                    MelonLogger.Msg("已完成击杀任务!");
                }
            }
        }
    }
}
