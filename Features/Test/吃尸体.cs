using Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GGD_Hack.Features.Test
{
    //TODO: 全屏吃尸体无视距离
    //[03:34:37.333] [[开发者专用版]Liuhaixv's_GGD_Hack_mod] 接收到事件: EAT
    //[03:34:37.333] [[开发者专用版]Liuhaixv's_GGD_Hack_mod] Event 19: {(Byte)245=(String[]){U99TNMzBA2U7WsBAX6DwsmtA40m2,nHL5JcF4h8g8R5chp9MWmSsHYei2}, (Byte)254=(Int32)0}
    internal class 吃尸体
    {
        public void Eat(string userId)
        {
            string userId_ = userId;
            MainManager.Instance.pluginEventsManager.DFKGKFJMMJB(userId_);
        }
    }
}
