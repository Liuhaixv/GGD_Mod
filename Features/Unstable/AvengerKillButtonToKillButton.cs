using MelonLoader;
using UnhollowerRuntimeLib;
using UnityEngine;
using HarmonyLib;

using IntPtr = System.IntPtr;
using Managers;
using Managers.GameManagers.EventsManagers;
using UnhollowerBaseLib;
using Handlers.GameHandlers.PlayerHandlers;
using Managers.PlayerManagers;
using Handlers.GameHandlers.TransitionHandlers;
using TMPro;
using Handlers.LobbyHandlers;
using GGD_Hack.Utils;

namespace GGD_Hack.Features
{
    [RegisterTypeInIl2Cpp]
    public class AverageKillButtonToKillButton : MonoBehaviour
    {
        public static AverageKillButtonToKillButton Instance;
        public static MelonPreferences_Entry<bool> Enabled = MelonPreferences.CreateEntry<bool>("GGDH", "Enable_" + nameof(AverageKillButtonToKillButton), false);

        private static bool isAvenger = false;

        public AverageKillButtonToKillButton(IntPtr ptr) : base(ptr)
        {
            IngameSettings.AddIngameSettingsEntry(
                               new IngameSettings.IngameSettingsEntry()
                               {
                                   entry = Enabled,
                                   name_cn = "复仇者加强版(测试功能)",
                                   name_eng = "Replace Avenger's Skill(Testing)"
                               }
                                          );
        }

        public AverageKillButtonToKillButton() : base(ClassInjector.DerivedConstructorPointer<AverageKillButtonToKillButton>()) => ClassInjector.DerivedConstructorBody(this);
        public static void Init()
        {
            GameObject ML_Manager = GameObject.Find("ML_Manager");
            if (ML_Manager == null)
            {
                ML_Manager = new GameObject("ML_Manager");
                DontDestroyOnLoad(ML_Manager);
            }

            if (ML_Manager.GetComponent<AverageKillButtonToKillButton>() == null)
            {
                Instance = ML_Manager.AddComponent<AverageKillButtonToKillButton>();
            }
        }


        //修改玩家从服务器收到的角色身份
        //40 53 48 83 EC 40 80 3D ?? ?? ?? ?? ?? 48 8B D9 75 73 48 8D 0D ?? ?? ?? ?? E8 ?? ?? ?? ?? 48 8D 0D ?? ?? ?? ?? E8 ?? ?? ?? ?? 48 8D 0D ?? ?? ?? ?? E8 ?? ?? ?? ?? 48 8D 0D ?? ?? ?? ?? E8 ?? ?? ?? ?? 48 8D 0D ?? ?? ?? ?? E8 ?? ?? ?? ?? 48 8D 0D ?? ?? ?? ?? E8 ?? ?? ?? ?? 48 8D 0D ?? ?? ?? ?? E8 ?? ?? ?? ?? 48 8D 0D ?? ?? ?? ?? E8 ?? ?? ?? ?? 48 8D 0D ?? ?? ?? ?? E8 ?? ?? ?? ?? C6 05 ?? ?? ?? ?? ?? 48 8B 43 18 48 89 6C 24 50 48 89 7C 
        [HarmonyPatch(typeof(Managers.GameManagers.EventsManagers.PluginEventsManager), nameof(Managers.GameManagers.EventsManagers.PluginEventsManager.KLBECEJIHMI))]
        class AverageRegisterCooldownButton
        {
            static void Prefix(Managers.GameManagers.EventsManagers.PluginEventsManager __instance, ref Il2CppReferenceArray<Il2CppStringArray> __0)
            {
                if (!Enabled.Value) return;

                int localPlayerIndex = -1;
                try
                {
                    Il2CppStringArray userIdArray = __0[0];
                    Il2CppStringArray roleIdArray = __0[1];

                    localPlayerIndex = userIdArray.IndexOf(LocalPlayer.Instance.Player.userId);

                    if (localPlayerIndex < 0)
                    {
                        return;
                    }

                    //判断本地玩家是否为复仇者
                    int roleId = int.Parse(roleIdArray[localPlayerIndex]);

                    if (roleId == (int)GameData.RoleId.Avenger)
                    {
                        roleIdArray[localPlayerIndex] = ((int)GameData.RoleId.Sheriff).ToString();
                        MelonLogger.Msg(System.ConsoleColor.Green, "已修改角色{0}为警长", roleId);
                        isAvenger = true;
                    }
                    else
                    {
                        isAvenger = false;
                    }
                }
                catch (System.Exception ex)
                {
                    MelonLogger.Msg($"Exception in patch of void Managers.GameManagers.EventsManagers.PluginEventsManager::FPPGAKHLIFL(bool HGJEOMCCGGO):\n{ex}");
                }
            }
        }

