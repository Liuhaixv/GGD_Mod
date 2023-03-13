using Handlers.MenuSceneHandlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GGD_Hack.Utils
{
    public class JoinRoom
    {
        public static void Join(string roomCode)
        {
            //修改加入游戏下方的输入框内容
            TMPro.TMP_InputField roomIdIF = MenuSceneHandler.Instance.roomIdIF;
            roomIdIF.text = roomCode;
            //调用按钮点击事件
            MenuSceneHandler.Instance.JoinRoom();
        }

        public static void Refresh()
        {
            MenuSceneHandler.Instance.RefreshRooms();
        }
    }
}
