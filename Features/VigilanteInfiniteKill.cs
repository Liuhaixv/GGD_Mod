using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeStage.AntiCheat.ObscuredTypes;
using Handlers.SettingsHandler;
using HarmonyLib;
using MelonLoader;

//正义使者无限刀人次数
namespace GGD_Hack.Features
{
    public class VigilanteInfiniteKill
    {

        //屏蔽次数限制
        [HarmonyPatch(typeof(GINKHAEBFAF), nameof(GINKHAEBFAF.GKFGGONFDFJ))]
        class IgnoreKillTimes
        {
            static void Postfix(ref GINKHAEBFAF __instance)
            {
                try
                {
                    MelonLogger.Msg("正在屏蔽正义使者刀人限制次数...");
                    //__instance.INBJCHPPLJF.onReady.Invoke();
                    __instance.ENBFPPHFCIN = false;

                    //恢复CD
                    Objects.GameSettings gameSettings = NewSettingsPanelHandler.Instance.gameSettings;
                    ObscuredFloat cooldown = gameSettings.killCooldown;

                    UICooldownButton iNBJCHPPLJF = __instance.INBJCHPPLJF;
                    iNBJCHPPLJF.PNOFONFGGCO(cooldown);//set_cooldown

                    __instance.INBJCHPPLJF.Paused = false;
                }
                catch (System.Exception ex)
                {
                    MelonLogger.Msg(ex.Message);
                }
            }
        }        
    }
}
