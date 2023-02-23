
using Handlers.GameHandlers.PlayerHandlers;
using Handlers.GameHandlers.SpecialBehaviour;
using MelonLoader;
using UnhollowerRuntimeLib;
using UnityEngine;
using UnityEngine.UI;
using Exception = System.Exception;
using IntPtr = System.IntPtr;

namespace GGD_Hack.Features
{
    [RegisterTypeInIl2Cpp]
    public class MinimapTeleport : MonoBehaviour
    {
        private static MinimapTeleport Instance;
        public MinimapTeleport(IntPtr ptr) : base(ptr) { }

        // Optional, only used in case you want to instantiate this class in the mono-side
        // Don't use this on MonoBehaviours / Components!
        public MinimapTeleport() : base(ClassInjector.DerivedConstructorPointer<MinimapTeleport>()) => ClassInjector.DerivedConstructorBody(this);

        public static void Init()
        {
            GameObject ML_Manager = GameObject.Find("ML_Manager");
            if (ML_Manager == null)
            {
                ML_Manager = new GameObject("ML_Manager");
                DontDestroyOnLoad(ML_Manager);
            }

            if (ML_Manager.GetComponent<MinimapTeleport>() == null)
            {
                Instance = ML_Manager.AddComponent<MinimapTeleport>();
            }
        }

        private bool GetMouseLocalPositionOnMinimapPanel(out Vector2 localPosition)
        {
            localPosition = Vector2.zero;

            try
            {
                GameObject panel = Utils.GameInstances.FindGameObjectByPath("Canvas/MiniMap/Panel");

                if (panel == null)                    return false;

                Image image = panel.GetComponent<Image>();

                // 获取鼠标点击的屏幕坐标
                Vector2 mousePos = UnityEngine.Input.mousePosition;

                // 将屏幕坐标转换为相对于Image组件的坐标
                RectTransform rectTransform = image.GetComponent<RectTransform>();

                if (RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, mousePos, null, out localPosition))
                {
                    MelonLogger.Msg("Click position relative to MiniMap panel's image component: " + localPosition.x + ", " + localPosition.y);

                    return true;
                }
            }
            catch (Exception ex)
            {
                MelonLogger.Warning(ex.Message);
            }

            return false;
        }

        private bool LocalPositionOnMinimapToPosition(in Vector2 localPosition, out Vector3 position)
        {
            position = Vector3.zero;    

            MiniMapHandler miniMapHandler = MinimapESP.miniMapHandler;

            if (miniMapHandler == null) return false;

            position = new Vector3(
                (float)(localPosition.x - miniMapHandler.xOffset) / miniMapHandler.xFactor,
                (float)(localPosition.y - miniMapHandler.yOffset) / miniMapHandler.yFactor,
                0);

            return true;
        }

        /// <summary>
        /// 判断鼠标是否被点击并传送
        /// </summary>
        private void Update()
        {

            if (Input.GetMouseButtonDown(0))
            {
                //小地图未显示
                if (!GameObject.Find("Canvas/MiniMap")) return;

                Vector2 mouseLocalPosition;
                Vector3 targetTeleportPosition;

                if (!GetMouseLocalPositionOnMinimapPanel(out mouseLocalPosition)) return;

                if (!LocalPositionOnMinimapToPosition(mouseLocalPosition, out targetTeleportPosition)) return;

                //传送
                LocalPlayer.Instance.transform.position = targetTeleportPosition;
            }
        }
    }
}
