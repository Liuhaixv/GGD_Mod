using MelonLoader;
using HarmonyLib;
using Managers;

namespace GGD_Hack.Features.Override
{
    public class MaxChatMessages
    {
        public static MelonPreferences_Entry<int> OverrideMaxChatMessages = MelonPreferences.CreateEntry<int>("GGDH", "Override_" + nameof(MaxChatMessages), 50);
        
        [HarmonyPatch(typeof(MainManager),nameof(MainManager.Awake))]
        class MainManager_Awake
        {
            static void Postfix(MainManager __instance)
            {
                if (OverrideMaxChatMessages.Value > 0)
                {
                    __instance.maxChatMessages = OverrideMaxChatMessages.Value;
                }
            }
        }
    }
}
