using ExitGames.Client.Photon;
using Handlers.GameHandlers.PlayerHandlers;
using HarmonyLib;
using Managers;
using Managers.ConnectionManagers;

namespace GGD_Hack.Features
{
    //40 53 57 41 55 48 83 EC 40 80 3D ?? ?? ?? ?? ?? 48 8B DA 4C 8B E9 0F 85 3F
    public class AllowToSeeDeadPlayersChatMessages
    {
        //该函数中会判断是否是幽灵，然后显示死亡玩家的消息
        [HarmonyPatch(typeof(ChatManager), nameof(ChatManager.IKMLHFKHHDD))]
        class OnReceivedMessage
        {
            static void Prefix(ref bool __state)
            {
                if ((byte)MainManager.Instance.gameManager.gameState != (byte)GameData.GameState.Voting)
                {
                    return;
                }

                __state = LocalPlayer.Instance.Player.isGhost;
                //非幽灵
                if (!__state)
                {
                    //暂时改为幽灵
                    LocalPlayer.Instance.Player.isGhost = true;
                }
            }

            static void Postfix(ref bool __state)
            {
                if ((byte)MainManager.Instance.gameManager.gameState != (byte)GameData.GameState.Voting)
                {
                    return;
                }

                //恢复原先的幽灵状态
                LocalPlayer.Instance.Player.isGhost = __state;
            }
        }
    }
}
