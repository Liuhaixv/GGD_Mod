using Harmony;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GGD_Hack.GameData
{
    static class IntegrityError
    {
        static Dictionary<int, string> errCodes = new Dictionary<int, string>
        {
            { 0, "Platform was not initialized at the time of connecting"},
            { 32765, "Platform was not initialized at the time of connecting" },
            { 32766, "EasyAntiCheat is not running" },
            { 32767, "Failed to obtain application configuration" },
        };
    }
}
