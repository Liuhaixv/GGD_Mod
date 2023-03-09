using MelonLoader;
using HarmonyLib;
using UnhollowerBaseLib;

using IntPtr = System.IntPtr;
using Photon.Pun;
using System.Text;
using Il2CppSystem.Reflection;
using System;

namespace GGD_Hack.Features.Test
{
    public static class RPC
    {
        //可以传递最大长度32个Unicode字符，也就是32*2=64个字节
        public static void ChangeCurrentRoom(string roomId, Photon.Pun.RpcTarget rpcTarget = Photon.Pun.RpcTarget.AllViaServer)
        {
            //获取玩家朝向
            Photon.Pun.PhotonView photonView = Handlers.GameHandlers.PlayerHandlers.LocalPlayer.Instance.gameObject.GetComponent<Photon.Pun.PhotonView>();

            if (photonView == null)
            {
                MelonLogger.Warning("LocalPlayer的PhotonView为空");
                return;
            }

            UnhollowerBaseLib.Il2CppReferenceArray<Il2CppSystem.Object> Objects = new Il2CppReferenceArray<Il2CppSystem.Object>(1);
            Objects[0] = roomId;

            MelonLogger.Msg("准备发送ChangeCurrentRoom");

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
                            "ChangeCurrentRoom",
                            rpcTarget,
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

        public static void Fake_Flip(int code)
        {
            //获取玩家朝向
            Photon.Pun.PhotonView photonView = Handlers.GameHandlers.PlayerHandlers.LocalPlayer.Instance.gameObject.GetComponent<Photon.Pun.PhotonView>();

            if (photonView == null)
            {
                MelonLogger.Warning("LocalPlayer的PhotonView为空");
                return;
            }

            UnhollowerBaseLib.Il2CppReferenceArray<Il2CppSystem.Object> Objects = new Il2CppReferenceArray<Il2CppSystem.Object>(3);
            Objects[0] = new Il2CppSystem.Int32 { m_value = code }.BoxIl2CppObject();
            Objects[1] = new Il2CppSystem.Int32 { m_value = photonView.ViewID }.BoxIl2CppObject();
            Objects[2] = new Il2CppSystem.Int32 { m_value = 10101}.BoxIl2CppObject();

            MelonLogger.Msg("准备发送Flip");

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

        public static void Flip()
        {
            //获取玩家朝向
            bool facingRight = Handlers.GameHandlers.PlayerHandlers.LocalPlayer.Instance.Player.facingRight;
            Photon.Pun.PhotonView photonView = Handlers.GameHandlers.PlayerHandlers.LocalPlayer.Instance.gameObject.GetComponent<Photon.Pun.PhotonView>();

            if (photonView == null)
            {
                MelonLogger.Warning("LocalPlayer的PhotonView为空");
                return;
            }

            UnhollowerBaseLib.Il2CppReferenceArray<Il2CppSystem.Object> Objects = new Il2CppReferenceArray<Il2CppSystem.Object>(2);
            Objects[0] = new Il2CppSystem.Int32 { m_value = facingRight ? 0 : 1 }.BoxIl2CppObject();
            Objects[1] = new Il2CppSystem.Int32 { m_value = photonView.ViewID }.BoxIl2CppObject();

            MelonLogger.Msg("准备发送Flip");

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
#if Developer
        /*
        //接收到RPC请求
        [HarmonyPatch(typeof(PhotonNetwork), "ExecuteRpc")]
        class ExecuteRpc_
        {
            static void Postfix(ExitGames.Client.Photon.Hashtable __0, Photon.Realtime.Player __1)
            {
                try
                {
                    //UnityExplorer.CSConsole.ScriptInteraction.Copy(__0);
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine("--------------------");
                    sb.AppendLine("static void Photon.Pun.PhotonNetwork::ExecuteRpc(ExitGames.Client.Photon.Hashtable rpcData, Photon.Realtime.Player sender)");
                    sb.Append("- Parameter 0 'rpcData': ").AppendLine(__0?.ToString() ?? "null");
                    sb.Append("- Parameter 1 'sender': ").AppendLine(__1?.ToString() ?? "null");
                    MelonLogger.Msg(sb.ToString());
                }
                catch (System.Exception ex)
                {
                    MelonLogger.Warning($"Exception in patch of static void Photon.Pun.PhotonNetwork::ExecuteRpc(ExitGames.Client.Photon.Hashtable rpcData, Photon.Realtime.Player sender):\n{ex}");
                }
            }
        }
        */

        [HarmonyPatch(typeof(PhotonView), nameof(PhotonView.RPC), new System.Type[] { typeof(string), typeof(RpcTarget), typeof(UnhollowerBaseLib.Il2CppReferenceArray<Il2CppSystem.Object>) })]
        class RPC_
        {
            static void Postfix(MethodBase __originalMethod, Photon.Pun.PhotonView __instance, string __0, Photon.Pun.RpcTarget __1, UnhollowerBaseLib.Il2CppReferenceArray<Il2CppSystem.Object> __2)
            {
                try
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine("--------------------");
                    sb.AppendLine("void Photon.Pun.PhotonView::RPC(string methodName, Photon.Pun.RpcTarget target, UnhollowerBaseLib.Il2CppReferenceArray<Il2CppSystem.Object> parameters)");
                    sb.Append("- __instance: ").AppendLine(__instance.ToString());
                    sb.Append("- Parameter 0 'methodName': ").AppendLine(__0?.ToString() ?? "null");
                    sb.Append("- Parameter 1 'target': ").AppendLine(__1.ToString());
                    sb.Append("- Parameter 2 'parameters': ").AppendLine(__2?.ToString() ?? "null");

                    MelonLogger.Msg(sb.AppendLine("参数数量:" + __2.Length));
                }
                catch (System.Exception ex)
                {
                    //UnityExplorer.ExplorerCore.LogWarning($"Exception in patch of void Photon.Pun.PhotonView::RPC(string methodName, Photon.Pun.RpcTarget target, UnhollowerBaseLib.Il2CppReferenceArray<Il2CppSystem.Object> parameters):\n{ex}");
                    MelonLogger.Msg($"Exception in patch of void Photon.Pun.PhotonView::RPC(string methodName, Photon.Pun.RpcTarget target, UnhollowerBaseLib.Il2CppReferenceArray<Il2CppSystem.Object> parameters):\n{ex}");

                }
            }

        }
#endif
    }
}
