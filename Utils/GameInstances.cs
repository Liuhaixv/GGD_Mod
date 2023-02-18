using UnityEngine;

namespace Utils
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
    }
}
