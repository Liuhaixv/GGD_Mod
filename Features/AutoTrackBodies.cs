using MelonLoader;


namespace GGD_Hack.Features
{
    public class AutoTrackBodies
    {
#if Legit
        public static MelonPreferences_Entry<bool> Enabled = MelonPreferences.CreateEntry<bool>("GGDH", "Enable_" + nameof(AutoTrackBodies), false);
#else
        public static MelonPreferences_Entry<bool> Enabled = MelonPreferences.CreateEntry<bool>("GGDH","Enable_"+ nameof(AutoTrackBodies), true);
#endif
    }
}