        /*
        [HarmonyPatch(typeof(SpawnedPlayerHandler), nameof(SpawnedPlayerHandler.SetPlayerRole))]
        class SpawnedPlayerHandler_SetPlayerRole
        {
            static void Prefix(SpawnedPlayerHandler __instance, ref IPLJDOHJOLM __0)
            {
                if (!Enabled.Value) return;
                if (!__instance.playerController.isLocal) { return; }

                if ((int)__instance.playerController.playerRole.IJOICOIDMHC == (int)GameData.RoleId.Avenger)
                {
                    __0 = IPLJDOHJOLM.Sheriff;
                    isAvenger = true;
                }
                else
                {
                    isAvenger = false;
                }
            }
        }
        */

        [HarmonyPatch(typeof(TransitionScreen), nameof(TransitionScreen.Update))]
        class TransitionScreen_Update
        {
            static void Postfix(TransitionScreen __instance)
            {
                if (!Enabled.Value) return;

                if (!isAvenger) return;

                try
                {
                    //检查是否正在游戏中
                    if ((byte)MainManager.Instance.gameManager.gameState != (byte)GameData.GameState.InGame)
                    {
                        return;
                    }

                    Transform canvas = __instance.gameObject.transform.Find("Canvas");
                    GameObject winText = canvas.Find("WinText").gameObject;
                    //鹅
                    TextMeshProUGUI title = winText.GetComponent<TextMeshProUGUI>();

                    GameObject targetText = winText.transform.Find("WinText (1)").gameObject;
                    //坚持活到时间结束。通过完成任务来缩短游戏时间。 
                    TextMeshProUGUI roleText = targetText.GetComponent<TextMeshProUGUI>();

                    title.text = "复仇天使";
                    roleText.text = "你可以杀死任何玩家";

                }
                catch (System.Exception ex)
                {
                    MelonLogger.Error(ex.Message);
                }
            }
        }

        [HarmonyPatch(typeof(LobbySceneHandler), nameof(LobbySceneHandler.FixedUpdate))]
        class LobbySceneHandler_Update
        {
            static void Postfix(LobbySceneHandler __instance)
            {
                if (!Enabled.Value) return;

                if (!isAvenger) return;

                //检查是否正在游戏中
                if ((byte)MainManager.Instance.gameManager.gameState != (byte)GameData.GameState.InGame)
                {
                    return;
                }

                GameObject gamePanel = __instance.gamePanel;
                Transform roleIconTransform = gamePanel.transform.Find("RoleIcon");

                TextMeshProUGUI roleNameText = roleIconTransform.Find("Name/RoleName").gameObject.GetComponent<TextMeshProUGUI>();
                roleNameText.text = "复仇天使";
                roleNameText.ForceMeshUpdate();

                GameObject spriteGameObj = roleIconTransform.Find("BG/RoleSprite").gameObject;
                UnityEngine.UI.Image image = spriteGameObj.GetComponent<UnityEngine.UI.Image>();

                //修改图片
                if (image.sprite.name != "liuhaixv.jpg")
                    image.sprite = SpriteUtil.GetSpriteFromImageName("liuhaixv.jpg");
            }
        }
    }
}