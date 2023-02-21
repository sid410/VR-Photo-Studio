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
    private Texture initialTexture;

    // to store here the textures loaded from memory
    private List<Texture2D> savedPhotosTexture = new List<Texture2D>();

    private Renderer camDisplayRenderer;

    private string galleryFolder;
    private string[] savedPhotosList;
    private int numPhotos, currPhoto;

    private void Start()
    {
        // get the reference for renderer in the gallery camera and save the initial texture
        camDisplayRenderer = camDisplayGallery.GetComponent<Renderer>();
        initialTexture = camDisplayRenderer.material.mainTexture;

        // define the gallery folder in the persistent data path
        galleryFolder = Path.Combine(Application.persistentDataPath, "Gallery/");

        LoadPhotosFromMemory();
    }

    /// <summary>
    /// gets all the png files inside a folder, convert them to textures, then add to the texture list
    /// </summary>
    public void LoadPhotosFromMemory()
    {
        // first clear the list
        savedPhotosTexture.Clear();
        
        // then get the filenames of all the png images
        savedPhotosList = Directory.GetFiles(galleryFolder, "*.png");

        // finally, convert them to textures then add to the texture list
        foreach (string imageFile in savedPhotosList)
        {
            Texture2D tex = imageFile.LoadPNG();
            savedPhotosTexture.Add(tex);
        }

        // the number of photos inside the folder
        numPhotos = savedPhotosTexture.Count;

        // initialize with the topmost photo
        currPhoto = 0;
        UpdateCameraDisplayRenderer(currPhoto);
    }

    /// <summary>
    /// Delete the photo from memory
    /// </summary>
    public void RemovePhotoFromMemory()
    {
        // do not do anything if the array is empty
        if (savedPhotosList.Length == 0)
        {
            return;
        }
        // check first if the file exists in memory before deleting
        if (File.Exists(savedPhotosList[currPhoto]))
        {
            File.Delete(savedPhotosList[currPhoto]);
            LoadPhotosFromMemory();
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

        UpdateCameraDisplayRenderer(currPhoto);
    }

    /// <summary>
    /// Update the texture of the 3D cam model, if the photos from memory are not empty
    /// </summary>
    /// <param name="photoCounter">the index of the current photo being interacted</param>
    private void UpdateCameraDisplayRenderer(int photoCounter)
    {
        if (savedPhotosTexture.Count != 0)
        {
            camDisplayRenderer.material.mainTexture = savedPhotosTexture[photoCounter];
        }
        else
        {
            camDisplayRenderer.material.mainTexture = initialTexture;
        }
    }
}
