using CodeStage.AntiCheat.ObscuredTypes;
using Handlers.SettingsHandler;
using HarmonyLib;
using MelonLoader;

//正义使者无限刀人次数

namespace GGD_Hack.Features
{

    public class VigilanteInfiniteKill
    {
#if Legit
        public static MelonPreferences_Entry<bool> Enabled = MelonPreferences.CreateEntry<bool>("GGDH", "Enable_" + nameof(VigilanteInfiniteKill), false);

#else
        public static MelonPreferences_Entry<bool> Enabled = MelonPreferences.CreateEntry<bool>("GGDH","Enable_"+ nameof(VigilanteInfiniteKill), true);

#endif

        //void UnityEngine.Events.InvokableCall::Invoke()
        //- 方法名: ELCAEGJNILK
        //- 完整方法名: .IBGBHJCDMIP.ELCAEGJNILK
        //屏蔽次数限制
        //40 57 48 83 EC 20 80 3D ?? ?? ?? ?? ?? 48 8B F9 75 1F 48 8D 0D ?? ?? ?? ?? E8 ?? ?? ?? ?? 48 8D 0D ?? ?? ?? ?? E8 ?? ?? ?? ?? C6 05 ?? ?? ?? ?? ?? 48 8B 97 A8 00 00 00 48 89 5C 24 30 48 85 D2 0F 84 86 00 00 00 48 8B 5A 40 48 85 DB 74 72 4C 8B 05 ?? ?? ?? ?? 48 8B 03 41 0F B6 88 2C 01 00 00 38 88 2C 01 00 00 72 58 48 8B 80 C8 00 00 00 4C 39 44 C8 F8 75 4A C6 82 80 00 00 00 01 C6 87 B0 00 00 00 01
        //TODO: 存在bug按钮按下自动重置 [HarmonyPatch(typeof(BLJFNIOHALE), nameof(BLJFNIOHALE.GKDNDNLIHAF))]
        /*
        class IgnoreKillTimes
        {
            static void Postfix(ref BLJFNIOHALE __instance)
            {
                try
                {
                    //检查功能是否启用
                    if (VigilanteInfiniteKill.Enabled.Value == false)
                    {
                        return;
                    }
                       
                    MelonLogger.Msg("正在屏蔽正义使者刀人限制次数...");

                    //
                    __instance.DIJCKDBCNDD = false;

                    //恢复CD
                    Objects.GameSettings gameSettings = NewSettingsPanelHandler.Instance.gameSettings;
                    ObscuredFloat cooldown = gameSettings.killCooldown;

                    UICooldownButton iNBJCHPPLJF = __instance.LIFIAAKKLGF;
                    //调用invokes
                    //iNBJCHPPLJF.onReady.Invoke();
                    iNBJCHPPLJF.IIKGFIKEOIJ(cooldown);//UICooldownButton$$set_cooldown

                    __instance.LIFIAAKKLGF.Paused = false;
                }
                catch (System.Exception ex)
                {
                    MelonLogger.Msg(ex.Message);
                }
            }
        }*/
    }
}
