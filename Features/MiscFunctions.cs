using Handlers.GameHandlers.PlayerHandlers;
using Handlers.LobbyHandlers;
using Photon.Pun;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace GGD_Hack.Features
{
    public class MiscFunctions
    {
        /// <summary>
        /// 控制连接殖民地中的飞船启动
        /// </summary>
        public static void MoveShuttle()
        {
            PluginEventsManager.Precursor(true);
            SendEventToPlugin.MOVE_SHUTTLE();
            PluginEventsManager.Precursor(false);
        }

        /// <summary>
        /// 自杀
        /// </summary>
        public static void Suicide()
        {
            //判断地图
            string roomMap = LobbySceneHandler.Instance.roomMap;
            int mapIndex = int.Parse(roomMap);

            switch (mapIndex)
            {
                //沙漠
                case 8:
                    //被木乃伊杀死
                    SendEventToPlugin.SPECIAL_KILL(7);
                    break;
                default:
                    SendEventToPlugin.SPECIAL_KILL(7);
                    break;
            }
        }

        /*2.17.01
        public static void JoinRandomRoom()
        {
            Il2CppSystem.Collections.Generic.List<string> roomsList = MFLEJLLJKBN.PLAHBPDMINK;            
        }
        */
    }
}
namespace GGD_Hack
{


    public static class SendEventToPlugin
    {
        //控制连接殖民地中的飞船启动
        public static void MOVE_SHUTTLE()
        {
            APIs.Photon.PhotonEventAPI.SendEventToPlugin(30, null, false);
        }

        public static void SPECIAL_KILL(int type)
        {
            string[] killedType = new string[1];
            killedType[0] = type.ToString();

            // 0 DieBySpace（太空中死亡）
            // 1 DieByTeleporter（通过传送门死亡）
            // 2 DieByShuttleSabotage（穿梭机破坏死亡）
            // 3 corpseSplatterPrefab（尸体喷溅效果）
            // 4 victorianCorpseSplatterPrefab（维多利亚时期尸体喷溅效果）
            // 5 DropPlayerOnBridge（在桥上掉落死亡）
            // 6 DieByBoulder（被滚石砸死）
            // 7 DieByMummy（被木乃伊杀死）
            // 8 DieByLocusts（被蝗虫杀死）

            APIs.Photon.PhotonEventAPI.SendEventToPlugin(12, fromStringArray(killedType), false);
        }

        public static void RECEIVE_KILL()
        {
            //TODO: 无效
            //string[] killedType = new string[1];
            //killedType[0] = type.ToString();

            APIs.Photon.PhotonEventAPI.SendEventToPlugin(3, null, false);
        }

        public static void MorphIntoMummy()
        {
            APIs.Photon.PhotonEventAPI.SendEventToPlugin(82, null, false);
        }

        public static Il2CppSystem.Object fromStringArray(System.String[] stringArr)
        {
            int size = stringArr.Length;
            UnhollowerBaseLib.Il2CppStringArray strs = new UnhollowerBaseLib.Il2CppStringArray(size);

            for (int i = 0; i < size; i++)
            {
                strs[i] = stringArr[i];
            }

            Il2CppSystem.Object obj = new Il2CppSystem.Object(strs.Pointer);

            return obj;
        }
    }
}