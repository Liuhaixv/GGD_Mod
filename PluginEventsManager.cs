using GGD_Hack.GameData;
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
        public static void Helium(string userId)
        {
            //TODO:2.18.00更新  Managers.MainManager.Instance.pluginEventsManager.OGINDGDMPAB(userId);
        }

        public static void Kill(string userId)
        {
            string[] strs = { userId };
            //TODO:2.18.00更新 Managers.MainManager.Instance.pluginEventsManager.EHECNLJMLBF(strs, LocalPlayer.Instance.Player.stingerId);
            //播放杀人音效
            
            Handlers.CommonHandlers.SoundHandler.Instance.PlayKillTarget();
        }

        //鹈鹕吃人
        public static void PelicanEat(string userId)
        {
            string[] strs = { userId };

            //TODO:2.18.00更新 Managers.MainManager.Instance.pluginEventsManager.CIBACNBFHED(strs);

            //播放吃人音效
            Handlers.CommonHandlers.SoundHandler.Instance.PlayChompEat();
        }

        //超能力鸭子
        public static void Esper(string userId)
        {
            //TODO:2.18.00更新 Managers.MainManager.Instance.pluginEventsManager.CGFMLAGDNFJ(userId);
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

        public static void RingBell()
        {
            APIs.Photon.PhotonEventAPI.SendEventToPlugin(2, null);
        }
    }
}
