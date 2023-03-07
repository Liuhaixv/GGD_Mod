using UnityEngine;
namespace GGD_Hack.Features.Test
{
#if false
    internal class Class1
    {
        public static void Pe()
        {
            float newRadius = 500f; // 圆形的半径为2个单位

            Handlers.GameHandlers.PlayerHandlers.PlayerController player = Handlers.GameHandlers.PlayerHandlers.LocalPlayer.Instance.Player;
            
            //使用的范围修改
            //player.bodyCollider.size = new Vector2(newRadius , newRadius); // 在水平和垂直方向上分别伸展2倍半径
            
            player.size = new Vector2(newRadius , newRadius );
        }
    }
#endif
}
