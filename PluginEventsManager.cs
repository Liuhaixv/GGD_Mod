using GGD_Hack.GameData;
using GGD_Hack.Hook;
using Handlers.GameHandlers;
using Handlers.GameHandlers.PlayerHandlers;
using UnhollowerBaseLib.Runtime;

namespace GGD_Hack
{
    public class PluginEventsManager
    {
        //把炸弹传递给别人
        //84 c0 0f 84 8e 00 00 00 48 8b 83 a0 00 00 00 48 85 c0 0f 84 84 00 00 00 48 8b 58 40 48 85 db 74 75
        //    JDNKCCJOEOK = this->fields.JDNKCCJOEOK;
        //if ( !JDNKCCJOEOK )
        //  goto LABEL_14;
        //currentTarget = (System_String_o**) JDNKCCJOEOK->fields.currentTarget;
        public static void Throw_Bomb(string userId)
        {
            Managers.MainManager.Instance.pluginEventsManager.COELPNIOKGC(userId);
        }

        //静音鸭子技能
        //Handlers_GameHandlers_PlayerHandlers_LocalPlayer__Silence
        public static void Silence(string userId)
        {
            Managers.MainManager.Instance.pluginEventsManager.GANGMAMOOKK(userId);
        }

        //派对鸭技能
        //Handlers_GameHandlers_PlayerHandlers_LocalPlayer__Helium
        public static void Helium(string userId)
        {
            Managers.MainManager.Instance.pluginEventsManager.NEPAGJIDJJO(userId);
        }

        //Handlers_GameHandlers_PlayerHandlers_LocalPlayer__Kill
        public static void Kill(string userId)
        {
            string[] strs = { userId };
            Managers.MainManager.Instance.pluginEventsManager.GCMNABOLCMH(strs, LocalPlayer.Instance.Player.stingerId);
            //播放杀人音效

            Handlers.CommonHandlers.SoundHandler.Instance.PlayKillTarget();
        }

        /// <summary>
        /// 秃鹫吃尸体
        /// </summary>
        /// <param name="userId"></param>
        public static void Eat(string userId)
        {
            Managers.MainManager.Instance.pluginEventsManager.LKLLIMJKJPC(userId);
        }

        //鹈鹕吃人
        public static void PelicanEat(string userId)
        {
            string[] strs = { userId };

            Managers.MainManager.Instance.pluginEventsManager.CENBNPEMKCK(strs);

            //播放吃人音效
            Handlers.CommonHandlers.SoundHandler.Instance.PlayChompEat();
        }

        //超能力鸭子
        public static void Esper(string userId)
        {
            Managers.MainManager.Instance.pluginEventsManager.PEBEEIKBEAO(userId);
        }

        public static void Drag_Body(BodyHandler body)
        {
            //PDAHABHMDFE.ALALGHGDAAN

            Managers.MainManager.Instance.pluginEventsManager.ENINGCGJDFO(body);
        }

        public static void Drop_Body(BodyHandler body)
        {
            Managers.MainManager.Instance.pluginEventsManager.BFGHPLBEDHO(body);
        }

        /// <summary>
        /// 鸽子感染
        /// </summary>
        /// <param name="userId"></param>
        public static void Infect(string userId)
        {
            Managers.MainManager.Instance.pluginEventsManager.PMOHCINILOD(userId);
        }

        /// <summary>
        /// 钻管道
        /// </summary>
        /// <param name="ventId">VentHandler.Instance.ventId</param>
        public static void Vent(string ventId)
        {
            APIs.Photon.PhotonEventAPI.SendEventToPlugin((byte)EventDataCode.VENT, (Il2CppSystem.Object)ventId);
        }

        /// <summary>
        /// 刺客狙击
        /// </summary>
        /// <param name="userId">目标</param>
        /// <param name="roldId">要猜测的身份id</param>
        /// 48 8B D8 48 85 C0 0F 84 92 00 00 00 48 85 FF 74 14 48 8B 10 48 8B CF 48 8B 52 40 E8 ? ? ? ? 48 85 C0 74 7F 
        public static void Assasinate(string userId, int roldId)
        {
            Managers.MainManager.Instance.pluginEventsManager.GNHPOECJCLN(userId, (OBBMCDJMMOK)roldId);
        }

        /// <summary>
        /// 拉铃
        /// </summary>
        public static void RingBell()
        {
            Managers.MainManager.Instance.pluginEventsManager.PBHJJPFHCPO();
            // APIs.Photon.PhotonEventAPI.SendEventToPlugin((byte)EventDataCode.EMERGENCY, null);
        }
    }
}
