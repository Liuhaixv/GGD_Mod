using HarmonyLib;
using Managers;
using MelonLoader;
using System.Collections.Generic;
using TMPro;
using UnhollowerRuntimeLib;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

using IntPtr = System.IntPtr;
using RoomInfo = Photon.Realtime.RoomInfo;

namespace GGD_Hack.Features
{
    //Mode Select - Safe zone H
    [RegisterTypeInIl2Cpp]
    public class RandomJoinRoom : MonoBehaviour
    {
        public static List<Photon.Realtime.RoomInfo> roomInfos = null;

        public static GameObject randomJoin = null;
        public static GameObject findButton = null;

        public static RandomJoinRoom Instance;
        public RandomJoinRoom(IntPtr ptr) : base(ptr) { }

        // Optional, only used in case you want to instantiate this class in the mono-side
        // Don't use this on MonoBehaviours / Components!
        public RandomJoinRoom() : base(ClassInjector.DerivedConstructorPointer<RandomJoinRoom>()) => ClassInjector.DerivedConstructorBody(this);
        public static void Init()
        {
            GameObject ML_Manager = GameObject.Find("ML_Manager");
            if (ML_Manager == null)
            {
                ML_Manager = new GameObject("ML_Manager");
                DontDestroyOnLoad(ML_Manager);
            }

            if (ML_Manager.GetComponent<RandomJoinRoom>() == null)
            {
                Instance = ML_Manager.AddComponent<RandomJoinRoom>();
            }

            //更新房间数据
            APIs.Photon.PhotonCallbacksAPI.AddOnRoomListUpdateListener((Il2CppSystem.Action<Il2CppSystem.Collections.Generic.List<RoomInfo>>)UpdateRoomsInfo, true);
        }

        //调整按钮的位置，向上挪动
        //主持游戏、查找游戏
        public static bool AdujstButtonsPositions()
        {
            //已经添加过
            if (randomJoin != null)
            {
                return true;
            }

            // 查找Mode Select - Safe zone H
            GameObject safeZone = GameObject.Find("Mode Select - Safe zone H");

            // 查找Goose
            Transform goose = safeZone.transform.Find("Goose");

            // 查找Host和Find
            Transform host = goose.Find("Host");
            Transform find = goose.Find("Find");
            Transform join = goose.Find("Join");
            Transform host_1 = goose.Find("Host (1)");
            Transform find_1 = goose.Find("Find (1)");

            RandomJoinRoom.findButton = find.gameObject;

            // 计算间隔
            float gap = join.localPosition.y - find.localPosition.y;

            //移动Host和Find
            host.localPosition -= new Vector3(0, gap, 0);
            find.localPosition -= new Vector3(0, gap, 0);

            //移动Host_1和Find_1
            host_1.localPosition -= new Vector3(0, gap, 0);
            find_1.localPosition -= new Vector3(0, gap, 0);

            // 克隆Find按钮
            randomJoin = GameObject.Instantiate(find.gameObject, goose);

            randomJoin.name = "JoinRandomRoom";
            randomJoin.transform.SetSiblingIndex(find.GetSiblingIndex() + 1);

            //添加按钮监听器
            GameObject.Destroy(randomJoin.GetComponent<Button>());

            // 修改Find(clone)的TextMeshProUGUI组件中的文本
            TextMeshProUGUI findCloneTMP = randomJoin.transform.Find("Font").GetComponent<TextMeshProUGUI>();
            findCloneTMP.text = "随机加入";

            return true;
        }

        public static void DoRandomJoinRoom()
        {
            try
            {
                //1.获取所有房间号
                if (roomInfos == null || roomInfos.Count == 0)
                {
                    MelonLogger.Warning("当前无可用房间可加入！");
                    return;
                }

                List<RoomInfo> filterdRoomInfos = new List<RoomInfo>();

                //筛选2/3人数以内的房间
                foreach (var room in roomInfos)
                {
                    int playerCount = room.PlayerCount;
                    int maxPlayers = room.MaxPlayers;

                    if (((playerCount / (float)(maxPlayers)) < (2.0 / 3.0)))
                    {
                        filterdRoomInfos.Add(room);
                    }
                }

                if (filterdRoomInfos.Count == 0)
                {
                    MelonLogger.Warning("筛选后无可用房间可加入！");
                    return;
                }


                int randomIndex = Random.RandomRangeInt(0, filterdRoomInfos.Count);
                RoomInfo roomInfo = filterdRoomInfos[randomIndex];

                MelonLogger.Msg("随机房间:" + roomInfo.ToStringFull());

                //房间号
                string roomName = roomInfo.Name;
                string nickname = PlayerPrefs.GetString("nick name");

                //设置要加入的房间号
                //Parameter 0 'EILJAJOKPMD': PWCVTBS
                //-Parameter 1 'LMEPJAHBAOD': nickname
                //-Parameter 2 'OPGMBEOPKBL':
                //-Parameter 3 'FOBJLMIAMJA':
                //-Parameter 4 'FNFFFOIGHLM': True
                MainManager.Instance.roomManager.JoinRoom(roomName, nickname, "", "", true);
            }
            catch (System.Exception e)
            {
                MelonLogger.Warning("DoRandomJoinRoom失败: " + e.ToString());
            }
        }

        private static void UpdateRoomsInfo(Il2CppSystem.Collections.Generic.List<RoomInfo> roomInfos)
        {
            List<Photon.Realtime.RoomInfo> newList = new List<Photon.Realtime.RoomInfo>();

            foreach (var room in roomInfos)
            {
                //MelonLogger.Msg(room.ToStringFull());
                newList.Add(room);
            }

            RandomJoinRoom.roomInfos = newList;
        }

        private void Update()
        {

            if (randomJoin != null && randomJoin.GetComponent<Button>() == null)
            {

                Button button = randomJoin.AddComponent<Button>();

                button.onClick.AddListener(new System.Action(() =>
                {
                    DoRandomJoinRoom();
                }));
            }
        }
    }

    [HarmonyPatch(typeof(SceneManager), nameof(SceneManager.Internal_SceneLoaded))]
    class SceneManager_
    {
        private static void Postfix(Scene scene)
        {
            if (scene.name == "MenuScene")
            {
                MelonLogger.Msg("场景MenuScene" + "已加载");
                bool success = RandomJoinRoom.AdujstButtonsPositions();

                if (success)
                    MelonLogger.Msg("已成功添加随机加入房间的按钮!");
                else
                    MelonLogger.Warning("添加随机加入房间的按钮失败!");
            }
        }
    }

    /*
    //APIs.Photon.PhotonCallbacksAPI.OnRoomListUpdate
    [HarmonyPatch(typeof(PhotonCallbacksAPI), nameof(PhotonCallbacksAPI.OnRoomListUpdate))]
    class OnRoomListUpdate_
    {
        static void Prefix(List<Photon.Realtime.RoomInfo> __0)
        {
            List<Photon.Realtime.RoomInfo> newList = new List<Photon.Realtime.RoomInfo>();

            foreach (var room in __0)
            {
                MelonLogger.Msg(room.ToString());
                newList.Add(room);
            }

            RandomJoinRoom.roomInfos = newList;
        }
    }
    */
}
