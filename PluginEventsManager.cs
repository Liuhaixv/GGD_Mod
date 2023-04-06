﻿using GGD_Hack.GameData;
using GGD_Hack.Hook;
using Handlers.GameHandlers;
using Handlers.GameHandlers.PlayerHandlers;
using UnhollowerBaseLib.Runtime;

namespace GGD_Hack
{
    public class PluginEventsManager
    {
        //把炸弹传递给别人
        //40 53 48 83 EC 20 80 3D ?? ?? ?? ?? ?? 48 8B DA 75 13 48 8D 0D ?? ?? ?? ?? E8 ?? ?? ?? ?? C6 05 ?? ?? ?? ?? ?? 48 8B 0D ?? ?? ?? ?? 83 B9 E0 00 00 00 00 75 05 E8 ?? ?? ?? ?? 45 33 C9 45 33 C0 48 8B D3 B1 30
        public static void Throw_Bomb(string userId)
        {
            //Managers.MainManager.Instance.pluginEventsManager.NCJLHHALAKH(userId);
            APIs.Photon.PhotonEventAPI.SendEventToPlugin((byte)GameData.EventDataCode.THROW_BOMB, userId);
            Handlers.CommonHandlers.SoundHandler.Instance.PlayMedium(true);
        }

        //报警尸体
        //48 89 5C 24 08 57 48 83 EC 20 80 3D ?? ?? ?? ?? ?? 48 8B FA 75 1F 48 8D 0D ?? ?? ?? ?? E8 ?? ?? ?? ?? 48 8D 0D ?? ?? ?? ?? E8 ?? ?? ?? ?? C6 05 ?? ?? ?? ?? ?? 48 8B 0D ?? ?? ?? ?? BA 01 00 00 00 E8 ?? ?? ?? ?? 48 8B D8 48 85 C0 74 52 48 85 FF 74 14 48 8B 10 48 8B CF 48 8B 52 40 E8 ?? ?? ?? ?? 48 85 C0 74 3F 83 7B 18 00 76 49 48 89 7B 20 48 8B 0D ?? ?? ?? ?? 83 B9 E0 00 00 00 00 75 05 E8 ?? ?? ?? ?? 45 33 C9 45 33 C0 48 8B D3 B1 04
        public static void Report(string userId)
        {
            //Managers.MainManager.Instance.pluginEventsManager.ANNHCPEAGMF(userId);
            APIs.Photon.PhotonEventAPI.SendEventToPlugin((byte)GameData.EventDataCode.REPORT, userId);
        }

        //炸弹鸭子技能
        //40 53 48 83 EC 20 80 3D ?? ?? ?? ?? ?? 48 8B DA 75 13 48 8D 0D ?? ?? ?? ?? E8 ?? ?? ?? ?? C6 05 ?? ?? ?? ?? ?? 48 8B 0D ?? ?? ?? ?? 83 B9 E0 00 00 00 00 75 05 E8 ?? ?? ?? ?? 45 33 C9 45 33 C0 48 8B D3 B1 3E
        public static void Generate_Bomb(string userId)
        {
            //Managers.MainManager.Instance.pluginEventsManager.IOPLDNMNDFM(userId);
            APIs.Photon.PhotonEventAPI.SendEventToPlugin((byte)GameData.EventDataCode.GENERATE_BOMB, userId);
            Handlers.CommonHandlers.SoundHandler.Instance.PlayMedium(true);
        }

        //静音鸭子技能
        //Handlers_GameHandlers_PlayerHandlers_LocalPlayer__Silence
        //40 53 48 83 EC 20 80 3D ?? ?? ?? ?? ?? 48 8B DA 75 13 48 8D 0D ?? ?? ?? ?? E8 ?? ?? ?? ?? C6 05 ?? ?? ?? ?? ?? 48 8B 0D ?? ?? ?? ?? 83 B9 E0 00 00 00 00 75 05 E8 ?? ?? ?? ?? 45 33 C9 45 33 C0 48 8B D3 B1 18
        public static void Silence(string userId)
        {
            //Managers.MainManager.Instance.pluginEventsManager.LANPDIOBPKM(userId);
            APIs.Photon.PhotonEventAPI.SendEventToPlugin((byte)GameData.EventDataCode.SILENCE, userId);
        }

