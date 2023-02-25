using Handlers.MenuSceneHandlers;
using HarmonyLib;
using MelonLoader;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace GGD_Hack.Features
{
    //Mode Select - Safe zone H
    public class RandomJoinRoom
    {
        public static GameObject randomJoin = null;

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

            // 计算间隔
            float gap = join.localPosition.y - find.localPosition.y;

            //移动Host和Find
            host.localPosition -= new Vector3(0, gap, 0);
            find.localPosition -= new Vector3(0, gap, 0);

            //移动Host_1和Find_1
            host_1.localPosition -= new Vector3(0, gap, 0);
            find_1.localPosition -= new Vector3(0, gap, 0);

            // 克隆Find按钮
            GameObject findClone = GameObject.Instantiate(find.gameObject, goose);
            findClone.name = "Find(clone)";
            findClone.transform.SetSiblingIndex(find.GetSiblingIndex() + 1);

            // 修改Find(clone)的显示文本
            Button findCloneButton = findClone.GetComponent<Button>();

            //修改按钮的点击事件

            findCloneButton.onClick.AddListener((UnityEngine.Events.UnityAction)DoRandomJoinRoom);

            // 修改Find(clone)的TextMeshProUGUI组件中的文本
            TextMeshProUGUI findCloneTMP = findClone.transform.Find("Font").GetComponent<TextMeshProUGUI>();
            findCloneTMP.text = "随机加入";

            randomJoin = findClone;

            return true;
        }

        private static void DoRandomJoinRoom()
        {
            MenuSceneHandler.Instance.JoinRandomRoom(0);
        }
    }

    [HarmonyPatch(typeof(SceneManager), "Internal_SceneLoaded")]
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
}
