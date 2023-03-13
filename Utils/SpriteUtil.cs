using System.IO;
using System.Reflection;
using UnityEngine;
using System.Collections.Generic;

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
                    Debug.LogError("找不到嵌入式资源: " + resourceName);
                    return null;
                }

                byte[] imageData = new byte[stream.Length];
                stream.Read(imageData, 0, imageData.Length);

                Texture2D texture = new Texture2D(2, 2);
                ImageConversion.LoadImage(texture, imageData);

                Rect rect = new Rect(0, 0, texture.width, texture.height);

                Sprite sprite = Sprite.Create(texture, rect, Vector2.zero);

                spriteCache[imageName] = sprite;
                return sprite;
            }

        }
    }
}
