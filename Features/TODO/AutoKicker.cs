using Handlers.LobbyHandlers;
using MelonLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnhollowerRuntimeLib;
using UnityEngine;

//根据名称自动踢出玩家
namespace GGD_Hack.Features
{
    [RegisterTypeInIl2Cpp]
    public class AutoKicker : MonoBehaviour
    {
        public static MelonPreferences_Entry<bool> Enabled = MelonPreferences.CreateEntry<bool>("GGDH", "Enable_" + nameof(AutoKicker), true);
        public static AutoKicker Instance = null;

        private List<string> rules = new List<string>();
        private string rulePath = "";//TODO

        public AutoKicker(IntPtr ptr) : base(ptr) { }

        // Optional, only used in case you want to instantiate this class in the mono-side
        // Don't use this on MonoBehaviours / Components!
        public AutoKicker() : base(ClassInjector.DerivedConstructorPointer<AutoKicker>()) => ClassInjector.DerivedConstructorBody(this);

        public static void Init()
        {
            GameObject ML_Manager = GameObject.Find("ML_Manager");
            if (ML_Manager == null)
            {
                ML_Manager = new GameObject("ML_Manager");
                DontDestroyOnLoad(ML_Manager);
            }

            if (ML_Manager.GetComponent<MinimapESP>() == null)
            {
                Instance = ML_Manager.AddComponent<AutoKicker>();
            }
        }

        private void Start()
        {

        }

        /// <summary>
        /// 更新所有gameObjects的坐标，相对于LocalPlayer
        /// </summary>
        private void Update()
        {
            if (!Enabled.Value)
            {
                return;
            }

            //判断是否在房间内
            //if(LobbySceneHandler.Instance.)

            //判断自己是否为房主


        }
    }
}