        //派对鸭技能
        //Handlers_GameHandlers_PlayerHandlers_LocalPlayer__Helium
        //40 53 48 83 EC 20 80 3D ?? ?? ?? ?? ?? 48 8B DA 75 13 48 8D 0D ?? ?? ?? ?? E8 ?? ?? ?? ?? C6 05 ?? ?? ?? ?? ?? 48 8B 0D ?? ?? ?? ?? 83 B9 E0 00 00 00 00 75 05 E8 ?? ?? ?? ?? 45 33 C9 45 33 C0 48 8B D3 B1 2E
        public static void Helium(string userId)
        {
            //Managers.MainManager.Instance.pluginEventsManager.EJAKKAIMHIM(userId);
            APIs.Photon.PhotonEventAPI.SendEventToPlugin((byte)GameData.EventDataCode.HELIUM, userId);
        }

        //Handlers_GameHandlers_PlayerHandlers_LocalPlayer__Kill
        //48 89 5C 24 08 48 89 74 24 10 57 48 83 EC 20 80 3D ?? ?? ?? ?? ?? 49 8B F0 48 8B FA 75 1F 48 8D 0D ?? ?? ?? ?? E8 ?? ?? ?? ?? 48 8D 0D ?? ?? ?? ?? E8 ?? ?? ?? ?? C6 05 ?? ?? ?? ?? ?? 48 8B 0D ?? ?? ?? ?? BA 02 00 00 00 E8 ?? ?? ?? ?? 48 8B D8 48 85 C0 74 7E 48 85 FF 74 14 48 8B 10 48 8B CF 48 8B 52 40 E8 ?? ?? ?? ?? 48 85 C0 74 6B 83 7B 18 00 0F 86 81 00 00 00 48 89 7B 20 48 85 F6 74 14 48 8B 13 48 8B CE 48 8B 52 40 E8 ?? ?? ?? ?? 48 85 C0 74 54 83 7B 18 01 76 5E 48 89 73 28 48 8B 0D ?? ?? ?? ?? 83 B9 E0 00 00 00 00 75 05 E8 ?? ?? ?? ?? 45 33 C9 45 33 C0 48 8B D3 B1 14
        /// <summary>
        /// 击杀其他玩家
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="stingerId">击杀动画id</param>
        public static void Kill(string userId, string stingerId = null)
        {
            string[] strs = { userId };
            Managers.MainManager.Instance.pluginEventsManager.AOGMBHOEOOL(strs, stingerId != null ? stingerId : LocalPlayer.Instance.Player.stingerId);

            //播放杀人音效
            Handlers.CommonHandlers.SoundHandler.Instance.PlayKillTarget();
        }

        /// <summary>
        /// 秃鹫吃尸体
        /// </summary>
        /// <param name="userId"></param>
        ///40 53 48 83 EC 20 80 3D ?? ?? ?? ?? ?? 48 8B DA 75 13 48 8D 0D ?? ?? ?? ?? E8 ?? ?? ?? ?? C6 05 ?? ?? ?? ?? ?? 48 8B 0D ?? ?? ?? ?? 83 B9 E0 00 00 00 00 75 05 E8 ?? ?? ?? ?? 45 33 C9 45 33 C0 48 8B D3 B1 13
        public static void Eat(string userId)
        {
            //Managers.MainManager.Instance.pluginEventsManager.PDGEAJBLJHN(userId);
            APIs.Photon.PhotonEventAPI.SendEventToPlugin((byte)GameData.EventDataCode.EAT, userId);

            //播放吃人音效
            Handlers.CommonHandlers.SoundHandler.Instance.PlayChompEat();
        }

