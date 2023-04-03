using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GGD_Hack.Features
{
    public class AutoReady
    {
        public static void Ready()
        {
            Handlers.LobbyHandlers.LobbySceneHandler.Instance.ChangeReadyState();
        }
    }
}
