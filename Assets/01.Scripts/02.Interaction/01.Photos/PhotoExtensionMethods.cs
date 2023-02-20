using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Extension methods to handle the render texture to avoid becoming black
/// </summary>
public static class PhotoExtensionMethods
{
    public static Texture2D ConvertToTexture2D(this RenderTexture renderTexture)
    {
        Texture2D tex = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.RGB24, false);
        var oldRenderTexture = RenderTexture.active;
        RenderTexture.active = renderTexture;

        tex.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
        tex.Apply();

        RenderTexture.active = oldRenderTexture;
        return tex;
    }
}
