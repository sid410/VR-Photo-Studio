using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

/// <summary>
/// Load and control the view of the images during gallery mode
/// </summary>
public class PhotoGallery : MonoBehaviour
{
    // the cam display object for gallery mode
    [SerializeField] private GameObject camDisplayGallery;

    // to store here the textures loaded from memory
    private List<Texture2D> savedPhotosTexture = new List<Texture2D>();

    private Renderer camDisplayRenderer;

    private string galleryFolder;
    private int numPhotos, currPhoto;

    private void Start()
    {
        // get the reference for renderer in the gallery camera
        camDisplayRenderer = camDisplayGallery.GetComponent<Renderer>();

        // define the gallery folder in the persistent data path
        galleryFolder = Path.Combine(Application.persistentDataPath, "Gallery/");

        LoadPhotosFromMemory(galleryFolder);
    }

    /// <summary>
    /// gets all the png files inside a folder, convert them to textures, then add to the texture list
    /// </summary>
    /// <param name="folderName">the folder that contains the png files to read from</param>
    private void LoadPhotosFromMemory(string folderName)
    {
        // first clear the list
        savedPhotosTexture.Clear();
        
        // then get the filenames of all the png images
        string[] savedPhotosList = Directory.GetFiles(folderName, "*.png");

        // finally, convert them to textures then add to the texture list
        foreach (string imageFile in savedPhotosList)
        {
            Texture2D tex = imageFile.LoadPNG();
            savedPhotosTexture.Add(tex);
        }

        // the number of photos inside the folder
        numPhotos = savedPhotosTexture.Count;

        // initialize with the topmost photo, if not empty
        currPhoto = 0;
        if (savedPhotosTexture.Count != 0)
        {
            camDisplayRenderer.material.mainTexture = savedPhotosTexture[currPhoto];
        }
    }

    /// <summary>
    /// When in gallery mode, call this function to view the next or previous photo.
    /// </summary>
    /// <param name="next">set to true to view next, false to view previous</param>
    public void ViewNextOrPreviousPhoto(bool next)
    {
        if (next)
        {
            currPhoto++;
            if (currPhoto == numPhotos)
            {
                currPhoto = 0;
            }
        }
        else
        {
            currPhoto--;
            if (currPhoto < 0)
            {
                currPhoto = numPhotos - 1;
            }
        }

        camDisplayRenderer.material.mainTexture = savedPhotosTexture[currPhoto];
    }
}
