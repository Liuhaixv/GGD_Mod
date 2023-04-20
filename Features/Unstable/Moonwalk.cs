using MelonLoader;
using Photon.Pun;
using UnhollowerRuntimeLib;
using UnityEngine;
using HarmonyLib;

using IntPtr = System.IntPtr;
using System.Text;
using UnhollowerBaseLib;
using Handlers.GameHandlers.PlayerHandlers;

//主要测试关于Object的封装问题
//以及RPC
namespace GGD_Hack.Features
{
    [RegisterTypeInIl2Cpp]
    public class Moonwalk : MonoBehaviour
    {
        public static Moonwalk Instance;
        public static MelonPreferences_Entry<bool> Enabled = MelonPreferences.CreateEntry<bool>("GGDH", "Enable_" + nameof(Moonwalk), false);
        public static bool lastFrameMovingRight = false;
        public static bool movingRight = false;

        public Moonwalk(IntPtr ptr) : base(ptr)
        {
            IngameSettings.AddIngameSettingsEntry(
                               new IngameSettings.IngameSettingsEntry()
                               {
                                   entry = Enabled,
                                   name_cn = "太空步",
                                   name_eng = "Moonwalk"
                               }
                                          );
        }

        public Moonwalk() : base(ClassInjector.DerivedConstructorPointer<Moonwalk>()) => ClassInjector.DerivedConstructorBody(this);
        public static void Init()
        {
            GameObject ML_Manager = GameObject.Find("ML_Manager");
            if (ML_Manager == null)
            {
                ML_Manager = new GameObject("ML_Manager");
                DontDestroyOnLoad(ML_Manager);
            }

            if (ML_Manager.GetComponent<Moonwalk>() == null)
            {
                Instance = ML_Manager.AddComponent<Moonwalk>();
            }
        }

        //从右向左转
        private static void OnChangeDirection(bool turnRight)
        {
            bool shouldTurnRight = turnRight;
            shouldTurnRight = !shouldTurnRight;
            //发送伪装的RPC,朝右转
            //获取玩家朝向
            Photon.Pun.PhotonView photonView = Handlers.GameHandlers.PlayerHandlers.LocalPlayer.Instance.gameObject.GetComponent<Photon.Pun.PhotonView>();

            if (photonView == null)
            {
                MelonLogger.Warning("LocalPlayer的PhotonView为空");
                return;
            }

            UnhollowerBaseLib.Il2CppReferenceArray<Il2CppSystem.Object> Objects = new Il2CppReferenceArray<Il2CppSystem.Object>(2);
            Objects[0] = new Il2CppSystem.Int32 { m_value = shouldTurnRight ? 1 : 0 }.BoxIl2CppObject();
            Objects[1] = new Il2CppSystem.Int32 { m_value = photonView.ViewID }.BoxIl2CppObject();
#if Developer
            MelonLogger.Msg("准备发送Flip");
#endif
            try
            {
                string rpcInfo = "暂未获取rpc方法";

                System.Reflection.MethodInfo rpc = AccessTools.Method(typeof(PhotonView), "RPC",
                new System.Type[] {
                    typeof(string), typeof(Photon.Pun.RpcTarget), typeof(UnhollowerBaseLib.Il2CppReferenceArray<Il2CppSystem.Object>)
                });

                if (rpc != null)
                {
                    rpcInfo = rpc.ToString();
                    MelonLogger.Msg(System.ConsoleColor.Green, rpcInfo);

                    try
                    {
                        System.Object[] parameters = new object[]
                        {
                            "Flip",
                            Photon.Pun.RpcTarget.AllViaServer,
                            Objects
                        };

                        rpc.Invoke(photonView, parameters);
                        MelonLogger.Msg(System.ConsoleColor.Green, "Invoke成功");
                    }
                    catch (System.Exception ex)
                    {

                        MelonLogger.Error("Invoke失败" + ex.Message);
                    }
                }
            }
            catch (System.Exception e)
            {
                MelonLogger.Error(e.Message);
            }
        }

