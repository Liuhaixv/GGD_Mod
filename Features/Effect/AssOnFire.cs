#if Developer
using Handlers.GameHandlers.PlayerHandlers;
using MelonLoader;
using UnityEngine;
using UnityEngine.SceneManagement;

//炙热焦臀
namespace GGD_Hack.Features.Effect
{
    public class AssOnFire
    {
        public static void AttachFireToLocalPlayer()
        {
            GameObject gameObject = GetFire();
            if(gameObject == null)
            {
                MelonLogger.Error("Fire为空");
            }
            gameObject.transform.SetParent(LocalPlayer.Instance.gameObject.transform, false);
            gameObject.transform.localPosition= Vector3.zero;
        }

        public static void AttachFireToPlayer(string userId)
        {
            if (!PlayerController.playersList.ContainsKey(userId))
            {
                return;
            }

            PlayerController player = PlayerController.playersList[userId];

            GetFire().transform.SetParent(player.gameObject.transform, false);
        }

        private static GameObject GetFire()
        {
            SceneManager.LoadScene("MotherGoose", LoadSceneMode.Additive);
        }
    }
}
#endif