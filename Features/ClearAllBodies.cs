using UnityEngine;

//将所有尸体扔出地图外
namespace GGD_Hack.Features
{
    public class ClearAllBodies
    {
        //将所有尸体扔到地图外
        public static void ThrowAllBodiesAwayFromMap()
        {
            DragAllBodies.MoveAllBodiesTo(new Vector3(0.0f, 0.0f, 0.0f));
        }
    }
}
