using MelonLoader;
using HarmonyLib;
using UnhollowerBaseLib;

using IntPtr = System.IntPtr;
using Photon.Pun;
using System.Text;
using Il2CppSystem.Reflection;


namespace GGD_Hack.Features.Test
{
    public static class RPC
    {
        [PunRPC]
        public static void Suicide()
        {
            UnityMainThreadDispatcher.Instance().Enqueue(new System.Action(() =>
            {
                MelonLogger.Msg(System.ConsoleColor.Green, "RPC:" + "Suicide");
                MiscFunctions.Suicide();
            })); 
        }

        //Void RPC(System.String, Photon.Pun.RpcTarget, UnhollowerBaseLib.Il2CppReferenceArray`1[Il2CppSystem.Object])
        public static void ChangeRoom()
        {
            /*
            Photon.Pun.PhotonView photonView = Handlers.GameHandlers.PlayerHandlers.LocalPlayer.Instance.Player.photonView;
            MelonLogger.Msg(photonView.ToString());
            //photonView.RPC("ChangeCurrentRoom", RpcTarget.AllViaServer,false, "JPV9GCU");

            System.Object[] Objects = new System.Object[2];

            // 获取 Il2CppSystem.Int64 类型的 Type 对象
            Type il2Int64Type = Type.GetType("Il2CppSystem.Int64, il2cpp");

            Objects[0] = (Il2CppObject)Activator.CreateInstance(il2Int64Type, 1);
            Objects[1] = (Il2CppObject)Activator.CreateInstance(il2Int64Type, photonView.ViewID);

            photonView.RPC("Flip", Photon.Pun.RpcTarget.AllViaServer, Objects);
            */
        }

        public static void RPC_Suicide()
        {
            Photon.Pun.PhotonView photonView = Handlers.GameHandlers.PlayerHandlers.LocalPlayer.Instance.gameObject.GetComponent<Photon.Pun.PhotonView>();

            if (photonView == null)
            {
                MelonLogger.Warning("LocalPlayer的PhotonView为空");
                return;
            }

            UnhollowerBaseLib.Il2CppReferenceArray<Il2CppSystem.Object> Objects = new Il2CppReferenceArray<Il2CppSystem.Object>(0);

            MelonLogger.Msg("准备发送Suicide");

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
                            "Suicide",
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
