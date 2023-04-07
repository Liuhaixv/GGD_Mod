using MelonLoader;
using UnhollowerRuntimeLib;
using UnityEngine;
using HarmonyLib;

using IntPtr = System.IntPtr;
using Handlers.GameHandlers.PlayerHandlers;
using TMPro;

namespace GGD_Hack.Features.Admin
{
    [RegisterTypeInIl2Cpp]
    public class ModDeveloperName : MonoBehaviour
    {
        public static ModDeveloperName Instance;
        public const string developerName = "Mod Developer Name";

        public ModDeveloperName(IntPtr ptr) : base(ptr)
        {
        }

        public ModDeveloperName() : base(ClassInjector.DerivedConstructorPointer<ModDeveloperName>()) => ClassInjector.DerivedConstructorBody(this);
        public static void Init()
        {
            GameObject ML_Manager = GameObject.Find("ML_Manager");
            if (ML_Manager == null)
            {
                ML_Manager = new GameObject("ML_Manager");
                DontDestroyOnLoad(ML_Manager);
            }

            if (ML_Manager.GetComponent<ModDeveloperName>() == null)
            {
                Instance = ML_Manager.AddComponent<ModDeveloperName>();
            }
        }

        public static void MarkAsDev(string userId)
        {
            if (!PlayerController.playersList.ContainsKey(userId)) return;

            PlayerController playerController = PlayerController.playersList[userId];

            playerController.playerNameText.gameObject.transform.Find(developerName)?.gameObject.SetActive(true);
        }

        [HarmonyPatch(typeof(PlayerController),nameof(PlayerController.Start))]
        class PlayerController_Start
        {
            static void Postfix(PlayerController __instance)
            {
                TMPro.TextMeshProUGUI playerNameText = __instance.playerNameText;

                GameObject modDeveloperName = new GameObject();
                modDeveloperName.name = developerName;
                modDeveloperName.transform.SetParent(playerNameText.gameObject.transform, false);
                modDeveloperName.transform.localPosition = new Vector3(0,40,0);


                TextMeshProUGUI textMeshProUGUI = modDeveloperName.AddComponent<TextMeshProUGUI>();
                textMeshProUGUI.rectTransform.sizeDelta = new Vector2(500, 50);
                textMeshProUGUI.alignment = TextAlignmentOptions.Center;
                textMeshProUGUI.horizontalAlignment = HorizontalAlignmentOptions.Center;
                textMeshProUGUI.font = playerNameText.font;
                textMeshProUGUI.text =string.Format("<sprite name=\"1f60e\"> <color=#FFD700>{0}</color> <sprite name=\"1f60e\">", Utils.Utils.IsChineseSystem() ? "Mod开发者" : "Mod Dev");

                modDeveloperName.active = false;

#if Developer
                if (__instance.isLocal)
                {
                    modDeveloperName.active = true;
                }
#endif
            }
        }
    }
}