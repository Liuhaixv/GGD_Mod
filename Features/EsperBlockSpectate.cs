using MelonLoader;
using UnhollowerRuntimeLib;
using UnityEngine;
using HarmonyLib;

using IntPtr = System.IntPtr;
using APIs.Photon;
using GGD_Hack.GameData;
using System;

namespace GGD_Hack.Features
{
    [RegisterTypeInIl2Cpp]
    public class EsperBlockSpectate : MonoBehaviour
    {
        public static EsperBlockSpectate Instance;
        public static MelonPreferences_Entry<bool> Enabled = MelonPreferences.CreateEntry<bool>("GGDH", "Enable_" + nameof(EsperBlockSpectate), true);

        public EsperBlockSpectate(IntPtr ptr) : base(ptr)
        {
            IngameSettings.AddIngameSettingsEntry(
                               new IngameSettings.IngameSettingsEntry()
                               {
                                   entry = Enabled,
                                   name_cn = "超能力鸭取消附身阶段",
                                   name_eng = "Esper disable spectate"
                               }
                                          );
        }

        public EsperBlockSpectate() : base(ClassInjector.DerivedConstructorPointer<EsperBlockSpectate>()) => ClassInjector.DerivedConstructorBody(this);
        public static void Init()
        {
            GameObject ML_Manager = GameObject.Find("ML_Manager");
            if (ML_Manager == null)
            {
                ML_Manager = new GameObject("ML_Manager");
                DontDestroyOnLoad(ML_Manager);
            }

            if (ML_Manager.GetComponent<EsperBlockSpectate>() == null)
            {
                Instance = ML_Manager.AddComponent<EsperBlockSpectate>();
            }
        }

        [HarmonyPatch(typeof(PhotonEventAPI), nameof(PhotonEventAPI.OnEvent), typeof(ExitGames.Client.Photon.EventData))]
        class PhotonEventAPI_OnEvent
        {
            static bool Prefix(ExitGames.Client.Photon.EventData __0)
            {
                if (!Enabled.Value) return true;

                try
                {
                    bool shouldBlockEvent = false;
                    int code = __0.Code;

                    //屏蔽事件
                    switch (code)
                    {
                        case (int)EventDataCode.ESPER_SPECTATE:
                            shouldBlockEvent = true;
                            break;
                    }

                    //开始屏蔽事件
                    if (shouldBlockEvent)
                    {
                        MelonLogger.Warning("已屏蔽超能力附身事件");
                        return false;
                    }

                    return true;
                }
                catch (System.Exception e)
                {
                    MelonLogger.Error(e.ToString());
                    return true;
                }
            }
        }
    }
}