using MelonLoader;
using HarmonyLib;
using UnityEngine;
using Handlers.GameHandlers.PlayerHandlers;

//通过UnityEngine.Events.InvokableCall.Invoke断点跟进追踪方法流，查找按钮点击后被点击的方法
//所有的带冷却按钮都是通过PlayerButtonController.RegisterCooldownButton来动态注册的，所有没有按钮的静态变量
namespace GGD_Hack.Internal.Buttons
{    
    public class KillCooldownButton
    {
        public static UICooldownButton kill = null;
        public static ButtonType type = ButtonType.Primary;

        public static PlayerController CurrentTarget
        {
            get
            {
                if (kill == null)
                {
                    return null;
                }

                return kill.currentTarget as PlayerController;
            }
            set
            {
                kill.currentTarget = value;
            }
        }

        public static bool KillFarest()
        {
            Vector3 localPlayerPosition = LocalPlayer.Instance.transform.position;

            float farestDistance = 0;
            PlayerController target = null;

            foreach (var Entry in PlayerController.playersList)
            {
                string userId = Entry.Key;
                PlayerController playerController = Entry.Value;

                if (userId == null || playerController == null) continue;

                if (playerController.timeOfDeath == 0) continue;

                float distance = Vector3.Distance(localPlayerPosition, playerController.PMPPIFBLAPL);
                if (distance > farestDistance)
                {
                    target = playerController;
                }
            }

            return Kill(target);
        }

        /// <summary>
        /// 如果不指定目标则默认击杀按钮指定的目标
        /// </summary>
        /// <param name="specificTarget"></param>
        /// <returns></returns>
        public static bool Kill(PlayerController specificTarget = null)
        {
            //击杀按钮
            if (kill == null)
            {
                MelonLogger.Warning("kill按钮未注册！");
                return false;
            }

            PlayerController targetToKill = CurrentTarget;

            if (specificTarget)
            {
                CurrentTarget = specificTarget;
                targetToKill = CurrentTarget;
            }

            MelonLogger.Msg("正在执行kill按钮方法，目标: " + targetToKill.nickname);
            Click(kill);
            return true;
        }

        //普通刀人
        private static void Click(UICooldownButton cooldownButton)
        {
            KLNOEKIODEE killCooldownButton = new KLNOEKIODEE();
            killCooldownButton.INBJCHPPLJF = cooldownButton;

            killCooldownButton.GKFGGONFDFJ();
        }
    } 
    
    public class AvengerKillCooldownButton
    {
        public static UICooldownButton avengerKill = null;//复仇者 Primary
        public static ButtonType type = ButtonType.Primary;
    }

    //鸽子感染按钮
    public class InfectCooldownButton
    {
        public static UICooldownButton infect = null;
        public static ButtonType type = ButtonType.Primary;

        //TODO
        public static void Infect()
        {
            //DDOOGGBCIFL__MGPMDJODPGK
        }
    }
    
    //侦探技能按钮
    public class InvestigateCooldownButton
    {
        public static UICooldownButton investigate = null;
        public static ButtonType type = ButtonType.Primary;

        //TODO
        public static void Investigate()
        {
        }

    }

    //超能力技能按钮
    public class TelepatCooldownButton
    {
        public static UICooldownButton telepat = null;
        public static ButtonType type = ButtonType.Primary;

        //TODO: 开始一阶段
        public static void Telepat()
        {
        }

    }

    //鹈鹕吃人按钮
    public class PelicanCooldownButton
    {

        //EPFILBHLDML.EKDKJNOFGCA
        public static void Eat()
        {

        }

    }
    /*
     * 
     * 
     */


    //MBFPLDLFKNO.MFKOEDBMPAH
    public class SilenceCooldownButton
    {

    }

    //通过hook注册按钮的函数来动态更新
    [HarmonyPatch(typeof(PlayerButtonController), nameof(PlayerButtonController.RegisterCooldownButton))]
    class RegisterCooldownButton_
    {
        static void Postfix(PlayerButtonController __instance, UICooldownButton __result, string __0, JGEEBFIBHGD __1, UnityEngine.Transform __2, float __3, bool __4)
        {
            /*
             --------------------
                UICooldownButton PlayerButtonController::RegisterCooldownButton(string AKMFDDABJMI, JGEEBFIBHGD KIDEMMMFKPP, UnityEngine.Transform AJHOFGOFCKM, float PMPIAPCEKCO, bool LPAEPADMEFM)
                - __instance: UIButtons (PlayerButtonController)
                - Parameter 0 'AKMFDDABJMI': bomb
                - Parameter 1 'KIDEMMMFKPP': Use
                - Parameter 2 'AJHOFGOFCKM': UseAnchor (UnityEngine.RectTransform)
                - Parameter 3 'PMPIAPCEKCO': 3
                - Parameter 4 'LPAEPADMEFM': True
                - Return value: bomb (UICooldownButton)
            --------------------
                UICooldownButton PlayerButtonController::RegisterCooldownButton(string AKMFDDABJMI, JGEEBFIBHGD KIDEMMMFKPP, UnityEngine.Transform AJHOFGOFCKM, float PMPIAPCEKCO, bool LPAEPADMEFM)
                - __instance: UIButtons (PlayerButtonController)
                - Parameter 0 'AKMFDDABJMI': kill
                - Parameter 1 'KIDEMMMFKPP': Primary
                - Parameter 2 'AJHOFGOFCKM': PrimaryRoleActionAnchor (UnityEngine.RectTransform)
                - Parameter 3 'PMPIAPCEKCO': 10
                - Parameter 4 'LPAEPADMEFM': True
                - Return value: kill (UICooldownButton)
             */
            try
            {
                string gameObjectName = __0;
                int buttonType = (int)__1;
                Transform transform = __2;
                float unknown_float = __3;
                bool unkown_bool = __4;

                MelonLogger.Msg("================================");
                MelonLogger.Msg("检测到按钮注册: " + gameObjectName);
                MelonLogger.Msg(__1.ToString());
                MelonLogger.Msg("================================");

                //更新实例引用
                switch (gameObjectName)
                {
                    case "kill":
                        KillCooldownButton.kill = __result;
                        MelonLogger.Msg(System.ConsoleColor.Green, "已通过hook更新kill按钮的GameObject");
                        break;
                    case "avengerKill":
                        AvengerKillCooldownButton.avengerKill = __result;
                        MelonLogger.Msg(System.ConsoleColor.Green, "已通过hook更新avengerKill按钮的GameObject");
                        break;
                    case "infect":
                        InfectCooldownButton.infect = __result;
                        MelonLogger.Msg(System.ConsoleColor.Green, "已通过hook更新infect按钮的GameObject");
                        break;
                    case "investigate":
                        InvestigateCooldownButton.investigate = __result;
                        MelonLogger.Msg(System.ConsoleColor.Green, "已通过hook更新investigate按钮的GameObject");
                        break;
                    case "telepat":
                        TelepatCooldownButton.telepat = __result;
                        MelonLogger.Msg(System.ConsoleColor.Green, "已通过hook更新telepat按钮的GameObject");
                        break;
                }
            }
            catch (System.Exception ex)
            {
                MelonLogger.Warning(ex.Message);
            }
        }
    }

    public enum ButtonType
    {
        Mute,
        PTT,
        Deafen,
        Fart,
        Primary,
        Secondary,
        Tertiary,
        Use,
        Report,
        Map,
        Chat,
        Tasks,
        MoveUp,
        MoveDown,
        MoveLeft,
        MoveRight
    }
}