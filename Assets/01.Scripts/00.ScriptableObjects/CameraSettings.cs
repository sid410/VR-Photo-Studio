using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CameraSettings", menuName = "ScriptableObjects/CameraSettings")]
public class CameraSettings : ScriptableObject
{
    public float camZoom;
    public int maxPhotos;
    public Vector3[] standPositions;
}
