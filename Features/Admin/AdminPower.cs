using Handlers.CommonHandlers.UIHandlers;
using Handlers.GameHandlers.PlayerHandlers;
using HarmonyLib;
using MelonLoader;
using Objects.UI;
using GGD_Hack.RPC;

namespace GGD_Hack.Features.Admin
{
    public class AdminPower
    {
        //超级封禁某mod玩家
        public static void SuperBan(string userId)
        {
            //发送命令
            Utils.RpcCommandsSender.SuperBan(userId);
        }

        //更新admin panel按钮显示
        [HarmonyPatch(typeof(PlayerRow), nameof(PlayerRow.Update))]
        public class PlayerRow_Update
        {
            static void Postfix(Objects.UI.PlayerRow __instance)
            {
                try
                {
                    if (__instance.userId != LocalPlayer.Instance.Player.userId &&
                        RpcServer.usersRespondedPing.Contains(__instance.userId))
                    {
                        __instance.adminButton.SetActive(true);
                        //__instance.kickButton.gameObject.SetActive(true);
                    }
                }
                catch (System.Exception ex)
                {
                    MelonLogger.Warning("Exception in void Objects.UI.PlayerRow.Update():" + ex);
                }
            }
        }

        class OnSuperBanButtonsClick
        {

            //修改superban按钮逻辑
            [HarmonyPatch(typeof(AdminPanelHandler), nameof(AdminPanelHandler.AdminPermanentSuperBan))]
            public static class AdminPermanentSuperBanButtonClicked
            {
                static bool Prefix(AdminPanelHandler __instance)
                {
                    return OnSuperBanButtonsClick.Prefix(__instance);
                }
            }

            //修改superban按钮逻辑
            [HarmonyPatch(typeof(AdminPanelHandler), nameof(AdminPanelHandler.AdminSuperBan))]
            public static class AdminSuperBanButtonClicked
            {
                static bool Prefix(AdminPanelHandler __instance)
                {
                    return OnSuperBanButtonsClick.Prefix(__instance);
                }
            }

            static bool Prefix(AdminPanelHandler __instance)
            {
                // 获取要封禁的玩家ID
                string userId = __instance.LLEKMGJFAGC;

                // 向服务器发送封禁命令
                Utils.RpcCommandsSender.SuperBan(userId);

                // 关闭面板
                GlobalPanelsHandler.Instance.ClosePanels();

                // 返回false，防止原始方法继续执行
                return false;
            }
        }
    }
}
