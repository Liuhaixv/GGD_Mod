using APIs.Photon;
using Handlers.GameHandlers.PlayerHandlers;
using Handlers.GameHandlers.VotingHandlers;
using HarmonyLib;
using Managers;

namespace GGD_Hack.Features
{
    public class AllowGhostToSkipVote
    {
        [HarmonyPatch(typeof(VotePrefabHandler), nameof(VotePrefabHandler.Update))]
        class ForceSkipButtonInteractable
        {
            static void Prefix(ref bool __state)
            {                
                if(LocalPlayer.Instance?.Player == null) return;

                __state = LocalPlayer.Instance.Player.isGhost;

                if ((byte)MainManager.Instance.gameManager.gameState != (byte)GameData.GameState.Voting)
                {
                    return;
                }

                //暂时改为非幽灵
                LocalPlayer.Instance.Player.isGhost = false;
            }

            static void Postfix(ref bool __state)
            {
                if (LocalPlayer.Instance?.Player == null) return;

                //恢复原先的幽灵状态
                LocalPlayer.Instance.Player.isGhost = __state;
            }
        }
    }
}
