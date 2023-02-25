using BestHTTP;
using HarmonyLib;
using MelonLoader;

namespace GGD_Hack.Features
{
    public static class UnlockAllItems
    {
        public static MelonPreferences_Entry<bool> Enabled;

        static UnlockAllItems()
        {
            if (!MelonPreferences.HasEntry("GGDH", nameof(UnlockAllItems)))
            {
                Enabled = MelonPreferences.CreateEntry<bool>("GGDH", nameof(UnlockAllItems), false);
            }
            else
                Enabled = MelonPreferences.GetEntry<bool>("GGDH", nameof(UnlockAllItems));
        }
    }

    [HarmonyPatch(typeof(HTTPManager), nameof(HTTPManager.SendRequest),typeof(HTTPRequest))]
    class HTTPManager_SendRequest_Patch
    {
        static void Postfix(HTTPRequest request)
        {
            //string a = request.Uri.ToString(); 
            MelonLogger.Msg("HTTPManager.SendRequest: " );
        }
    }
}
