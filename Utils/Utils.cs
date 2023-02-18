using System;
using MelonLoader;

namespace GGD_Hack.Utils
{
    public class Utils
    {
        //发送聊天消息
        public static void SendTextMessage(string text)
        {
            try
            {
                //IntPtr intPtr = UnhollowerBaseLib.Il2CppStringArray.AllocateArray(2);
                //UnhollowerBaseLib.Il2CppStringArray strs = new UnhollowerBaseLib.Il2CppStringArray(intPtr);
                UnhollowerBaseLib.Il2CppStringArray strs = new UnhollowerBaseLib.Il2CppStringArray(2);

                strs[0] = text;
                strs[1] = "false";

                Il2CppSystem.Object obj = new Il2CppSystem.Object(strs.Pointer);

                APIs.Photon.PhotonEventAPI.SendEventToPlugin(66, obj, false);
            }
            catch (System.Exception ex)
            {
                MelonLogger.Error("ERROR!!: SendTextMessage");
            }
        }
    }
}
