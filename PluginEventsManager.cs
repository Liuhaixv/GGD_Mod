using Handlers.GameHandlers.PlayerHandlers;

namespace GGD_Hack
{
    public class PluginEventsManager
    {
        //把炸弹传递给别人
        public static void Throw_Bomb(string userId)
        {
            Managers.MainManager.Instance.pluginEventsManager.GFIHIIABBKL(userId);
        }

        //静音鸭子技能
        public static void Silence(string userId)
        {

            Managers.MainManager.Instance.pluginEventsManager.NDJOCHMBDND(userId);

        }

        public static void Kill(string userId)
        {
            string[] strs = { userId };
            Managers.MainManager.Instance.pluginEventsManager.EHECNLJMLBF(strs, LocalPlayer.Instance.Player.stingerId);
            //播放杀人音效
            Handlers.CommonHandlers.SoundHandler.Instance.PlayKillTarget();
        }

        //鹈鹕吃人
        public static void PelicanEat(string userId)
        {
            string[] strs = { userId };

            Managers.MainManager.Instance.pluginEventsManager.CIBACNBFHED(strs);
            //播放吃人音效
            Handlers.CommonHandlers.SoundHandler.Instance.PlayChompEat();
        }

        //超能力鸭子
        public static void Esper(string userId)
        {
            Managers.MainManager.Instance.pluginEventsManager.CGFMLAGDNFJ(userId);
        }

        public static void RingBell()
        {
            APIs.Photon.PhotonEventAPI.SendEventToPlugin(2, null);
        }
    }
}
