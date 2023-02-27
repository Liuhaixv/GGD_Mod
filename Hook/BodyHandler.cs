using Handlers.GameHandlers;
using static MelonLoader.MelonLogger;
using UnityEngine;
using MelonLoader;
using HarmonyLib;
using Handlers.GameHandlers.PlayerHandlers;

namespace GGD_Hack.Hook
{
    /// <summary>
    /// 屏蔽吃尸体瞬移、位移
    /// </summary>
    public class BodyHandler_
    {
        /// <summary>
        /// 
        /// </summary>

        [HarmonyPatch(typeof(BodyHandler), nameof(BodyHandler.GetNearestValidLocation))]
        public class GetNearestValidLocation_
        {
            [HarmonyPrefix]
            public static bool Prefix(ref Vector2 __result)
            {
                Vector3 position = LocalPlayer.Instance.transform.position;

                __result = new Vector2(position.x, position.y);

                return false;
            }
        }

        [HarmonyPatch(typeof(BodyHandler), nameof(BodyHandler.AMFDOIDPDKK))]
        public class AMFDOIDPDKK_
        {
            [HarmonyPrefix]
            public static bool Prefix(ref Vector2 __result)
            {
                Vector3 position = LocalPlayer.Instance.transform.position;

                __result = new Vector2(position.x, position.y);

                return false;
            }
        }

        [HarmonyPatch(typeof(BodyHandler), nameof(BodyHandler.HFHGEKGODNG))]
        public class HFHGEKGODNG_
        {
            [HarmonyPrefix]
            public static bool Prefix(ref Vector2 __result)
            {
                Vector3 position = LocalPlayer.Instance.transform.position;

                __result = new Vector2(position.x, position.y);

                return false;
            }
        }

        [HarmonyPatch(typeof(BodyHandler), nameof(BodyHandler.IJECFDHCFIN))]
        public class IJECFDHCFIN_
        {
            [HarmonyPrefix]
            public static bool Prefix(ref Vector2 __result)
            {
                Vector3 position = LocalPlayer.Instance.transform.position;

                __result = new Vector2(position.x, position.y);

                return false;
            }
        }
    }
}