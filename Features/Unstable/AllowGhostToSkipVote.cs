using APIs.Photon;
using Handlers.GameHandlers.PlayerHandlers;
using Handlers.GameHandlers.VotingHandlers;
using HarmonyLib;
using Managers;

namespace GGD_Hack.Features
{
    public class AllowGhostToSkipVote
    {
        //[HarmonyPatch(typeof(VotePrefabHandler), nameof(VotePrefabHandler.Start))]
        class ReplaceSkipButtonOnclick
        {
            static void Post(VotePrefabHandler __instance)
            {
                if (!__instance.isSkip)
                {
                    return;
                }

                __instance.voteButton = new UnityEngine.UI.Button();
                __instance.voteButton.interactable = true;
            }
        }

        [HarmonyPatch(typeof(VotePrefabHandler), nameof(VotePrefabHandler.Update))]
        class ForceSkipButtonInteractable
        {
            static void Prefix(VotePrefabHandler __instance)
            {

                if (!__instance.isSkip)
                {
                    return;
                }


                PlayerController player = LocalPlayer.Instance?.Player ?? null;

                if(player == null)
                {
                    return;
                }

                if ((byte)MainManager.Instance.gameManager.gameState == (byte)GameData.GameState.Voting)
                {
                    __instance.voteButton.interactable = true;

                    //投票设置为非幽灵，可以跳过投票
                    player.isGhost = false;
                }else
                {
                    player.isGhost = (player.timeOfDeath > 0);
                }

                /*
                if (__instance.voteButton.onClick.m_PersistentCalls.Count == 1)
                {
                    __instance.voteButton.onClick.AddListener(new System.Action(() =>
                    {
                        PhotonEventAPI.SendEventToPlugin((byte)GameData.EventDataCode.VOTE, "skip");
                    }));
                }*/
            }
        }
    }
}
