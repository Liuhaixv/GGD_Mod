using MelonLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//TODO: 移除战争迷雾
//Remove fog of war
namespace GGD_Hack.Features
{
    public class RemoveFOW
    {
        public static MelonPreferences_Entry<bool> Enabled = MelonPreferences.CreateEntry<bool>("GGDH", "Enable_" + nameof(RemoveFOW), true);
    }
}
