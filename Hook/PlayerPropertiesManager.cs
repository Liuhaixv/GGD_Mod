using HarmonyLib;
using Managers.PlayerManagers;
using MelonLoader;

//修改装扮PlayerPropertiesManager__ChangeUserProperties
//->APIs_Photon_PhotonEventAPI__SendEventToPlugin(0x34u, (Il2CppObject *)CLJBGHKKMJB_UserProperties, 0, 0i64);

//如果装扮了未购买的装扮，修改后会被UpdatePlayerProperties回滚到无装扮
namespace GGD_Hack.Hook
{
    public class PlayerPropertiesManager_
    {

        /*
        /// <summary>
        /// 回滚玩家的装扮
        /// </summary>
        [HarmonyPatch(typeof(PlayerPropertiesManager), nameof(PlayerPropertiesManager.FallbackLegacyProperty))]
        class FallbackLegacyProperty_
        {                     

        }
        */

        /// <summary>
        /// 清除玩家的装扮缓存
        /// </summary>
        [HarmonyPatch(typeof(PlayerPropertiesManager), nameof(PlayerPropertiesManager.ClearTempUserProperties))]
        class ClearTempUserProperties_
        {
            //禁止清除玩家的装扮缓存
            static bool Prefix()
            {
                return false;
            }
        }
    }
}
