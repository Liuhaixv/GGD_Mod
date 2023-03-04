using CodeStage.AntiCheat.ObscuredTypes;
using Handlers.SettingsHandler;
using HarmonyLib;
using MelonLoader;

//正义使者无限刀人次数

namespace GGD_Hack.Features
{

    public class VigilanteInfiniteKill
    {

        //void UnityEngine.Events.InvokableCall::Invoke()
        //- 方法名: ELCAEGJNILK
        //- 完整方法名: .IBGBHJCDMIP.ELCAEGJNILK
        //屏蔽次数限制
        //TODO:2.18.01[HarmonyPatch(typeof(LGJGPPKKBPD), nameof(LGJGPPKKBPD.KIMGDDNPHOH))]
        /*
        class IgnoreKillTimes
        {
            static void Postfix(ref LGJGPPKKBPD __instance)
            {
                try
                {
                    MelonLogger.Msg("正在屏蔽正义使者刀人限制次数...");

                    //
                    __instance.NFICBOMPFIH = false;

                    //恢复CD
                    Objects.GameSettings gameSettings = NewSettingsPanelHandler.Instance.gameSettings;
                    ObscuredFloat cooldown = gameSettings.killCooldown;

                    UICooldownButton iNBJCHPPLJF = __instance.ANEHHKMJNCO;
                    //调用invokes
                    //iNBJCHPPLJF.onReady.Invoke();
                    iNBJCHPPLJF.OFFHJJNJKBE(cooldown);//UICooldownButton$$set_cooldown

                    __instance.ANEHHKMJNCO.Paused = false;
                }
                catch (System.Exception ex)
                {
                    MelonLogger.Msg(ex.Message);
                }
            }
        }*/
    }
}
