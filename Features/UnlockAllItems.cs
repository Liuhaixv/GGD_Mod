using MelonLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
}
