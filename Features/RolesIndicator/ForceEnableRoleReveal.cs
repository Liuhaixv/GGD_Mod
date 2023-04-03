using Handlers.SettingsHandler;
using HarmonyLib;
using MelonLoader;

namespace GGD_Hack.Features.RolesIndicator
{
    //本地修改
    [HarmonyPatch(typeof(NewSettingsPanelHandler), nameof(NewSettingsPanelHandler.Update))]
    public class LocalEnableRoleReveal
    {
        static void Postfix(NewSettingsPanelHandler __instance)
        {
            Objects.GameSettings gameSettings = __instance.gameSettings;

            if (gameSettings == null)
            {
                //TODO
                MelonLogger.Error("NewSettingsPanelHandler为空");
                return;
            }

            gameSettings.roleReveal = true;
        }
    }

    //房主时候修改房规
    //会卡住其他人动不了
    //[HarmonyPatch(typeof(APIs.Photon.PhotonEventAPI), nameof(APIs.Photon.PhotonEventAPI.SendEventToPlugin))]
    public class RoomEnableRoleReveal
    {
        static void Prefix(APIs.Photon.PhotonEventAPI __instance, byte __0,ref Il2CppSystem.Object __1)
        {
            if (__0 != (byte)GameData.EventDataCode.SETTINGS_UPDATE)
            {
                return;
            }

            Il2CppSystem.Collections.Generic.Dictionary<string, float> newGameSettings = __1 as Il2CppSystem.Collections.Generic.Dictionary<string, float>;

            if(newGameSettings == null)
            {
                return;
            }

            //揭示身份
            newGameSettings["vote_reveal"] = 1;
            //随机位置
            newGameSettings["random_start_positions"] = 0;
            //推荐
            newGameSettings["isrecommended"] = 1;
            newGameSettings["number_of_ducks"] = 6;
        }
    }

    //房主时候修改房规
    //TODO:创房崩溃
#if false
    [HarmonyPatch(typeof(Managers.GameManagers.EventsManagers.PluginEventsManager), nameof(Managers.GameManagers.EventsManagers.PluginEventsManager.KNIMBEJDIPP), typeof(Il2CppSystem.Collections.Generic.Dictionary<string, float>))]
    public class RoomEnableRoleReveal
    {
        static void Prefix(APIs.Photon.PhotonEventAPI __instance, Il2CppSystem.Collections.Generic.Dictionary<string, float> __0)
        {

            Il2CppSystem.Collections.Generic.Dictionary<string, float> newGameSettings = __0;
           foreach(var entry in __0)
            {
                entry.ToString();
            }

           //揭示身份
            newGameSettings["vote_reveal"] = 1;
            //随机位置
            newGameSettings["random_start_positions"] = 0;
        }
    }
#endif
}
