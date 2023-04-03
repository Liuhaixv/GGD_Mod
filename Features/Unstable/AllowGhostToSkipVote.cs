using APIs.Photon;
using Handlers.GameHandlers.VotingHandlers;
using HarmonyLib;
using Managers;

namespace GGD_Hack.Features
{
    public class AllowGhostToSkipVote
    {
        [HarmonyPatch(typeof(VotePrefabHandler), nameof(VotePrefabHandler.Start))]
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
            static void Postfix(VotePrefabHandler __instance)
            {
                if (!__instance.isSkip)
                {
                    return;
                }

                if(__instance.voteButton.onClick.m_PersistentCalls.Count == 1)
                {
                    UnityEngine.Events.PersistentCall persistentCall = __instance.voteButton.onClick.m_PersistentCalls.m_Calls[0];
                    if(persistentCall.methodName == "Vote")
                    {
                        __instance.voteButton.onClick.RemoveAllListeners();
                        __instance.voteButton.onClick.AddListener(new System.Action(() =>
                        {
                            if (MainManager.Instance.votesManager.hasVoted)
                            {
                                return;
                            }

                            PhotonEventAPI.SendEventToPlugin((byte)GameData.EventDataCode.VOTE, "skip");
                            MainManager.Instance.votesManager.hasVoted = true;
                            __instance.voteButton.interactable = false;
                        }));
                    }
                }

                if (!MainManager.Instance.votesManager.hasVoted)
                    __instance.voteButton.interactable = true;
            }
        }
    }
}
