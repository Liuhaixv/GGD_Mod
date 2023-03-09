#if Developer
using APIs.Photon;
using Handlers.GameHandlers.PlayerHandlers;
using HarmonyLib;
using MelonLoader;

namespace GGD_Hack.Features.Admin
{
    [HarmonyPatch(typeof(PlayerController), nameof(PlayerController.Start))]
    public class AutoPingPlayers
    {
        static void Postfix()
        {
            try
            {
                Utils.RpcCommandsSender.Ping();
            }
            catch (System.Exception ex)
            {
                MelonLogger.Warning($"Exception in patch of void APIs.Photon.PhotonCallbacksAPI::OnPlayerEnteredRoom(Photon.Realtime.Player OGIDFFCPIDI):\n{ex}");
            }
        }
    }
}
#endif