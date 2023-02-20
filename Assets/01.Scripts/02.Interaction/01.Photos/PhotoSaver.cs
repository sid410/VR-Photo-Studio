using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

/// <summary>
/// Saves png photos from the RenderTexture we see in the camDisplay
/// </summary>
public class PhotoSaver : MonoBehaviour
{
    [SerializeField] private RenderTexture camDisplay;
    private string saveLocation;

    private void Start()
    {
        // save the photos in this gallery folder
        saveLocation = Path.Combine(Application.persistentDataPath, "Gallery/");
        MakeFolder(saveLocation);
    }

    /// <summary>
    /// Call this to save a png file from the camDisplay we pre-visualize.
    /// First convert the RenderTexture to Texture2D, then to png.
    /// </summary>
    public void SavePhoto()
    {
        Texture2D camTexture = camDisplay.ConvertToTexture2D();
        byte[] bytes = camTexture.EncodeToPNG();
        File.WriteAllBytes(saveLocation + MakeFileNameFromTime(DateTime.UtcNow), bytes);
    }

    /// <summary>
    /// generate a string value based from the DateTime
    /// </summary>
    /// <param name="dateTime">pass here the DateTime value, for example UtcNow</param>
    /// <returns></returns>
    private string MakeFileNameFromTime(DateTime dateTime)
    {
        string fileName = dateTime.ToString("yyyy-MM-dd\\THH-mm-ss\\Z");
        fileName += ".png";
        return fileName;
    }

    /// <summary>
    /// make a folder if it does not exist
    /// </summary>
    /// <param name="folderPath">create folder in this path</param>
    private void MakeFolder(string folderPath)
    {
        try
        {
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
        }
        catch (IOException ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}