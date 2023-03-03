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
        public static void Throw_Bomb(string userId)
        {
            //TODO:2.18.00更新  Managers.MainManager.Instance.pluginEventsManager.GFIHIIABBKL(userId);
        }

        //静音鸭子技能
        public static void Silence(string userId)
        {
            //TODO:2.18.00更新 Managers.MainManager.Instance.pluginEventsManager.NDJOCHMBDND(userId);
        }

        //派对鸭技能
        //Handlers_GameHandlers_PlayerHandlers_LocalPlayer__Helium
        public static void Helium(string userId)
        {
            Managers.MainManager.Instance.pluginEventsManager.AACKJICGHMG(userId);
        }

        //Handlers_GameHandlers_PlayerHandlers_LocalPlayer__Kill
        public static void Kill(string userId)
        {
            string[] strs = { userId };
            Managers.MainManager.Instance.pluginEventsManager.OBKJMFPNAGE(strs, LocalPlayer.Instance.Player.stingerId);
            //播放杀人音效
            
            Handlers.CommonHandlers.SoundHandler.Instance.PlayKillTarget();
        }

        //鹈鹕吃人
        public static void PelicanEat(string userId)
        {
            string[] strs = { userId };

            Managers.MainManager.Instance.pluginEventsManager.MPJIBPEAOGL(strs);

            //播放吃人音效
            Handlers.CommonHandlers.SoundHandler.Instance.PlayChompEat();
        }

        //超能力鸭子
        public static void Esper(string userId)
        {
            //TODO:2.18.00更新 Managers.MainManager.Instance.pluginEventsManager.CGFMLAGDNFJ(userId);
        }

        public static void Drag_Body(BodyHandler body)
        {
            Managers.MainManager.Instance.pluginEventsManager.JPKOJEKGAGB(body);
        }

        public static void Drop_Body(BodyHandler body)
        {
            Drag_Body(body);
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
        public static void Assasinate(string userId, int roldId)
        {
            //TODO:2.18.00更新 Managers.MainManager.Instance.pluginEventsManager.JKCOFDKFAAD(userId, (FJCKNAJLDIG)roldId);
        }

        public static void AssasinateGoose(string userId)
        {
            //TODO:2.18.00更新 Managers.MainManager.Instance.pluginEventsManager.JKCOFDKFAAD(userId, FJCKNAJLDIG.Goose);
        }

        /// <summary>
        /// 拉铃
        /// </summary>
        public static void RingBell()
        {
            APIs.Photon.PhotonEventAPI.SendEventToPlugin((byte)EventDataCode.EMERGENCY, null);
        }       
    }
}