        [HarmonyPatch(typeof(LocalPlayer),nameof(LocalPlayer.MoveTowards),typeof(Vector3))]
        class PlayerController_MoveTowards
        {
            static void Prefix(LocalPlayer __instance, Vector3 __0)
            {
                if(!Enabled.Value) { return; }

                movingRight = __0.x - LocalPlayer.Instance.gameObject.transform.position.x > 0;
                if(movingRight != lastFrameMovingRight)
                {
                    OnChangeDirection(movingRight);
                }
                lastFrameMovingRight = movingRight;
            }
        }

        //2.20.00
        //被LocalPlayer$$MoveTowards调用
        //40 56 57 48 83 EC 38 80 3D ?? ?? ?? ?? ?? 48 8B F9 0F B6 F2 75 37 48 8D 0D ?? ?? ?? ?? E8 ?? ?? ?? ?? 48 8D 0D ?? ?? ?? ?? E8 ?? ?? ?? ?? 48 8D 0D ?? ?? ?? ?? E8 ?? ?? ?? ?? 48 8D 0D ?? ?? ?? ?? E8 ?? ?? ?? ?? C6 05 ?? ?? ?? ?? ?? 45 33 C0 40 0F B6 D6 48 8B CF E8 ?? ?? ?? ?? 48 8B 0D ?? ?? ?? ?? 83 B9 E0 00 00 00 00 75 05 E8 ?? ?? ?? ?? 33 C9 E8 ?? ?? ?? ?? 84 C0 0F 84 F5 00 00 00 48 8B 0D ?? ?? ?? ?? BA 02
        [HarmonyPatch(typeof(PlayerController), nameof(PlayerController.OAHMONBAHDN), typeof(bool))]
        class PlayerController_Flip
        {
            static bool Prefix(LocalPlayer __instance, ref bool __0)
            {
                if (Enabled.Value)
                {
                    return false;
                }
                else {
                    return true;
                }
            }
        }

        /*
        [HarmonyPatch(typeof(PhotonView), nameof(PhotonView.RPC), new System.Type[] { typeof(string), typeof(RpcTarget), typeof(UnhollowerBaseLib.Il2CppReferenceArray<Il2CppSystem.Object>) })]
        class RPC_
        {
            static bool Prefix(Photon.Pun.PhotonView __instance, string __0, Photon.Pun.RpcTarget __1, ref UnhollowerBaseLib.Il2CppReferenceArray<Il2CppSystem.Object> __2)
            {
                if (!Enabled.Value) return true;

                try
                {
                    if (__0 != "Flip")
                    {
                        return true;
                    }

                    if (!allowFlip)
                    {
                        return false;
                    }
                    else { return true; }
                   
                bool facingRight = LocalPlayer.Instance.Player.facingRight;

                __2[0] = new Il2CppSystem.Int32 { m_value = (LocalPlayer.Instance.Player.facingRight ? 1 : 0) }.BoxIl2CppObject();

                MelonLogger.Msg(System.ConsoleColor.Green, "朝向修改为:" + (facingRight ? 1 : 0));
                }
                catch (System.Exception ex)
                {
                    //UnityExplorer.ExplorerCore.LogWarning($"Exception in patch of void Photon.Pun.PhotonView::RPC(string methodName, Photon.Pun.RpcTarget target, UnhollowerBaseLib.Il2CppReferenceArray<Il2CppSystem.Object> parameters):\n{ex}");
                    MelonLogger.Msg($"Exception in patch of void Photon.Pun.PhotonView::RPC(string methodName, Photon.Pun.RpcTarget target, UnhollowerBaseLib.Il2CppReferenceArray<Il2CppSystem.Object> parameters):\n{ex}");

                }
                return true;
            }
        }*/
    }
}
