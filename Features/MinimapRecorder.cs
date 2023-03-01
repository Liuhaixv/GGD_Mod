using Handlers.GameHandlers.PlayerHandlers;
using MelonLoader;
using System.Collections.Generic;
using System.Linq;
using UnhollowerRuntimeLib;
using UnityEngine;
using UnityEngine.UI;
using IntPtr = System.IntPtr;

namespace GGD_Hack.Features
{
    /// <summary>
    /// 记录所有玩家从对局开始到对局结束的所有位置，并附带一个播放器，可以播放位置信息，可以拖拽时间轴
    /// </summary>
    [RegisterTypeInIl2Cpp]
    public class MinimapRecorder : MonoBehaviour
    {
        public static MinimapRecorder Instance = null;

        private bool isRecording = false;
        private float recordInterval = 0.1f; // 记录间隔
        private float recordTime = 0f; // 当前记录时间
        private float lastRecordTime = 0f; // 上一次更新时间
        private Dictionary<string, string> playersToRecord = new Dictionary<string, string>();//要统计位置的玩家 userId -> nickname
        private List<Dictionary<string, (string nickname, Vector3 position)>> records = new List<Dictionary<string, (string nickname, Vector3 position)>>(); // 记录所有玩家的位置信息

        public MinimapRecorder(IntPtr ptr) : base(ptr) { }

        // Optional, only used in case you want to instantiate this class in the mono-side
        // Don't use this on MonoBehaviours / Components!
        public MinimapRecorder() : base(ClassInjector.DerivedConstructorPointer<MinimapRecorder>()) => ClassInjector.DerivedConstructorBody(this);

        public static void Init()
        {
            GameObject ML_Manager = GameObject.Find("ML_Manager");
            if (ML_Manager == null)
            {
                ML_Manager = new GameObject("ML_Manager");
                DontDestroyOnLoad(ML_Manager);
            }

            if (ML_Manager.GetComponent<MinimapRecorder>() == null)
            {
                Instance = ML_Manager.AddComponent<MinimapRecorder>();
            }
        }

        public void StartRecording()
        {
            //确定要记录数据的玩家
            playersToRecord.Clear();
            foreach (var Entry in PlayerController.playersList)
            {
                if (Entry.Key == null || Entry.Value == null) continue;
                PlayerController playerController = Entry.Value;
                playersToRecord.Add(playerController.userId, playerController.nickname);
            }

            // 开始记录数据
            records.Clear();
            recordTime = 0f;
            MelonLogger.Msg("开始录制玩家位置！");
            isRecording = true;
        }

        public void StopRecording()
        {
            isRecording = false;
            // 停止记录数据
            playersToRecord.Clear();
        }

        public void RestartRecords()
        {
            MelonLogger.Msg("正在重新开始录制玩家位置");
            StartRecording();
        }

        public void RecordPositions()
        {
            // 记录所有玩家的位置信息
            Dictionary<string, (string nickname, Vector3 position)> positions = new Dictionary<string, (string nickname, Vector3 position)>();

            Il2CppSystem.Collections.Generic.Dictionary<string, PlayerController> playersList = PlayerController.playersList;
            foreach (var player in playersToRecord.ToList())
            {
                string userId = player.Key;
                string nickname = player.Value;

                //判断玩家是否存在
                if (!playersList.ContainsKey(userId) || playersList[userId] == null)
                {
                    playersToRecord.Remove(userId);
                    continue;
                }

                PlayerController playerController = playersList[userId];

                //判断玩家是否死亡
                if (playerController.timeOfDeath != 0)
                {
                    playersToRecord.Remove(userId);
                    continue;
                }

                Vector3 position = playerController.DDKEEHDDBDF;

                positions[userId] = (nickname, position);
            }
            records.Add(positions);

            // 更新时间
            recordTime += recordInterval;
        }

        private void Update()
        {
            if(!isRecording)
            {
                return;
            }

            if (Time.time - lastRecordTime >= recordInterval)
            {
                RecordPositions();
                lastRecordTime = Time.time;
            }
        }
    }
}