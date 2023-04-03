using Handlers.LobbyHandlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace GGD_Hack.Utils
{
    public class RoleIcon
    {
        public static Sprite GetRoleIcon(int roleID)
        {
            return LobbySceneHandler.Instance?.uIRoleIconHandler.GetRoleIcon((IPLJDOHJOLM)roleID);
        }
    }
}
