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

        //40 56 57 48 83 EC 38 80 3D ?? ?? ?? ?? ?? 48 8B F9 0F
        /*[HarmonyPatch(typeof(LocalPlayer), nameof(LocalPlayer.MoveTowards), typeof(Vector3))]
        class PlayerController_Flip
        {
            static void Prefix(LocalPlayer __instance, Vector3 __0)
            {
                if (Enabled.Value)
                {
                    bool movingRight = __0.x - LocalPlayer.Instance.gameObject.transform.position.x > 0;
                    MelonLogger.Msg("移动方向:{0}", movingRight ? "右" : "左");

                    if(movingRight)
                    {
                        if(LocalPlayer.Instance.Player.facingRight == true)
                        {
                            //40 56 57 48 83 EC 38 80 3D ?? ?? ?? ?? ?? 48 8B F9 0F
                            LocalPlayer.Instance.Player.MMHJKMIPBGE(false);
                        }
                    } else
                    {
                        if (LocalPlayer.Instance.Player.facingRight == false)
                        {
                            //40 56 57 48 83 EC 38 80 3D ?? ?? ?? ?? ?? 48 8B F9 0F
                            LocalPlayer.Instance.Player.MMHJKMIPBGE(true);
                        }
                    }
                }
            }
        }*/

        /*
        [HarmonyPatch(typeof(PhotonView), nameof(PhotonView.RPC), new System.Type[] { typeof(string), typeof(RpcTarget), typeof(UnhollowerBaseLib.Il2CppReferenceArray<Il2CppSystem.Object>) })]
        class RPC_
        {
            static void Prefix(Photon.Pun.PhotonView __instance, string __0, Photon.Pun.RpcTarget __1, ref UnhollowerBaseLib.Il2CppReferenceArray<Il2CppSystem.Object> __2)
            {
                if (!Enabled.Value) return;

                try
                {
                    if (__0 != "Flip")
                    {
                        return;
                    }

                    bool facingRight = LocalPlayer.Instance.Player.facingRight;

                    __2[0] = new Il2CppSystem.Int32 { m_value = (LocalPlayer.Instance.Player.facingRight ? 1 : 0) }.BoxIl2CppObject();

                    MelonLogger.Msg(System.ConsoleColor.Green, "朝向修改为:" + (facingRight ? 1 : 0));
                }
                catch (System.Exception ex)
                {
                    //UnityExplorer.ExplorerCore.LogWarning($"Exception in patch of void Photon.Pun.PhotonView::RPC(string methodName, Photon.Pun.RpcTarget target, UnhollowerBaseLib.Il2CppReferenceArray<Il2CppSystem.Object> parameters):\n{ex}");
                    MelonLogger.Msg($"Exception in patch of void Photon.Pun.PhotonView::RPC(string methodName, Photon.Pun.RpcTarget target, UnhollowerBaseLib.Il2CppReferenceArray<Il2CppSystem.Object> parameters):\n{ex}");

                }
            }
        }*/
    }
}
