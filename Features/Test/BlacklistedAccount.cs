using HarmonyLib;
using MelonLoader;
using Photon.Realtime;
using System.Text;

//转圈圈封禁
namespace GGD_Hack.Features.Test
{
    //[HarmonyPatch(typeof(LoadBalancingClient), nameof(LoadBalancingClient.OnOperationResponse))]
    public class BlacklistedAccount
    {
        //[05:36:18.241][[开发者专用版] _Liuhaixv's_GGD_Hack_mod] 接收到事件: AppStats
        //[05:36:18.241][[开发者专用版] _Liuhaixv's_GGD_Hack_mod] Event 226: {(Byte)227=(Int32)771, (Byte)229=(Int32)8920, (Byte)228=(Int32)805}
        static void Prefix(Photon.Realtime.LoadBalancingClient __instance, ExitGames.Client.Photon.OperationResponse __0)
        {
            try
            {
                //账号被拉黑
                if(__0.ReturnCode == 32767)
                {
                    __0.ReturnCode = 0;
                    MelonLogger.Msg(System.ConsoleColor.Green, "账号被拉黑");
                }

                    StringBuilder sb = new StringBuilder();
                sb.AppendLine("--------------------");
                sb.AppendLine("void Photon.Realtime.LoadBalancingClient::OnOperationResponse(ExitGames.Client.Photon.OperationResponse operationResponse)");
                sb.Append("- __instance: ").AppendLine(__instance.ToString());
                sb.Append("- Parameter 0 'operationResponse': ").AppendLine(__0?.ToStringFull() ?? "null");
                MelonLogger.Msg(sb.ToString());
            }
            catch (System.Exception ex)
            {
                MelonLogger.Warning($"Exception in patch of void Photon.Realtime.LoadBalancingClient::OnOperationResponse(ExitGames.Client.Photon.OperationResponse operationResponse):\n{ex}");
            }
        }

    }
}
