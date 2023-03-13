using Handlers.CommonHandlers.UIHandlers;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace GGD_Hack.Features.Test
{
    public class ShowImage
    {
        public static string chocolateImg = "chocolate.jpg";
        public static Sprite chocolateSprite = null;
        public static void ShowImagePopup()
        {
            if(chocolateSprite == null)
            {
                chocolateSprite = Utils.SpriteUtil.GetSpriteFromImageName(chocolateImg);
                /*
                Assembly assembly = Assembly.GetExecutingAssembly();

                using (Stream stream = assembly.GetManifestResourceStream(chocolateImg))
                {
                    if (stream == null)
                    {
                        Debug.LogError("找不到嵌入式资源: " + chocolateImg);
                        return;
                    }

                    byte[] imageData = new byte[stream.Length];
                    stream.Read(imageData, 0, imageData.Length);

                    Texture2D texture = new Texture2D(2, 2);
                    ImageConversion.LoadImage(texture,imageData);

                    Rect rect = new Rect(0, 0, texture.width, texture.height);
                    Sprite sprite = Sprite.Create(texture, rect, Vector2.zero);

                    chocolateSprite = sprite;
                    GameObject spriteObject = new GameObject("ChocolateSprite");
                    spriteObject.transform.SetParent(spriteObject.transform);
                    spriteObject.transform.localPosition = Vector3.zero;

                    SpriteRenderer renderer = spriteObject.AddComponent<SpriteRenderer>();
                    renderer.sprite = sprite;
                }*/
            }            

            Handlers.CommonHandlers.UIHandlers.GlobalPanelsHandler.Instance.OpenErrorPanelWithImage(
                "Title",
                "Content",
                chocolateSprite,
                "ButtonText",
                new System.Action(() => {
                    Handlers.CommonHandlers.UIHandlers.GlobalPanelsHandler.Instance.ClosePanels();
                }),
                "5"                
                );
        }
    }
}
