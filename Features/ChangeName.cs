#if false
using HarmonyLib;
using Managers.ConnectionManagers;
using MelonLoader;
using Photon.Pun;
using System.Text;
using UnityEngine;

namespace GGD_Hack.Features
{
    public class ChangeName
    {
        public static string overrideName = null;

        public static void OverrideName(string name)
        {
            overrideName = name;
        }

        public static void StopOverride()
        {
           overrideName = null;
        }

        public static void CreateChangeColorButton()
        {
            GameObject inputNickname = Handlers.MenuSceneHandlers.MenuSceneHandler.Instance.playerNameIF.gameObject;

            GameObject changeColorButton = GameObject.Find("NicknameColorButton");

            if(changeColorButton == null)
            {
                changeColorButton = GameObject.Instantiate(GameObject.Find("CycleButton"));
                changeColorButton.name = "NicknameColorButton";
            }

            changeColorButton.transform.localPosition = new Vector3(-1000, -60, 0);
            changeColorButton.transform.localScale = Vector3.one;

            TMPro.TextMeshProUGUI textMeshProUGUI = changeColorButton.transform.Find("Label").gameObject.GetComponent<TMPro.TextMeshProUGUI>();
            textMeshProUGUI.text = "Color";
            textMeshProUGUI.ForceMeshUpdate();

            changeColorButton.transform.SetParent(inputNickname.transform);
        }

        [HarmonyPatch(typeof(RoomManager), nameof(RoomManager.GEEMGMMAHKK))]
        public class OverrideNamePatch
        {
            static void Prefix(Managers.ConnectionManagers.RoomManager __instance, Il2CppSystem.Collections.IEnumerator __result, string __0, ref string __1, string __2, string __3, bool __4)
            {
                try
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine("------------------------");
                    sb.AppendLine("原始名字:" + __1);
                    sb.AppendLine("替换名字:" + overrideName);

                    if (overrideName != null)
                    {
                        __1 = overrideName;
                    }

                    MelonLogger.Msg(System.ConsoleColor.Green, sb.ToString());
                }
                catch (System.Exception ex)
                {
                    MelonLogger.Msg($"Exception in patch of Il2CppSystem.Collections.IEnumerator Managers.ConnectionManagers.RoomManager::GEEMGMMAHKK(string LOPKLMEJFGC, string GBNIEEGIBFD, string OMHOMNEPPKM, string CLDANIALKPF, bool IEHHFCLHFAP):\n{ex}");
                }
            }

        }
    }
}
#endif