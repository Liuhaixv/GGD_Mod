
using MelonLoader;
using Photon.Pun;
using System;
using UnhollowerBaseLib;

using IntPtr = System.IntPtr;

namespace GGD_Hack.Features.Test
{
    public class RPC
    {/*
        unsafe public static void ChangeRoom()
        {
            Photon.Pun.PhotonView photonView = Handlers.GameHandlers.PlayerHandlers.LocalPlayer.Instance.Player.photonView;
            MelonLogger.Msg(photonView.ToString());
            //photonView.RPC("ChangeCurrentRoom", RpcTarget.AllViaServer,false, "JPV9GCU");

            System.Object[] Objects = new System.Object[2];

            // 获取 Il2CppSystem.Int64 类型的 Type 对象
            Type il2Int64Type = Type.GetType("Il2CppSystem.Int64, il2cpp");

            Objects[0] = (Il2CppObject)Activator.CreateInstance(il2Int64Type, 1);
            Objects[1] = (Il2CppObject)Activator.CreateInstance(il2Int64Type, photonView.ViewID);

            photonView.RPC("Flip", Photon.Pun.RpcTarget.AllViaServer, Objects);
        }

        public static void Flip(bool facingRight = true)
        {
            Photon.Pun.PhotonView photonView = Handlers.GameHandlers.PlayerHandlers.LocalPlayer.Instance.gameObject.GetComponent<Photon.Pun.PhotonView>();
            MelonLogger.Msg(photonView.ToString());

            //Il2CppSystem.Object[] Objects = new Il2CppSystem.Object[2];
            //UnhollowerBaseLib.Il2CppReferenceArray<Il2CppSystem.Object> Objects = new UnhollowerBaseLib.Il2CppReferenceArray<Il2CppSystem.Object>(2);
            //System.Object[] Objects= new System.Object[2];
            Il2CppSystem.Array arrayObjNew = Il2CppSystem.Array.CreateInstance(Il2CppSystem.Int64.Il2CppType, 2);
            
            Il2CppSystem.Int64 right = new Il2CppSystem.Int64(): m
            Il2CppSystem.Int64 viewId = new Il2CppSystem.Int64();

            arrayObjNew.SetValue(new Il2CppSystem.Int64(1), 1);

            {
                int intValue = 6;
                Il2CppSystem.Object objValue = (Il2CppSystem.Object)Il2CppSystem.Int64.Box(intValue);

            }


            photonView.RPC("Flip", Photon.Pun.RpcTarget.AllViaServer, Objects);
        }*/
    }
}
