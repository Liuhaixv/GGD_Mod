using MelonLoader;


namespace GGD_Hack.Features
{
    public class AutoTrackBodies
    {
        public static MelonPreferences_Entry<bool> Enabled = MelonPreferences.CreateEntry<bool>("GGDH", nameof(AutoTrackBodies), true);
    }
}
