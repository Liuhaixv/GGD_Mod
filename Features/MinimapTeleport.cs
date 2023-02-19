using Handlers.GameHandlers.PlayerHandlers;
using Handlers.GameHandlers.SpecialBehaviour;
using MelonLoader;
using System;
using UnhollowerRuntimeLib;
using UnityEngine;
using UnityEngine.UI;
using static MelonLoader.MelonLogger;

namespace GGD_Hack.Features
{
    [RegisterTypeInIl2Cpp]
    public class MinimapTeleport : MonoBehaviour
    {
        private static MinimapTeleport instance;
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
                instance = ML_Manager.AddComponent<MinimapTeleport>();
            }
        }

        private void Teleport()
        {

        }

        private Vector2? GetMouseLocalPositionOnMinimapPanel()
        {
            Vector2 localPoint;

            try
            {
                GameObject panel = Utils.FindGameObjectByPath("Canvas/MiniMap/Panel");

                if (panel == null) return null;

                Image image = panel.GetComponent<Image>();

                // 获取鼠标点击的屏幕坐标
                Vector2 mousePos = UnityEngine.Input.mousePosition;

                // 将屏幕坐标转换为相对于Image组件的坐标
                RectTransform rectTransform = image.GetComponent<RectTransform>();

                if (RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, mousePos, null, out localPoint))
                {
                    MelonLogger.Msg("Click position relative to MiniMap panel's image component: " + localPoint.x + ", " + localPoint.y);
                    return localPoint;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                MelonLogger.Warning(ex.Message);
                return null;
            }
        }

        private Vector3? localPositionOnMinimapToPosition(Vector2 localPosition)
        {
            MiniMapHandler miniMapHandler = MinimapESP.miniMapHandler;

            if (miniMapHandler == null) return null;

            Vector3 vector3 = new Vector3(
                (float)(localPosition.x - miniMapHandler.xOffset) / miniMapHandler.xFactor,
                (float)(localPosition.y - miniMapHandler.yOffset) / miniMapHandler.yFactor,
                0);

            return vector3;
        }

        /// <summary>
        /// 判断鼠标是否被点击并传送
        /// </summary>
        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector2? mouseLocalPosition = GetMouseLocalPositionOnMinimapPanel();

                if (!mouseLocalPosition.HasValue) return;

                Vector3? targetTeleportPosition = localPositionOnMinimapToPosition(mouseLocalPosition.Value);

                if (!targetTeleportPosition.HasValue) return;

                //传送
                LocalPlayer.Instance.transform.position = targetTeleportPosition.Value;
            }
        }
    }
}
