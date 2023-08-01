using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ExtensionMethods;
using System.Threading.Tasks;

public class RenderTextureToImage : MonoBehaviour
{
    public RenderTexture renderTexture;
    private RawImage image;
    private void Awake()
    {
        image = GetComponent<RawImage>();
    }

    void Update()
    {
        image.texture = ToTexture2D(renderTexture);
    }

    private Texture2D ToTexture2D(RenderTexture rTex)
    {
        Texture2D tex = new Texture2D(rTex.width, rTex.height, TextureFormat.RGBA32, false);
        // ReadPixels looks at the active RenderTexture.
        RenderTexture.active = rTex;
        tex.ReadPixels(new Rect(0, 0, rTex.width, rTex.height), 0, 0);
        Color[] currentPixels = tex.GetPixels();
        int width = tex.width;
        int height = tex.height;
        Color empty = new Color(1, 1, 1, 0);

#if !UNITY_WEBGL
        // Use Parallel.For for parallel execution
        Parallel.For(0, width, x =>
#else
        for (int x = 0; x < width; x++)
#endif
        {
            for (int y = 0; y < height; y++)
            {
                if (currentPixels[x + y * width] != Color.white)
                    currentPixels[x + y * width] = empty;
            }
        }

#if !UNITY_WEBGL
        )
#endif
        ;

        tex.SetPixels(currentPixels);
        tex.Apply();
        return tex;
    }
}
