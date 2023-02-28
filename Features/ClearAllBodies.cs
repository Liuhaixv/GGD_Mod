using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

//将所有尸体扔出地图外
namespace GGD_Hack.Features
{
    public class ClearAllBodies
    {
        //将所有尸体扔到地图外
        public static void ThrowAllBodiesAwayFromMap()
        {
            DragAllBodies.MoveAllBodiesTo(new Vector3(666.0f, 777.0f, 888.0f), true);
        }
    }
}
