﻿using UnityEngine;

namespace GGD_Hack.Utils
{
    public class GameInstances
    {
        //获取LocalPlayer
        public static Handlers.GameHandlers.PlayerHandlers.LocalPlayer GetLocalPlayer()
        {
            //通过tag查找玩家
            GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("Player");

            GameObject player = null;
            foreach (GameObject gameObject in gameObjects)
            {
                if (gameObject.GetComponent<Handlers.GameHandlers.PlayerHandlers.LocalPlayer>() == null)
                {
                    continue;
                }
                else
                {
                    player = gameObject;
                    break;
                }
            }

            //未找到玩家实例
            if (player == null)
            {
                return null;
            }

            return player.GetComponent<Handlers.GameHandlers.PlayerHandlers.LocalPlayer>();
        }

        /// <summary>
        /// 无论是否激活
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static GameObject FindGameObjectByPath(string path)
        {
            // 分割路径字符串
            string[] pathElements = path.Split('/');

            // 从根开始遍历场景层次结构
            Transform currentTransform = null;
            for (int i = 0; i < pathElements.Length; i++)
            {
                // 获取当前层级的Transform
                if (currentTransform == null)
                {
                    // 如果是根节点
                    currentTransform = GameObject.Find(pathElements[i]).transform;
                }
                else
                {
                    // 如果不是根节点
                    currentTransform = currentTransform.Find(pathElements[i]);
                }

                // 如果找不到当前层级的Transform，返回 null
                if (currentTransform == null)
                {
                    return null;
                }
            }

            // 返回找到的 GameObject
            return currentTransform.gameObject;
        }
    }
}