        //鹈鹕吃人
        //40 53 48 83 EC 20 80 3D ?? ?? ?? ?? ?? 48 8B DA 75 13 48 8D 0D ?? ?? ?? ?? E8 ?? ?? ?? ?? C6 05 ?? ?? ?? ?? ?? 48 8B 0D ?? ?? ?? ?? 83 B9 E0 00 00 00 00 75 05 E8 ?? ?? ?? ?? 45 33 C9 45 33 C0 48 8B D3 B1 4E
        public static void PelicanEat(string userId)
        {
            string[] strs = { userId };

            //Managers.MainManager.Instance.pluginEventsManager.IAKEDFKLKKM(strs);
            APIs.Photon.PhotonEventAPI.SendEventToPlugin((byte)GameData.EventDataCode.PELICAN_EAT, userId);

            //播放吃人音效
            Handlers.CommonHandlers.SoundHandler.Instance.PlayChompEat();
        }

        //超能力鸭子
        //40 53 55 48 83 EC 28 80 3D ?? ?? ?? ?? ?? 48 8B EA 0F
        /*
        public static void Esper(string userId)
        {
            Managers.MainManager.Instance.pluginEventsManager.BNDCLHFBEBH(userId);
        }
        */

        //40 53 48 83 EC 20 80 3D ?? ?? ?? ?? ?? 48 8B DA 75 13 48 8D 0D ?? ?? ?? ?? E8 ?? ?? ?? ?? C6 05 ?? ?? ?? ?? ?? 48 85 DB 74 2E 48 8B 0D ?? ?? ?? ?? 48 8B 5B 18 83 B9 E0 00 00 00 00 75 05 E8 ?? ?? ?? ?? 45 33 C9 45 33 C0 48 8B D3 B1 3B
        public static void Drag_Body(BodyHandler body)
        {
            //PDAHABHMDFE.ALALGHGDAAN

            Managers.MainManager.Instance.pluginEventsManager.BNPIDBHLEBC(body);
        }

        //40 53 48 83 EC 20 80 3D ?? ?? ?? ?? ?? 48 8B DA 75 2B 48 8D 0D ?? ?? ?? ?? E8 ?? ?? ?? ?? 48 8D 0D ?? ?? ?? ?? E8 ?? ?? ?? ?? 48 8D 0D ?? ?? ?? ?? E8 ?? ?? ?? ?? C6 05 ?? ?? ?? ?? ?? 48 89 6C
        public static void Drop_Body(BodyHandler body)
        {
            Managers.MainManager.Instance.pluginEventsManager.MDEIHFLFGLO(body);
        }

        /// <summary>
        /// 鸽子感染
        /// </summary>
        /// <param name="userId"></param>
        ///40 53 48 83 EC 20 80 3D ?? ?? ?? ?? ?? 48 8B DA 75 13 48 8D 0D ?? ?? ?? ?? E8 ?? ?? ?? ?? C6 05 ?? ?? ?? ?? ?? 48 8B 0D ?? ?? ?? ?? 83 B9 E0 00 00 00 00 75 05 E8 ?? ?? ?? ?? 45 33 C9 45 33 C0 48 8B D3 B1 20
        public static void Infect(string userId)
        {
            //Managers.MainManager.Instance.pluginEventsManager.IAEHPPKDLFE(userId);
            APIs.Photon.PhotonEventAPI.SendEventToPlugin((byte)GameData.EventDataCode.INFECT, userId);
        }

        /// <summary>
        /// 钻管道
        /// </summary>
        /// <param name="ventId">VentHandler.Instance.ventId</param>
        ///40 53 48 83 EC 20 80 3D ?? ?? ?? ?? ?? 48 8B DA 75 13 48 8D 0D ?? ?? ?? ?? E8 ?? ?? ?? ?? C6 05 ?? ?? ?? ?? ?? 48 8B 0D ?? ?? ?? ?? 83 B9 E0 00 00 00 00 75 05 E8 ?? ?? ?? ?? 45 33 C9 45 33 C0 48 8B D3 B1 07
        public static void Vent(string ventId)
        {
            //Managers.MainManager.Instance.pluginEventsManager.EFKHKEOHKKO(ventId);
            APIs.Photon.PhotonEventAPI.SendEventToPlugin((byte)GameData.EventDataCode.VENT, ventId);
        }

