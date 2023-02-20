using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ScriptableObject for storing data of the camera settings.
/// </summary>
[CreateAssetMenu(fileName = "CameraSettings", menuName = "ScriptableObjects/CameraSettings")]
public class CameraSettings : ScriptableObject
{
    public float camZoom;
    public int maxPhotos;

    [SerializeField] public List<Vector3> standPositions;
}
