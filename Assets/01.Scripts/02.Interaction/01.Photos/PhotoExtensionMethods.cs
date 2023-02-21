using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Extension methods to handle the render texture to avoid becoming black
/// </summary>
public static class PhotoExtensionMethods
{
    /// <summary>
    /// Extension method for converting a RenderTexture to a Texture2D
    /// </summary>
    /// <param name="renderTexture">The input render texture, usually from the Unity cam</param>
    /// <returns></returns>
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

    /// <summary>
    /// Loads an Image file (png) as a Texture2D
    /// </summary>
    /// <param name="filePath">the directory the png file is located</param>
    /// <returns></returns>
    public static Texture2D LoadPNG(this string filePath)
    {
        Texture2D tex = null;
        byte[] fileData;

        if (System.IO.File.Exists(filePath))
        {
            fileData = System.IO.File.ReadAllBytes(filePath);
            tex = new Texture2D(2, 2);
            tex.LoadImage(fileData); //..this will auto-resize the texture dimensions.
        }
        return tex;
    }
}