        /// <summary>
        /// 刺客狙击
        /// </summary>
        /// <param name="userId">目标</param>
        /// <param name="roldId">要猜测的身份id</param>
        //48 89 5C 24 08 48 89 74 24 18 57 48 83 EC 20 80 3D ?? ?? ?? ?? ?? 48 8B FA 41 0F BF F0 75 1F 48 8D 0D ?? ?? ?? ?? E8 ?? ?? ?? ?? 48 8D 0D ?? ?? ?? ?? E8 ?? ?? ?? ?? C6 05 ?? ?? ?? ?? ?? 48 8B 0D ?? ?? ?? ?? BA 02
        public static void Assasinate(string userId, int roldId)
        {
            Managers.MainManager.Instance.pluginEventsManager.OIKLNCHNNAN(userId, (IPLJDOHJOLM)roldId);
        }

        /// <summary>
        /// 拉铃
        /// </summary>
        ///48 83 EC 28 80 3D ?? ?? ?? ?? ?? 75 13 48 8D 0D ?? ?? ?? ?? E8 ?? ?? ?? ?? C6 05 ?? ?? ?? ?? ?? 48 8B 0D ?? ?? ?? ?? 83 B9 E0 00 00 00 00 75 05 E8 ?? ?? ?? ?? 45 33 C9 45 33 C0 33 D2 B1 02
        public static void RingBell()
        {
            //Managers.MainManager.Instance.pluginEventsManager.BLBDMPAAFIA();
            APIs.Photon.PhotonEventAPI.SendEventToPlugin((byte)EventDataCode.EMERGENCY, null);
        }

        //48 89 5C 24 08 48 89 74 24 10 57 48 83 EC 20 80 3D ?? ?? ?? ?? ?? 49 8B F0 48 8B FA 75 1F 48 8D 0D ?? ?? ?? ?? E8 ?? ?? ?? ?? 48 8D 0D ?? ?? ?? ?? E8 ?? ?? ?? ?? C6 05 ?? ?? ?? ?? ?? 48 8B 0D ?? ?? ?? ?? BA 02 00 00 00 E8 ?? ?? ?? ?? 48 8B D8 48 85 C0 74 7E 48 85 FF 74 14 48 8B 10 48 8B CF 48 8B 52 40 E8 ?? ?? ?? ?? 48 85 C0 74 6B 83 7B 18 00 0F 86 81 00 00 00 48 89 7B 20 48 85 F6 74 14 48 8B 13 48 8B CE 48 8B 52 40 E8 ?? ?? ?? ?? 48 85 C0 74 54 83 7B 18 01 76 5E 48 89 73 28 48 8B 0D ?? ?? ?? ?? 83 B9 E0 00 00 00 00 75 05 E8 ?? ?? ?? ?? 45 33 C9 45 33 C0 48 8B D3 B1 11
        public static void Kick_Player(string userId, string reason)
        {
            Managers.MainManager.Instance.pluginEventsManager.IGLGBEBKOPM(userId, reason);
        }

        //48 83 EC 28 80 3D ?? ?? ?? ?? ?? 75 13 48 8D 0D ?? ?? ?? ?? E8 ?? ?? ?? ?? C6 05 ?? ?? ?? ?? ?? 48 8B 0D ?? ?? ?? ?? 83 B9 E0 00 00 00 00 75 05 E8 ?? ?? ?? ?? 45 33 C9 45 33 C0 33 D2 B1 1E
        //移动飞船前必须调用Precursor(true)，之后再Precursor(false)
        public static void Move_Shuttle()
        {
            //TODO:
        }

