using Handlers.LobbyHandlers;
using MelonLoader;
using HarmonyLib;
using TMPro;
using UnityEngine;
using Handlers.GameHandlers.PlayerHandlers;
using Cinemachine;
using UnityEngine.UI;

namespace GGD_Hack.Features
{
    public class ZoomSlider
    {
        public static UnityEngine.UI.Slider zoomSlider = null;

        //创建Slider滑块
        public static void CreateSlider()
        {
            //获取要克隆的Slider对象
            UnityEngine.GameObject masterVolumeGameObject = LobbySceneHandler.Instance.clientSettings.masterVolume.gameObject;           
           
            if(masterVolumeGameObject == null ) {
                MelonLogger.Warning("masterVolumeGameObject为空！");
                return;
            }

            //获取身份图标的transform
            Transform roleIconTransform = LobbySceneHandler.Instance.gamePanel.transform.Find("RoleIcon");

            //克隆对象
            GameObject zoomSliderGameObject = GameObject.Instantiate(masterVolumeGameObject, roleIconTransform);
            zoomSliderGameObject.name = "ZoomSlider";
            zoomSliderGameObject.transform.localPosition = new Vector3(300, 40, 0);
            zoomSliderGameObject.transform.localScale = new Vector3(0.5f, 0.5f, 1);

            //修改文字
            TextMeshProUGUI sliderText = zoomSliderGameObject.transform.Find("Label").gameObject.GetComponent<TextMeshProUGUI>();
            sliderText.text = Utils.Utils.IsChineseSystem() ? "视野缩放" : "Zoom Size";
            sliderText.ForceMeshUpdate();

            //删除数字
            GameObject.Destroy(zoomSliderGameObject.transform.Find("Value").gameObject);
            GameObject.Destroy(zoomSliderGameObject.GetComponent<GamepadButtonSelector>());
            GameObject.Destroy(zoomSliderGameObject.GetComponent<SliderGamepadHandler>());

            //添加Slider绑定的函数
            zoomSlider = zoomSliderGameObject.GetComponent<UnityEngine.UI.Slider>();

            zoomSlider.onValueChanged.RemoveAllListeners();

            zoomSlider.minValue = 4.0f;
            zoomSlider.maxValue = 40.0f;
            zoomSlider.value = 4.0f;

            zoomSlider.onValueChanged.AddListener(new System.Action<float>((v) =>
            {
                ChangeOrthographicSize(zoomSlider.value);
            }));
        }

        //修改视野范围
        private static void ChangeOrthographicSize(float newValue)
        {
            Cinemachine.CinemachineStateDrivenCamera stateDrivenCamera = LocalPlayer.Instance.PIILFGEKDHA;
            Cinemachine.CinemachineVirtualCamera virtualCamera = LocalPlayer.Instance.IABFCECPODJ;

            if(stateDrivenCamera.m_AnimatedTarget != null)
            {
                stateDrivenCamera.m_AnimatedTarget = null;
            }

            LensSettings newLens = virtualCamera.m_Lens;
            newLens.OrthographicSize = newValue;

            virtualCamera.m_Lens = newLens;

           MelonLogger.Msg(System.ConsoleColor.Green, "新的视野缩放数值" + newValue);
        }

        [HarmonyPatch(typeof(Handlers.LobbyHandlers.LobbySceneHandler), nameof(Handlers.LobbyHandlers.LobbySceneHandler.Start))]
        class InitSlider
        {
            static void Postfix()
            {
                CreateSlider();
            }
        }

        [HarmonyPatch(typeof(Handlers.LobbyHandlers.LobbySceneHandler), nameof(Handlers.LobbyHandlers.LobbySceneHandler.Update))]
        class UpdateSliderName
        {
            static void Postfix()
            {
                //修改文字
                TextMeshProUGUI sliderText = zoomSlider.gameObject.transform.Find("Label").gameObject.GetComponent<TextMeshProUGUI>();
                sliderText.text = Utils.Utils.IsChineseSystem() ? "视野缩放" : "Zoom Size";
            }
        }
    }
}
