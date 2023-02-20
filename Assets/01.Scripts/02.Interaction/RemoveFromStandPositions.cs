using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script attached to the teleport anchor prefab.
/// When selected by the left hand ray, it will remove the position data from the scriptable object
/// then destroy the corresponding teleport anchor.
/// </summary>
public class RemoveFromStandPositions : MonoBehaviour
{
    // the ScriptableObject to pass all the data to regarding number of photos and where to stand
    [SerializeField] private CameraSettings cameraSettingsValues;

    /// <summary>
    /// remove this anchor from the scriptable object CameraSettings standPositions list
    /// then destroy the object this script is attached to.
    /// </summary>
    public void RemoveTeleportAnchor()
    {
        cameraSettingsValues.standPositions.Remove(gameObject.transform.position);
        Destroy(gameObject);
    }
}
