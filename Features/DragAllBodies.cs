
using UnityEngine;

namespace GGD_Hack.Features
{
    //拖拽所有尸体
    public class DragAllBodies
    {
        //1.获取一具尸体的BodyHandler
        //2.获取LocalPlayer的PlayerController
        //3.调用函数：bodyHandler.StartDragginBody(playerController);

        public static void MoveBodyTo(Handlers.GameHandlers.BodyHandler bodyHandler, Vector3 targetPosition, bool disableBody = false)
        {

            //开始拖拽尸体
            //本地
            //bodyHandler.StartDraggingBody(LocalPlayer.Instance.Player);
            //bodyHandler.StopDraggingBody(LocalPlayer.Instance.transform.position, bodyHandler.bodyUserId, true);

            //发送网络事件
            //开始拖拽尸体
            Managers.MainManager.Instance.pluginEventsManager.FCDELIMBDAE(bodyHandler);

            //放下尸体

            bodyHandler.gameObject.active = false;

            //防止尸体被瞬移回到原处
            bodyHandler.isBeingDragged = true;
            bodyHandler.gameObject.transform.position = targetPosition;

            //放下尸体的事件
            Managers.MainManager.Instance.pluginEventsManager.ACNEFCPPDLP(bodyHandler);

            if (!disableBody)
                bodyHandler.gameObject.active = true;
        }

        //将所有尸体移动到某处
        public static void MoveAllBodiesTo(Vector3 targetPosition, bool disableBody = false)
        {

            //遍历所有玩家
            foreach (var entry in Handlers.GameHandlers.PlayerHandlers.PlayerController.playersList)
            {
                string userId = entry.Key;
                Handlers.GameHandlers.PlayerHandlers.PlayerController playerController = entry.Value;

                //跳过无效玩家
                if (userId == null || userId == "" || playerController == null || playerController.isLocal)
                {
                    continue;
                }

                //玩家未死亡
                if (playerController.timeOfDeath == 0)
                {
                    continue;
                }

                Handlers.GameHandlers.BodyHandler bodyHandler = Managers.MainManager.Instance.gameManager.BodyFromUserId(playerController.userId);

                MoveBodyTo(bodyHandler, targetPosition, disableBody);
            }
        }
    }
}
