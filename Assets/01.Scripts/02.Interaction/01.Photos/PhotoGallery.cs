using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class PhotoGallery : MonoBehaviour
{
    [SerializeField] private GameObject camDisplay;
    private Material displayMaterial;

    private Texture2D texture;

    private void Start()
    {
        displayMaterial = camDisplay.GetComponent<Material>();

        string galleryFolder = Path.Combine(Application.persistentDataPath, "Gallery/");
        string pngFilePath = galleryFolder + "2023-02-20T22-32-44Z.png";
        Debug.Log(pngFilePath);

        texture = pngFilePath.LoadPNG();
        displayMaterial = camDisplay.GetComponent<MeshRenderer>().material;
        displayMaterial.mainTexture = texture;

    }
}
