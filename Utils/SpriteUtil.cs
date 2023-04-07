using System.IO;
using System.Reflection;
using UnityEngine;
using System.Collections.Generic;
using MelonLoader;
using TMPro;

namespace GGD_Hack.Utils
{
    public class SpriteUtil
    {
        public static Dictionary<string, Sprite> spriteCache = new Dictionary<string, Sprite>();

        public static Sprite GetSpriteFromImageName(string imageName)
        {

            if (spriteCache.ContainsKey(imageName))
            {
                if (spriteCache[imageName] != null)
                    return spriteCache[imageName];
            }

            Assembly assembly = Assembly.GetExecutingAssembly();
            string resourceName = nameof(GGD_Hack) + ".Resources." + imageName;

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            {
                if (stream == null)
                {
                    MelonLogger.Error("找不到嵌入式资源: " + resourceName);
                    return null;
                }

                byte[] imageData = new byte[stream.Length];
                stream.Read(imageData, 0, imageData.Length);

                Texture2D texture = new Texture2D(2, 2);
                ImageConversion.LoadImage(texture, imageData);

                Rect rect = new Rect(0, 0, texture.width, texture.height);

                Sprite sprite = Sprite.Create(texture, rect, Vector2.zero);
                sprite.name = imageName;
                spriteCache[imageName] = sprite;

                Object.DontDestroyOnLoad(sprite);
                return sprite;
            }
        }
    }
}