        //40 53 48 83 EC 20 80 3D ?? ?? ?? ?? ?? 0F B6 DA 75 1F 48 8D 0D ?? ?? ?? ?? E8 ?? ?? ?? ?? 48 8D 0D ?? ?? ?? ?? E8 ?? ?? ?? ?? C6 05 ?? ?? ?? ?? ?? 48 8B 0D ?? ?? ?? ?? 48 8D 54 24 38 88 5C 24 38 E8 ?? ?? ?? ?? 48 8B 0D ?? ?? ?? ?? 48 8B D8 83 B9 E0 00 00 00 00 75 05 E8 ?? ?? ?? ?? 45 33 C9 45 33 C0 48 8B D3 B1 91
        public static void Precursor(bool enable)
        {
            Managers.MainManager.Instance.pluginEventsManager.AFBDLHKELGG(enable);
        }

        //40 53 48 83 EC 20 80 3D ?? ?? ?? ?? ?? 48 8B DA 75 13 48 8D 0D ?? ?? ?? ?? E8 ?? ?? ?? ?? C6 05 ?? ?? ?? ?? ?? 48 8B 0D ?? ?? ?? ?? 83 B9 E0 00 00 00 00 75 05 E8 ?? ?? ?? ?? 45 33 C9 45 33 C0 48 8B D3 B1 32
        public static void Settings_Update(Il2CppSystem.Collections.Generic.Dictionary<string, float> newRoomSettings)
        {
            Managers.MainManager.Instance.pluginEventsManager.KNIMBEJDIPP(newRoomSettings);
        }

        //40 53 48 81 EC B0 00 00 00 48 8B DA
        public static void HandleReconnectionData()
        {

        }

        //48 89 5C 24 08 48 89 6C 24 10 48 89 74 24 18 57 48 83 EC 20 80 3D ?? ?? ?? ?? ?? 49 8B F1 49 8B E8 48 8B FA 75 1F 48 8D 0D ?? ?? ?? ?? E8 ?? ?? ?? ?? 48 8D 0D ?? ?? ?? ?? E8 ?? ?? ?? ?? C6 05 ?? ?? ?? ?? ?? 48 8B 0D ?? ?? ?? ?? BA 03 00 00 00 E8 ?? ?? ?? ?? 48 8B D8 48 85 C0 0F 84 B2 00 00 00 48 85 FF 74 18 48 8B 10 48 8B CF 48 8B 52 40 E8 ?? ?? ?? ?? 48 85 C0 0F 84 9B 00 00 00 83 7B 18 00 0F 86 C1 00 00 00 48 89 7B 20 48 85 ED 74 18 48 8B 13 48 8B CD 48 8B 52 40 E8 ?? ?? ?? ?? 48 85 C0 0F 84 80 00 00 00 83 7B 18 01 0F 86 96 00 00 00 48 89 6B 28 48 85 F6 74 14 48 8B 13 48 8B CE 48 8B 52 40 E8 ?? ?? ?? ?? 48 85 C0 74 69 83 7B 18 02 76 73 48 89 73 30 48 8B 0D ?? ?? ?? ?? 83 B9 E0 00 00 00 00 75 05 E8 ?? ?? ?? ?? 45 33 C9 45
        public static void Complete_Task(string userId, string taskId, string s3 = "")
        {
            Managers.MainManager.Instance.taskEventsManager.OEJGOPFACBH(userId, taskId, s3);
        }

        //E8 ?? ?? ?? ?? 48 8B 15 ?? ?? ?? ?? 48 85 DB 0F 84 AB 0F 00 00
        public static void RevealRoleInternalLink(string userId, int roleId)
        {
            if(LocalPlayer.Instance.Player.userId == userId)
            {
                //防止出bug
                return;
            }
            Managers.MainManager.Instance.pluginEventsManager.NGCEEDDMGEH(userId, (IPLJDOHJOLM)roleId);
        }
    }
}
